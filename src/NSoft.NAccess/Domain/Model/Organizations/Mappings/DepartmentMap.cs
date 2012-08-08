using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class DepartmentMap : ClassMap<Department>
    {
        public DepartmentMap()
        {
            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            References(x => x.Company).LazyLoad().Fetch.Select().Not.Nullable();

            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.EName);
            Map(x => x.Kind).CustomType("AnsiString").Length(128);

            HasMany(x => x.Members)
                .Inverse()
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .AsSet();

            References(x => x.Parent).Fetch.Select().LazyLoad();

            HasMany(x => x.Children)
                .Inverse()
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .AsSet();

            Component<TreeNodePosition>(x => x.NodePosition,
                                        p =>
                                        {
                                            p.Map(x => x.Order, "TreeOrder".AsNamingText());
                                            p.Map(x => x.Level, "TreeLevel".AsNamingText());
                                        });

            Map(x => x.IsActive);
            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<DepartmentLocale>(x => x.LocaleMap)
                .Table("DepartmentLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .Fetch.Select()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Column("DepartmentName".AsNamingText());
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("DepartmentMeta".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText()).Length(1024);
                           });
        }

        public override NaturalIdPart<Department> NaturalId()
        {
            return base.NaturalId()
                .Reference(x => x.Company)
                .Property(x => x.Code);
        }
    }
}