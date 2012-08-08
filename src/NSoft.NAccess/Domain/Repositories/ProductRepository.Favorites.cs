using System.Collections.Generic;
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
    /// 제품 관련 Domain Service
    /// </summary>
    public partial class ProductRepository
    {
        /// <summary>
        /// 즐겨찾기를 위한 질의 객체 (<see cref="DetachedCriteria"/>)를 빌드합니다.
        /// </summary>
        /// <param name="product">지정된 제품, null이면 검색조건에서 제외합니다.</param>
        /// <param name="company">지정된 회사, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerCode">소유자 코드, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerKind">소유자 종류, <see cref="ActorKinds.Unknown"/>이면 검색조건에서 제외합니다.</param>
        /// <param name="registerCode">등록자 코드</param>
        /// <param name="registTimeRange">등록일 검색 범위</param>
        /// <returns></returns>
        public virtual QueryOver<Favorite, Favorite> BuildQueryOverOfFavorite(Product product,
                                                                              Company company,
                                                                              string ownerCode,
                                                                              ActorKinds? ownerKind = null,
                                                                              string registerCode = null,
                                                                              ITimePeriod registTimeRange = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"즐겨찾기 조회를 위한 QueryOver를 생성합니다... " +
                          @"company={0}, product={1}, ownerCode={2}, ownerKind={3}, registerCode={4}, registTimeRange={5}",
                          company, product, ownerCode, ownerKind, registerCode, registTimeRange);

            var query = QueryOver.Of<Favorite>();

            if(product != null)
                query.AddWhere(f => f.ProductCode == product.Code);

            if(company != null)
                query.AddWhere(f => f.CompanyCode == company.Code);

            if(ownerCode.IsNotWhiteSpace())
                query.AddWhere(f => f.OwnerCode == ownerCode);

            if(ownerKind.HasValue)
                query.AddWhere(f => f.OwnerKind == ownerKind.Value);


            if(registerCode.IsNotWhiteSpace())
                query.AddWhere(f => f.RegisterCode == registerCode);

            if(registTimeRange != null && registTimeRange.IsAnytime == false)
                query.AddBetween(f => f.RegistDate, registTimeRange.StartAsNullable, registTimeRange.EndAsNullable);

            return query;
        }

        /// <summary>
        /// 즐겨찾기를 위한 질의 객체 (<see cref="DetachedCriteria"/>)를 빌드합니다.
        /// </summary>
        /// <param name="product">지정된 제품, null이면 검색조건에서 제외합니다.</param>
        /// <param name="company">지정된 회사, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerCode">소유자 코드, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerKind">소유자 종류, <see cref="ActorKinds.Unknown"/>이면 검색조건에서 제외합니다.</param>
        /// <param name="registerCode">등록자 코드</param>
        /// <param name="registTimeRange">등록일 검색 범위</param>
        /// <returns></returns>
        public virtual DetachedCriteria BuildCriteriaOfFavorite(Product product, Company company,
                                                                string ownerCode, ActorKinds? ownerKind, string registerCode, ITimePeriod registTimeRange)
        {
            return BuildQueryOverOfFavorite(product, company, ownerCode, ownerKind, registerCode, registTimeRange).DetachedCriteria;
        }

        /// <summary>
        /// 즐겨찾기를 조회합니다. DB에 존재하지 않으면 새로 생성합니다.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="company"></param>
        /// <param name="ownerCode"></param>
        /// <param name="ownerKind"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Favorite GetOrCreateFavorite(Product product, Company company, string ownerCode, ActorKinds ownerKind, string content)
        {
            var favorite = FindOneFavorite(product, company, ownerCode, ownerKind);

            if(favorite != null)
                return favorite;

            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    CreateFavorite(product, company, ownerCode, ownerKind, content);
                    UnitOfWork.Current.TransactionalFlush();
                }
            }

            favorite = FindOneFavorite(product, company, ownerCode, ownerKind);
            favorite.AssertExists("favorite");

            if(log.IsInfoEnabled)
                log.Info(@"새로운 즐겨찾기 정보를 생성했습니다. New Favorite = " + favorite);

            return favorite;
        }

        /// <summary>
        /// 즐겨찾기 생성
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="company">소유자 회사</param>
        /// <param name="ownerCode">소유자 코드</param>
        /// <param name="ownerKind">소유자 종류</param>
        /// <param name="content">즐겨찾기 내용</param>
        /// <returns></returns>
        public Favorite CreateFavorite(Product product, Company company, string ownerCode, ActorKinds ownerKind, string content)
        {
            product.ShouldNotBeNull("product");
            company.ShouldNotBeNull("company");
            ownerCode.ShouldNotBeWhiteSpace("owernCode");

            if(IsDebugEnabled)
                log.Debug(@"새로운 즐겨찾기 정보를 생성합니다... product={0}, company={1}, ownerCode={2}, ownerKind={3}, content={4}",
                          product, company, ownerCode, ownerKind, content);

            var favorite = new Favorite(product, company, ownerCode, ownerKind, content);
            return Repository<Favorite>.SaveOrUpdate(favorite);
        }

        /// <summary>
        /// 즐겨찾기 조회
        /// </summary>
        public Favorite FindOneFavorite(Product product, Company company, string ownerCode, ActorKinds ownerKind)
        {
            if(IsDebugEnabled)
                log.Debug(@"새로운 즐겨찾기(Favorite) 정보를 조회합니다... product={0}, company={1}, ownerCode={2}, ownerKind={3}",
                          product, company, ownerCode, ownerKind);

            return Repository<Favorite>.FindOne(BuildQueryOverOfFavorite(product, company, ownerCode, ownerKind));
        }

        /// <summary>
        /// 소유자의 즐겨찾기 정보를 조회합니다.
        /// </summary>
        /// <param name="product">지정된 제품, null이면 검색조건에서 제외합니다.</param>
        /// <param name="company">지정된 회사, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerCode">소유자 Id, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerKind">소유자 종류, <see cref="ActorKinds.Unknown"/>이면 검색조건에서 제외합니다.</param>
        /// <returns></returns>
        public IList<Favorite> FindAllFavoriteByOwner(Product product, Company company, string ownerCode, ActorKinds ownerKind)
        {
            return FindAllFavoriteByOwner(product, company, ownerCode, ownerKind, null, null);
        }

        /// <summary>
        /// 소유자의 즐겨찾기 정보를 조회합니다.
        /// </summary>
        /// <param name="product">지정된 제품, null이면 검색조건에서 제외합니다.</param>
        /// <param name="company">지정된 회사, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerCode">소유자 Id, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerKind">소유자 종류, <see cref="ActorKinds.Unknown"/>이면 검색조건에서 제외합니다.</param>
        /// <param name="firstResult">첫번째 레코드 인덱스</param>
        /// <param name="maxResults">최대 레코드 수</param>
        /// <param name="orders">정렬 방식</param>
        /// <returns></returns>
        public IList<Favorite> FindAllFavoriteByOwner(Product product, Company company, string ownerCode, ActorKinds ownerKind,
                                                      int? firstResult, int? maxResults, params INHOrder<Favorite>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"소유자의 모든 즐겨찾기 정보를 조회합니다... " +
                          @"ownerCode={0}, ownerKind={1}, company={2}, product={3}, firstResult={4}, maxResults={5}, orders={6}",
                          ownerCode, ownerKind, company, product, firstResult, maxResults, orders.CollectionToString());

            var query = BuildQueryOverOfFavorite(product, company, ownerCode, ownerKind).AddOrders(orders);

            return Repository<Favorite>.FindAll(query,
                                                firstResult.GetValueOrDefault(),
                                                maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 소유자의 즐겨찾기 정보를 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="product">지정된 제품, null이면 검색조건에서 제외합니다.</param>
        /// <param name="company">지정된 회사, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerCode">소유자 Id, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerKind">소유자 종류, <see cref="ActorKinds.Unknown"/>이면 검색조건에서 제외합니다.</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<Favorite> GetPageOfFavoriteByOwner(Product product, Company company, string ownerCode, ActorKinds ownerKind,
                                                              int pageIndex, int pageSize, params INHOrder<Favorite>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"소유자의 즐겨찾기 정보를 Paging 처리해서 로드합니다... " +
                          @"product={0}, company={1}, ownerCode={2}, ownerKind={3}, pageIndex={4}, pageSize={5}, orders={6}",
                          product, company, ownerCode, ownerKind, pageIndex, pageSize, orders.CollectionToString());

            var query = BuildQueryOverOfFavorite(product, company, ownerCode, ownerKind);

            return Repository<Favorite>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// 소유자의 즐겨찾기 정보를 삭제합니다
        /// </summary>
        /// <param name="company">지정된 회사, null이면 검색조건에서 제외합니다.</param>
        /// <param name="product">지정된 제품, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerCode">소유자 Id, null이면 검색조건에서 제외합니다.</param>
        /// <param name="ownerKind"></param>
        /// <returns></returns>
        public void DeleteAllFavoriteByOwner(Product product, Company company, string ownerCode, ActorKinds ownerKind)
        {
            if(IsDebugEnabled)
                log.Debug(@"소유자의 즐겨찾기 정보를 삭제합니다... ownerCode={0}, ownerKind={1}, company={2}, product={3}",
                          ownerCode, ownerKind, company, product);

            Repository<Favorite>.DeleteAll(BuildQueryOverOfFavorite(product, company, ownerCode, ownerKind));
        }
    }
}