using System;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.InversionOfControl;
using NSoft.NFramework.UnitTesting;
using NUnit.Framework;
using SharpTestsEx;

namespace NSoft.NAccess.SetUp
{
    [TestFixture]
    public class InversionOfControlTestFixture
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        #endregion

        [Test]
        public void Can_Initialize_IoC_Container_From_AppConfig()
        {
            if(log.IsDebugEnabled)
                log.Debug("Can Initialize IoC Container Test");

            if(IoC.IsNotInitialized)
                IoC.Initialize();

            IoC.IsInitialized.Should().Be.True();

            Assert.IsNotNull(IoC.Resolve<IUnitOfWorkFactory>());
        }

        [Test]
        public void Thread_Test()
        {
            TestTool.RunTasks(3, Can_Initialize_IoC_Container_From_AppConfig);
        }
    }
}