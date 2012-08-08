using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class CalendarMap : ClassMap<Calendar>
    {
        public CalendarMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.Code).CustomType("AnsiString").Not.Nullable().Length(128);
            Map(x => x.Name).Not.Nullable();

            Map(x => x.ProjectId).CustomType("AnsiString").Length(50);
            Map(x => x.ResourceId).CustomType("AnsiString").Length(50);
            Map(x => x.TimeZone).CustomType("AnsiString").Length(50);

            References(x => x.Parent)
                .Column("ParentId".AsNamingText())
                .LazyLoad()
                .Fetch.Select();

            HasMany(x => x.Children)
                .KeyColumn("ParentId".AsNamingText())
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

            HasMany(x => x.Rules)
                .Cascade.AllDeleteOrphan()
                .Inverse()
                .LazyLoad()
                .AsSet();

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<CalendarLocale>(x => x.LocaleMap)
                .Table("CalendarLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .Fetch.Select()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Column("CalendarName".AsNamingText()).Not.Nullable();
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("CALENDAR_META")
                .Access.CamelCaseField(Prefix.Underscore)
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .KeyColumn("CAL_ID")
                .ForeignKeyConstraintName("FK_CAL_META_CAL")
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText());
                           });
        }

        public override NaturalIdPart<Calendar> NaturalId()
        {
            return base.NaturalId()
                .Property(x => x.CompanyCode)
                .Property(x => x.Code);
        }
    }
}