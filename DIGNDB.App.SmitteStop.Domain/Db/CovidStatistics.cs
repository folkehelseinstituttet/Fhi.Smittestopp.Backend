using System;

namespace DIGNDB.App.SmitteStop.Domain.Db
{
    public class CovidStatistics
    {
        public int Id { get; set; }
        public int ConfirmedCasesToday { get; set; }
        public int ConfirmedCasesTotal { get; set; }
        public int TestsConductedToday { get; set; }
        public int TestsConductedTotal { get; set; }
        public int PatientsAdmittedToday { get; set; }
        public int IcuAdmittedToday { get; set; }
        public int VaccinatedFirstDoseTotal { get; set; }
        public int VaccinatedFirstDoseToday { get; set; }
        public int VaccinatedSecondDoseTotal { get; set; }
        public int VaccinatedSecondDoseToday { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public override bool Equals(object? other)
        {
            CovidStatistics otherObject = other as CovidStatistics;
            return Id == otherObject.Id && ConfirmedCasesToday == otherObject.ConfirmedCasesToday
                                        && ConfirmedCasesTotal == otherObject.ConfirmedCasesTotal
                                        && TestsConductedToday == otherObject.TestsConductedToday
                                        && TestsConductedTotal == otherObject.TestsConductedTotal
                                        && PatientsAdmittedToday == otherObject.PatientsAdmittedToday
                                        && IcuAdmittedToday == otherObject.IcuAdmittedToday
                                        && VaccinatedFirstDoseTotal.Equals(otherObject.VaccinatedFirstDoseTotal)
                                        && VaccinatedFirstDoseToday.Equals(otherObject.VaccinatedFirstDoseToday)
                                        && VaccinatedSecondDoseTotal.Equals(otherObject.VaccinatedSecondDoseTotal)
                                        && VaccinatedSecondDoseToday.Equals(otherObject.VaccinatedSecondDoseToday)
                                        && ModificationDate.Equals(otherObject.ModificationDate)
                                        && EntryDate.Equals(otherObject.EntryDate);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(ConfirmedCasesToday);
            hashCode.Add(ConfirmedCasesTotal);
            hashCode.Add(TestsConductedToday);
            hashCode.Add(TestsConductedTotal);
            hashCode.Add(PatientsAdmittedToday);
            hashCode.Add(IcuAdmittedToday);
            hashCode.Add(VaccinatedFirstDoseTotal);
            hashCode.Add(VaccinatedFirstDoseToday);
            hashCode.Add(VaccinatedSecondDoseTotal);
            hashCode.Add(VaccinatedSecondDoseToday);
            hashCode.Add(ModificationDate);
            hashCode.Add(EntryDate);
            return hashCode.ToHashCode();
        }
    }
}