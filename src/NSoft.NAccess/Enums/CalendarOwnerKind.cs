namespace NSoft.NAccess
{
    /// <summary>
    /// Calendar 소유자 종류
    /// </summary>
    public enum CalendarOwnerKind
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 표준 (Owner가 없음)
        /// </summary>
        Standard,

        /// <summary>
        /// 사용자
        /// </summary>
        User,

        /// <summary>
        /// 부서 
        /// </summary>
        Department,

        /// <summary>
        /// 그룹
        /// </summary>
        Group,

        /// <summary>
        /// 회사
        /// </summary>
        Company,

        /// <summary>
        /// System
        /// </summary>
        System,

        /// <summary>
        /// Project
        /// </summary>
        Project,
    }
}