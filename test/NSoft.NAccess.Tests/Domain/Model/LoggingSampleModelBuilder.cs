using System;
using System.Linq;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.LinqEx;
using NSoft.NFramework.TimePeriods;

namespace NSoft.NAccess.Domain.Model.Loggings
{
    public class LoggingSampleModelBuilder : SampleModelBuilderBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public override void CreateSampleModels()
        {
            CreateUserLoginHistory();
            UnitOfWork.Current.TransactionalFlush();
        }

        protected virtual void CreateUserLoginHistory()
        {
            // 1년동안 매일 로그인 했다고 설정 
            //
            var dateRange = new TimeRange(DateTime.Now, DurationUtil.Days(30));

            var activeProducts = NAccessContext.Domains.ProductRepository.FindAllActiveProduct();
            var activeCompanys = NAccessContext.Domains.OrganizationRepository.FindAllActiveCompany();

            dateRange
                .ForEachDays()
                .AsParallel()
                .AsOrdered()
                .RunEach(loginTime =>
                         {
                             foreach(var product in activeProducts)
                                 foreach(var company in activeCompanys)
                                     foreach(var user in NAccessContext.Domains.OrganizationRepository.FindAllUserByCompany(company))
                                         NAccessContext.Domains.LoggingRepository
                                             .InsertUserLoginLog(product, user, null, loginTime.Start);
                         });
        }
    }
}