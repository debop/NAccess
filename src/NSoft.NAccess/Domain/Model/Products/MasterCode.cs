using System;
using System.Collections.Generic;
using System.Linq;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Tools;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 마스터 코드 정보
    /// </summary>
    [Serializable]
    public class MasterCode : LocaledEntityBase<Guid, MasterCodeLocale>, IUpdateTimestampedEntity
    {
        protected MasterCode()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="product">소속 제품</param>
        /// <param name="code">코드 코드</param>
        /// <param name="name">코드 명</param>
        public MasterCode(Product product, string code, string name = null): this()
        {
            product.ShouldNotBeNull("product");
            code.ShouldNotBeWhiteSpace("code");

            Product = product;
            Code = code;
            Name = name ?? code;

            IsActive = true;
        }

        /// <summary>
        /// 소속 제품
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// 마스터 코드 코드
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 마스터 코드 표시명
        /// </summary>
        public virtual string Name { get; set; }

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
        /// 최종 갱신일자
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        private Iesi.Collections.Generic.ISet<MasterCodeItem> _items;

        /// <summary>
        /// 마스터 코드 아이템 컬렉션
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<MasterCodeItem> Items
        {
            get { return _items ?? (_items = new Iesi.Collections.Generic.HashedSet<MasterCodeItem>()); }
            protected set { _items = value; }
        }

        public virtual IEnumerable<MasterCodeItem> GetSortedCodeItems()
        {
            return Items.OrderBy(x => x.ViewOrder.GetValueOrDefault());
        }

        public virtual MasterCodeItem this[int index]
        {
            get { return Items.ElementAt(index); }
        }

        public virtual MasterCodeItem this[string itemCode]
        {
            get { return Items.FirstOrDefault(x => x.Code.EqualTo(itemCode)); }
        }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(Product, Code);
        }
    }

    [Serializable]
    public class MasterCodeLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 마스터 코드 표시명
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