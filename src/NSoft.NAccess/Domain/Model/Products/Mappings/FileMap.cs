using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model
{
    public class FileMap : ClassMap<File>
    {
        public FileMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            References(x => x.FileMapping).LazyLoad().Fetch.Select();

            Map(x => x.Category).Length(50);
            Map(x => x.FileName).Length(1024).Not.Nullable();

            Map(x => x.ResourceId).Index("IX_FILE_RESOURCE");
            Map(x => x.ResourceKind).Index("IX_FILE_RESOURCE");

            Map(x => x.OwnerCode).CustomType("AnsiString").Length(128);
            Map(x => x.OwnerKind).CustomType<ActorKinds>();

            Map(x => x.StoredFileName).Length(1024);
            Map(x => x.StoredFilePath).Length(1024);

            Map(x => x.FileSize);
            Map(x => x.FileType).CustomType("AnsiString").Length(128);

            Map(x => x.LinkUrl).Length(1024);
            Map(x => x.State);
            Map(x => x.StateId).CustomType("AnsiString").Length(50);

            Map(x => x.IsRecentVersion);
            Map(x => x.Version).CustomType("AnsiString").Length(32);
            Map(x => x.VersionDesc).Length(MappingContext.MaxStringLength);

            Map(x => x.FileGroup);
            Map(x => x.FileFloor);

            Map(x => x.CreateDate);
            Map(x => x.DeleteDate);

            Map(x => x.Description).Length(MappingContext.MaxStringLength);
            Map(x => x.ExAttr).Length(MappingContext.MaxStringLength);

            Map(x => x.UpdateTimestamp).CustomType("Timestamp");
        }

        public override NaturalIdPart<File> NaturalId()
        {
            return base.NaturalId()
                .Reference(x => x.FileMapping)
                .Property(x => x.Category)
                .Property(x => x.FileName);
        }
    }
}