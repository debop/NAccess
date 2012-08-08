using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 사용자 환경 설정 정보
    /// </summary>
    [Serializable]
    public class UserConfig : DataEntityBase<UserConfigIdentity>, IUpdateTimestampedEntity
    {
        protected UserConfig() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">사용자 소속 회사 코드</param>
        /// <param name="userCode">사용자 코드</param>
        /// <param name="key">설정 키</param>
        /// <param name="value">설정 값</param>
        public UserConfig(string productCode, string companyCode, string userCode, string key, string value)
            : this(new UserConfigIdentity(productCode, companyCode, userCode, key), value) {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="identity">Identity</param>
        /// <param name="value">설정 값</param>
        public UserConfig(UserConfigIdentity identity, string value)
        {
            identity.ShouldNotBeNull("identity");

            Id = identity;
            Value = value;
        }

        /// <summary>
        /// 사용자 설정 값
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// 기본 값
        /// </summary>
        public virtual string DefaultValue { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장속성
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

            return HashTool.Compute(Id);
        }

        public override string ToString()
        {
            return string.Format("UserConfig# Id={0}, Value={1}, DefaultValue={2}", Id, Value, DefaultValue);
        }
    }
}