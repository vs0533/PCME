using PCME.Domain.AggregatesModel.WorkUnitAccountAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.UnitAggregates
{
    public class WorkUnit:Entity,IAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }

        public string LinkMan { get;private set; }

        public string LinkPhone { get; private set; }

        public string Email { get;private set; }

        public string Address { get;private set; }

        public int? PID { get; private set; }

        public WorkUnitNature WorkUnitNature { get; private set; }
        public int WorkUnitNatureId { get; private set; }

        private readonly List<WorkUnitAccount> _accounts;
        public IReadOnlyCollection<WorkUnitAccount> Accounts => _accounts;

        private readonly List<WorkUnit> _childs;
        public IReadOnlyCollection<WorkUnit> Childs => _childs;

        public WorkUnit Parent { get; private set; }


        [Timestamp]
        public byte[] Version { get; set; }

        public WorkUnit()
        {
            _childs = new List<WorkUnit>();
            _accounts = new List<WorkUnitAccount>();
        }

        public void AddAccount(string password,int accountType,string holderName, string accountname = null) {
            
            var existed = _accounts.FirstOrDefault(c => c.AccountName == accountname);
            if (existed != null)
            {
                throw new Exception("单位账号已经存在");
            }
            string accountName_ = accountname ?? GenerateAccountName();
            _accounts.Add(new WorkUnitAccount(
                accountName_, 
                accountType,
                password,
                holderName
            ));
        }

        public WorkUnit(string code, string name, int level, string linkMan, 
            string linkPhoto, string email, string address, int? pID,
            int workUnitNatureId
            )
        {
            _childs = new List<WorkUnit>();
            _accounts = new List<WorkUnitAccount>();

            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Level = level;
            LinkMan = linkMan;
            LinkPhone = linkPhoto;
            Email = email;;
            Address = address;
            PID = pID;
            WorkUnitNatureId = workUnitNatureId;
        }

        private string GenerateAccountName()
        {
            try
            {
                
                return Code + (1).ToString().PadLeft(2, '0');

                //var lastaccount = _accounts.OrderByDescending(c => c.Id).Last();
                //string lastaccountName = lastaccount == null ? Code : lastaccount.AccountName;
                
                //string accountNumStr = lastaccountName.Substring(lastaccountName.Length - 2, 2);
                //string accountContent = lastaccountName.Substring(0, lastaccountName.Length - 2);
                //int accountNum = 1;
                //int.TryParse(accountNumStr, out accountNum);
                //accountNum += 1;
                //string okNum = accountNum.ToString().PadLeft(2, '0');
                //return accountContent + okNum;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void Update(string name,string linkman,string email,string linkphone,string address,int workunitnatureid)
        {
            Name = name;
            LinkMan = linkman;
            Email = email;
            LinkPhone = linkphone;
            Address = address;
            WorkUnitNatureId = workunitnatureid;
        }
    }
}
