using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// 파일 정보
    /// </summary>
    [Serializable]
    public class File : DataEntityBase<Guid>, IUpdateTimestampedEntity
    {
        protected File()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="category"></param>
        /// <param name="filename"></param>
        /// <param name="fileMapping"></param>
        public File(string category, string filename, FileMapping fileMapping = null)
        {
            Category = category;
            FileName = filename;
            FileMapping = fileMapping;

            FileSize = 0;
        }

        /// <summary>
        /// 파일 매핑 관련 정보
        /// </summary>
        public virtual FileMapping FileMapping { get; protected set; }

        /// <summary>
        /// 리소스 종류
        /// </summary>
        public virtual string ResourceKind { get; set; }

        /// <summary>
        /// 리소스 Id
        /// </summary>
        public virtual string ResourceId { get; set; }

        /// <summary>
        /// 소유자 Id
        /// </summary>
        public virtual string OwnerCode { get; set; }

        /// <summary>
        /// 소유자 종류
        /// </summary>
        public virtual ActorKinds? OwnerKind { get; set; }

        public virtual string Category { get; set; }
        public virtual string FileName { get; set; }

        public virtual string StoredFileName { get; set; }
        public virtual string StoredFilePath { get; set; }

        public virtual long? FileSize { get; set; }

        /// <summary>
        /// File Content Type (Mime type)
        /// </summary>
        public virtual string FileType { get; set; }

        public virtual string LinkUrl { get; set; }

        /// <summary>
        /// TODO: 이게 뭘 뜻하는지 모르겠네??? 정확한 의미를 파악해서 기술할 것
        /// </summary>
        public virtual int? State { get; set; }

        /// <summary>
        /// TODO: 이게 뭘 뜻하는지 모르겠네??? 정확한 의미를 파악해서 기술할 것
        /// </summary>
        public virtual string StateId { get; set; }

        /// <summary>
        /// 최신버전인지 여부
        /// </summary>
        public virtual bool? IsRecentVersion { get; set; }

        /// <summary>
        /// 파일 버전
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// 파일 버전 설명
        /// </summary>
        public virtual string VersionDesc { get; set; }

        /// <summary>
        /// 파일 그룹
        /// TODO: 이게 뭘 뜻하는지 모르겠네??? 정확한 의미를 파악해서 기술할 것
        /// </summary>
        public virtual int? FileGroup { get; set; }

        /// <summary>
        /// 파일 Floor. 
        /// TODO: 이게 뭘 뜻하는지 모르겠네??? 정확한 의미를 파악해서 기술할 것
        /// </summary>
        public virtual int? FileFloor { get; set; }

        /// <summary>
        /// 생성 일자
        /// </summary>
        public virtual DateTime? CreateDate { get; set; }

        /// <summary>
        /// 삭제 일자 (삭제일자에 값이 존재하고, 현재 날짜보다 값이 작다면 삭제되었다는 뜻이다)
        /// </summary>
        public virtual DateTime? DeleteDate { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 특성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 최종 갱신 시각
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(FileName, Category, FileMapping);
        }

        public override string ToString()
        {
            return string.Format("File# Id=[{0}], StoredFileName=[{1}], Category=[{2}]", Id, StoredFileName, Category);
        }
    }
}