using System;
using NSoft.NFramework;
using NSoft.NFramework.TimePeriods;
using NSoft.NFramework.TimePeriods.TimeRanges;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// Calendar Rule에 의해 생성된 실제 Time 정보
    /// </summary>
    [Serializable]
    public class WorkTimeByRange : WorkTimeByTimeBase
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected WorkTimeByRange() { }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="timeRange"></param>
        /// <param name="previousCumulatedWorkInMinute"></param>
        public WorkTimeByRange(Calendar calendar, ITimePeriod timeRange, int previousCumulatedWorkInMinute)
            : base(calendar, timeRange.Start)
        {
            timeRange.ShouldNotBeNull("timeRange");
            Guard.Assert(timeRange.HasPeriod, @"timeRange는 명시적인 구간을 가져야 합니다.");

            TimePeriod.Setup(timeRange.Start, timeRange.End);

            CumulatedInMinute = previousCumulatedWorkInMinute + WorkInMinute;
        }

        private ITimePeriod _timePeriod;

        /// <summary>
        /// Work Time의 기간 ( Date Part는 같고, Time Part만 변화한다 )
        /// </summary>
        public virtual ITimePeriod TimePeriod
        {
            get { return _timePeriod ?? (_timePeriod = new DayRange(DateTime.Now)); }
            protected set { _timePeriod = value; }
        }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(CalendarCode, TimePeriod);
        }

        public override string ToString()
        {
            return
                string.Format(@"WorkTimeByRange#CalendarCode={0}, TimePeriod={1}, IsWork={2}, WorkInMinute={3}, CumulatedInMinute={4}",
                              CalendarCode, TimePeriod, IsWork, WorkInMinute, CumulatedInMinute);
        }
    }
}