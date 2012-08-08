using System.Globalization;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NAccess.Domain.Model.Calendars;

namespace NSoft.NAccess.Domain.Model
{
    public class CalendarSampleFluentModelBuilder : CalendarSampleModelBuilder
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public new void CreateSampleModels()
        {
            var calendar = new Calendar("CAL_0000");
            calendar.AddMetadata("a", new MetadataValue("A"));
            calendar.AddMetadata("b", new MetadataValue("B"));
            calendar.AddLocale(new CultureInfo("en"), new CalendarLocale {Name = "Calendar"});
            calendar.AddLocale(new CultureInfo("ko"), new CalendarLocale {Name = "달력"});
            Repository<Calendar>.SaveOrUpdate(calendar);

            var calendarRule = new CalendarRule(calendar, "테스트규칙");
            Repository<CalendarRule>.SaveOrUpdate(calendarRule);
        }
    }
}