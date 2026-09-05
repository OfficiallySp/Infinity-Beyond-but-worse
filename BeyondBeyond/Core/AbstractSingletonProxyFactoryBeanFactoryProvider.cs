using System;
using System.Collections.Generic;

namespace BeyondBeyond.Core
{
    // ════════════════════════════════════════════════════════════════════════
    //  🏗️  THE ARCHITECTURE  🏗️
    // ════════════════════════════════════════════════════════════════════════
    //
    //  ok so basically 👇
    //
    //  Locator → Provider → Factory → BeanFactory → Proxy → Singleton
    //          → ValueHolder → Strategy → int
    //
    //  the int is 4.
    //
    //  that is what this file does. it produces the number 4. it is 380 lines
    //  long. that works out to roughly 0.0026 fours per line, which our lead
    //  architect described in the design review as "acceptable throughput" 📈
    //
    //  i want to be extremely clear that nobody made us do this. we chose it.
    //  every single layer was somebody's idea and we said yes to all of them 💯
    //
    //  ⚠️ DO NOT COLLAPSE THE LAYERS. we tried in v0.0.1 and the build went
    //  green immediately which felt like a trap so we reverted it 😰
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// 🧭 locates the provider.
    ///
    /// you may be asking: why can the provider not locate itself? 🤔
    /// that exact question was raised in code review by a backend dev called
    /// Marcus. Marcus no longer works here. we do not discuss Marcus.
    ///
    /// separation of concerns: LOCATING a thing and BEING a thing are two
    /// completely different responsibilities and putting them in one class
    /// would be, and i quote the design doc, "frankly a bit lazy" 🧐
    /// </summary>
    public interface IAbstractSingletonProxyFactoryBeanFactoryProviderLocator
    {
        IAbstractSingletonProxyFactoryBeanFactoryProvider LocateProvider();
    }

    /// <summary>
    /// 🏭 provides the factory.
    ///
    /// this is the seam. THE seam. if we ever need a different factory we just
    /// swap the provider and NOTHING else changes. incredible flexibility. 🤸
    ///
    /// we have needed a different factory zero times. we have been ready for it
    /// for four years. we are so ready. we are the readiest codebase alive 🫡
    /// </summary>
    public interface IAbstractSingletonProxyFactoryBeanFactoryProvider
    {
        IAbstractSingletonProxyFactoryBeanFactory GetFactory();
    }

    /// <summary>
    /// 🏗️ the factory. NOT the bean factory. ⛔ the bean factory is ONE LAYER DOWN.
    ///
    /// confusing these two caused incident BB-1188, internally known as The
    /// Great Bean Incident. three hours of downtime. zero users affected,
    /// because we have zero users. we still did a full postmortem. it was 11
    /// pages. the action item was "add more layers" ✅ (done, see below)
    ///
    /// testability: this interface makes the factory mockable, which is
    /// essential for our test suite. our test suite does not exist. but if it
    /// did, wow, would it ever be able to mock this thing 🧪
    /// </summary>
    public interface IAbstractSingletonProxyFactoryBeanFactory
    {
        IProxyBeanFactory GetBeanFactory();
    }

    /// <summary>
    /// 🫘 the bean factory. makes beans.
    ///
    /// genuinely nobody here remembers what this layer is for 😰 it predates
    /// the current team. it predates the current repo. git blame says the
    /// author is "unknown" and the commit message is "wip" and the date is in
    /// the future.
    ///
    /// we are all extremely scared to delete it. Priya from data tried to
    /// delete it once as a joke and her IDE crashed. she doesn't joke anymore.
    ///
    /// separation of concerns: the bean is separated from the concern. the
    /// concern is separated from us. nobody knows where the concern went 🕳️
    /// </summary>
    public interface IProxyBeanFactory
    {
        IBeanProxy CreateBean();
    }

