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
    /// 마스터 코드 관련 정보
    /// </summary>
    public partial class ProductRepository
    {
        /// <summary>
        /// MasterCode 를 조회하기 위해 QueryOver 를 빌드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">MasterCode의 코드</param>
        /// <param name="name">MasterCode의 Name</param>
        /// <param name="isActive">사용여부</param>
        /// <returns></returns>
        public QueryOver<MasterCode, MasterCode> BuildQueryOverOfMasterCode(Product product,
                                                                            string code = null,
                                                                            string name = null,
                                                                            bool? isActive = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"MasterCode 를 조회하기 위해 Criteria를 빌드합니다... product={0}, code={1}, name={2}, isActive={3}",
                          product, code, name, isActive);

            var query = QueryOver.Of<MasterCode>();

            if(product != null)
                query.AddWhere(mc => mc.Product == product);

            if(code.IsNotWhiteSpace())
                query.AddWhere(mc => mc.Code == code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(mc => mc.Name == name);

            if(isActive.HasValue)
                query.AddNullAsTrue(mc => mc.IsActive, isActive);

            return query;
        }

        /// <summary>
        /// MasterCode 를 조회하기 위해 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">MasterCode의 코드</param>
        /// <param name="name">MasterCode의 Name</param>
        /// <param name="isActive">사용여부</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfMasterCode(Product product,
                                                          string code = null,
                                                          string name = null,
                                                          bool? isActive = null)
        {
            return BuildQueryOverOfMasterCode(product, code, name, isActive).DetachedCriteria;
        }

        /// <summary>
        /// MasterCode를 조회합니다. 없다면 새로 생성합니다.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public MasterCode GetOrCreateMasterCode(Product product, string code)
        {
            var masterCode = FindOneMasterCodeByCode(product, code);

            if(masterCode != null)
                return masterCode;

            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    CreateMasterCode(product, code, code, true, null, null);
                    UnitOfWork.Current.TransactionalFlush();
                }
            }

            masterCode = FindOneMasterCodeByCode(product, code);
            masterCode.AssertExists("masterCode");

            return masterCode;
        }

        /// <summary>
        /// 새로운 MasterCode를 생성합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">MasterCode의 코드</param>
        /// <param name="name">MasterCode의 Name</param>
        /// <param name="isActive">사용여부</param>
        /// <param name="description">설명</param>
        /// <param name="exAttr">확장속성</param>
        /// <returns>새로 생성된 MasterCode</returns>
        public MasterCode CreateMasterCode(Product product, string code, string name, bool? isActive, string description, string exAttr)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"새로운 MasterCode를 생성합니다... " +
                          @"product={0}, code={1}, name={2}, isActive={3}, description={4}, exAttr={5}",
                          product, code, name, isActive, description, exAttr);

            var masterCode = new MasterCode(product, code, name)
                             {
                                 IsActive = isActive,
                                 Description = description,
                                 ExAttr = exAttr
                             };

            return Repository<MasterCode>.SaveOrUpdate(masterCode);
        }

        /// <summary>
        /// 코드정보로 MasterCode를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">찾을 코드 값</param>
        /// <returns></returns>
        public MasterCode FindOneMasterCodeByCode(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"코드정보로 MasterCode를 조회합니다... product={0}, code={1}", product, code);

            return Repository<MasterCode>.FindOne(BuildCriteriaOfMasterCode(product, code));
        }

        /// <summary>
        /// 제품에 소속된 모든 MasterCode를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<MasterCode> FindAllMasterCodeByProduct(Product product, params INHOrder<MasterCode>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품에 소속된 모든 MasterCode를 조회합니다... " + product);

            return Repository<MasterCode>.FindAll(BuildQueryOverOfMasterCode(product).AddOrders(orders));
        }

        /// <summary>
        /// 제품에 소속된 모든 사용가능한 MasterCode를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<MasterCode> FindAllActiveMasterCodeByProduct(Product product, params INHOrder<MasterCode>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품에 소속된 모든 사용가능한 MasterCode를 조회합니다... product=" + product);

            return Repository<MasterCode>.FindAll(BuildQueryOverOfMasterCode(product, null, null, true).AddOrders(orders));
        }

        /// <summary>
        /// 제품에 소속된 모든 MasterCode를 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<MasterCode> FindAllMasterCodeByProduct(Product product, int pageIndex, int pageSize, params INHOrder<MasterCode>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품에 소속된 모든 MasterCode를 Paging 처리해서 로드합니다... " +
                          @"product={0}, pageIndex={1}, pagesize={2}, orders={3}",
                          product, pageIndex, pageSize, orders.CollectionToString());

            return Repository<MasterCode>.GetPage(pageIndex,
                                                  pageSize,
                                                  BuildQueryOverOfMasterCode(product),
                                                  orders);
        }

        /// <summary>
        /// 새로운 MasterCodeItem을 생성합니다.
        /// </summary>
        /// <param name="masterCode">MasterCode 인스턴스</param>
        /// <param name="itemCode">Item Code</param>
        /// <param name="itemName">Item Name (표시명)</param>
        /// <param name="itemValue">Item Value (값)</param>
        /// <param name="viewOrder">정렬 순서</param>
        /// <returns>새로운 <see cref="MasterCodeItem"/></returns>
        public MasterCodeItem CreateMasterCodeItem(MasterCode masterCode, string itemCode, string itemName, string itemValue, int? viewOrder)
        {
            masterCode.ShouldNotBeNull("masterCode");
            itemCode.ShouldNotBeWhiteSpace("itemCode");

            if(IsDebugEnabled)
                log.Debug(@"새로운 MasterCodeItem을 생성합니다... masterCode={0}, itemCode={1}, itemName={2}, itemValue={3}, viewOrder={4}",
                          masterCode, itemCode, itemName, itemValue, viewOrder);

            var codeItem = new MasterCodeItem(masterCode, itemCode, itemName, itemValue)
                           {
                               ViewOrder = viewOrder
                           };

            masterCode.Items.Add(codeItem);
            //  UnitOfWork.CurrentSession.SaveOrUpdate(masterCode);

            // 정렬 순서를 넣는다.
            if(viewOrder.HasValue == false)
                codeItem.ViewOrder = Math.Max(0, masterCode.Items.Count - 1);

            return Repository<MasterCodeItem>.SaveOrUpdate(codeItem);
        }

        /// <summary>
        /// 새로운 MasterCodeItem을 생성합니다.
        /// </summary>
        /// <param name="masterCode">MasterCode 인스턴스</param>
        /// <param name="itemCode">Item Code</param>
        /// <param name="itemName">Item Name (표시명)</param>
        /// <param name="itemValue">Item Value (값)</param>
        /// <returns>새로운 <see cref="MasterCodeItem"/></returns>
        public MasterCodeItem CreateMasterCodeItem(MasterCode masterCode, string itemCode, string itemName, string itemValue)
        {
            return CreateMasterCodeItem(masterCode, itemCode, itemCode, itemValue, null);
        }

        /// <summary>
        /// 새로운 MasterCodeItem을 생성합니다.
        /// </summary>
        /// <param name="masterCode">MasterCode 인스턴스</param>
        /// <param name="itemCode">Item Code</param>
        /// <param name="itemValue">Item Value (값)</param>
        /// <returns>새로운 <see cref="MasterCodeItem"/></returns>
        public MasterCodeItem CreateMasterCodeItem(MasterCode masterCode, string itemCode, string itemValue)
        {
            return CreateMasterCodeItem(masterCode, itemCode, itemCode, itemValue);
        }

        /// <summary>
        /// 제품에 속한 모든 <see cref="MasterCodeItem"/> 정보를 조회합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="orders">정렬순서</param>
        /// <returns></returns>
        public IList<MasterCodeItem> FindAllMasterCodeItemByProduct(Product product, params INHOrder<MasterCodeItem>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품에 소속된 모든 MasterCodeItem를 조회합니다... product={0}, orders={1}", product, orders.CollectionToString());

            MasterCode @masterCodeAlias = null;
            var query =
                QueryOver.Of<MasterCodeItem>()
                    .JoinAlias(mci => mci.MasterCode, () => @masterCodeAlias)
                    .AddWhere(() => @masterCodeAlias.Product == product)
                    .AddOrders(orders);

            return Repository<MasterCodeItem>.FindAll(query);
        }

        /// <summary>
        /// 제품에 소속된 MasterCodeItem를 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<MasterCodeItem> GetPageOfMasterCodeItemByProduct(Product product, int pageIndex, int pageSize, params INHOrder<MasterCodeItem>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품에 소속된 MasterCodeItem를 Paging 처리해서 로드합니다... product={0}, pageIndex={1}, pagesize={2}, orders={3}",
                          product, pageIndex, pageSize, orders.CollectionToString());

            MasterCode @masterCodeAlias = null;

            var query =
                QueryOver.Of<MasterCodeItem>()
                    .JoinAlias(mci => mci.MasterCode, () => @masterCodeAlias)
                    .AddWhere(() => @masterCodeAlias.Product == product)
                    .AddOrders(orders);

            return Repository<MasterCodeItem>.GetPage(pageIndex, pageSize, query);
        }
    }
}