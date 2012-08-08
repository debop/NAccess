using System;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 일별 작업 시간 정보 (한 레코드가 하루 (One Day)를 표현한다)
    /// </summary>
    [Serializable]
    public class WorkTimeByHour : WorkTimeByTimeBase
    {
        protected WorkTimeByHour() : base() {}
        public WorkTimeByHour(Calendar calendar, DateTime workHour) : base(calendar, workHour) {}

        /// <summary>
        /// 작업 시각
        /// </summary>
        public virtual DateTime WorkHour
        {
            get { return base.WorkTime; }
            protected set { base.WorkTime = value; }
        }
    }
}