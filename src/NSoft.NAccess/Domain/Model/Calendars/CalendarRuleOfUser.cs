using System;
using NSoft.NFramework;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.TimePeriods;

namespace NSoft.NAccess.Domain.Model
{
    /// <summary>
    /// Calendar Rule Data of User
    /// </summary>
    [Serializable]
    public class CalendarRuleOfUser : DataEntityBase<long>, IUpdateTimestampedEntity
    {
        protected CalendarRuleOfUser()
        {
            DayOrException = 0;
            IsWorking = 0;
            IsActive = true;
        }

        public CalendarRuleOfUser(User user, CalendarRule calendarRule)
            : this()
        {
            user.ShouldNotBeNull("user");
            calendarRule.ShouldNotBeNull("calendarRule");

            CompanyCode = user.Company.Code;
            UserCode = user.Code;
            CalendarRule = calendarRule;
        }

        /// <summary>
        /// 사용자 소속 회사
        /// </summary>
        public virtual string CompanyCode { get; protected set; }

        /// <summary>
        /// 사용자 코드
        /// </summary>
        public virtual string UserCode { get; protected set; }

        /// <summary>
        /// 원본 Calendar Rule 
        /// </summary>
        public virtual CalendarRule CalendarRule { get; protected set; }

        public virtual int? DayOrException { get; set; }
        public virtual int? ExceptionType { get; set; }
        public virtual string ExceptionPattern { get; set; }
        public virtual string ExceptionClassName { get; set; }

        public virtual int? IsWorking { get; set; }

        private ITimePeriod _rulePeriod;

        /// <summary>
        /// 규칙이 적용되는 시간 범위
        /// </summary>
        public virtual ITimePeriod RulePeriod
        {
            get { return _rulePeriod ?? (_rulePeriod = new TimeRange()); }
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

        /// <summary>
        /// 활성화 여부
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 설명
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

            return HashTool.Compute(CompanyCode, UserCode, CalendarRule);
        }
    }
}