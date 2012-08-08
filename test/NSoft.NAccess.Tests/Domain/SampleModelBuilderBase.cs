using System;

namespace NSoft.NAccess.Domain
{
    /// <summary>
    /// Sample용 Model 정보를 빌드하는 클래스의 최상위 추상화 클래스입니다.
    /// </summary>
    public abstract class SampleModelBuilderBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public static readonly Random RandomGenerator = new Random((int) DateTime.Now.Ticks);

        public abstract void CreateSampleModels();
    }
}