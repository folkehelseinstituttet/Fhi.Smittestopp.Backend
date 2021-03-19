using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services
{
    public class CovidStatisticsBuilder : ICovidStatisticsBuilder
    {
        private App.SmitteStop.Domain.Db.CovidStatistics _statistics;
        private CovidStatisticsCsvContent _inputData;
        private readonly IDateTimeResolver _dateTimeResolver;
        private readonly ICovidStatisticsCsvDataRetrieveService _covidStatisticsCsvDataRetrieveService;

        public CovidStatisticsBuilder(IDateTimeResolver dateTimeResolver,
            ICovidStatisticsCsvDataRetrieveService covidStatisticsCsvDataRetrieveService)
        {
            _covidStatisticsCsvDataRetrieveService = covidStatisticsCsvDataRetrieveService;
            _dateTimeResolver = dateTimeResolver;
        }

        public App.SmitteStop.Domain.Db.CovidStatistics BuildStatistics(CovidStatisticsCsvContent inputData)
        {
            _inputData = inputData;
            _statistics = new App.SmitteStop.Domain.Db.CovidStatistics
            {
                ConfirmedCasesToday = CalculateConfirmedToday(),
                ConfirmedCasesTotal = CalculateConfirmedTotal(),

                TestsConductedToday = CalculateTestedToday(),
                TestsConductedTotal = CalculateTestedTotal(),

                PatientsAdmittedToday = CalculateAdmittedToday(),
                IcuAdmittedToday = CalculateIcuAdmittedToday(),

                VaccinatedFirstDoseTotal = CalculateVaccinatedFirstDoseTotal(),
                VaccinatedFirstDoseToday = CalculateVaccinatedFirstDoseToday(),
                VaccinatedSecondDoseTotal = CalculateVaccinatedSecondDoseTotal(),
                VaccinatedSecondDoseToday = CalculateVaccinatedSecondDoseToday(),

                ModificationDate = DateTime.UtcNow,
                EntryDate = _dateTimeResolver.GetDateTime().Date,
            };

            return _statistics;
        }

        #region Calculation methods

        private int CalculateConfirmedToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<TimeLocationCsvContent>)?
                    .Where(x => (x as TimeLocationCsvContent)?.Region == TimeLocationCsvContent.NorwayRegionName),
                x => ((TimeLocationCsvContent) x).ConfirmedCasesToday);
        }

        private int CalculateConfirmedTotal()
        {
            return _covidStatisticsCsvDataRetrieveService.GetEntryByDate(_inputData?.FileContents
                        .Single(x => x is IEnumerable<LocationCsvContent>)?
                        .Where(x => (x as LocationCsvContent)?.Region == LocationCsvContent.NorwayRegionName),
                    LocationCsvContent.LocationDateTime,
                    x => ((LocationCsvContent) x).ConfirmedCasesTotal);
        }

        private int CalculateTestedToday()
        {
            var positiveTested = _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent) x).Positive);
            var negativeTested = _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent) x).Negative);
            return negativeTested + positiveTested;
        }

        private int CalculateTestedTotal()
        {
            var positiveTested = _covidStatisticsCsvDataRetrieveService.GetSumFromEntries(_inputData?.FileContents
                    .Single(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent) x).Positive);
            var negativeTested = _covidStatisticsCsvDataRetrieveService.GetSumFromEntries(_inputData?.FileContents
                    .Single(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent) x).Negative);
            return negativeTested + positiveTested;
        }

        private int CalculateAdmittedToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .SingleOrDefault(x => x is IEnumerable<HospitalCsvContent>),
                x => ((HospitalCsvContent) x).HospitalAdmitted);
        }

        private int CalculateIcuAdmittedToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<HospitalCsvContent>),
                x => ((HospitalCsvContent) x).IcuPatients);
        }

        private int CalculateVaccinatedFirstDoseTotal()
        {
            return _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<VaccinatedCsvContent>)
                    ?.Where(x => (x as VaccinatedCsvContent)?.Region == VaccinatedCsvContent.NorwayRegionName),
                x => ((VaccinatedCsvContent) x).FirstDoseTotal);
        }

        private int CalculateVaccinatedFirstDoseToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<VaccinatedCsvContent>)
                    ?.Where(x => (x as VaccinatedCsvContent)?.Region == VaccinatedCsvContent.NorwayRegionName),
                x => ((VaccinatedCsvContent) x).FirstDose);
        }

        private int CalculateVaccinatedSecondDoseTotal()
        {
            return _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<VaccinatedCsvContent>)
                    ?.Where(x => (x as VaccinatedCsvContent)?.Region == VaccinatedCsvContent.NorwayRegionName),
                x => ((VaccinatedCsvContent) x).SecondDoseTotal);
        }

        private int CalculateVaccinatedSecondDoseToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetFromMostRecentEntry(_inputData?.FileContents
                    .Single(x => x is IEnumerable<VaccinatedCsvContent>)
                    ?.Where(x => (x as VaccinatedCsvContent)?.Region == VaccinatedCsvContent.NorwayRegionName),
                x => ((VaccinatedCsvContent) x).SecondDose);
        }

        #endregion
    }
}