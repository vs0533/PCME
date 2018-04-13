using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure.ResourceOwnerPasswordValidator
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork = null;
        public ResourceOwnerPasswordValidator(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public static Claim[] GetUserClaims(int userid, string displayname, string username, string email, string valtype)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, userid.ToString()),
                new Claim("DisplayName",displayname),
                new Claim(JwtClaimTypes.Name, username),
                new Claim(JwtClaimTypes.Email, email),
                new Claim(JwtClaimTypes.Role, valtype)
            };
        }

        private async Task<Student> GetStudentByName(string username)
        {
            var studentRepository = unitOfWork.GetRepository<Student>();
            var student = await studentRepository.GetFirstOrDefaultAsync(predicate: c => c.IDCard == username);
            return student;
        }
        private async Task<WorkUnit> GetUnitByName(string username)
        {
            var reporitory = unitOfWork.GetRepository<WorkUnit>();
            var result = await reporitory.GetFirstOrDefaultAsync(predicate: c => c.Code == username);
            return result;
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
                                                claims: GetUserClaims(student.Id, student.Name, student.IDCard, student.Email, valtype)
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
                        var unit = await GetUnitByName(context.UserName);
                        if (unit == null)
                        {
                            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "用户名不存在");
                        }
                        else
                        {
                            if (!(unit.PassWord == context.Password))
                            {
                                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "密码不正确");
                            }
                            else
                            {
                                try
                                {
                                    context.Result = new GrantValidationResult(
                                                subject: unit.Code,
                                                authenticationMethod: "custom",
                                                claims: GetUserClaims(unit.Id,unit.Name,unit.Code, unit.Email, valtype)
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
