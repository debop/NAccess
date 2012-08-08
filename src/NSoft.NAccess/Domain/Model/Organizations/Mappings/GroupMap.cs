using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class GroupMap : ClassMap<Group>
    {
        public GroupMap()
        {
            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            References(x => x.Company).LazyLoad().Fetch.Select();
            Map(x => x.Code).CustomType("AnsiString").Not.Nullable();

            Map(x => x.Name).Not.Nullable();
            Map(x => x.Kind).CustomType<GroupKinds>();

            Map(x => x.IsActive);

            Map(x => x.SqlStatement).Length(MappingContext.MaxStringLength);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<GroupLocale>(x => x.LocaleMap)
                .Table("GroupLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Column("GroupName".AsNamingText());
                               m.Map(x => x.SqlStatement).Column("SqlStatement".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("GroupMeta".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText()).Length(1024);
                           });
        }

        public override NaturalIdPart<Group> NaturalId()
        {
            return base.NaturalId()
                .Reference(x => x.Company)
                .Property(x => x.Code);
        }
    }
}