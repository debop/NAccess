using System.Collections.Generic;
using System.Linq;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Reflections;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    public partial class OrganizationRepository
    {
        /// <summary>
        /// 직급 정보를 조회합니다. 없으면 새로 생성합니다.
        /// </summary>
        public EmployeeGrade GetOrCreateEmployeeGrade(Company company, string code)
        {
            var grade = FindOneEmployeeGradeByCode(company, code);

            if(grade == null)
            {
                lock(_syncLock)
                    using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                    {
                        CreateEmployeeGrade(company, code, code);
                        UnitOfWork.Current.Flush();
                    }
                grade = FindOneEmployeeGradeByCode(company, code);
                grade.AssertExists("grade");
            }

            return grade;
        }

        /// <summary>
        /// 새로운 직급 정보를 생성합니다.
        /// </summary>
        public EmployeeGrade CreateEmployeeGrade(Company company, string code, string name)
        {
            var grade = new EmployeeGrade(company, code) {Name = name};

            if(IsDebugEnabled)
                log.Debug(@"새로운 직급 정보를 생성합니다... " + grade);

            return Repository<EmployeeGrade>.SaveOrUpdate(grade);
        }

        /// <summary>
        /// 직급정보를 조회합니다.
        /// </summary>
        public EmployeeGrade FindOneEmployeeGradeByCode(Company company, string code)
        {
            return
                NAccessContext.Linq.EmployeeGrades
                    .AddWhere(g => g.Company == company && g.Code == code)
                    .SingleOrDefault();
        }

        /// <summary>
        /// 지정된 회사의 모든 사원 직급 정보를 가져옵니다.
        /// </summary>
        public IList<EmployeeGrade> FindAllEmployeeGradeByCompany(Company company)
        {
            return FindAllEmployeeGradeByCompany(company, null, null);
        }

        /// <summary>
        /// 지정된 회사의 모든 사원 직급 정보를 가져옵니다.
        /// </summary>
        public IList<EmployeeGrade> FindAllEmployeeGradeByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<EmployeeGrade>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"사원 직급 정보(EmployeeGrade)를 조회합니다. company=" + company);

            var query =
                QueryOver.Of<EmployeeGrade>()
                    .AddWhere(g => g.Company == company)
                    .AddOrders(orders);

            return Repository<EmployeeGrade>.FindAll(query,
                                                     firstResult.GetValueOrDefault(),
                                                     maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정한 회사의 모든 사원 직급 정보를 페이징 처리해서 로드합니다.
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다)</param>
        /// <param name="pageSize">Page Size (0보다 큰 값이어야 합니다)</param>
        /// <param name="orders">정렬 방식</param>
        /// <returns></returns>
        public IPagingList<EmployeeGrade> GetPageOfEmployeeGradeByCompany(Company company, int pageIndex, int pageSize, params INHOrder<EmployeeGrade>[] orders)
        {
            Guard.ShouldNotBeNull(company, "company");

            if(IsDebugEnabled)
                log.Debug(@"지정한 회사의 모든 사원 직급 정보 (EmployeeGrade) 를 페이징 처리해서 로드합니다..." +
                          @"company={0}, pageIndex={1}, pageSize={2}, orders={3}",
                          company, pageIndex, pageSize, orders.CollectionToString());

            var query =
                QueryOver.Of<EmployeeGrade>()
                    .AddWhere(g => g.Company == company);

            return Repository<EmployeeGrade>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// 직위 (Employee Position) 정보를 조회합니다. 없으면 새로 생성합니다.
        /// </summary>
        public EmployeePosition GetOrCreateEmployeePosition(Company company, string code)
        {
            var position = FindOneEmployeePositionByCode(company, code);

            if(position == null)
            {
                position = CreateEmployeePosition(company, code, code);
                position.AssertExists("position");
            }

            return position;
        }

        /// <summary>
        /// 사원 직위 정보를 생성합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public EmployeePosition CreateEmployeePosition(Company company, string code, string name)
        {
            var position = new EmployeePosition(company, code) {Name = name};

            if(IsDebugEnabled)
                log.Debug("새로운 EmployeePosition를 생성합니다... " + position);

            return Repository<EmployeePosition>.SaveOrUpdate(position);
        }

        /// <summary>
        /// 직위 정보를 조회합니다.
        /// </summary>
        public EmployeePosition FindOneEmployeePositionByCode(Company company, string code)
        {
            return
                NAccessContext.Linq.EmployeePositions
                    .AddWhere(g => g.Company == company && g.Code == code)
                    .SingleOrDefault();
        }

        /// <summary>
        /// 지정된 회사의 사원 직위 정보를 모두 조회합니다.
        /// </summary>
        public IList<EmployeePosition> FindAllEmployeePositionByCompany(Company company)
        {
            return FindAllEmployeePositionByCompany(company, null, null);
        }

        /// <summary>
        /// 지정된 회사의 사원 직위 정보를 모두 조회합니다.
        /// </summary>
        public IList<EmployeePosition> FindAllEmployeePositionByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<EmployeePosition>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug("사원 직위 정보를 조회합니다... Company=" + company);

            var query = QueryOver.Of<EmployeePosition>()
                .AddWhere(p => p.Company == company)
                .AddOrders(orders);

            return Repository<EmployeePosition>.FindAll(query,
                                                        firstResult.GetValueOrDefault(),
                                                        maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정한 회사의 모든 사원 직위 정의 정보를 페이징 처리해서 로드합니다.
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다)</param>
        /// <param name="pageSize">Page Size (0보다 큰 값이어야 합니다)</param>
        /// <param name="orders">정렬 방식</param>
        /// <returns></returns>
        public IPagingList<EmployeePosition> GetPageOfEmployeePositionByCompany(Company company, int pageIndex, int pageSize, params INHOrder<EmployeePosition>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug(@"지정한 회사의 모든 사원 직위 정의(EmployeePosition) 정보를 페이징 처리해서 로드합니다..." +
                          @"company={0}, pageIndex={1}, pageSize={2}, orders={3}",
                          company, pageIndex, pageSize, orders.CollectionToString());

            var query = QueryOver.Of<EmployeePosition>().AddWhere(p => p.Company == company);

            return Repository<EmployeePosition>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// 직책 (Employee Title) 정보를 조회합니다. 없으면 새로 생성합니다.
        /// </summary>
        public EmployeeTitle GetOrCreateEmployeeTitle(Company company, string code)
        {
            var title = FindOneEmployeeTitleByCode(company, code);

            if(title == null)
            {
                title = CreateEmployeeTitle(company, code, code);
                title.AssertExists("title");
            }

            return title;
        }

        /// <summary>
        /// 사원 직책 정보를 생성합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public EmployeeTitle CreateEmployeeTitle(Company company, string code, string name)
        {
            var title = new EmployeeTitle(company, code) {Name = name};

            if(IsDebugEnabled)
                log.Debug("새로운 EmployeeTitle을 생성합니다... " + title);

            return Repository<EmployeeTitle>.SaveOrUpdate(title);
        }

        /// <summary>
        /// 직책 정보를 조회합니다.
        /// </summary>
        public EmployeeTitle FindOneEmployeeTitleByCode(Company company, string code)
        {
            return
                NAccessContext.Linq.EmployeeTitles
                    .AddWhere(g => g.Company == company && g.Code == code)
                    .SingleOrDefault();
        }

        /// <summary>
        /// 지정된 회사의 사원 직위 정보를 모두 조회합니다.
        /// </summary>
        public IList<EmployeeTitle> FindAllEmployeeTitleByCompany(Company company)
        {
            return FindAllEmployeeTitleByCompany(company, null, null);
        }

        /// <summary>
        /// 지정된 회사의 사원 직위 정보를 모두 조회합니다.
        /// </summary>
        public IList<EmployeeTitle> FindAllEmployeeTitleByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<EmployeeTitle>[] orders)
        {
            if(IsDebugEnabled)
                log.Debug("사원 직책 정보를 조회합니다... Company=" + company);

            var query =
                QueryOver.Of<EmployeeTitle>()
                    .AddWhere(t => t.Company == company)
                    .AddOrders(orders);

            return Repository<EmployeeTitle>.FindAll(query,
                                                     firstResult.GetValueOrDefault(),
                                                     maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정한 회사의 모든 사원 직책 정의 정보를 페이징 처리해서 로드합니다.
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="pageIndex">Page Index (0부터 시작합니다)</param>
        /// <param name="pageSize">Page Size (0보다 큰 값이어야 합니다)</param>
        /// <param name="orders">정렬 방식</param>
        /// <returns></returns>
        public IPagingList<EmployeeTitle> GetPageOfEmployeeTitleByCompany(Company company, int pageIndex, int pageSize, params INHOrder<EmployeeTitle>[] orders)
        {
            Guard.ShouldNotBeNull(company, "company");

            if(IsDebugEnabled)
                log.Debug(@"지정한 회사의 모든 사원 직책 정의(EmployeeTitle) 정보를 페이징 처리해서 로드합니다... " +
                          @"company={0}, pageIndex={1}, pageSize={2}, orders={3}",
                          company, pageIndex, pageSize, orders.CollectionToString());

            var query = QueryOver.Of<EmployeeTitle>().AddWhere(t => t.Company == company);

            return Repository<EmployeeTitle>.GetPage(pageIndex, pageSize, query, orders);
        }
    }
}