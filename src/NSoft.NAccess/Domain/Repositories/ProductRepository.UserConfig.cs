using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 제품의 Configuration 관련 Domain Service
    /// </summary>
    public partial class ProductRepository
    {
        /// <summary>
        /// 사용자 설정 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">사용자 소속 회사 코드</param>
        /// <param name="userCode">사용자 코드</param>
        /// <param name="key">환경설정 키</param>
        /// <param name="value">환경설정 값</param>
        /// <returns></returns>
        public QueryOver<UserConfig, UserConfig> BuildQueryOverOfUserConfig(string productCode,
                                                                            string companyCode = null,
                                                                            string userCode = null,
                                                                            string key = null,
                                                                            string value = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"사용자 설정 정보를 조회하기 위한 Criteria를 빌드합니다... productCode={0}, companyCode={1}, userCode={2}, key={3}, value={4}",
                          productCode, companyCode, userCode, key, value);

            var query = QueryOver.Of<UserConfig>();

            if(productCode.IsNotWhiteSpace())
                query.AddWhere(uc => uc.Id.ProductCode == productCode);

            if(companyCode.IsNotWhiteSpace())
                query.AddWhere(uc => uc.Id.CompanyCode == companyCode);

            if(userCode.IsNotWhiteSpace())
                query.AddWhere(uc => uc.Id.UserCode == userCode);

            if(key.IsNotWhiteSpace())
                query.AddWhere(uc => uc.Id.Key == key);

            if(value.IsNotWhiteSpace())
                query.AddWhere(uc => uc.Value == value);

            return query;
        }

        /// <summary>
        /// 사용자 설정 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">사용자 소속 회사 코드</param>
        /// <param name="userCode">사용자 코드</param>
        /// <param name="key">환경설정 키</param>
        /// <param name="value">환경설정 값</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfUserConfig(string productCode,
                                                          string companyCode = null,
                                                          string userCode = null,
                                                          string key = null,
                                                          string value = null)
        {
            return BuildQueryOverOfUserConfig(productCode, companyCode, userCode, key, value).DetachedCriteria;
        }

        /// <summary>
        /// 사용자 환경설정 정보를 조회합니다. 없다면, 새로 생성해서 저장 후 반환합니다.
        /// </summary>
        /// <param name="product">제품 정보</param>
        /// <param name="user">사용자 정보</param>
        /// <param name="key">설정 키</param>
        /// <returns></returns>
        public UserConfig GetOrCreateUserConfig(Product product, User user, string key)
        {
            product.ShouldNotBeNull("product");
            user.ShouldNotBeNull("user");
            key.ShouldNotBeWhiteSpace("key");

            var userConfigIdentity = new UserConfigIdentity(product.Code, user.Company.Code, user.Code, key);

            if(IsDebugEnabled)
                log.Debug(@"UserConfig 정보를 조회합니다... " + userConfigIdentity);

            var userConfig = Repository<UserConfig>.Get(userConfigIdentity);

            if(userConfig != null)
                return userConfig;

            if(IsDebugEnabled)
                log.Debug(@"기존 UserConfig 정보가 없습니다. 새로 생성합니다... " + userConfigIdentity);

            // 성능을 위해 StatelessSession을 사용하여 저장합니다.
            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    Repository<UserConfig>.SaveOrUpdate(new UserConfig(userConfigIdentity, null));
                    UnitOfWork.Current.TransactionalFlush();
                }
            }

            userConfig = Repository<UserConfig>.Get(userConfigIdentity);
            userConfig.AssertExists("userConfig");

            if(log.IsInfoEnabled)
                log.Info(@"새로운 UserConfig 정보를 생성했습니다!!! New UserConfig = " + userConfig);

            return userConfig;
        }

        /// <summary>
        /// 사용자 환경설정 정보의 값을 가져옵니다. 환경설정 정보가 없거나, Value나 DefaultValue 속성이 빈문자열이라면 지정된 기본값을 반환합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="user">사용자</param>
        /// <param name="key">설정 키</param>
        /// <param name="defaultValue">기본 값 (설정값이 정의되어 있지 않았을 경우)</param>
        /// <returns></returns>
        public string GetUserConfigValue(Product product, User user, string key, string defaultValue)
        {
            var userConfig = FindOneUserConfigById(product, user, key);

            if(userConfig != null)
            {
                // 1. 실제 환경 설정 값이 있다면, 그 값을 반환한다.
                if(userConfig.Value.IsNotWhiteSpace())
                    return userConfig.Value;

                // 2. 환경설정 엔티티의 DefaultValue가 정의되어 있다면 그 값을 반환한다.
                if(userConfig.DefaultValue.IsNotWhiteSpace())
                    return userConfig.DefaultValue;
            }

            // 3. 호출자가 지정한 defaultValue를 반환한다.
            return defaultValue;
        }

        /// <summary>
        /// 사용자 환경설정 정보를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="user">사용자</param>
        /// <param name="key">설정 키</param>
        /// <returns></returns>
        public UserConfig FindOneUserConfigById(Product product, User user, string key)
        {
            product.ShouldNotBeNull("product");
            user.ShouldNotBeNull("user");
            key.ShouldNotBeWhiteSpace("key");

            if(IsDebugEnabled)
                log.Debug(@"UserConfig 정보를 조회합니다... productCode={0}, userCode={1}, key={2}", product.Code, user.Code, key);

            return Repository<UserConfig>.FindOne(BuildQueryOverOfUserConfig(product.Code, user.Company.Code, user.Code, key));
        }

        /// <summary>
        /// 지정된 회사에 소속된 모든 사용자의 UserConfig 정보를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="company">회사</param>
        /// <returns></returns>
        public IList<UserConfig> FindAllUserConfigByProductAndCompany(Product product, Company company)
        {
            product.ShouldNotBeNull("product");
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사에 소속된 모든 사용자의 UserConfig 정보를 조회합니다... productCode={0}, companyCode={1}",
                          product.Code, company.Code);

            return Repository<UserConfig>.FindAll(BuildQueryOverOfUserConfig(product.Code, company.Code));
        }

        /// <summary>
        /// 지정된 사용자의 모든 UserConfig 정보를 조회합니다.
        /// </summary>
        /// <param name="user">사용자</param>
        /// <returns></returns>
        public IList<UserConfig> FindAllUserConfigByUser(User user)
        {
            user.ShouldNotBeNull("user");

            if(IsDebugEnabled)
                log.Debug(@"지정된 사용자의 모든 UserConfig 정보를 조회합니다... " + user);

            return Repository<UserConfig>.FindAll(BuildQueryOverOfUserConfig(null, user.Company.Code, user.Code));
        }

        /// <summary>
        /// 지정된 회사에 소속된 모든 사용자의 UserConfig 정보를 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="product">검색 대상 제품</param>
        /// <param name="company">검색 대상 회사</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다.)</param>
        /// <param name="pageSize">Page Size (보통 10입니다. 0보다 커야 합니다.)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<UserConfig> GetPageOfUserConfigByProductAndCompany(Product product, Company company,
                                                                              int pageIndex, int pageSize, params INHOrder<UserConfig>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사에 소속된 사용자의 UserConfig 정보를 Paging 처리해서 로드합니다... " +
                          @"productCode={0}, companyCode={1}, pageIndex={2}, pageSize={3}, orders={4}",
                          product.Code, company.Code, pageIndex, pageSize, orders);

            return Repository<UserConfig>.GetPage(pageIndex,
                                                  pageSize,
                                                  BuildQueryOverOfUserConfig(product.Code, company.Code),
                                                  orders);
        }

        /// <summary>
        /// 지정된 사용자의 UserConfig 정보를 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="user">사용자</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다.)</param>
        /// <param name="pageSize">Page Size (보통 10입니다. 0보다 커야 합니다.)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<UserConfig> GetPageOfUserConfigByUser(User user, int pageIndex, int pageSize, params INHOrder<UserConfig>[] orders)
        {
            user.ShouldNotBeNull("user");

            if(IsDebugEnabled)
                log.Debug(@"지정된 사용자의 UserConfig 정보를 Paging 처리해서 로드합니다... " +
                          @"user={0}, pageIndex={1}, pagesize={2}, orders={3}",
                          user, pageIndex, pageSize, orders);

            return Repository<UserConfig>.GetPage(pageIndex,
                                                  pageSize,
                                                  BuildQueryOverOfUserConfig(null, user.Company.Code, user.Code),
                                                  orders);
        }

        /// <summary>
        /// UserConfig(사용자 환경설정) 정보를 삭제합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="user">사용자</param>
        /// <param name="key">설정 키</param>
        public void DeleteUserConfigById(Product product, User user, string key)
        {
            product.ShouldNotBeNull("product");
            user.ShouldNotBeNull("user");
            key.ShouldNotBeWhiteSpace("key");

            if(IsDebugEnabled)
                log.Debug(@"UserConfig 정보를 삭제합니다... productCode={0}, userCode={1}, key={2}", product.Code, user.Code, key);

            Repository<UserConfig>.DeleteAll(BuildQueryOverOfUserConfig(product.Code, user.Company.Code, user.Code, key));
        }

        /// <summary>
        /// 지정된 사용자의 모든 <see cref="UserConfig"/> 정보를 삭제합니다.
        /// </summary>
        /// <param name="user">사용자</param>
        public void DeleteAllUserConfigByUser(User user)
        {
            user.ShouldNotBeNull("user");

            if(IsDebugEnabled)
                log.Debug(@"지정된 사용자의 모든 UserConfig 정보를 삭제합니다... " + user);

            Repository<UserConfig>.DeleteAll(BuildQueryOverOfUserConfig(null, user.Company.Code, user.Code));
        }
    }
}