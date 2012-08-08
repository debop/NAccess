using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class CodeMap : ClassMap<Code>
    {
        public CodeMap()
        {
            Cache.NonStrictReadWrite().IncludeAll();

            Id(x => x.Id).GeneratedBy.Native();

            Component(x => x.Group, m =>
                                    {
                                        m.Map(x => x.CompanyCode).CustomType("AnsiString").Length(128).Not.Nullable();
                                        m.Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();
                                        m.Map(x => x.Name).CustomType("AnsiString").Length(128).Not.Nullable();
                                    });

            Map(x => x.ItemCode).CustomType("AnsiString").Length(128).Not.Nullable();

            Map(x => x.IsActive);
            Map(x => x.IsSysDefined);
            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp);

            HasMany<CodeLocale>(x => x.LocaleMap)
                .Table("CodeLoc".AsNamingText())
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Fetch.Select()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.GroupName).Not.Nullable();
                               m.Map(x => x.ItemName).Not.Nullable();
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });
        }

        public override NaturalIdPart<Code> NaturalId()
        {
            return base.NaturalId()
                .Property(x => x.Group.CompanyCode)
                .Property(x => x.Group.Code)
                .Property(x => x.Group.Name)
                .Property(x => x.ItemCode);
        }
    }
}