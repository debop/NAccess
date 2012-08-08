using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 리소스에 대한 사용자의 접근 권한 정보
    /// </summary>
    [Serializable]
    public class ResourceActor : DataEntityBase<ResourceActorIdentity>, IUpdateTimestampedEntity
    {
        /// <summary>
        /// 생성자
        /// </summary>
        protected ResourceActor() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="resource">리소스 종류 정보</param>
        /// <param name="resourceInstanceId">리소스 인스턴스의 Id</param>
        /// <param name="companyCode">회사</param>
        /// <param name="actorCode">접근자 Id (회사,부서,그룹,사용자)</param>
        /// <param name="actorKind">접근자 종류</param>
        /// <param name="authorityKind">접근 권한 종류</param>
        public ResourceActor(Resource resource,
                             string resourceInstanceId,
                             string companyCode,
                             string actorCode,
                             ActorKinds actorKind = ActorKinds.User,
                             AuthorityKinds authorityKind = AuthorityKinds.All)
            : this(new ResourceActorIdentity(resource.ProductCode, resource.Code, resourceInstanceId, companyCode, actorCode, actorKind), authorityKind) {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="resourceCode">리소스 종류 코드</param>
        /// <param name="resourceInstanceId">리소스 인스턴스의 Id</param>
        /// <param name="companyCode">회사</param>
        /// <param name="actorCode">접근자 Id (회사,부서,그룹,사용자)</param>
        /// <param name="actorKind">접근자 종류</param>
        /// <param name="authorityKind">접근 권한 종류</param>
        public ResourceActor(string productCode,
                             string resourceCode,
                             string resourceInstanceId,
                             string companyCode,
                             string actorCode,
                             ActorKinds actorKind = ActorKinds.User,
                             AuthorityKinds authorityKind = AuthorityKinds.All)
            : this(new ResourceActorIdentity(productCode, resourceCode, resourceInstanceId, companyCode, actorCode, actorKind), authorityKind) {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="identity">ResourceActor의 composite-id</param>
        /// <param name="authorityKind">접근 권한 종류</param>
        public ResourceActor(ResourceActorIdentity identity, AuthorityKinds authorityKind = AuthorityKinds.All)
        {
            identity.ShouldNotBeNull("identity");

            Id = identity;
            AuthorityKind = authorityKind;
        }

        /// <summary>
        /// 접근 권한 종류 (읽기|쓰기|삭제 등)
        /// </summary>
        public virtual AuthorityKinds AuthorityKind { get; set; }

        /// <summary>
        /// 확장 속성 정보
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

            return HashTool.Compute(Id);
        }

        public override string ToString()
        {
            return string.Format(@"ResourceActor# Id={0}, AuthorityKind={1}", Id, AuthorityKind);
        }
    }
}