using System.Linq;
using FluentNHibernate.Conventions;
using NSoft.NFramework.Data.NHibernateEx;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NSoft.NAccess.Domain.Model.Products
{
    public class ProductSampleModelBuilder : SampleModelBuilderBase
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

        protected virtual void CreateProduct()
        {
            foreach(var code in SampleData.ProductCodes)
            {
                var product = NAccessContext.Domains.ProductRepository.GetOrCreateProduct(code);

                product.AddLocale(SampleData.English, new ProductLocale() {Name = product.Name});
                product.AddMetadata("제작사", "리얼웹");

                UnitOfWork.CurrentSession.SaveOrUpdate(product);
            }
        }

        protected virtual void CreateMasterCode()
        {
            var products = NAccessContext.Domains.ProductRepository.FindAllActiveProduct();

            foreach(var product in products)
            {
                foreach(var masterCodeCode in SampleData.GetCodes("MASTER_CODE_", SampleData.AvgSampleCount))
                {
                    var code = new MasterCode(product, masterCodeCode, masterCodeCode)
                               {
                                   Description = "MasterCode " + masterCodeCode
                               };

                    int viewOrder = 0;

                    foreach(var itemCode in SampleData.GetCodes("CODE_ITEM_CODE_", SampleData.MaxSampleCount))
                    {
                        var item = new MasterCodeItem(code, itemCode, itemCode, itemCode)
                                   {
                                       Description = "MasterCodeItem " + itemCode,
                                       ViewOrder = viewOrder
                                   };

                        code.Items.Add(item);

                        //Console.WriteLine("Item[{0}] HashCode=[{1}]", item.Code, item.GetHashCode());

                        viewOrder++;
                    }

                    Assert.AreEqual(viewOrder, code.Items.Count);

                    Repository<MasterCode>.SaveOrUpdate(code);
                }
            }

            UnitOfWork.Current.TransactionalFlush();
        }

        protected virtual void CreateMenuTemplate()
        {
            var products = NAccessContext.Domains.ProductRepository.FindAllActiveProduct();

            foreach(var product in products)
            {
                MenuTemplate parent = null;
                foreach(var code in SampleData.GetCodes("MENU_TEMPLATE_", SampleData.AvgSampleCount))
                {
                    var menuTemplate = NAccessContext.Domains.ProductRepository.CreateMenuTemplate(product, code, code, @"http://www.realweb21.com/menu.aspx?Code=" + code);

                    menuTemplate.AddLocale(SampleData.English, new MenuTemplateLocale()
                                                               {
                                                                   Name = menuTemplate.Name,
                                                                   MenuUrl = @"http://www.realweb21.com/menu.aspx?LocaleKey=en-US&Code=" + menuTemplate.Code
                                                               });

                    menuTemplate.AddMetadata("관리자", "매니저 of " + product.Name);
                    menuTemplate.AddMetadata("개발", "개발자 of " + code);

                    if(parent != null)
                        NAccessContext.Domains.ProductRepository.ChangeMenuTemplateParent(menuTemplate, parent);

                    Repository<MenuTemplate>.SaveOrUpdate(menuTemplate);

                    parent = menuTemplate;
                }
            }
        }

        protected virtual void CreateMenu()
        {
            var orders = new Order[]
                         {
                             Order.Asc("Product"),
                             Order.Asc("NodePosition.Level"),
                             Order.Asc("NodePosition.Order")
                         };

            foreach(var menuTempate in Repository<MenuTemplate>.FindAll(orders))
            {
                var menu = NAccessContext.Domains.ProductRepository.CreateMenu(menuTempate, menuTempate.Code.Replace("TEMPLATE_", string.Empty));

                if(menuTempate.Parent != null)
                {
                    var parent = NAccessContext.Domains.ProductRepository.FindAllMenuByMenuTemplate(menuTempate.Parent).FirstOrDefault();

                    if(parent != null)
                        NAccessContext.Domains.ProductRepository.ChangeMenuParent(menu, parent);
                }

                UnitOfWork.Current.TransactionalFlush();
            }
        }

        protected virtual void CreateResource()
        {
            var products = NAccessContext.Domains.ProductRepository.FindAllActiveProduct();

            foreach(var product in products)
                foreach(var code in SampleData.GetCodes("RESOURCE_", SampleData.AvgSampleCount))
                {
                    var resource = NAccessContext.Domains.ProductRepository.GetOrCreateResource(product, code);
                    resource.ExAttr = "확장 속성입니다. Metadata와는 다릅니다.";
                    Repository<Resource>.SaveOrUpdate(resource);
                }
        }

        protected virtual void CreateResourceActor()
        {
            foreach(var resource in Repository<Resource>.FindAll())
                foreach(var company in Repository<Company>.FindAll())
                    foreach(var instanceId in SampleData.GetCodes("RES_INSTANCE_ID_", SampleData.AvgSampleCount))
                    {
                        NAccessContext.Domains.ProductRepository
                            //.CreateResourceActor(resource, instanceId, company.Code, company.Code, ActorKinds.Company, AuthorityKinds.All);
                            .InsertResourceActor(resource,
                                                 instanceId,
                                                 company.Code,
                                                 company.Code,
                                                 ActorKinds.Company, AuthorityKinds.All);
                    }

            foreach(var resource in Repository<Resource>.FindAll())
                foreach(var department in Repository<Department>.FindAll())
                    foreach(var instanceId in SampleData.GetCodes("RES_INSTANCE_ID_", SampleData.AvgSampleCount))
                    {
                        NAccessContext.Domains.ProductRepository
                            //.CreateResourceActor(resource, instanceId, department.Company.Code, department.Code, ActorKinds.Department, AuthorityKinds.Edit | AuthorityKinds.Delete);
                            .InsertResourceActor(resource,
                                                 instanceId,
                                                 department.Company.Code,
                                                 department.Code,
                                                 ActorKinds.Department,
                                                 AuthorityKinds.Edit | AuthorityKinds.Delete);
                    }

            foreach(var resource in NAccessContext.Linq.Resources)
                foreach(var user in NAccessContext.Linq.Users)
                    foreach(var instanceId in SampleData.GetCodes("RES_INSTANCE_ID_", SampleData.AvgSampleCount))
                    {
                        NAccessContext.Domains.ProductRepository
                            // .CreateResourceActor(resource, instanceId, user.Company.Code, user.Code, ActorKinds.User, AuthorityKinds.Edit);
                            .InsertResourceActor(resource,
                                                 instanceId,
                                                 user.Company.Code,
                                                 user.Code,
                                                 ActorKinds.User,
                                                 AuthorityKinds.Edit);
                    }
        }

        public virtual void CreateFavorite()
        {
            foreach(var product in NAccessContext.Domains.ProductRepository.FindAllActiveProduct())
            {
                var menus = NAccessContext.Domains.ProductRepository.FindAllMenuByProduct(product);

                if(menus.IsEmpty())
                    continue;

                foreach(var company in Repository<Company>.FindAll())
                    NAccessContext.Domains.ProductRepository.CreateFavorite(product,
                                                                            company,
                                                                            company.Code,
                                                                            ActorKinds.Company,
                                                                            menus.ElementAt(RandomGenerator.Next(0, menus.Count)).Code);
                foreach(var department in Repository<Department>.FindAll())
                    NAccessContext.Domains.ProductRepository.CreateFavorite(product,
                                                                            department.Company,
                                                                            department.Code,
                                                                            ActorKinds.Department,
                                                                            menus.ElementAt(RandomGenerator.Next(0, menus.Count)).Code);

                foreach(var user in Repository<User>.FindAll())
                    NAccessContext.Domains.ProductRepository.CreateFavorite(product,
                                                                            user.Company,
                                                                            user.Code,
                                                                            ActorKinds.User,
                                                                            menus.ElementAt(RandomGenerator.Next(0, menus.Count)).Code);
            }
        }

        public virtual void CreateFileMapping()
        {
            foreach(var product in NAccessContext.Domains.ProductRepository.FindAllActiveProduct())
                foreach(var systemId in SampleData.GetCodes("FILE_MAP_SYS_", SampleData.MinSampleCount))
                    foreach(var subId in SampleData.GetCodes("FILE_MAP_SUB_", SampleData.MinSampleCount))
                    {
                        var fileMapping = NAccessContext.Domains.ProductRepository.CreateFileMapping(product.Code, systemId, subId);
                        fileMapping.Key1 = "Key1";
                        fileMapping.Key2 = "Key2";

                        Repository<FileMapping>.SaveOrUpdate(fileMapping);
                    }
        }

        public virtual void CreateFile()
        {
            // TODO: 파일 생성
        }

        public virtual void CreateUserConfig()
        {
            foreach(var product in NAccessContext.Linq.Products.Where(p => p.IsActive == true))
                foreach(var user in NAccessContext.Linq.Users)
                    foreach(var key in SampleData.GetCodes("USER_KEY_", SampleData.MinSampleCount))
                    {
                        var userConfig = NAccessContext.Domains.ProductRepository.GetOrCreateUserConfig(product, user, key);
                    }
        }
    }
}