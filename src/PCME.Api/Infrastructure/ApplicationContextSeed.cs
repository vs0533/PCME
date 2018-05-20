using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PCME.Domain.AggregatesModel.AuditStatusAggregates;
using PCME.Domain.AggregatesModel.ExamOpenInfoAggregates;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Infrastructure;
using PCME.MOPDB;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure
{

    public class ApplicationContextSeed
    {
        private Policy CreatePolicy(string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        //logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }

        #region 枚举字典表初实例化
        private IEnumerable<WorkUnitNature> GetPredefinedWorkUnitNature()
        {
            return new List<WorkUnitNature>()
            {
                WorkUnitNature.JgUnit,
                WorkUnitNature.SyUnit,
                WorkUnitNature.Company,
                WorkUnitNature.Unknown
            };
        }
        private IEnumerable<WorkUnitAccountType> GetPredefinedWorkUnitAccountType()
        {
            return new List<WorkUnitAccountType>()
            {
                WorkUnitAccountType.Manager,
                WorkUnitAccountType.Approve,
                WorkUnitAccountType.CE,
                WorkUnitAccountType.CS
            };
        }
        private IEnumerable<StudentType> GetPredefinedStudentType()
        {
            return new List<StudentType>()
            {
                StudentType.Professional,
                StudentType.CivilServant
            };
        }
        private IEnumerable<Sex> GetPredefinedSex()
        {
            return new List<Sex>()
            {
                Sex.Man,
                Sex.Woman,
                Sex.Unknown
            };
        }
        private IEnumerable<StudentStatus> GetPredefinedStudentStatus()
        {
            return new List<StudentStatus>()
            {
                StudentStatus.Normal,
                StudentStatus.Resign,
                StudentStatus.Retire,
                StudentStatus.BeNotIn
            };
        }
        private IEnumerable<ExamSubjectStatus> GetPredefinedExamSubjectStatus()
        {
            return new List<ExamSubjectStatus>()
            {
                ExamSubjectStatus.Default,
                ExamSubjectStatus.Forbidden
            };
        }
        private IEnumerable<ExamType> GetPredefinedExamType()
        {
            return new List<ExamType>()
            {
                ExamType.NoScene,
                ExamType.Scene
            };
        }
        private IEnumerable<OpenType> GetPredefinedOpenType()
        {
            return new List<OpenType>()
            {
                OpenType.Professional,
                OpenType.CivilServant
            };
        }
        private IEnumerable<AuditStatus> GetPredefinedAuditStatus()
        {
            return new List<AuditStatus>()
            {
                AuditStatus.Pass,
                AuditStatus.Veto,
                AuditStatus.Wait
            };
        }
        private IEnumerable<PromoteType> GetPredefinedPromoteType()
        {
            return new List<PromoteType>()
            {
                PromoteType.Exam,
                PromoteType.Review
            };
        }
        #endregion


        public async Task SeedAsync(ApplicationDbContext context, MOPDBContext mopcontext, IHostingEnvironment env, IOptions<ApplicationSettings> settings)
        {
            var policy = CreatePolicy(nameof(ApplicationContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    context.Database.Migrate();
                    #region 枚举字典表初始化数据
                    if (!context.WorkUnitNature.Any())
                    {
                        context.WorkUnitNature.AddRange(GetPredefinedWorkUnitNature());
                        await context.SaveChangesAsync();
                    }
                    if (!context.WorkUnitAccountType.Any())
                    {
                        context.WorkUnitAccountType.AddRange(GetPredefinedWorkUnitAccountType());
                        await context.SaveChangesAsync();
                    }
                    if (!context.StudentTypes.Any())
                    {
                        context.StudentTypes.AddRange(GetPredefinedStudentType());
                        await context.SaveChangesAsync();
                    }
                    if (!context.Sex.Any())
                    {
                        context.Sex.AddRange(GetPredefinedSex());
                        await context.SaveChangesAsync();
                    }
                    if (!context.StudentStatus.Any())
                    {
                        context.StudentStatus.AddRange(GetPredefinedStudentStatus());
                        await context.SaveChangesAsync();
                    }
                    if (!context.ExamSubjectStatuses.Any())
                    {
                        context.ExamSubjectStatuses.AddRange(GetPredefinedExamSubjectStatus());
                        await context.SaveChangesAsync();
                    }
                    if (!context.ExamTypes.Any())
                    {
                        context.ExamTypes.AddRange(GetPredefinedExamType());
                        await context.SaveChangesAsync();
                    }
                    if (!context.OpenTypes.Any())
                    {
                        context.OpenTypes.AddRange(GetPredefinedOpenType());
                        await context.SaveChangesAsync();
                    }
                    if (!context.AuditStatus.Any())
                    {
                        context.AuditStatus.AddRange(GetPredefinedAuditStatus());
                        await context.SaveChangesAsync();
                    }
                    if (!context.PromoteTypes.Any())
                    {
                        context.PromoteTypes.AddRange(GetPredefinedPromoteType());
                        await context.SaveChangesAsync();
                    }
                    #endregion
                    #region 字典表初始化数据
                    if (!context.WorkUnits.Any())
                    {
                        List<WorkUnit> workunits = new List<WorkUnit>();
                        var workUnit = new WorkUnit("3703", "淄博市人力资源和社会保障局", 0, "卢瑞生", "", "", "", null, WorkUnitNature.JgUnit.Id);
                        workUnit.AddAccount("111111", WorkUnitAccountType.Manager.Id, "堂堂");
                        workunits.Add(workUnit);

                        context.WorkUnits.AddRange(workunits);
                    }
                    if (!context.Levels.Any())
                    {
                        IEnumerable<Level> levels = new List<Level>() {
                            new Level("未定"),
                            new Level("助级"),
                            new Level("员级"),
                            new Level("中级"),
                            new Level("副高"),
                            new Level("正高")
                        };
                        context.Levels.AddRange(levels);
                    }
                    if (!context.Seriess.Any())
                    {
                        IEnumerable<Series> series = new List<Series>()
                        {
                            new Series("工程系列"),
                            new Series("卫生系列"),
                            new Series("自然科学研究"),
                            new Series("社会科学研究"),
                            new Series("农业技术"),
                            new Series("经济系列"),
                            new Series("审计系列"),
                            new Series("会计系列"),
                            new Series("统计系列"),
                            new Series("教练员"),
                            new Series("新闻(编辑)"),
                            new Series("翻译系列"),
                            new Series("播音系列"),
                            new Series("出版系列"),
                            new Series("技校教师"),
                            new Series("实验技术"),
                            new Series("高校教师"),
                            new Series("中专教师"),
                            new Series("中学教师"),
                            new Series("小学教师"),
                            new Series("图书资料"),
                            new Series("群众文化"),
                            new Series("文物博物"),
                            new Series("美术系列"),
                            new Series("艺术系列"),
                            new Series("档案系列"),
                            new Series("工艺美术"),
                            new Series("医药中药"),
                            new Series("文学创作"),
                            new Series("党校教师"),
                            new Series("律师系列"),
                            new Series("公证员"),
                            new Series("其他"),
                        };
                        context.Seriess.AddRange(series);
                    }
                    if (!context.Specialtys.Any())
                    {
                        IEnumerable<Specialty> specialty = new List<Specialty>()
                        {
                            new Specialty("编导"),
                            new Specialty("编辑-出版"),
                            new Specialty("编辑-新闻"),
                            new Specialty("编剧"),
                            new Specialty("播音"),
                            new Specialty("出版"),
                            new Specialty("党校教师"),
                            new Specialty("档案专业"),
                            new Specialty("导演"),
                            new Specialty("地质勘探"),
                            new Specialty("电机电器"),
                            new Specialty("电子工程"),
                            new Specialty("翻译"),
                            new Specialty("纺织工程"),
                            new Specialty("高校教师"),
                            new Specialty("工艺美术"),
                            new Specialty("公证员"),
                            new Specialty("广电工程"),
                            new Specialty("护理"),
                            new Specialty("化工工程"),
                            new Specialty("环保工程"),
                            new Specialty("会计"),
                            new Specialty("机械工程"),
                            new Specialty("记者"),
                            new Specialty("技术编辑"),
                            new Specialty("技校"),
                            new Specialty("建材工程"),
                            new Specialty("建筑工程"),
                            new Specialty("交通工程"),
                            new Specialty("教练"),
                            new Specialty("经济"),
                            new Specialty("理论课教师"),
                            new Specialty("林业工程"),
                            new Specialty("律师"),
                            new Specialty("煤炭工程"),
                            new Specialty("美术专业"),
                            new Specialty("农业技术"),
                            new Specialty("其他"),
                            new Specialty("轻工工程"),
                            new Specialty("群众文化"),
                            new Specialty("燃气工程"),
                            new Specialty("社会科学"),
                            new Specialty("审计"),
                            new Specialty("实习课教师"),
                            new Specialty("实验技术"),
                            new Specialty("水利工程"),
                            new Specialty("统计"),
                            new Specialty("图书资料"),
                            new Specialty("文物博物"),
                            new Specialty("文学创作"),
                            new Specialty("舞台技术"),
                            new Specialty("舞台设计"),
                            new Specialty("小学教师"),
                            new Specialty("校对"),
                            new Specialty("演员"),
                            new Specialty("演奏员"),
                            new Specialty("药品技术"),
                            new Specialty("冶金工程"),
                            new Specialty("医技"),
                            new Specialty("医疗"),
                            new Specialty("医药"),
                            new Specialty("艺术系列"),
                            new Specialty("艺术指导"),
                            new Specialty("指挥"),
                            new Specialty("质量工程"),
                            new Specialty("中学教师"),
                            new Specialty("中专教师"),
                            new Specialty("自然科学"),
                            new Specialty("作词"),
                            new Specialty("作曲")
                        };
                        context.Specialtys.AddRange(specialty);
                    }

                    await context.SaveChangesAsync();
                    if (!context.DutyLevels.Any())
                    {
                        IReadOnlyCollection<CivilServantDutyLevel> civilServantDutyLevels = mopcontext.CivilServantDutyLevel.ToList();
                        List<DutyLevel> dutyLevels = new List<DutyLevel>();
                        foreach (var item in civilServantDutyLevels)
                        {
                            DutyLevel dutyLevel = new DutyLevel(item.DutyName);
                            dutyLevels.Add(dutyLevel);
                        }
                        context.DutyLevels.AddRange(dutyLevels);
                    }
                    if (!context.ProfessionalTitles.Any())
                    {
                        var p = mopcontext.DirectoryZwName.Include(s => s.ClassNameNavigation).ToList();
                        var levels = context.Levels.ToList();
                        var serieses = context.Seriess.ToList();
                        var specialtys = context.Specialtys.ToList();

                        List<ProfessionalTitle> ptitles = new List<ProfessionalTitle>();

                        foreach (var item in p)
                        {
                            string name = item.ZwName;
                            Specialty specialty = specialtys.FirstOrDefault(c => c.Name == item.Zy.Trim()) ?? specialtys.FirstOrDefault(c => c.Name == "其他");
                            Level level = levels.FirstOrDefault(c => c.Name == item.ZcJb.Trim()) ?? levels.FirstOrDefault(c => c.Name == "未定");
                            Series series = serieses.FirstOrDefault(c => c.Name == item.ClassNameNavigation?.ClassName.Trim()) ?? serieses.FirstOrDefault(c => c.Name == "其他");

                            ProfessionalTitle ptitle = new ProfessionalTitle(name, specialty, series, level);
                            ptitles.Add(ptitle);
                        }
                        context.ProfessionalTitles.AddRange(ptitles);
                        await context.SaveChangesAsync();
                    }
                    #endregion
                    #region 导入单位和账号
                    if ((context.WorkUnits.Count() == 1))
                    {
                        var u_all = mopcontext.Unit.Include(s => s.Account).ToList();
                        var u_1 = u_all.Where(c => c.UnitId.Length == 6).ToList();
                        var u_2 = u_all.Where(c => c.UnitId.Length == 9).ToList();
                        var u_3 = u_all.Where(c => c.UnitId.Length == 12).ToList();
                        var u_4 = u_all.Where(c => c.UnitId.Length == 15).ToList();


                        List<WorkUnit> workUnits = new List<WorkUnit>();
                        var rootUnit = await context.WorkUnits.FirstOrDefaultAsync(c => c.Level == 0);
                        foreach (var item in u_1)
                        {

                            WorkUnit workUnit = new WorkUnit(
                                item.UnitId,
                                item.UnitName,
                                1,
                                item.Linkman,
                                item.Mobile,
                                item.Email,
                                item.Address,
                                rootUnit.Id,
                                WorkUnitNature.Unknown.Id
                                );
                            foreach (var account in item.Account)
                            {
                                int accounttypeid = 0;
                                switch (account.TypeId)
                                {
                                    case 11:
                                        accounttypeid = 1;
                                        break;
                                    case 12:
                                        accounttypeid = 2;
                                        break;
                                    case 21:
                                        accounttypeid = 4;
                                        break;
                                    default:
                                        accounttypeid = 3;
                                        break;
                                }
                                workUnit.AddAccount(account.Password, accounttypeid, workUnit.LinkMan, account.AccountName);
                            }

                            workUnits.Add(workUnit);
                        }
                        context.WorkUnits.AddRange(workUnits);
                        await context.SaveChangesAsync();


                        List<WorkUnit> workUnits2 = new List<WorkUnit>();
                        List<WorkUnit> curAddedWorkUnit2 = context.WorkUnits.ToList();
                        foreach (var item in u_2)
                        {
                            var parentUnitId = u_1.FirstOrDefault(c => c.UnitId == item.UnitId.Substring(0, item.UnitId.Length - 3));
                            var parentUnit = curAddedWorkUnit2.FirstOrDefault(c => c.Code == parentUnitId.UnitId);
                            if (parentUnit == null)
                            {
                                break;
                            }
                            WorkUnit workUnit = new WorkUnit(
                                item.UnitId,
                                item.UnitName,
                                2,
                                item.Linkman,
                                item.Mobile,
                                item.Email,
                                item.Address,
                                parentUnit.Id,
                                WorkUnitNature.Unknown.Id
                                );
                            foreach (var account in item.Account)
                            {
                                int accounttypeid = 0;
                                switch (account.TypeId)
                                {
                                    case 11:
                                        accounttypeid = 1;
                                        break;
                                    case 12:
                                        accounttypeid = 2;
                                        break;
                                    case 21:
                                        accounttypeid = 4;
                                        break;
                                    default:
                                        accounttypeid = 3;
                                        break;
                                }
                                workUnit.AddAccount(account.Password, accounttypeid, workUnit.LinkMan, account.AccountName);
                            }

                            workUnits2.Add(workUnit);
                        }
                        context.WorkUnits.AddRange(workUnits2);
                        await context.SaveChangesAsync();

                        List<WorkUnit> workUnits3 = new List<WorkUnit>();
                        List<WorkUnit> curAddedWorkUnit3 = context.WorkUnits.ToList();
                        foreach (var item in u_3)
                        {
                            var parentUnitId = u_2.FirstOrDefault(c => c.UnitId == item.UnitId.Substring(0, item.UnitId.Length - 3));
                            var parentUnit = curAddedWorkUnit3.FirstOrDefault(c => c.Code == parentUnitId.UnitId);
                            if (parentUnit == null)
                            {
                                break;
                            }
                            WorkUnit workUnit = new WorkUnit(
                                item.UnitId,
                                item.UnitName,
                                3,
                                item.Linkman,
                                item.Mobile,
                                item.Email,
                                item.Address,
                                parentUnit.Id,
                                WorkUnitNature.Unknown.Id
                                );
                            foreach (var account in item.Account)
                            {
                                int accounttypeid = 0;
                                switch (account.TypeId)
                                {
                                    case 11:
                                        accounttypeid = 1;
                                        break;
                                    case 12:
                                        accounttypeid = 2;
                                        break;
                                    case 21:
                                        accounttypeid = 4;
                                        break;
                                    default:
                                        accounttypeid = 3;
                                        break;
                                }
                                workUnit.AddAccount(account.Password, accounttypeid, workUnit.LinkMan, account.AccountName);
                            }

                            workUnits3.Add(workUnit);
                        }
                        context.WorkUnits.AddRange(workUnits3);
                        await context.SaveChangesAsync();

                        List<WorkUnit> workUnits4 = new List<WorkUnit>();
                        List<WorkUnit> curAddedWorkUnit4 = context.WorkUnits.ToList();
                        foreach (var item in u_4)
                        {
                            var parentUnitId = u_3.FirstOrDefault(c => c.UnitId == item.UnitId.Substring(0, item.UnitId.Length - 3));
                            var parentUnit = curAddedWorkUnit4.FirstOrDefault(c => c.Code == parentUnitId.UnitId);
                            if (parentUnit == null)
                            {
                                break;
                            }
                            WorkUnit workUnit = new WorkUnit(
                                item.UnitId,
                                item.UnitName,
                                4,
                                item.Linkman,
                                item.Mobile,
                                item.Email,
                                item.Address,
                                parentUnit.Id,
                                WorkUnitNature.Unknown.Id
                                );
                            foreach (var account in item.Account)
                            {
                                int accounttypeid = 0;
                                switch (account.TypeId)
                                {
                                    case 11:
                                        accounttypeid = 1;
                                        break;
                                    case 12:
                                        accounttypeid = 2;
                                        break;
                                    case 21:
                                        accounttypeid = 4;
                                        break;
                                    default:
                                        accounttypeid = 3;
                                        break;
                                }
                                workUnit.AddAccount(account.Password, accounttypeid, workUnit.LinkMan, account.AccountName);
                            }

                            workUnits4.Add(workUnit);
                        }

                        context.WorkUnits.AddRange(workUnits4);
                        await context.SaveChangesAsync();
                    }
                    #endregion
                    #region 导入人员
                    if (!context.Students.Any())
                    {
                        IReadOnlyCollection<PersonPhoto> photo = mopcontext.PersonPhoto.ToList();
                        IReadOnlyCollection<UnitDept> office = mopcontext.UnitDept.ToList();
                        IReadOnlyCollection<Money> money = mopcontext.Money.ToList();
                        IReadOnlyCollection<Person> oldPerson = mopcontext.Person
                            .Include(s => s.PersonIdentity)//.Skip(0).Take(1753)
                            .ToList();
                        IReadOnlyCollection<WorkUnit> curWorkUnits = context.WorkUnits.ToList();
                        IReadOnlyCollection<ProfessionalTitle> curProfessionTitles = context.ProfessionalTitles.ToList();


                        List<Student> studentContent = new List<Student>();
                        //foreach (var item in oldPerson)
                        int ctr = 0;
                        int insertCtr = 0;
                        int insertCount = 0;
                        const int SAVECTR = 8;
                        Object thisLock = new Object();
                        Parallel.ForEach(oldPerson, item =>
                        {
                            //var curworkunit = curWorkUnits.FirstOrDefault(c => c.Code == item.WorkUnitId);
                            //if (curworkunit == null)
                            //{
                            //    return;
                            //}
                            //int pid = curworkunit.Id;
                            int pid = curWorkUnits.FirstOrDefault(c => c.Code == item.WorkUnitId).Id;

                            var m = money.FirstOrDefault(c => c.PersonId == item.PersonId);
                            decimal mV = m is null ? 0 : m.MoneyVirtual;
                            decimal mA = m is null ? 0 : m.MoneyActual;

                            var of = office.FirstOrDefault(c => c.DeptId == item.DeptId);

                            string officestr = of is null ? "待定" : of.DeptName;
                            var pho = photo.FirstOrDefault(c => c.PersonId == item.PersonId);


                            int sex = Sex.Man.Id;
                            switch (item.Sex)
                            {
                                case "男":
                                    sex = Sex.Man.Id;
                                    break;
                                case "女":
                                    sex = Sex.Woman.Id;
                                    break;
                                default:
                                    sex = Sex.Unknown.Id;
                                    break;
                            }

                            int typeId = 1;
                            switch (item.PersonIdentityId)
                            {
                                case "01":
                                    typeId = StudentType.CivilServant.Id;
                                    break;
                                default:
                                    typeId = StudentType.Professional.Id;
                                    break;
                            }
                            bool photoisVal = true;
                            if (photo is null)
                            {
                                photoisVal = false;

                            }

                            Student student = new Student(
                                item.PersonName.Trim()
                                , item.Idcard.Trim()
                                , item.Password.Trim()
                                , sex, typeId, item.Birthday, item.GraduateSchool, item.GraduateSpecialty, item.WorkDate
                                , officestr, pho?.PhotoUrl, photoisVal
                                , item.Email, false, item.Address, mA, mV, pid, StudentStatus.Normal.Id
                                );

                            lock (thisLock)
                            {
                                studentContent.Add(student);
                                ctr++;
                                if ((ctr >= oldPerson.Count() / SAVECTR) ||
                                    ((ctr == (oldPerson.Count % SAVECTR)) && insertCtr == SAVECTR)
                                    )
                                {
                                    Console.Write("开始提交数据库");
                                    context.Students.AddRange(studentContent);
                                    context.SaveChanges();
                                    insertCount += studentContent.Count();
                                    insertCtr++;
                                    Console.WriteLine(string.Format("已经提交...{0}次；本次提交{1}条;共提交{2}条", insertCtr.ToString(), studentContent.Count(), insertCount.ToString()));
                                    studentContent.Clear();
                                    ctr = 0;
                                }
                            }
                        });
                    }
                    #endregion

                    #region 导入专业技术职务信息
                    if (!context.ProfessionalInfos.Any())
                    {
                        IReadOnlyCollection<Student> students = context.Students.ToList();
                        IReadOnlyCollection<PersonTechnician> technicaian = mopcontext.PersonTechnician
                        .Include(s => s.Duty)
                        .Include(s => s.Person)
                        .ToList();
                        IReadOnlyCollection<ProfessionalTitle> professionalTitles = context.ProfessionalTitles.ToList();
                        //IReadOnlyCollection<Series> series = context.Seriess.ToList();
                        IReadOnlyCollection<PromoteType> promoteTypes = context.PromoteTypes.ToList();

                        List<ProfessionalInfo> professionalInfos = new List<ProfessionalInfo>();
                        
                        int ctr = 0;
                        int insertCtr = 0;
                        int insertCount = 0;
                        const int SAVECTR = 8;
                        Object thisLock = new Object();
                        Parallel.ForEach(technicaian, item =>
                        //foreach (var item in technicaian)
                        {
                            var professionalTitle = professionalTitles.Where(c => c.Name == item.Duty?.ZwName).FirstOrDefault();
                            var student = students.Where(c => c.IDCard == item.Person.Idcard.Trim()).FirstOrDefault();

                            if (professionalTitle != null && student != null)
                            {
                                ProfessionalInfo pinfo = new ProfessionalInfo(
                                professionalTitle.Id
                                , item.RepresentDate ?? DateTime.MinValue
                                , item.TopQualification
                                , PromoteType.FromName(item.AuditCategory).Id
                                , student.Id
                                );
                                //professionalInfos.Add(pinfo);
                                lock (thisLock)
                                {
                                    professionalInfos.Add(pinfo);
                                    //Console.WriteLine(string.Format("公务员信息 身份证:{0},性名:{1},ID:{2}", student.IDCard, student.Name, item.PersonId));
                                    ctr++;
                                    if ((ctr >= technicaian.Count() / SAVECTR) ||
                                        ((ctr == (technicaian.Count % SAVECTR)) && insertCtr == SAVECTR)
                                        )
                                    {
                                        Console.Write("专业技术职务信息 开始提交数据库");
                                        context.ProfessionalInfos.AddRange(professionalInfos);
                                        context.SaveChanges();
                                        insertCount += professionalInfos.Count();
                                        insertCtr++;
                                        Console.WriteLine(string.Format("已经提交...{0}次；本次提交{1}条;共提交{2}条", insertCtr.ToString(), professionalInfos.Count(), insertCount.ToString()));
                                        professionalInfos.Clear();
                                        ctr = 0;
                                    }
                                }
                            }

                            
                            //Console.WriteLine(string.Format("专业技术职务信息 身份证:{0},性名:{1},ID:{2}", student.IDCard, student.Name, item.PersonId));
                        });
                        //context.ProfessionalInfos.AddRange(professionalInfos);
                        //await context.SaveChangesAsync();
                    }
                    #endregion

                    #region 导入公务员信息
                    if (!context.CivilServantInfos.Any())
                    {
                        IReadOnlyCollection<Student> students = context.Students.ToList();
                        IReadOnlyCollection<PersonCivilServant> personCivilServants = mopcontext.PersonCivilServant
                        .Include(s => s.DutyLevelNavigation)
                        .Include(s => s.Person)
                        .ToList();
                        IReadOnlyCollection<DutyLevel> dutyLevels = context.DutyLevels.ToList();

                        List<CivilServantInfo> civilServantInfos = new List<CivilServantInfo>();

                        int ctr = 0;
                        int insertCtr = 0;
                        int insertCount = 0;
                        const int SAVECTR = 8;
                        Object thisLock = new Object();
                        Parallel.ForEach(personCivilServants, item =>
                        // foreach (var item in personCivilServants)
                        {
                            var dutylevel = dutyLevels.Where(c => c.Name == item.DutyLevelNavigation.DutyName).FirstOrDefault();
                            var student = students.Where(c => c.IDCard == item.Person.Idcard.Trim()).FirstOrDefault();

                            if (dutylevel != null && student != null)
                            {
                                CivilServantInfo civilServantInfo = new CivilServantInfo(
                                item.ChiefDuty,
                                dutylevel.Id
                                , item.RepresentDate
                                , item.RawQualification
                                , item.TopQualification
                                , item.IsJoinPromotion
                                , student.Id);


                                lock (thisLock)
                                {
                                    civilServantInfos.Add(civilServantInfo);
                                    //Console.WriteLine(string.Format("公务员信息 身份证:{0},性名:{1},ID:{2}", student.IDCard, student.Name, item.PersonId));
                                    ctr++;
                                    if ((ctr >= personCivilServants.Count() / SAVECTR) ||
                                        ((ctr == (personCivilServants.Count % SAVECTR)) && insertCtr == SAVECTR)
                                        )
                                    {
                                        Console.Write("公务员信息 开始提交数据库");
                                        context.CivilServantInfos.AddRange(civilServantInfos);
                                        context.SaveChanges();
                                        insertCount += civilServantInfos.Count();
                                        insertCtr++;
                                        Console.WriteLine(string.Format("已经提交...{0}次；本次提交{1}条;共提交{2}条", insertCtr.ToString(), civilServantInfos.Count(), insertCount.ToString()));
                                        civilServantInfos.Clear();
                                        ctr = 0;
                                    }
                                }
                            }
                        }
                        );
                        //context.CivilServantInfos.AddRange(civilServantInfos);
                        //await context.SaveChangesAsync();

                    }
                    #endregion

                    #region 导入培训点和培训点账号
                    if (!context.TrainingCenter.Any())
                    {
                        IReadOnlyCollection<PxdUnitInfo> pxdUnitInfo = mopcontext.PxdUnitInfo.ToList();
                        IReadOnlyCollection<PxdAccount> pxdAccount = mopcontext.PxdAccount.ToList();

                        List<TrainingCenter> trainingCenters = new List<TrainingCenter>();
                        foreach (var item in pxdUnitInfo)
                        {
                            var account = pxdAccount.FirstOrDefault(c => c.UnitID == item.Id);
                            int opentype = item.Type == 2 ? OpenType.CivilServant.Id : OpenType.Professional.Id;
                            TrainingCenter tc = new TrainingCenter(account?.LogName, account?.LogPassWord, item.PxdName, item.PxdAddress, opentype);
                            trainingCenters.Add(tc);
                        }
                        context.AddRange(trainingCenters);
                        await context.SaveChangesAsync();
                    }
                    #endregion

                    #region 导入科目

                    try
                    {
                        if (!(context.ExamSubjects.Any()))
                        {
                            Console.Write("开始导入科目");
                            IReadOnlyCollection<MOPDB.ExamSubject> examSubjects = mopcontext.ExamSubject.ToList();
                            IReadOnlyCollection<Series> series = context.Seriess.ToList();
                            IReadOnlyCollection<DirectoryZwClass> directoryZwClasses = mopcontext.DirectoryZwClass.ToList();

                            List<Domain.AggregatesModel.ExamSubjectAggregates.ExamSubject> examSubjects_ = new List<Domain.AggregatesModel.ExamSubjectAggregates.ExamSubject>();
                            foreach (var item in examSubjects)
                            {
                                string zwClassName = directoryZwClasses.FirstOrDefault(c => c.ClassId == item.ClassId)?.ClassName;
                                int? seriesId = zwClassName == null ? null : series.FirstOrDefault(c => c.Name == zwClassName.Trim())?.Id;
                                

                                Domain.AggregatesModel.ExamSubjectAggregates.ExamSubject examSubject = new Domain.AggregatesModel.ExamSubjectAggregates.ExamSubject(
                                item.SubjectId
                                , item.SubjectName
                                , item.Pxtype
                                            , item.Kstype ?? 1
                                            , seriesId
                                , ExamSubjectStatus.Default.Id
                                            , item.Mscount
                                            , item.CreditHour ?? 0
                                );
                                examSubjects_.Add(examSubject);
                            }

                            context.ExamSubjects.AddRange(examSubjects_);
                            await context.SaveChangesAsync();
                            Console.Write("导入科目...完成");
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion

                    #region 导入科目开设申请信息
                    if (!(context.ExamSubjectOpenInfo.Any()))
                    {
                        List<ExamSubjectOpenInfo> examSubjectOpenInfos = new List<ExamSubjectOpenInfo>()
                        {
                            new ExamSubjectOpenInfo(1,1,new DateTime(2018,5,12),new DateTime(2018,06,20),1,"2018-07左右考试",1)
                        };
                        context.ExamSubjectOpenInfo.AddRange(examSubjectOpenInfos);
                        await context.SaveChangesAsync();
                    }
                    #endregion
                }
            });


            Console.Write("seed");
        }
    }
}
