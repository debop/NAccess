using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 그룹-구성원 정보
    /// </summary>
    [Serializable]
    public class GroupActor : DataEntityBase<GroupActorIdentity>, IUpdateTimestampedEntity
    {
        protected GroupActor() {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="group">그룹</param>
        /// <param name="actorCode">그룹 소속원 Id</param>
        /// <param name="actorKind">소속원 종류 (사용자|부서|회사 등)</param>
        public GroupActor(Group group, string actorCode, ActorKinds actorKind) : this(new GroupActorIdentity(group, actorCode, actorKind)) {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="companyCode">회사코드</param>
        /// <param name="groupCode">그룹 코드</param>
        /// <param name="actorCode">소속원 코드</param>
        /// <param name="actorKind">소속원 종류 (회사|부서|사용자 등)</param>
        public GroupActor(string companyCode, string groupCode, string actorCode, ActorKinds actorKind) : this(new GroupActorIdentity(companyCode, groupCode, actorCode, actorKind)) {}

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="identity"></param>
        public GroupActor(GroupActorIdentity identity)
        {
            identity.ShouldNotBeNull("identity");

            Id = identity;
        }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Descriptoin { get; set; }

        /// <summary>
        /// 확장 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 최신 갱신 일자
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
            return string.Format(@"GroupActor# Id={0}, UpdateTimestamp={1}", Id, UpdateTimestamp);
        }
    }
}