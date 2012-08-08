using System.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Reflections;
using NSoft.NFramework.Tools;
using NHibernate.Criterion;
using NSoft.NAccess.Domain.Model;

namespace NSoft.NAccess.Domain.Repositories
{
    /// <summary>
    /// 파일 관련 Domain Service
    /// </summary>
    public partial class ProductRepository
    {
        /// <summary>
        /// FileMapping 정보 조회를 위한 QueryOver를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="systemId">시스템 ID</param>
        /// <param name="subId">서브 ID</param>
        /// <param name="keyValues">키-값 설정</param>
        /// <param name="state">상태</param>
        /// <returns></returns>
        public QueryOver<FileMapping, FileMapping> BuildQueryOverOfFileMapping(string productCode,
                                                                               string systemId,
                                                                               string subId = null,
                                                                               IDictionary<string, string> keyValues = null,
                                                                               int? state = null)
        {
            if(IsDebugEnabled)
                log.Debug(@"FileMapping 정보 조회를 위한 Critieria를 빌드합니다... " +
                          @"productCode={0}, systemId={1}, subId={2}, keyValues={3}, state={4}",
                          productCode, systemId, subId, keyValues.DictionaryToString(), state);

            var query = QueryOver.Of<FileMapping>();

            if(productCode.IsNotWhiteSpace())
                query.AddWhere(fm => fm.ProductCode == productCode);

            if(systemId.IsNotWhiteSpace())
                query.AddWhere(fm => fm.SystemId == systemId);

            if(subId.IsNotWhiteSpace())
                query.AddWhere(fm => fm.SubId == subId);

            if(keyValues != null)
                foreach(var pair in keyValues)
                {
                    var pair1 = pair;
                    query.AddWhere(() => pair1.Key == pair1.Value);
                }

            if(state.HasValue)
                query.AddWhere(fm => fm.State == state.Value);

            return query;
        }

        /// <summary>
        /// FileMapping 정보 조회를 위한 Critieria를 빌드합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="systemId">시스템 ID</param>
        /// <param name="subId">서브 ID</param>
        /// <param name="keyValues">키-값 설정</param>
        /// <param name="state">상태</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfFileMapping(string productCode, string systemId, string subId, IDictionary<string, string> keyValues, int? state)
        {
            return BuildQueryOverOfFileMapping(productCode, systemId, subId, keyValues, state).DetachedCriteria;
        }

        /// <summary>
        /// 새로운 FileMapping 정보를 생성합니다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="systemId">시스템 ID</param>
        /// <param name="subId">시스템 서브 ID</param>
        /// <returns>생성된 FileMapping 인스턴스</returns>
        public FileMapping CreateFileMapping(string productCode, string systemId, string subId)
        {
            productCode.ShouldNotBeWhiteSpace("productCode");

            if(IsDebugEnabled)
                log.Debug(@"새로운 FileMapping 정보를 생성합니다. productCode={0}, systemId={1}, subId={2}", productCode, systemId, subId);

            var fileMapping = new FileMapping(productCode, systemId, subId);

            return Repository<FileMapping>.SaveOrUpdate(fileMapping);
        }

        /// <summary>
        /// 지정된 SystemId와 SubId 값을 가지는 모든 FileMapping 정보를 Load한다.
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="systemId">시스템 ID</param>
        /// <param name="subId">시스템 서브 ID</param>
        /// <param name="keyValues">Key-Value 검색조건 (예: Key1="user"), null이면 검색조건에서 제외된다.</param>
        /// <param name="state">Mapping 상태 정보, null이면 검색조건에서 제외된다.</param>
        /// <returns>검색된 FileMapping 컬렉션</returns>
        public IList<FileMapping> FindAllFileMapping(string productCode, string systemId, string subId, IDictionary<string, string> keyValues, int? state)
        {
            if(IsDebugEnabled)
                log.Debug(@"FileMapping 정보를 조회합니다... " +
                          @"productCode={0}, systemId={1}, subId={2}, keyValues={3}, state={4}",
                          productCode, systemId, subId, keyValues.DictionaryToString(), state);

            return Repository<FileMapping>.FindAll(BuildQueryOverOfFileMapping(productCode, systemId, subId, keyValues, state));
        }

        /// <summary>
        /// 검색조건에 해당하는 모든 FileMapping 정보를 삭제합니다. (만약 DeleteDate 속성으로 삭제여부를 사용하고 싶다면, Update를 사용해라)
        /// </summary>
        /// <param name="productCode">제품 코드</param>
        /// <param name="systemId">시스템 ID</param>
        /// <param name="subId">시스템 서브 ID</param>
        /// <param name="keyValues">Key-Value 검색조건 (예: Key1="user"), null이면 검색조건에서 제외된다.</param>
        /// <param name="state">Mapping 상태 정보, null이면 검색조건에서 제외된다.</param>
        public void DeleteAllFileMapping(string productCode, string systemId, string subId, IDictionary<string, string> keyValues, int? state)
        {
            if(IsDebugEnabled)
                log.Debug(@"FileMapping 정보를 삭제합니다... " +
                          @"productCode={0}, systemId={1}, subId={2}, keyValues={3}, state={4}",
                          productCode, systemId, subId, keyValues.DictionaryToString(), state);

            var mappings = FindAllFileMapping(productCode, systemId, subId, keyValues, state);

            foreach(var mapping in mappings)
            {
                DeleteAllFileByFileMapping(mapping);
                Repository<FileMapping>.Delete(mapping);
            }
        }

