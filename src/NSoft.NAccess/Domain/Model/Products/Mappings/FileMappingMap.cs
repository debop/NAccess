using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model
{
    public class FileMappingMap : ClassMap<FileMapping>
    {
        public FileMappingMap()
        {
            Id(x => x.Id).GeneratedBy.Native();

            Map(x => x.ProductCode).CustomType("AnsiString").Length(128).Not.Nullable().Index("IX_FILE_MAP_PRD");
            Map(x => x.SystemId).Length(128).Not.Nullable().Index("IX_FILE_MAP_PRD");
            Map(x => x.SubId).Length(128).Index("IX_FILE_MAP_PRD");

            Map(x => x.Key1).Length(MappingContext.MaxStringLength);
            Map(x => x.Key2).Length(MappingContext.MaxStringLength);
            Map(x => x.Key3).Length(MappingContext.MaxStringLength);
            Map(x => x.Key4).Length(MappingContext.MaxStringLength);
            Map(x => x.Key5).Length(MappingContext.MaxStringLength);

            Map(x => x.State);

            Map(x => x.CreateDate);
            Map(x => x.DeleteDate);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }
    }
}