using System;
using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.TimePeriods;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// Calendar 정보 관련 Domain Service
    /// </summary>
    public partial class CalendarRepository : NAccessRepositoryBase
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        #endregion

        private readonly object _syncLock = new object();

        public CalendarRepository()
        {
            if(log.IsInfoEnabled)
                log.Info(@"Calendar Domain Service 인스턴스를 생성했습니다.");
        }

        #region << Calendar >>

        /// <summary>
        /// <see cref="Calendar"/> 조회를 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="code">Calendar 고유 코드</param>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <param name="resourceId">관련 리소스의 고유 ID</param>
        /// <param name="company">Calendar 적용 회사</param>
        /// <param name="name">Calendar 명</param>
        /// <returns></returns>
        public QueryOver<Calendar, Calendar> BuildQueryOverOfCalendar(string code, string projectId, string resourceId, Company company, string name)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"Calendar 정보 검색을 위한 Criteria를 빌드합니다... " +
                          @"code={0}, projectId={1}, resourceId={2}, company={3}, name={4}",
                          code, projectId, resourceId, company, name);

            var query = QueryOver.Of<Calendar>();

            if(code.IsNotWhiteSpace())
                query.AddWhere(c => c.Code == code);

            if(projectId.IsNotWhiteSpace())
                query.AddWhere(c => c.ProjectId == projectId);

            if(resourceId.IsNotWhiteSpace())
                query.AddWhere(c => c.ResourceId == resourceId);

            if(company != null)
                query.AddWhere(c => c.CompanyCode == company.Code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(c => c.Name == name);

            return query;
        }

        /// <summary>
        /// <see cref="Calendar"/> 조회를 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="code">Calendar 고유 코드</param>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <param name="resourceId">관련 리소스의 고유 ID</param>
        /// <param name="company">Calendar 적용 회사</param>
        /// <param name="name">Calendar 명</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfCalendar(string code, string projectId, string resourceId, Company company, string name)
        {
            return BuildQueryOverOfCalendar(code, projectId, resourceId, company, name).DetachedCriteria;
        }

        /// <summary>
        /// 해당하는 Calendar가 존재하는가?
        /// </summary>
        /// <param name="code">Calendar 고유 코드</param>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <returns></returns>
        public bool ExistsCalendar(string code, string projectId)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"지정된 Calendar 정보가 존재하는지 확인합니다... code={0}, projectId={1}", code, projectId);

            return Repository<Calendar>.Exists(BuildQueryOverOfCalendar(code, projectId, null, null, null));
        }

        /// <summary>
        /// 지정한 Id에 해당하는 Calendar를 
        /// </summary>
        /// <param name="code">Calendar 고유 코드 값</param>
        /// <param name="projectId">Project Id</param>
        /// <returns></returns>
        public Calendar GetOrCreateCalendar(string code, string projectId)
        {
            var calendar = FindOneCalendarByCode(code, projectId);

            if(calendar != null)
                return calendar;

            if(log.IsDebugEnabled)
                log.Debug(@"기존 Calendar가 없으므로, 새로운 Calendar를 생성합니다... code={0}, projectId={1}", code, projectId);

            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    Repository<Calendar>.SaveOrUpdate(new Calendar(code, projectId));
                    UnitOfWork.Current.TransactionalFlush();
                }
            }

            calendar = FindOneCalendarByCode(code, projectId);
            calendar.AssertExists("calendar");

            if(log.IsDebugEnabled)
                log.Debug(@"새로운 Calendar 생성에 성공했습니다!!! " + calendar);

            return calendar;
        }

        /// <summary>
        /// 지정된 Id를 가지는 유일한 Calenar를 반환한다.
        /// </summary>
        /// <param name="code">Calendar 고유 코드</param>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <returns></returns>
        public Calendar FindOneCalendarByCode(string code, string projectId)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"Calendar 정보를 조회합니다... code={0}, projectId={1}", code, projectId);

            return Repository<Calendar>.FindOne(BuildQueryOverOfCalendar(code, projectId, null, null, null));
        }

        /// <summary>
        /// 표준 Calendar 정보를 로드합니다.
        /// </summary>
        /// <returns></returns>
        public Calendar FindOneStandardCalendar()
        {
            if(log.IsDebugEnabled)
                log.Debug(@"Load Standard Calendar. (ProjectId와 CalendarId가 모두 -1 인 Calendar)");

            return FindOneCalendarByCode(Calendar.StandardCalendarCode, null);
        }

        /// <summary>
        /// 프로젝트용 Calendar 정보를 로드합니다.
        /// </summary>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <returns></returns>
        public Calendar FindOneProjectCalendar(string projectId)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"FindOne Project Calendar. projectId=" + projectId);

            return Repository<Calendar>.FindOne(BuildQueryOverOfCalendar(null, projectId, null, null, null));
        }

        /// <summary>
        /// 리소스용 Calendar 정보를 로드합니다.
        /// </summary>
        /// <param name="resourceId">관련 리소스의 고유 ID</param>
        /// <returns></returns>
        public Calendar FindOneResourceCalendar(string resourceId)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"FindOne Resource Calendar. resourceId=" + resourceId);

            return Repository<Calendar>.FindOne(BuildQueryOverOfCalendar(null, null, resourceId, null, null));
        }

        /// <summary>
        /// 리소스 프로젝트 Calendar 정보를 로드합니다.
        /// </summary>
        /// <param name="resourceId">관련 리소스의 고유 ID</param>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <returns></returns>
        public Calendar FindOneResourceProjectCalendar(string resourceId, string projectId)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"FindOne Resource Project Calendar. resourceId={0}, projectId={1}", resourceId, projectId);

            return Repository<Calendar>.FindOne(BuildQueryOverOfCalendar(null, projectId, resourceId, null, null));
        }

        #endregion

        #region << CalendarRule >>

        public static QueryOver<CalendarRule, CalendarRule> BuildQueryOverOfCalendarRule(string calendarCode, string projectId, string name)
        {
            var query = QueryOver.Of<CalendarRule>();

            Calendar @calendarAlias = null;

            if(calendarCode.IsNotWhiteSpace() || projectId.IsNotWhiteSpace())
                query = query.JoinAlias(r => r.Calendar, () => @calendarAlias);

            if(calendarCode.IsNotWhiteSpace())
                query.AddWhere(() => @calendarAlias.Code == calendarCode);


            if(projectId.IsNotWhiteSpace())
                query.AddWhere(() => @calendarAlias.ProjectId == projectId);


            if(name.IsNotWhiteSpace())
                query.AddWhere(r => r.Name == name);


            return query;
        }

        public static DetachedCriteria BuildCriteriaOfCalendarRule(string calendarCode, string projectId, string name)
        {
            return BuildQueryOverOfCalendarRule(calendarCode, projectId, name).DetachedCriteria;
        }

        /// <summary>
        /// <paramref name="calendar"/>의 Rules 컬렉션에 <paramref name="rule"/>을 추가합니다.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="rule"></param>
        public void AddCalendarRule(Calendar calendar, CalendarRule rule)
        {
            calendar.ShouldNotBeNull("calendar");
            rule.ShouldNotBeNull("rule");

            rule.Calendar = calendar;

            if(calendar.Rules.Contains(rule) == false)
                calendar.Rules.Add(rule);

            Session.SaveOrUpdate(calendar);
            Session.SaveOrUpdate(rule);
        }

        /// <summary>
        /// 특정 Calendar에 CalendarRule을 새로 만든다.
        /// </summary>
        /// <param name="calendar">규칙이 적용될 <see cref="Calendar"/></param>
        /// <param name="name">규칙 명</param>
        /// <returns></returns>
        public CalendarRule CreateCalendarRule(Calendar calendar, string name)
        {
            calendar.ShouldNotBeNull("calendar");
            name.ShouldNotBeWhiteSpace("name");

            if(log.IsDebugEnabled)
                log.Debug(@"새로운 CalendarRule (Working Time 규칙)을 생성합니다. calendar={0}, name={1}", calendar, name);

            var calendarRule = new CalendarRule(calendar, name);

            calendar.Rules.Add(calendarRule);
            Session.SaveOrUpdate(calendar);

            return Repository<CalendarRule>.SaveOrUpdate(calendarRule);
        }

        /// <summary>
        /// 특정 Calendar에 CalendarRule을 새로 만든다.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="name"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="isWorking"></param>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        public CalendarRule CreateCalendarRule(Calendar calendar, string name, DayOfWeek dayOfWeek, int? isWorking, ITimePeriod timePeriod)
        {
            calendar.ShouldNotBeNull("calendar");

            if(log.IsDebugEnabled)
                log.Debug(@"새로운 CalendarRule (Working Time 규칙)을 생성합니다... " +
                          @"calendar={0}, name={1}, dayOfWeek={2}, isWorking={3}, timePeriod={4}",
                          calendar, name, dayOfWeek, isWorking, timePeriod);

            var rule = new CalendarRule(calendar, name);

            if(timePeriod != null && timePeriod.IsAnytime == false)
                rule.RulePeriod.Setup(timePeriod.StartAsNullable, timePeriod.EndAsNullable);

            rule.DayOrException = dayOfWeek.GetHashCode() + 1;
            rule.IsWorking = isWorking;

            calendar.Rules.Add(rule);
            Session.SaveOrUpdate(calendar);

            return Repository<CalendarRule>.SaveOrUpdate(rule);
        }

        /// <summary>
        /// 특정 날짜를 공휴일로 지정하는 룰을 만듭니다. 
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="holyday"></param>
        /// <returns></returns>
        public CalendarRule CreateHolyDayCalendarRule(Calendar calendar, DateTime holyday)
        {
            calendar.ShouldNotBeNull("calendar");

            if(log.IsDebugEnabled)
                log.Debug(@"Calendar[{0}] 에 공휴일에 대한 Rule을 추가합니다  공휴일=[{1}]", calendar.Code, holyday);

            var rule = new CalendarRule(calendar, "Holyday");
            rule.RulePeriod.Setup(holyday.StartTimeOfDay(), holyday.EndTimeOfDay());
            rule.DayOrException = 0;
            rule.IsWorking = 0;
            rule.ViewOrder = rule.DayOrException;

            calendar.Rules.Add(rule);
            Session.SaveOrUpdate(rule);

            return rule;
        }

        /// <summary>
        /// 특정 Project, 특정 Calendar와 관련된 CalendarRule 목록을 로드합니다.
        /// </summary>
        /// <param name="calendarCode">Calendar 고유 코드</param>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<CalendarRule> FindAllCalendarRule(string calendarCode, string projectId, int? firstResult, int? maxResults, params INHOrder<CalendarRule>[] orders)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"Find All CalendarRules by calendarCode={0}, projectId={1}", calendarCode, projectId);

            var query = BuildQueryOverOfCalendarRule(calendarCode, projectId, null).AddOrders(orders);

            return Repository<CalendarRule>.FindAll(query,
                                                    firstResult.GetValueOrDefault(),
                                                    maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 특정 Project, 특정 Calendar와 관련된 CalendarRule 목록을 로드합니다.
        /// </summary>
        /// <param name="calendarCode">Calendar 고유 코드</param>
        /// <param name="projectId">관련 Proejct의 고유 ID</param>
        /// <returns></returns>
        public IList<CalendarRule> FindAllCalendarRule(string calendarCode, string projectId)
        {
            return FindAllCalendarRule(calendarCode, projectId, null, null);
        }

        /// <summary>
        /// 지정된 Calendar에 속한 모든 Calendar Rule을 삭제합니다.
        /// </summary>
        /// <param name="calendar"></param>
        public void ClearCalendarRulesOf(Calendar calendar)
        {
            calendar.ShouldNotBeNull("calendar");

            if(log.IsDebugEnabled)
                log.Debug(@"Calendar의 모든 규칙 정보(CalendarRule)를 삭제합니다... " + calendar);

            // 안해도 cascade delete가 된다.
            //foreach(var rule in calendar.Rules.ToArray())
            //    Repository<CalendarRule>.Delete(rule);

            calendar.Rules.Clear();
            Repository<Calendar>.SaveOrUpdate(calendar);
        }

        #endregion
    }
}