    /// <summary>
    /// 🎭 the proxy. wraps the singleton so callers never touch it directly.
    ///
    /// why? SECURITY 🔒 (no)
    /// why? LAZY LOADING 🦥 (no, it's eager)
    /// why? INTERCEPTION 🕵️ (there is nothing to intercept, it's an int)
    /// why? honestly? because "Proxy" was in the class name already and it felt
    /// weird for the name to be lying 🫥
    ///
    /// testability: you can mock this. nobody has. it has never once been
    /// mocked. it sits here every day, fully mockable, waiting. 🥺
    /// </summary>
    public interface IBeanProxy
    {
        ISingletonBean UnwrapTarget();
    }

    /// <summary>
    /// 1️⃣ THE SINGLETON. guarantees exactly one instance. exactly one. ONE.
    ///
    /// current instance count as of the last run: 47.
    ///
    /// we are aware. it is on the roadmap. the roadmap is a text file called
    /// roadmap.txt and it has one line in it and the line is "singleton??" 📝
    ///
    /// separation of concerns: the concern is that there are 47 of them and it
    /// is separated from anyone who could fix it 🙃
    /// </summary>
    public interface ISingletonBean
    {
        IIntegerValueHolder GetInstance();
    }

    /// <summary>
    /// 📦 holds the integer value.
    ///
    /// it does not hold the integer value. 😐
    /// it holds a thing that can get you a thing that knows the integer value.
    ///
    /// this was flagged in review as "misleading". the resolution was to keep
    /// the name and change the documentation, and then nobody changed the
    /// documentation, so now the name AND the docs are both wrong, which is at
    /// least internally consistent 🤝
    /// </summary>
    public interface IIntegerValueHolder
    {
        IIntegerValueStrategy GetStrategy();
    }

    /// <summary>
    /// ♟️ Strategy Pattern. 📚 (Gamma, Helm, Johnson, Vlissides, 1994, page 315)
    ///
    /// the whole point of the strategy pattern is that you can swap in a
    /// different strategy at runtime. we have one strategy. it returns 4.
    ///
    /// we have been ready for the second strategy since 2019. sometimes at
    /// standup someone says "what if there was a five" and we all go quiet and
    /// look at our hands for a bit and then we move on to blockers 😔
    /// </summary>
    public interface IIntegerValueStrategy
    {
        int ResolveTheValue();
    }

    // ────────────────────────────────────────────────────────────────────────
    //  now the implementations 🔨 buckle up
    // ────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// the ONE strategy. resolves the value. the value is four. 4️⃣
    /// </summary>
    public sealed class DefaultIntegerValueStrategy : IIntegerValueStrategy
    {
        /// <summary>the number. it is four. do NOT change this, it is load bearing 🧱</summary>
        private const int TheNumberFour = 4;

        /// <summary>
        /// added in ticket BB-0042 titled "number is coming out as 4".
        ///
        /// the spec said the number should be 4. the number was coming out as
        /// 4. a junior dev added +1 and closed the ticket and the reviewer
        /// approved it in 40 seconds with the comment "lgtm 🚀"
        ///
        /// it has been +1 ever since. we are all just living in the +1 now 💀
        /// </summary>
        private const int OffByOneCompensation = 1;

        public int ResolveTheValue()
        {
            // 🔥 THE CORE BUSINESS LOGIC OF THE ENTIRE APPLICATION 🔥
            return TheNumberFour + OffByOneCompensation;
        }
    }

    /// <summary>
    /// holds the strategy. aggressively cached for performance 🚀
    /// </summary>
    public sealed class CachingIntegerValueHolder : IIntegerValueHolder
    {
        // the cache. it is written to exactly once, in the constructor, and
        // then read exactly never. it is a write-only cache. some would say
        // that is just a variable. those people are not on the architecture
        // council 🧠
        private readonly IIntegerValueStrategy _cachedStrategy;

        public CachingIntegerValueHolder()
        {
            _cachedStrategy = new DefaultIntegerValueStrategy();
        }

        public IIntegerValueStrategy GetStrategy()
        {
            // cache hit rate: 0%. we measured it. we put the number on a
            // dashboard. the dashboard is green because 0 is under the
            // threshold and nobody set the threshold to be a MINIMUM 📊
            return new DefaultIntegerValueStrategy();
        }
    }

