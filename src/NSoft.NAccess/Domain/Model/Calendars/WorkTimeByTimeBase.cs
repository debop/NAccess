using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 특정 단위 시각의 작업 시간에 대한 정보 (일단위, 시간단위, 분단위, 5분단위, 월단위, 주단위 등 모두 가능하다)
    /// </summary>
    /// <remarks>
    /// NOTE: http://www.nhforge.org/doc/nh/en/index.html#inheritance-tableperconcrete 처럼 하고 싶지만, Id 컬럼에 Identity나 native generator를 사용하지 못한다. 그래서 포기!!!
    /// </remarks>
    [Serializable]
    public class WorkTimeByTimeBase : DataEntityBase<Guid>, IWorkTimeByTime
    {
        protected WorkTimeByTimeBase()
        {
            base.Id = Guid.NewGuid();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="calendar">기준 Calendar</param>
        /// <param name="workTime">기준 시각</param>
        protected WorkTimeByTimeBase(Calendar calendar, DateTime workTime)
            : this()
        {
            calendar.ShouldNotBeNull("calendar");

            CalendarCode = calendar.Code;
            WorkTime = workTime;
        }

        /// <summary>
        /// 작업 시간 계산의 기준이 되는 Calendar 코드
        /// </summary>
        public virtual string CalendarCode { get; protected set; }

        /// <summary>
        /// 작업 시간
        /// </summary>
        public virtual DateTime WorkTime { get; protected set; }

        /// <summary>
        /// 지정둰 <see cref="WorkTime"/>이 작업시간이 존재하는지 (<see cref="WorkInMinute"/> > 0) 
        /// </summary>
        public virtual bool? IsWork { get; set; }

        /// <summary>
        /// 작업시간의 분단위로 표시
        /// </summary>
        public virtual int? WorkInMinute { get; set; }

        /// <summary>
        /// 작업시간의 누적시간을 분단위로 표시 (시작시각과 소요작업시간만 알면 완료시각을 빨리 알 수 있도록 하기 위해)
        /// </summary>
        public virtual long? CumulatedInMinute { get; set; }

        /// <summary>
        /// 최종 갱신일
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        /// <summary>
        /// Set new identity value.
        /// </summary>
        /// <param name="newId">new identity value</param>
        public virtual void SetIdentity(Guid newId)
        {
            base.Id = newId;
        }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(CalendarCode, WorkTime);
        }

        public override string ToString()
        {
            return string.Format(@"{0}# CalendarCode={0}, WorkTime={1}, IsWork={2}, WorkInMinute={3}, CumulatedInMinute={4}",
                                 CalendarCode, WorkTime, IsWork, WorkInMinute, CumulatedInMinute);
        }
    }
}