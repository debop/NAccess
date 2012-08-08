using System;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 조직 정보 관련 Domain Service
    /// </summary>
    public partial class OrganizationRepository : NAccessRepositoryBase
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        private static bool IsDebugEnabled
        {
            get { return lazyLog.Value.IsDebugEnabled; }
        }

        #endregion

        private readonly object _syncLock = new object();

        /// <summary>
        /// 생성자
        /// </summary>
        public OrganizationRepository()
        {
            if(log.IsInfoEnabled)
                log.Info(@"조직관련 정보를 서비스하는 OrganizationRepository 인스턴스가 생성되었습니다.");
        }
    }
}