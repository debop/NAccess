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
    /// Calendar Domain Service �߿� WorkTimeByDay �� ���� ����
    /// </summary>
    public partial class CalendarRepository
    {
        /// <summary>
        /// <see cref="WorkTimeByDay"/> ������ ��ȸ�ϱ� ���� Criteria�� �����մϴ�.
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
                log.Debug(@"WorkTimeByDay ������ ��ȸ�ϱ� ���� Criteria�� �����մϴ�... " +
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
        /// <see cref="WorkTimeByDay"/> ������ ��ȸ�ϱ� ���� Criteria�� �����մϴ�.
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
        /// ������ ��¥�� WorkTimeByDay ������ �ε��մϴ�.
        /// </summary>
        /// <param name="calendar">DayCalendar�� ���� Calendar ����</param>
        /// <param name="workDay">�˻��� ��¥</param>
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
        /// Ư�� Calendar�� ������ ������ <see cref="WorkTimeByDay"/> ����� �����ɴϴ�.
        /// </summary>
        /// <param name="calendarCode">WorkTime ������ ������ �� Calendar�� �ڵ尪 (null�̸� �ȵȴ�)</param>
        /// <param name="searchPeriod">�˻� �Ⱓ (null�̰ų� �Ⱓ�� ������� ��� �Ⱓ�� �����´�)</param>
        /// <param name="firstResult">ù��° ��� ���� �ε��� (0���� ����. null�̸� 0���� ����)</param>
        /// <param name="maxResults">��� ���� �ִ� ���ڵ� �� (null �Ǵ� 0 ������ ���� ���õȴ�)</param>
        /// <param name="orders">���� ����</param>
        /// <returns></returns>
        public IList<WorkTimeByDay> FindAllWorkTimeByDayInRange(string calendarCode,
                                                                ITimePeriod searchPeriod,
                                                                int? firstResult,
                                                                int? maxResults,
                                                                params INHOrder<WorkTimeByDay>[] orders)
        {
            calendarCode.ShouldNotBeWhiteSpace("calendarCode");

            if(log.IsDebugEnabled)
                log.Debug(@"WorkTimeByDay ������ �ε��մϴ�... " +
                          @"calendarCode={0}, searchPeriod={1}, firstResult={2}, maxResults={3}, orders={4}",
                          calendarCode, searchPeriod, firstResult, maxResults, orders);

            var query = BuildQueryOverOfWorkTimeByDay(calendarCode, null, searchPeriod).AddOrders(orders);

            return Repository<WorkTimeByDay>.FindAll(query,
                                                     firstResult.GetValueOrDefault(),
                                                     maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// ������ �Ⱓ�� <see cref="WorkTimeByDay"/> ����� �����ɴϴ�.
        /// </summary>
        /// <param name="searchPeriod"></param>
        /// <returns></returns>
        public IList<WorkTimeByDay> FindAllWorkTimeByDayInRange(ITimePeriod searchPeriod)
        {
            Guard.Assert(searchPeriod != null && searchPeriod.IsAnytime == false, @"�˻��Ⱓ�� �������� �ʾҽ��ϴ�.");

            if(log.IsDebugEnabled)
                log.Debug(@"WorkTimeByDay ������ �ε��մϴ�... searchPeriod=" + searchPeriod);

            return Repository<WorkTimeByDay>.FindAll(BuildQueryOverOfWorkTimeByDay(null, null, searchPeriod, null));
        }

        /// <summary>
        /// ������ �Ⱓ�� WorkTimeByDay ������ Pagingó���ؼ� �ε��մϴ�
        /// </summary>
        /// <param name="calendarCode">WorkTime ������ ������ �� Calendar�� �ڵ尪 (null�̸� �ȵȴ�)</param>
        /// <param name="searchPeriod">�˻� �Ⱓ (null�̰ų� �Ⱓ�� ������� ��� �Ⱓ�� �����´�)</param>
        /// <param name="pageIndex">Page Index (0���� �����մϴ�.)</param>
        /// <param name="pageSize">Page Size (���� 10�Դϴ�. 0���� Ŀ�� �մϴ�.)</param>
        /// <param name="orders">���� ����</param>
        /// <returns></returns>
        public IPagingList<WorkTimeByDay> GetPageOfWorkTimeByDayInRange(string calendarCode,
                                                                        ITimePeriod searchPeriod,
                                                                        int pageIndex,
                                                                        int pageSize,
                                                                        params INHOrder<WorkTimeByDay>[] orders)
        {
            calendarCode.ShouldNotBeWhiteSpace("calendarCode");

            if(log.IsDebugEnabled)
                log.Debug(@"������ �Ⱓ�� WorkTimeByDay ������ Pagingó���ؼ� �ε��մϴ�... " +
                          @"calendarCode={0}, searchPeriod={1}, pageIndex={2}, pageSize={3}, orders={4}",
                          calendarCode, searchPeriod, pageIndex, pageSize, orders);

            return Repository<WorkTimeByDay>.GetPage(pageIndex,
                                                     pageSize,
                                                     BuildQueryOverOfWorkTimeByDay(calendarCode, null, searchPeriod),
                                                     orders);
        }
    }
}