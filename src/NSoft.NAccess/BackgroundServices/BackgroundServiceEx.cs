using System.Threading.Tasks;

namespace NSoft.NAccess.BackgroundServices
{
    /// <summary>
    /// <see cref="IBackgroundService"/> 의 확장 메소드를 제공합니다.
    /// </summary>
    public static class BackgroundServiceEx
    {
        /// <summary>
        /// 비동가 방식으로 서비스를 실행시킵니다.
        /// </summary>
        /// <param name="backgroundService">실행할 Background Service 인스턴스</param>
        /// <param name="args">실행 인자</param>
        /// <returns>실행 결과를 담은 <see cref="Task"/></returns>
        public static Task<bool> ExecuteServiceAsync(this IBackgroundService backgroundService, params object[] args)
        {
            return Task.Factory.StartNew(() => backgroundService.Execute(args));
        }
    }
}
