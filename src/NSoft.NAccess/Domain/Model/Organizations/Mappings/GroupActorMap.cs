using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model
{
    public class GroupActorMap : ClassMap<GroupActor>
    {
        public GroupActorMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            CompositeId(x => x.Id)
                .KeyProperty(x => x.CompanyCode)
                .KeyProperty(x => x.GroupCode)
                .KeyProperty(x => x.ActorCode)
                .KeyProperty(x => x.ActorKind);

            Map(x => x.Descriptoin).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }
    }

    public class GroupActorIdentityMap : ComponentMap<GroupActorIdentity>
    {
        public GroupActorIdentityMap()
        {
            Map(x => x.CompanyCode).CustomType("AnsiString").Length(128);
            Map(x => x.GroupCode).CustomType("AnsiString").Length(128);
            Map(x => x.ActorCode).CustomType("AnsiString").Length(128);
            Map(x => x.ActorKind).CustomType<ActorKinds>();
        }
    }
}