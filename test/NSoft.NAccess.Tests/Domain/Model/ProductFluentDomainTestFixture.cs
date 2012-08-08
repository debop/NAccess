using System;
using System.Collections.Generic;
using System.Globalization;
using FluentNHibernate.Testing;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.ForTesting;
using NUnit.Framework;
using SharpTestsEx;

namespace NSoft.NAccess.Domain.Model.Products
{
    [TestFixture]
    public class ProductFluentDomainTestFixture : FluentDomainTestFixtureBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        #region << 테스트 준비 작업 >>

        protected override void OnTestFixtureSetUp()
        {
            base.OnTestFixtureSetUp();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            new OrganizationSampleFluentModelBuilder().CreateSampleModels();
            new ProductSampleFluentModelBuilder().CreateSampleModels();
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            UnitOfWork.CurrentSession.Clear();
        }

        #endregion

        [Test]
        public void ProductTestByUnitOfWork()
        {
            var product = new Product("PRT_1") {Name = "Product", Description = "설명입니다."};
            product.AddMetadata("a", new MetadataValue("A"));
            product.AddMetadata("b", new MetadataValue("B"));
            product.AddLocale(new CultureInfo("en"), new ProductLocale {Name = "Product1"});
            product.AddLocale(new CultureInfo("ko"), new ProductLocale {Name = "제품1"});

            Repository<Product>.SaveOrUpdate(product);
            UnitOfWork.Current.TransactionalFlush();
            UnitOfWork.Current.Clear();

            var loaded = Repository<Product>.Get(product.Id);
            Assert.AreEqual(product, loaded);

            loaded.LocaleMap.Count.Should().Be(2);
        }

        [Test]
        public void ProductTestByHybrid()
        {
            var product = new Product("PRT_2") {Name = "Product2", Description = "설명입니다"};
            product.AddMetadata("a", new MetadataValue("A"));
            product.AddMetadata("b", new MetadataValue("B"));
            product.AddLocale(new CultureInfo("en"), new ProductLocale {Name = "Product2"});
            product.AddLocale(new CultureInfo("ko"), new ProductLocale {Name = "제품2"});

            new PersistenceSpecification<Product>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(product);
        }

