using System.Linq;
using NSoft.NFramework;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess
{
    public static partial class NAccessContext
    {
        public const string CurrentCompanyCodeKey = @"NSoft.NAccess.Current.CompanyCode.Key";
        public const string CurrentDepartmentCodeKey = @"NSoft.NAccess.Current.DepartmentCode.Key";
        public const string CurrentUserCodeKey = @"NSoft.NAccess.Current.UserCode.Key";

        /// <summary>
        /// Current Thread Context에서 공통으로 사용할 정보를 나타냅니다.
        /// </summary>
        public static class Current
        {
            private static readonly object _syncLock = new object();

            /// <summary>
            /// 현재 지정된 회사 코드
            /// </summary>
            public static string CompanyCode
            {
                get { return (string) Local.Data[CurrentCompanyCodeKey]; }
                set { Local.Data[CurrentCompanyCodeKey] = value; }
            }

            /// <summary>
            /// 현 컨텍스트에 지정된 <see cref="Company"/> 정보
            /// </summary>
            public static Company Company
            {
                get { return Linq.Companies.SingleOrDefault(c => c.Code == CompanyCode); }
            }

            /// <summary>
            /// 현재 부서 코드
            /// </summary>
            public static string DepartmentCode
            {
                get { return (string) Local.Data[CurrentDepartmentCodeKey]; }
                set { Local.Data[CurrentDepartmentCodeKey] = value; }
            }

            /// <summary>
            /// 현 컨텍스트에 지정된 부서 정보
            /// </summary>
            public static Department Department
            {
                get { return Linq.Departments.SingleOrDefault(dept => dept.Company.Code == CompanyCode && dept.Code == DepartmentCode); }
            }

            /// <summary>
            /// 현재 사용자 코드
            /// </summary>
            public static string UserCode
            {
                get { return (string) Local.Data[CurrentUserCodeKey]; }
                set { Local.Data[CurrentUserCodeKey] = value; }
            }

            /// <summary>
            /// 현 컨텍스트에 지정된 사용자 정보
            /// </summary>
            public static User User
            {
                get { return Linq.Users.SingleOrDefault(user => user.Company.Code == CompanyCode && user.Code == UserCode); }
            }
        }
    }
}