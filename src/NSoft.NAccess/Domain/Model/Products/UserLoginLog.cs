using System;
using System.Globalization;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Tools;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 사용자 시스템 로그인 이력
    /// </summary>
    [Serializable]
    public class UserLoginLog : DataEntityBase<Int64>
    {
        protected UserLoginLog() {}

        /// <summary>
        /// 생성자
        /// </summary>
        public UserLoginLog(string productCode, string companyCode, string loginId, string localeKey = null, DateTime? loginTime = null)
        {
            productCode.ShouldNotBeWhiteSpace("productCode");
            companyCode.ShouldNotBeWhiteSpace("companyCode");
            loginId.ShouldNotBeWhiteSpace("loginId");


            ProductCode = productCode;
            CompanyCode = companyCode;
            LoginId = loginId;

            LocaleKey = (localeKey.IsNotWhiteSpace()) ? localeKey : CultureInfo.CurrentUICulture.Name;
            LoginTime = loginTime ?? DateTime.Now;
        }

        /// <summary>
        /// 제품 코드
        /// </summary>
        public virtual string ProductCode { get; protected set; }

        /// <summary>
        /// 회사 코드
        /// </summary>
        public virtual string CompanyCode { get; protected set; }

        /// <summary>
        /// 부서 코드 (겸직일 경우)
        /// </summary>
        public virtual string DepartmentCode { get; set; }

        /// <summary>
        /// 사용자 코드
        /// </summary>
        public virtual string UserCode { get; set; }

        /// <summary>
        /// 로그인 Id (사용자 코드가 아니다) <see cref="User.LoginId"/>
        /// </summary>
        public virtual string LoginId { get; protected set; }

        /// <summary>
        /// 로그인 시각
        /// </summary>
        public virtual DateTime? LoginTime { get; protected set; }

        /// <summary>
        /// 로그인 시 지역화 키 (예: ko-KR, en-US 등)
        /// </summary>
        public virtual string LocaleKey { get; protected set; }

        /// <summary>
        /// 로그인한 제품의 이름
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 사용자 소속 회사 명
        /// </summary>
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// 사용자 소속 부서 명
        /// </summary>
        public virtual string DepartmentName { get; set; }

        /// <summary>
        /// 로그인 사용자 명
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 확장 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(ProductCode, CompanyCode, LoginId, LocaleKey, LoginTime);
        }

        public override string ToString()
        {
            return string.Format(@"UserLoginLog# ProductCode={0}, CompanyCode={1}, LoginId={2}, LocaleKey={3}, LoginTime={4}",
                                 ProductCode, CompanyCode, LoginId, LocaleKey, LoginTime);
        }
    }
}