        /// <summary>
        /// <see cref="File"/> 정보를 조회하기 위한 QueryOver를 빌드합니다.
        /// </summary>
        /// <param name="fileMapping">파일 매핑 정보</param>
        /// <param name="resourceId">리소스 Id</param>
        /// <param name="resourceKind">리소스 종류</param>
        /// <param name="ownerCode">소유자 (부서|사원|그룹) 의 코드</param>
        /// <param name="ownerKind">소유자의 종류 (부서|사원|그룹 등)</param>
        /// <param name="category">분류</param>
        /// <param name="fileName">파일명</param>
        /// <param name="fileType">파일 종류</param>
        /// <param name="fileGroup">파일 그룹</param>
        /// <param name="fileFloor">파일 Floor</param>
        /// <returns></returns>
        public QueryOver<File, File> BuildQueryOverOfFile(FileMapping fileMapping, string resourceId, string resourceKind,
                                                          string ownerCode, ActorKinds? ownerKind,
                                                          string category, string fileName, string fileType,
                                                          int? fileGroup, int? fileFloor)
        {
            if(IsDebugEnabled)
                log.Debug(@"파일 정보를 조회하기 위한 QueryOver를 빌드합니다... " +
                          @"fileMapping={0}, resourceId={1}, resourceKind={2}, ownerCode={3}, ownerKind={4}, " +
                          @"category={5}, fileName={6}, fileType={7}, fileGroup={8}, fileFloor={9}",
                          fileMapping, resourceId, resourceKind, ownerCode, ownerKind,
                          category, fileName, fileType, fileGroup, fileFloor);

            var query = QueryOver.Of<File>();

            query.AddEqOrNull(f => f.FileMapping, fileMapping);

            if(resourceId.IsNotWhiteSpace())
                query.AddWhere(f => f.ResourceId == resourceId);

            if(resourceKind.IsNotWhiteSpace())
                query.AddWhere(f => f.ResourceKind == resourceKind);

            if(ownerCode.IsNotWhiteSpace())
                query.AddWhere(f => f.OwnerCode == ownerCode);

            if(ownerKind.HasValue)
                query.AddWhere(f => f.OwnerKind == ownerKind.Value);

            if(category.IsNotWhiteSpace())
                query.AddWhere(f => f.Category == category);

            if(fileName.IsNotWhiteSpace())
                query.AddWhere(f => f.FileName == fileName);

            if(fileType.IsNotWhiteSpace())
                query.AddWhere(f => f.FileType == fileType);

            if(fileGroup.HasValue)
                query.AddWhere(f => f.FileGroup == fileGroup);
            if(fileFloor.HasValue)
                query.AddWhere(f => f.FileFloor == fileFloor);

            return query;
        }

        /// <summary>
        /// <see cref="File"/> 정보를 조회하기 위한 Criteria를 빌드합니다.
        /// </summary>
        /// <param name="fileMapping">파일 매핑 정보</param>
        /// <param name="resourceId">리소스 Id</param>
        /// <param name="resourceKind">리소스 종류</param>
        /// <param name="ownerCode">소유자 (부서|사원|그룹) 의 코드</param>
        /// <param name="ownerKind">소유자의 종류 (부서|사원|그룹 등)</param>
        /// <param name="category">분류</param>
        /// <param name="fileName">파일명</param>
        /// <param name="fileType">파일 종류</param>
        /// <param name="fileGroup">파일 그룹</param>
        /// <param name="fileFloor">파일 Floor</param>
        /// <returns></returns>
        public DetachedCriteria BuildCriteriaOfFile(FileMapping fileMapping, string resourceId, string resourceKind,
                                                    string ownerCode, ActorKinds? ownerKind,
                                                    string category, string fileName, string fileType,
                                                    int? fileGroup, int? fileFloor)
        {
            return
                BuildQueryOverOfFile(fileMapping, resourceId, resourceKind,
                                     ownerCode, ownerKind, category,
                                     fileName, fileType, fileGroup, fileFloor)
                    .DetachedCriteria;
        }

        /// <summary>
        /// 새로운 파일 정보를 생성합니다.
        /// </summary>
        /// <param name="fileMapping">파일 매핑 정보</param>
        /// <param name="category">분류</param>
        /// <param name="fileName">파일명</param>
        /// <returns></returns>
        public File CreateFile(string category, string fileName, FileMapping fileMapping)
        {
            fileMapping.ShouldNotBeNull("fileMapping");

            if(IsDebugEnabled)
                log.Debug(@"새로운 File 정보를 생성합니다. category={0}, fileName={1}, fileMapping={2}", category, fileName, fileMapping);

            var file = new File(category, fileName, fileMapping);
            return Repository<File>.SaveOrUpdate(file);
        }

