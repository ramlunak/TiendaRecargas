using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Models;

namespace TiendaRecargas.ViewModels
{
    public class VMAccount : BaseModelo
    {
        public List<account_info> Accounts { get; set; }
    }
}