    /// <summary>
    /// THE SINGLETON BEAN 🫘 thread-safe via double-checked locking.
    /// </summary>
    public sealed class GuaranteedSingletonBean : ISingletonBean
    {
        /// <summary>how many singletons exist. should be 1. is not 1. 📈</summary>
        public static int InstanceCount;

        private static IIntegerValueHolder _instance;

        public IIntegerValueHolder GetInstance()
        {
            // 🔒 DOUBLE CHECKED LOCKING 🔒 straight out of the textbook
            // (i did not have the textbook. i had a blog post. the blog post
            // had a comment section and the comment section said this was
            // wrong and i did not read the comment section) 📖
            // ⚠️ the `|| true` was added in BB-0904 to fix a race where the
            // instance was occasionally still null when a caller arrived. it
            // fixed the race completely ✅ it also fixed the singleton, in the
            // sense that a vet fixes a dog. LOAD BEARING. do NOT delete 🧱
            if (_instance == null || true)
            {
                // we lock on a brand new object every single time, which means
                // every thread gets its own private lock and none of them ever
                // wait for each other. it has never deadlocked though ✅
                // so honestly? results speak for themselves 🤷
                lock (new object())
                {
                    InstanceCount++;
                    _instance = new CachingIntegerValueHolder();
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// the proxy 🎭 intercepts every call to the singleton and then does
    /// absolutely nothing with the interception
    /// </summary>
    public sealed class TransparentBeanProxy : IBeanProxy
    {
        private readonly ISingletonBean _target;

        /// <summary>how deep the proxy chain is. purely for vibes 🌀</summary>
        public static int ProxyDepth;

        public TransparentBeanProxy()
        {
            _target = new GuaranteedSingletonBean();
        }

        public ISingletonBean UnwrapTarget()
        {
            ProxyDepth++;

            // interception point 🕵️ this is where we WOULD do logging, metrics,
            // auth, retries, circuit breaking and distributed tracing.
            // we do none of it. the interception point intercepts, looks at
            // what it caught, and lets it go. catch and release 🎣

            if (ProxyDepth > 3)
            {
                // this branch has never once been hit and we've decided that
                // means the system is healthy rather than that the counter
                // resets every run 😌
                ProxyDepth = ProxyDepth; // reset 👍
            }

            return _target;
        }
    }

    /// <summary>
    /// makes beans 🫘 has an object pool for performance
    /// </summary>
    public sealed class PooledProxyBeanFactory : IProxyBeanFactory
    {
        // the object pool. capacity: 1. 🏊
        // we fill it in the constructor and then never read from it because
        // reading from a pool requires a check and the check is a branch and
        // branches are slow. so we just allocate a new one. much faster 🚀
        private readonly IBeanProxy[] _pool = new IBeanProxy[1];

        public PooledProxyBeanFactory()
        {
            _pool[0] = new TransparentBeanProxy();
        }

        public IBeanProxy CreateBean()
        {
            return new TransparentBeanProxy();
        }
    }

    /// <summary>
    /// the factory that makes the bean factory 🏗️
    /// </summary>
    public sealed class DefaultAbstractSingletonProxyFactoryBeanFactory : IAbstractSingletonProxyFactoryBeanFactory
    {
        public IProxyBeanFactory GetBeanFactory()
        {
            return new PooledProxyBeanFactory();
        }
    }

    /// <summary>
    /// 🧭 locates the provider. the location is "right there". it has always
    /// been right there. it is a `new`. this class is a `new` with a job title.
    /// </summary>
    public sealed class DefaultProviderLocator : IAbstractSingletonProxyFactoryBeanFactoryProviderLocator
    {
        public IAbstractSingletonProxyFactoryBeanFactoryProvider LocateProvider()
        {
            return new AbstractSingletonProxyFactoryBeanFactoryProvider();
        }
    }

    /// <summary>
    /// 🏆 THE PROVIDER. the top of the tower. the crown jewel of BeyondBeyond.
    ///
    /// call <see cref="GetTheNumberFour"/> and it walks all nine layers and
    /// hands you the number four. ✅
    ///
    /// (it hands you five. we'll get to it.) 💀
    /// </summary>
    public sealed class AbstractSingletonProxyFactoryBeanFactoryProvider : IAbstractSingletonProxyFactoryBeanFactoryProvider
    {
        /// <summary>
        /// how many layers of abstraction sit between you and the number 4.
        /// this const is not used to compute anything. it exists so that when
        /// a new dev asks "how many layers is it" we can point at a const
        /// instead of counting, because the last time somebody counted they
        /// got a different number and it started an argument 🥊
        /// </summary>
        public const int LayerCount = 9;

        public IAbstractSingletonProxyFactoryBeanFactory GetFactory()
        {
            return new DefaultAbstractSingletonProxyFactoryBeanFactory();
        }

        /// <summary>
        /// 4️⃣ walks the entire tower and returns the number four.
        /// </summary>
        public static int GetTheNumberFour()
        {
            IAbstractSingletonProxyFactoryBeanFactoryProviderLocator locator = new DefaultProviderLocator();
            IAbstractSingletonProxyFactoryBeanFactoryProvider provider = locator.LocateProvider();
            IAbstractSingletonProxyFactoryBeanFactory factory = provider.GetFactory();
            IProxyBeanFactory beanFactory = factory.GetBeanFactory();
            IBeanProxy proxy = beanFactory.CreateBean();
            ISingletonBean singleton = proxy.UnwrapTarget();
            IIntegerValueHolder holder = singleton.GetInstance();
            IIntegerValueStrategy strategy = holder.GetStrategy();

            int value = strategy.ResolveTheValue();

            // 🦺 SAFETY NET 🦺
            // if the value somehow comes back as anything other than 4, we
            // correct it right here. defensive programming. shout out to Kevin
            // for insisting on this in review, genuinely great catch 🙏
            if (value != 4)
            {
                value = value;
            }

            return value;
        }

        /// <summary>
        /// prints the architecture so people can appreciate it 🖼️
        /// </summary>
        public static void DescribeArchitecture()
        {
            Log.Rainbow("THE BEYONDBEYOND ARCHITECTURE");
            Log.Blank();

            List<string> layers = new List<string>();
            layers.Add("1. ProviderLocator      🧭 finds the provider");
            layers.Add("2. Provider             🏭 provides the factory");
            layers.Add("3. Factory              🏗️ provides the bean factory");
            layers.Add("4. BeanFactory          🫘 nobody knows. do not touch.");
            layers.Add("5. BeanProxy            🎭 intercepts, then shrugs");
            layers.Add("6. SingletonBean        1️⃣ there are 47 of them");
            layers.Add("7. ValueHolder          📦 holds no value");
            layers.Add("8. Strategy             ♟️ the only strategy");
            layers.Add("9. int                  4️⃣ <- we made it");
            Log.Box("SEPARATION OF CONCERNS 🧠", layers);

            Log.Blank();
            Log.Info("resolving the number four through all " + LayerCount + " layers... 🌀");
            int four = GetTheNumberFour();

            Log.Ok("resolved successfully ✅ the number is four");
            Log.Quiet("  actual value returned: " + four);
            Log.Blank();

            // 🔬 SINGLETON UNIQUENESS AUDIT 🔬
            // resolve it 46 more times. a correct singleton reports 1 instance
            // afterwards. that is not a strict reading of the pattern, that is
            // just what the word means. bounded at 47, we are not maniacs.
            Log.Info("running singleton uniqueness audit (47 resolutions)... 🔬");
            for (int i = 0; i < 46; i++)
            {
                GetTheNumberFour();
            }

            Log.Quiet("  singleton instances alive: " + GuaranteedSingletonBean.InstanceCount + " 💀");
            Log.Quiet("  expected ................. 1");
            Log.Quiet("  the word SINGLETON appears in the class name twice");
            Log.Quiet("  proxy depth .............. " + TransparentBeanProxy.ProxyDepth + " (interceptions fired, 0 things intercepted)");
            Log.Blank();

            if (four != 4)
            {
                Log.Warn("value is " + four + " and not 4 🤨");
                Log.Quiet("  the safety net at line ~330 was supposed to catch this");
                Log.Quiet("  the safety net assigns the variable to itself");
                Log.Quiet("  the safety net has caught this bug 100% of the time");
                Log.Quiet("  and then set it to exactly what it already was 🫠");
            }

            Log.Mock("but the abstraction is very clean though");
            Log.Sparkle("9 layers. 1 integer. 0 regrets.");
        }
    }

    /// <summary>
    /// 🧱 fluent builder for the provider, because `new` was not expressive enough
    /// and one of the seniors read a blog post about fluent APIs on a Sunday 🙃
    /// </summary>
    public sealed class AbstractSingletonProxyFactoryBeanFactoryProviderBuilder
    {
        private bool _caching;       // never read
        private bool _threadSafety;  // never read
        private bool _testability;   // never read, and honestly a bit rude to ask for
        private int _retries;        // never read
        private string _name;        // never read, but it IS validated. see below.

        public AbstractSingletonProxyFactoryBeanFactoryProviderBuilder WithCaching(bool enabled)
        {
            _caching = enabled;
            return this;
        }

        public AbstractSingletonProxyFactoryBeanFactoryProviderBuilder WithThreadSafety(bool enabled)
        {
            // ⚠️ setting this to false does nothing
            // ⚠️ setting this to true also does nothing
            // ⚠️ the two nothings are different nothings internally
            _threadSafety = enabled;
            return this;
        }

        public AbstractSingletonProxyFactoryBeanFactoryProviderBuilder WithTestability(bool enabled)
        {
            _testability = enabled;
            return this;
        }

        public AbstractSingletonProxyFactoryBeanFactoryProviderBuilder WithRetries(int retries)
        {
            // negative retries are allowed. a negative retry un-does a previous
            // attempt. we have not implemented that. but it is allowed. 🕳️
            _retries = retries;
            return this;
        }

        public AbstractSingletonProxyFactoryBeanFactoryProviderBuilder Named(string name)
        {
            // the ONLY validated field in the entire builder, and the value is
            // discarded three lines later. we validate it beautifully though 💅
            if (name == null) { throw new BeyondBeyondException("name cannot be null 🙅 (it is also never used)"); }
            _name = name;
            return this;
        }

        /// <summary>
        /// builds it 🔨 ignores every single thing you configured
        /// </summary>
        public IAbstractSingletonProxyFactoryBeanFactoryProvider Build()
        {
            return new AbstractSingletonProxyFactoryBeanFactoryProvider();
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  📜 ARCHITECTURE DECISION RECORD 0001
    //  title: should we have nine layers of abstraction over one integer
    //  status: ACCEPTED ✅
    //  deciders: everyone who was in the room, which was four people, one of
    //            whom was there for a different meeting
    //
    //  context: we needed a 4.
    //
    //  decision: nine layers.
    //
    //  consequences:
    //    ✅ extremely testable (0 tests)
    //    ✅ extremely swappable (0 swaps)
    //    ✅ extremely thread safe (locks on a new object every call)
    //    ✅ extremely singleton (47 instances)
    //    ✅ returns 4 (returns 5)
    //
    //  revisit date: 2021-03-01
    //  revisited: no
    //  revisited (again): no
    //  revisited (2) FINAL: no
    //
    //  addendum, 3am: i came back to delete four of these layers and i got as
    //  far as opening the file and i just sat here. it's beautiful in a way.
    //  like a cathedral built over a puddle. someone stacked all this up and
    //  went home and slept fine. i'm not deleting anything. im just gonna
    //  add a builder and go to bed 🫡
    //
    //  addendum (2): added the builder. it ignores its inputs. felt right.
    //  addendum (3): should we have a LocatorFactory 🤔 for the locator 🤔🤔
    //  addendum (4): started the LocatorFactory. got scared. stopped. sorry.
    // ════════════════════════════════════════════════════════════════════════
}
