using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class WorkTimeByTimeBaseMap : ClassMap<WorkTimeByTimeBase>
    {
        public WorkTimeByTimeBaseMap()
        {
            UseUnionSubclassForInheritanceMapping();

            DynamicInsert();
            DynamicUpdate();

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.CalendarCode).CustomType("AnsiString").Length(128).Not.Nullable();
            Map(x => x.WorkTime).Not.Nullable();

            Map(x => x.IsWork).Not.Nullable();
            Map(x => x.WorkInMinute);
            Map(x => x.CumulatedInMinute);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }

        public override NaturalIdPart<WorkTimeByTimeBase> NaturalId()
        {
            return base.NaturalId()
                .Property(x => x.CalendarCode)
                .Property(x => x.WorkTime);
        }
    }

    public class WorkTimeByDayMap : SubclassMap<WorkTimeByDay>
    {
        public WorkTimeByDayMap()
        {
        }
    }

    public class WorkTimeByHourMap : SubclassMap<WorkTimeByHour>
    {
        public WorkTimeByHourMap()
        {
        }
    }

    public class WorkTimeByMinuteMap : SubclassMap<WorkTimeByMinute>
    {
        public WorkTimeByMinuteMap()
        {
        }
    }

    public class WorkTimeByRangeMap : SubclassMap<WorkTimeByRange>
    {
        public WorkTimeByRangeMap()
        {
            Map(x => x.TimePeriod)
                .CustomType<TimeRangeUserType>()
                .Access.CamelCaseField(Prefix.Underscore)
                .Columns.Clear()
                .Columns.Add("WorkTime1".AsNamingText())
                .Columns.Add("WorkTime2".AsNamingText());
        }
    }
}