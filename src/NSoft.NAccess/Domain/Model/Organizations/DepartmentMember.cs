using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 부서의 구성원 정보 (부서 - 사원의 구성 정보)
    /// </summary>
    [Serializable]
    public class DepartmentMember : DataEntityBase<Int32>, IUpdateTimestampedEntity
    {
        protected DepartmentMember() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="department">부서</param>
        /// <param name="user">사용자</param>
        public DepartmentMember(Department department, User user)
        {
            department.ShouldNotBeNull("department");
            user.ShouldNotBeNull("user");

            Department = department;
            User = user;

            IsLeader = false;
            IsActive = true;
        }

        /// <summary>
        /// 부서
        /// </summary>
        public virtual Department Department { get; set; }

        /// <summary>
        /// 사용자
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 직책 정보
        /// </summary>
        public virtual EmployeeTitle EmployeeTitle { get; set; }

        /// <summary>
        /// 조직 책임자인가?
        /// </summary>
        public virtual bool? IsLeader { get; set; }

        /// <summary>
        /// 사용 여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 정보 최종 갱신일
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(Department, User);
        }

        public override string ToString()
        {
            return string.Format("DepartmentMember# Department={0}, User={1}, EmployeeTitle={2}, IsLeader={3}, IsActive={4}",
                                 Department, User, EmployeeTitle, IsLeader, IsActive);
        }
    }
}