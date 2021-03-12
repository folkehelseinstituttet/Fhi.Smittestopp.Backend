using AutoMapper;
using System.Collections.Generic;
using TemporaryExposureKeyGatewayDtoProto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayDto;

namespace FederationGatewayApi.Mappers
{
    public class EuGatewayProtoToDtosMapper : Profile
    {
        public EuGatewayProtoToDtosMapper()
        {
            CreateMap<TemporaryExposureKeyGatewayDtoProto, Models.TemporaryExposureKeyGatewayDto>().ConvertUsing(new TemporaryExposureKeyGatewayProt2DtoConverter());
        }

        public class TemporaryExposureKeyGatewayProt2DtoConverter : ITypeConverter<TemporaryExposureKeyGatewayDtoProto, Models.TemporaryExposureKeyGatewayDto>
        {
            public Models.TemporaryExposureKeyGatewayDto Convert(TemporaryExposureKeyGatewayDtoProto source, Models.TemporaryExposureKeyGatewayDto destination, ResolutionContext context)
            {
                var dtoObject = new Models.TemporaryExposureKeyGatewayDto()
                {
                    KeyData = source.KeyData.ToByteArray(),
                    RollingStartIntervalNumber = source.RollingStartIntervalNumber,
                    RollingPeriod = source.RollingPeriod,
                    TransmissionRiskLevel = source.TransmissionRiskLevel,
                    DaysSinceOnsetOfSymptoms = source.DaysSinceOnsetOfSymptoms,
                    ReportType = MapReportType(source.ReportType),
                    Origin = source.Origin
                };
                dtoObject.VisitedCountries = new List<string>(source.VisitedCountries);
                return dtoObject;
            }

            private string MapReportType(TemporaryExposureKeyGatewayDtoProto.Types.ReportType reportType)
            {
                switch (reportType)
                {
                    case TemporaryExposureKeyGatewayDtoProto.Types.ReportType.ConfirmedTest:
                        return "CONFIRMED_TEST";
                    case TemporaryExposureKeyGatewayDtoProto.Types.ReportType.ConfirmedClinicalDiagnosis:
                        return "CONFIRMED_CLINICAL_DIAGNOSIS";
                    case TemporaryExposureKeyGatewayDtoProto.Types.ReportType.SelfReport:
                        return "SELF_REPORT";
                    case TemporaryExposureKeyGatewayDtoProto.Types.ReportType.Recursive:
                        return "RECURSIVE";
                    case TemporaryExposureKeyGatewayDtoProto.Types.ReportType.Revoked:
                        return "REVOKED";
                    default:
                        return "UNKNOWN";
                }
            }
        }
    }
}
