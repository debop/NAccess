using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class CalendarRuleOfUserMap : ClassMap<CalendarRuleOfUser>
    {
        public CalendarRuleOfUserMap()
        {
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.CompanyCode).Length(50).Not.Nullable().Index("IX_CAL_USER_RULE_USER");
            Map(x => x.UserCode).Length(50).Not.Nullable().Index("IX_CAL_USER_RULE_USER");

            References(x => x.CalendarRule).Fetch.Select().LazyLoad().Not.Nullable().Index("IX_CAL_USER_RULE_USER");

            Map(x => x.DayOrException).Default("0");
            Map(x => x.ExceptionType);
            Map(x => x.ExceptionPattern).CustomType("AnsiString").Length(1024);
            Map(x => x.ExceptionClassName).CustomType("AnsiString").Length(1024);

            Map(x => x.IsWorking, "IS_WORKING").Not.Nullable();

            Map(x => x.RulePeriod).CustomType<TimeRangeUserType>()
                .Index("IX_CAL_USER_RULE_USER")
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

            Map(x => x.IsActive);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }
    }
}