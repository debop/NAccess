using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// Identity of <see cref="UserConfig"/>
    /// </summary>
    [Serializable]
    public class UserConfigIdentity : DataObjectBase
    {
        protected UserConfigIdentity() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="companyCode">사용자 소속 회사 코드</param>
        /// <param name="userCode">사용자 코드</param>
        /// <param name="key">설정 키</param>
        public UserConfigIdentity(string productCode, string companyCode, string userCode, string key)
        {
            productCode.ShouldNotBeWhiteSpace("productCode");
            companyCode.ShouldNotBeWhiteSpace("companyCode");
            userCode.ShouldNotBeWhiteSpace("userCode");
            key.ShouldNotBeWhiteSpace("key");

            ProductCode = productCode;
            CompanyCode = companyCode;
            UserCode = userCode;
            Key = key;
        }

        /// <summary>
        /// 사용자 설정이 적용된 제품 코드
        /// </summary>
        public virtual string ProductCode { get; protected set; }

        /// <summary>
        /// 회사 코드
        /// </summary>
        public virtual string CompanyCode { get; protected set; }

        /// <summary>
        /// 사용자 코드
        /// </summary>
        public virtual string UserCode { get; protected set; }

        /// <summary>
        /// 설정 키
        /// </summary>
        public virtual string Key { get; protected set; }

        public override int GetHashCode()
        {
            return HashTool.Compute(ProductCode, CompanyCode, UserCode, Key);
        }

        public override string ToString()
        {
            return string.Format(@"UserConfigIdentity# ProductCode={0}, CompanyCode={1}, UserCode={2}, Key={3}",
                                 ProductCode, CompanyCode, UserCode, Key);
        }
    }
}