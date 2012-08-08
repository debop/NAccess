using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 코드 정보
    /// </summary>
    [Serializable]
    public class Code : LocaledEntityBase<Int64, CodeLocale>, IUpdateTimestampedEntity
    {
        protected Code() { }
        public Code(string companyCode, string groupCode, string itemCode) : this(companyCode, groupCode, itemCode, itemCode) { }

        public Code(string companyCode, string groupCode, string itemCode, string itemName)
        {
            itemCode.ShouldNotBeWhiteSpace("itemCode");

            _group = new CodeGroup(companyCode, groupCode);
            ItemCode = itemCode;
            ItemName = itemName ?? itemCode;
        }

        private CodeGroup _group;

        public virtual CodeGroup Group
        {
            get
            {
                return _group ?? (_group = new CodeGroup());
            }
            protected set { _group = value; }
        }

        /// <summary>
        /// 코드 아이템 코드
        /// </summary>
        public virtual string ItemCode { get; set; }

        /// <summary>
        /// 코드 아이템 명
        /// </summary>
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 정렬 순서
        /// </summary>
        public virtual int? ViewOrder { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 시스템 정의 여부
        /// </summary>
        public virtual bool? IsSysDefined { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 추가 속성
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

            return HashTool.Compute(Group, ItemCode);
        }

        public override string ToString()
        {
            return string.Format(@"Code# Id={0}, CodeGroup=[{1}], ItemCode={2}, ItemName={3}, ", Id, Group, ItemCode, ItemName);
        }
    }

    /// <summary>
    /// 코드 지역화 정보
    /// </summary>
    [Serializable]
    public class CodeLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 코드 그룹 명
        /// </summary>
        public virtual string GroupName { get; set; }

        /// <summary>
        /// 코드 명
        /// </summary>
        public virtual string ItemName { get; set; }

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
            return string.Format("CodeLocale# GroupName={0}, ItemName={1}, Description={2}", GroupName, ItemName, Description);
        }
    }
}