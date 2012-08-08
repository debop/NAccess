using System;
using System.Collections.Generic;
using System.Globalization;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Reflections;
using NSoft.NFramework.TimePeriods;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 사용자 로그인 이력 
    /// </summary>
    public partial class LoggingRepository
    {
        /// <summary>
        /// 사용자 로그인 이력을 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="loginId">로그인 ID (사용자 코드가 아니다)</param>
        /// <param name="localeKey">지역화 정보 (<see cref="CultureInfo.Name"/>)</param>
        /// <param name="loginTimePeriod">로그인 시간의 검색 범위</param>
        /// <returns></returns>
        public QueryOver<UserLoginLog, UserLoginLog> BuildQueryOverOfUserLoginLog(string productCode,
                                                                                  string companyCode = null,
                                                                                  string loginId = null,
                                                                                  string localeKey = null,
                                                                                  ITimePeriod loginTimePeriod = null)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"사용자 로그인 이력을 조회하기 위한 Criteria를 빌드합니다... " +
                          @"productCode={0}, companyCode={1}, loginId={2}, localeKey={3}, loginTimePeriod={4}",
                          productCode, companyCode, loginId, localeKey, loginTimePeriod);

            var query = QueryOver.Of<UserLoginLog>();

            if(productCode.IsNotWhiteSpace())
                query.AddWhere(ulog => ulog.ProductCode == productCode);

            if(companyCode.IsNotWhiteSpace())
                query.AddWhere(ulog => ulog.CompanyCode == companyCode);

            if(loginId.IsNotWhiteSpace())
                query.AddWhere(ulog => ulog.LoginId == loginId);

            if(localeKey.IsNotWhiteSpace())
                query.AddWhere(ulog => ulog.LocaleKey == localeKey);

            if(loginTimePeriod != null && loginTimePeriod.IsAnytime == false)
                query.AddBetween(ulog => ulog.LoginTime, loginTimePeriod.StartAsNullable, loginTimePeriod.EndAsNullable);

            return query;
        }

        /// <summary>
        /// 사용자 로그인 이력을 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="loginId">로그인 ID (사용자 코드가 아니다)</param>
        /// <param name="localeKey">지역화 정보 (<see cref="CultureInfo.Name"/>)</param>
        /// <param name="loginTimePeriod">로그인 시간의 검색 범위</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfUserLoginLog(string productCode,
                                                            string companyCode = null,
                                                            string loginId = null,
                                                            string localeKey = null,
                                                            ITimePeriod loginTimePeriod = null)
        {
            return BuildQueryOverOfUserLoginLog(productCode, companyCode, loginId, localeKey, loginTimePeriod).DetachedCriteria;
        }

        /// <summary>
        /// 사용자 로그인 이력 정보를 조회합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="loginId">로그인 ID (사용자 코드가 아니다)</param>
        /// <param name="localeKey">지역화 정보 (<see cref="CultureInfo.Name"/>)</param>
        /// <param name="loginTimePeriod">로그인 시간의 검색 범위</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서 (보통 LoginTime DESC 이다)</param>	
        /// 
        /// <returns></returns>
        public IList<UserLoginLog> FindAllUserLoginLog(string productCode,
                                                       string companyCode,
                                                       string loginId,
                                                       string localeKey,
                                                       ITimePeriod loginTimePeriod,
                                                       int? firstResult,
                                                       int? maxResults,
                                                       params INHOrder<UserLoginLog>[] orders)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"사용자 로그인 이력 정보를 조회합니다... " +
                          @"productCode={0}, companyCode={1}, loginId={2}, localeKey={3}, loginTimePeriod={4}, firstResult={5}, maxResults={6}, orders={7}",
                          productCode, companyCode, loginId, localeKey, loginTimePeriod, firstResult, maxResults, orders.CollectionToString());

            var query = BuildQueryOverOfUserLoginLog(productCode,
                                                     companyCode,
                                                     loginId,
                                                     localeKey,
                                                     loginTimePeriod);

            return Repository<UserLoginLog>.FindAll(query.AddOrders(orders),
                                                    firstResult.GetValueOrDefault(),
                                                    maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 사용자 로그인 이력 정보를 Paging 조회를 수행합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="loginId">로그인 ID (사용자 코드가 아니다)</param>
        /// <param name="localeKey">지역화 정보 (<see cref="CultureInfo.Name"/>)</param>
        /// <param name="loginTimePeriod">로그인 시간의 검색 범위</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다)</param>
        /// <param name="pageSize">Page Size (한 페이지의 요소의 수. 보통 10개)</param>
        /// <param name="orders">정렬 순서 (보통 LoginTime DESC 이다)</param>		
        /// <returns></returns>
        public IPagingList<UserLoginLog> GetPageOfUserLoginLog(string productCode,
                                                               string companyCode,
                                                               string loginId,
                                                               string localeKey,
                                                               ITimePeriod loginTimePeriod,
                                                               int pageIndex,
                                                               int pageSize,
                                                               params INHOrder<UserLoginLog>[] orders)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"사용자 로그인 이력 정보를 Paging 조회를 수행합니다... " +
                          @"productCode={0}, companyCode={1}, loginId={2}, localeKey={3}, loginTimePeriod={4}, pageIndex={5}, pageSize={6}, orders={7}",
                          productCode, companyCode, loginId, localeKey, loginTimePeriod, pageIndex, pageSize, orders.CollectionToString());

            var query = BuildQueryOverOfUserLoginLog(productCode, companyCode, loginId, localeKey, loginTimePeriod);

            return Repository<UserLoginLog>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// 사용자 로그인 이력 정보를 Paging 조회를 수행합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다)</param>
        /// <param name="pageSize">Page Size (한 페이지의 요소의 수. 보통 10개)</param>
        /// <param name="orders">정렬 순서 (보통 LoginTime DESC 이다)</param>
        /// <returns></returns>
        public IPagingList<UserLoginLog> GetPageOfUserLoginLog(string productCode,
                                                               string companyCode,
                                                               int pageIndex,
                                                               int pageSize,
                                                               params INHOrder<UserLoginLog>[] orders)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"사용자 로그인 이력 정보를 Paging 조회를 수행합니다..." +
                          @"productCode={0}, companyCode={1}, pageIndex={2}, pageSize={3}, orders={4}",
                          productCode, companyCode, pageIndex, pageSize, orders);

            return GetPageOfUserLoginLog(productCode, companyCode, null, null, null, pageIndex, pageSize, orders);
        }

        /// <summary>
        /// 사용자 Login 이력을 DB에 기록합니다. (Sessionless를 사용하므로 함수명이 Insert 로 시작합니다)
        /// </summary>
        /// <param name="product">로그인한 제품 정보</param>
        /// <param name="user">로그인 사용자</param>
        /// <param name="department">로그인 사용자의 부서</param>
        /// <param name="localeKey">로그인 사용자의 Locale 정보</param>
        /// <param name="loginTime">로그인 시각</param>
        /// <param name="exAttr">확장 속성 정보</param>
        public void InsertUserLoginLog(Product product, User user, Department department = null, string localeKey = null, DateTime? loginTime = null, string exAttr = null)
        {
            product.ShouldNotBeNull("product");
            user.ShouldNotBeNull("user");

            var loginLog = new UserLoginLog(product.Code, user.Company.Code, user.LoginId, localeKey, loginTime)
                           {
                               ProductName = product.Name,
                               CompanyName = user.Company.Name,
                               DepartmentCode = (department != null) ? department.Code : null,
                               DepartmentName = (department != null) ? department.Name : null,
                               UserCode = user.Code,
                               UserName = user.Name,
                               ExAttr = exAttr
                           };

            if(log.IsDebugEnabled)
                log.Debug(@"사용자 로그인 이력 정보를 생성합니다... " + loginLog);

            // NOTE : Log 정보를 기록할 때에는 Session에 저장할 필요없고, 성능을 위해서 StatelessSession를 사용합니다.
            //
            loginLog.InsertStateless(Session);
        }

        /// <summary>
        /// 사용자 Login 이력을 DB에 기록합니다. (Sessionless를 사용하므로 함수명이 Insert 로 시작합니다)
        /// </summary>
        /// <param name="product">로그인한 제품 정보</param>
        /// <param name="user">로그인 사용자</param>
        /// <param name="localeKey">로그인 사용자의 Locale 정보</param>
        /// <param name="loginTime">로그인 시각</param>
        /// <param name="exAttr">확장 속성 정보</param>
        public void InsertUserLoginLog(Product product, User user, string localeKey = null, DateTime? loginTime = null, string exAttr = null)
        {
            InsertUserLoginLog(product, user, null, localeKey, loginTime, exAttr);
        }

        /// <summary>
        /// 사용자 Login 이력을 DB에 기록합니다. (Sessionless를 사용하므로 함수명이 Insert 로 시작합니다)
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">사용자 소속 회사 코드</param>
        /// <param name="loginId">로그인 Id</param>
        /// <param name="localeKey">로그인 사용자의 Locale 정보</param>
        /// <param name="loginTime">로그인 시각</param>
        public void InsertUserLoginLog(string productCode, string companyCode, string loginId, string localeKey = null, DateTime? loginTime = null)
        {
            var loginLog = new UserLoginLog(productCode, companyCode, loginId, localeKey, loginTime);

            if(log.IsDebugEnabled)
                log.Debug(@"사용자 로그인 이력 정보를 생성합니다... " + loginLog);

            // NOTE : Log 정보를 기록할 때에는 Session에 저장할 필요없고, 성능을 위해서 StatelessSession를 사용합니다.
            //
            loginLog.InsertStateless(Session);
        }
    }
}