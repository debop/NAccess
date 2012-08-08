using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model
{
    public class UserLoginLogMap : ClassMap<UserLoginLog>
    {
        public UserLoginLogMap()
        {
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.ProductCode).CustomType("AnsiString").Not.Nullable().Length(128);
            Map(x => x.CompanyCode).CustomType("AnsiString").Not.Nullable().Length(128);
            Map(x => x.DepartmentCode).CustomType("AnsiString").Length(128);
            Map(x => x.UserCode).CustomType("AnsiString").Not.Nullable().Length(128);

            Map(x => x.LoginId).Not.Nullable().Length(50);
            Map(x => x.LoginTime).CustomType("Timestamp").Not.Nullable();
            Map(x => x.LocaleKey).CustomType("AnsiString").Length(64);

            Map(x => x.ProductName);
            Map(x => x.CompanyName);
            Map(x => x.DepartmentName);
            Map(x => x.UserName);

            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
        }
    }
}