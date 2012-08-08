using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class MenuTemplateMap : ClassMap<MenuTemplate>
    {
        public MenuTemplateMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            References(x => x.Product).Fetch.Select().LazyLoad().Not.Nullable();
            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();

            Map(x => x.Name).Length(128).Not.Nullable();
            Map(x => x.MenuUrl).Length(MappingContext.MaxStringLength);

            References(x => x.Parent).Fetch.Select().LazyLoad();

            HasMany(x => x.Children)
                                .Cascade.AllDeleteOrphan()
                .Inverse()
                .LazyLoad()
                .AsSet();

            Component<TreeNodePosition>(x => x.NodePosition,
                                        p =>
                                        {
                                            p.Map(x => x.Order, "TreeOrder".AsNamingText());
                                            p.Map(x => x.Level, "TreeLevel".AsNamingText());
                                        });

            Map(x => x.TreePath).CustomType("AnsiString").Length(MappingContext.MaxAnsiStringLength);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<MenuTemplateLocale>(x => x.LocaleMap)
                .Table("MenuTemplateLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Not.Nullable();
                               m.Map(x => x.MenuUrl).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("MenuTemplateMeta".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText()).Length(1024);
                           });
        }

        public override NaturalIdPart<MenuTemplate> NaturalId()
        {
            return base.NaturalId()
                .Reference(x => x.Product)
                .Property(x => x.Code);
        }
    }
}