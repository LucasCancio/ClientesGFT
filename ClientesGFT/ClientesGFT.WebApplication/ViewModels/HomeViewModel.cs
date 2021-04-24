using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Util;
using System.Collections.Generic;

namespace ClientesGFT.WebApplication.ViewModels
{
    public class HomeViewModel
    {
        public FluxoFilter Filter { get; set; } = new FluxoFilter();
        public IEnumerable<Fluxo> Fluxos { get; set; }
    }
}
