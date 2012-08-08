using System.Linq;
using NSoft.NFramework.Data.NHibernateEx;

namespace NSoft.NAccess.Domain.Model.Organizations
{
    /// <summary>
    /// 조직관련 Sample Data 생성
    /// </summary>
    public class OrganizationSampleModelBuilder : SampleModelBuilderBase
    {
        #region << logger >>

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly bool IsDebugEnabled = log.IsDebugEnabled;

        #endregion

        public override void CreateSampleModels()
        {
            CreateCompany();
            UnitOfWork.Current.TransactionalFlush();

            CreateCode();
            UnitOfWork.Current.TransactionalFlush();

            CreateEmployeeCodes();
            UnitOfWork.Current.TransactionalFlush();

            CreateDepartment();
            UnitOfWork.Current.TransactionalFlush();

            CreateUser();
            UnitOfWork.Current.TransactionalFlush();

            CreateDepartmentMember();
            UnitOfWork.Current.TransactionalFlush();

            CreateGroup();
            UnitOfWork.Current.TransactionalFlush();

            CreateGroupActor();
            UnitOfWork.Current.TransactionalFlush();
        }

        protected virtual void CreateCompany()
        {
            var company = NAccessContext.Domains.OrganizationRepository.GetOrCreateComapny(SampleData.CompanyCode);
            company.Name = company.Id + " Name";
            company.IsActive = true;
            company.Description = "테스트용 기본 Company입니다.";
            company.ExAttr = "확장 속성입니다.";

            company.AddLocale(SampleData.English, new CompanyLocale {Name = @"RealWeb", Description = "Default Company for Testing", ExAttr = "External Attributes"});
            company.AddLocale(SampleData.Korean, new CompanyLocale {Name = @"리얼웹", Description = "테스트용 기본 Company입니다.", ExAttr = "확장 속성"});
            company.AddMetadata("기본", true);

            Repository<Company>.SaveOrUpdate(company);


            foreach(var companyId in SampleData.CompanyCodes)
            {
                company = NAccessContext.Domains.OrganizationRepository.GetOrCreateComapny(companyId);
                company.Name = company.Code + " Name";
                company.IsActive = false;
                company.ExAttr = "확장 속성입니다.";

                company.AddMetadata("기본", false);

                Repository<Company>.SaveOrUpdate(company);
            }
        }

        protected virtual void CreateCode()
        {
            foreach(var company in NAccessContext.Linq.Companies.ToList())
                foreach(var groupId in SampleData.CodeGroupIds)
                    foreach(var codeId in SampleData.CodeIds)
                    {
                        var code = NAccessContext.Domains.OrganizationRepository.GetOrCreateCode(company, groupId, codeId);
                        code.Group.Name = "그룹 " + groupId;
                        code.ItemName = "코드 " + codeId;

                        UnitOfWork.CurrentSession.SaveOrUpdate(code);
                    }
        }

        protected virtual void CreateEmployeeCodes()
        {
            foreach(var company in NAccessContext.Linq.Companies.ToList())
            {
                int viewOrder = 0;
                foreach(var code in SampleData.GetCodes("EMP_POSITION_", SampleData.MinSampleCount))
                {
                    var position = NAccessContext.Domains.OrganizationRepository.CreateEmployeePosition(company, code, "직위 " + code);
                    position.ViewOrder = viewOrder++;

                    Repository<EmployeePosition>.SaveOrUpdate(position);
                }
                UnitOfWork.Current.Flush();

                viewOrder = 0;
                foreach(var code in SampleData.GetCodes("EMP_GRADE_", SampleData.MinSampleCount))
                {
                    var grade = NAccessContext.Domains.OrganizationRepository.CreateEmployeeGrade(company, code, "직급 " + code);
                    grade.ViewOrder = viewOrder++;

                    Repository<EmployeeGrade>.SaveOrUpdate(grade);
                }
                UnitOfWork.Current.Flush();

                viewOrder = 0;
                foreach(var code in SampleData.GetCodes("EMP_TITLE_", SampleData.MinSampleCount))
                {
                    var title = NAccessContext.Domains.OrganizationRepository.CreateEmployeeTitle(company, code, "직책 " + code);
                    title.ViewOrder = viewOrder++;

                    Repository<EmployeeTitle>.SaveOrUpdate(title);
                }
                UnitOfWork.Current.Flush();
            }
        }

