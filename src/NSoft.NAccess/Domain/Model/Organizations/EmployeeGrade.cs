using System;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 직원 직급 (Grade) 정보 (예: 1급, 2급 등 호봉체계에 사용된다.)
    /// </summary>
    [Serializable]
    public class EmployeeGrade : EmployeeCodeBase
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected EmployeeGrade() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="code">직급 코드</param>
        /// <param name="name">직급 명</param>
        public EmployeeGrade(Company company, string code, string name = null) : base(company, code, name) {}

        /// <summary>
        /// 상위 직급 코드
        /// </summary>
        public virtual string ParentCode { get; set; }

        /// <summary>
        /// 시간당 급여 (시급)
        /// </summary>
        public virtual decimal? HourlyWages { get; set; }

        public override string ToString()
        {
            return string.Format(@"EmployeeGrade# Code={0}, Name={1}, Company={2}, ParentCode={3}, HourlyWages={4}",
                                 Code, Name, Company, ParentCode, HourlyWages);
        }
    }
}