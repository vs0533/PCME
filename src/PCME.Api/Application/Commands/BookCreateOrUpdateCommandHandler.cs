using MediatR;
using PCME.Domain.AggregatesModel.BookAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class BookCreateOrUpdateCommandHandler : IRequestHandler<BookCreateOrUpdateCommand, Dictionary<string,object>>
    {
        private readonly ApplicationDbContext context;
        private readonly IRepository<Book> bookRepository;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        public BookCreateOrUpdateCommandHandler(ApplicationDbContext context, IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            bookRepository = this.unitOfWork.GetRepository<Book>();
        }
        public async Task<Dictionary<string, object>> Handle(BookCreateOrUpdateCommand request, CancellationToken cancellationToken)
        {
            var isExists = await bookRepository.FindAsync(request.Id);
            if (isExists != null)
            {
                isExists.Update(request.Num
                    , request.Name, request.PublishingHouse, request.ExamSubjectId, request.Pirce, request.Discount);
                bookRepository.Update(isExists);
                await unitOfWork.SaveChangesAsync();
                return GetShow(isExists.Id);

            }
            else {
                Book book = new Book(request.Num, request.Name, request.PublishingHouse, request.ExamSubjectId, request.Pirce, request.Discount);
                await bookRepository.InsertAsync(book);
                await unitOfWork.SaveChangesAsync();
                return GetShow(book.Id);
            }
        }
        public Dictionary<string, object> GetShow(int key) {
            var query = from books in context.Books
                        join examsubjects in context.ExamSubjects on books.ExamSubjectId equals examsubjects.Id into left1
                        from examsubjects in left1.DefaultIfEmpty()
                        where books.Id == key
                        select new { books, examsubjects };
            var result = query.Select(c => new Dictionary<string, object>
            {   
                {"id",c.books.Id},
                {"books.Name",c.books.Name},
                {"books.Num",c.books.Num},
                {"books.Pirce",c.books.Pirce},
                {"books.PublishingHouse",c.books.PublishingHouse},
                {"examsubjects.Id",c.examsubjects.Id},
                {"examsubjects.Name",c.examsubjects.Name}
            }).FirstOrDefault();

            return result;
        }
    }
}
