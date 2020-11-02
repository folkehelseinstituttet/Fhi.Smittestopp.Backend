using DIGNDB.App.SmitteStop.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly IGenericRepository<Translation> _translationRepository;
        private const string DenmarkIsoCode = "DK";

        public CountryRepository(DigNDB_SmittestopContext context, IGenericRepository<Translation> translationRepository) : base(context)
        {
            _translationRepository = translationRepository;
        }

        public override async Task<IEnumerable<Country>> GetAllAsync()
        {
            var countries = await base.GetAllAsync();

            var translations = _translationRepository.Get(t => t.EntityName == nameof(Country));
            foreach (var country in countries)
                country.EntityTranslations = translations.Where(t => t.EntityId == country.Id).ToList();

            return countries;
        }

        public async Task<IEnumerable<Country>> GetVisibleAsync()
        {
            var countries = await GetAllAsync();

            return countries.Where(c => c.VisitedCountriesEnabled).ToList();
        }

        public async Task<IEnumerable<Country>> GetGetCountriesToPullFrom()
        {
            var countries = await GetAllAsync();

            return countries.Where(c => c.PullingFromGatewayEnabled).ToList();
        }

        public Country FindByIsoCode(string isoCode)
        {
            return Get(filter: c => c.Code.ToLower() == isoCode.ToLower()).SingleOrDefault();
        }

        public IEnumerable<Country> FindByIsoCodes(IList<string> isoCodes)
        {
            var normalizedIsoCodes = isoCodes.Select(code => code.ToLower())
                .ToList();

            return _context.Country
                .Where(countryEntity => normalizedIsoCodes.Contains(countryEntity.Code.ToLower()))
                .ToList();
        }

        public Country GetDenmarkCountry()
        {
            return FindByIsoCode(DenmarkIsoCode);
        }

    }
}
