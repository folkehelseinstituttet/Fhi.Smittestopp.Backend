using System;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class CovidStatisticsDto
    {
        public int ConfirmedCasesToday { get; set; }
        public int ConfirmedCasesTotal { get; set; }
        public int TestsConductedToday { get; set; }
        public int TestsConductedTotal { get; set; }
        public int PatientsAdmittedToday { get; set; }
        public int IcuAdmittedToday { get; set; }
        public double VaccinatedFirstDoseTotal { get; set; }
        public double VaccinatedFirstDoseToday { get; set; }
        public double VaccinatedSecondDoseTotal { get; set; }
        public double VaccinatedSecondDoseToday { get; set; }
        public DateTime Date { get; set; }
    }
}
