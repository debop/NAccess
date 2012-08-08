using System;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 제품 관련 Domain Service
    /// </summary>
    public partial class ProductRepository : NAccessRepositoryBase
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
        public ProductRepository()
        {
            if(log.IsInfoEnabled)
                log.Info(@"ProductRepository 인스턴스가 생성되었습니다.");
        }
    }
}