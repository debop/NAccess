using System;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 일별 작업 시간 정보 (한 레코드가 하루 (One Day)를 표현한다)
    /// </summary>
    [Serializable]
    public class WorkTimeByDay : WorkTimeByTimeBase
    {
        protected WorkTimeByDay() : base() {}

        public WorkTimeByDay(Calendar calendar, DateTime workDay) : base(calendar, workDay)
        {
            DayOfWeek = (int) workDay.DayOfWeek;
        }

        /// <summary>
        /// 요일 정보의 Integer 값
        /// </summary>
        public virtual int? DayOfWeek { get; set; }

        /// <summary>
        /// 작업 일
        /// </summary>
        public virtual DateTime WorkDay
        {
            get { return base.WorkTime; }
            protected set { base.WorkTime = value; }
        }
    }
}