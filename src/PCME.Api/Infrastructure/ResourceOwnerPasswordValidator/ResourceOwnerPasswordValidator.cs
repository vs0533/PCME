using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure.ResourceOwnerPasswordValidator
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork = null;
        private readonly ApplicationDbContext dbContext = null;
        public ResourceOwnerPasswordValidator(IUnitOfWork<ApplicationDbContext> unitOfWork, ApplicationDbContext dbContext)
        {
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }
        public static Claim[] GetUserClaims(
            string accountId,
            string accountName,
            string workUnitId,
            string workUnitName,
            string displayName,
            string valtype, string accountType)
        {
            return new Claim[]
            {
                new Claim("AccountId", accountId),
                new Claim("AccountName",accountName),
                new Claim("WorkUnitId",workUnitId),
                new Claim("WorkUnitName",workUnitName),
                new Claim("DisplayName",displayName),
                new Claim(JwtClaimTypes.Role, valtype),
                new Claim("AccountType",accountType)
            };
        }

        private async Task<Student> GetStudentByName(string username)
        {
            var studentRepository = unitOfWork.GetRepository<Student>();
            var student = await studentRepository.GetFirstOrDefaultAsync(predicate: c => c.IDCard == username);
            return student;
        }
        private async Task<WorkUnitAccount> GetUnitAccountByAccountName(string username)
        {
            var workUnitAccount = await dbContext.WorkUnitAccounts
                .Include(s=>s.WorkUnit)
                .FirstOrDefaultAsync(c => c.AccountName == username);
            return workUnitAccount;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var valtype = context.Request.Raw["valtype"];
            if (valtype == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "登陆类型不正确");
            }
            else
            {
                switch (valtype)
                {
                    case "Student":
                        var student = await GetStudentByName(context.UserName);
                        var studentUnit = await (from c in dbContext.WorkUnits
                                                 where c.Id == student.WorkUnitId
                                                 select c).FirstOrDefaultAsync();
                        if (student == null)
                        {
                            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "用户名不存在");
                        }
                        else
                        {
                            if (!(student.Password == context.Password))
                            {
                                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "密码不正确");
                            }
                            else
                            {
                                try
                                {
                                    context.Result = new GrantValidationResult(
                                                subject: student.Name,
                                                authenticationMethod: "custom",
                                                claims: GetUserClaims(
                                                    student.Id.ToString(),
                                                    student.IDCard,
                                                    student.WorkUnitId.ToString(),
                                                    studentUnit.Name,
                                                    student.Name, valtype, StudentType.Professional.Id.ToString()
                                                    )
                                            );
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message);
                                }
                            }
                        }
                        break;
                    case "Unit":
                        var account = await GetUnitAccountByAccountName(context.UserName);
                        if (account == null)
                        {
                            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "用户名不存在");
                        }
                        else
                        {
                            if (!(account.PassWord == context.Password))
                            {
                                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "密码不正确");
                            }
                            else
                            {
                                try
                                {
                                    context.Result = new GrantValidationResult(
                                                subject: account.WorkUnit.Code,
                                                authenticationMethod: "custom",
                                                claims: GetUserClaims(
                                                    account.Id.ToString(),
                                                    account.AccountName,
                                                    account.WorkUnit.Id.ToString(),
                                                    account.WorkUnit.Name,
                                                    account.WorkUnit.Name,
                                                    valtype, account.WorkUnitAccountTypeId.ToString()
                                                    )
                                            );
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message);
                                }
                            }
                        }
                        break;
                }
            }
            return;
        }
    }
}
