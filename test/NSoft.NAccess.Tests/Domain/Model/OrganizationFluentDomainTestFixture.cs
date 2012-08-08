using System;
using System.Collections.Generic;
using System.Globalization;
using FluentNHibernate.Testing;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NUnit.Framework;
using SharpTestsEx;

namespace NSoft.NAccess.Domain.Model.Organizations
{
    [TestFixture]
    public class OrganizationFluentDomainTestFixture : FluentDomainTestFixtureBase
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
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            UnitOfWork.CurrentSession.Clear();
        }

        #endregion

        [Test]
        public void CompanyTestByUnitOfWork()
        {
            var company = new Company("RWC") {Name = "RealWeb", Description = "설명입니다"};
            company.AddMetadata("a", new MetadataValue("A"));
            company.AddMetadata("b", new MetadataValue("B"));
            company.AddLocale(new CultureInfo("en"), new CompanyLocale {Name = "REALWEB"});
            company.AddLocale(new CultureInfo("ko"), new CompanyLocale {Name = "리얼웹"});

            Repository<Company>.SaveOrUpdate(company);
            UnitOfWork.Current.TransactionalFlush();
            UnitOfWork.Current.Clear();

            var loaded = Repository<Company>.Get(company.Id);
            Assert.AreEqual(company, loaded);

            loaded.LocaleMap.Count.Should().Be(2);
        }

        [Test]
        public void CompanyTestByHybrid()
        {
            var company = new Company("RWC2") {Name = "RealWeb", Description = "설명입니다"};
            company.AddMetadata("a", new MetadataValue("A"));
            company.AddMetadata("b", new MetadataValue("B"));
            company.AddLocale(new CultureInfo("en"), new CompanyLocale {Name = "REALWEB"});
            company.AddLocale(new CultureInfo("ko"), new CompanyLocale {Name = "리얼웹"});

            new PersistenceSpecification<Company>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(company);
        }

        [Test]
        public void CompanyTestByFluent()
        {
            new PersistenceSpecification<Company>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Code, "RWC_F")
                .CheckProperty(x => x.Name, "RealWeb")
                .CheckProperty(x => x.Description, "설명입니다.")
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, CompanyLocale>
                                    {
                                        {new CultureInfo("en"), new CompanyLocale {Name = "REALWEB"}},
                                        {new CultureInfo("ko"), new CompanyLocale {Name = "리얼웹"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void DepartmentTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            new PersistenceSpecification<Department>(UnitOfWork.CurrentSession)
                .CheckReference(x => x.Company, company)
                .CheckProperty(x => x.Code, "DEPT_0001")
                .CheckProperty(x => x.Name, "팀")
                .CheckProperty(x => x.EName, "Team")
                .CheckProperty(x => x.Parent, null)
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, DepartmentLocale>
                                    {
                                        {new CultureInfo("en"), new DepartmentLocale {Name = "Team"}},
                                        {new CultureInfo("ko"), new DepartmentLocale {Name = "팀"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void GroupTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            new PersistenceSpecification<Group>(UnitOfWork.CurrentSession)
                .CheckReference(x => x.Company, company)
                .CheckProperty(x => x.Code, "GRP_0001")
                .CheckProperty(x => x.Name, "시스템그룹1")
                .CheckProperty(x => x.Kind, GroupKinds.System)
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, GroupLocale>
                                    {
                                        {new CultureInfo("en"), new GroupLocale {Name = "System Group1"}},
                                        {new CultureInfo("ko"), new GroupLocale {Name = "시스템그룹1"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void EmployeePositionTestByHybrid()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var employeePosition = new EmployeePosition(company, "POS_0001");
            employeePosition.AddLocale(new CultureInfo("en"), new EmployeeCodeLocale {Name = "과장"});
            employeePosition.AddLocale(new CultureInfo("ko"), new EmployeeCodeLocale {Name = "Manager"});

            new PersistenceSpecification<EmployeePosition>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(employeePosition);
        }

        [Test]
        public void EmployeeGradeTestByHybrid()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var employeeGrade = new EmployeeGrade(company, "GRD_0001");
            employeeGrade.AddLocale(new CultureInfo("en"), new EmployeeCodeLocale {Name = "1 Grade"});
            employeeGrade.AddLocale(new CultureInfo("ko"), new EmployeeCodeLocale {Name = "1급"});

            new PersistenceSpecification<EmployeeGrade>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(employeeGrade);
        }

        [Test]
        public void UserTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            new PersistenceSpecification<User>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Company, company)
                .CheckProperty(x => x.Code, "0001")
                .CheckProperty(x => x.Name, "신동규")
                .CheckProperty(x => x.EmpNo, "0001")
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, UserLocale>
                                    {
                                        {new CultureInfo("en"), new UserLocale {Name = "Dongkyu Shin"}},
                                        {new CultureInfo("ko"), new UserLocale {Name = "신동규"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void GroupActorTestByFluent()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();
            var group = Repository<Group>.FindFirst(x => x.Company == company);
            group.Should().Not.Be.Null();

            new PersistenceSpecification<GroupActor>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Id, new GroupActorIdentity(company.Code, group.Code, "ACT_0001", ActorKinds.User))
                .VerifyTheMappings();
        }

        [Test]
        public void CodeTestByHybrid()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();
            var group = Repository<Group>.FindFirst();
            group.Should().Not.Be.Null();

            var code = new Code(company.Code, group.Code, "CODE_0001", "코드_0001");
            code.AddLocale(new CultureInfo("en"), new CodeLocale {GroupName = group.Locales[new CultureInfo("en")].Name, ItemName = "CODE_0001"});
            code.AddLocale(new CultureInfo("ko"), new CodeLocale {GroupName = group.Locales[new CultureInfo("ko")].Name, ItemName = "코드_0001"});

            new PersistenceSpecification<Code>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(code);
        }
    }
}