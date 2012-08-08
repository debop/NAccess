using System;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 특정 단위 시각의 작업 시간에 대한 정보의 인터페이스 (일단위, 시간단위, 분단위, 5분단위, 월단위, 주단위 등 모두 가능하다)
    /// </summary>
    public interface IWorkTimeByTime : IUpdateTimestampedEntity
    {
        /// <summary>
        /// 작업 시간 계산의 기준이 되는 Calendar 정보
        /// </summary>
        string CalendarCode { get; }

        /// <summary>
        /// 기준 작업 시각
        /// </summary>
        DateTime WorkTime { get; }

        /// <summary>
        /// 지정둰 <see cref="WorkTime"/>이 작업시간이 존재하는지 (<see cref="WorkInMinute"/> > 0) 
        /// </summary>
        bool? IsWork { get; set; }

        /// <summary>
        /// 작업시간의 분단위로 표시
        /// </summary>
        int? WorkInMinute { get; set; }

        /// <summary>
        /// 작업시간의 누적시간을 분단위로 표시 (시작시각과 소요작업시간만 알면 완료시각을 빨리 알 수 있도록 하기 위해)
        /// </summary>
        long? CumulatedInMinute { get; set; }
    }
}