using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain.UserTypes;

namespace NSoft.NAccess.Domain.Model
{
    public class EmployeeCodeBaseMap : ClassMap<EmployeeCodeBase>
    {
        public EmployeeCodeBaseMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            Id(x => x.Id).UnsavedValue(null).GeneratedBy.GuidComb();

            DiscriminateSubClassesOnColumn("CodeKind".AsNamingText()).Length(32).Not.Nullable();

            References(x => x.Company).Fetch.Select().LazyLoad().Not.Nullable();
            Map(x => x.Code).CustomType("AnsiString").Length(128).Not.Nullable();

            Map(x => x.Name).Length(128).Not.Nullable();
            Map(x => x.EName).Length(128);

            Map(x => x.ViewOrder);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");

            HasMany<EmployeeCodeLocale>(x => x.LocaleMap)
                .Table("EmployeeCodeLoc".AsNamingText())
                .Cascade.AllDeleteOrphan()
                .Fetch.Select()
                .LazyLoad()
                .AsMap<CultureUserType>("LocaleKey".AsNamingText())
                .Component(m =>
                           {
                               m.Map(x => x.Name).Length(128);
                               m.Map(x => x.Description).Length(MappingContext.MaxStringLength);
                               m.Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);
                           });
        }
    }

    public class EmployeeGradeMap : SubclassMap<EmployeeGrade>
    {
        public EmployeeGradeMap()
        {
            DiscriminatorValue("Grade");

            Map(x => x.ParentCode).CustomType("AnsiString").Length(128);
            Map(x => x.HourlyWages).CustomType<decimal>();
        }
    }

    public class EmployeePositionMap : SubclassMap<EmployeePosition>
    {
        public EmployeePositionMap()
        {
            DiscriminatorValue("Position");
        }
    }

    public class EmployeeTitleMap : SubclassMap<EmployeeTitle>
    {
        public EmployeeTitleMap()
        {
            DiscriminatorValue("Title");
        }
    }
}