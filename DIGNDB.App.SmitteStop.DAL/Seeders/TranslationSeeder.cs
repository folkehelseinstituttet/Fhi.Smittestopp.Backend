using DIGNDB.App.SmitteStop.Domain.Db;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.DAL.Seeders
{
    public class TranslationSeeder
    {
        public IEnumerable<dynamic> GetSeedData()
        {
            return new[]
            {
                new {Id = 1L, EntityId = 1L, EntityName = nameof(Country), Value = "Østrig", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 2L, EntityId = 2L, EntityName = nameof(Country), Value = "Belgien", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 3L, EntityId = 3L, EntityName = nameof(Country), Value = "Bulgarien", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 4L, EntityId = 4L, EntityName = nameof(Country), Value = "Kroatien", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 5L, EntityId = 5L, EntityName = nameof(Country), Value = "Cypern", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 6L, EntityId = 6L, EntityName = nameof(Country), Value = "Tjekkiet", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 7L, EntityId = 7L, EntityName = nameof(Country), Value = "Danmark", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 8L, EntityId = 8L, EntityName = nameof(Country), Value = "Estland", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 9L, EntityId = 9L, EntityName = nameof(Country), Value = "Finland", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 10L, EntityId = 10L, EntityName = nameof(Country), Value = "Frankrig", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 11L, EntityId = 11L, EntityName = nameof(Country), Value = "Tyskland", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 12L, EntityId = 12L, EntityName = nameof(Country), Value = "Grækenland", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 13L, EntityId = 13L, EntityName = nameof(Country), Value = "Ungarn", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 14L, EntityId = 14L, EntityName = nameof(Country), Value = "Irland", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 15L, EntityId = 15L, EntityName = nameof(Country), Value = "Italien", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 16L, EntityId = 16L, EntityName = nameof(Country), Value = "Letland", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 17L, EntityId = 17L, EntityName = nameof(Country), Value = "Litauen", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 18L, EntityId = 18L, EntityName = nameof(Country), Value = "Luxembourg", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 19L, EntityId = 19L, EntityName = nameof(Country), Value = "Malta", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 20L, EntityId = 20L, EntityName = nameof(Country), Value = "Holland", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 21L, EntityId = 21L, EntityName = nameof(Country), Value = "Polen", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 22L, EntityId = 22L, EntityName = nameof(Country), Value = "Portugal", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 23L, EntityId = 23L, EntityName = nameof(Country), Value = "Rumænien", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 24L, EntityId = 24L, EntityName = nameof(Country), Value = "Slovakiet", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 25L, EntityId = 25L, EntityName = nameof(Country), Value = "Slovenien", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 26L, EntityId = 26L, EntityName = nameof(Country), Value = "Spanien", LanguageCountryId = CountrySeeder.NorwayCountryId},
                new {Id = 27L, EntityId = 27L, EntityName = nameof(Country), Value = "Sverige", LanguageCountryId = CountrySeeder.NorwayCountryId},

                new {Id = 28L, EntityId = 1L, EntityName = nameof(Country), Value = "Austria", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 29L, EntityId = 2L, EntityName = nameof(Country), Value = "Belgium", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 30L, EntityId = 3L, EntityName = nameof(Country), Value = "Bulgaria", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 31L, EntityId = 4L, EntityName = nameof(Country), Value = "Croatia", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 32L, EntityId = 5L, EntityName = nameof(Country), Value = "Cyprus", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 33L, EntityId = 6L, EntityName = nameof(Country), Value = "Czech Republic", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 34L, EntityId = 7L, EntityName = nameof(Country), Value = "Denmark", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 35L, EntityId = 8L, EntityName = nameof(Country), Value = "Estonia", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 36L, EntityId = 9L, EntityName = nameof(Country), Value = "Finland", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 37L, EntityId = 10L, EntityName = nameof(Country), Value = "France", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 38L, EntityId = 11L, EntityName = nameof(Country), Value = "Germany", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 39L, EntityId = 12L, EntityName = nameof(Country), Value = "Greece", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 40L, EntityId = 13L, EntityName = nameof(Country), Value = "Hungary", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 41L, EntityId = 14L, EntityName = nameof(Country), Value = "Ireland", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 42L, EntityId = 15L, EntityName = nameof(Country), Value = "Italy", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 43L, EntityId = 16L, EntityName = nameof(Country), Value = "Latvia", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 44L, EntityId = 17L, EntityName = nameof(Country), Value = "Lithuania", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 45L, EntityId = 18L, EntityName = nameof(Country), Value = "Luxembourg", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 46L, EntityId = 19L, EntityName = nameof(Country), Value = "Malta", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 47L, EntityId = 20L, EntityName = nameof(Country), Value = "Netherlands", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 48L, EntityId = 21L, EntityName = nameof(Country), Value = "Poland", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 49L, EntityId = 22L, EntityName = nameof(Country), Value = "Portugal", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 50L, EntityId = 23L, EntityName = nameof(Country), Value = "Romania", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 51L, EntityId = 24L, EntityName = nameof(Country), Value = "Slovakia", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 52L, EntityId = 25L, EntityName = nameof(Country), Value = "Slovenia", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 53L, EntityId = 26L, EntityName = nameof(Country), Value = "Spain", LanguageCountryId = CountrySeeder.EnglandCountryId},
                new {Id = 54L, EntityId = 27L, EntityName = nameof(Country), Value = "Sweden", LanguageCountryId = CountrySeeder.EnglandCountryId}
            };
        }
    }
}