using System.Collections.Generic;
using System.Linq;
using NSoft.NFramework.Data;
using NSoft.NFramework.InversionOfControl;
using NUnit.Framework;
using SharpTestsEx;

namespace NSoft.NAccess.BackgroundServices
{
    [TestFixture]
    public class AdoPreCacheServiceFixture : NAccessFixtureBase
    {
        public static string[] EntityNames = {
                                                 "CODE", "COMPANY", "DEPARTMENT", "GROUP", "USER", "MENU", "PRODUCT", "RESOURCE"
                                             };

        public const string SelectEntityFormat = @"SELECT * FROM [{0}] WITH (NOLOCK)";

        public static IEnumerable<string> SelectEntityQueries
        {
            get { return EntityNames.Select(entity => string.Format(SelectEntityFormat, entity)); }
        }

        public static IAdoRepository Repository
        {
            get { return IoC.TryResolve(() => AdoRepositoryFactory.Instance.CreateRepositoryByProvider(), true); }
        }

        [Test]
        public void PrepareConnection()
        {
            using(var table = Repository.ExecuteDataTableAsync("SELECT * FROM dbo.sysobjects").Result)
            {
                table.Should().Not.Be.Null();
                table.HasErrors.Should().Be.False();
            }
        }

        [Test]
        public void RunAdoPreCacheService()
        {
            // 지정한 여러 쿼리문을 동시에 실행하여, RDBMS에서 메모리로 미리 캐시하도록 합니다.
            //
            var adoPreCacheService = new AdoPreCacheService(Repository, SelectEntityQueries.ToArray());

            adoPreCacheService.Execute()
                .Should().Be.True();
        }

        [Test]
        public void RunAdoPreCacheService_IsCached()
        {
            // 지정한 여러 쿼리문을 동시에 실행하여, RDBMS에서 메모리로 미리 캐시하도록 합니다.
            //
            var adoPreCacheService = new AdoPreCacheService(Repository, SelectEntityQueries.ToArray());

            adoPreCacheService.Execute()
                .Should().Be.True();
        }

        [Test]
        public void RunAdoPreCacheServiceAsync()
        {
            var adoPreCacheService = new AdoPreCacheService(Repository, SelectEntityQueries.ToArray());

            var executed = adoPreCacheService.ExecuteServiceAsync().Result;
            Assert.IsTrue(executed);
        }
    }
}