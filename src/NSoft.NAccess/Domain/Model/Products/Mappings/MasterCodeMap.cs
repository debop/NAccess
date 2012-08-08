using System;
using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class MasterCodeMap : ClassMap<MasterCode>
    {
        public MasterCodeMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Assigned();

            References(x => x.Product).Fetch.Select().Not.Nullable().LazyLoad().Index("IX_MASTERCODE_PRD");
            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();

            Map(x => x.Name).Not.Nullable().Length(128);
            Map(x => x.IsActive);
            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany(x => x.Items)
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsSet();

            HasMany<MasterCodeLocale>(x => x.LocaleMap)
                .Table("MasterCodeLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Column("CodeName".AsNamingText()).Not.Nullable().Length(128);
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });
        }
    }
}