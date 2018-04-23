using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using System.Data.SqlClient;
using System.Linq;

namespace PCME.Infrastructure.Repositories
{
    public class WorkUnitRepository : RepositoryBase<WorkUnit>, IRepository<WorkUnit>
    {
        private readonly ApplicationDbContext _dbcontext;
        public WorkUnitRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public IQueryable<WorkUnit> GetTree(int id)
        {
            var sqlparams = new SqlParameter("id",id);
            string sql = @"WITH temp  
                            AS  
                            (  
                            SELECT * FROM Unit WHERE id = @id 
                            UNION ALL  
                            SELECT m.* FROM Unit  AS m  
                            INNER JOIN temp AS child ON m.PID = child.Id  
                            )  
                            SELECT * FROM temp";
            var item = _dbcontext.WorkUnits.FromSql(sql,sqlparams);
            return item;
        }
    }
}
