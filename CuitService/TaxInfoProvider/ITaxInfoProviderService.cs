using System.Threading.Tasks;

namespace CuitService.TaxInfoProvider
{
    public interface ITaxInfoProviderService
    {
        // TODO: avoid primitive obsession in cuit parameter.
        Task<TaxInfo> GetTaxInfoByCuit(string cuit);
    }
}
