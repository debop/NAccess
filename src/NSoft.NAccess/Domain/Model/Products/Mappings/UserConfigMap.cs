using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model
{
    public class UserConfigMap : ClassMap<UserConfig>
    {
        public UserConfigMap()
        {
            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            CompositeId(x => x.Id)
                .KeyProperty(u => u.ProductCode)
                .KeyProperty(u => u.CompanyCode)
                .KeyProperty(u => u.UserCode)
                .KeyProperty(u => u.Key, "ConfigKey".AsNamingText());

            Map(x => x.Value).Length(MappingContext.MaxStringLength);
            Map(x => x.DefaultValue).Length(MappingContext.MaxStringLength);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }
    }

    public class UserConfigIdentityMap : ComponentMap<UserConfigIdentity>
    {
        public UserConfigIdentityMap()
        {
            Map(x => x.ProductCode).CustomType("AnsiString").Length(128);
            Map(x => x.CompanyCode).CustomType("AnsiString").Length(128);
            Map(x => x.UserCode).CustomType("AnsiString").Length(128);
            Map(x => x.Key).Column("ConfigKey".AsNamingText());
        }
    }
}