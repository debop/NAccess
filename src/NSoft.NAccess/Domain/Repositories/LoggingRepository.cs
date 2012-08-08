using System;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 시스템 사용에 대한 로그 서비스
    /// </summary>
    public partial class LoggingRepository : NAccessRepositoryBase
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        #endregion

        private readonly object _syncLock = new object();
    }
}