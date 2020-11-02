using System;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    /// <summary>
    /// Intersection entity (many to many)
    /// </summary>
    public partial class TemporaryExposureKeyCountry
    {
        public long CountryId { get; set; }
        public Country Country { get; set; }

        public Guid TemporaryExposureKeyId { get; set; }
        public TemporaryExposureKey TemporaryExposureKey { get; set; }
    }
}
