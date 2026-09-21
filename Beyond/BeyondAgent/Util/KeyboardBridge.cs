using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace BeyondAgent.Util
{
    // Windows delivers Raw Input (WM_INPUT) only to the foreground window. The
    // launcher hosts the game as a child of its own window, so the game window
    // is never the foreground one, and Unity feeds the new Input System from
    // Raw Input ("<RI> Initializing input." in Player.log). Result: inside the
    // launcher Keyboard.current never sees a key, and every game bind that
    // polls it is dead - Enter-to-chat, WASD, skills 1-6.
    //
    // WM_KEYDOWN still reaches the focused child window, so legacy
    // UnityEngine.Input works. That is what the game itself used until the
    // 2026-09-19 update moved its binds to InputManager/Keyboard.current, which
    // is when this broke.
    //
    // So bridge one into the other: read legacy Input each frame and queue it
    // as a keyboard state event. One event feeds every consumer at once -
    // Keyboard.current polling, InputActions, the UI module - rather than
    // patching each call site in the game.
    //
    // Needs BeyondLifecycle's runInBackground + IgnoreFocus settings to stick:
    // without them InputManager.ShouldFlushEventBuffer drops the whole event
    // buffer each update while the game reads as unfocused, ours included.
    //
    // Hopefully, this shouldn't change again from here on out, but we can't
    // confirm that. Thanks, AE. Very cool. - retrograde.
    internal static class KeyboardBridge
    {
        // Key and KeyCode agree on almost every name. These are the families
        // that don't, plus Numpad->Keypad and Digit->Alpha handled in BuildMap.
        private static readonly Dictionary<string, string> Renames = new()
        {
            ["Enter"] = "Return",
            ["LeftCtrl"] = "LeftControl",
            ["RightCtrl"] = "RightControl",
            ["LeftMeta"] = "LeftWindows",
            ["RightMeta"] = "RightWindows",
            ["ContextMenu"] = "Menu",
            ["PrintScreen"] = "Print",
        };

        private static readonly Dictionary<Key, KeyCode> Keys = BuildMap();

        // Previous frame's pressed set as a bitfield, so we can tell "changed"
        // without reading KeyboardState's fixed buffer (that needs unsafe).
        private static ulong _lastLo;
        private static ulong _lastHi;

        public static void Tick()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            // Idle fast path: nothing held now and nothing held last frame.
            if (!Input.anyKey && _lastLo == 0 && _lastHi == 0)
            {
                return;
            }

            KeyboardState state = default;
            ulong lo = 0;
            ulong hi = 0;
            foreach (KeyValuePair<Key, KeyCode> pair in Keys)
            {
                if (!Input.GetKey(pair.Value))
                {
                    continue;
                }

                state.Press(pair.Key);
                int bit = (int)pair.Key;
                if (bit < 64)
                {
                    lo |= 1UL << bit;
                }
                else
                {
                    hi |= 1UL << (bit - 64);
                }
            }

            // Queue only on change. wasPressedThisFrame is the edge between two
            // consecutive states, and a per-frame stream would also stomp real
            // Raw Input when the game runs outside the launcher.
            if (lo == _lastLo && hi == _lastHi)
            {
                return;
            }

            _lastLo = lo;
            _lastHi = hi;
            InputSystem.QueueStateEvent(keyboard, state);
        }

        private static Dictionary<Key, KeyCode> BuildMap()
        {
            Dictionary<Key, KeyCode> map = [];
            foreach (Key key in (Key[])Enum.GetValues(typeof(Key)))
            {
                if (key == Key.None)
                {
                    continue;
                }

                string name = key.ToString();
                if (name.StartsWith("Numpad", StringComparison.Ordinal))
                {
                    name = "Keypad" + name.Substring(6);
                }
                else if (name.StartsWith("Digit", StringComparison.Ordinal))
                {
                    name = "Alpha" + name.Substring(5);
                }
                else if (Renames.TryGetValue(name, out string renamed))
                {
                    name = renamed;
                }

                // Keys with no legacy equivalent (OEM1-5, F16+) just drop out.
                if (Enum.TryParse(name, true, out KeyCode code) && code != KeyCode.None)
                {
                    map[key] = code;
                }
            }

            // Self-check: one key per naming family (plain, rename,
            // Digit->Alpha, Numpad->Keypad). A miss means the enums drifted.
            foreach (Key probe in new[] { Key.W, Key.Enter, Key.Digit1, Key.NumpadEnter })
            {
                if (!map.ContainsKey(probe))
                {
                    BeyondLog.Error($"[KeyboardBridge] no KeyCode for Key.{probe} — binds using it stay dead");
                }
            }

            return map;
        }
    }
}