        [Test]
        public void ProductTestByFluent()
        {
            new PersistenceSpecification<Product>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Code, "PRT_F")
                .CheckProperty(x => x.Name, "Product2_F")
                .CheckProperty(x => x.Description, "설명입니다.")
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, ProductLocale>
                                    {
                                        {new CultureInfo("en"), new ProductLocale {Name = "Product2_F"}},
                                        {new CultureInfo("ko"), new ProductLocale {Name = "제품2_F"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void MasterCodeTestByFluent()
        {
            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            new PersistenceSpecification<MasterCode>(UnitOfWork.CurrentSession)
                .CheckReference(x => x.Product, product)
                .CheckProperty(x => x.Code, "MCODE_F")
                .CheckProperty(x => x.Name, "마스터코드_F")
                .CheckProperty(x => x.Description, "설명입니다.")
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, MasterCodeLocale>
                                    {
                                        {new CultureInfo("en"), new MasterCodeLocale {Name = "MCODE_F"}},
                                        {new CultureInfo("ko"), new MasterCodeLocale {Name = "마스터코드_F"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void MasterCodeItemTestByFluent()
        {
            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            var masterCode = Repository<MasterCode>.FindFirst();
            masterCode.Should().Not.Be.Null();

            new PersistenceSpecification<MasterCodeItem>(UnitOfWork.CurrentSession)
                .CheckReference(x => x.MasterCode, masterCode)
                .CheckProperty(x => x.Code, "MCODE_ITEM_F")
                .CheckProperty(x => x.Value, "MCODE_ITEM__VALUE_F")
                .CheckProperty(x => x.Name, "마스터코드아이템_F")
                .CheckProperty(x => x.Description, "설명입니다.")
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, MasterCodeItemLocale>
                                    {
                                        {new CultureInfo("en"), new MasterCodeItemLocale {Name = "MCODE_ITEM_F"}},
                                        {new CultureInfo("ko"), new MasterCodeItemLocale {Name = "마스터코드아이템_F"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void MenuTemplateTestByFluent()
        {
            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            var parentMenuTemplate = Repository<MenuTemplate>.FindFirst();
            parentMenuTemplate.Should().Not.Be.Null();

            new PersistenceSpecification<MenuTemplate>(UnitOfWork.CurrentSession)
                .CheckReference(x => x.Product, product)
                .CheckReference(x => x.Parent, parentMenuTemplate)
                .CheckProperty(x => x.Code, "MENU_TEMPLATE_F")
                .CheckProperty(x => x.Name, "메뉴템플릿_F")
                .CheckProperty(x => x.Description, "설명입니다.")
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, MenuTemplateLocale>
                                    {
                                        {new CultureInfo("en"), new MenuTemplateLocale {Name = "MENU_TEMPLATE_F"}},
                                        {new CultureInfo("ko"), new MenuTemplateLocale {Name = "메뉴템플릿_F"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void MenuTestByFluent()
        {
            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            var menuTemplate = Repository<MenuTemplate>.FindFirst();
            menuTemplate.Should().Not.Be.Null();

            new PersistenceSpecification<Menu>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Code, "MENU_F")
                .CheckProperty(x => x.MenuTemplate, menuTemplate)
                .CheckProperty(x => x.Description, "설명입니다.")
                .VerifyTheMappings();
        }

        [Test]
        public void ResourceTestByFluent()
        {
            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            new PersistenceSpecification<Resource>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.ProductCode, product.Code)
                .CheckProperty(x => x.Code, "RESOURCE_F")
                .CheckProperty(x => x.Name, "리소스_F")
                .CheckProperty(x => x.Description, "설명입니다.")
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, ResourceLocale>
                                    {
                                        {new CultureInfo("en"), new ResourceLocale {Name = "RESOURCE_F"}},
                                        {new CultureInfo("ko"), new ResourceLocale {Name = "리소스_F"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void ResourceActorTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            var resource = Repository<Resource>.FindFirst();
            resource.Should().Not.Be.Null();

            new PersistenceSpecification<ResourceActor>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Id,
                               new ResourceActorIdentity(product.Code, resource.Code, "RES_INSTANCE_ID",
                                                         company.Code, company.Code, ActorKinds.Company))
                .CheckProperty(x => x.AuthorityKind, AuthorityKinds.All)
                .VerifyTheMappings();
        }

        [Test]
        public void FavoriteTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            new PersistenceSpecification<Favorite>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.ProductCode, product.Code)
                .CheckProperty(x => x.CompanyCode, company.Code)
                .CheckProperty(x => x.OwnerCode, company.Code)
                .CheckProperty(x => x.OwnerKind, ActorKinds.Company)
                .CheckProperty(x => x.Description, "설명입니다.")
                .VerifyTheMappings();
        }

        [Test]
        public void FileTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            new PersistenceSpecification<File>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.FileName, "파일_F")
                .CheckProperty(x => x.OwnerCode, company.Code)
                .CheckProperty(x => x.OwnerKind, ActorKinds.Company)
                .VerifyTheMappings();
        }

        [Test]
        public void UseConfigTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var user = Repository<User>.FindFirst(x => x.Company == company);
            user.Should().Not.Be.Null();

            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            new PersistenceSpecification<UserConfig>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Id,
                               new UserConfigIdentity(product.Code, company.Code, user.Code, "KEY_F"))
                .VerifyTheMappings();
        }

        [Test]
        public void UserLoginLogTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var user = Repository<User>.FindFirst(x => x.Company == company);
            user.Should().Not.Be.Null();

            var product = Repository<Product>.FindFirst();
            product.Should().Not.Be.Null();

            UnitOfWork.CurrentSession.Specification<UserLoginLog>()
                .CheckProperty(x => x.ProductCode, product.Code)
                .CheckProperty(x => x.CompanyCode, company.Code)
                .CheckProperty(x => x.UserCode, user.Code)
                .CheckProperty(x => x.LoginId, user.LoginId)
                .CheckProperty(x => x.LoginTime, DateTime.Now)
                .VerifyTheMappings();
        }
    }
}