using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.TimePeriods;
using NUnit.Framework;

namespace NSoft.NAccess.Domain.Model.Calendars
{
    public class CalendarSampleModelBuilder : SampleModelBuilderBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public static readonly DateTime[] HolyDays = new DateTime[]
                                                     {
                                                         new DateTime(2010, 1, 1),
                                                         new DateTime(2010, 3, 1),
                                                         new DateTime(2010, 5, 5),
                                                         new DateTime(2010, 6, 6),
                                                         new DateTime(2010, 8, 15),
                                                         new DateTime(2010, 12, 25)
                                                     };

        public override void CreateSampleModels()
        {
            if(log.IsTraceEnabled)
                log.Trace(@"Calendar 정보를 생성합니다.");

            CreateStandardCalendar(Calendar.StandardCalendarCode, null);
            CreateStandardCalendar(NAccessContext.Current.CompanyCode, null);

            CreateDrivedCalendar();

            UnitOfWork.Current.TransactionalFlush();

            if(log.IsTraceEnabled)
                log.Trace(@"Calendar 샘플 정보 생성을 완료했습니다.");
        }

        protected void CreateStandardCalendar(string calendarCode, string projectId)
        {
            calendarCode.ShouldNotBeEmpty("calendarCode");

            var stdCalendar = NAccessContext.Domains.CalendarRepository.GetOrCreateCalendar(calendarCode, projectId);

            stdCalendar.Description = calendarCode + "입니다";
            stdCalendar.Metadatas["사용처"].Value = "기본";

            // 주(Week) 단위로 Calendar Rule을 만듭니다. 주말은 휴일이고, 주중은 근무일입니다. 
            foreach(DayOfWeek dow in Enum.GetValues(typeof(DayOfWeek)))
            {
                bool isWorking = (dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday);
                var rule = CreateCalendarRule(stdCalendar, dow, isWorking.GetHashCode(), TimeRange.Anytime);
                rule.ViewOrder = rule.DayOrException;

                NAccessContext.Domains.CalendarRepository.AddCalendarRule(stdCalendar, rule);
            }

            // 공휴일 추가
            AddHolyDays(stdCalendar);

            Repository<Calendar>.SaveOrUpdate(stdCalendar);
        }

        protected void CreateDrivedCalendar()
        {
            var baseCalendar = NAccessContext.Domains.CalendarRepository.FindOneCalendarByCode(NAccessContext.Current.CompanyCode, null);
            Assert.IsNotNull(baseCalendar, NAccessContext.Current.CompanyCode + " 코드의 Calendar 정보가 없습니다.");

            var rndCalendar = NAccessContext.Domains.CalendarRepository.GetOrCreateCalendar("R&D OF " + NAccessContext.Current.CompanyCode, null);

            rndCalendar.Parent = baseCalendar;
            rndCalendar.Description = "개발본부 Calendar입니다.";
            rndCalendar.Metadatas["사용처"].Value = "테스트용";

            var rule = CreateCalendarRule(rndCalendar, DayOfWeek.Saturday, 1, TimeRange.Anytime);

            Repository<Calendar>.SaveOrUpdate(rndCalendar);

            UnitOfWork.Current.TransactionalFlush();
        }

        private static CalendarRule CreateCalendarRule(Calendar calendar, DayOfWeek dow, int? isWorking, ITimePeriod range)
        {
            var rule = NAccessContext.Domains.CalendarRepository.CreateCalendarRule(calendar, null, dow, isWorking, range);

            if(isWorking.GetValueOrDefault(0) > 0)
            {
                var baseDate = new DateTime(2000, 1, 1).Add(TimeSpan.FromHours(9));

                rule.RulePeriods[0].Setup(baseDate, baseDate.AddHours(3));
                rule.RulePeriods[1].Setup(baseDate.AddHours(4), baseDate.AddHours(8));
            }

            return rule;
        }

        private static void AddHolyDays(Calendar calendar)
        {
            foreach(var holyday in HolyDays)
            {
                var rule = NAccessContext.Domains.CalendarRepository.CreateHolyDayCalendarRule(calendar, holyday);

                if(log.IsDebugEnabled)
                    log.Debug(@"휴일관련 Rule 생성. rule=" + rule);
            }
        }
    }
}