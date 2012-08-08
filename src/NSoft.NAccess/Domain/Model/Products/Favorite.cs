using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 즐겨찾기 정보
    /// </summary>
    [Serializable]
    public class Favorite : DataEntityBase<Int64>, IUpdateTimestampedEntity
    {
        protected Favorite() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="company">회사</param>
        /// <param name="ownerCode">소유자 코드</param>
        /// <param name="ownerKind">소유자 종류</param>
        /// <param name="content">즐겨찾기 내용</param>
        public Favorite(Product product, Company company, string ownerCode, ActorKinds ownerKind = ActorKinds.User, string content = null)
            : this(product.Code, company.Code, ownerCode, ownerKind, content) {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="productCode">제품</param>
        /// <param name="companyCode">회사</param>
        /// <param name="ownerCode">소유자 코드</param>
        /// <param name="ownerKind">소유자 종류</param>
        /// <param name="content">즐겨찾기 내용</param>
        public Favorite(string productCode, string companyCode, string ownerCode, ActorKinds ownerKind = ActorKinds.User, string content = null)
        {
            ProductCode = productCode;
            CompanyCode = companyCode;

            OwnerCode = ownerCode;
            OwnerKind = ownerKind;
            Content = content;

            RegistDate = DateTime.Now;
        }

        /// <summary>
        /// 제품
        /// </summary>
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// 회사
        /// </summary>
        public virtual string CompanyCode { get; set; }

        /// <summary>
        /// 즐겨찾기 소유자 Id (회사|부서|사용자|그룹 의 고유 코드)
        /// </summary>
        public virtual string OwnerCode { get; set; }

        /// <summary>
        /// 즐겨찾기 소유자 종류 (회사|부서|사용자|그룹 등)
        /// </summary>
        public virtual ActorKinds? OwnerKind { get; set; }

        /// <summary>
        /// 소유자 명
        /// </summary>
        public virtual string OwnerName { get; set; }

        /// <summary>
        /// 즐겨찾기 내용
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 등록자
        /// </summary>
        public virtual string RegisterCode { get; set; }

        /// <summary>
        /// 등록일
        /// </summary>
        public virtual DateTime? RegistDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int? Preference { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 특성
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

            return HashTool.Compute(ProductCode, CompanyCode, OwnerCode, OwnerKind, Content);
        }

        public override string ToString()
        {
            return string.Format(@"Favorite# ProductCode={0}, CompanyCode={1}, OwnerCode={2}, OwnerKind={3}, Content={4}",
                                 ProductCode, CompanyCode, OwnerCode, OwnerKind, Content);
        }
    }
}