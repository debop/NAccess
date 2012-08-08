using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// Composite Id of <see cref="ResourceActor"/>
    /// </summary>
    [Serializable]
    public class ResourceActorIdentity : DataObjectBase
    {
        protected ResourceActorIdentity() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="resourceCode">리소스 종류 코드</param>
        /// <param name="resourceInstanceId">리소스 인스턴스의 Id</param>
        /// <param name="companyCode">회사 코드</param>
        /// <param name="actorCode">접근자 Id (회사,부서,그룹,사용자)</param>
        /// <param name="actorKind">접근자 종류</param>
        public ResourceActorIdentity(string productCode,
                                     string resourceCode,
                                     string resourceInstanceId,
                                     string companyCode,
                                     string actorCode,
                                     ActorKinds actorKind = ActorKinds.User)
        {
            productCode.ShouldNotBeWhiteSpace("productCode");
            resourceCode.ShouldNotBeWhiteSpace("resourceCode");
            resourceInstanceId.ShouldNotBeWhiteSpace("resourceInstanceId");
            companyCode.ShouldNotBeWhiteSpace("companyCode");
            actorCode.ShouldNotBeWhiteSpace("actorCode");

            ProductCode = productCode;
            ResourceCode = resourceCode;
            ResourceInstanceId = resourceInstanceId;

            CompanyCode = companyCode;
            ActorCode = actorCode;
            ActorKind = actorKind;
        }

        /// <summary>
        /// 제품 코드
        /// </summary>
        public virtual string ProductCode { get; protected set; }

        /// <summary>
        /// 리소스 종류 코드
        /// </summary>
        public virtual string ResourceCode { get; protected set; }

        /// <summary>
        /// 리소스 인스턴스의 ID 값(접근 대상) 
        /// </summary>
        public virtual string ResourceInstanceId { get; protected set; }

        /// <summary>
        /// 회사 
        /// </summary>
        public virtual string CompanyCode { get; protected set; }

        /// <summary>
        /// Actor Code (접근자 코드 : 회사|부서|그룹|사용자 코드)
        /// </summary>
        public virtual string ActorCode { get; protected set; }

        /// <summary>
        /// 접근자 종류 (부서|사용자|그룹 등)
        /// </summary>
        public virtual ActorKinds? ActorKind { get; protected set; }

        public override int GetHashCode()
        {
            return HashTool.Compute(ProductCode, ResourceCode, ResourceInstanceId, CompanyCode, ActorCode, ActorKind);
        }

        public override string ToString()
        {
            return string.Format(@"ResourceActorIdentity# ProductCode={0}, ResourceCode={1}, ResourceInstanceId={2}, CompanyCode={3}, ActorCode={4}, ActorKind={5}",
                                 ProductCode, ResourceCode, ResourceInstanceId, CompanyCode, ActorCode, ActorKind);
        }
    }
}