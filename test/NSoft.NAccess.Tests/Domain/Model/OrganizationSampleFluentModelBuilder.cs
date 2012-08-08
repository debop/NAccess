using System.Globalization;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NAccess.Domain.Model.Organizations;

namespace NSoft.NAccess.Domain.Model
{
    public class OrganizationSampleFluentModelBuilder : OrganizationSampleModelBuilder
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public new void CreateSampleModels()
        {
            CreateCompany();
            UnitOfWork.Current.TransactionalFlush();

            CreateGroup();
            UnitOfWork.Current.TransactionalFlush();

            CreateUser();
            UnitOfWork.Current.TransactionalFlush();
        }

        protected new void CreateCompany()
        {
            var company = new Company("TEST") {Name = "Test", Description = "설명입니다"};
            company.AddMetadata("a", new MetadataValue("A"));
            company.AddMetadata("b", new MetadataValue("B"));
            company.AddLocale(new CultureInfo("en"), new CompanyLocale {Name = "TEST"});
            company.AddLocale(new CultureInfo("ko"), new CompanyLocale {Name = "테스트"});

            Repository<Company>.SaveOrUpdate(company);
        }

        protected new void CreateGroup()
        {
            var company = Repository<Company>.FindFirst();

            var group = new Group(company, "GROUP") {Name = "Group", Description = "설명입니다"};
            company.AddMetadata("a", new MetadataValue("A"));
            company.AddMetadata("b", new MetadataValue("B"));
            company.AddLocale(new CultureInfo("en"), new CompanyLocale {Name = "GROUP"});
            company.AddLocale(new CultureInfo("ko"), new CompanyLocale {Name = "그룹"});

            Repository<Group>.SaveOrUpdate(group);
        }

        protected override void CreateUser()
        {
            var company = Repository<Company>.FindFirst();

            var user = new User(company, "USER_0000");

            Repository<User>.SaveOrUpdate(user);
        }

        protected new void CreateGroupActor() {}
    }
}