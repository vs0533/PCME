using MediatR;
using Newtonsoft.Json;
using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class WorkUnitAccountCreateOrUpdateCommand:IRequest<Dictionary<string, object>>
    {
        public int Id { get; private set; }
        [Required]
        [JsonProperty("workunitaccount.AccountName")]
        public string AccountName { get; private set; }
        [JsonProperty("workunitaccount.HolderName")]
        public string HolderName { get; private set; }
        [JsonProperty("workunitaccounttype.Id")]
        public int WorkUnitAccountTypeId { get; private set; }
        [Required]
        [JsonProperty("workunitaccount.PassWord")]
        public string PassWord { get; private set; }
        public int WorkUnitId { get; private set; }
        public void SetUnitId(int id)
        {
            WorkUnitId = id;
        }
        public void SetId(int id)
        {
            Id = id;
        }
        
        public void SetWorkUnitAccountType(int typeid)
        {
            WorkUnitAccountTypeId = typeid;
        }
        public WorkUnitAccountCreateOrUpdateCommand(int id,string accountName, int workAccountTypeId, string passWord, string holderName)
        {
            Id = id;
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
            WorkUnitAccountTypeId = workAccountTypeId;
            PassWord = passWord ?? throw new ArgumentNullException(nameof(passWord));
            HolderName = holderName;
        }
        
    }
}
