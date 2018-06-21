using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.Commands;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Book")]
    public class BookController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMediator _mediator;
        public BookController(ApplicationDbContext context,IMediator _mediator)
        {
            this.context = context;
            this._mediator = _mediator;
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Admin")]
        public IActionResult ReadStore(int start, int limit, string filter, string query, string navigates)
        {
            var workUnitId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var search = from books in context.Books
                         join examsubjects in context.ExamSubjects on books.ExamSubjectId equals examsubjects.Id into left1
                         from examsubjects in left1.DefaultIfEmpty()
                         select new { books, examsubjects };

            search = search.FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());

            var item = search.Skip(start).Take(limit);
            var total = search.Count();
            var result = search.Select(c => new Dictionary<string, object>
            {
                {"id",c.books.Id},
                {"books.Num",c.books.Num},
                {"books.Name",c.books.Name},
                {"books.PublishingHouse",c.books.PublishingHouse},
                {"books.Pirce",c.books.Pirce},
                {"examsubjects.Id",c.examsubjects.Id},
                {"examsubjects.Name",c.examsubjects.Name}
            });
            return Ok(new { total, data = result });
        }
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Post([FromBody]BookCreateOrUpdateCommand command, string opertype) {
            if (opertype == "new")
            {
                command.SetId(0);
            }
            var nameExists = context.Books.Where(c => c.Name == command.Name && c.Id != command.Id).Any();
            if (nameExists)
            {
                ModelState.AddModelError("books.name", "相同名称的书籍已经存在");
            }
            //var numExists = context.Books.Where(c => c.Num == command.Num).Any();
            //if (numExists)
            //{
            //    ModelState.AddModelError("books.num", "相同的书籍编号已经存在");
            //}
            ModelState.Remove("opertype");
            if (ModelState.IsValid)
            {
                Dictionary<string, object> result = await _mediator.Send(command);
                return Ok(new { success = true, data = result });
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove([FromBody]JObject data)
        {
            var id = data["id"].ToObject<int>();

            var del = await context.Books.FindAsync(id);
            if (del is null)
            {
                return Ok(new { message = "该条记录已经被删除" });
            }

            context.Books.Remove(del);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
