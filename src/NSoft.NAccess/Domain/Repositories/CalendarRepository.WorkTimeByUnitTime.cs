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
    /// Calendar Domain Service 중에 WorkTimeByDay 에 대한 서비스
    /// </summary>
    public partial class CalendarRepository
    {
        /// <summary>
        /// <see cref="WorkTimeByDay"/> 정보를 조회하기 위해 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="calendarCode"></param>
        /// <param name="workDay"></param>
        /// <param name="workPeriod"></param>
        /// <param name="isWork"></param>
        /// <returns></returns>
        public QueryOver<WorkTimeByDay, WorkTimeByDay> BuildQueryOverOfWorkTimeByDay(string calendarCode,
                                                                                     DateTime? workDay = null,
                                                                                     ITimePeriod workPeriod = null,
                                                                                     bool? isWork = null)
        {
            if(log.IsDebugEnabled)
                log.Debug(@"WorkTimeByDay 정보를 조회하기 위해 Criteria를 빌드합니다... " +
                          @"calendarCode={0}, workDay={1}, workRange={2}, isWork={3}",
                          calendarCode, workDay, workPeriod, isWork);

            var query = QueryOver.Of<WorkTimeByDay>();

            if(calendarCode.IsNotWhiteSpace())
                query.AddWhere(wt => wt.CalendarCode == calendarCode);

            if(workDay.HasValue)
                query.AddWhere(wt => wt.WorkDay == workDay);

            if(workPeriod.IsAnytime == false)
                query.AddBetween(wt => wt.WorkDay, workPeriod.StartAsNullable, workPeriod.EndAsNullable);

            if(isWork.HasValue)
                query.AddNullAsTrue(wt => wt.IsWork, isWork.Value);

            return query;
        }

        /// <summary>
        /// <see cref="WorkTimeByDay"/> 정보를 조회하기 위해 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="calendarCode"></param>
        /// <param name="workDay"></param>
        /// <param name="workPeriod"></param>
        /// <param name="isWork"></param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfWorkTimeByDay(string calendarCode,
                                                             DateTime? workDay = null,
                                                             ITimePeriod workPeriod = null,
                                                             bool? isWork = null)
        {
            return BuildQueryOverOfWorkTimeByDay(calendarCode, workDay, workPeriod, isWork).DetachedCriteria;
        }

        /// <summary>
        /// 지정한 날짜의 WorkTimeByDay 정보를 로드합니다.
        /// </summary>
        /// <param name="calendar">DayCalendar가 속한 Calendar 정보</param>
        /// <param name="workDay">검색할 날짜</param>
        /// <returns></returns>
        public WorkTimeByDay FindOneWorkTimeByDayByWorkDay(Calendar calendar, DateTime workDay)
        {
            calendar.ShouldNotBeNull("calendar");

            if(log.IsDebugEnabled)
                log.Debug("Load WorkTimeByDay. calendar={0}, day={1}", calendar, workDay);

            var query = BuildQueryOverOfWorkTimeByDay(calendar.Code, workDay);

            return Repository<WorkTimeByDay>.FindOne(query);
        }

        /// <summary>
        /// 특정 Calendar의 지정된 범위의 <see cref="WorkTimeByDay"/> 목록을 가져옵니다.
        /// </summary>
        /// <param name="calendarCode">WorkTime 산출의 기준이 된 Calendar의 코드값 (null이면 안된다)</param>
        /// <param name="searchPeriod">검색 기간 (null이거나 기간이 비었으면 모든 기간을 가져온다)</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<WorkTimeByDay> FindAllWorkTimeByDayInRange(string calendarCode,
                                                                ITimePeriod searchPeriod,
                                                                int? firstResult,
                                                                int? maxResults,
                                                                params INHOrder<WorkTimeByDay>[] orders)
        {
            calendarCode.ShouldNotBeWhiteSpace("calendarCode");

            if(log.IsDebugEnabled)
                log.Debug(@"WorkTimeByDay 정보를 로드합니다... " +
                          @"calendarCode={0}, searchPeriod={1}, firstResult={2}, maxResults={3}, orders={4}",
                          calendarCode, searchPeriod, firstResult, maxResults, orders);

            var query = BuildQueryOverOfWorkTimeByDay(calendarCode, null, searchPeriod).AddOrders(orders);

            return Repository<WorkTimeByDay>.FindAll(query,
                                                     firstResult.GetValueOrDefault(),
                                                     maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정된 기간의 <see cref="WorkTimeByDay"/> 목록을 가져옵니다.
        /// </summary>
        /// <param name="searchPeriod"></param>
        /// <returns></returns>
        public IList<WorkTimeByDay> FindAllWorkTimeByDayInRange(ITimePeriod searchPeriod)
        {
            Guard.Assert(searchPeriod != null && searchPeriod.IsAnytime == false, @"검색기간이 설정되지 않았습니다.");

            if(log.IsDebugEnabled)
                log.Debug(@"WorkTimeByDay 정보를 로드합니다... searchPeriod=" + searchPeriod);

            return Repository<WorkTimeByDay>.FindAll(BuildQueryOverOfWorkTimeByDay(null, null, searchPeriod, null));
        }

        /// <summary>
        /// 지정한 기간의 WorkTimeByDay 정보를 Paging처리해서 로드합니다
        /// </summary>
        /// <param name="calendarCode">WorkTime 산출의 기준이 된 Calendar의 코드값 (null이면 안된다)</param>
        /// <param name="searchPeriod">검색 기간 (null이거나 기간이 비었으면 모든 기간을 가져온다)</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다.)</param>
        /// <param name="pageSize">Page Size (보통 10입니다. 0보다 커야 합니다.)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<WorkTimeByDay> GetPageOfWorkTimeByDayInRange(string calendarCode,
                                                                        ITimePeriod searchPeriod,
                                                                        int pageIndex,
                                                                        int pageSize,
                                                                        params INHOrder<WorkTimeByDay>[] orders)
        {
            calendarCode.ShouldNotBeWhiteSpace("calendarCode");

            if(log.IsDebugEnabled)
                log.Debug(@"지정한 기간의 WorkTimeByDay 정보를 Paging처리해서 로드합니다... " +
                          @"calendarCode={0}, searchPeriod={1}, pageIndex={2}, pageSize={3}, orders={4}",
                          calendarCode, searchPeriod, pageIndex, pageSize, orders);

            return Repository<WorkTimeByDay>.GetPage(pageIndex,
                                                     pageSize,
                                                     BuildQueryOverOfWorkTimeByDay(calendarCode, null, searchPeriod),
                                                     orders);
        }
    }
}