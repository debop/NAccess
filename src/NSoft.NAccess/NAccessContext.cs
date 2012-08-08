using System;
using NSoft.NFramework.InversionOfControl;

namespace NSoft.NAccess
{
    public static partial class NAccessContext
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        #endregion

        static NAccessContext()
        {
            if(IoC.IsNotInitialized)
                IoC.Initialize();
        }

        /// <summary>
        /// Library Name
        /// </summary>
        public const string LibraryName = @"RealAdmin";

        /// <summary>
        /// UserId of Administrator
        /// </summary>
        public const string Administrator = @"admin";
    }
}