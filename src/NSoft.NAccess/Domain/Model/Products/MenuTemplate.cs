using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 메뉴 템플릿 정보
    /// </summary>
    [Serializable]
    public class MenuTemplate : LocaledMetadataTreeNodeEntityBase<MenuTemplate, Int64, MenuTemplateLocale>, IUpdateTimestampedEntity
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected MenuTemplate() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="product">제품</param>
        /// <param name="code">메뉴 템플릿 코드</param>
        public MenuTemplate(Product product, string code)
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            Product = product;
            Code = code;
            Name = code;
        }

        /// <summary>
        /// 제품
        /// </summary>
        public virtual Product Product { get; protected set; }

        /// <summary>
        /// 메뉴 템플릿 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 메뉴 템플릿 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 메뉴의 Script Path
        /// </summary>
        public virtual string MenuUrl { get; set; }

        /// <summary>
        /// Tree Path of MenuTemplate Code
        /// </summary>
        public virtual string TreePath { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 속성
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

            return HashTool.Compute(Product, Code);
        }

        public override string ToString()
        {
            return string.Format(@"MenuTemplate# Code={0}, Name={1}, Url={2}, Product={3}", Code, Name, MenuUrl, Product);
        }
    }

    /// <summary>
    /// 메뉴 템플릿 지역화 정보
    /// </summary>
    [Serializable]
    public class MenuTemplateLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 메뉴 템플릿 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 메뉴의 Script Path
        /// </summary>
        public virtual string MenuUrl { get; set; }

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