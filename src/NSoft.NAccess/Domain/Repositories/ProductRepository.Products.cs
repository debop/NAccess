using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
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
        /// 제품 정보를 조회하기 위해 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="code">제품 코드</param>
        /// <param name="name">제품 명</param>
        /// <param name="isActive">활성화 여부</param>
        /// <returns>질의용 Criteria</returns>
        public QueryOver<Product, Product> BuildQueryOverOfProduct(string code = null, string name = null, bool? isActive = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"제품 정보를 조회하기 위한 Criteria를 빌드합니다... code={0}, name={1}, isActive={2}", code, name, isActive);

            var query = QueryOver.Of<Product>();

            if(code.IsNotWhiteSpace())
                query.AddWhere(p => p.Code == code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(p => p.Name == name);

            // NOTE: 컬럼 값이 NULL인 경우에는 True 라고 간주하고, 비교한다.)
            if(isActive.HasValue)
                query.AddNullAsTrue(p => p.IsActive, isActive.Value);

            return query;
        }

        /// <summary>
        /// 제품 정보를 조회하기 위해 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="code">제품 코드</param>
        /// <param name="name">제품 명</param>
        /// <param name="isActive">활성화 여부</param>
        /// <returns>질의용 Criteria</returns>
        public DetachedCriteria BuildCriteriaOfProduct(string code = null, string name = null, bool? isActive = null)
        {
            return BuildQueryOverOfProduct(code, name, isActive).DetachedCriteria;
        }

        /// <summary>
        /// 지정된 제품 코드에 해당하는 정보를 로드합니다. 만약 없다면, 새로 생성하고, 저장한 후 반환합니다.
        /// </summary>
        /// <param name="code">제품 코드</param>
        /// <returns></returns>
        public Product GetOrCreateProduct(string code)
        {
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"제품 정보를 로드합니다... code=" + code);

            var product = FindOneProductByCode(code);

            if(product != null)
                return product;

            // 해당 Product가 없다면 새로 만든다.

            if(IsDebugEnabled)
                log.Debug("기존 Product 정보가 없으므로, 새로 생성합니다... code=" + code);

            // 성능을 위해 StatelessSession을 이용하여 저장합니다.
            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    Repository<Product>.SaveOrUpdate(new Product(code));
                    UnitOfWork.Current.TransactionalFlush();
                }
            }

            product = FindOneProductByCode(code);
            product.AssertExists("product");

            if(log.IsInfoEnabled)
                log.Info(@"새로운 Product 생성에 성공했습니다!!! New Product = " + product);

            return product;
        }

        /// <summary>
        /// 지정한 제품 코드에 해당하는 제품을 조회합니다. 없으면 null을 반환합니다.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Product FindOneProductByCode(string code)
        {
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug("Product 를 조회합니다... code=" + code);

            return Repository<Product>.FindOne(BuildQueryOverOfProduct(code));
        }

        /// <summary>
        /// 지정된 제품이름의 제품 정보를 로드합니다.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Product FindOneProductByName(string name)
        {
            name.ShouldNotBeWhiteSpace("name");

            if(IsDebugEnabled)
                log.Debug("Product를 조회합니다... name=" + name);

            return Repository<Product>.FindOne(BuildQueryOverOfProduct(name: name));
        }

        /// <summary>
        /// Active인 Product를 조회합니다.
        /// </summary>
        /// <returns></returns>
        public IList<Product> FindAllActiveProduct()
        {
            if(IsDebugEnabled)
                log.Debug("모든 Active Product를 조회합니다...");

            return Repository<Product>.FindAll(BuildQueryOverOfProduct(isActive: true));
        }

        /// <summary>
        /// 제품명 매칭 검색을 수행합니다.
        /// </summary>
        /// <param name="nameToMatch">매칭 검색할 제품명</param>
        /// <param name="matchMode">매칭 모드</param>
        /// <returns></returns>
        public IList<Product> FindAllProductByNameToMatch(string nameToMatch, MatchMode matchMode)
        {
            if(IsDebugEnabled)
                log.Debug(@"제품명 매칭 검색을 수행합니다... nameToMatch={0}, matchMode={1}", nameToMatch, matchMode);

            var query = QueryOver.Of<Product>().AddInsensitiveLike(p => p.Name, nameToMatch, matchMode ?? MatchMode.Anywhere);
            return Repository<Product>.FindAll(query);
        }

        /// <summary>
        /// 지정된 Product 를 삭제합니다.
        /// </summary>
        /// <param name="product"></param>
        public void DeleteProduct(Product product)
        {
            if(product == null)
                return;

            DeleteEntityTransactional(product);
        }
    }
}