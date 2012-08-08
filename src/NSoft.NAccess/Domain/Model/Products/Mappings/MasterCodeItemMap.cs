using System;
using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class MasterCodeItemMap : ClassMap<MasterCodeItem>
    {
        public MasterCodeItemMap()
        {
            Cache.Region("NSoft.NAccess").NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Assigned();

            References(x => x.MasterCode)
                .Column("CODE_ID")
                .LazyLoad()
                .Fetch.Select()
                .Not.Nullable()
                .ForeignKey("FK_MC_ITEM_MC");
            Map(x => x.Code, "ITEM_CODE").CustomType("AnsiString").Length(128).Not.Nullable();

            Map(x => x.Name, "ITEM_NAME").Not.Nullable();
            Map(x => x.Value, "ITEM_VALUE").Not.Nullable().Length(MappingContext.MaxStringLength);

            Map(x => x.ViewOrder, "VIEW_ORDER");

            Map(x => x.IsActive);
            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<MasterCodeItemLocale>(x => x.LocaleMap)
                .Table("MASTER_CODE_ITEM_LOC")
                .Access.CamelCaseField(Prefix.Underscore)
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .Fetch.Select()
                .KeyColumn("ITEM_ID")
                .ForeignKeyConstraintName("FK_MC_ITEM_LOC_MC_ITEM")
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Column("ITEM_NAME").Not.Nullable();
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });
        }

        public override NaturalIdPart<MasterCodeItem> NaturalId()
        {
            return base.NaturalId()
                .Reference(x => x.MasterCode)
                .Property(x => x.Code);
        }
    }
}