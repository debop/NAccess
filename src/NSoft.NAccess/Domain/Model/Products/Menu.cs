using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 메뉴 정보
    /// </summary>
    [Serializable]
    public class Menu : TreeNodeEntityBase<Menu, Int64>, IUpdateTimestampedEntity
    {
        protected Menu() {}

        public Menu(MenuTemplate menuTemplate, string code)
        {
            menuTemplate.ShouldNotBeNull("menuTemplate");
            code.ShouldNotBeWhiteSpace("code");

            MenuTemplate = menuTemplate;
            Code = code;

            IsActive = true;
        }

        /// <summary>
        /// 메뉴 고유 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 메뉴 템플릿 
        /// </summary>
        public virtual MenuTemplate MenuTemplate { get; protected set; }

        /// <summary>
        /// 사용가능 여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

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

            return HashTool.Compute(Code, MenuTemplate);
        }

        public override string ToString()
        {
            return string.Format(@"Menu# Id={0}, Code={1}, MenuTemplate={2}", Id, Code, MenuTemplate);
        }
    }
}