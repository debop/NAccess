using NSoft.NFramework.Data;

namespace NSoft.NAccess.BackgroundServices
{
    /// <summary>
    /// 시스템이 사용할 Data를 RDBMS에서 미리 Cache 할 수 있도록, Background 에서 필요한 Data를 Cache에 올라오도록 SQL문장을 수행한다.
    /// </summary>
    public interface IAdoPreCacheService : IBackgroundService
    {
        /// <summary>
        /// <see cref="IAdoRepository"/> 인스턴스
        /// </summary>
        IAdoRepository AdoRepository { get; set; }

        /// <summary>
        /// 실행할 SQL Statements
        /// </summary>
        string[] SqlStatements { get; set; }
    }
}