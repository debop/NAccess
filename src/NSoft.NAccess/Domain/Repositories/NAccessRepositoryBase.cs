using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx;
using NHibernate;
using NHibernate.Engine;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// RealAdmin Domain Service의 최상의 추상 클래스
    /// </summary>
    public abstract class NAccessRepositoryBase
    {
        #region << logger >>

        private static readonly Lazy<NLog.Logger> lazyLog = new Lazy<NLog.Logger>(() => NLog.LogManager.GetCurrentClassLogger());

        private static NLog.Logger log
        {
            get { return lazyLog.Value; }
        }

        #endregion

        /// <summary>
        /// UnitOfWork에서 활성화된 NHibernate <see cref="ISession"/>
        /// </summary>
        protected ISession Session
        {
            get { return UnitOfWork.CurrentSession; }
        }

        /// <summary>
        /// UnitOfWork에서 활성화된 NHibernate <see cref="ISessionImplementor"/>
        /// </summary>
        protected ISessionImplementor SessionImpl
        {
            get { return UnitOfWork.CurrentSession as ISessionImplementor; }
        }

        /// <summary>
        /// 현재 활성화된 UnitOfWork의 <see cref="SessionFactory"/>
        /// </summary>
        protected ISessionFactory SessionFactory
        {
            get { return UnitOfWork.CurrentSessionFactory; }
        }

        /// <summary>
        /// 현재 활성화된 UnitOfWork의 <see cref="SessionFactoryImplementor"/>
        /// </summary>
        protected ISessionFactoryImplementor SessionFactoryImplementor
        {
            get { return UnitOfWork.CurrentSessionFactory as ISessionFactoryImplementor; }
        }

        /// <summary>
        /// 삭제 시 Transaction을 이용한다.
        /// </summary>
        /// <param name="entity">삭제하고자 하는 entity</param>
        /// <exception cref="InvalidOperationException">삭제하고자 하는 엔티티가 null일 경우</exception>
        protected void DeleteEntityTransactional(IDataObject entity)
        {
            entity.ShouldNotBeNull("entity");

            if(log.IsDebugEnabled)
                log.Debug("엔티티 삭제를 시작합니다... entity=[{0}]", entity);

            var tx = UnitOfWork.Current.BeginTransaction();

            try
            {
                UnitOfWork.CurrentSession.Delete(entity);
                tx.Commit();
            }
            catch(Exception ex)
            {
                if(log.IsErrorEnabled)
                    log.ErrorException("지정된 엔티티를 삭제하는데 실패했습니다!!! entity: " + entity, ex);

                if(tx != null)
                    tx.Rollback();

                throw;
            }

            if(log.IsDebugEnabled)
                log.Debug("엔티티 삭제가 완료되었습니다!!!");
        }
    }
}