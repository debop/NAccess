using System;
using System.Threading;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.InversionOfControl;
using NSoft.NFramework.UnitTesting;
using NUnit.Framework;
using SharpTestsEx;

namespace NSoft.NAccess.SetUp
{
    [TestFixture]
    public class UnitOfWorkSetUpTestFixture
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        #endregion

        [TestFixtureSetUp]
        public void ClassSetUp()
        {
            if(IoC.IsNotInitialized)
                IoC.Initialize();
        }

        [Test]
        public void Can_Initialize_UnitOfWork()
        {
            Thread.Sleep(10);

            if(IoC.IsNotInitialized)
                IoC.Initialize();

            Thread.Sleep(1);

            IoC.IsInitialized.Should("IoC.IsInitialized").Be.True();

            Thread.Sleep(10);

            using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
            {
                UnitOfWork.IsStarted.Should("UnitOfWork.IsStarted").Be.True();
                UnitOfWork.Current.Should("UnitOfWork.Current").Not.Be.Null();
            }
        }

        [Test]
        public void Thread_Test()
        {
            TestTool.RunTasks(3, Can_Initialize_UnitOfWork);
        }
    }
}