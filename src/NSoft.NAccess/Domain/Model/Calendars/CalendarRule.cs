using System;
using NSoft.NFramework;
using NSoft.NFramework.Data;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.TimePeriods;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// Calendar Rule Data
    /// </summary>
    [Serializable]
    public class CalendarRule : LocaledMetadataEntityBase<long, CalendarRuleLocale>, IUpdateTimestampedEntity
    {
        protected CalendarRule() {}

        public CalendarRule(Calendar calendar, string name)
        {
            calendar.ShouldNotBeNull("calendar");
            // name.ShouldNotBeWhiteSpace("name");

            Calendar = calendar;
            Name = name ?? "Undefiend";

            DayOrException = 0;
            IsWorking = 0;
        }

        /// <summary>
        /// Calendar
        /// </summary>
        public virtual Calendar Calendar { get; set; }

        /// <summary>
        /// 규칙 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 특정 날짜인지, 아니면 휴일 지정 예외 규칙인지 나타낸다.
        /// </summary>
        public virtual int? DayOrException { get; set; }

        /// <summary>
        /// 예외 규칙의 종류
        /// </summary>
        public virtual int? ExceptionType { get; set; }

        /// <summary>
        /// 예외 패턴 (아직 준비중이다)
        /// </summary>
        public virtual string ExceptionPattern { get; set; }

        /// <summary>
        /// 예외 패턴을 적용할 클래스 타입명
        /// </summary>
        public virtual string ExceptionClassName { get; set; }

        /// <summary>
        /// 일하는 날인가?
        /// </summary>
        public virtual int? IsWorking { get; set; }

        private ITimePeriod _rulePeriod;

        /// <summary>
        /// 규칙이 적용되는 시간 범위
        /// </summary>
        public virtual ITimePeriod RulePeriod
        {
            get { return _rulePeriod ?? (_rulePeriod = new TimeRange());}
            protected set { _rulePeriod = value; }
        }

        private ITimePeriod _rulePeriod1;
        private ITimePeriod _rulePeriod2;
        private ITimePeriod _rulePeriod3;
        private ITimePeriod _rulePeriod4;
        private ITimePeriod _rulePeriod5;

        public virtual ITimePeriod RulePeriod1
        {
            get { return _rulePeriod1 ?? (_rulePeriod1 = new TimeRange()); }
            protected set { _rulePeriod1 = value; }
        }

        public virtual ITimePeriod RulePeriod2
        {
            get { return _rulePeriod2 ?? (_rulePeriod2 = new TimeRange()); }
            protected set { _rulePeriod2 = value; }
        }

        public virtual ITimePeriod RulePeriod3
        {
            get { return _rulePeriod3 ?? (_rulePeriod3 = new TimeRange()); }
            protected set { _rulePeriod3 = value; }
        }

        public virtual ITimePeriod RulePeriod4
        {
            get { return _rulePeriod4 ?? (_rulePeriod4 = new TimeRange()); }
            protected set { _rulePeriod4 = value; }
        }

        public virtual ITimePeriod RulePeriod5
        {
            get { return _rulePeriod5 ?? (_rulePeriod5 = new TimeRange()); }
            protected set { _rulePeriod5 = value; }
        }

        private ITimePeriod[] _rulePeriods;

        public virtual ITimePeriod[] RulePeriods
        {
            get
            {
                return _rulePeriods ?? (_rulePeriods = new[]
                                                       {
                                                           RulePeriod1,
                                                           RulePeriod2,
                                                           RulePeriod3,
                                                           RulePeriod4,
                                                           RulePeriod5
                                                       });
            }
        }

        public virtual int? ViewOrder { get; set; }

        /// <summary>
        /// 규칙 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 속성
        /// </summary>
        public virtual string ExAttr { get; set; }

        /// <summary>
        /// 최종갱신일
        /// </summary>
        public virtual DateTime? UpdateTimestamp { get; set; }

        public override int GetHashCode()
        {
            if(IsSaved)
                return base.GetHashCode();

            return HashTool.Compute(Calendar, DayOrException, _rulePeriod);
        }

        public override string ToString()
        {
            return string.Format(@"CalendarRule# Id={0}, Name={1}, Calendar={2}, RulePeriod={3}", Id, Name, Calendar, _rulePeriod);
        }
    }

    /// <summary>
    /// <see cref="CalendarRule"/>의 지역화 정보
    /// </summary>
    [Serializable]
    public class CalendarRuleLocale : DataObjectBase, ILocaleValue
    {
        /// <summary>
        /// 규칙 명
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 규칙 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 확장 속성
        /// </summary>
        public virtual string ExAttr { get; set; }
    }
}