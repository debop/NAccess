using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Table("`Company`".AsNamingText());

            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();

            Map(x => x.Name).Not.Nullable();
            Map(x => x.IsActive);
            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<CompanyLocale>(x => x.LocaleMap)
                .Table("CompanyLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Column("CompanyName".AsNamingText()).Not.Nullable();
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });

            //
            // NOTE: MetadataMap 이 IDictionary<string, IMetadataValue> 로 정의되어, Instancing 예외가 발생한다.
            // NOTE: Interface 와 관련된다면, 이렇게 관련 Concrete Class 로 매핑해주어야 한다.
            //
            HasMany<MetadataValue>(x => x.MetadataMap)
                .Table("CompanyMeta".AsNamingText())
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .AsMap<string>("MetaKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Value).Column("MetaValue".AsNamingText()).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ValueType).Column("MetaValueType".AsNamingText()).Length(1024);
                           });
        }

        public override NaturalIdPart<Company> NaturalId()
        {
            return base.NaturalId()
                .Property(x => x.Code);
        }
    }
}