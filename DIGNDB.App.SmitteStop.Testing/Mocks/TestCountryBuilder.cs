using DIGNDB.App.SmitteStop.Domain.Db;
using System;

namespace DIGNDB.App.SmitteStop.Testing.Mocks
{
    public class TestCountryBuilder
    {
        private Country _instance;

        public TestCountryBuilder()
        {

        }

        private void CheckMissingInitializationOrThrow()
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("Missing initialization. Call New() method before using this one.");
            }
        }

        public TestCountryBuilder New(string isoCode)
        {
            _instance = new Country();
            _instance.Code = isoCode;
            return this;
        }

        public TestCountryBuilder SetIsVisibleOnVisitedCountriesList(bool visible)
        {
            CheckMissingInitializationOrThrow();
            _instance.VisitedCountriesEnabled = visible;
            return this;
        }

        public TestCountryBuilder SetIsPullingFromGatewayEnabled(bool enabled)
        {
            CheckMissingInitializationOrThrow();
            _instance.PullingFromGatewayEnabled = enabled;
            return this;
        }

        public Country Build()
        {
            CheckMissingInitializationOrThrow();
            return _instance;
        }

        #region Default configurations

        public static TestCountryBuilder Denmark
        {
            get
            {
                return new TestCountryBuilder()
                    .New("DK")
                    .SetIsPullingFromGatewayEnabled(false)
                    .SetIsVisibleOnVisitedCountriesList(false);
            }
        }

        public static TestCountryBuilder Germany
        {
            get
            {
                return new TestCountryBuilder()
                    .New("DE")
                    .SetIsPullingFromGatewayEnabled(true)
                    .SetIsVisibleOnVisitedCountriesList(true);
            }
        }

        public static TestCountryBuilder Poland
        {
            get
            {
                return new TestCountryBuilder()
                    .New("PL")
                    .SetIsPullingFromGatewayEnabled(true)
                    .SetIsVisibleOnVisitedCountriesList(true);
            }
        }

        public static TestCountryBuilder Latvia
        {
            get
            {
                return new TestCountryBuilder()
                    .New("LV")
                    .SetIsPullingFromGatewayEnabled(true)
                    .SetIsVisibleOnVisitedCountriesList(true);
            }
        }
        #endregion
    }
}
