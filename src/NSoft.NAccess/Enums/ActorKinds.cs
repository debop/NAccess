using System;

namespace NSoft.NAccess
{
    /// <summary>
    /// 리소스에 접근할 수 있는 Actor 의 종류
    /// </summary>
    [Flags]
    public enum ActorKinds
    {
        /// <summary>
        /// 알 수 없음
        /// </summary>
        Unknown = 0x0000,

        /// <summary>
        /// 사용자
        /// </summary>
        User = 0x0001,

        /// <summary>
        /// 부서
        /// </summary>
        Department = 0x0002,

        /// <summary>
        /// 그룹
        /// </summary>
        Group = 0x0004,

        /// <summary>
        /// 파트
        /// </summary>
        Part = 0x0008,

        /// <summary>
        /// 회사
        /// </summary>
        Company = 0x0010
    }
}