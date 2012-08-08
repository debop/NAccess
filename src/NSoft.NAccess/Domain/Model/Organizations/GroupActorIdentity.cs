using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// Composite Id of GroupActor
    /// </summary>
    [Serializable]
    public class GroupActorIdentity : DataObjectBase
    {
        protected GroupActorIdentity() {}
        public GroupActorIdentity(Group group, string actorCode, ActorKinds actorKind = ActorKinds.User) : this(group.Company.Code, group.Code, actorCode, actorKind) {}

        public GroupActorIdentity(string companyCode, string groupCode, string actorCode, ActorKinds actorKind = ActorKinds.User)
        {
            companyCode.ShouldNotBeWhiteSpace("companyCode");
            groupCode.ShouldNotBeWhiteSpace("groupCode");
            actorCode.ShouldNotBeWhiteSpace("actorCode");

            CompanyCode = companyCode;
            GroupCode = groupCode;
            ActorCode = actorCode;
            ActorKind = actorKind;
        }

        /// <summary>
        /// 회사 코드
        /// </summary>
        public virtual string CompanyCode { get; set; }

        /// <summary>
        /// 그룹 코드
        /// </summary>
        public virtual string GroupCode { get; set; }

        /// <summary>
        /// 그룹 소속원 코드
        /// </summary>
        public virtual string ActorCode { get; set; }

        /// <summary>
        /// 그룹 소속원 종류 (회사|부서|사용자 ...)
        /// </summary>
        public virtual ActorKinds? ActorKind { get; set; }

        public override int GetHashCode()
        {
            return HashTool.Compute(CompanyCode, GroupCode, ActorCode, ActorKind);
        }

        public override string ToString()
        {
            return string.Format(@"GroupActorIdentity# CompanyCode={0}, GroupCode={1}, ActorCode={2}, ActorKind={3}",
                                 CompanyCode, GroupCode, ActorCode, ActorKind);
        }
    }
}