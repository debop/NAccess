using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 사원 관련 코드 정보의 추상화 클래스입니다.
    /// </summary>
    [Serializable]
    public abstract class EmployeeCodeBase : LocaledEntityBase<Guid, EmployeeCodeLocale>, ICodeEntity, INamedEntity, IUpdateTimestampedEntity
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected EmployeeCodeBase()
        {
            base.Id = Guid.NewGuid();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="company">회사</param>
        /// <param name="code">직원 코드의 Business적으로 유일한 코드</param>
        /// <param name="name">직원 코드 명</param>
        protected EmployeeCodeBase(Company company, string code, string name = null) : this()
        {
            company.ShouldNotBeNull("company");
            code.ShouldNotBeWhiteSpace("code");

            Company = company;
            Code = code;
            Name = name ?? code;

            ViewOrder = 0;
        }

        /// <summary>
        /// 소속 회사
        /// </summary>
        public virtual Company Company { get; set; }

        /// <summary>
        /// 직원 코드 값
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 코드 표시 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 영문 표시 명
        /// </summary>
        public virtual string EName { get; set; }

        /// <summary>
        /// 정렬 순서
        /// </summary>
        public virtual int? ViewOrder { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 추가 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 최종 갱신 시각
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(Company, Code);
        }
    }

    /// <summary>
    /// 사원관련 코드 정보 (<see cref="EmployeeCodeBase"/>) 에 대한 지역화 정보
    /// </summary>
    [Serializable]
    public class EmployeeCodeLocale : DataObjectBase, ILocaleValue
    {
        public EmployeeCodeLocale() {}

        public EmployeeCodeLocale(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 코드 표시 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 추가 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        public override string ToString()
        {
            return string.Format(@"EmployeeCodeLocale# Name={0}, Description={1}, ExAttr={2}", Name, Description, ExAttr);
        }
    }
}