using System;
using NSoft.NFramework.InversionOfControl;
using NUnit.Framework;

namespace NSoft.NAccess
{
    public abstract class NAccessFixtureBase
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        protected static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        protected static bool IsDebugEnabled
        {
            get { return lazyLog.Value.IsDebugEnabled; }
        }

        #endregion

        [TestFixtureSetUp]
        public void ClassSetUp()
        {
            IoC.Initialize();
        }

        [TestFixtureTearDown]
        public void ClassTearDown()
        {
            IoC.Reset();
        }
    }
}