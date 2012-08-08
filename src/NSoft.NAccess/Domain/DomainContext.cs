namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// Domain Context
    /// </summary>
    public static class DomainContext
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion
    }
}