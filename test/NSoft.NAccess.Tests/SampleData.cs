using System.Collections.Generic;
using System.Globalization;

namespace NSoft.NAccess
{
    public static class SampleData
    {
        public const int MaxSampleCount = 10;
        public const int AvgSampleCount = 5;
        public const int MinSampleCount = 3;

        public const string CompanyCode = "RealWeb";
        public static readonly string DepartmentCode = DepartmentCodes[0];
        public static readonly string UserCode = UserCodes[0];

        private static IList<string> _companyCodes;

        public static IList<string> CompanyCodes
        {
            get { return _companyCodes ?? (_companyCodes = GetCodes("CO_", 2)); }
        }

        private static IList<string> _codeGroupIds;

        public static IList<string> CodeGroupIds
        {
            get { return _codeGroupIds ?? (_codeGroupIds = GetCodes("GROUP_", MinSampleCount)); }
        }

        private static IList<string> _codeIds;

        public static IList<string> CodeIds
        {
            get { return _codeIds ?? (_codeIds = GetCodes("CODE_", MinSampleCount)); }
        }

        private static IList<string> _departmentCodes;

        public static IList<string> DepartmentCodes
        {
            get { return _departmentCodes ?? (_departmentCodes = GetCodes("DEPT_", AvgSampleCount)); }
        }

        private static IList<string> _userCodes;

        public static IList<string> UserCodes
        {
            get { return _userCodes ?? (_userCodes = GetCodes("USER_", MaxSampleCount)); }
        }

        public static IList<string> GetCodes(string prefix, int count)
        {
            var result = new List<string>(count);

            for(var i = 0; i < count; i++)
                result.Add(prefix + i.ToString("X4"));

            return result;
        }

        /// <summary>
        /// 미국 문화권
        /// </summary>
        public static readonly CultureInfo English = new CultureInfo("en-US");

        /// <summary>
        /// 한국 문화권
        /// </summary>
        public static readonly CultureInfo Korean = new CultureInfo("ko-KR");

        public const string ProductCode = "RealAdmin";
        public static readonly string[] ProductCodes = new string[] {"RealAdmin", "RealBPA", "RealBPM", "RealEA", "RealPMS", "RealPPMS"};
    }
}