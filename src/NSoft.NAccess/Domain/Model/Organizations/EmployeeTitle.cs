using System;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 직책 (보직) 을 나타낸다. (개발본부장, TFT 팀장, 경영지원실장 등)
    /// </summary>
    [Serializable]
    public class EmployeeTitle : EmployeeCodeBase
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected EmployeeTitle() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="code">직책 코드</param>
        /// <param name="name">직책 명</param>
        public EmployeeTitle(Company company, string code, string name = null) : base(company, code, name) {}

        public override string ToString()
        {
            return string.Format(@"EmployeeTitle# Code={0}, Name={1}, Company={2}", Code, Name, Company);
        }
    }
}