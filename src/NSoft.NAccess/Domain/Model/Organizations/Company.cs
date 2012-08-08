using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 회사 정보
    /// </summary>
    [Serializable]
    public class Company : LocaledMetadataEntityBase<Int32, CompanyLocale>, IUpdateTimestampedEntity
    {
        protected Company()
        {
            UpdateTimestamp = DateTime.Now;
        }

        public Company(string code) : this()
        {
            code.ShouldNotBeWhiteSpace("code");

            Code = code;
            Name = code;

            IsActive = true;
        }

        /// <summary>
        /// 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 확정 속성
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

            return (Code ?? string.Empty).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(@"Company# Id={0}, Code={1}, Name={2}, IsActive={3}", Id, Code, Name, IsActive);
        }
    }

    /// <summary>
    /// 회사 지역화 정보
    /// </summary>
    [Serializable]
    public class CompanyLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 회사명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확정 속성
        /// </summary>
        public virtual string ExAttr { get; set; }
    }
}