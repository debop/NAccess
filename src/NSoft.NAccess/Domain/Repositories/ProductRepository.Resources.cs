using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 리소스 관련 Domain Service 입니다.
    /// </summary>
    public partial class ProductRepository
    {
        /// <summary>
        /// 리소스 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">리소스 코드</param>
        /// <param name="name">리소스 명</param>
        /// <returns></returns>
        public QueryOver<Resource, Resource> BuildQueryOverOfResource(Product product, string code = null, string name = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"리소스 정보를 조회하기 위해 DetachedCriteria를 빌드합니다... product={0}, code={1}, name={2}",
                          product, code, name);

            var query = QueryOver.Of<Resource>();

            if(product != null)
                query.AddWhere(r => r.ProductCode == product.Code);

            if(code.IsNotWhiteSpace())
                query.AddWhere(r => r.Code == code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(r => r.Name == name);

            return query;
        }

        /// <summary>
        /// 리소스 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">리소스 코드</param>
        /// <param name="name">리소스 명</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfResource(Product product, string code, string name)
        {
            return BuildQueryOverOfResource(product, code, name).DetachedCriteria;
        }

        /// <summary>
        /// 리소스 정보를 로드합니다. 없다면, 새로 생성, 저장 후 반환합니다.
        /// </summary>
        /// <param name="product">리소스가 정의된 제품</param>
        /// <param name="code">리소스 Id</param>
        /// <returns></returns>
        public Resource GetOrCreateResource(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"Resource 정보를 로드합니다... product={0}, code={1}", product, code);

            var resource = FindOneResourceByCode(product, code);
            if(resource != null)
                return resource;

            if(IsDebugEnabled)
                log.Debug(@"Resource 정보가 없습니다. 새로 생성합니다... product={0}, code={1}", product, code);

            // 성능을 위해 StatelessSession을 사용하여 저장합니다.
            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    Repository<Resource>.SaveOrUpdate(new Resource(product.Code, code));
                    UnitOfWork.Current.TransactionalFlush();
                }
            }

            resource = FindOneResourceByCode(product, code);
            resource.AssertExists("resource");

            if(log.IsInfoEnabled)
                log.Info(@"새로운 Resource 정보를 생성했습니다!!! New Resource = " + resource);

            return resource;
        }

        /// <summary>
        /// 리소스 정보를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">리소스 코드</param>
        /// <returns></returns>
        public Resource FindOneResourceByCode(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"Resource 정보를 조회합니다... product={0}, code={1}", product, code);

            return Repository<Resource>.FindOne(BuildQueryOverOfResource(product, code));
        }

        /// <summary>
        /// 지정된 제품의 모든 리소스 정보를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <returns></returns>
        public IList<Resource> FindAllResourceByProduct(Product product)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"지정된 제품의 모든 Resource 정보를 조회합니다... " + product);

            return Repository<Resource>.FindAll(BuildQueryOverOfResource(product));
        }

        /// <summary>
        /// 리소스 이름으로 매칭된 리소스 정보를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="nameToMatch">검색하고자 하는 리소스 명</param>
        /// <param name="matchMode">매칭 모드</param>
        /// <returns></returns>
        public IList<Resource> FindAllResourceByNameToMatch(Product product, string nameToMatch, MatchMode matchMode)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"Resource 정보를 Name 매칭 검색으로 조회합니다... product={0}, nameToMatch=[{1}], matchMode={2}",
                          product, nameToMatch, matchMode);

            var query =
                BuildQueryOverOfResource(product)
                    .AddInsensitiveLike(r => r.Name, nameToMatch, matchMode ?? MatchMode.Anywhere);

            return Repository<Resource>.FindAll(query);
        }

        /// <summary>
        /// 지정된 제품의 리소스 정보를 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<Resource> GetPageOfResourceByProduct(Product product, int pageIndex, int pageSize, params INHOrder<Resource>[] orders)
        {
            product.ShouldNotBeNull("product");
            pageIndex.ShouldBePositiveOrZero("pageIndex");
            pageSize.ShouldBePositive("pageSize");

            if(IsDebugEnabled)
                log.Debug(@"지정된 제품의 리소스 정보를 Paging 처리해서 로드합니다... " +
                          @"product={0}, pageIndex={1}, pagesize={2}, orders={3}",
                          product, pageIndex, pageSize, orders);

            return Repository<Resource>.GetPage(pageIndex,
                                                pageSize,
                                                BuildQueryOverOfResource(product),
                                                orders);
        }

        /// <summary>
        /// Resource 정보를 Name 매칭 검색 결과를 Paging처리해서 로드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="codeToMatch">검색하고자 하는 리소스 명</param>
        /// <param name="matchMode">매칭 모드</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<Resource> GetPageOfResourceByCodeToMatch(Product product, string codeToMatch, MatchMode matchMode,
                                                                    int pageIndex, int pageSize, params INHOrder<Resource>[] orders)
        {
            product.ShouldNotBeNull("product");
            pageIndex.ShouldBePositiveOrZero("pageIndex");
            pageSize.ShouldBePositive("pageSize");

            if(IsDebugEnabled)
                log.Debug(@"Resource 정보를 Code 매칭 검색 결과를 Paging처리해서 로드합니다... " +
                          @"product={0}, codeToMatch={1}, matchMode={2}, pageIndex={3}, pagesize={4}, orders={5}",
                          product, codeToMatch, matchMode, pageIndex, pageSize, orders);

            var query =
                BuildQueryOverOfResource(product)
                    .AddInsensitiveLike(r => r.Code, codeToMatch, matchMode ?? MatchMode.Anywhere);

            return Repository<Resource>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// Resource 정보를 Name 매칭 검색 결과를 Paging처리해서 로드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="nameToMatch">검색하고자 하는 리소스 명</param>
        /// <param name="matchMode">매칭 모드</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<Resource> GetPageOfResourceByNameToMatch(Product product, string nameToMatch, MatchMode matchMode,
                                                                    int pageIndex, int pageSize, params INHOrder<Resource>[] orders)
        {
            product.ShouldNotBeNull("product");
            pageIndex.ShouldBePositiveOrZero("pageIndex");
            pageSize.ShouldBePositive("pageSize");

            if(IsDebugEnabled)
                log.Debug(@"Resource 정보를 Name 매칭 검색 결과를 Paging처리해서 로드합니다... " +
                          @"product={0}, nameToMatch={1}, matchMode={2}, pageIndex={3}, pagesize={4}, orders={5}",
                          product, nameToMatch, matchMode, pageIndex, pageSize, orders);

            var query =
                BuildQueryOverOfResource(product)
                    .AddInsensitiveLike(r => r.Name, nameToMatch, matchMode ?? MatchMode.Anywhere);

            return Repository<Resource>.GetPage(pageIndex, pageSize, query, orders);
        }
    }
}