using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Tools;

namespace NSoft.NAccess.BackgroundServices
{
    /// <summary>
    /// 시스템이 사용할 Data를 RDBMS에서 미리 Cache 할 수 있도록, Background 에서 필요한 Data를 Cache에 올라오도록 SQL문장을 수행한다.
    /// </summary>
    public class AdoPreCacheService : IAdoPreCacheService
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        #endregion

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="repository">Ado Repository</param>
        /// <param name="sqlStatements">실행할 쿼리문의 컬렉션</param>
        public AdoPreCacheService(IAdoRepository repository, IEnumerable<string> sqlStatements)
        {
            repository.ShouldNotBeNull("repository");
            sqlStatements.ShouldNotBeEmpty<string>("sqlStatements");

            _adoRepository = repository;
            SqlStatements = sqlStatements.ToArray();

            if(log.IsInfoEnabled)
                log.Debug(@"AdoPreCacheService 의 인스턴스가 생성되었습니다.");
        }

        private IAdoRepository _adoRepository;

        /// <summary>
        /// AdoRepository
        /// </summary>
        public IAdoRepository AdoRepository
        {
            get { return _adoRepository; }
            set
            {
                if(value != null)
                    _adoRepository = value;
            }
        }

        /// <summary>
        /// RDBMS에서 데이타를 미리 캐시하기 위해 실행할 SQL 문장의 컬렉션
        /// </summary>
        public string[] SqlStatements { get; set; }

        /// <summary>
        /// 지정된 SQL 문장을 실행하여 RDBMS가 관련 Data를 메모리에 Cache 할 수 있도록 한다.
        /// </summary>
        public bool Execute(params object[] args)
        {
            if(SqlStatements == null || SqlStatements.Length == 0)
            {
                if(log.IsWarnEnabled)
                    log.Warn(@"실행할 SQL 문이 없습니다...");

                return false;
            }

            if(log.IsDebugEnabled)
                log.Debug(@"RDBMS에서 자주 사용하는 정보를 미리 메모리 캐시하기 위해, Background에서 미리 조회용 Query 문장을 호출합니다...");

            var queries = SqlStatements.Where(sql => sql.IsNotWhiteSpace()).ToArray();

            var loopResult =
                Parallel.ForEach(queries,
                                 query =>
                                 With.TryAction(() =>
                                                {
                                                    if(log.IsTraceEnabled)
                                                        log.Trace(@"다음 Query 문장을 비동기 방식으로 실행합니다... Query=" + query);

                                                    AdoRepository.ExecuteDataTable(query);

                                                    if(log.IsTraceEnabled)
                                                        log.Trace(@"지정한 쿼리문을 실행하여 DataTable로 로드했습니다!!! Query=" + query);
                                                },
                                                ex =>
                                                {
                                                    if(log.IsWarnEnabled)
                                                        log.WarnException(@"쿼리 문장을 실행하는 동안 예외가 발생했습니다. (예외는 무시합니다) query=" + query, ex);
                                                }));

            if(log.IsInfoEnabled)
                log.Info(@"RDBMS에 필요한 Data PreCache를 위한 BackgroundService 작업이 완료되었습니다!!! loopResult.IsCompleted=" + loopResult.IsCompleted);

            return loopResult.IsCompleted;
        }
    }
}