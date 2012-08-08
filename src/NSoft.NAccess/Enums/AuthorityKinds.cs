using System;

namespace NSoft.NAccess
{
    /// <summary>
    /// 권한 종류
    /// </summary>
    [Flags]
    public enum AuthorityKinds
    {
        /// <summary>
        /// 없음
        /// </summary>
        Nothing = 0x0000,

        /// <summary>
        /// 읽기
        /// </summary>
        Read = 0x0001,

        /// <summary>
        /// 쓰기
        /// </summary>
        Write = 0x0002,

        /// <summary>
        /// 편집
        /// </summary>
        Edit = 0x0004,

        /// <summary>
        /// 삭제
        /// </summary>
        Delete = 0x0008,

        /// <summary>
        /// 모든 권한
        /// </summary>
        All = 0xFFFF
    }
}