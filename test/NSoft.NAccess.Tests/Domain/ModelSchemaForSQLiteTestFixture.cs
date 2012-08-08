using NSoft.NFramework.Data.NHibernateEx;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NSoft.NAccess.Domain
{
    [TestFixture]
    public class ModelSchemaForSQLiteTestFixture : FluentDomainTestFixtureBase// DomainTestFixtureBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        protected override DatabaseEngine GetDatabaseEngine()
        {
            return DatabaseEngine.SQLite;
        }

        [Test]
        public void GenerateSchema()
        {
            if(log.IsDebugEnabled)
                log.Debug(@"Generate Schema");

            new SchemaExport(CurrentContext.NHConfiguration).SetDelimiter(@";").Create(true, false);
        }

        [Test]
        public void GenerateSchemaToFile()
        {
            if(log.IsDebugEnabled)
                log.Debug(@"Schema를 생성하고, Oracle DB를 실제로 생성합니다...");

            new SchemaExport(CurrentContext.NHConfiguration)
                .SetOutputFile(@".\..\..\NAccess.SQLite.sql")
                .SetDelimiter(@";")
                .Create(true, true);

            if(log.IsDebugEnabled)
                log.Debug(@"Schema를 생성하고, DB를 실제로 생성했습니다!!!");
        }
    }
}