using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model
{
    public class DepartmentMemberMap : ClassMap<DepartmentMember>
    {
        public DepartmentMemberMap()
        {
            Cache.Region("NSoft.NAccess").ReadWrite().IncludeAll();

            Id(x => x.Id).UnsavedValue(0).GeneratedBy.Native();

            References(x => x.Department).Fetch.Select().LazyLoad().Not.Nullable();
            References(x => x.User).Fetch.Select().LazyLoad().Not.Nullable();
            References(x => x.EmployeeTitle).Fetch.Select().LazyLoad();

            Map(x => x.IsLeader);
            Map(x => x.IsActive);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }

        public override NaturalIdPart<DepartmentMember> NaturalId()
        {
            return base.NaturalId()
                .Reference(x => x.Department)
                .Reference(x => x.User);
        }
    }
}