using FluentNHibernate.Mapping;

namespace NSoft.NAccess.Domain.Model
{
    public class ResourceActorMap : ClassMap<ResourceActor>
    {
        public ResourceActorMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            CompositeId(x => x.Id)
                .KeyProperty(x => x.ProductCode)
                .KeyProperty(x => x.ResourceCode)
                .KeyProperty(x => x.ResourceInstanceId)
                .KeyProperty(x => x.CompanyCode)
                .KeyProperty(x => x.ActorCode)
                .KeyProperty(x => x.ActorKind);

            Map(x => x.AuthorityKind).CustomType<AuthorityKinds>();
        }
    }

    public class ResourceActorIdentityMap : ComponentMap<ResourceActorIdentity>
    {
        public ResourceActorIdentityMap()
        {
            Map(x => x.ProductCode).CustomType("AnsiString").Length(128);
            Map(x => x.ResourceCode).CustomType("AnsiString").Length(128);
            Map(x => x.ResourceInstanceId);
            Map(x => x.CompanyCode).CustomType("AnsiString").Length(128);
            Map(x => x.ActorCode).CustomType("AnsiString").Length(128);
            Map(x => x.ActorKind).CustomType<ActorKinds>();
        }
    }
}