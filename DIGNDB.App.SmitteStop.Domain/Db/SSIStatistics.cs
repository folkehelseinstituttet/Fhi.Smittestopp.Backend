using System;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public partial class SSIStatistics
    {
        public int Id { get; set; }
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