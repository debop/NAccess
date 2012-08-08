using System;

namespace NSoft.NAccess
{
    /// <summary>
    /// 계층구조상에서 선택 사항에 대한 종류를 나타낸다.
    /// </summary>
    [Flags]
    public enum HierarchyContainsKinds
    {
        /// <summary>
        /// 자신
        /// </summary>
        Self = 0x01,

        /// <summary>
        /// 자손들 (자신을 제외한)
        /// </summary>
        Descendents = 0x02,

        /// <summary>
        /// 자신과 자손들
        /// </summary>
        SelfAndDescendents = Self | Descendents,

        /// <summary>
        /// 조상들 (자신을 제외한)
        /// </summary>
        Ancestors = 0x04,

        /// <summary>
        /// 자신과 조상들
        /// </summary>
        SelfAndAncestors = Self | Ancestors,

        /// <summary>
        /// 자신을 제외한 자손과 조상들
        /// </summary>
        DescendentsAndAncestors = Descendents | Ancestors,

        /// <summary>
        /// 모두
        /// </summary>
        All = 0xFF
    }
}