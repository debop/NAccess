using System;
using System.Collections.Generic;
using System.Globalization;
using FluentNHibernate.Testing;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;
using NSoft.NFramework.TimePeriods;
using NUnit.Framework;
using SharpTestsEx;

namespace NSoft.NAccess.Domain.Model.Calendars
{
    [TestFixture]
    public class CalendarFluentDomainTestFixture : FluentDomainTestFixtureBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        #region << 테스트 준비 작업 >>

        protected override void OnTestFixtureSetUp()
        {
            base.OnTestFixtureSetUp();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            new OrganizationSampleFluentModelBuilder().CreateSampleModels();
            new ProductSampleFluentModelBuilder().CreateSampleModels();
            new CalendarSampleFluentModelBuilder().CreateSampleModels();
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            UnitOfWork.CurrentSession.Clear();
        }

        #endregion

        [Test]
        public void CalendarTestByUnitOfWork()
        {
            var calendar = new Calendar("CAL_0001");
            calendar.AddMetadata("a", new MetadataValue("A"));
            calendar.AddMetadata("b", new MetadataValue("B"));
            calendar.AddLocale(new CultureInfo("en"), new CalendarLocale {Name = "System Calendar"});
            calendar.AddLocale(new CultureInfo("ko"), new CalendarLocale {Name = "시스템달력"});

            Repository<Calendar>.SaveOrUpdate(calendar);
            UnitOfWork.Current.TransactionalFlush();
            UnitOfWork.Current.Clear();

            var loaded = Repository<Calendar>.Get(calendar.Id);
            Assert.AreEqual(calendar, loaded);

            loaded.LocaleMap.Count.Should().Be(2);
        }

        [Test]
        public void CalendarRuleTestByFluent()
        {
            var calendar = Repository<Calendar>.FindFirst();
            calendar.Should().Not.Be.Null();

            new PersistenceSpecification<CalendarRule>(UnitOfWork.CurrentSession)
                .CheckProperty(x => x.Calendar, calendar)
                .CheckProperty(x => x.Name, "규칙")
                .CheckProperty(x => x.IsWorking, 1)
                .CheckComponentList(x => x.MetadataMap,
                                    new Dictionary<string, IMetadataValue>
                                    {
                                        {"A", new MetadataValue("a")},
                                        {"B", new MetadataValue("b")}
                                    },
                                    (c, m) => c.AddMetadata(m.Key, m.Value))
                .CheckComponentList(x => x.LocaleMap,
                                    new Dictionary<CultureInfo, CalendarRuleLocale>
                                    {
                                        {new CultureInfo("en"), new CalendarRuleLocale {Name = "Rule"}},
                                        {new CultureInfo("ko"), new CalendarRuleLocale {Name = "규칙"}}
                                    },
                                    (c, loc) => c.AddLocale(loc.Key, loc.Value))
                .VerifyTheMappings();
        }

        [Test]
        public void CalendarRuleOfUserTestByHybrid()
        {
            var company = Repository<Company>.FindFirst();
            company.Should().Not.Be.Null();

            var calendar = Repository<Calendar>.FindFirst();
            calendar.Should().Not.Be.Null();

            var user = Repository<User>.FindFirst();
            user.Should().Not.Be.Null();

            var calendarRule = Repository<CalendarRule>.FindFirst();
            user.Should().Not.Be.Null();

            var calendarRuleOfUser = new CalendarRuleOfUser(user, calendarRule);

            new PersistenceSpecification<CalendarRuleOfUser>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(calendarRuleOfUser);
        }

        [Test]
        public void WorkTimeByDayTestByHybrid()
        {
            var calendar = Repository<Calendar>.FindFirst();
            calendar.Should().Not.Be.Null();

            var workTimeByDay = new WorkTimeByDay(calendar, DateTime.Now) {IsWork = true};

            new PersistenceSpecification<WorkTimeByDay>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(workTimeByDay);
        }

        [Test]
        public void WorkTimeByHourTestByHybrid()
        {
            var calendar = Repository<Calendar>.FindFirst();
            calendar.Should().Not.Be.Null();

            var workTimeByHour = new WorkTimeByHour(calendar, DateTime.Now) {IsWork = true};

            new PersistenceSpecification<WorkTimeByHour>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(workTimeByHour);
        }

        [Test]
        public void WorkTimeByMinuteTestByHybrid()
        {
            var calendar = Repository<Calendar>.FindFirst();
            calendar.Should().Not.Be.Null();

            var workTimeByMinute = new WorkTimeByMinute(calendar, DateTime.Now) {IsWork = true};

            new PersistenceSpecification<WorkTimeByMinute>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(workTimeByMinute);
        }

        [Test]
        [Ignore("TimeRange객체 생성시 System.InvalidOperationException : Current object is Readonly 발생")]
        public void WorkTimeByRangeTestByHybrid()
        {
            var calendar = Repository<Calendar>.FindFirst();
            calendar.Should().Not.Be.Null();

            // NOTE : TimeRange객체 생성시 System.InvalidOperationException : Current object is Readonly 발생
            var workTimeByRange = new WorkTimeByRange(calendar,
                                                      TimeRange.Anytime,
                                                      60);

            new PersistenceSpecification<WorkTimeByRange>(UnitOfWork.CurrentSession)
                .VerifyTheMappings(workTimeByRange);
        }
    }
}