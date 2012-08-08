using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 파일 정보 연계를 위한 정보
    /// </summary>
    [Serializable]
    public class FileMapping : DataEntityBase<Int64>, IUpdateTimestampedEntity
    {
        protected FileMapping() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="systemId">시스템 Id</param>
        /// <param name="subId">시스템 Sub Id</param>
        public FileMapping(string productCode, string systemId, string subId = null)
        {
            ProductCode = productCode;
            SystemId = systemId;
            SubId = subId;

            CreateDate = DateTime.Now;
        }

        public virtual string ProductCode { get; protected set; }

        public virtual string SystemId { get; protected set; }
        public virtual string SubId { get; set; }

        public virtual string Key1 { get; set; }
        public virtual string Key2 { get; set; }
        public virtual string Key3 { get; set; }
        public virtual string Key4 { get; set; }
        public virtual string Key5 { get; set; }

        public virtual int? State { get; set; }

        public virtual DateTime? CreateDate { get; set; }
        public virtual DateTime? DeleteDate { get; set; }

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

            return HashTool.Compute(ProductCode, SystemId, SubId, Key1, Key2, Key3, Key4, Key5);
        }

        public override string ToString()
        {
            return string.Format(@"FileMapping# ProductCode={0}, SystemId={1}, SubId={2}, Key1={3}, Key2={4}, Key3={5}. Key4={6}, Key5={7}",
                                 ProductCode, SystemId, SubId, Key1, Key2, Key3, Key4, Key5);
        }
    }
}