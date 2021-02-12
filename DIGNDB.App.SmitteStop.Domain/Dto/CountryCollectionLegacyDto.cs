using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class CountryCollectionLegacyDto
    {
        public IEnumerable<CountryLegacyDto> CountryCollection { get; set; }
    }
}