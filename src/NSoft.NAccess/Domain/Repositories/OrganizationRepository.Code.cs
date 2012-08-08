using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Reflections;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 코드 정보 서비스
    /// </summary>
    public partial class OrganizationRepository
    {
        /// <summary>
        /// <see cref="Code"/> 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="groupCode">코드 그룹 코드</param>
        /// <param name="itemCode">코드 아이템 코드</param>
        /// <param name="groupName">코드 그룹 명</param>
        /// <param name="itemName">코드 아이템 명</param>
        /// <param name="isActive">Is Active?</param>
        /// <returns></returns>
        public QueryOver<Code, Code> BuildQueryOverOfCode(string companyCode,
                                                          string groupCode = null,
                                                          string itemCode = null,
                                                          string groupName = null,
                                                          string itemName = null,
                                                          bool? isActive = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"Code 정보를 조회하기 위한 질의를 빌드합니다... " +
                          @"companyCode={0}, groupCode={1}, itemCode={2}, groupName={3}, itemName={4}, isActive={5}",
                          companyCode, groupCode, itemCode, groupName, itemName, isActive);

            var query = QueryOver.Of<Code>();

            if(companyCode.IsNotWhiteSpace())
                query.AddWhere(c => c.Group.CompanyCode == companyCode);

            if(groupCode.IsNotWhiteSpace())
                query.AddWhere(c => c.Group.Code == groupCode);

            if(itemCode.IsNotWhiteSpace())
                query.AddWhere(c => c.ItemCode == itemCode);

            if(groupName.IsNotWhiteSpace())
                query.AddWhere(c => c.Group.Name == groupName);

            if(itemName.IsNotWhiteSpace())
                query.AddWhere(c => c.ItemName == itemName);

            if(isActive.HasValue)
                query.AddNullAsTrue(c => c.IsActive, isActive);

            return query;
        }

        /// <summary>
        /// <see cref="Code"/> 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="groupCode">코드 그룹 코드</param>
        /// <param name="itemCode">코드 아이템 코드</param>
        /// <param name="groupName">코드 그룹 명</param>
        /// <param name="itemName">코드 아이템 명</param>
        /// <param name="isActive">Is Active?</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfCode(string companyCode,
                                                    string groupCode = null,
                                                    string itemCode = null,
                                                    string groupName = null,
                                                    string itemName = null,
                                                    bool? isActive = null)
        {
            return BuildQueryOverOfCode(companyCode, groupName, itemCode, groupName, itemName, isActive).DetachedCriteria;
        }

        /// <summary>
        /// 지정된 코드를 조회한다. 없다면, 새로 생성하여 저장한 후, 반환한다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="itemCode">아이템 코드</param>
        /// <returns></returns>
        public Code GetOrCreateCode(Company company, string groupCode, string itemCode)
        {
            company.ShouldNotBeNull("company");
            return GetOrCreateCode(company.Code, groupCode, itemCode);
        }

        /// <summary>
        /// 지정된 코드를 조회한다. 없다면, 새로 생성하여 저장한 후, 반환한다.
        /// </summary>
        /// <param name="companyCode">회사</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="itemCode">아이템 코드</param>
        /// <returns></returns>
        public Code GetOrCreateCode(string companyCode, string groupCode, string itemCode)
        {
            var code = FindOneCodeByCode(companyCode, groupCode, itemCode);

            // 코드가 존재하지 않는다면, 새로 생성한다.
            if(code == null)
            {
                if(log.IsInfoEnabled)
                    log.Info("새로운 Code 정보를 생성합니다.  companyCode={0}, groupCode={1}, itemCode={2}", companyCode, groupCode, itemCode);

                lock(_syncLock)
                {
                    using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                    {
                        UnitOfWork.CurrentSession.SaveOrUpdate(new Code(companyCode, groupCode, itemCode));
                        UnitOfWork.Current.TransactionalFlush();
                    }
                }

                code = FindOneCodeByCode(companyCode, groupCode, itemCode);
                code.AssertExists("Code");


                if(log.IsInfoEnabled)
                    log.Info("새로운 Code 정보를 생성했습니다!!! Code=" + code);
            }

            return code;
        }

        /// <summary>
        /// 코드 Identity 값으로부터 코드 정보를 조회합니다.
        /// </summary>
        /// <param name="companyCode">회사 콛ㅡ</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="itemCode">아이템 코드</param>
        /// <returns></returns>
        public Code FindOneCodeByCode(string companyCode, string groupCode, string itemCode)
        {
            companyCode.ShouldNotBeWhiteSpace("companyCode");
            groupCode.ShouldNotBeWhiteSpace("groupCode");
            itemCode.ShouldNotBeWhiteSpace("itemCode");

            if(IsDebugEnabled)
                log.Debug(@"Code 정보를 조회합니다... companyCode={0}, groupCode={1}, itemCode={2}", companyCode, groupCode, itemCode);

            return Repository<Code>.FindOne(BuildQueryOverOfCode(companyCode, groupCode, itemCode, null, null, null));
        }

        /// <summary>
        /// 코드 Identity 값으로부터 코드 정보를 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="itemCode">아이템 코드</param>
        /// <returns></returns>
        public Code FindOneCodeByCode(Company company, string groupCode, string itemCode)
        {
            company.ShouldNotBeNull("company");
            return FindOneCodeByCode(company.Code, groupCode, itemCode);
        }

        /// <summary>
        /// 지정된 회사의 코드 그룹 정보를 가져온다.
        /// </summary>
        /// <param name="companyCode">회사 코드</param>
        /// <returns></returns>
        public IList<CodeGroup> FindAllCodeGroupByCompany(string companyCode)
        {
            companyCode.ShouldNotBeWhiteSpace("companyCode");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사의 코드 그룹 정보를 가져온다... companyCode=" + companyCode);

            return
                BuildQueryOverOfCode(companyCode, null, null, null, null, null)
                    .Select(Projections.ProjectionList().Add(Projections.Group<Code>(c => c.Group)))
                    .GetExecutableQueryOver(Session)
                    .List<CodeGroup>();
        }

        /// <summary>
        /// 지정된 회사의 코드 그룹 정보를 가져온다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <returns></returns>
        public IList<CodeGroup> FindAllCodeGroupByCompany(Company company)
        {
            company.ShouldNotBeNull("company");
            return FindAllCodeGroupByCompany(company.Code);
        }

        /// <summary>
        /// 특정 회사의 모든 코드 정보를 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<Code> FindAllCodeByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<Code>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"특정 회사의 모든 코드 정보를 조회합니다... " +
                          @"companyCode={0}, firstResult={1}, maxResults={2}, orders={3}",
                          company.Code, firstResult, maxResults, orders.CollectionToString());

            var query = QueryOver.Of<Code>().AddWhere(c => c.Group.CompanyCode == company.Code).AddOrders(orders);

            return Repository<Code>.FindAll(query,
                                            firstResult.GetValueOrDefault(),
                                            maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 특정 회사의 모든 코드 정보를 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <returns></returns>
        public IList<Code> FindAllCodeByCompany(Company company)
        {
            return FindAllCodeByCompany(company, null, null);
        }

        /// <summary>
        /// 지정한 코드 그룹의 코드 정보들을 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<Code> FindAllCodeByGroup(Company company, string groupCode, int? firstResult, int? maxResults, params INHOrder<Code>[] orders)
        {
            company.ShouldNotBeNull("company");
            groupCode.ShouldNotBeWhiteSpace("groupCode");

            if(IsDebugEnabled)
                log.Debug(@"지정한 코드 그룹의 코드 정보들을 조회합니다... " +
                          @"companyCode={0}, groupCode={1}, firstResult={2}, maxResults={3}, orders={4}",
                          company.Code, groupCode, firstResult, maxResults, orders.CollectionToString());

            var query = BuildQueryOverOfCode(company.Code, groupCode, null, null, null, null).AddOrders(orders);

            return Repository<Code>.FindAll(query,
                                            firstResult.GetValueOrDefault(),
                                            maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정한 코드 그룹의 코드 정보들을 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <returns></returns>
        public IList<Code> FindAllCodeByGroup(Company company, string groupCode)
        {
            return FindAllCodeByGroup(company, groupCode, null, null);
        }

        /// <summary>
        /// 지정한 코드 그룹명에 매칭되는 모든 코드 정보를 조회합니다.
        /// </summary>
        /// <param name="companyCode">회사</param>
        /// <param name="groupNameToMatch">검색할 코드 그룹명</param>
        /// <param name="matchMode">매칭 모드</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<Code> FindAllCodeByGroupNameToMatch(string companyCode,
                                                         string groupNameToMatch,
                                                         MatchMode matchMode,
                                                         int? firstResult,
                                                         int? maxResults,
                                                         params INHOrder<Code>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"지정된 아이템 이름과 매칭되는 모든 코드 정보를 조회합니다... " +
                          @"companyCode={0}, groupNameToMatch={1}, matchMode={2}, firstResult={3}, maxResults={4}, orders={5}",
                          companyCode, groupNameToMatch, matchMode, firstResult, maxResults, orders.CollectionToString());

            var query = QueryOver.Of<Code>();

            if(companyCode.IsNotWhiteSpace())
                query.AddWhere(c => c.Group.CompanyCode == companyCode);

            if(groupNameToMatch.IsNotWhiteSpace())
                query.AddInsensitiveLike(c => c.Group.Name, groupNameToMatch, matchMode ?? MatchMode.Anywhere);

            return Repository<Code>.FindAll(query.AddOrders(orders),
                                            firstResult.GetValueOrDefault(),
                                            maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정된 아이템 이름과 매칭되는 모든 코드 정보를 조회합니다.
        /// </summary>
        /// <param name="companyCode">회사</param>
        /// <param name="itemNameToMatch">검색할 코드 아이템 명</param>
        /// <param name="matchMode">매칭 모드</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<Code> FindAllCodeByItemNameToMatch(string companyCode,
                                                        string itemNameToMatch,
                                                        MatchMode matchMode,
                                                        int? firstResult,
                                                        int? maxResults,
                                                        params INHOrder<Code>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"지정된 아이템 이름과 매칭되는 모든 코드 정보를 조회합니다... " +
                          @"company={0}, itemNameToMatch={1}, matchMode={2}, firstResult={3}, maxResults={4}, orders={5}",
                          companyCode, itemNameToMatch, matchMode, firstResult, maxResults, orders);

            var query = QueryOver.Of<Code>();

            if(companyCode.IsNotWhiteSpace())
                query.AddWhere(c => c.Group.CompanyCode == companyCode);

            if(itemNameToMatch.IsNotWhiteSpace())
                query.AddInsensitiveLike(c => c.ItemName, itemNameToMatch, matchMode ?? MatchMode.Anywhere);

            return Repository<Code>.FindAll(query.AddOrders(orders),
                                            firstResult.GetValueOrDefault(),
                                            maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정한 코드 정보를 삭제합니다.
        /// </summary>
        /// <param name="code">삭제할 코드</param>
        public void DeleteCode(Code code)
        {
            if(code == null)
                return;

            if(log.IsInfoEnabled)
                log.Info(@"지정된 코드 정보를 삭제합니다... Code=" + code);

            DeleteEntityTransactional(code);
        }

        /// <summary>
        /// 지정된 코드 그룹의 모든 코드정보를 삭제합니다.
        /// </summary>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="groupCode">그룹 코드</param>
        public void DeleteAllCodeByGroup(string companyCode, string groupCode)
        {
            if(IsDebugEnabled)
                log.Debug("지정한 코드 그룹 삭제를 시작합니다... companyCode={0}, groupCode={1}", companyCode, groupCode);

            Repository<Code>.DeleteAll(BuildQueryOverOfCode(companyCode, groupCode, null, null, null, null));

            //var tx = UnitOfWork.Current.BeginTransaction();
            //try
            //{
            //    Repository<Code>.DeleteAll(DetachedCriteria.For<Code>()
            //                                .AddEq("Id.CompanyCode", company)
            //                                .AddEq("Id.GroupCode", groupCode));

            //    tx.Commit();

            //    if (IsInfoEnabled)
            //        log.Info("지정한 코드 그룹 삭제에 성공했습니다!!! companyCode={0}, groupCode={1}", company.Code, groupCode);
            //}
            //catch (Exception ex)
            //{
            //    if (IsErrorEnabled)
            //        log.ErrorException("코드 정보를 삭제하는데 실패했습니다. groupCode=" + groupCode, ex);

            //    if (tx != null)
            //        tx.Rollback();

            //    throw;
            //}
        }
    }
}