using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 코드 정보중에 코드 그룹만을 따로 정보를 추출하기 위해 사용한다.
    /// </summary>
    [Serializable]
    public class CodeGroup : DataObjectBase
    {
        public CodeGroup() {}
        public CodeGroup(string companyCode, string groupCode) : this(companyCode, groupCode, groupCode) {}

        public CodeGroup(string companyCode, string groupCode, string groupName)
        {
            companyCode.ShouldNotBeWhiteSpace("companyCode");
            groupCode.ShouldNotBeWhiteSpace("groupCode");

            CompanyCode = companyCode;
            Code = groupCode;
            Name = groupName;
        }

        /// <summary>
        /// 회사 코드
        /// </summary>
        public virtual string CompanyCode { get; protected set; }

        /// <summary>
        /// 코드의 그룹 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 코드 그룹 명
        /// </summary>
        public virtual string Name { get; set; }

        public override int GetHashCode()
        {
            return HashTool.Compute(CompanyCode, Code);
        }

        public override string ToString()
        {
            return string.Format(@"CodeGroup# CompanyCode={0}, Code={1}, Name={2}", CompanyCode, Code, Name);
        }
    }
}