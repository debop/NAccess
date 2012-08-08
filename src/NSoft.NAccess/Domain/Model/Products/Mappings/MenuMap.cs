using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    public class MenuMap : ClassMap<Menu>
    {
        public MenuMap()
        {
            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();

            References(x => x.MenuTemplate).Fetch.Select().LazyLoad().Not.Nullable();

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

            Map(x => x.IsActive);
            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }

        public override NaturalIdPart<Menu> NaturalId()
        {
            return base.NaturalId()
                .Property(x => x.Code)
                .Reference(x => x.MenuTemplate);
        }
    }
}