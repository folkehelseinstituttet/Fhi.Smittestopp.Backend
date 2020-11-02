using System;
using System.Collections.Generic;
using System.Globalization;

namespace DIGNDB.App.SmitteStop.Domain.Dto
{
    //copied verbatim from: https://github.com/googlesamples/android-play-safetynet
    public sealed class AttestationStatement
    {
        /// <summary>
        /// The original claims dictionary.
        /// </summary>
        public Dictionary<string, string> Claims { get; private set; }

        /// <summary>
        /// The nonce provided when running the attestation on the device.
        /// </summary>
        public byte[] Nonce { get; private set; }

        /// <summary>
        /// The timestamp when the attestation was performed.
        /// </summary>
        public long TimestampMs { get; private set; }

        /// <summary>
        /// The package name of the calling APK.
        /// </summary>
        public string ApkPackageName { get; private set; }

        /// <summary>
        /// The SHA256 digest of the calling APK.
        /// </summary>
        public byte[] ApkDigestSha256 { get; private set; }

        /// <summary>
        /// The SHA256 digest of the signing certificate for the APK.
        /// </summary>
        public byte[] ApkCertificateDigestSha256 { get; private set; }

        /// <summary>
        /// Whether or not the device was determined to have a CTS profile
        /// match.
        /// </summary>
        public bool CtsProfileMatch { get; private set; }

        /// <summary>
        /// Whether or not the device was determined to have basic integrity.
        /// </summary>
        public bool BasicIntegrity { get; private set; }

        /// <summary>
        /// Constructs an Attestation statement from a dictionary of claims.
        /// </summary>
        /// <param name="claims">The claims retrieved from an attestation
        /// statement string.</param>
        public AttestationStatement(Dictionary<string, string> claims)
        {
            Claims = claims;

            if (claims.ContainsKey("nonce"))
            {
                Nonce = Convert.FromBase64String(claims["nonce"]);
            }

            if (claims.ContainsKey("timestampMs"))
            {
                long timestampMsLocal;
                long.TryParse(
                    claims["timestampMs"],
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out timestampMsLocal);
                TimestampMs = timestampMsLocal;
            }

            if (claims.ContainsKey("apkPackageName"))
            {
                ApkPackageName = claims["apkPackageName"];
            }

            if (claims.ContainsKey("apkDigestSha256"))
            {
                ApkDigestSha256 = Convert.FromBase64String(
                    claims["apkDigestSha256"]);
            }

            if (claims.ContainsKey("apkCertificateDigestSha256"))
            {
                ApkCertificateDigestSha256 = Convert.FromBase64String(
                    claims["apkCertificateDigestSha256"]);
            }

            if (claims.ContainsKey("ctsProfileMatch"))
            {
                bool ctsProfileMatchLocal;
                bool.TryParse(
                    claims["ctsProfileMatch"],
                    out ctsProfileMatchLocal);
                CtsProfileMatch = ctsProfileMatchLocal;
            }

            if (claims.ContainsKey("basicIntegrity"))
            {
                bool basicIntegrityLocal;
                bool.TryParse(
                    claims["basicIntegrity"],
                    out basicIntegrityLocal);
                BasicIntegrity = basicIntegrityLocal;
            }
        }
    }
}
