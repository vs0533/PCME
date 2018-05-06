﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.AggregatesModel.StudentAggregates;
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
                            var parentUnitId = u_1.SingleOrDefault(c => c.UnitId == item.UnitId.Substring(0, item.UnitId.Length - 3));
                            var parentUnit = curAddedWorkUnit2.SingleOrDefault(c => c.Code == parentUnitId.UnitId);
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
                            var parentUnitId = u_2.SingleOrDefault(c => c.UnitId == item.UnitId.Substring(0, item.UnitId.Length - 3));
                            var parentUnit = curAddedWorkUnit3.SingleOrDefault(c => c.Code == parentUnitId.UnitId);
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
                            var parentUnitId = u_3.SingleOrDefault(c => c.UnitId == item.UnitId.Substring(0, item.UnitId.Length - 3));
                            var parentUnit = curAddedWorkUnit4.SingleOrDefault(c => c.Code == parentUnitId.UnitId);
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

                    if (!context.Students.Any())
                    {
                        var photo = mopcontext.PersonPhoto.ToList();
                        var office = mopcontext.UnitDept.ToList();
                        var money = mopcontext.Money.ToList();
                        var oldPerson = mopcontext.Person
                            .Include(s=>s.PersonIdentity)
                            .ToList();
                        var curWorkUnits = context.WorkUnits.ToList();
                        var curProfessionTitles = context.ProfessionalTitles.ToList();
                        

                        List<Student> studentContent = new List<Student>();
                        //foreach (var item in oldPerson)
                        Parallel.ForEach(oldPerson, item => {
                            try
                            {
                                int pid = curWorkUnits.FirstOrDefault(c => c.Code == item.WorkUnitId).Id;
                                //int professionalTitleId = 1;//curProfessionTitles.SingleOrDefault(c => c.Name == "待定").Id;

                                var m = money.FirstOrDefault(c => c.PersonId == item.PersonId);
                                decimal mV = m == null ? 0 : m.MoneyVirtual;
                                decimal mA = m == null ? 0 : m.MoneyActual;

                                var of = office.FirstOrDefault(c => c.DeptId == item.DeptId);

                                string officestr = of == null ? "待定" : of.DeptName;
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

                                Student student = new Student(
                                    item.Idcard
                                    , item.PersonName
                                    , item.Password
                                    , sex, typeId, item.Birthday, item.GraduateSchool, item.GraduateSpecialty, item.WorkDate
                                    , officestr, pho == null ? string.Empty : pho.PhotoUrl, pho == null ? false : pho.IsOk
                                    , item.Email, false, item.Address, mA, mV, pid
                                    );
                                studentContent.Add(student);
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        });
                        //foreach (var item in oldPerson)
                        //{
                            
                            
                        //}
                        //var studenttypeNull = studentContent.Where(c => c.StudentType is null).ToList();
                        //var studenttypeNull1 = studentContent.Where(c => c.StudentType != null).ToList();
                        context.Students.AddRange(studentContent);
                        await context.SaveChangesAsync();
                    }
                }
            });


            Console.Write("seed");
        }
    }
}
