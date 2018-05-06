using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        public class test {
            public test(test2 db, IMediator mediator)
            {

            }
        }
        public class test2 {
            public string Name { get; set; }
            public test2()
            {
                Name = "test2";
            }
        }
        private readonly IServiceProvider provider;
        private readonly ServiceCollection _serviceCollection;
        public UnitTest1()
        {
            

            _serviceCollection = new ServiceCollection();
            //_serviceCollection.AddMediatR();
            _serviceCollection.AddDbContext<ApplicationDbContext>(
                    options =>
                    {
                        options.UseSqlServer(@"Server=.;Initial Catalog=PCME;Integrated Security=true");
                    }
                ).AddUnitOfWork<ApplicationDbContext>();
            _serviceCollection.AddMediatR();
            _serviceCollection.AddScoped<test>();
            _serviceCollection.AddScoped<test2>();
            _serviceCollection.AddUnitOfWork<ApplicationDbContext>();
            
            provider = _serviceCollection.BuildServiceProvider();
        }
        [Fact]
        public void Test1()
        {
            //var unitOfWork = provider.GetService<IUnitOfWork<ApplicationDbContext>>();
            //var repository = unitOfWork.GetRepository<WorkUnit>();
            //var workunit = new WorkUnit("370303", "◊Õ≤©Œ¿ ¢ø∆ºº", 1, "Ã∆¡÷", "18653311771", "3440", null, null, WorkUnitNature.JgUnit.Id,WorkUnitAccountType.Manager.Id);
            //repository.InsertAsync(workunit);
            //unitOfWork.SaveEntitiesAsync();
            //IUnitOfWork <ApplicationDbContext> unitofwork = provider.GetService<IUnitOfWork<ApplicationDbContext>>();
        }
    }
}
