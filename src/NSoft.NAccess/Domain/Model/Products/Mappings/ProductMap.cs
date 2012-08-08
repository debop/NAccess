using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();

            Map(x => x.Name).Length(128).Not.Nullable();
            Map(x => x.AbbrName).Length(128);

            Map(x => x.IsActive);
            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<ProductLocale>(x => x.LocaleMap)
                .Table("ProductLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name);
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("ProductMeta".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText()).Length(1024);
                           });
        }

        public override NaturalIdPart<Product> NaturalId()
        {
            return base.NaturalId()
                .Property(x => x.Code);
        }
    }
}