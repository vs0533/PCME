using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
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
			string[] valtype, string accountType)
		{
			var claims = new List<Claim>()
			{
				new Claim("AccountId", accountId),
				new Claim("AccountName",accountName),
				new Claim("WorkUnitId",workUnitId),
				new Claim("WorkUnitName",workUnitName),
				new Claim("DisplayName",displayName),
				new Claim("AccountType",accountType)
			};
			foreach (var item in valtype)
			{
				claims.Add(new Claim(JwtClaimTypes.Role, item));
			}
			return claims.ToArray();
		}

		private async Task<Student> GetStudentByName(string username)
		{
			var studentRepository = unitOfWork.GetRepository<Student>();
			var student = await studentRepository.GetFirstOrDefaultAsync(predicate: c => c.IDCard == username);
			return student;
		}
		private async Task<WorkUnitAccount> GetAdminByAccountName(string username)
		{
			var workUnitAccount = await dbContext.WorkUnitAccounts
				.Include(s => s.WorkUnit)
				.FirstOrDefaultAsync(c => c.AccountName == username && c.WorkUnit.Level == 0 && c.WorkUnitAccountType == WorkUnitAccountType.Manager);
			return workUnitAccount;
		}
		private async Task<WorkUnitAccount> GetUnitAccountByAccountName(string username)
		{
			var workUnitAccount = await dbContext.WorkUnitAccounts
				.Include(s => s.WorkUnit)
				.FirstOrDefaultAsync(c => c.AccountName == username);
			return workUnitAccount;
		}
		private async Task<TrainingCenter> GetTrainingCenterByAccountName(string username)
		{
			var trainingcenter = await dbContext.TrainingCenter
				.FirstOrDefaultAsync(c => c.LogName == username);
			return trainingcenter;
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
													student.Name,
													new string[] { valtype, StudentType.Professional.Name },
													StudentType.Professional.Id.ToString()
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
													new string[] { valtype, WorkUnitAccountType.From(account.WorkUnitAccountTypeId).Name },
													account.WorkUnitAccountTypeId.ToString()
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
					case "Admin":
						var admin = await GetAdminByAccountName(context.UserName);
						if (admin == null)
						{
							context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "用户名不存在");
						}
						else
						{
							if (!(admin.PassWord == context.Password))
							{
								context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "密码不正确");
							}
							else
							{
								try
								{
									context.Result = new GrantValidationResult(
												subject: admin.WorkUnit.Code,
												authenticationMethod: "custom",
												claims: GetUserClaims(
													admin.Id.ToString(),
													admin.AccountName,
													admin.WorkUnit.Id.ToString(),
													admin.WorkUnit.Name,
													admin.WorkUnit.Name,
													new string[] { valtype, WorkUnitAccountType.From(admin.WorkUnitAccountTypeId).Name },
													admin.WorkUnitAccountTypeId.ToString()
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
					case "TrainingCenter":
						var training = await GetTrainingCenterByAccountName(context.UserName);
						if (training == null)
						{
							context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "用户名不存在");
						}
						else
						{
							if (!(training.LogPassWord == context.Password))
							{
								context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "密码不正确");
							}
							else
							{
								context.Result = new GrantValidationResult(
												subject: training.LogName,
												authenticationMethod: "custom",
												claims: GetUserClaims(
										            training.Id.ToString(),
													training.LogName,
										            training.Id.ToString(),
                                                    "",
                                                    training.Name,
													new string[] { valtype },
													""
													)
											);
							}
						}
						break;
				}
			}
			return;
		}
	}
}
