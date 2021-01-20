using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class ExposureKeyValidatorTests
    {
        private Mock<ICountryRepository> countryRepositoryMock;
        private ExposureKeyValidator exposureKeyValidator;
        private TemporaryExposureKeyBatchDto parameterArgument;
        private KeyValidationConfiguration configurationArgument;

        private Mock<ILogger<ExposureKeyValidator>> logger;


        [SetUp]
        public void Init()
        {
            SetupMockServices();
            exposureKeyValidator = new ExposureKeyValidator(countryRepositoryMock.Object, logger.Object);
            parameterArgument = new TemporaryExposureKeyBatchDto()
            {
                deviceVerificationPayload = "eyJhbGciOiJSUzI1NiIsIng1YyI6WyJNSUlGa3pDQ0JIdWdBd0lCQWdJUkFOY1NramRzNW42K0NBQUFBQUFwYTBjd0RRWUpLb1pJaHZjTkFRRUxCUUF3UWpFTE1Ba0dBMVVFQmhNQ1ZWTXhIakFjQmdOVkJBb1RGVWR2YjJkc1pTQlVjblZ6ZENCVFpYSjJhV05sY3pFVE1CRUdBMVVFQXhNS1IxUlRJRU5CSURGUE1UQWVGdzB5TURBeE1UTXhNVFF4TkRsYUZ3MHlNVEF4TVRFeE1UUXhORGxhTUd3eEN6QUpCZ05WQkFZVEFsVlRNUk13RVFZRFZRUUlFd3BEWVd4cFptOXlibWxoTVJZd0ZBWURWUVFIRXcxTmIzVnVkR0ZwYmlCV2FXVjNNUk13RVFZRFZRUUtFd3BIYjI5bmJHVWdURXhETVJzd0dRWURWUVFERXhKaGRIUmxjM1F1WVc1a2NtOXBaQzVqYjIwd2dnRWlNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0SUJEd0F3Z2dFS0FvSUJBUUNXRXJCUVRHWkdOMWlaYk45ZWhSZ2lmV0J4cWkyUGRneHcwM1A3VHlKWmZNeGpwNUw3ajFHTmVQSzVIemRyVW9JZDF5Q0l5Qk15eHFnYXpxZ3RwWDVXcHNYVzRWZk1oSmJOMVkwOXF6cXA2SkQrMlBaZG9UVTFrRlJBTVdmTC9VdVp0azdwbVJYZ0dtNWpLRHJaOU54ZTA0dk1ZUXI4OE5xd1cva2ZaMWdUT05JVVQwV3NMVC80NTIyQlJXeGZ3eGMzUUUxK1RLV2tMQ3J2ZWs2V2xJcXlhQzUyVzdNRFI4TXBGZWJ5bVNLVHZ3Zk1Sd3lLUUxUMDNVTDR2dDQ4eUVjOHNwN3dUQUhNL1dEZzhRb3RhcmY4T0JIa25vWjkyWGl2aWFWNnRRcWhST0hDZmdtbkNYaXhmVzB3RVhDdnFpTFRiUXRVYkxzUy84SVJ0ZFhrcFFCOUFnTUJBQUdqZ2dKWU1JSUNWREFPQmdOVkhROEJBZjhFQkFNQ0JhQXdFd1lEVlIwbEJBd3dDZ1lJS3dZQkJRVUhBd0V3REFZRFZSMFRBUUgvQkFJd0FEQWRCZ05WSFE0RUZnUVU2REhCd3NBdmI1M2cvQzA3cHJUdnZ3TlFRTFl3SHdZRFZSMGpCQmd3Rm9BVW1OSDRiaERyejV2c1lKOFlrQnVnNjMwSi9Tc3daQVlJS3dZQkJRVUhBUUVFV0RCV01DY0dDQ3NHQVFVRkJ6QUJoaHRvZEhSd09pOHZiMk56Y0M1d2Eya3VaMjl2Wnk5bmRITXhiekV3S3dZSUt3WUJCUVVITUFLR0gyaDBkSEE2THk5d2Eya3VaMjl2Wnk5bmMzSXlMMGRVVXpGUE1TNWpjblF3SFFZRFZSMFJCQll3RklJU1lYUjBaWE4wTG1GdVpISnZhV1F1WTI5dE1DRUdBMVVkSUFRYU1CZ3dDQVlHWjRFTUFRSUNNQXdHQ2lzR0FRUUIxbmtDQlFNd0x3WURWUjBmQkNnd0pqQWtvQ0tnSUlZZWFIUjBjRG92TDJOeWJDNXdhMmt1WjI5dlp5OUhWRk14VHpFdVkzSnNNSUlCQkFZS0t3WUJCQUhXZVFJRUFnU0I5UVNCOGdEd0FIY0E5bHlVTDlGM01DSVVWQmdJTUpSV2p1Tk5FeGt6djk4TUx5QUx6RTd4Wk9NQUFBRnZudXkwWndBQUJBTUFTREJHQWlFQTdlLzBZUnUzd0FGbVdIMjdNMnZiVmNaL21ycCs0cmZZYy81SVBKMjlGNmdDSVFDbktDQ0FhY1ZOZVlaOENDZllkR3BCMkdzSHh1TU9Ia2EvTzQxaldlRit6Z0IxQUVTVVpTNnc3czZ2eEVBSDJLaitLTURhNW9LKzJNc3h0VC9UTTVhMXRvR29BQUFCYjU3c3RKTUFBQVFEQUVZd1JBSWdFWGJpb1BiSnA5cUMwRGoyNThERkdTUk1BVStaQjFFaVZFYmJiLzRVdk5FQ0lCaEhrQnQxOHZSbjl6RHZ5cmZ4eXVkY0hUT1NsM2dUYVlBLzd5VC9CaUg0TUEwR0NTcUdTSWIzRFFFQkN3VUFBNElCQVFESUFjUUJsbWQ4TUVnTGRycnJNYkJUQ3ZwTVhzdDUrd3gyRGxmYWpKTkpVUDRqWUZqWVVROUIzWDRFMnpmNDluWDNBeXVaRnhBcU9SbmJqLzVqa1k3YThxTUowajE5ekZPQitxZXJ4ZWMwbmhtOGdZbExiUW02c0tZN1AwZXhmcjdIdUszTWtQMXBlYzE0d0ZFVWFHcUR3VWJHZ2wvb2l6MzhGWENFK0NXOEUxUUFFVWZ2YlFQVFliS3hZait0Q05sc3MwYlRTb0wyWjJkL2ozQnBMM01GdzB5eFNLL1VUcXlrTHIyQS9NZGhKUW14aStHK01LUlNzUXI2MkFuWmF1OXE2WUZvaSs5QUVIK0E0OFh0SXlzaEx5Q1RVM0h0K2FLb2hHbnhBNXVsMVhSbXFwOEh2Y0F0MzlQOTVGWkdGSmUwdXZseWpPd0F6WHVNdTdNK1BXUmMiLCJNSUlFU2pDQ0F6S2dBd0lCQWdJTkFlTzBtcUdOaXFtQkpXbFF1REFOQmdrcWhraUc5dzBCQVFzRkFEQk1NU0F3SGdZRFZRUUxFeGRIYkc5aVlXeFRhV2R1SUZKdmIzUWdRMEVnTFNCU01qRVRNQkVHQTFVRUNoTUtSMnh2WW1Gc1UybG5iakVUTUJFR0ExVUVBeE1LUjJ4dlltRnNVMmxuYmpBZUZ3MHhOekEyTVRVd01EQXdOREphRncweU1URXlNVFV3TURBd05ESmFNRUl4Q3pBSkJnTlZCQVlUQWxWVE1SNHdIQVlEVlFRS0V4VkhiMjluYkdVZ1ZISjFjM1FnVTJWeWRtbGpaWE14RXpBUkJnTlZCQU1UQ2tkVVV5QkRRU0F4VHpFd2dnRWlNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0SUJEd0F3Z2dFS0FvSUJBUURRR005RjFJdk4wNXprUU85K3ROMXBJUnZKenp5T1RIVzVEekVaaEQyZVBDbnZVQTBRazI4RmdJQ2ZLcUM5RWtzQzRUMmZXQllrL2pDZkMzUjNWWk1kUy9kTjRaS0NFUFpSckF6RHNpS1VEelJybUJCSjV3dWRnem5kSU1ZY0xlL1JHR0ZsNXlPRElLZ2pFdi9TSkgvVUwrZEVhbHROMTFCbXNLK2VRbU1GKytBY3hHTmhyNTlxTS85aWw3MUkyZE44RkdmY2Rkd3VhZWo0YlhocDBMY1FCYmp4TWNJN0pQMGFNM1Q0SStEc2F4bUtGc2JqemFUTkM5dXpwRmxnT0lnN3JSMjV4b3luVXh2OHZObWtxN3pkUEdIWGt4V1k3b0c5aitKa1J5QkFCazdYckpmb3VjQlpFcUZKSlNQazdYQTBMS1cwWTN6NW96MkQwYzF0Skt3SEFnTUJBQUdqZ2dFek1JSUJMekFPQmdOVkhROEJBZjhFQkFNQ0FZWXdIUVlEVlIwbEJCWXdGQVlJS3dZQkJRVUhBd0VHQ0NzR0FRVUZCd01DTUJJR0ExVWRFd0VCL3dRSU1BWUJBZjhDQVFBd0hRWURWUjBPQkJZRUZKalIrRzRRNjgrYjdHQ2ZHSkFib090OUNmMHJNQjhHQTFVZEl3UVlNQmFBRkp2aUIxZG5IQjdBYWdiZVdiU2FMZC9jR1lZdU1EVUdDQ3NHQVFVRkJ3RUJCQ2t3SnpBbEJnZ3JCZ0VGQlFjd0FZWVphSFIwY0RvdkwyOWpjM0F1Y0d0cExtZHZiMmN2WjNOeU1qQXlCZ05WSFI4RUt6QXBNQ2VnSmFBamhpRm9kSFJ3T2k4dlkzSnNMbkJyYVM1bmIyOW5MMmR6Y2pJdlozTnlNaTVqY213d1B3WURWUjBnQkRnd05qQTBCZ1puZ1F3QkFnSXdLakFvQmdnckJnRUZCUWNDQVJZY2FIUjBjSE02THk5d2Eya3VaMjl2Wnk5eVpYQnZjMmwwYjNKNUx6QU5CZ2txaGtpRzl3MEJBUXNGQUFPQ0FRRUFHb0ErTm5uNzh5NnBSamQ5WGxRV05hN0hUZ2laL3IzUk5Ha21VbVlIUFFxNlNjdGk5UEVhanZ3UlQyaVdUSFFyMDJmZXNxT3FCWTJFVFV3Z1pRK2xsdG9ORnZoc085dHZCQ09JYXpwc3dXQzlhSjl4anU0dFdEUUg4TlZVNllaWi9YdGVEU0dVOVl6SnFQalk4cTNNRHhyem1xZXBCQ2Y1bzhtdy93SjRhMkc2eHpVcjZGYjZUOE1jRE8yMlBMUkw2dTNNNFR6czNBMk0xajZieWtKWWk4d1dJUmRBdktMV1p1L2F4QlZielltcW13a201ekxTRFc1bklBSmJFTENRQ1p3TUg1NnQyRHZxb2Z4czZCQmNDRklaVVNweHU2eDZ0ZDBWN1N2SkNDb3NpclNtSWF0ai85ZFNTVkRRaWJldDhxLzdVSzR2NFpVTjgwYXRuWnoxeWc9PSJdfQ.eyJub25jZSI6InY1S0FXenU3YXArYnlRbnpnWnB0bGc9PSIsInRpbWVzdGFtcE1zIjoxNTkwOTU3NzYyMDg2LCJhcGtQYWNrYWdlTmFtZSI6ImNvbS5uZXRjb21wYW55LnNtaXR0ZXN0b3BfZXhwb3N1cmVfbm90aWZpY2F0aW9uIiwiYXBrRGlnZXN0U2hhMjU2IjoiTGUzZFRxMk9Bd0gxb2hJU0hUUk43MHc3QjYrZHVQc0JUb3p0cnpSUU5kTT0iLCJjdHNQcm9maWxlTWF0Y2giOnRydWUsImFwa0NlcnRpZmljYXRlRGlnZXN0U2hhMjU2IjpbIlRzSVA1SXZqZFo4VElPUEs4OHg0cUxWLzcwRFhZNHpVLzBmZ2tJUDV6M0k9Il0sImJhc2ljSW50ZWdyaXR5Ijp0cnVlLCJldmFsdWF0aW9uVHlwZSI6IkJBU0lDIn0.f8BgnP7h9JjLoqCU5IUNEX4paRederRGcq_KBccNzLq1JUNqL04Lx6sVW8PCJrWjOYmJd5FSks9wp0Rb85H78mwmE4QJBt90FEOtO-WFWVdVqU-MNfVxXQqfDexusZx2Sk2khVcqeJkWGd7UXHO-DwHaZuhsB15nkhAOBirEVKneZFSUW4dKRRzcXrSGyeOpnhr8YVGwokfidgZbkj0Z_fzfBbQN6ihr9PGknwLYyir04gNvB2k8hPqGmKs3RIB6zzw-nPG4HK1PoBl27GKl-n7nX97FHvRCn5j5XRZOVY5fnA2iDhbXcdYsNPLYs60ndRAE42oQW5xUiNDymIQteg",
                appPackageName = "com.netcompany.smittestop-exposure-notification",
                platform = "Android",

                keys = new List<TemporaryExposureKeyDto>()
                {
                    new TemporaryExposureKeyDto()
                    {
                        key = new byte[TemporaryExposureKeyDto.KeyLength] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                        rollingDuration = "1.00:00:00",
                        rollingStart = DateTime.UtcNow.Date.AddDays(-1),
                        transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
                    }
                },
                regions = new List<string>()
                {
                    "dk"
                },
                visitedCountries = new List<string>()
                {
                    "AT"
                }
            }
            ;
            configurationArgument = new KeyValidationConfiguration()
            {
                OutdatedKeysDayOffset = 14,
                PackageNames = new PackageNameConfig() { Apple = "com.netcompany.smittestop-exposure-notification", Google = "com.netcompany.smittestop-exposure-notification" },
            };
        }

        private void SetupMockServices()
        {
            countryRepositoryMock = new Mock<ICountryRepository>();
            countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("AT")).Returns(new Country() { Code = "AT", VisitedCountriesEnabled = true });
            countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("FR")).Returns(new Country() { Code = "FR", VisitedCountriesEnabled = false });
            countryRepositoryMock.Setup(countryRepo => countryRepo.FindByIsoCode("PL")).Returns((Country)null);
            countryRepositoryMock.Setup(countryRepo => countryRepo.GetApiOriginCountry()).Returns(new Country() { Code = "DK", VisitedCountriesEnabled = false });
            logger = new Mock<ILogger<ExposureKeyValidator>>();
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldPassValidation()
        {
            Assert.DoesNotThrow(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument));
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectKeyCount()
        {
            configurationArgument.OutdatedKeysDayOffset = 0;
            Assert.Throws<ArgumentException>(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument), "Incorrect key count.");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowDuplicateKeyValues()
        {
            parameterArgument.keys.Add(
                new TemporaryExposureKeyDto()
                {
                    key = parameterArgument.keys.First().key,
                    rollingDuration = "1.00:00:00",
                    rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                    transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
                });
            Assert.Throws<ArgumentException>(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument), "Duplicate key values.");
        }

        [TestCase(-16)]
        [TestCase(2)]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectStartDate(int value)
        {
            parameterArgument.keys[0].rollingStart = parameterArgument.keys[0].rollingStart.AddDays(value);
            Assert.Throws<ArgumentException>(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument), "Incorrect start date.");
        }

        [TestCase("1.00:00:01")]
        [TestCase("0.00:09:59")]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectSpan(string value)
        {
            parameterArgument.keys.Add(
                new TemporaryExposureKeyDto()
                {
                    key = new byte[TemporaryExposureKeyDto.KeyLength] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
                    rollingDuration = value,
                    rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                    transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
                });
            Assert.Throws<ArgumentException>(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument), "Incorrect span.");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectIntervals()
        {

            parameterArgument.keys.Add(
                new TemporaryExposureKeyDto()
                {
                    key = new byte[TemporaryExposureKeyDto.KeyLength] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                    rollingDuration = "1.00:00:00",
                    rollingStart = DateTime.UtcNow.Date.AddDays(-1),
                    transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
                });
            Assert.Throws<ArgumentException>(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument), $"Incorrect intervals. {parameterArgument.keys[0].ToJson()}");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowNotFoundVisitedCountry()
        {
            parameterArgument.visitedCountries = new List<string> { "PL" };

            Assert.Throws<ArgumentException>(
                () => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument,
                    configurationArgument), $"ISO codes of countries not found in the database: {string.Join(", ", parameterArgument.visitedCountries)}. ");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowDisabledCountry()
        {
            parameterArgument.visitedCountries = new List<string> { "FR" };

            Assert.Throws<ArgumentException>(
                () => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument,
                    configurationArgument), $"ISO codes of countries marked as disabled in the database: {string.Join(", ", parameterArgument.visitedCountries)}. ");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectRegion()
        {
            parameterArgument.regions = new List<string>() { "aaa" };
            Assert.Throws<ArgumentException>(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument), "Incorrect region.");
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_ShouldThrowIncorrectPackageName()
        {
            parameterArgument.appPackageName = "test";
            Assert.Throws<ArgumentException>(() => exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument), "Incorrect package name.");
        }

        [TestCase(new byte[TemporaryExposureKeyDto.KeyLength - 1] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(new byte[TemporaryExposureKeyDto.KeyLength + 1] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 })]
        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_KeySize_ShouldReturnFalse(byte[] value)
        {
            var tempKey = new TemporaryExposureKeyDto()
            {
                key = value,
                rollingDuration = "1.00:00:00",
                rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
            };
            parameterArgument.keys.Add(tempKey);
            exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument);
            Assert.IsFalse(parameterArgument.keys.Contains(tempKey));
        }

        [Test]
        public void TestValidateParameterAndThrowIfIncorrect_CorrectKey_ShouldReturnTrue()
        {
            var tempKeyWithCorrectLength = new TemporaryExposureKeyDto()
            {
                key = new byte[TemporaryExposureKeyDto.KeyLength] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 },
                rollingDuration = "1.00:00:00",
                rollingStart = DateTime.UtcNow.Date.AddDays(-2),
                transmissionRiskLevel = RiskLevel.RISK_LEVEL_MEDIUM_HIGH
            };
            parameterArgument.keys.Add(tempKeyWithCorrectLength);
            exposureKeyValidator.ValidateParameterAndThrowIfIncorrect(parameterArgument, configurationArgument);
            Assert.IsTrue(parameterArgument.keys.Contains(tempKeyWithCorrectLength));
        }
    }
}
