using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 제품 (Product) 정보
    /// </summary>
    [Serializable]
    public class Product : LocaledMetadataEntityBase<Int64, ProductLocale>, IUpdateTimestampedEntity
    {
        protected Product() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="code">제품 코드</param>
        /// <param name="name">제품 명</param>
        /// <param name="isActive">활성화 여부</param>
        public Product(string code, string name = null, bool isActive = true)
        {
            code.ShouldNotBeWhiteSpace("code");

            Code = code;
            Name = name ?? code;

            IsActive = isActive;
        }

        /// <summary>
        /// 제품 코드 (고유한 값이어야 한다)
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 제품 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 제품 축약 명
        /// </summary>
        public virtual string AbbrName { get; set; }

        /// <summary>
        /// 사용 여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 특성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 최종 갱신일
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
            return string.Format(@"Product# Id={0}, Code={1}, Name={2}, IsActive={3},Description={4}",
                                 Id, Code, Name, IsActive, Description);
        }
    }

    /// <summary>
    /// 제품 지역화 정보
    /// </summary>
    [Serializable]
    public class ProductLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 특성
        /// </summary>
        public virtual string ExAttr { get; set; }
    }
}