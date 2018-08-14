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

namespace PCME.Exam.Api.Infrastructure.ResourceOwnerPasswordValidator
{
	public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
	{
		private readonly ApplicationDbContext dbContext = null;
		public ResourceOwnerPasswordValidator(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public static Claim[] GetUserClaims(
			string accountId,
			string accountName,
			string workUnitId,
			string workUnitName,
			string displayName,
            string parentWorkUnitName,
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
            if (!string.IsNullOrEmpty(parentWorkUnitName))
            {
                claims.Add(new Claim("ParentWorkUnitName", parentWorkUnitName));
            }
			foreach (var item in valtype)
			{
				claims.Add(new Claim(JwtClaimTypes.Role, item));
			}
			return claims.ToArray();
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
					case "RoomAccount":
                        var account = await dbContext.ExaminationRoomAccount.Where(c => c.AccountName == context.UserName)
                            .Join(dbContext.TrainingCenter,l=>l.TrainingCenterId,r=>r.Id,(l,r)=>new { account=l,trainingcenter=r})
                            .FirstOrDefaultAsync();
                        if (account == null)
                        {
                            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "用户名不存在");
                        }
                        else {
                            if (!(account.account.Password == context.Password))
                            {
                                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "密码不正确");
                            }
                            else {
                                try
                                {
                                    context.Result = new GrantValidationResult(
                                                subject: account.account.AccountName,
                                                authenticationMethod: "custom",
                                                claims: GetUserClaims(
                                                    account.account.Id.ToString(),
                                                    account.account.AccountName,
                                                    account.account.TrainingCenterId.ToString(),
                                                    account.trainingcenter.Name,
                                                    account.account.AccountName,
                                                   "淄博市人力资源和社会保障局",
                                                    new string[] { valtype },
                                                    ""
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
