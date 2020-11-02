namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    /// <summary>
    /// Exposure Configuration element to be shared with the mobile apps based on:
    /// https://static.googleusercontent.com/media/www.google.com/pl//covid19/exposurenotifications/pdfs/Android-Exposure-Notification-API-documentation-v1.3.1.pdf
    /// Comments on fields come from the same document.
    /// </summary>
    public class ExposureConfiguration
    {
        //Minimum risk score. Excludes exposure incidents with scores lower than this.Defaults to no minimum
        public int MinimumRiskScore { get; set; }
        /*Scores for attenuation buckets. Must contain 8 scores, one for each bucket as defined below:
        attenuationScores[0] when Attenuation > 73
        attenuationScores[1] when 73 >= Attenuation > 63
        attenuationScores[2] when 63 >= Attenuation > 51
        attenuationScores[3] when 51 >= Attenuation > 33
        attenuationScores[4] when 33 >= Attenuation > 27
        attenuationScores[5] when 27 >= Attenuation > 15
        attenuationScores[6] when 15 >= Attenuation > 10
        attenuationScores[7] when 10 >= Attenuation */
        public int[] AttenuationScores { get; set; }
        //Weight to apply to the attenuation score.Must be in the range 0-100. Reserved for future use.
        public int AttenuationWeight { get; set; }
        /* Scores for days since last exposure buckets. Must contain 8 scores, one for each bucket as defined below:
        daysSinceLastExposureScores[0] when Days >= 14
        daysSinceLastExposureScores[1] when Days >= 12
        daysSinceLastExposureScores[2] when Days >= 10
        daysSinceLastExposureScores[3] when Days >= 8
        daysSinceLastExposureScores[4] when Days >= 6
        daysSinceLastExposureScores[5] when Days >= 4
        daysSinceLastExposureScores[6] when Days >= 2
        daysSinceLastExposureScores[7] when Days >= 0 */
        public int[] DaysSinceLastExposureScores { get; set; }
        //Weight to apply to the exposure score.Must be in the range 0-100. Reserved for future use.
        public int DaysSinceLastExposureWeight { get; set; }
        /* Scores for duration buckets. Must contain 8 scores, one for each bucket as defined below:
        durationScores[0] when Duration == 0
        durationScores[1] when Duration <= 5
        durationScores[2] when Duration <= 10
        durationScores[3] when Duration <= 15
        durationScores[4] when Duration <= 20
        durationScores[5] when Duration <= 25
        durationScores[6] when Duration <= 30
        durationScores[7] when Duration > 30 */
        public int[] DurationScores { get; set; }
        //Weight to apply to the duration score.Must be in the range 0-100. Reserved for future use.
        public double DurationWeight { get; set; }
        /* Scores for transmission risk buckets. Must contain 8 scores, one for each bucket as defined below:
        transmissionRiskScores[0] when RISK_SCORE_LOWEST
        transmissionRiskScores[1] when RISK_SCORE_LOW
        transmissionRiskScores[2] when RISK_SCORE_LOW_MEDIUM
        transmissionRiskScores[3] when RISK_SCORE_MEDIUM
        transmissionRiskScores[4] when RISK_SCORE_MEDIUM_HIGH
        transmissionRiskScores[5] when RISK_SCORE_HIGH
        transmissionRiskScores[6] when RISK_SCORE_VERY_HIGH
        transmissionRiskScores[7] when RISK_SCORE_HIGHEST */
        public int[] TransmissionRiskScores { get; set; }
        //Weight to apply to the transmission risk score.Must be in the range 0-100. Reserved for future use.
        public double TransmissionRiskWeight { get; set; }
        /* Attenuation thresholds to apply when calculating duration at attenuation.
        Must contain two thresholds, each in range of 0 - 255.
        durationAtAttenuationThresholds[0] has to be <=
        durationAtAttenuationThresholds[1]. These are used used to populate {@link
        ExposureSummary#getAttenuationDurationsInMinutes} and {@link
        ExposureInformation#getAttenuationDurationsInMinutes}. */
        public int[] DurationAtAttenuationThresholds { get; set; }

    }
}
