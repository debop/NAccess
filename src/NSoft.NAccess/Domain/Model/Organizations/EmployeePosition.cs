using System;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 직원의 직위 (Position) (예:사원,대리,과장,차장,부장 등) 을 나타낸다.
    /// </summary>
    [Serializable]
    public class EmployeePosition : EmployeeCodeBase
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected EmployeePosition() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="code">직위 Code</param>
        /// <param name="name">직위 명</param>
        public EmployeePosition(Company company, string code, string name = null) : base(company, code, name) {}

        public override string ToString()
        {
            return string.Format(@"EmployeePosition# Code={0}, Name={1}, Company={2}", Code, Name, Company);
        }
    }
}