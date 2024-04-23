using AutoMapper;
using Bluesoft.Bank.Business.DTOs;
using Bluesoft.Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluesoft.Bank.Business
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<AccountMovement, AccountMovementDto>();
        }
    }
}
