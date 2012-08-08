using System;

namespace NSoft.NAccess
{
    /// <summary>
    /// 그룹 (가상의 사용자 집단)의 종류
    /// </summary>
    [Flags]
    public enum GroupKinds
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        /// System defined group
        /// </summary>
        System = 0x01,

        /// <summary>
        /// User custom defined group
        /// </summary>
        Custom = 0x02,

        /// <summary>
        /// All
        /// </summary>
        All = 0xFF
    }
}