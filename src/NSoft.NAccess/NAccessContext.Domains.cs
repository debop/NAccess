using NSoft.NFramework.InversionOfControl;
using NSoft.NAccess.Domain.Repositories;

namespace NSoft.NAccess
{
    public static partial class NAccessContext
    {
        /// <summary>
        /// Domain Services
        /// </summary>
        public static class Domains
        {
            // private static readonly object _sync = new object();

            /// <summary>
            /// Calendar Domain Service
            /// </summary>
            public static CalendarRepository CalendarRepository
            {
                get { return IoC.TryResolve<CalendarRepository>(); }
            }

            /// <summary>
            /// Organization Domain Service
            /// </summary>
            public static OrganizationRepository OrganizationRepository
            {
                get { return IoC.TryResolve<OrganizationRepository>(); }
            }

            /// <summary>
            /// Product Domain Service
            /// </summary>
            public static ProductRepository ProductRepository
            {
                get { return IoC.TryResolve<ProductRepository>(); }
            }

            /// <summary>
            /// Logging Domain Service
            /// </summary>
            public static LoggingRepository LoggingRepository
            {
                get { return IoC.TryResolve<LoggingRepository>(); }
            }
        }
    }
}