using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.WalletAggregates
{
    public interface IWalletRepository:IRepository<IWalletRepository>
    {
    }
}
