using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    public partial class OrganizationRepository
    {
        /// <summary>
        /// 그룹 정보 조회를 위한 <see cref="QueryOver"/>를 빌드합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="code"></param>
        /// <param name="kind"></param>
        /// <param name="name"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static QueryOver<Group, Group> BuildQueryOverOfGroup(Company company,
                                                                    string code = null,
                                                                    GroupKinds? kind = null,
                                                                    string name = null,
                                                                    bool? isActive = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"그룹 정보 조회를 위한 QueryOver 를 빌드합니다... company={0}, code={1} kind={2}, name={3}, isActive={4}",
                          company, code, kind, name, isActive);

            var query = QueryOver.Of<Group>();

            if(company != null)
                query.AddWhere(g => g.Company == company);

            if(code.IsNotWhiteSpace())
                query.AddWhere(g => g.Code == code);

            if(kind.HasValue)
                query.AddWhere(g => g.Kind == kind.Value);

            if(name.IsNotWhiteSpace())
                query.AddWhere(g => g.Name == name);

            if(isActive.HasValue)
                query.AddNullAsTrue(g => g.IsActive, isActive);

            return query;
        }

        /// <summary>
        /// 그룹 정보 조회를 위한 <see cref="DetachedCriteria"/>를 빌드합니다.
        /// </summary>
        public static DetachedCriteria BuildCriteriaOfGroup(Company company,
                                                            string code = null,
                                                            GroupKinds? kind = null,
                                                            string name = null,
                                                            bool? isActive = null)
        {
            return BuildQueryOverOfGroup(company, code, kind, name, isActive).DetachedCriteria;
        }

        /// <summary>
        /// 지정된 회사의 <paramref name="code"/>를 가지는 그룹 정보를 로드한다. 만약 없다면, 새로 생성해서 반환한다.
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="code">그룹 Id</param>
        /// <returns></returns>
        public Group GetOrCreateGroup(Company company, string code)
        {
            company.ShouldNotBeNull("comapny");

            if(IsDebugEnabled)
                log.Debug(@"그룹 정보를 조회합니다... companyCode={0}, code={1}", company.Code, code);

            var group = FindOneGroupByCode(company, code);

            if(group == null)
            {
                if(IsDebugEnabled)
                    log.Debug(@"새로운 Group 정보를 생성합니다... company={0}, code={1}", company.Id, code);

                lock(_syncLock)
                    using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                    {
                        UnitOfWork.CurrentSession.Save(new Group(company, code));
                        UnitOfWork.Current.TransactionalFlush();
                    }

                group = FindOneGroupByCode(company, code);
                group.AssertExists("group");

                if(log.IsInfoEnabled)
                    log.Info("새로운 Group 정보를 생성했습니다!!! " + group);
            }

            return group;
        }

        /// <summary>
        /// 지정된 회사의 <paramref name="code"/>를 가지는 그룹 정보를 로드한다.
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="code">그룹 Id</param>
        /// <returns></returns>
        public Group FindOneGroupByCode(Company company, string code)
        {
            if(IsDebugEnabled)
                log.Debug(@"Group Identity 정보로 그룹 정보를 조회합니다... company={0}, code={1}", company, code);

            return Repository<Group>.FindOne(BuildQueryOverOfGroup(company, code, null, null, null));
        }

        /// <summary>
        /// 특정 회사의 모든 Group 정보를 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <returns></returns>
        public IList<Group> FindAllGroupByCompany(Company company)
        {
            return FindAllGroupByCompany(company, null, null);
        }

        /// <summary>
        /// 특정 회사의 모든 Group 정보를 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<Group> FindAllGroupByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<Group>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"특정 회사의 모든 Group 정보를 조회합니다... company={0}, firstResult={1}, maxResults={2}, orders={3}",
                          company, firstResult, maxResults, orders);

            var query = QueryOver.Of<Group>().AddWhere(g => g.Company == company).AddOrders(orders);

            return Repository<Group>.FindAll(query,
                                             firstResult.GetValueOrDefault(),
                                             maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 특정 회사의 Group 정보를 Paging 조회합니다
        /// </summary>
        public IPagingList<Group> GetPageOfGroupByCompany(Company company, int pageIndex, int pageSize, params INHOrder<Group>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"특정 회사의 Group 정보를 Paging 조회합니다... company={0}, pageIndex={1}, pageSize={2}, orders={3}",
                          company, pageIndex, pageSize, orders);

            var query = QueryOver.Of<Group>().AddWhere(g => g.Company == company);

            return Repository<Group>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// 그룹-구성원(GroupActor) 정보를 조회하기 위한 QueryOver를 빌드합니다
        /// </summary>
        /// <param name="companyCode">회사코드</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="actorCode">소속원 코드</param>
        /// <param name="actorKind">소속원 종류 (회사|부서|사용자 등)</param>		
        public static QueryOver<GroupActor, GroupActor> BuildQueryOverOfGroupActor(string companyCode, string groupCode, string actorCode, ActorKinds? actorKind)
        {
            if(IsDebugEnabled)
                log.Debug(@"그룹-구성원(GroupActor) 정보를 조회하기 위한 QueryOver를 빌드합니다... companyCode={0}, groupCode={1}, actorCode={2}, actorKind={3}",
                          companyCode, groupCode, actorCode, actorKind);

            var query = QueryOver.Of<GroupActor>();

            if(companyCode.IsNotWhiteSpace())
                query.AddWhere(ga => ga.Id.CompanyCode == companyCode);

            if(groupCode.IsNotWhiteSpace())
                query.AddWhere(ga => ga.Id.GroupCode == groupCode);

            if(actorCode.IsNotWhiteSpace())
                query.AddWhere(ga => ga.Id.ActorCode == actorCode);

            if(actorKind.HasValue)
                query.AddWhere(ga => ga.Id.ActorKind == actorKind);

            return query;
        }

        /// <summary>
        /// 그룹-구성원(GroupActor) 정보를 조회하기 위한 QueryOver를 빌드합니다
        /// </summary>
        /// <param name="group">Group</param>
        /// <param name="actorCode">소속원 코드</param>
        /// <param name="actorKind">소속원 종류 (회사|부서|사용자 등)</param>		
        public static QueryOver<GroupActor, GroupActor> BuildQueryOverOfGroupActor(Group group, string actorCode, ActorKinds? actorKind)
        {
            if(IsDebugEnabled)
                log.Debug(@"그룹-구성원(GroupActor) 정보를 조회하기 위한 QueryOver를 빌드합니다... group={0}, actorCode={1}, actorKind={2}",
                          group, actorCode, actorKind);

            var query = QueryOver.Of<GroupActor>();

            if(group != null)
            {
                if(group.Company != null)
                    query.AddWhere(ga => ga.Id.CompanyCode == group.Company.Code);

                query.AddWhere(ga => ga.Id.GroupCode == group.Code);
            }

            if(actorCode.IsNotWhiteSpace())
                query.AddWhere(ga => ga.Id.ActorCode == actorCode);

            if(actorKind.HasValue)
                query.AddWhere(ga => ga.Id.ActorKind == actorKind);

            return query;
        }

        /// <summary>
        /// 그룹구성원 정보를 조회하기 위한 Criteria를 빌드합니다. (null 이거나 empty string인 경우에는 검색 조건에서 제외됩니다.)
        /// </summary>
        /// <param name="companyCode">회사코드</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="actorCode">소속원 코드</param>
        /// <param name="actorKind">소속원 종류 (회사|부서|사용자 등)</param>
        /// <returns></returns>
        public static DetachedCriteria BuildCriteriaOfGroupActor(string companyCode, string groupCode, string actorCode, ActorKinds? actorKind)
        {
            return BuildQueryOverOfGroupActor(companyCode, groupCode, actorCode, actorKind).DetachedCriteria;
        }

        /// <summary>
        /// 그룹구성원 정보를 조회하기 위한 Criteria를 빌드합니다. (null 이거나 empty string인 경우에는 검색 조건에서 제외됩니다.)
        /// </summary>
        /// <param name="group">그룹</param>
        /// <param name="actorCode">소속원 코드</param>
        /// <param name="actorKind">소속원 종류 (회사|부서|사용자 등)</param>
        /// <returns></returns>
        public static DetachedCriteria BuildCriteriaOfGroupActor(Group group, string actorCode, ActorKinds? actorKind)
        {
            return BuildQueryOverOfGroupActor(group, actorCode, actorKind).DetachedCriteria;
        }

        /// <summary>
        /// GroupActor를 조회합니다. 없으면 새로 생성해서 반환합니다.
        /// </summary>
        public GroupActor GetOrCreateGroupActor(Group group, string actorCode, ActorKinds actorKind)
        {
            group.ShouldNotBeNull("group");

            var groupActor = FindOneGroupActor(group, actorCode, actorKind);

            if(groupActor != null)
                return groupActor;

            lock(_syncLock)
                new GroupActor(group, actorCode, actorKind).InsertStateless();

            groupActor = FindOneGroupActor(group, actorCode, actorKind);
            groupActor.AssertExists("groupActor");

            return groupActor;
        }

        /// <summary>
        /// 새로운 그룹을 만듭니다.
        /// </summary>
        /// <param name="group">그룹 정보</param>
        /// <param name="actorCode">그룹 소속원 코드 (Actor 코드 : 회사|부서|그룹|사용자 코드)</param>
        /// <param name="actorKind">그룹 소속원 종류 (사용자|부서| 타 그룹)</param>
        /// <returns></returns>
        public GroupActor CreateGroupActor(Group group, string actorCode, ActorKinds actorKind)
        {
            group.ShouldNotBeNull("group");
            actorCode.ShouldNotBeWhiteSpace("actorCode");

            if(IsDebugEnabled)
                log.Debug(@"새로운 그룹-구성원 정보를 생성합니다... group={0}, actorCode={1}, actorKind={2}", group, actorCode, actorKind);

            return Repository<GroupActor>.SaveOrUpdate(new GroupActor(group, actorCode, actorKind));
        }

        /// <summary>
        /// <see cref="GroupActor"/>를 조회합니다.
        /// </summary>
        public GroupActor FindOneGroupActor(Group group, string actorCode, ActorKinds actorKind)
        {
            return Repository<GroupActor>.FindOne(BuildQueryOverOfGroupActor(group, actorCode, actorKind));
        }

        /// <summary>
        /// 지정된 그룹-구성원(GroupActor) 정보를 구합니다.
        /// </summary>
        /// <param name="group">그룹 정보</param>
        /// <returns></returns>
        public IList<GroupActor> FindAllGroupActorByGroup(Group group)
        {
            group.ShouldNotBeNull("group");

            if(IsDebugEnabled)
                log.Debug("지정된 그룹-구성원(GroupActor) 정보를 조회합니다... CompanyCode={0}, GroupCode={1}", group.Company.Code, group.Code);

            return Repository<GroupActor>.FindAll(BuildQueryOverOfGroupActor(group, null, null));
        }

        /// <summary>
        /// 지정된 회사의 모든 그룹-구성원(GroupActor) 정보를 조회합니다.
        /// </summary>
        /// <param name="company">그룹이 소속된 회사</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<GroupActor> FindAllGroupActorByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<GroupActor>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(log.IsDebugEnabled)
                log.Debug(@"지정한 회사에 속한 그룹-구성원(GroupActor) 정보를 조회합니다... company={0}, firstResult={1}, maxResults={2}, orders={3}",
                          company, firstResult, maxResults, orders);

            return Repository<GroupActor>.FindAll(BuildQueryOverOfGroupActor(company.Code, null, null, null).AddOrders(orders),
                                                  firstResult.GetValueOrDefault(),
                                                  maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정된 회사의 모든 그룹-구성원(GroupActor) 정보를 조회합니다.
        /// </summary>
        /// <param name="company">그룹이 소속된 회사</param>
        /// <returns></returns>
        public IList<GroupActor> FindAllGroupActorByCompany(Company company)
        {
            return FindAllGroupActorByCompany(company, null, null);
        }

        /// <summary>
        /// 지정한 구성원이 속한 그룹구성원 정보를 조회합니다.
        /// </summary>
        /// <param name="company">그룹이 소속된 회사</param>
        /// <param name="actorCode">그룹 소속원 코드 (Actor 코드 : 회사|부서|그룹|사용자 코드)</param>
        /// <param name="actorKind">그룹 소속원 종류 (사용자|부서| 타 그룹)</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<GroupActor> FindAllGroupActorByActor(Company company, string actorCode, ActorKinds? actorKind,
                                                          int? firstResult, int? maxResults, params INHOrder<GroupActor>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(log.IsDebugEnabled)
                log.Debug(@"지정한 구성원이 속한 그룹-구성원 정보를 조회합니다... " +
                          @"actorCode={0}, actorKind={1}, company={2}, firstResult={3}, maxResults={4}, orders={5}",
                          actorCode, actorKind, company, firstResult, maxResults, orders);

            var query = BuildQueryOverOfGroupActor(company.Code, null, actorCode, actorKind).AddOrders(orders);

            return Repository<GroupActor>.FindAll(query,
                                                  firstResult.GetValueOrDefault(),
                                                  maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정한 회사에 속한 그룹-구성원(GroupActor) 정보를 Paging 처리해서 로드합니다
        /// </summary>
        /// <param name="company">검색할 회사</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<GroupActor> GetPageOfGroupActorByCompany(Company company, int pageIndex, int pageSize, params INHOrder<GroupActor>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(log.IsDebugEnabled)
                log.Debug(@"지정한 회사에 속한 그룹-구성원(GroupActor) 정보를 Paging 처리해서 로드합니다... " +
                          @"company={0}, pageIndex={1}, pageSize={2}, orders={3}",
                          company, pageIndex, pageSize, orders);

            return Repository<GroupActor>.GetPage(pageIndex,
                                                  pageSize,
                                                  BuildQueryOverOfGroupActor(company.Code, null, null, null),
                                                  orders);
        }

        /// <summary>
        /// 지정된 그룹에 지정한 actor가 구성원으로 등록되어 있는지 검사한다.
        ///	NOTE: 현재로서는 직접적인 매핑정보만을 수집한다. 즉 그룹에 부서가 구성원이고, 그 부서의 구성원으로서의 직원은 검출하지 못한다.
        /// </summary>
        /// <param name="group">그룹 정보</param>
        /// <param name="actorCode">그룹 소속원 코드 (Actor 코드 : 회사|부서|그룹|사용자 코드)</param>
        /// <param name="actorKind">그룹 소속원 종류 (사용자|부서| 타 그룹)</param>
        /// <returns></returns>
        public bool IsGroupMember(Group group, string actorCode, ActorKinds actorKind)
        {
            group.ShouldNotBeNull("group");

            if(IsDebugEnabled)
                log.Debug(@"그룹에 소속된 구성원인지 검사합니다... groupCode={0}, actorCode={1}, actorKind={2}", group.Code, actorCode, actorKind);

            var isMember = Repository<GroupActor>.Exists(BuildQueryOverOfGroupActor(group, actorCode, actorKind));

            if(IsDebugEnabled)
                log.Debug(@"그룹에 소속된 구성원인가요? IsGroupMember=" + isMember);

            return isMember;
        }
    }
}