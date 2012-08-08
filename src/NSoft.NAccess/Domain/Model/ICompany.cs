using System;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    // TODO : 이게 필요할 지 검토해야 함!!!
    /// <summary>
    /// Interface of Company
    /// </summary>
    public interface ICompany : IDataEntity<Int32>, IUpdateTimestampedEntity
    {
        /// <summary>
        /// 회사 코드
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 회사 명
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 사용 여부
        /// </summary>
        bool? IsActive { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 확장 속성
        /// </summary>
        object ExAttr { get; set; }
    }
}