using System;
using Iesi.Collections.Generic;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// Calenar 정보
    /// </summary>
    [Serializable]
    public class Calendar : LocaledMetadataEntityBase<long, CalendarLocale>, ITreeNodeEntity<Calendar>, IUpdateTimestampedEntity
    {
        public const string StandardCalendarCode = @"Standard";

        protected Calendar() {}
        public Calendar(string code) : this(null, code, null) {}
        public Calendar(string code, string projectId) : this(null, code, projectId) {}
        public Calendar(Company company, string code) : this(company, code, null) {}

        public Calendar(Company company, string code, string projectId)
        {
            CompanyCode = (company != null) ? company.Code : null;
            Code = code;
            Name = code;
            ProjectId = projectId;
        }

        /// <summary>
        /// 소속 회사
        /// </summary>
        public virtual string CompanyCode { get; set; }

        /// <summary>
        /// Calendar 코드
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// Calendar 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 프로젝트 Calendar인 경우 관련 프로젝트 Id
        /// </summary>
        public virtual string ProjectId { get; set; }

        /// <summary>
        /// 리소스 Calendar인 경우 관련 Resource Id
        /// </summary>
        public virtual string ResourceId { get; set; }

        /// <summary>
        /// TimeZone 정보
        /// </summary>
        public virtual string TimeZone { get; set; }

        private ISet<Calendar> _children;
        private ITreeNodePosition _nodePosition;

        /// <summary>
        /// 부모 노드
        /// </summary>
        public virtual Calendar Parent { get; set; }

        /// <summary>
        /// 자식 노드들
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Calendar> Children
        {
            get { return _children ?? (_children = new Iesi.Collections.Generic.HashedSet<Calendar>()); }
            protected set { _children = value; }
        }

        /// <summary>
        /// TreeNode의 위치
        /// </summary>
        public virtual ITreeNodePosition NodePosition
        {
            get { return _nodePosition ?? (_nodePosition = new TreeNodePosition(0, 0)); }
            set { _nodePosition = value; }
        }

        private ISet<CalendarRule> _rules;

        /// <summary>
        /// Calendar Working Time 생성 규칙(Rule)
        /// </summary>
        public virtual ISet<CalendarRule> Rules
        {
            get { return _rules ?? (_rules = new HashedSet<CalendarRule>()); }
            protected set { _rules = value; }
        }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 최종갱신일
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        /// <summary>
        /// Base Calendar 인가? (Calendar Owner Kind가 Standard 여야 한다)
        /// </summary>
        public virtual bool IsBaseCalendar
        {
            get { return (Parent == null); }
        }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(CompanyCode, Code, ProjectId, ResourceId);
        }

        public override string ToString()
        {
            return string.Format("Calendar# Id={0}, CompanyCode={1}, Code={2}, Name={3}, ProjectId={4}, ResourceId={5}, ",
                                 Id, CompanyCode, Code, Name, ProjectId, ResourceId);
        }
    }

    /// <summary>
    /// Calendar 지역화 정보
    /// </summary>
    [Serializable]
    public class CalendarLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// Calendar 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장속성
        /// </summary>
        public virtual string ExAttr { get; set; }
    }
}