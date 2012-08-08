namespace NSoft.NAccess.BackgroundServices
{
    /// <summary>
    /// 백그라운드에서 수행하는 서비스를 나타내는 인터페이스입니다.
    /// </summary>
    public interface IBackgroundService
    {
        /// <summary>
        /// 서비스를 실행합니다.
        /// </summary>
        /// <param name="args">실행 인자들</param>
        bool Execute(params object[] args);
    }
}