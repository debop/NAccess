using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 가상의 조직을 나타내는 그룹 정보 (그룹의 구성은은 <see cref="GroupActor"/>이다.
    /// </summary>
    [Serializable]
    public class Group : LocaledMetadataEntityBase<Int64, GroupLocale>, IUpdateTimestampedEntity
    {
        protected Group() {}

        /// <summary>
        /// 가상의 조직
        /// </summary>
        /// <param name="company"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="kind"></param>
        public Group(Company company, string code, string name = null, GroupKinds kind = GroupKinds.Custom)
        {
            company.ShouldNotBeNull("company");
            code.ShouldNotBeWhiteSpace("code");

            Company = company;
            Code = code;
            Name = name ?? code;

            Kind = kind;

            IsActive = true;
        }

        /// <summary>
        /// 소속 회사
        /// </summary>
        public virtual Company Company { get; protected set; }

        /// <summary>
        /// 그룹 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 그룹 종류
        /// </summary>
        public virtual GroupKinds Kind { get; set; }

        /// <summary>
        /// 사용 여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 동적으로 그룹을 정의할 때 사용할 SqlStatement 문장
        /// TODO : 동적 그룹 조회 기능 추가할 것
        /// </summary>
        public virtual string SqlStatement { get; set; }

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

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(Company, Code);
        }

        public override string ToString()
        {
            return string.Format(@"Group# Id={0}, Code={1}, Name={2}, Kind={3}, Company={4}", Id, Code, Name, Kind, Company);
        }
    }

    /// <summary>
    /// 그룹 지역화 정보
    /// </summary>
    [Serializable]
    public class GroupLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 그룹 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 동적으로 그룹을 정의할 때 사용할 SqlStatement 문장
        /// </summary>
        public virtual string SqlStatement { get; set; }

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
            return string.Format("GroupLocale# Name={0},SqlStatement={1},Description={2},ExAttr={3}", Name, SqlStatement, Description, ExAttr);
        }
    }
}