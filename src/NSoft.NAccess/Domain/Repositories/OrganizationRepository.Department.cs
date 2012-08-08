using System.Collections.Generic;
using System.Linq;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Reflections;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    public partial class OrganizationRepository
    {
        /// <summary>
        /// 부서 (<see cref="Department"/>) 정보를 조회하기 위한 QueryOver를 빌드합니다.
        /// </summary>
        public static QueryOver<Department, Department> BuildQueryOverOfDepartment(string companyCode,
                                                                                   string code = null,
                                                                                   string name = null,
                                                                                   string parentCode = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"Department 정보를 조회하기 위해 Criteria를 빌드합니다... companyCode={0}, code={1}, name={2}, parentCode={3}",
                          companyCode, code, name, parentCode);

            var query = QueryOver.Of<Department>();

            Company @company = null;

            if(companyCode.IsNotWhiteSpace())
            {
                query.Inner.JoinAlias(d => d.Company, () => @company)
                    .AddWhere(() => @company.Code == companyCode);
            }

            if(code.IsNotWhiteSpace())
                query.AddWhere(d => d.Code == code);

            if(name.IsNotWhiteSpace())
                query.AddWhere(d => d.Name == name);

            if(parentCode.IsNotWhiteSpace())
                query.AddWhere(d => d.Parent.Code == parentCode);

            return query;
        }

        /// <summary>
        /// 부서 (<see cref="Department"/>) 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        public static DetachedCriteria BuildCriteriaOfDepartment(string companyCode, string code, string name, string parentCode)
        {
            return BuildQueryOverOfDepartment(companyCode, code, name, parentCode).DetachedCriteria;
        }

        /// <summary>
        /// 부서 (<see cref="Department"/>) 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        public static DetachedCriteria BuildCriteriaOfDepartment(Company company, string code, string name, Department parent)
        {
            return BuildQueryOverOfDepartment(company.Code, code, name, (parent != null) ? parent.Code : null).DetachedCriteria;
        }

        /// <summary>
        /// 지정된 부서 Id를 가진 부서 정보를 로드한다. 만약 없다면 새로 생성하여 저장한 후 로드한다.
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="code">부서 코드</param>
        /// <returns></returns>
        public Department GetOrCreateDepartment(Company company, string code)
        {
            company.ShouldNotBeNull("company");
            code.ShouldNotBeWhiteSpace("code");

            var department = FindOneDepartmentByCode(company, code);

            if(department == null)
            {
                if(IsDebugEnabled)
                    log.Debug(@"지정된 회사['{0}']에 부서['{1}']가 없습니다. 새로 생성합니다.", company.Code, code);

                lock(_syncLock)
                {
                    using(UnitOfWork.Start(UnitOfWorkNestingOptions.CreateNewOrNestUnitOfWork))
                    {
                        // Depatment.Name이 not-null이기 때문에 값이 들어가야 한다!!!
                        Repository<Department>.Save(new Department(company, code));
                        UnitOfWork.Current.TransactionalFlush();
                    }
                }

                department = FindOneDepartmentByCode(company, code);
                department.AssertExists("department");

                if(log.IsInfoEnabled)
                    log.Info("새로운 Department 정보를 생성했습니다. Department=" + department);
            }

            return department;
        }

        /// <summary>
        /// 지정된 상위 부서 밑으로 부서를 추가합니다. 이때, 상위부서는 null이면 안됩니다. (최상위 부서는 <see cref="GetOrCreateDepartment"/> 메소드를 사용하세요)
        /// </summary>
        public Department CreateDepartmentOf(Department parent, string childCode)
        {
            parent.ShouldNotBeNull("parent");
            childCode.ShouldNotBeWhiteSpace("childCode");

            if(IsDebugEnabled)
                log.Debug("부서[{0}] 하위로 새로운 부서를 추가합니다. 추가할 부서 Id=[{1}] 입니다.", parent.Code, childCode);

            var child = new Department(parent.Company, childCode);

            child.SetParent(parent);
            child.UpdateNodePosition();

            Repository<Department>.SaveOrUpdate(parent);
            return Repository<Department>.SaveOrUpdate(child);
        }

        /// <summary>
        /// 지정된 child 부서를 newParent의 하위부서로 이동시킨다.
        /// </summary>
        public void ChangeDepartmentParent(Department child, Department newParent)
        {
            child.ShouldNotBeNull("child");

            var oldParent = child.Parent;

            if(Equals(oldParent, newParent))
            {
                if(log.IsWarnEnabled)
                    log.Warn("변경할 부모가 현재 부모와 같으므로, 부모 변경 작업을 수행하지 않습니다.");
                return;
            }

            if(IsDebugEnabled)
                log.Debug("부서의 부모를 변경합니다. child={0}, new parent={1}, old parent={2}",
                          child.Code,
                          (newParent != null) ? newParent.Code : EntityTool.NULL_STRING,
                          (oldParent != null) ? oldParent.Code : EntityTool.NULL_STRING);

            child.ChangeParent(oldParent, newParent);

            Repository<Department>.SaveOrUpdate(child);
        }

        /// <summary>
        /// 지정된 회사의 부서 코드에 해당하는 부서정보를 로드합니다.
        /// </summary>
        public Department FindOneDepartmentByCode(string companyCode, string code)
        {
            companyCode.ShouldNotBeNull("companyCode");
            code.ShouldNotBeWhiteSpace("code");

            if(IsDebugEnabled)
                log.Debug("부서 정보를 조회합니다... companyCode={0}, code={1}", companyCode, code);

            return Repository<Department>.FindOne(BuildQueryOverOfDepartment(companyCode, code, null, null));
        }

        /// <summary>
        /// 지정된 회사의 부서 코드에 해당하는 부서정보를 로드합니다.
        /// </summary>
        public Department FindOneDepartmentByCode(Company company, string code)
        {
            company.ShouldNotBeNull("company");
            return FindOneDepartmentByCode(company.Code, code);
        }

        /// <summary>
        /// 최상위 부서들의 정보를 조회한다.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public IList<Department> FindRootDepartmentByCompany(Company company)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사의 최상위 부서 (Parent 가 Null인) 정보를 조회합니다... company=" + company);

            var query =
                QueryOver.Of<Department>()
                    .AddWhere(dept => dept.Company == company)
                    .WhereRestrictionOn(dept => dept.Parent).IsNull;

            return Repository<Department>.FindAll(query);
        }

        /// <summary>
        /// 지정된 회사의 모든 부서 정보를 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<Department> FindAllDepartmentByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<Department>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사의 모든 부서 정보를 조회합니다... company={0}, firstResult={1}, maxResults={2}, orders={3}",
                          company, firstResult, maxResults, orders.CollectionToString());

            var query = QueryOver.Of<Department>().AddWhere(dept => dept.Company == company);
            return Repository<Department>.FindAll(query.AddOrders(orders),
                                                  firstResult.GetValueOrDefault(),
                                                  maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정된 회사의 모든 부서 정보를 조회합니다.
        /// </summary>
        /// <param name="company">회사</param>
        public IList<Department> FindAllDepartmentByCompany(Company company)
        {
            return FindAllDepartmentByCompany(company, null, null);
        }

        /// <summary>
        /// 조직명 매칭 검색 (LIKE 검색)을 수행합니다.
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="nameToMatch">검색할 조직명</param>
        /// <param name="matchMode">검색 매칭 모드</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<Department> FindAllDepartmentByNameToMatch(Company company, string nameToMatch, MatchMode matchMode, int? firstResult, int? maxResults,
                                                                params INHOrder<Department>[] orders)
        {
            company.ShouldNotBeNull("company");

            if(nameToMatch.IsWhiteSpace())
                return FindAllDepartmentByCompany(company, firstResult, maxResults, orders);

            if(IsDebugEnabled)
                log.Debug(@"조직명 매칭 검색 (LIKE 검색)을 수행합니다... " +
                          @"company={0}, nameToMatch={1}, matchMode={2}, firstResult={3}, maxResults={4}, orders={5}",
                          company.Code, nameToMatch, matchMode, firstResult, maxResults, orders.CollectionToString());

            var query =
                QueryOver.Of<Department>()
                    .AddWhere(dept => dept.Company == company)
                    .AddInsensitiveLike(dept => dept.Name, nameToMatch, matchMode ?? MatchMode.Anywhere)
                    .AddOrders(orders);

            return Repository<Department>.FindAll(query,
                                                  firstResult.GetValueOrDefault(),
                                                  maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정된 부서와 옵션에 따라 하위 부서 또는 상위부서를 포함한 부서정보를 제공합니다.
        /// </summary>
        /// <param name="department">조회할 기준 부서</param>
        /// <param name="hierarchyContainsKind">부서의 조상/자손도 포함할 것인가 여부</param>
        /// <returns></returns>
        public IList<Department> FindAllDepartmentByHierarchy(Department department, HierarchyContainsKinds hierarchyContainsKind)
        {
            // NOTE: 현재는 Loop 방식을 사용하여 round-trip이 많이 발생하게끔 되어 있다. Second Cache로 속도를 극복할 수 밖에 없다.

            if(IsDebugEnabled)
                log.Debug(@"계층구조에 따른 부서 정보를 조회합니다. department={0}, hierarchyContainsKind={1}",
                          department, hierarchyContainsKind);

            var results = new List<Department>();

            if(department == null)
                return results;

            // 1. 조상 부서 조회 및 추가
            if((hierarchyContainsKind & HierarchyContainsKinds.Ancestors) > 0)
                results.AddRange(department.GetAncestors());

            // 2. 현재 부서 추가
            if((hierarchyContainsKind & HierarchyContainsKinds.Self) > 0)
                results.Add(department);

            // 3. 자손 부서 조회 및 추가
            if((hierarchyContainsKind & HierarchyContainsKinds.Descendents) > 0)
                results.AddRange(department.GetDescendents());

            return results;
        }

        /// <summary>
        /// 지정된 Department 를 삭제합니다. (NOTE: 하위 부서 및 Association의 삭제는 HBM의 cascade에 따라 달라집니다)
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public void DeleteDepartment(Department department)
        {
            if(department == null)
                return;

            if(IsDebugEnabled)
                log.Debug("부서 정보를 삭제합니다. department=" + department);

            base.DeleteEntityTransactional(department);
        }

        /// <summary>
        /// 지정된 Company의 모든 부서정보를 삭제합니다.
        /// </summary>
        /// <param name="company"></param>
        public void DeleteAllDepartmentByCompany(Company company)
        {
            company.ShouldNotBeNull("company");

            if(IsDebugEnabled)
                log.Debug(@"지정된 회사의 모든 부서 정보를 삭제합니다. company=" + company);

            Repository<Department>.DeleteAll(QueryOver.Of<Department>().AddWhere(dept => dept.Company == company));
        }

        /// <summary>
        /// 부서-소속원 정보 (<see cref="DepartmentMember"/>)를 검색하기 위한 Criteria를 빌드합니다.
        /// </summary>
        public QueryOver<DepartmentMember, DepartmentMember> BuildQueryOverOfDepartmentMember(
            Department department, User user, EmployeeTitle employeeTitle, bool? isLeader, bool? isActive)
        {
            if(IsDebugEnabled)
                log.Debug(@"부서-소속원 정보 (DepartmentMember)를 검색하기 위한 Criteria를 빌드합니다... " +
                          @"department={0}, user={1}, employeeTitle={2}, isLeader={3}, isActive={4}",
                          department, user, employeeTitle, isLeader, isActive);

            var query = QueryOver.Of<DepartmentMember>();

            if(department != null)
                query.AddWhere(dm => dm.Department == department);

            if(user != null)
                query.AddWhere(dm => dm.User == user);

            if(employeeTitle != null)
                query.AddWhere(dm => dm.EmployeeTitle == employeeTitle);

            if(isLeader.HasValue)
                query.AddWhere(dm => dm.IsLeader == isLeader);

            if(isActive.HasValue)
                query.AddNullAsTrue(dm => dm.IsActive, isActive);

            return query;
        }

        /// <summary>
        /// 부서-소속원 정보 (<see cref="DepartmentMember"/>)를 검색하기 위한 Criteria를 빌드합니다.
        /// </summary>
        public DetachedCriteria BuildCriteriaOfDepartmentMember(Department department, User user,
                                                                EmployeeTitle employeeTitle, bool? isLeader, bool? isActive)
        {
            return BuildQueryOverOfDepartmentMember(department, user, employeeTitle, isLeader, isActive).DetachedCriteria;
        }

        /// <summary>
        /// 부서에 소속된 사용자인가 검사합니다.
        /// </summary>
        /// <param name="department"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsDepartmentMember(Department department, User user)
        {
            department.ShouldNotBeNull("department");
            user.ShouldNotBeNull("user");

            if(IsDebugEnabled)
                log.Debug(@"부서에 소속된 사용자인가 검사합니다. department={0}, user={1}", department, user);

            var query = BuildQueryOverOfDepartmentMember(department, user, null, null, null);
            var isMember = Repository<DepartmentMember>.Exists(query);

            if(IsDebugEnabled)
                log.Debug(@"부서에 소속된 사용자인가 검사했습니다!!! department={0}, user={1}, 소속여부:{2}", department, user, isMember);

            return isMember;
        }

        /// <summary>
        /// 부서에 직원 소속 정보를 생성합니다.
        /// </summary>
        /// <param name="department">부서</param>
        /// <param name="user">사용자</param>
        /// <param name="employeeTitle">직책 정보</param>
        /// <returns></returns>
        public DepartmentMember CreateDepartmentMember(Department department, User user, EmployeeTitle employeeTitle)
        {
            department.ShouldNotBeNull("department");
            user.ShouldNotBeNull("user");

            DepartmentMember member;

            // 1. 이미 부서에 소속되어 있다면, 정보 갱신만 하고, 반환한다.
            if(IsDepartmentMember(department, user))
            {
                if(log.IsInfoEnabled)
                    log.Info("이미 부서에 소속된 직원이므로, 새로 생성하지 않고 기존 정보를 읽어 반환합니다. department={0}, user={1}", department, user);

                member = department.Members.FirstOrDefault(m => m.User.Equals(user));
                // member = department.Members.Where(m => m.User.Equals(user)).FirstOrDefault();

                if(member != null && member.EmployeeTitle != employeeTitle)
                {
                    member.EmployeeTitle = employeeTitle;
                    Repository<DepartmentMember>.SaveOrUpdate(member);
                }

                return member;
            }

            // 2. 새로운 부서 소속 정보를 생성하고, 저장한 후 반환한다.
            if(IsDebugEnabled)
                log.Debug(@"부서에 새로운 직원을 소속시킵니다. department={0}, user={1}, employeeTitle={2}",
                          department.Id, user.Id, employeeTitle);

            var company = department.Company;
            Guard.Assert(company != null, @"해당 Company 정보를 찾을 수 없습니다. department=[{0}]", department);

            member = new DepartmentMember(department, user)
                     {
                         EmployeeTitle = employeeTitle
                     };

            return Repository<DepartmentMember>.SaveOrUpdate(member);
        }

        /// <summary>
        /// 지정된 직원의 소속 부서를 변경합니다.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldDepartment"></param>
        /// <param name="newDepartment"></param>
        /// <returns></returns>
        public DepartmentMember ChangeDepartmentFor(User user, Department oldDepartment, Department newDepartment)
        {
            user.ShouldNotBeNull("user");
            oldDepartment.ShouldNotBeNull("oldDepartment");
            newDepartment.ShouldNotBeNull("newDepartment");
            Guard.Assert(oldDepartment != newDepartment, "현재 소속된 부서가 변경할 부서와 같습니다.");

            if(IsDebugEnabled)
                log.Debug("사용자[{0}]의 소속 부서를 변경합니다... 기존부서={1}, 신규부서={2}",
                          user.Id, oldDepartment.Id, newDepartment.Id);

            // 1. 기존 부서에 소속된 정보를 제거합니다.
            DeleteDepartmentMember(oldDepartment, user);

            // 2. 변경할 부서에 이미 소속되어 있으므로, 기존 부서 정보만 삭제하고 반환한다.
            if(IsDepartmentMember(newDepartment, user))
            {
                if(log.IsDebugEnabled)
                    log.Debug(@"변경할 부서에 이미 소속되어 있습니다... 기존 부서 소속 정보만 삭제합니다.");

                return newDepartment.Members.Where(m => m.User.Equals(user)).FirstOrDefault();
            }

            return CreateDepartmentMember(newDepartment, user, null);
        }

        /// <summary>
        /// 지정된 회사의 모든 부서-직원 소속 정보를 로드합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="firstResult">첫번째 결과 셋의 인덱스 (0부터 시작. null이면 0으로 간주)</param>
        /// <param name="maxResults">결과 셋의 최대 레코드 수 (null 또는 0 이하의 값은 무시된다)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IList<DepartmentMember> FindAllDepartmemtMemberByCompany(Company company, int? firstResult, int? maxResults, params INHOrder<DepartmentMember>[] orders)
        {
            company.ShouldNotBeNull("company");

            Department @departmentAlias = null;

            var query =
                QueryOver.Of<DepartmentMember>()
                    .JoinAlias(dm => dm.Department, () => @departmentAlias)
                    .AddWhere(() => @departmentAlias.Company == company);

            return Repository<DepartmentMember>.FindAll(query.AddOrders(orders),
                                                        firstResult.GetValueOrDefault(),
                                                        maxResults.GetValueOrDefault());
        }

        /// <summary>
        /// 지정된 회사의 모든 부서-직원 소속 정보를 로드합니다.
        /// </summary>
        /// <param name="company">회사</param>
        /// <returns></returns>
        public IList<DepartmentMember> FindAllDepartmemtMemberByCompany(Company company)
        {
            return FindAllDepartmemtMemberByCompany(company, null, null);
        }

        /// <summary>
        /// 지정된 부서의 조상 또는 자손의 소속 정보까지 가져올 것인가?  
        /// </summary>
        /// <param name="department">조회할 부서</param>
        /// <param name="hierarchyContainsKinds">부서의 조상/자손도 포함할 것인가 여부</param>
        /// <returns></returns>
        public IEnumerable<DepartmentMember> FindAllDepartmentMemberByDepartmentHierarchy(Department department,
                                                                                          HierarchyContainsKinds hierarchyContainsKinds)
        {
            department.ShouldNotBeNull("department");

            if(IsDebugEnabled)
                log.Debug(@"지정된 부서 및 부서의 조상/자손의 직원 소속 정보를 가져옵니다. department={0}, hierarchyContainsKind={1}",
                          department.Id, hierarchyContainsKinds);

            var departments = FindAllDepartmentByHierarchy(department, hierarchyContainsKinds);

            // 조회한 부서에 대한 부서-직원 소속 정보를 가져온다
            //
            return departments.SelectMany(d => d.Members);
        }

        /// <summary>
        /// 부서-직원 소속 정보를 제거합니다.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool DeleteDepartmentMember(DepartmentMember member)
        {
            member.ShouldNotBeNull("member");

            if(log.IsDebugEnabled)
                log.Debug(@"부서-직원 소속 정보를 제거합니다. departmentMember=" + member);

            var department = member.Department;
            if(department != null)
            {
                department.Members.Remove(member);
                Session.SaveOrUpdate(department);
            }

            DeleteEntityTransactional(member);

            return true;
        }

        /// <summary>
        /// 부서-직원 소속 정보를 제거합니다.
        /// </summary>
        /// <param name="department"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool DeleteDepartmentMember(Department department, User user)
        {
            department.ShouldNotBeNull("department");
            user.ShouldNotBeNull("user");

            if(log.IsDebugEnabled)
                log.Debug(@"부서-직원 소속 정보를 제거합니다...부서={0}, 직원={1}", department, user);

            if(IsDepartmentMember(department, user) == false)
            {
                if(IsDebugEnabled)
                    log.Debug("부서에 소속된 사용자가 아니므로, 소속 정보를 삭제할 것이 없습니다.");

                return false;
            }

            var member = department.Members.Where(m => m.User.Equals(user)).FirstOrDefault();

            if(member != null)
                return DeleteDepartmentMember(member);

            return false;
        }

        /// <summary>
        /// 지정한 부서의 부서원 소속 정보를 모두 삭제합니다.
        /// </summary>
        /// <param name="department"></param>
        public void DeleteAllDepartmentMemberByDepartment(Department department)
        {
            department.ShouldNotBeNull("department");

            department.Members.Clear();
            Repository<Department>.SaveOrUpdate(department);
        }

        /// <summary>
        /// 지정된 사용자의 모든 부서 소속 정보를 삭제합니다.
        /// </summary>
        /// <param name="user"></param>
        public void DeleteAllDepartmentMemberByUser(User user)
        {
            user.ShouldNotBeNull("user");

            if(IsDebugEnabled)
                log.Debug("사용자의 모든 부서 소속 정보 삭제를 시작합니다... user=" + user);

            Repository<DepartmentMember>
                .DeleteAll(DetachedCriteria
                               .For<DepartmentMember>()
                               .AddEq("User", user));

            // NOTE: UnitOfWork.TransactionalFlush()를 사용한다.
            //
            //var tx = UnitOfWork.Current.BeginTransaction();
            //try
            //{
            //    var crit = DetachedCriteria.For<DepartmentMember>().AddEq("User", user);
            //    Repository<DepartmentMember>.DeleteAll(crit);
            //    tx.Commit();

            //    if (IsInfoEnabled)
            //        log.Info(@"사용자의 부서 소속 정보를 삭제했습니다. user=" + user);
            //}
            //catch (Exception ex)
            //{
            //    if (IsErrorEnabled)
            //        log.ErrorException(@"사용자의 부서 소속정보를 삭제하는데 실패했습니다. user=" + user, ex);

            //    if (tx != null)
            //        tx.Rollback();
            //    throw;
            //}
        }
    }
}