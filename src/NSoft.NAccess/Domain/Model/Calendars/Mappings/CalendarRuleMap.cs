using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class CalendarRuleMap : ClassMap<CalendarRule>
    {
        public CalendarRuleMap()
        {
            Id(x => x.Id).GeneratedBy.Native();

            References(x => x.Calendar).Fetch.Select().Not.Nullable().Index("IX_CalRule_Cal".AsNamingText());

            Map(x => x.Name, "CAL_RULE_NAME");

            Map(x => x.DayOrException);
            Map(x => x.ExceptionType);
            Map(x => x.ExceptionPattern).CustomType("AnsiString").Length(1024);
            Map(x => x.ExceptionClassName).CustomType("AnsiString").Length(1024);

            Map(x => x.IsWorking).Not.Nullable();

            Map(x => x.RulePeriod).CustomType<TimeRangeUserType>()
                .Index("IX_CalRule_Cal")
                .Columns.Clear()
                .Columns.Add("FromTime".AsNamingText())
                .Columns.Add("ToTime".AsNamingText());

            Map(x => x.RulePeriod1).CustomType<TimeRangeUserType>()
                .Columns.Clear()
                .Columns.Add("FromTime1".AsNamingText())
                .Columns.Add("ToTime1".AsNamingText());
            Map(x => x.RulePeriod2).CustomType<TimeRangeUserType>()
                .Columns.Clear()
                .Columns.Add("FromTime2".AsNamingText())
                .Columns.Add("ToTime2".AsNamingText());
            Map(x => x.RulePeriod3).CustomType<TimeRangeUserType>()
                .Columns.Clear()
                .Columns.Add("FromTime3".AsNamingText())
                .Columns.Add("ToTime3".AsNamingText());
            Map(x => x.RulePeriod4).CustomType<TimeRangeUserType>()
                .Columns.Clear()
                .Columns.Add("FromTime4".AsNamingText())
                .Columns.Add("ToTime4".AsNamingText());
            Map(x => x.RulePeriod5).CustomType<TimeRangeUserType>()
                .Columns.Clear()
                .Columns.Add("FromTime5".AsNamingText())
                .Columns.Add("ToTime5".AsNamingText());

            Map(x => x.ViewOrder);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<CalendarRuleLocale>(x => x.LocaleMap)
                .Table("CALENDAR_RULE_LOC")
                .Access.CamelCaseField(Prefix.Underscore)
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .Fetch.Select()
                .KeyColumn("CAL_RULE_ID")
                .ForeignKeyConstraintName("FK_CAL_RULE_LOC_CAL_RULE")
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Not.Nullable();
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("CALENDAR_RULE_META")
                .Access.CamelCaseField(Prefix.Underscore)
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .KeyColumn("CAL_RULE_ID")
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText()).Length(1024);
                           });
        }
    }
}