using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model
{
    public class FavoriteMap : ClassMap<Favorite>
    {
        public FavoriteMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.ProductCode).CustomType("AnsiString").Length(128).Not.Nullable();
            Map(x => x.CompanyCode).CustomType("AnsiString").Length(128).Not.Nullable();
            Map(x => x.OwnerCode).CustomType("AnsiString").Length(128).Not.Nullable();
            Map(x => x.OwnerKind).CustomType<ActorKinds>();

            Map(x => x.OwnerName);
            Map(x => x.Content).Length(MappingContext.MaxStringLength);

            Map(x => x.RegisterCode).CustomType("AnsiString").Length(128);
            Map(x => x.RegistDate);

            Map(x => x.Preference);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }

        public override NaturalIdPart<Favorite> NaturalId()
        {
            return base.NaturalId()
                .Property(x => x.ProductCode)
                .Property(x => x.CompanyCode)
                .Property(x => x.OwnerCode)
                .Property(x => x.OwnerKind);
        }
    }
}