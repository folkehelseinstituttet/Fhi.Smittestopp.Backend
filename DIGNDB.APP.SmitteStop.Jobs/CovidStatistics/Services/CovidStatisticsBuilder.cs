using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services
{
    public class CovidStatisticsBuilder : ICovidStatisticsBuilder
    {
        private App.SmitteStop.Domain.Db.CovidStatistics _statistics;
        private CovidStatisticsCsvContent _inputData;
        private readonly IDateTimeResolver _dateTimeResolver;
        private readonly ICovidStatisticsCsvDataRetrieveService _covidStatisticsCsvDataRetrieveService;

        public CovidStatisticsBuilder(IDateTimeResolver dateTimeResolver, ICovidStatisticsCsvDataRetrieveService covidStatisticsCsvDataRetrieveService)
        {
            _covidStatisticsCsvDataRetrieveService = covidStatisticsCsvDataRetrieveService;
            _dateTimeResolver = dateTimeResolver;
        }
        public App.SmitteStop.Domain.Db.CovidStatistics BuildStatistics(CovidStatisticsCsvContent inputData)
        {
            _inputData = inputData;
            _statistics = new App.SmitteStop.Domain.Db.CovidStatistics()
            {
                Date = _dateTimeResolver.GetDateTimeNow()
            };
            _statistics.PatientsAdmittedToday = Convert.ToInt32(CalculateAdmittedToday());
            _statistics.ConfirmedCasesToday = Convert.ToInt32(CalculateConfirmedToday());
            _statistics.TestsConductedToday = Convert.ToInt32(CalculateTestedToday());
            _statistics.IcuAdmittedToday = Convert.ToInt32(CalculateIcuAdmittedToday());
            _statistics.VaccinatedFirstDoseToday = Convert.ToDouble(CalculateVaccinatedFirstDoseToday());
            _statistics.VaccinatedSecondDoseToday = Convert.ToDouble(CalculateVaccinatedSecondDoseToday());
            _statistics.ConfirmedCasesTotal = Convert.ToInt32(CalculateConfirmedTotal());
            _statistics.TestsConductedTotal = Convert.ToInt32(CalculateTestedTotal());
            _statistics.VaccinatedFirstDoseTotal = Convert.ToDouble(CalculateVaccinatedFirstDoseTotal());
            _statistics.VaccinatedSecondDoseTotal = Convert.ToDouble(CalculateVaccinatedSecondDoseTotal());
            return (_statistics);
        }

        private object CalculateAdmittedToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetMostRecentEntry(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<HospitalCsvContent>),
                x => ((HospitalCsvContent)x).HospitalAdmitted);
        }

        private object CalculateConfirmedToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetMostRecentEntry(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent)x).Positive);
        }

        private object CalculateTestedToday()
        {
            var positiveTested = _covidStatisticsCsvDataRetrieveService.GetMostRecentEntry(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent)x).Positive);
            var negativeTested = _covidStatisticsCsvDataRetrieveService.GetMostRecentEntry(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent)x).Negative);
            return negativeTested + positiveTested;
        }

        private object CalculateIcuAdmittedToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetMostRecentEntry(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<HospitalCsvContent>),
                x => ((HospitalCsvContent)x).IcuPatients);
        }

        private object CalculateVaccinatedFirstDoseTotal()
        {
            return _covidStatisticsCsvDataRetrieveService.GetMostRecentEntry(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<VaccinatedCsvContent>)?.Where(x => (x as VaccinatedCsvContent)?.Region == "norge"),
                x => ((VaccinatedCsvContent)x).FirstDose);
        }

        private object CalculateVaccinatedSecondDoseTotal()
        {
            return _covidStatisticsCsvDataRetrieveService.GetMostRecentEntry(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<VaccinatedCsvContent>)?.Where(x => (x as VaccinatedCsvContent)?.Region == "norge"),
                x => ((VaccinatedCsvContent)x).SecondDose);
        }

        private object CalculateVaccinatedSecondDoseToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetDifferenceBetweenMostRecentEntries(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<VaccinatedCsvContent>)?.Where(x => (x as VaccinatedCsvContent)?.Region == "norge"),
                x => ((VaccinatedCsvContent)x).SecondDose);
        }

        private object CalculateVaccinatedFirstDoseToday()
        {
            return _covidStatisticsCsvDataRetrieveService.GetDifferenceBetweenMostRecentEntries(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<VaccinatedCsvContent>)?.Where(x => (x as VaccinatedCsvContent)?.Region == "norge"),
                x => ((VaccinatedCsvContent)x).FirstDose);
        }

        private object CalculateConfirmedTotal()
        {
            return _covidStatisticsCsvDataRetrieveService.GetSumOfEntries(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent)x).Positive);
        }

        private object CalculateTestedTotal()
        {
            var positiveTested = _covidStatisticsCsvDataRetrieveService.GetSumOfEntries(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent)x).Positive);
            var negativeTested = _covidStatisticsCsvDataRetrieveService.GetSumOfEntries(_inputData.FileContents.SingleOrDefault(x => x is IEnumerable<TestedCsvContent>),
                x => ((TestedCsvContent)x).Negative);
            return negativeTested + positiveTested;
        }
    }
}