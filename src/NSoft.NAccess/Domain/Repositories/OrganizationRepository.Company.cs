using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// Company 관련 Domain Service
    /// </summary>
    public partial class OrganizationRepository
    {
        /// <summary>
        /// Company 조회를 위한 QueryOver를 빌드합니다.
        /// </summary>
        /// <param name="code">회사 코드</param>
        /// <param name="name">회사 명</param>
        /// <param name="isActive">활성화 여부</param>
        public static QueryOver<Company, Company> BuildQueryOverOfCompany(string code, string name = null, bool? isActive = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"Company 조회를 위한 QueryOver를 빌드합니다. code={0}, name={1}, isActive={2}", code, name, isActive);

            var query = QueryOver.Of<Company>();

            if(code.IsNotWhiteSpace())
                query.AddWhere(c => c.Code == code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(c => c.Name == name);

            if(isActive.HasValue)
                query.AddNullAsTrue(c => c.IsActive, isActive);

            return query;
        }

        /// <summary>
        /// Company 조회를 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="code">회사 코드</param>
        /// <param name="name">회사 명</param>
        /// <param name="isActive">활성화 여부</param>
        public static DetachedCriteria BuildCriteriaOfCompany(string code, string name, bool? isActive)
        {
            return BuildQueryOverOfCompany(code, name, isActive).DetachedCriteria;
        }

        /// <summary>
        /// 지정된 회사 코드를 가진 Company 정보를 로드합니다. 없으면, 새로 만들어서 저장 후, 반환합니다.
        /// </summary>
        /// <param name="code">회사 코드</param>
        /// <returns></returns>
        public Company GetOrCreateComapny(string code)
        {
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"Company 정보를 조회합니다... code=" + code);

            var company = FindOneCompanyByCode(code);

            if(company == null)
            {
                if(log.IsInfoEnabled)
                    log.Info("해당하는 Company 정보가 없습니다. 새로 생성합니다... code=" + code);

                lock(_syncLock)
                {
                    using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                    {
                        UnitOfWork.CurrentSession.Save(new Company(code));
                        UnitOfWork.Current.TransactionalFlush();
                    }
                }

                company = FindOneCompanyByCode(code);
                company.AssertExists("company");


                if(log.IsInfoEnabled)
                    log.Info("새로운 Company 정보를 생성했습니다!!! " + company);
            }

            return company;
        }

        /// <summary>
        /// 지정한 코드를 가지는 회사를 찾습니다.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Company FindOneCompanyByCode(string code)
        {
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"Company를 조회합니다... code=" + code);

            return Repository<Company>.FindOne(BuildQueryOverOfCompany(code));
        }

        /// <summary>
        /// Company Name으로 조회하기
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Company FindOneCompanyByName(string name)
        {
            name.ShouldNotBeWhiteSpace("name");

            if(IsDebugEnabled)
                log.Debug(@"Company를 조회합니다... name=" + name);

            return Repository<Company>.FindOne(BuildQueryOverOfCompany(null, name));
        }

        /// <summary>
        /// 사용가능한 모든 Company 정보를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public IList<Company> FindAllActiveCompany()
        {
            if(IsDebugEnabled)
                log.Debug(@"모든 Active인 Company 를 조회합니다...");

            return Repository<Company>.FindAll(BuildQueryOverOfCompany(null, null, true));
        }

        /// <summary>
        /// 지정한 Company 정보를 삭제합니다.
        /// </summary>
        /// <param name="company"></param>
        public void DeleteCompany(Company company)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"Company를 삭제합니다... company=" + company);

            DeleteEntityTransactional(company);

            if(log.IsInfoEnabled)
                log.Info(@"Company를 삭제했습니다!!! company=" + company);
        }
    }
}