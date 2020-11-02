
namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    /**
     * https://developers.google.com/android/exposure-notifications/exposure-notifications-api#data-structures TemporaryExposureKey section
     */
    public enum ReportType
    {
        // Report type metadata is missing from the diagnosis key.
        UNKNOWN = 0,
        //  A medical provider has confirmed the user had a positive diagnostic test for COVID-19.
        CONFIRMED_TEST = 1,
        //  A medical provider has confirmed the user had symptoms consistent with a COVID-19 diagnosis.
        CONFIRMED_CLINICAL_DIAGNOSIS = 2,
        // The user has self-reported symptoms consistent with COVID-19 without confirmation from a medical provider.
        SELF_REPORT = 3,
        // This value is reserved for future use.
        RECURSIVE = 4,
        // In v1.5, REVOKED is not used. In v1.6 and higher, REVOKED eliminates exposures associated with that key from the detected exposures
        REVOKED = 5
    }
}
