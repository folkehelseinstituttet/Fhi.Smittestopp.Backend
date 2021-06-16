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
        public int PatientsAdmittedTotal { get; set; }
        public int IcuAdmittedToday { get; set; }
        public int IcuAdmittedTotal { get; set; }
        public int VaccinatedFirstDoseTotal { get; set; }
        public int VaccinatedFirstDoseToday { get; set; }
        public int VaccinatedSecondDoseTotal { get; set; }
        public int VaccinatedSecondDoseToday { get; set; }
        public int DeathsCasesTotal { get; set; }
        public DateTime Date { get; set; }
    }
}
