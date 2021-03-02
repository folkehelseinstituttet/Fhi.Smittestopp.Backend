using System;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    public class SSIStatisticsDto
    {
        public int ConfirmedCasesToday { get; set; }
        public int ConfirmedCasesTotal { get; set; }
        public int TestsConductedToday { get; set; }
        public int TestsConductedTotal { get; set; }
        public int PatientsAdmittedToday { get; set; }
        public double VaccinatedFirstDose { get; set; }
        public double VaccinatedSecondDose { get; set; }
        public DateTime Date { get; set; }
    }
}