        /// <summary>
        /// 지정한 <see cref="FileMapping"/> 정보와 연관된 File 정보를 조회합니다.
        /// </summary>
        /// <param name="fileMapping">파일 매핑 정보</param>
        /// <returns></returns>
        public IList<File> FindAllFileByFileMapping(FileMapping fileMapping)
        {
            fileMapping.ShouldNotBeNull("fileMapping");

            if(IsDebugEnabled)
                log.Debug(@"지정한 FileMapping 정보를 가진 File 정보를 조회합니다... fileMapping=" + fileMapping);

            return Repository<File>.FindAll(QueryOver.Of<File>().AddEqOrNull(f => f.FileMapping, fileMapping));
        }

        /// <summary>
        /// 소유자로 파일을 찾습니다.
        /// </summary>
        /// <param name="ownerCode">소유자 (부서|사원|그룹) 의 코드</param>
        /// <param name="ownerKind">소유자의 종류 (부서|사원|그룹 등)</param>
        /// <returns></returns>
        public IList<File> FindAllFileByOwner(string ownerCode, ActorKinds? ownerKind)
        {
            if(IsDebugEnabled)
                log.Debug(@"Owner에 의한 File 정보를 조회합니다. ownerCode={0}, ownerKind={1}", ownerCode, ownerKind);

            var query = BuildQueryOverOfFile(null, null, null, ownerCode, ownerKind, null, null, null, null, null);

            return Repository<File>.FindAll(query);
        }

        /// <summary>
        /// 지정한 리소스 정보를 가진 File을 조회합니다.
        /// </summary>
        /// <param name="resourceId">리소스 ID</param>
        /// <param name="resourceKind">리소스 종류</param>
        /// <returns></returns>
        public IList<File> FindAllFileByResource(string resourceId, string resourceKind)
        {
            resourceId.ShouldNotBeWhiteSpace("resourceId");

            if(IsDebugEnabled)
                log.Debug(@"리소스 정보를 가진 File을 조회합니다... resourceId={0}, resourceKind={1}", resourceId, resourceKind);

            var query = BuildQueryOverOfFile(null, resourceId, resourceKind, null, null, null, null, null, null, null);

            return Repository<File>.FindAll(query);
        }

        /// <summary>
        /// 지정한 소유자의 모든 File을 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="ownerCode">소유자 (부서|사원|그룹) 의 코드</param>
        /// <param name="ownerKind">소유자의 종류 (부서|사원|그룹 등)</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<File> GetPageOfFileByOwner(string ownerCode, ActorKinds? ownerKind, int pageIndex, int pageSize, params INHOrder<File>[] orders)
        {
            ownerCode.ShouldNotBeWhiteSpace("ownerCode");

            if(IsDebugEnabled)
                log.Debug(@"지정한 소유자의 모든 File을 Paging 처리해서 로드합니다... ownerCode={0}, ownerKind={1}, pageIndex={2}, pageSize={3}, orders={4}",
                          ownerCode, ownerKind, pageIndex, pageSize, orders);

            var query = BuildQueryOverOfFile(null, null, null, ownerCode, ownerKind, null, null, null, null, null);

            return Repository<File>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// 리소스 정보를 가진 File을 Paging 처리해서 로드합니다.
        /// </summary>
        /// <param name="resourceId">리소스 ID</param>
        /// <param name="resourceKind">리소스 종류</param>
        /// <param name="pageIndex">페이지 인덱스 (0부터 시작)</param>
        /// <param name="pageSize">페이지 크기 (0보다 커야 한다. 보통 10)</param>
        /// <param name="orders">정렬 순서</param>
        /// <returns></returns>
        public IPagingList<File> GetPageOfFileByResource(string resourceId, string resourceKind, int pageIndex, int pageSize, params INHOrder<File>[] orders)
        {
            resourceId.ShouldNotBeWhiteSpace("resourceId");

            if(IsDebugEnabled)
                log.Debug(@"리소스 정보를 가진 File을 Paging 처리해서 로드합니다... resourceId={0}, resourceKind={1}, pageIndex={2}, pageSize={3}, orders={4}",
                          resourceId, resourceKind, pageIndex, pageSize, orders);

            var query = BuildQueryOverOfFile(null, resourceId, resourceKind, null, null, null, null, null, null, null);

            return Repository<File>.GetPage(pageIndex, pageSize, query, orders);
        }

        /// <summary>
        /// 지정된 파일매핑 정보를 가진 모든 File 정보를 삭제합니다.
        /// </summary>
        /// <param name="fileMapping"></param>
        public void DeleteAllFileByFileMapping(FileMapping fileMapping)
        {
            if(fileMapping == null)
                return;

            if(IsDebugEnabled)
                log.Debug(@"FileMapping 정보와 관련된 File들을 삭제합니다... " + fileMapping);

            Repository<File>.DeleteAll(QueryOver.Of<File>().AddWhere(f => f.FileMapping == fileMapping));
        }
    }
}