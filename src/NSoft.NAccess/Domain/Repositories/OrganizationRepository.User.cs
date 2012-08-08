using System.Collections.Generic;
using System.Linq;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.LinqEx;
using NSoft.NFramework.Reflections;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NHibernate.Linq;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    public partial class OrganizationRepository
    {
        /// <summary>
        /// 사용자 (<see cref="User"/>) 정보를 조회하기 위한 IQueryable{User} 를 빌드합니다.
        /// </summary>
        public IQueryable<User> BuildQueryableOfUser(Company company, string loginId, string password = null)
        {
            var query = Repository<User>.Session.Query<User>();

            if(company != null)
                query = query.AddWhere(u => u.Company == company);

            if(loginId.IsNotWhiteSpace())
                query = query.AddWhere(u => u.LoginId == loginId);

            if(password.IsNotWhiteSpace())
                query = query.AddWhere(u => u.Password == password);

            return query;
        }

        /// <summary>
        /// 사용자 (<see cref="User"/>) 정보를 조회하기 위한 IQueryable{User} 를 빌드합니다.
        /// </summary>
        public IQueryable<User> BuildQueryableOfUser(Company company,
                                                     Department department = null,
                                                     string code = null,
                                                     string name = null,
                                                     int? kind = null,
                                                     string loginId = null,
                                                     string password = null,
                                                     string empNo = null)
        {
            var query = Repository<User>.Session.Query<User>();

            if(company != null)
                query = query.AddWhere(u => u.Company == company);

            if(department != null)
            {
                var userInDepts =
                    NAccessContext.Linq.DepartmentMembers
                        .Where(m => m.Department == department)
                        .Select(m => m.User.Id)
                        .Distinct()
                        .ToList();

                query = query.AddWhere(u => userInDepts.Contains(u.Id));
            }
            if(code.IsNotWhiteSpace())
                query = query.AddWhere(u => u.Code == code);

            if(name.IsNotWhiteSpace())
                query = query.AddWhere(u => u.Name == name);

            if(kind.HasValue)
                query = query.AddWhere(u => u.Kind == kind.Value);

            if(loginId.IsNotWhiteSpace())
                query = query.AddWhere(u => u.LoginId == loginId);

            if(password.IsNotWhiteSpace())
                query = query.AddWhere(u => u.Password == password);

            if(empNo.IsNotWhiteSpace())
                query = query.AddWhere(u => u.EmpNo == empNo);

            return query;
        }

        /// <summary>
        /// 사용자 (<see cref="User"/>) 정보를 조회하기 위한 QueryOver{User, User}를 빌드합니다.
        /// </summary>
        public QueryOver<User, User> BuildQueryOverOfUser(Company company, string loginId, string password = null)
        {
            var query = QueryOver.Of<User>();
            if(company != null)
                query.AddWhere(u => u.Company == company);

            if(loginId.IsNotWhiteSpace())
                query.AddWhere(u => u.LoginId == loginId);

            if(password.IsNotWhiteSpace())
                query.AddWhere(u => u.Password == password);

            return query;
        }

        /// <summary>
        /// 사용자 (<see cref="User"/>) 정보를 조회하기 위한 QueryOver{User, User}를 빌드합니다.
        /// </summary>
        public QueryOver<User, User> BuildQueryOverOfUser(Company company,
                                                          Department department = null,
                                                          string code = null,
                                                          string name = null,
                                                          int? kind = null,
                                                          string loginId = null,
                                                          string password = null,
                                                          string empNo = null)
        {
            var query = QueryOver.Of<User>();

            if(company != null)
                query.AddWhere(u => u.Company == company);

            if(department != null)
            {
                var userInDeptQuery =
                    QueryOver.Of<DepartmentMember>()
                        .AddWhere(m => m.Department == department)
                        .Select(Projections.Group<DepartmentMember>(m => m.User.Id));

                query.WithSubquery.WhereProperty(u => u.Id).In(userInDeptQuery);
            }
            if(code.IsNotWhiteSpace())
                query.AddWhere(u => u.Code == code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(u => u.Name == name);

            if(kind.HasValue)
                query.AddWhere(u => u.Kind == kind.Value);

            if(loginId.IsNotWhiteSpace())
                query.AddWhere(u => u.LoginId == loginId);

            if(password.IsNotWhiteSpace())
                query.AddWhere(u => u.Password == password);

            if(empNo.IsNotWhiteSpace())
                query.AddWhere(u => u.EmpNo == empNo);

            return query;
        }

        /// <summary>
        /// 사용자 (<see cref="User"/>) 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        public DetachedCriteria BuildCriteriaOfUser(Company company, Department department, string code, string name, int? kind, string loginId, string password,
                                                    string empNo)
        {
            return BuildQueryOverOfUser(company, department, code, name, kind, loginId, password, empNo).DetachedCriteria;
        }

        /// <summary>
        /// 사용자 로그인 계정 정보로 사용자를 인증합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AuthenticateUser(Company company, string loginId, string password)
        {
            company.ShouldNotBeNull("company");
            loginId.ShouldNotBeWhiteSpace("loginId");
            // password.ShouldNotBeWhiteSpace("password");

            if(log.IsDebugEnabled)
                log.Debug(@"사용자 인증을 실시합니다... companyId={0}, loginId={1}, password={2}", company.Id, loginId, password);

            return BuildQueryableOfUser(company, loginId, password).Any();

            //return NAccessContext.Linq.Users.Any(u => u.Company == company &&
            //                                          u.LoginId == loginId &&
            //                                          u.Password == password);
        }

        /// <summary>
        /// 지정한 회사의 사원 정보를 로드합니다. 만약 없다면, 새로 생성해서 저장한 후 로드 합니다. (그냥 Load 시에는 <see cref="FindOneUserByCode"/>로 호출하시기 바랍니다.
        /// </summary>
        public User GetOrCreateUser(Company company, string code)
        {
            company.ShouldNotBeNull("company");
            code.ShouldNotBeWhiteSpace("code");

            var user = FindOneUserByCode(company, code);

            if(user != null)
                return user;

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사[{0}]에 사용자[{1}]가 없습니다. 새로 생성합니다.", company.Code, code);

            lock(_syncLock)
            {
                using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                {
                    Repository<User>.Save(new User(company, code));
                    UnitOfWork.CurrentSession.Flush();
                }
            }

            user = FindOneUserByCode(company, code);
            user.AssertExists("user");

            if(log.IsInfoEnabled)
                log.Info("새로운 User 정보를 생성했습니다!!! New User = " + user);

            return user;
        }

        /// <summary>
        /// 특정 사용자 정보를 Load합니다.
        /// </summary>
        public User FindOneUserByCode(Company company, string code)
        {
            company.ShouldNotBeNull("company");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug(@"사용자 정보를 조회합니다. Company={0}, code={1}", company.Id, code);

            return
                NAccessContext.Linq.Users
                    .Where(u => u.Company == company &&
                                u.Code == code)
                    .SingleOrDefault();
        }

        /// <summary>
        /// 사번으로 직원 찾기
        /// </summary>
        /// <param name="company"></param>
        /// <param name="empNo"></param>
        /// <returns></returns>
        public User FindOneUserByEmpNo(Company company, string empNo)
        {
            company.ShouldNotBeNull("company");
            empNo.ShouldNotBeWhiteSpace("empNo");

            if(IsDebugEnabled)
                log.Debug("특정 사번의 사용자 정보를 조회합니다. Company={0}, empNo={1}", company, empNo);

            return
                NAccessContext.Linq.Users
                    .Where(u => u.Company == company &&
                                u.EmpNo == empNo)
                    .SingleOrDefault();
        }

        /// <summary>
        /// 로그인 계정을 가진 사용자 정보를 로드합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public User FindOneUserByLogin(Company company, string loginId)
        {
            return FindOneUserByLogin(company, loginId, null);
        }

        /// <summary>
        /// 로그인 정보를 가진 사용자 정보를 로드합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <returns>반환되는 User 정보가 있다면, 로그인 성공, 없다면 비밀번호가 틀렸다는 의미이다.</returns>
        public User FindOneUserByLogin(Company company, string loginId, string password)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug("사용자 계정으로 검색을 실시합니다... company.Code={0}, loginId={1}, password={2}", company.Code, loginId, password);

            var query =
                QueryOver.Of<User>()
                    .AddWhere(u => u.Company == company)
                    .AddWhere(u => u.LoginId == loginId);

            if(password.IsNotEmpty())
                query.AddWhere(u => u.Password == password);

            return Repository<User>.FindOne(query);
        }

        /// <summary>
        /// 지정된 회사의 모든 사용자 정보를 조회합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public IList<User> FindAllUserByCompany(Company company)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사의 모든 사용자 정보를 조회합니다... companyCode=" + company.Code);

            return NAccessContext.Linq.Users.Where(u => u.Company == company).ToList();
        }

        /// <summary>
        /// 지정된 회사에서, 지정된 이름과 매칭되는 (LIKE 검색) 사용자 정보를 조회합니다.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="nameToMatch"></param>
        /// <param name="matchMode"></param>
        /// <returns></returns>
        public IList<User> FindAllUserByCompanyAndNameToMatch(Company company, string nameToMatch, MatchMode matchMode)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug("사용자 이름과 매칭 검색을 수행합니다. nameToMatch={0}, matchMode={1}", nameToMatch, matchMode);

            var query =
                QueryOver.Of<User>()
                    .AddWhere(u => u.Company == company)
                    .AddInsensitiveLike(u => u.Name, nameToMatch, matchMode ?? MatchMode.Anywhere);

            return Repository<User>.FindAll(query);
        }

        /// <summary>
        /// 지정된 부서에 소속된 직원들을 반환합니다.
        /// </summary>
        /// <param name="department">부서</param>
        /// <param name="hierarchyContainsKind">부서의 조상/자손도 포함할 것인가 여부</param>
        /// <returns>부서 소속원 컬렉션을 반환한다.</returns>
        public IList<User> FindAllUserByDepartment(Department department, HierarchyContainsKinds hierarchyContainsKind)
        {
            department.ShouldNotBeNull("department");

            if(IsDebugEnabled)
                log.Debug(@"부서 소속 사원 정보를 가져옵니다. 부서의 상위 또는 하위 부서의 소속원들도 포함시킬 수 있습니다... " +
                          @"department={0}, hierarchyContainsKind={1}",
                          department, hierarchyContainsKind);

            // 중복 사용자를 피하기 위해 HashedSet을 사용합니다.
            var users = new HashSet<User>();

            // 1. 조상부서에 소속된 직원 정보)
            if((hierarchyContainsKind & HierarchyContainsKinds.Ancestors) > 0)
                department.GetAncestors().RunEach(dept => dept.GetUsers().RunEach(u => users.Add(u)));

            // 2. 현재부서에 소속된 직원 정보
            if((hierarchyContainsKind & HierarchyContainsKinds.Self) > 0)
                department.GetUsers().RunEach(u => users.Add(u));

            // 3. 자손부서에 소속된 직원 정보)
            if((hierarchyContainsKind & HierarchyContainsKinds.Descendents) > 0)
                department.GetDescendents().RunEach(dept => dept.GetUsers().RunEach(u => users.Add(u)));

            return users.ToList();
        }

        /// <summary>
        /// 지정된 회사의 모든 사용자 정보를 Paging 처리하여 로드합니다
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<User> GetPageOfUserByCompany(Company company, int pageIndex, int pageSize, params INHOrder<User>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사의 모든 사용자 정보를 Paging 처리하여 로드합니다... company={0}, pageIndex={1}, pageSize={2}, orders={3}",
                          company.Code, pageIndex, pageSize, orders.CollectionToString());

            return Repository<User>.GetPage(pageIndex,
                                            pageSize,
                                            QueryOver.Of<User>().AddWhere(u => u.Company == company),
                                            orders);
        }

        /// <summary>
        /// 지정된 부서에 소속된 직원들을 반환합니다.
        /// </summary>
        /// <param name="department">부서</param>
        /// <param name="hierarchyContainsKind">부서의 조상/자손도 포함할 것인가 여부</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns>부서 소속원 컬렉션을 반환한다.</returns>
        public IPagingList<User> GetPageOfUserByDepartment(Department department,
                                                           HierarchyContainsKinds hierarchyContainsKind,
                                                           int pageIndex,
                                                           int pageSize,
                                                           params INHOrder<User>[] orders)
        {
            department.ShouldNotBeNull("department");
            department.Company.ShouldNotBeNull("company of department");

            var companyCode = department.Company.Code;

            // TODO : 이 함수는 검증해봐야 한다!!!
            // NOTE : 왜 Paging 시에 간단하게 DepartMember로 하지 않고, 직접 User로 해야 되냐면, 겸직인 경우 두번 나오게 된다. 그럼 Paging 처리가 이상하게 된다.););

            if(IsDebugEnabled)
                log.Debug(@"부서 소속 사원 정보를 가져옵니다. 부서의 상위 또는 하위 부서의 소속원들도 포함시킬 수 있습니다." +
                          @"department={0}, hierarchyContainsKind={1}, pageIndex={2}, pageSize={3}, orders={4}",
                          department, hierarchyContainsKind, pageIndex, pageSize, orders.CollectionToString());

            // Department 조회
            var departments = FindAllDepartmentByHierarchy(department, hierarchyContainsKind);

            Company @companyAlias = null;
            Department @departmentAlias = null;
            User @userAlias = null;


            // Department들의 소속부서원 정보의 모든 부서원의 Id 값 조회
            var userIdQuery =
                QueryOver.Of<DepartmentMember>()
                    .JoinAlias(dm => dm.Department, () => @departmentAlias)
                    .JoinAlias(() => @departmentAlias.Company, () => @companyAlias)
                    .AddWhere(() => @companyAlias.Code == companyCode)
                    .AddWhereRestrictionOn(u => u.Department).IsIn(departments.ToArray())
                    .JoinAlias(dm => dm.User, () => @userAlias)
                    .Select(Projections.Group(() => @userAlias.Id));


            // 부서원 Id를 조회하는 서브쿼리에 만족하는 모든 User 조회
            var query =
                QueryOver.Of<User>()
                    .JoinAlias(u => u.Company, () => @companyAlias)
                    .AddWhere(() => @companyAlias.Code == companyCode)
                    .WithSubquery.WhereProperty(u => u.Id).In(userIdQuery);

            return Repository<User>.GetPage(pageIndex, pageSize, query, orders);
        }
    }
}