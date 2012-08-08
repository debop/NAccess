using System;
using System.Collections.Generic;
using System.IO;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.ForTesting;
using NSoft.NFramework.Tools;
using NSoft.NAccess.Domain.Model;
using NUnit.Framework;

namespace NSoft.NAccess.Domain
{
    /// <summary>
    /// Domain 관련 테스트를 위한 기본 클래스입니다.
    /// </summary>
    [TestFixture]
    public abstract class DomainTestFixtureBase : DatabaseTestFixtureBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        private const bool UseSecondLevelCache = false;
        private const bool ShowSql = false;
        private const int CommandTimeout = 90;
        private const int BatchSize = 30;
        private const string SecondLevelCacheSharedCacheProvider = @"NSoft.NFramework.Caching.SharedCache.NHCaches.SharedCacheProvider, NSoft.NFramework.Caching.SharedCache";
        private const string SecondLevelCacheHashtableCacheProvider = @"NHibernate.Cache.HashtableCacheProvider, NHibernate";

        #region << For Unit Testing >>

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            OnTestFixtureSetUp();

            //With.TryAction(OnTestFixtureSetUp,
            //    ex =>
            //    {
            //        if(log.IsErrorEnabled)
            //        {
            //            log.ErrorException(@"NHibernate 초기화시에 예외가 발생했습니다.", ex);
            //        }
            //    });
        }

        [SetUp]
        public void SetUp()
        {
            OnSetUp();
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            OnTestFixtureTearDown();
        }

        protected virtual void OnTestFixtureSetUp()
        {
            //IoC.Reset();
            //DisposeAndRemoveAllUoWTestContexts();

            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            InitializeNHibernateAndIoC(ContainerFilePath,
                                       GetDatabaseEngine(),
                                       GetDatabaseName(),
                                       GetMappingInfo(),
                                       GetNHibernateProperties(),
                                       cfg =>
                                       {
                                           // 속도를 느리게하는 주범입니다. ㅠ.ㅠ
                                           //if(cfg != null)
                                           //{
                                           //    cfg.SetListener(ListenerType.PreInsert, new NSoft.NFramework.Data.NH.UpdateTimestampEventListener());
                                           //    cfg.SetListener(ListenerType.PreUpdate, new NSoft.NFramework.Data.NH.UpdateTimestampEventListener());
                                           //}
                                       });

            CurrentContext.CreateUnitOfWork();

            // NOTE: Domain을 테스트할 때, 꼭 Current Context의 CompanyCode, DepartmentCode, UserId 를 설정해줘야 한다.
            //
            NAccessContext.Current.CompanyCode = SampleData.CompanyCode;
            NAccessContext.Current.DepartmentCode = SampleData.DepartmentCode;
            NAccessContext.Current.UserCode = SampleData.UserCode;

            if(UnitOfWork.CurrentSessionFactory.IsSQLite())
            {
                UnitOfWork.CurrentSession.CreateSQLQuery("PRAGMA synchronous=off;").ExecuteUpdate();
                UnitOfWork.CurrentSession.CreateSQLQuery("PRAGMA journal_mode=OFF;").ExecuteUpdate();
            }
        }

        protected virtual void OnSetUp() { }

        protected virtual void OnTearDown() { }

        protected virtual void OnTestFixtureTearDown()
        {
            DisposeAndRemoveAllUoWTestContexts();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region << 테스트 환경 설정 용 >>

        protected virtual string ContainerFilePath
        {
            get { return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"NAccess.IoC.config")); }
        }

        protected virtual DatabaseEngine GetDatabaseEngine()
        {
            return DatabaseEngine.MsSql2005;
        }

        protected virtual string GetDatabaseName()
        {
            if(GetDatabaseEngine() == DatabaseEngine.DevartOracle)
                return ConfigTool.GetConnectionString("LOCAL_XE");

            return AdoTool.DefaultDatabaseName;
        }

        protected virtual MappingInfo GetMappingInfo()
        {
            return MappingInfo.From(typeof(Company).Assembly);
        }

        protected virtual IDictionary<string, string> GetNHibernateProperties()
        {
            var properties = new Dictionary<string, string>();

            properties.Add(NHibernate.Cfg.Environment.CacheProvider, SecondLevelCacheHashtableCacheProvider); // SecondLevelCacheHashtableCacheProvider); //SecondLevelCacheSharedCacheProvider);
            properties.Add(NHibernate.Cfg.Environment.UseSecondLevelCache, UseSecondLevelCache.ToString());
            //properties.Add(NHibernate.Cfg.Environment.UseQueryCache, "True");
            properties.Add(NHibernate.Cfg.Environment.ShowSql, ShowSql.ToString());

            //properties.Add(NHibernate.Cfg.Environment.QuerySubstitutions, "true 1, false 0, yes 'Y', no 'N'");
            //properties.Add(NHibernate.Cfg.Environment.CommandTimeout, CommandTimeout.ToString());

            return properties;
        }

        #endregion
    }

    public abstract class DomainTestFixtureBase_No_IoC : DomainTestFixtureBase
    {
        protected override string ContainerFilePath
        {
            get { return string.Empty; }
        }
    }
}