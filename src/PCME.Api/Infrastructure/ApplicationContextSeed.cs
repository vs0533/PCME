using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
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
                WorkUnitNature.Company
            };
        }
        private IEnumerable<WorkUnitAccountType> GetPredefinedWorkUnitAccountType()
        {
            return new List<WorkUnitAccountType>()
            {
                WorkUnitAccountType.Manager,
                WorkUnitAccountType.CE,
                WorkUnitAccountType.CS
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
                    #region 美剧字典表初始化数据
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
                    #endregion
                    if (!context.WorkUnits.Any())
                    {
                        IEnumerable<WorkUnit> workunits = new List<WorkUnit>()
                        {
                            new WorkUnit("3703","淄博市人力资源和社会保障局",1,"卢瑞生","","","",null,WorkUnitNature.JgUnit.Id,WorkUnitAccountType.Manager.Id)
                        };
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
                        var p = mopcontext.DirectoryZwName.Include(s=>s.ClassNameNavigation).ToList();
                        var levels = context.Levels.ToList();
                        var serieses = context.Seriess.ToList();
                        var specialtys = context.Specialtys.ToList();

                        List<ProfessionalTitle> ptitles = new List<ProfessionalTitle>();

                        foreach (var item in p)
                        {
                            string name = item.ZwName;
                            Specialty specialty = specialtys.FirstOrDefault(c => c.Name == item.Zy.Trim()) ?? specialtys.FirstOrDefault(c=>c.Name == "其他");
                            Level level = levels.FirstOrDefault(c => c.Name == item.ZcJb.Trim()) ?? levels.FirstOrDefault(c=>c.Name == "未定");
                            Series series = serieses.FirstOrDefault(c => c.Name == item.ClassNameNavigation?.ClassName.Trim()) ?? serieses.FirstOrDefault(c=>c.Name == "其他");

                            ProfessionalTitle ptitle = new ProfessionalTitle(name,specialty, series,level);
                            ptitles.Add(ptitle);
                        }
                        context.ProfessionalTitles.AddRange(ptitles);
                        await context.SaveChangesAsync();
                    }
                    //if (!(context.WorkUnits.Count() == 1))
                    //{
                    //    var u_all = mopcontext.Unit.Include(s=>s.Account).ToList();
                    //    var u_1 = u_all.Where(c => c.UnitId.Length == 6).ToList();
                    //    var u_2 = u_all.Where(c => c.UnitId.Length == 8).ToList();
                    //    List<WorkUnit> workUnits = new List<WorkUnit>();
                        
                    //    foreach (var item in collection)
                    //    {

                    //        WorkUnit workUnit = new WorkUnit(
                                
                    //            );
                    //    }
                    //}
                }
            });


            Console.Write("seed");
        }
    }
}
