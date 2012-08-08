using System;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 일별 작업 시간 정보 (한 레코드가 하루 (One Day)를 표현한다)
    /// </summary>
    [Serializable]
    public class WorkTimeByMinute : WorkTimeByTimeBase
    {
        protected WorkTimeByMinute() : base() {}
        public WorkTimeByMinute(Calendar calendar, DateTime workMinute) : base(calendar, workMinute) {}

        /// <summary>
        /// 작업 시각 (분까지 구분되어야 함)
        /// </summary>
        public virtual DateTime WorkMinute
        {
            get { return base.WorkTime; }
            protected set { base.WorkTime = value; }
        }
    }
}