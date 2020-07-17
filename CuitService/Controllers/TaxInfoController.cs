using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CuitService.Controllers
{
    [Authorize]
    [ApiController]
    public class TaxInfoController
    {
        private readonly ILogger<TaxInfoController> _logger;

        public TaxInfoController(ILogger<TaxInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/taxinfo/by-cuit/{cuit}")]
        public async Task<TaxInfo> GetTaxInfoByCuit([FromRoute] CuitNumber cuit)
        {
            return await Task.FromResult(new TaxInfo()
            {
                ActividadPrincipal = "620100-SERVICIOS DE CONSULTORES EN INFORMÁTICA Y SUMINISTROS DE PROGRAMAS DE INFORMÁTICA",
                Apellido = null,
                CUIT = cuit.OriginalValue, // TODO: return the formatted value
                CatIVA = "RI",
                CatImpGanancias = "RI",
                DomicilioCodigoPostal = "7600",
                DomicilioDatoAdicional = null,
                DomicilioDireccion = "CALLE FALSA 123 Piso:2",
                DomicilioLocalidad = "MAR DEL PLATA SUR",
                DomicilioPais = "AR",
                DomicilioProvincia = "01",
                DomicilioTipo = "FISCAL",
                Error = false,
                EstadoCUIT = "ACTIVO",
                Message = null,
                Monotributo = false,
                MonotributoActividad = null,
                MonotributoCategoria = null,
                Nombre = null,
                PadronData = null,
                ParticipacionesAccionarias = true,
                PersonaFisica = false,
                RazonSocial = "RZS C.S. SA",
                StatCode = 0
            });
        }
    }
}
