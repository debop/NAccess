using System.Collections.Generic;
using System.Linq;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.LinqEx;
using NSoft.NFramework.Reflections;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 메뉴와 관련된 Domain Service 입니다.
    /// </summary>
    public partial class ProductRepository
    {
        /// <summary>
        /// <see cref="MenuTemplate"/>를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">MenuTemplate 코드</param>
        /// <param name="name">MenuTemplate 명</param>
        /// <returns></returns>
        public QueryOver<MenuTemplate, MenuTemplate> BuildQueryOverOfMenuTemplate(Product product, string code, string name)
        {
            if(IsDebugEnabled)
                log.Debug(@"MenuTemplate 정보를 조회하기 위해 Criteria를 빌드합니다. product={0}, code={1}, name={2}", product, code, name);

            var query = QueryOver.Of<MenuTemplate>();

            if(product != null)
                query.AddWhere(mt => mt.Product == product);

            if(code.IsNotWhiteSpace())
                query.AddWhere(mt => mt.Code == code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(mt => mt.Name == name);

            return query;
        }

        /// <summary>
        /// <see cref="MenuTemplate"/>를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">MenuTemplate 코드</param>
        /// <param name="name">MenuTemplate 명</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfMenuTemplate(Product product, string code, string name)
        {
            return BuildQueryOverOfMenuTemplate(product, code, name).DetachedCriteria;
        }

        /// <summary>
        /// 메뉴템플릿을 조회합니다. 없다면 새로 생성해서 반환합니다.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public MenuTemplate GetOrCreateMenuTemplate(Product product, string code)
        {
            var menuTemplate = FindOneMenuTemplateByCode(product, code);
            if(menuTemplate != null)
                return menuTemplate;

            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    CreateMenuTemplate(product, code, code, null);
                    UnitOfWork.Current.TransactionalFlush();
                }
            }

            menuTemplate = FindOneMenuTemplateByCode(product, code);
            menuTemplate.AssertExists("menuTemplate");

            return menuTemplate;
        }

        /// <summary>
        /// 새로운 메뉴 템플릿을 생성합니다.
        /// </summary>
        /// <returns></returns>
        public MenuTemplate CreateMenuTemplate(Product product, string code, string name, string menuUrl)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"새로운 MenuTemplate를 생성합니다. product={0}, code={1}, name={2}, menuUrl={3}", product, code, name, menuUrl);

            var menuTemplate = new MenuTemplate(product, code)
                               {
                                   Name = name ?? code,
                                   MenuUrl = menuUrl
                               };
            menuTemplate.SetParent(null);
            menuTemplate.UpdateNodePosition();

            return Repository<MenuTemplate>.SaveOrUpdate(menuTemplate);
        }

        /// <summary>
        /// 지정된 부모 메뉴 템플릿의 하위로 자식 메뉴 템플릿을 생성합니다.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="menuUrl"></param>
        /// <returns></returns>
        public MenuTemplate CreateMenuTemplateOf(MenuTemplate parent, string code, string name, string menuUrl)
        {
            parent.ShouldNotBeNull("parent");

            var menuTemplate = CreateMenuTemplate(parent.Product, code, name, menuUrl);
            menuTemplate.SetParent(parent);

            return Repository<MenuTemplate>.SaveOrUpdate(menuTemplate);
        }

        /// <summary>
        /// 지정된 메뉴 템플릿을 새로운 부모의 자식으로 등록합니다.
        /// </summary>
        /// <param name="child"></param>
        /// <param name="newParent"></param>
        public void ChangeMenuTemplateParent(MenuTemplate child, MenuTemplate newParent)
        {
            child.ShouldNotBeNull("child");

            var oldParent = child.Parent;

            if(Equals(oldParent, newParent))
            {
                if(IsDebugEnabled)
                    log.Debug(@"변경할 부모가 현재 부모와 같습니다. 부모 변경 작업을 수행하지 않습니다!!!");
                return;
            }

            if(IsDebugEnabled)
                log.Debug(@"MenuTemplate의 부모 정보를 변경합니다... child code={0}, new parent code={1}, old parent code={2}",
                          child.Code,
                          (newParent != null) ? newParent.Code : EntityTool.NULL_STRING,
                          (oldParent != null) ? oldParent.Code : EntityTool.NULL_STRING);

            child.ChangeParent(oldParent, newParent);

            Repository<MenuTemplate>.SaveOrUpdate(child);
        }

        /// <summary>
        /// 지정한 Id 값을 가진 MenuTemplate를 조회합니다.
        /// </summary>
        /// <param name="product">소속 product</param>
        /// <param name="code">menu template id</param>
        /// <returns>메뉴 정보, 없으면 null을 반환한다.</returns>
        public MenuTemplate FindOneMenuTemplateByCode(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"MenuTemplate 정보를 조회합니다. product={0}, code={1}", product, code);

            return Repository<MenuTemplate>.FindOne(BuildQueryOverOfMenuTemplate(product, code, null));
        }

        /// <summary>
        /// 제품별로 메뉴 템플릿의 Root 템플릿을 가져옵니다.
        /// </summary>
        public IList<MenuTemplate> FindRootMenuTemplateByProduct(Product product, params INHOrder<MenuTemplate>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"지정한 제품의 Root MenuTemplate 정보를 조회합니다... " + product);

            var query =
                BuildQueryOverOfMenuTemplate(product, null, null)
                    .AddWhereRestrictionOn(mt => mt.Parent).IsNull
                    .AddOrders(orders);

            return Repository<MenuTemplate>.FindAll(query);
        }

        /// <summary>
        /// 지정한 제품의 모든 메뉴 템플릿 정보를 조회합니다.
        /// </summary>
        public IList<MenuTemplate> FindAllMenuTemplateByProduct(Product product, params INHOrder<MenuTemplate>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"지정한 제품의 모든 MenuTemplate 정보를 조회합니다... " + product);

            return Repository<MenuTemplate>.FindAll(BuildQueryOverOfMenuTemplate(product, null, null).AddOrders(orders));
        }

        /// <summary>
        /// 메뉴 템플릿 이름이 매칭되는 검색을 수행합니다.
        /// </summary>
        public IList<MenuTemplate> FindAllMenuTemplateByNameToMatch(Product product, string nameToMatch, MatchMode matchMode, params INHOrder<MenuTemplate>[] orders)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"지정한 제품의 모든 MenuTemplate 정보를 조회합니다... product={0}, nameToMatch={1}, matchMode={2}, orders={3}",
                          product, nameToMatch, matchMode, orders.CollectionToString());

            var query =
                BuildQueryOverOfMenuTemplate(product, null, null)
                    .AddInsensitiveLike(mt => mt.Name, nameToMatch, matchMode ?? MatchMode.Anywhere)
                    .AddOrders(orders);

            return Repository<MenuTemplate>.FindAll(query);
        }

        /// <summary>
        /// 지정된 Id에 해당하는 메뉴 템플릿을 조회합니다. 옵션으로 조상, 자손 메뉴 템플릿도 가져올 수 있도록 합니다.
        /// </summary>
        /// <param name="product">제품 정보</param>
        /// <param name="menuTemplateId">메뉴 템플릿 Id</param>
        /// <param name="hierarchyContainsKinds">메뉴 템플릿의 조상/자손도 포함할 것인가 여부</param>
        public IList<MenuTemplate> FildAllMenuTemplateByHierarchy(Product product, string menuTemplateId, HierarchyContainsKinds hierarchyContainsKinds)
        {
            // NOTE: 현재는 Loop 방식을 사용하여 round-trip이 많이 발생하게끔 되어 있다. Second Cache로 속도를 극복할 수 밖에 없다.
            //
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"계층구조에 따른 메뉴 템플릿 정보를 조회합니다. product={0}, code={1}, hierarchyContainsKinds={2}",
                          product, menuTemplateId, hierarchyContainsKinds);

            var results = new HashSet<MenuTemplate>();

            var menuTemplate = FindOneMenuTemplateByCode(product, menuTemplateId);

            if(menuTemplate == null)
                return results.ToList();

            // 1. 현재 메뉴 템플릿의 조상들 추가한다.
            if((hierarchyContainsKinds & HierarchyContainsKinds.Ancestors) > 0)
                menuTemplate.GetAncestors().RunEach(mt => results.Add(mt));

            // 2. 현재 메뉴 템플릿을 추가한다.
            if((hierarchyContainsKinds & HierarchyContainsKinds.Self) > 0)
                results.Add(menuTemplate);

            // 3. 현재 메뉴 템플릿의 자손들을 추가한다.
            if((hierarchyContainsKinds & HierarchyContainsKinds.Descendents) > 0)
                menuTemplate.GetDescendents().RunEach(mt => results.Add(mt));

            return results.ToList();
        }

        /// <summary>
        /// 지정한 메뉴 템플릿 정보를 삭제합니다. HBM의 cascade 설정에 따라 cascade-delete가 수행될 수 있습니다.
        /// </summary>
        public void DeleteMenuTemplate(MenuTemplate menuTemplate)
        {
            menuTemplate.ShouldNotBeNull("menuTemplate");

            if(IsDebugEnabled)
                log.Debug(@"지정한 MenuTemplate를 삭제합니다... " + menuTemplate);

            DeleteEntityTransactional(menuTemplate);
        }

        /// <summary>
        /// 지정된 Id 정보를 가진 메뉴 템플릿을 삭제합니다. HBM의 cascade 설정에 따라 cascade-delete가 수행될 수 있습니다.
        /// </summary>
        public void DeleteMenuTemplateByCode(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"지정한 MenuTemplate를 삭제합니다... product={0}, code={1}", product, code);

            var menuTemplate = FindOneMenuTemplateByCode(product, code);

            if(menuTemplate != null)
                DeleteMenuTemplate(menuTemplate);
        }

        /// <summary>
        /// 지정된 제품에 소속된 모든 MenuTemplate를 삭제합니다.
        /// </summary>
        /// <param name="product"></param>
        public void DeleteAllMenuTemplateByProduct(Product product)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품에 소속된 모든 MenuTemplate를 삭제합니다... " + product);

            Repository<MenuTemplate>.DeleteAll(BuildQueryOverOfMenuTemplate(product, null, null));
        }

        /// <summary>
        /// <see cref="Menu"/>정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        public QueryOver<Menu, Menu> BuildQueryOverOfMenu(Product product, string code, MenuTemplate menuTemplate, bool? isActive)
        {
            var query = QueryOver.Of<Menu>();

            MenuTemplate @menuTemplateAlias = null;

            if(product != null)
                query.JoinAlias(m => m.MenuTemplate, () => @menuTemplateAlias)
                    .AddWhere(() => @menuTemplateAlias.Product == product);

            if(code.IsNotWhiteSpace())
                query.AddWhere(m => m.Code == code);

            if(menuTemplate != null)
                query.AddWhere(m => m.MenuTemplate == menuTemplate);

            if(isActive.HasValue)
                query.AddNullAsTrue(m => m.IsActive, isActive.Value);

            return query;
        }

        /// <summary>
        /// <see cref="Menu"/>정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        public DetachedCriteria BuildCriteriaOfMenu(Product product, string code, MenuTemplate menuTemplate, bool? isActive)
        {
            return BuildQueryOverOfMenu(product, code, menuTemplate, isActive).DetachedCriteria;
        }

        /// <summary>
        /// 메뉴를 조회합니다. 없으면 새로 생성합니다.
        /// </summary>
        public Menu GetOrCreateMenu(MenuTemplate menuTemplate, string code)
        {
            var menu = FindOneMenuByTemplateAndCode(menuTemplate, code);

            if(menu != null)
                return menu;

            lock(_syncLock)
                new Menu(menuTemplate, code).InsertStateless();

            menu = FindOneMenuByTemplateAndCode(menuTemplate, code);
            menu.AssertExists("menu");

            return menu;
        }

        /// <summary>
        /// 새로운 메뉴를 생성합니다.
        /// </summary>
        public Menu CreateMenu(MenuTemplate menuTemplate, string code)
        {
            menuTemplate.ShouldNotBeNull("menuTemplate");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"새로운 Menu 정보를 생성합니다. code={0}, menuTemplate={1}", code, menuTemplate);

            var menu = new Menu(menuTemplate, code);

            return Repository<Menu>.SaveOrUpdate(menu);
        }

        /// <summary>
        /// 지정된 부모 메뉴의 자식 메뉴를 생성합니다.
        /// </summary>
        /// <param name="parent">부모 메뉴</param>
        /// <param name="code">새로운 메뉴의 Id</param>
        /// <param name="menuTemplate">새로운 메뉴의 메뉴 템플릿 정보</param>
        /// <returns></returns>
        public Menu CreateMenuOf(Menu parent, string code, MenuTemplate menuTemplate)
        {
            if(parent == null)
                return CreateMenu(menuTemplate, code);

            code.ShouldNotBeWhiteSpace("code");
            Guard.Assert(parent.MenuTemplate.Product == menuTemplate.Product,
                         @"부모 메뉴의 Product 정보와 새로 만들 MenuTemplate의 Product정보가 틀립니다.");

            if(IsDebugEnabled)
                log.Debug(@"새로운 Menu 정보를 생성합니다... parent={0}, code={1}, menuTemplate={2}",
                          parent, code, menuTemplate);

            var menu = new Menu(menuTemplate, code);

            menu.SetParent(parent);
            menu.UpdateNodePosition();

            return Repository<Menu>.SaveOrUpdate(menu);
        }

        /// <summary>
        /// 지정된 자식 Menu의 부모를 지정된 Menu로 변경합니다.
        /// </summary>
        /// <param name="child">child menu</param>
        /// <param name="newParent">new parent menu</param>
        public void ChangeMenuParent(Menu child, Menu newParent)
        {
            child.ShouldNotBeNull("child");

            var oldParent = child.Parent;

            if(Equals(oldParent, newParent))
            {
                if(log.IsInfoEnabled)
                    log.Info(@"변경할 부모가 현재 부모와 같습니다. 부모 변경 작업을 수행하지 않습니다.");

                return;
            }

            if(IsDebugEnabled)
                log.Debug("Menu의 부모 정보를 변경합니다... child code={0}, new parent code={1}, old parent code={2}",
                          child.Code,
                          (newParent != null) ? newParent.Code : EntityTool.NULL_STRING,
                          (oldParent != null) ? oldParent.Code : EntityTool.NULL_STRING);

            child.ChangeParent(oldParent, newParent);
            child.UpdateNodePosition();

            Repository<Menu>.SaveOrUpdate(child);
        }

        /// <summary>
        /// 메뉴코드로 메뉴를 조회합니다.
        /// </summary>
        public Menu FindOneMenuByTemplateAndCode(MenuTemplate menuTemplate, string code)
        {
            menuTemplate.ShouldNotBeNull("menuTemplate");
            code.ShouldNotBeWhiteSpace("code");

            return Repository<Menu>.FindOne(BuildQueryOverOfMenu(null, code, menuTemplate, null));
        }

        /// <summary>
        /// 메뉴코드로 메뉴를 조회합니다.
        /// </summary>
        /// <param name="product">소속 product</param>
        /// <param name="code">menu id</param>
        /// <returns>메뉴 정보, 없으면 null을 반환한다.</returns>
        public Menu FindOneMenuByProductAndCode(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            return Repository<Menu>.FindOne(BuildQueryOverOfMenu(product, code, null, null));
        }

        /// <summary>
        /// 제품별 Root Menu 정보 (Parent가 null인) 정보를 조회합니다.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public IList<Menu> FindRootMenuByProduct(Product product)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품별 Root Menu 정보를 조회합니다... " + product);

            var query = BuildQueryOverOfMenu(product, null, null, null).AddWhereRestrictionOn(m => m.Parent).IsNull;
            return Repository<Menu>.FindAll(query);
        }

        /// <summary>
        /// 지정된 제품에 소속된 모든 메뉴 정보를 조회합니다.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public IList<Menu> FindAllMenuByProduct(Product product)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"지정한 제품의 모든 Menu 정보를 조회합니다... " + product);

            return Repository<Menu>.FindAll(BuildQueryOverOfMenu(product, null, null, null));
        }

        /// <summary>
        /// 지정된 MenuTemplate와 관계된 Menu 들을 조회합니다.
        /// </summary>
        /// <param name="menuTemplate"></param>
        /// <returns></returns>
        public IList<Menu> FindAllMenuByMenuTemplate(MenuTemplate menuTemplate)
        {
            menuTemplate.ShouldNotBeNull("menuTemplate");

            if(IsDebugEnabled)
                log.Debug(@"지정한 메뉴 템플릿D의 모든 Menu 정보를 조회합니다... " + menuTemplate);

            return Repository<Menu>.FindAll(BuildQueryOverOfMenu(null, null, menuTemplate, null));
        }

        /// <summary>
        /// 지정된 Id에 해당하는 메뉴를 조회합니다. 옵션으로 조상, 자손 메뉴도 가져올 수 있도록 합니다.
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">메뉴 Id</param>
        /// <param name="hierarchyContainsKinds">메뉴의 조상/자손도 포함할 것인가 여부</param>
        /// <returns></returns>
        public IList<Menu> FindAllMenuByHierarchy(Product product, string code, HierarchyContainsKinds hierarchyContainsKinds)
        {
            // NOTE: 현재는 Loop 방식을 사용하여 round-trip이 많이 발생하게끔 되어 있다. Second Cache로 속도를 극복할 수 밖에 없다.

            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"계층구조에 따른 메뉴 정보를 조회합니다. product={0}, code={1}, hierarchyContainsKinds={2}",
                          product, code, hierarchyContainsKinds);

            var results = new HashSet<Menu>();

            // 0. 현재 Menu 정보를 조회한다.
            var menu = FindOneMenuByProductAndCode(product, code);

            if(menu == null)
                return results.ToList();

            // 1. 현재 메뉴의 조상들 추가한다.
            if((hierarchyContainsKinds & HierarchyContainsKinds.Ancestors) > 0)
                menu.GetAncestors().RunEach(m => results.Add(m));

            // 2. 현재 메뉴를 추가한다.
            if((hierarchyContainsKinds & HierarchyContainsKinds.Self) > 0)
                results.Add(menu);

            // 3. 현재 메뉴의 자손들을 추가한다.
            if((hierarchyContainsKinds & HierarchyContainsKinds.Descendents) > 0)
                menu.GetDescendents().RunEach(m => results.Add(m));

            return results.ToList();
        }

        /// <summary>
        /// 지정된 메뉴를 삭제합니다.
        /// </summary>
        /// <param name="menu"></param>
        public void DeleteMenu(Menu menu)
        {
            if(menu == null)
                return;

            if(IsDebugEnabled)
                log.Debug(@"지정한 Menu를 삭제합니다... " + menu);

            DeleteEntityTransactional(menu);
        }

        /// <summary>
        /// Id에 해당하는 Menu를 삭제합니다.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="code"></param>
        public void DeleteMenuByCode(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"지정한 Id를 가진 Menu를 삭제합니다... product={0}, code={1}", product, code);

            var menu = FindOneMenuByProductAndCode(product, code);

            if(menu != null)
                DeleteMenu(menu);
        }

        /// <summary>
        /// 제품에 소속된 모든 메뉴를 삭제합니다.
        /// </summary>
        /// <param name="product"></param>
        public void DeleteAllMenuByProduct(Product product)
        {
            product.ShouldNotBeNull("product");

            if(IsDebugEnabled)
                log.Debug(@"제품에 소속된 모든 메뉴를 삭제합니다... " + product);

            Repository<Menu>.DeleteAll(BuildQueryOverOfMenu(product, null, null, null));
        }

        /// <summary>
        /// 지정된 MenuTemplate과 관련된 모든 메뉴를 삭제합니다.
        /// </summary>
        /// <param name="menuTemplate"></param>
        public void DeleteAllMenuByMenuTemplate(MenuTemplate menuTemplate)
        {
            menuTemplate.ShouldNotBeNull("menuTemplate");

            if(IsDebugEnabled)
                log.Debug(@"지정된 MenuTemplate과 관련된 모든 메뉴를 삭제합니다... " + menuTemplate);

            Repository<Menu>.DeleteAll(BuildQueryOverOfMenu(null, null, menuTemplate, null));
        }
    }
}