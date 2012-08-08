using System;
using Iesi.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 사용자 정보 (직원을 포함한 사용자이다)
    /// </summary>
    [Serializable]
    public class User : LocaledMetadataEntityBase<Int32, UserLocale>, IUpdateTimestampedEntity
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected User()
        {
            IsActive = true;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="company">소속 회사</param>
        /// <param name="code">사용자 코드</param>
        public User(Company company, string code)
            : this()
        {
            company.ShouldNotBeNull("company");
            code.ShouldNotBeWhiteSpace("code");

            Company = company;
            Code = code;
            Name = code;
            EmpNo = code;

            LoginId = code;
        }

        /// <summary>
        /// 소속 회사
        /// </summary>
        public virtual Company Company { get; protected set; }

        /// <summary>
        /// 사용자 코드 (Business 적으로 Unique한 값을 나타낸다) (Login Id 또는 EmpNo 등이 쓰일 수 있다)
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 사용자 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// User English Name (for performance)
        /// </summary>
        public virtual string EName { get; set; }

        /// <summary>
        /// 사번 (Id, LoginId 와는 별개입니다!!!) 
        /// </summary>
        public virtual string EmpNo { get; set; }

        /// <summary>
        /// 직위 (대리, 과장, 부장, 이사 등)를 나타냄
        /// </summary>
        public virtual EmployeePosition Position { get; set; }

        /// <summary>
        /// 직급 (1급, 3급 등 호봉 체계를 나타냄)
        /// </summary>
        public virtual EmployeeGrade Grade { get; set; }

        /// <summary>
        /// 사용자 종류
        /// </summary>
        public virtual int? Kind { get; set; }

        /// <summary>
        /// 사용자 로그인 계정
        /// </summary>
        public virtual string LoginId { get; set; }

        /// <summary>
        /// 사용자 로그인 비밀번호
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 주민 번호
        /// </summary>
        public virtual string IdentityNumber { get; set; }

        /// <summary>
        /// 사용자 역할 코드
        /// </summary>
        public virtual string RoleCode { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 유선전화번호
        /// </summary>
        public virtual string Telephone { get; set; }

        /// <summary>
        /// 휴대전화번호
        /// </summary>
        public virtual string MobilePhone { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 사용자 상태 Flag (파견|휴직|퇴직 등)
        /// </summary>
        public virtual string StatusFlag { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 추가 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

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
        /// 최종 갱신 시각
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(Company, Code);
        }

        public override string ToString()
        {
            return string.Format(@"User# Id={0}, Code={1}, Name={2}, EmpNo={3}, Email={4}, Company={5}",
                                 Id, Code, Name, EmpNo, Email, Company);
        }
    }

    /// <summary>
    /// 사용자 지역화 정보
    /// </summary>
    [Serializable]
    public class UserLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// User Name
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