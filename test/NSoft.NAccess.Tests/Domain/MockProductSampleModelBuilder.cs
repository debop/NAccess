using System.Linq;
using NSoft.NFramework.Data.NHibernateEx;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model.Products;

namespace NSoft.NAccess.Domain.Model
{
    public class MockProductSampleModelBuilder : ProductSampleModelBuilder
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public override void CreateSampleModels()
        {
            CreateProduct();
            UnitOfWork.Current.TransactionalFlush();

            CreateMasterCode();
            UnitOfWork.Current.TransactionalFlush();

            CreateMenuTemplate();
            UnitOfWork.Current.TransactionalFlush();

            CreateMenu();
            UnitOfWork.Current.TransactionalFlush();

            CreateResource();
            UnitOfWork.Current.TransactionalFlush();

            CreateResourceActor();
            UnitOfWork.Current.TransactionalFlush();

            CreateFavorite();
            UnitOfWork.Current.TransactionalFlush();

            CreateFileMapping();
            UnitOfWork.Current.TransactionalFlush();

            CreateFile();
            UnitOfWork.Current.TransactionalFlush();

            CreateUserConfig();
            UnitOfWork.Current.TransactionalFlush();
        }

        protected override void CreateMenuTemplate()
        {
            //base.CreateMenuTemplate();

            var adminProduct = NAccessContext.Domains.ProductRepository.FindOneProductByCode(SampleData.ProductCode);

            var organizationMainMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000001", "조직관리", string.Empty);
            organizationMainMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "조직관리", MenuUrl = organizationMainMenuTemplate.MenuUrl});
            organizationMainMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Organizations", MenuUrl = organizationMainMenuTemplate.MenuUrl});

            var productMainMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000002", "제품관리", string.Empty);
            productMainMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "제품관리", MenuUrl = productMainMenuTemplate.MenuUrl});
            productMainMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Products", MenuUrl = productMainMenuTemplate.MenuUrl});

            var calendarMainMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000003", "달력관리", string.Empty);
            calendarMainMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "달력관리", MenuUrl = calendarMainMenuTemplate.MenuUrl});
            calendarMainMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Calendars", MenuUrl = calendarMainMenuTemplate.MenuUrl});


            var departmentMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000004", "부서",
                                                                                                     "/Organizations/Department/DepartmentList.aspx");
            departmentMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "부서", MenuUrl = departmentMenuTemplate.MenuUrl});
            departmentMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Departmets", MenuUrl = departmentMenuTemplate.MenuUrl});

            var groupMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000005", "그룹", "/Organizations/Group/GroupList.aspx");
            groupMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "그룹", MenuUrl = groupMenuTemplate.MenuUrl});
            groupMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Groups", MenuUrl = groupMenuTemplate.MenuUrl});

            var userMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000006", "사용자", "/Organizations/User/UserList.aspx");
            userMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "사용자", MenuUrl = userMenuTemplate.MenuUrl});
            userMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Users", MenuUrl = userMenuTemplate.MenuUrl});

            var codeMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000007", "코드", "/Organizations/Code/CodeList.aspx");
            codeMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "코드", MenuUrl = codeMenuTemplate.MenuUrl});
            codeMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Codes", MenuUrl = codeMenuTemplate.MenuUrl});

            var productCodeMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000008", "제품", "/Products/Product/ProductList.aspx");
            productCodeMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "제품", MenuUrl = productCodeMenuTemplate.MenuUrl});
            productCodeMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Products", MenuUrl = productCodeMenuTemplate.MenuUrl});

            var menuTemplateMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000009", "메뉴템플릿",
                                                                                                       "/Products/Product/MenuTemplateList.aspx");
            menuTemplateMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "메뉴템플릿", MenuUrl = menuTemplateMenuTemplate.MenuUrl});
            menuTemplateMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "MenuTemplates", MenuUrl = menuTemplateMenuTemplate.MenuUrl});

            var menuMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000010", "메뉴",
                                                                                               "/Products/Menu/MenuList.aspx");
            menuMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "메뉴", MenuUrl = menuMenuTemplate.MenuUrl});
            menuMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Menus", MenuUrl = menuMenuTemplate.MenuUrl});

            var resourceMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000011", "자원",
                                                                                                   "/Products/Resource/ResourceList.aspx");
            resourceMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "자원", MenuUrl = resourceMenuTemplate.MenuUrl});
            resourceMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Resources", MenuUrl = resourceMenuTemplate.MenuUrl});

            var favoriteMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000012", "즐겨찾기",
                                                                                                   "/Products/Favorite/FavoriteList.aspx");
            favoriteMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "즐겨찾기", MenuUrl = favoriteMenuTemplate.MenuUrl});
            favoriteMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Favorites", MenuUrl = favoriteMenuTemplate.MenuUrl});

            var fileMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000013", "파일",
                                                                                               "/Products/File/FileList.aspx");
            fileMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "파일", MenuUrl = fileMenuTemplate.MenuUrl});
            fileMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Files", MenuUrl = fileMenuTemplate.MenuUrl});

            var userConfigMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000014", "사용자환경",
                                                                                                     "/Products/UserConfig/UserConfigList.aspx");
            userConfigMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "사용자환경", MenuUrl = userConfigMenuTemplate.MenuUrl});
            userConfigMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "UserConfigs", MenuUrl = userConfigMenuTemplate.MenuUrl});

            var masterCodeMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000015", "마스터코드", "/Products/MasterCode/MasterCodeList.aspx");
            masterCodeMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "마스터코드", MenuUrl = masterCodeMenuTemplate.MenuUrl});
            masterCodeMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "MasterCodes", MenuUrl = masterCodeMenuTemplate.MenuUrl});

            var calendarMenuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(adminProduct, "TPL_ADMIN_M00000016", "달력", "/Calendars/Calendar/CalendarView.aspx");
            calendarMenuTemplate.AddLocale(SampleData.Korean, new MenuTemplateLocale {Name = "달력", MenuUrl = calendarMenuTemplate.MenuUrl});
            calendarMenuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale {Name = "Calendar", MenuUrl = calendarMenuTemplate.MenuUrl});

            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(departmentMenuTemplate, organizationMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(groupMenuTemplate, organizationMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(userMenuTemplate, organizationMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(codeMenuTemplate, organizationMainMenuTemplate);

            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(productCodeMenuTemplate, productMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(menuTemplateMenuTemplate, productMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(menuMenuTemplate, productMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(resourceMenuTemplate, productMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(favoriteMenuTemplate, productMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(fileMenuTemplate, productMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(userConfigMenuTemplate, productMainMenuTemplate);
            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(masterCodeMenuTemplate, productMainMenuTemplate);

            NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(calendarMenuTemplate, calendarMainMenuTemplate);

            Repository<MenuTemplate>.SaveOrUpdate(organizationMainMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(productMainMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(calendarMainMenuTemplate);

            Repository<MenuTemplate>.SaveOrUpdate(departmentMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(groupMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(userMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(codeMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(productCodeMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(menuTemplateMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(menuMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(resourceMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(favoriteMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(fileMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(userConfigMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(masterCodeMenuTemplate);
            Repository<MenuTemplate>.SaveOrUpdate(calendarMenuTemplate);
        }

        protected override void CreateMenu()
        {
            //base.CreateMenu();

            var orders = new Order[]
                         {
                             Order.Asc("Product"),
                             Order.Asc("NodePosition.Level"),
                             Order.Asc("NodePosition.Order")
                         };

            foreach(var menuTempate in Repository<MenuTemplate>.FindAll(orders))
            {
                var menu = NAccessContext.Domains.ProductRepository.CreateMenu(menuTempate, menuTempate.Code.Replace("TPL_ADMIN_", "ADMIN_"));

                if(menuTempate.Parent != null)
                {
                    var parent = NAccessContext.Domains.ProductRepository.FindAllMenuByMenuTemplate(menuTempate.Parent).FirstOrDefault();

                    if(parent != null)
                        NAccessContext.Domains.ProductRepository.ChangeMenuParent(menu, parent);
                }

                UnitOfWork.Current.TransactionalFlush();
            }
        }
    }
}