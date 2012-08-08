using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 리소스 종류 정보
    /// </summary>
    [Serializable]
    public class Resource : LocaledMetadataEntityBase<Int64, ResourceLocale>, IUpdateTimestampedEntity
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected Resource() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="productCode">리소스를 정의한 제품 코드</param>
        /// <param name="code">리소스 코드</param>
        public Resource(string productCode, string code)
        {
            productCode.ShouldNotBeWhiteSpace("productCode");
            code.ShouldNotBeWhiteSpace("code");

            ProductCode = productCode;
            Code = code;
            Name = code;
        }

        /// <summary>
        /// 제품 코드
        /// </summary>
        public virtual string ProductCode { get; protected set; }

        /// <summary>
        /// 리소스 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 리소스 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 속성
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

            return HashTool.Compute(ProductCode, Code);
        }

        public override string ToString()
        {
            return string.Format(@"Resource# Id=[{0}], Code=[{1}], Name=[{2}], ProductCode=[{3}]", Id, Code, Name, ProductCode);
        }
    }

    [Serializable]
    public class ResourceLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 리소스 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 속성
        /// </summary>
        public virtual string ExAttr { get; set; }
    }
}