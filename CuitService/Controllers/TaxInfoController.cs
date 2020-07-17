using CuitService.TaxInfoProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CuitService.Controllers
{
    [Authorize]
    public class TaxInfoController
    {
        private readonly ILogger<TaxInfoController> _logger;
        private readonly ITaxInfoProviderService _taxInfoProviderService;

        public TaxInfoController(ILogger<TaxInfoController> logger, ITaxInfoProviderService taxInfoProviderService)
        {
            _logger = logger;
            _taxInfoProviderService = taxInfoProviderService;
        }

        // TODO: validate CUIT before, in filter, binder, etc. And avoid
        // primitive obsession in cuit parameter.
        [HttpGet("/taxinfo/by-cuit/{cuit}")]
        public async Task<TaxInfo> GetTaxInfoByCuit(string cuit)
            => await _taxInfoProviderService.GetTaxInfoByCuit(cuit);
    }
}
