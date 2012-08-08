using System;
using System.Collections.Generic;
using System.Linq;
using Iesi.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 부서 정보  (난이도 때문에 Association을 제거했다.
    /// </summary>
    [Serializable]
    public class Department : LocaledMetadataTreeNodeEntityBase<Department, Int32, DepartmentLocale>, IUpdateTimestampedEntity
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected Department()
        {
            UpdateTimestamp = DateTime.Now;
            IsActive = true;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="code">부서 코드</param>
        /// <param name="name">부서 명</param>
        public Department(Company company, string code, string name = null)
            : this()
        {
            company.ShouldNotBeNull("company");
            code.ShouldNotBeWhiteSpace("code");

            Company = company;
            Code = code;
            Name = name ?? code;
        }

        /// <summary>
        /// 소속 회사
        /// </summary>
        public virtual Company Company { get; protected set; }

        /// <summary>
        /// 부서 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 부서명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 부서 영문 명 (속도를 위해)
        /// </summary>
        public virtual string EName { get; set; }

        /// <summary>
        /// 부서 종류
        /// </summary>
        public virtual string Kind { get; set; }

        /// <summary>
        /// 사용 여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 추가 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 최종갱신일
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        private Iesi.Collections.Generic.ISet<DepartmentMember> _members;

        /// <summary>
        /// 조직 구성원 정보
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<DepartmentMember> Members
        {
            get { return _members ?? (_members = new HashedSet<DepartmentMember>()); }
            protected set { _members = value; }
        }

        /// <summary>
        /// 소속 직원의 컬렉션을 반환합니다.
        /// </summary>
        /// <remarks>
        /// ( Members.Select(m=>m.User) 를 직접 호출하는 걸 추천합니다.^^ (IsActive 속성으로 필터링도 가능하기 때문에 )
        /// </remarks>
        /// <returns></returns>
        public virtual IEnumerable<User> GetUsers()
        {
            return Members.Select(m => m.User);
        }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(Company, Code);
        }

        public override string ToString()
        {
            return string.Format(@"Department# Id={0}, Code={1}, Name={2}, Company={3}", Id, Code, Name, Company);
        }
    }

    /// <summary>
    /// 부서 지역화 정보
    /// </summary>
    [Serializable]
    public class DepartmentLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 부서명
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
    }
}