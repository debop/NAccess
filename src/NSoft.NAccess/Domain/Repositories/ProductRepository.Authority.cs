using System;
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
    /// 권한 관련 Domain Service
    /// </summary>
    public partial class ProductRepository
    {
        /// <summary>
        /// 권한관련 정보를 가진 <see cref="ResourceActor"/>정보를 조회하기 위한 QueryOver 를 빌드합니다.
        /// </summary>
        /// <param name="resource">리소스 종류</param>
        /// <param name="resourceInstanceId">대상 리소스 ID</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 종류</param>
        /// <param name="authorityKind">권한 종류</param>
        /// <returns>접근 권한을 나타내는 <see cref="ResourceActor"/>를 조회하기 위한 QueryOver</returns>
        public QueryOver<ResourceActor, ResourceActor> BuildQueryOverOfResourceActor(Resource resource,
                                                                                     string resourceInstanceId,
                                                                                     string companyCode = null,
                                                                                     string actorCode = null,
                                                                                     ActorKinds? actorKind = null,
                                                                                     AuthorityKinds? authorityKind = null)
        {
            if(resource == null)
                return BuildQueryOverOfResourceActor(null, null, resourceInstanceId,
                                                     companyCode, actorCode, actorKind,
                                                     authorityKind);

            if(resource.ProductCode == null)
                return BuildQueryOverOfResourceActor(null, resource.Code, resourceInstanceId,
                                                     companyCode, actorCode, actorKind,
                                                     authorityKind);

            return BuildQueryOverOfResourceActor(resource.ProductCode, resource.Code, resourceInstanceId,
                                                 companyCode, actorCode, actorKind,
                                                 authorityKind);
        }

        /// <summary>
        /// 권한관련 정보를 가진 <see cref="ResourceActor"/>정보를 조회하기 위한 QueryOver 를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="resourceCode">리소스 종류 코드</param>
        /// <param name="resourceInstanceId">대상 리소스 ID</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 종류</param>
        /// <param name="authorityKind">권한 종류</param>
        /// <returns>접근 권한을 나타내는 <see cref="ResourceActor"/>를 조회하기 위한 QueryOver</returns>
        public QueryOver<ResourceActor, ResourceActor> BuildQueryOverOfResourceActor(string productCode,
                                                                                     string resourceCode,
                                                                                     string resourceInstanceId,
                                                                                     string companyCode,
                                                                                     string actorCode,
                                                                                     ActorKinds? actorKind,
                                                                                     AuthorityKinds? authorityKind)
        {
            if(IsDebugEnabled)
                log.Debug(@"권한관련 정보를 가진 ResourceActor 정보를 조회하기 위한 QueryOver를 빌드합니다..." + Environment.NewLine +
                          @"productCode={0}, resourceCode={1}, resourceInstanceId={2}, companyCode={3}, actorCode={4}, actorKind={5}, authorityKind={6}",
                          productCode, resourceCode, resourceInstanceId, companyCode, actorCode, actorKind, authorityKind);

            var query = QueryOver.Of<ResourceActor>();

            if(productCode.IsNotWhiteSpace())
                query.AddWhere(ra => ra.Id.ProductCode == productCode);

            if(resourceCode.IsNotWhiteSpace())
                query.AddWhere(ra => ra.Id.ResourceCode == resourceCode);

            if(resourceInstanceId.IsNotWhiteSpace())
                query.AddWhere(ra => ra.Id.ResourceInstanceId == resourceInstanceId);

            if(companyCode.IsNotWhiteSpace())
                query.AddWhere(ra => ra.Id.CompanyCode == companyCode);

            if(actorCode.IsNotWhiteSpace())
                query.AddWhere(ra => ra.Id.ActorCode == actorCode);

            if(actorKind.HasValue)
                query.AddWhere(ra => ra.Id.ActorKind == actorKind.Value);

            if(authorityKind.HasValue)
                query.AddWhere(ra => ra.AuthorityKind == authorityKind.Value);

            return query;
        }

        /// <summary>
        /// 권한관련 정보를 가진 <see cref="ResourceActor"/>정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="resource">리소스 종류</param>
        /// <param name="resourceInstanceId">대상 리소스 ID</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 종류</param>
        /// <param name="authorityKind">권한 종류</param>
        /// <returns>접근 권한을 나타내는 <see cref="ResourceActor"/>를 조회하기 위한 Criteria</returns>
        public DetachedCriteria BuildCriteriaOfResourceActor(Resource resource,
                                                             string resourceInstanceId,
                                                             string companyCode,
                                                             string actorCode,
                                                             ActorKinds? actorKind,
                                                             AuthorityKinds? authorityKind)
        {
            return BuildQueryOverOfResourceActor(resource, resourceInstanceId, companyCode, actorCode, actorKind, authorityKind).DetachedCriteria;
        }

        /// <summary>
        /// 권한관련 정보를 가진 <see cref="ResourceActor"/>정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="resourceCode">리소스 종류 코드</param>
        /// <param name="resourceInstanceId">대상 리소스 ID</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 종류</param>
        /// <param name="authorityKind">권한 종류</param>
        /// <returns>접근 권한을 나타내는 <see cref="ResourceActor"/>를 조회하기 위한 Criteria</returns>
        public DetachedCriteria BuildCriteriaOfResourceActor(string productCode,
                                                             string resourceCode,
                                                             string resourceInstanceId,
                                                             string companyCode,
                                                             string actorCode,
                                                             ActorKinds? actorKind,
                                                             AuthorityKinds? authorityKind)
        {
            return BuildQueryOverOfResourceActor(productCode, resourceCode, resourceInstanceId, companyCode, actorCode, actorKind, authorityKind).DetachedCriteria;
        }

        /// <summary>
        /// 새로운 리소스에 대한 접근 권한 정보를 생성합니다.
        /// </summary>
        /// <param name="resource">접근 대상 리소스 종류</param>
        /// <param name="resourceInstanceId">접근 대상 리소스 Id</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 (부서|사용자|그룹 등) 종류</param>
        /// <param name="authorityKind">접근 권한 종류</param>
        /// <returns>새로 생성한 리소스 접근 권한 정보</returns>
        public ResourceActor CreateResourceActor(Resource resource,
                                                 string resourceInstanceId,
                                                 string companyCode,
                                                 string actorCode,
                                                 ActorKinds actorKind,
                                                 AuthorityKinds authorityKind)
        {
            resource.ShouldNotBeNull("resource");
            resourceInstanceId.ShouldNotBeWhiteSpace("resourceInstanceId");
            companyCode.ShouldNotBeWhiteSpace("companyCode");

            if(IsDebugEnabled)
                log.Debug(@"새로운 리소스 접근 권한 정보를 생성합니다... " +
                          @"resource={0}, resourceInstanceId={1}, companyCode={2}, actorCode={3}, actorKind={4}, authorityKind={5}",
                          resource, resourceInstanceId, companyCode, actorCode, actorKind, authorityKind);

            var resourceActor = new ResourceActor(resource, resourceInstanceId, companyCode, actorCode, actorKind, authorityKind);
            return Repository<ResourceActor>.SaveOrUpdate(resourceActor);
        }

        /// <summary>
        /// 새로운 리소스에 대한 접근 권한 정보를 추가합니다. (NOTE: StatelessSession을 사용하여 추가하므로 성능이 빠릅니다.)
        /// </summary>
        /// <param name="resource">접근 대상 리소스 종류</param>
        /// <param name="resourceInstanceId">접근 대상 리소스 Id</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 (부서|사용자|그룹 등) 종류</param>
        /// <param name="authorityKind">접근 권한 종류</param>
        /// <returns>새로 생성한 리소스 접근 권한 정보</returns>
        public void InsertResourceActor(Resource resource,
                                        string resourceInstanceId,
                                        string companyCode,
                                        string actorCode,
                                        ActorKinds actorKind,
                                        AuthorityKinds authorityKind)
        {
            resource.ShouldNotBeNull("resource");
            resourceInstanceId.ShouldNotBeWhiteSpace("resourceInstanceId");
            companyCode.ShouldNotBeWhiteSpace("companyCode");

            if(IsDebugEnabled)
                log.Debug(@"새로운 리소스 접근 권한 정보를 IStatelessSession을 사용하여 추가합니다... " +
                          @"resource={0}, resourceInstanceId={1}, companyCode={2}, actorCode={3}, actorKind={4}, authorityKind={5}",
                          resource, resourceInstanceId, companyCode, actorCode, actorKind, authorityKind);

            var resourceActor = new ResourceActor(resource, resourceInstanceId, companyCode, actorCode, actorKind, authorityKind);

            //Guard.Assert(SessionFactory.IsSQLite() == false, "StatelessSession는 SQLite 메모리 DB는 지원하지 않습니다.");
            resourceActor.InsertStateless();
        }

        /// <summary>
        /// 리소스 정보를 기준으로 모든 접근 권한 정보를 조회합니다.
        /// </summary>
        /// <param name="resource">접근 대상 리소스 종류</param>
        /// <param name="resourceInstanceId">접근 대상 리소스 Id</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<ResourceActor> FindAllResourceActorByResource(Resource resource,
                                                                   string resourceInstanceId,
                                                                   string companyCode,
                                                                   int? firstResult,
                                                                   int? maxResults,
                                                                   params INHOrder<ResourceActor>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"리소스 접근 권한 정보를 조회합니다... " +
                          @"resource={0}, resourceInstanceId={1}, companyCode={2}, firstResult={3}, maxResults={4}, orders={5}",
                          resource, resourceInstanceId, companyCode, firstResult, maxResults, orders.CollectionToString());

            var query = BuildQueryOverOfResourceActor(resource, resourceInstanceId, companyCode, null, null, null).AddOrders(orders);

            return Repository<ResourceActor>.FindAll(query,
                                                     firstResult.GetValueOrDefault(),
                                                     maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 리소스 정보를 기준으로 모든 접근 권한 정보를 조회합니다.
        /// </summary>
        /// <param name="resource">접근 대상 리소스 종류</param>
        /// <param name="resourceInstanceId">접근 대상 리소스 Id</param>
        /// <param name="companyCode">회사 코드</param>
        /// <returns></returns>
        public IList<ResourceActor> FindAllResourceActorByResource(Resource resource, string resourceInstanceId, string companyCode)
        {
            return FindAllResourceActorByResource(resource, resourceInstanceId, companyCode);
        }

        /// <summary>
        /// 지정한 접근자의 리소스 접근 권한 정보를 모두 조회합니다.
        /// </summary>
        /// <param name="companyCode">접근자 소속 회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 (부서|사용자|그룹 등) 종류</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<ResourceActor> FindAllResourceActorByActor(string companyCode, string actorCode, ActorKinds? actorKind,
                                                                int? firstResult, int? maxResults, params INHOrder<ResourceActor>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"리소스 접근 권한 정보를 조회합니다... " +
                          @"companyCode={0}, actorCode={1}, actorKind={2}, firstResult={3}, maxResults={4}, orders={5}",
                          companyCode, actorCode, actorKind, firstResult, maxResults, orders.CollectionToString());

            var query = BuildQueryOverOfResourceActor(null, null, companyCode, actorCode, actorKind, null).AddOrders(orders);

            return Repository<ResourceActor>.FindAll(query,
                                                     firstResult.GetValueOrDefault(),
                                                     maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정한 접근자의 리소스 접근 권한 정보를 모두 조회합니다.
        /// </summary>
        /// <param name="companyCode">접근자 소속 회사 코드</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드)</param>
        /// <param name="actorKind">접근자 (부서|사용자|그룹 등) 종류</param>
        /// <returns></returns>
        public IList<ResourceActor> FindAllResourceActorByActor(string companyCode, string actorCode, ActorKinds? actorKind)
        {
            return FindAllResourceActorByActor(companyCode, actorCode, actorKind, null, null);
        }

        /// <summary>
        /// 리소스 접근 권한 정보(ResourceActor) 를 Paging 처리해서 로드합니다...
        /// </summary>
        /// <param name="resource">접근 대상 리소스 종류</param>
        /// <param name="resourceInstanceId">접근 대상 리소스 Id (null이면 검색 대상에서 제외)</param>
        /// <param name="companyCode">회사 코드 (null이면 검색 대상에서 제외)</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<ResourceActor> GetPageOfResourceActorByResource(Resource resource,
                                                                           string resourceInstanceId,
                                                                           string companyCode,
                                                                           int pageIndex,
                                                                           int pageSize,
                                                                           params INHOrder<ResourceActor>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"리소스 접근 권한 정보(ResourceActor) 를 Paging 처리해서 로드합니다... " +
                          @"resource={0}, resourceInstanceId={1}, companyCode={2}, pageIndex={3}, pageSize={4}, orders={5}",
                          resource, resourceInstanceId, companyCode, pageIndex, pageSize, orders.CollectionToString());

            var query = BuildQueryOverOfResourceActor(resource, resourceInstanceId, companyCode, null, null, null);

            return Repository<ResourceActor>.GetPage(pageIndex,
                                                     pageSize,
                                                     query,
                                                     orders);
        }

        /// <summary>
        /// 리소스 접근 권한 정보(ResourceActor) 를 Paging 처리해서 로드합니다...
        /// </summary>
        /// <param name="companyCode">접근자 소속 회사 코드 (not null)</param>
        /// <param name="actorCode">접근자 코드 (회사|부서|사용자|그룹 코드) (null 이면 검색 조건에서 제외)</param>
        /// <param name="actorKind">접근자 (부서|사용자|그룹 등) 종류 (null이면 검색 조건에서 제외)</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<ResourceActor> GetPageOfResourceActorByActor(string companyCode,
                                                                        string actorCode,
                                                                        ActorKinds? actorKind,
                                                                        int pageIndex,
                                                                        int pageSize,
                                                                        params INHOrder<ResourceActor>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"리소스 접근 권한 정보(ResourceActor) 를 Paging 처리해서 로드합니다... " +
                          @"companyCode={0}, actorCode={1}, actorKind={2}, pageIndex={3}, pageSize={4}, orders={5}",
                          companyCode, actorCode, actorKind, pageIndex, pageSize, orders.CollectionToString());

            var query = BuildQueryOverOfResourceActor(null, null, companyCode, actorCode, actorKind, null);

            return Repository<ResourceActor>.GetPage(pageIndex, pageSize, query, orders);
        }
    }
}