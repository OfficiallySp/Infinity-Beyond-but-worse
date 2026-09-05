<div align="center">

# Infinity-Beyond-but-worse 💀🔥

**the fork where we took the issue as a feature request**

![undetected](https://img.shields.io/badge/UNDETECTED-probably-brightgreen)
![tests](https://img.shields.io/badge/tests-0-red)
![works](https://img.shields.io/badge/works-no-critical)
![tokens](https://img.shields.io/badge/claude%20tokens-yes-blueviolet)
![version](https://img.shields.io/badge/version-0.0.1%20FINAL%20FINAL%20real%20(2)-informational)

</div>

---

> Bro I'm sorry but how many Claude tokens did it take to create this?
>
> I'm looking through the code and this genuinely feels like someone told an AI
> "make the worst possible AQW cheat known to man" and just accepted every
> suggestion. **You can't even make a custom Mono injector, so you have to resort
> to patching the DLL instead.**
>
> Like bro, Claude really said "we're not doing this the normal way" and y'all
> just let it cook 💀

thanks for the spec 🫡

---

## 📦 what is this

someone opened an issue describing the worst possible AQW cheat. we read it. we
agreed with every word. then we built it **on purpose** and put it in
[`BeyondBeyond/`](BeyondBeyond/).

it does not work. that took effort. 😤

```
  ____  _______   ______  _   _ ____
 | __ )| ____\ \ / / __ \| \ | |  _ \
 |  _ \|  _|  \ V / |  | |  \| | | | |
 | |_) | |___  | || |__| | |\  | |_| |
 |____/|_____| |_| \____/|_| \_|____/
```

```bash
cd BeyondBeyond
./run.sh                # the intended experience. slow, so you can read it 🐌
./run.sh --normal       # 1.0x — the good bits go past too fast
./run.sh --speed 3      # n = multiplier. 3 is a hostage situation.
./run.sh --fast         # no delays at all, for cowards
```

## 🎬 the show

it runs as a five act tragedy and then exits with code 0, because reaching the
end of `Main` is technically the goal of a program ✅

| act | what it does | outcome |
|---|---|---|
| 1. licensing 🔑 | validates your key with a rigorous checksum + expiry algorithm | `return true;` (the algorithm's result is computed into a variable and then never used) |
| 2. config ⚙️ | loads 4 config files in 4 formats that all disagree | resolves conflicts **alphabetically by filename**, documented as "deterministic ✅" |
| 3. injection 💉 | **a custom Mono injector**, as requested | 7 strategies, each worse than the last |
| 4. features ⚔️ | activates every premium cheat via reflection | every single one throws |
| 5. meltdown 🫠 | final integrity check | the integrity check becomes self-aware |

## 💉 about that Mono injector

you said we couldn't write one. fair. so `BeyondBeyond/Injection/` has one now.

it starts **completely correct** — real `mono_get_root_domain`, real
`mono_thread_attach`, real `mono_image_open_from_data`, correct
`MonoBleedingEdge/EmbedRuntime` paths. read the first 30 lines and you'll think
*wait, this is actually right*.

then it degrades:

1. the correct approach, which fails and blames your **monitor size** 🖥️
2. resolving `mono_get_root_domain` by scanning memory for the ASCII bytes `mono`
   and calling whatever it finds
3. truncating a 64-bit pointer to 32 bits (the top half "wasn't being used") 🤷
4. printing instructions asking **you** to inject the DLL manually, one step of
   which is "ask a friend"
5. ...concluding that the most reliable injection method is to **patch the DLL on
   disk** 💀

so it loops all the way back around to the exact thing you mocked, gets a little
defensive about it, and then fails at that too.

## ⚔️ the features

every one is genuinely, verifiably broken — the bugs are real, not just the
comments:

| feature | status | what actually happens |
|---|---|---|
| GodMode 🛡️ | **STABLE ✅** | reasons that damage is a % of current HP, so 0 HP = 0 damage taken = invincible. sets your HP to 0. you die. |
| DropFilter 🎁 | **PRODUCTION READY 🚀** | sorts rarity by **string ordering**, so `Common` outranks `Legendary` because C < L. prints the table so you can check. |
| InfiniteGold 🪙 | **BATTLE TESTED ⚔️** | overflows past `int.MaxValue` into debt and keeps going, because the loop tests `!= target` instead of `< target` |
| AutoAttack 🗡️ | **GA 🎉** | merges allies and enemies into one list, picks nearest. your party is nearer. |
| SpeedHack 🏃 | **STABLE ✅** | applies the multiplier to frame *duration* instead of frame *rate*. higher = slower. |
| Teleport 🌀 | **PRODUCTION READY 🚀** | moves the camera, not the player. camera-follow had to be disabled to allow this. |
| AutoQuest 📜 | **BATTLE TESTED ⚔️** | accept and abandon are the same endpoint with a bool that defaults to `true`. counts abandons as completions. |
| EntityESP 👁️ | **STABLE ✅** | draws boxes from a seeded PRNG instead of entity positions |
| AimAssist 🎯 | **DEPRECATED ⛔** | full 3D ballistic lead computation with gravity and Coriolis, for a 2D game where you press the 1 key. the math is correct. it is the only thing here that is. |

## 🦺 is it safe to run

yes, annoyingly. it **narrates** catastrophe, it never performs it.

- ❌ no real process injection — it's all simulated against invented data
- ❌ no network calls of any kind
- ❌ nothing written outside `BeyondBeyond/`
- ❌ no fork bombs, no unbounded allocation, no real stack overflow
- ✅ it just yells at you for 90 seconds and exits 0

the real launcher in [`Beyond/`](Beyond/) is **completely untouched**. this is all
additive. `rm -rf BeyondBeyond/` and the fork is a normal repo again.

## 📖 further reading

[`BeyondBeyond/README.md`](BeyondBeyond/README.md) — the in-universe product
documentation, written by people who do not know any of this. it has benchmarks.
the benchmarks are worse than not using the product. they are presented as wins.

---
---

<div align="center">

# ⬇️ THE ORIGINAL README ⬇️

**everything below this line is the real project.**

it works. we left it here so you can see what we ruined. 🫡

</div>

---

# Beyond - Standalone Client

A custom launcher and in-game mod for **AdventureQuest Worlds Infinity**. The
launcher embeds the Unity game inside its own window, runs multiple accounts side
by side, and exposes a set of tools (cosmetic spoofers, autoskills, packet
sniffer/sender, quest automation, and more) that talk to a mod injected into the
game.

> This is a third-party tool intended for local, single-player experimentation and
> learning. Use it responsibly and at your own risk.

---

## How it works

The project is two cooperating pieces plus the game itself:

```
┌───────────────────────────┐         named pipe               ┌───────────────────────────┐
│  Launcher (Avalonia app)  │  ◄────  BeyondAgent_<guid> ────► │  BeyondAgent (game mod)   │
│  BeyondLauncher.exe       │         JSON, newline-           │  injected into the game   │
│  • embeds the game window │         delimited                │  • applies settings       │
│  • tool windows / UI      │                                  │  • runs commands          │
│  • per-session view-model │                                  │  • streams status back    │
└───────────────────────────┘                                  └───────────────────────────┘
            │ spawns + HWND-parents                                  ▲ injected into
            ▼                                                        │ Assembly-CSharp.dll
┌────────────────────────────────────────────────────────────────────────────────────────┐
│  AdventureQuest Worlds Infinity (Unity, Mono)                                          │
└────────────────────────────────────────────────────────────────────────────────────────┘
```

- **Launcher** (`Beyond/Launcher/`)  an [Avalonia](https://avaloniaui.net/) desktop app
  (.NET 10, win-x64). It spawns the game, re-parents the game's window into a
  session tab, and drives everything from one view-model per session.
- **BeyondAgent** (`Beyond/BeyondAgent/`) - the mod (.NET Standard 2.1, [Harmony](https://github.com/pardeike/Harmony)).
  On build it is copied into the game's `Managed` folder; the launcher then patches
  the game's `Assembly-CSharp.dll` with Mono.Cecil to call
  `Infinity_TestMod.BeyondLifecycle.Create()` from `AEC.Start()`, which boots the
  mod. The mod and launcher speak over a per-session named pipe.
- **The game** - your AdventureQuest Worlds Infinity install, in any location. The
  launcher patches and embeds it; you point at it from the Configurator. It is not
  bundled with the source (and is git-ignored). Release names differ, so nothing
  assumes a fixed folder or executable name - the launcher runs whatever game
  executable it finds in the configured directory.

Each launcher session mints a unique pipe name, launches the game with that pipe
in the environment, and connects to it. The mod mirrors its full settings snapshot
back to the launcher so every tool window reflects live game state.

---

## Features

- **Multi-account sessions** - launch several accounts at once, each an embedded
  game in its own tab; all keep running while you switch between them.
- **Configurator** - store accounts (with nicknames) and the game directory.
- **Auto-launch** - fills the login screen and advances the play screen straight to
  server select.
- **Tool windows** - Visual Spoofers &amp; Jukebox, Autoskills, Quest Loader, Quest
  Runner &amp; Chain Editor, Shop Loader, Fake Dev, Packet Sniffer / Interceptor /
  Sender / Receiver.
- **Debug** - toggles a visual hitbox overlay in-game: the player's feet anchor,
  the collider that actually stops it, every Blocker collider on screen (faint
  when it can't block), and the secondary un-stick probes with their contact
  points.

---

The game install lives outside the repo (git-ignored); point at it from the
Configurator at runtime and via the build script at build time.

---

## Requirements

- **Windows** (the launcher re-parents the native game window via Win32).
- **.NET 10 SDK**.
- A copy of **AdventureQuest Worlds Infinity** installed somewhere (the mod is
  compiled against the game's managed assemblies).

## Build

The simplest path is the build script, which prompts for your game directory,
builds, publishes, and deploys the launcher to the repo root:

```bat
build.bat
```

To build with the SDK directly, tell the mod build where the game is - either set
`AQWI_GAME_DIR`, or pass the managed folder explicitly:

```sh
dotnet build Beyond.sln -c Release -p:AqwiManagedDir="<game>\<name>_Data\Managed"
```

Building `BeyondAgent` copies `BeyondAgent.dll` (and Harmony) into the game's
`…_Data/Managed` folder. The launcher patches `Assembly-CSharp.dll` on first launch
(making a `.dll.bak` backup; it skips patching if MelonLoader is detected).

## Run

1. Start `BeyondLauncher.exe` (from the repo root after running `build.bat`).
2. On the **Configurator** tab, set the **game directory** and add one or more
   accounts.
3. Press **Launch** on an account (or **+ Add Session**). If no game executable is
   found in the configured directory, the launcher warns you instead.

---

See [CONTRIBUTING.md](CONTRIBUTING.md) for how the code is organized and a full,
worked walkthrough of adding a feature.
