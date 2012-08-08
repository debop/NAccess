using System.Linq;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx;
using NHibernate;
using NHibernate.Linq;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess
{
    public static partial class NAccessContext
    {
        /// <summary>
        /// Domain Models for Linq
        /// </summary>
        public static class Linq
        {
            /// <summary>
            /// Current NHibernate Session
            /// </summary>
            private static ISession Session
            {
                get { return UnitOfWork.CurrentSession; }
            }

            /// <summary>
            /// 현재 사용중인 Session을 사용하는 IQueryable{T}를 반환합니다.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static IQueryable<T> GetQuery<T>() where T : DataObjectBase
            {
                return UnitOfWork.CurrentSession.Query<T>();
            }

            /// <summary>
            /// 지정된 <see cref="IStatelessSession"/>을 사용하는 IQueryable{T}를 생성합니다.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="statelessSession">Stateless session</param>
            /// <returns></returns>
            public static IQueryable<T> GetQuery<T>(IStatelessSession statelessSession) where T : DataObjectBase
            {
                return statelessSession.Query<T>();
            }

            #region << Organization >>

            /// <summary>
            /// Code
            /// </summary>
            public static IQueryable<Code> Codes
            {
                get { return GetQuery<Code>(); }
            }

            /// <summary>
            /// Company
            /// </summary>
            public static IQueryable<Company> Companies
            {
                get { return GetQuery<Company>(); }
            }

            /// <summary>
            /// Department
            /// </summary>
            public static IQueryable<Department> Departments
            {
                get { return GetQuery<Department>(); }
            }

            /// <summary>
            /// Department Members
            /// </summary>
            public static IQueryable<DepartmentMember> DepartmentMembers
            {
                get { return GetQuery<DepartmentMember>(); }
            }

            /// <summary>
            /// EmployeeGrade
            /// </summary>
            public static IQueryable<EmployeeGrade> EmployeeGrades
            {
                get { return GetQuery<EmployeeGrade>(); }
            }

            /// <summary>
            /// EmployeePosition
            /// </summary>
            public static IQueryable<EmployeePosition> EmployeePositions
            {
                get { return GetQuery<EmployeePosition>(); }
            }

            /// <summary>
            /// EmployeeTitle
            /// </summary>
            public static IQueryable<EmployeeTitle> EmployeeTitles
            {
                get { return GetQuery<EmployeeTitle>(); }
            }

            /// <summary>
            /// Group (가상 조직)
            /// </summary>
            public static IQueryable<Group> Groups
            {
                get { return GetQuery<Group>(); }
            }

            /// <summary>
            /// 그룹 - 소속원 관계 (소속원은 Company, Department, User 등이 될 수 있다)
            /// </summary>
            public static IQueryable<GroupActor> GroupActors
            {
                get { return GetQuery<GroupActor>(); }
            }

            /// <summary>
            /// User (사용자)
            /// </summary>
            public static IQueryable<User> Users
            {
                get { return GetQuery<User>(); }
            }

            public static IQueryable<User> ActiveUsers
            {
                get { return Users.Where(u => u.IsActive == true); }
            }

            #endregion

            #region << Product >>

            public static IQueryable<Favorite> Favorites
            {
                get { return GetQuery<Favorite>(); }
            }

            public static IQueryable<File> Files
            {
                get { return GetQuery<File>(); }
            }

            public static IQueryable<FileMapping> FileMappings
            {
                get { return GetQuery<FileMapping>(); }
            }

            public static IQueryable<MasterCode> MasterCodes
            {
                get { return GetQuery<MasterCode>(); }
            }

            public static IQueryable<MasterCodeItem> MasterCodeItems
            {
                get { return GetQuery<MasterCodeItem>(); }
            }

            public static IQueryable<Menu> Menus
            {
                get { return GetQuery<Menu>(); }
            }

            public static IQueryable<MenuTemplate> MenuTemplates
            {
                get { return GetQuery<MenuTemplate>(); }
            }

            public static IQueryable<Product> Products
            {
                get { return GetQuery<Product>(); }
            }

            public static IQueryable<Resource> Resources
            {
                get { return GetQuery<Resource>(); }
            }

            public static IQueryable<ResourceActor> ResourceActors
            {
                get { return GetQuery<ResourceActor>(); }
            }

            public static IQueryable<UserConfig> UserConfigs
            {
                get { return GetQuery<UserConfig>(); }
            }

            public static IQueryable<UserLoginLog> UserLoginLogs
            {
                get { return GetQuery<UserLoginLog>(); }
            }

            #endregion

            #region << Calendar >>

            public static IQueryable<Calendar> Calendars
            {
                get { return GetQuery<Calendar>(); }
            }

            public static IQueryable<CalendarRule> CalendarRules
            {
                get { return GetQuery<CalendarRule>(); }
            }

            public static IQueryable<CalendarRuleOfUser> CalendarRuleOfUsers
            {
                get { return GetQuery<CalendarRuleOfUser>(); }
            }

            public static IQueryable<WorkTimeByDay> WorkTimeByDays
            {
                get { return GetQuery<WorkTimeByDay>(); }
            }

            public static IQueryable<WorkTimeByHour> WorkTimeByHours
            {
                get { return GetQuery<WorkTimeByHour>(); }
            }

            public static IQueryable<WorkTimeByMinute> WorkTimeByMinutes
            {
                get { return GetQuery<WorkTimeByMinute>(); }
            }

            public static IQueryable<WorkTimeByRange> WorkTimeByRanges
            {
                get { return GetQuery<WorkTimeByRange>(); }
            }

            #endregion
        }
    }
}