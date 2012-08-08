using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("`User`".AsNamingText());

            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            References(x => x.Company).Not.Nullable().LazyLoad().Fetch.Select();

            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.EName);

            Map(x => x.EmpNo).Length(50).Not.Nullable().Index("IX_USER_EMP_NO");
            Map(x => x.Kind);

            HasMany(x => x.Members)
                .Cascade.AllDeleteOrphan()
                .Inverse()
                .LazyLoad()
                .AsSet();

            References(x => x.Position).LazyLoad().Fetch.Select();
            References(x => x.Grade).LazyLoad().Fetch.Select();

            Map(x => x.LoginId).Length(128).Index("IX_USER_LOGIN");
            Map(x => x.Password).Length(128).Index("IX_USER_LOGIN");

            Map(x => x.IdentityNumber).Length(50);
            Map(x => x.RoleCode).Length(50);

            Map(x => x.Email).CustomType("AnsiString").Length(50);
            Map(x => x.Telephone).CustomType("AnsiString").Length(50);
            Map(x => x.MobilePhone).CustomType("AnsiString").Length(50);

            Map(x => x.IsActive);

            Map(x => x.StatusFlag).CustomType("AnsiString");

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<UserLocale>(x => x.LocaleMap)
                .Table("UserLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Column("UserName".AsNamingText()).Not.Nullable().Length(50);
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("UserMeta".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText()).Length(1024);
                           });
        }

        public override NaturalIdPart<User> NaturalId()
        {
            return base.NaturalId()
                .Reference(x => x.Company)
                .Property(x => x.Code);
        }
    }
}