        protected virtual void CreateDepartment()
        {
            foreach(var company in NAccessContext.Linq.Companies.ToList())
            {
                Department parent = null;
                foreach(var departmentId in SampleData.DepartmentCodes)
                {
                    var department = (parent == null)
                                         ? NAccessContext.Domains.OrganizationRepository.GetOrCreateDepartment(company, departmentId)
                                         : NAccessContext.Domains.OrganizationRepository.CreateDepartmentOf(parent, departmentId);

                    department.Name = "부서 " + departmentId;
                    department.AddLocale(SampleData.English, new DepartmentLocale() {Name = "Dept " + departmentId});
                    department.AddMetadata("정", true);

                    Repository<Department>.SaveOrUpdate(department);


                    parent = department;
                }
            }
        }

        protected virtual void CreateUser()
        {
            var positions = NAccessContext.Linq.EmployeePositions.ToList();
            var grades = NAccessContext.Linq.EmployeeGrades.ToList();

            foreach(var company in NAccessContext.Linq.Companies.ToList())
                foreach(var userCode in SampleData.UserCodes)
                {
                    var user = NAccessContext.Domains.OrganizationRepository.GetOrCreateUser(company, userCode);

                    user.Name = "사용자 " + userCode;
                    user.EmpNo = "사번 " + userCode;

                    user.Password = userCode;

                    user.Position = positions.ElementAt(RandomGenerator.Next(0, positions.Count));
                    user.Grade = grades.ElementAt(RandomGenerator.Next(0, grades.Count));

                    user.AddMetadata("고향", "몰라요^^");
                    user.AddMetadata("결혼", RandomGenerator.Next(0, 2));

                    UnitOfWork.CurrentSession.SaveOrUpdate(user);
                }
        }

        protected virtual void CreateDepartmentMember()
        {
            var titles = NAccessContext.Linq.EmployeeTitles.ToList();

            foreach(var company in NAccessContext.Linq.Companies.ToList())
            {
                var departments = NAccessContext.Domains.OrganizationRepository.FindAllDepartmentByCompany(company);
                var users = NAccessContext.Domains.OrganizationRepository.FindAllUserByCompany(company);

                foreach(var department in departments)
                    foreach(var user in users)
                    {
                        var title = titles.ElementAt(RandomGenerator.Next(0, titles.Count));
                        var departmentMember = NAccessContext.Domains.OrganizationRepository.CreateDepartmentMember(department, user, title);
                    }
            }

            UnitOfWork.Current.TransactionalFlush();
        }

        protected virtual void CreateGroup()
        {
            foreach(var company in NAccessContext.Linq.Companies.ToList())
            {
                foreach(var code in SampleData.GetCodes("SYSTEM_GROUP_", SampleData.AvgSampleCount))
                {
                    var group = NAccessContext.Domains.OrganizationRepository.GetOrCreateGroup(company, code);
                    group.Kind = GroupKinds.System;
                    UnitOfWork.CurrentSession.SaveOrUpdate(group);
                }
            }
            UnitOfWork.Current.TransactionalFlush();

            foreach(var company in NAccessContext.Linq.Companies.ToList())
            {
                foreach(var code in SampleData.GetCodes("CUSTOM_GROUP_", SampleData.AvgSampleCount))
                {
                    var group = NAccessContext.Domains.OrganizationRepository.GetOrCreateGroup(company, code);
                    group.Kind = GroupKinds.Custom;
                    UnitOfWork.CurrentSession.SaveOrUpdate(group);
                }
            }
        }

        protected virtual void CreateGroupActor()
        {
            foreach(var group in NAccessContext.Linq.Groups)
            {
                var company = group.Company;

                NAccessContext.Domains.OrganizationRepository.CreateGroupActor(group, company.Code, ActorKinds.Company);

                foreach(var department in NAccessContext.Domains.OrganizationRepository.FindAllDepartmentByCompany(company))
                    NAccessContext.Domains.OrganizationRepository.CreateGroupActor(group, department.Code, ActorKinds.Department);

                foreach(var user in NAccessContext.Domains.OrganizationRepository.FindAllUserByCompany(company))
                    NAccessContext.Domains.OrganizationRepository.CreateGroupActor(group, user.Code, ActorKinds.User);
            }
        }
    }
}