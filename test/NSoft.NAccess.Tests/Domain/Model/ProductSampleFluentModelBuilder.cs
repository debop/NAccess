using System.Globalization;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NAccess.Domain.Model.Products;

namespace NSoft.NAccess.Domain.Model
{
    public class ProductSampleFluentModelBuilder : ProductSampleModelBuilder
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public new void CreateSampleModels()
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
        }

        protected new void CreateProduct()
        {
            var product = new Product("PRT") {Name = "Product", Description = "설명입니다."};
            product.AddMetadata("a", new MetadataValue("A"));
            product.AddMetadata("b", new MetadataValue("B"));
            product.AddLocale(new CultureInfo("en"), new ProductLocale {Name = "Product"});
            product.AddLocale(new CultureInfo("ko"), new ProductLocale {Name = "제품"});

            Repository<Product>.SaveOrUpdate(product);
        }

        protected new void CreateMasterCode()
        {
            var product = Repository<Product>.FindFirst();

            var masterCode = new MasterCode(product, "MCODE", "마스터코드1") {Name = "MCODE", Description = "설명입니다."};
            masterCode.AddLocale(new CultureInfo("en"), new MasterCodeLocale {Name = "MCODE"});
            masterCode.AddLocale(new CultureInfo("ko"), new MasterCodeLocale {Name = "마스터코드"});

            Repository<MasterCode>.SaveOrUpdate(masterCode);
        }

        protected new void CreateMenuTemplate()
        {
            var product = Repository<Product>.FindFirst();

            var menuTemplate = new MenuTemplate(product, "MENU_TEMPLATE") {Name = "메뉴템플릿", Description = "설명입니다."};
            menuTemplate.AddLocale(new CultureInfo("en"), new MenuTemplateLocale {Name = "MENU_TEMPLATE"});
            menuTemplate.AddLocale(new CultureInfo("ko"), new MenuTemplateLocale {Name = "메뉴템플릿"});

            Repository<MenuTemplate>.SaveOrUpdate(menuTemplate);
        }

        protected new void CreateMenu()
        {
            var menuTemplate = Repository<MenuTemplate>.FindFirst();

            var menu = new Menu(menuTemplate, "MENU") {Description = "설명입니다."};

            Repository<Menu>.SaveOrUpdate(menu);
        }

        protected new void CreateResource()
        {
            var product = Repository<Product>.FindFirst();

            var resource = new Resource(product.Code, "RES");

            Repository<Resource>.SaveOrUpdate(resource);
        }
    }
}