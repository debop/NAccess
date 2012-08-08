using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 마스터 코드 아이템
    /// </summary>
    [Serializable]
    public class MasterCodeItem : LocaledEntityBase<Guid, MasterCodeItemLocale>, IUpdateTimestampedEntity
    {
        protected MasterCodeItem()
        {
            Id = Guid.NewGuid();
        }

        public MasterCodeItem(MasterCode masterCode, string itemCode, string itemName, string itemValue):this()
        {
            masterCode.ShouldNotBeNull("masterCode");
            itemCode.ShouldNotBeWhiteSpace("itemCode");
            itemName.ShouldNotBeWhiteSpace("itemName");

            MasterCode = masterCode;
            Code = itemCode;
            Name = itemName;
            Value = itemValue;
        }

        /// <summary>
        /// 마스터 코드 
        /// </summary>
        public virtual MasterCode MasterCode { get; set; }

        /// <summary>
        /// 아이템 코드
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Item 표시명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 아이템 값
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Item 정렬 순서
        /// </summary>
        public virtual int? ViewOrder { get; set; }

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
        /// 최종 갱신일
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(MasterCode, Code);
        }

        public override string ToString()
        {
            return string.Format("MasterCodeItem# Id=[{0}], Code=[{1}], Name=[{2}], Value=[{3}], [{4}]", Id, Code, Name, Value, MasterCode);
        }
    }

    /// <summary>
    /// <see cref="MasterCodeItem"/>의 지역화 정보
    /// </summary>
    [Serializable]
    public class MasterCodeItemLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// Item 표시명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 아이템 값
        /// </summary>
        public virtual string Value { get; set; }

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