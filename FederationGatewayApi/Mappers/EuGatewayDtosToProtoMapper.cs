using AutoMapper;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Google.Protobuf;
using System;
using TemporaryExposureKeyGatewayDtoProto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayDto;

namespace FederationGatewayApi.Mappers
{
    public class EuGatewayDtosToProtoMapper : Profile
    {
        public EuGatewayDtosToProtoMapper()
        {
            CreateMap<Models.TemporaryExposureKeyGatewayDto, TemporaryExposureKeyGatewayDtoProto>().ConvertUsing(new TemporaryExposureKeyGatewayDto2ProtConverter());
        }

        public class TemporaryExposureKeyGatewayDto2ProtConverter : ITypeConverter<Models.TemporaryExposureKeyGatewayDto, TemporaryExposureKeyGatewayDtoProto>
        {
            public TemporaryExposureKeyGatewayDtoProto Convert(Models.TemporaryExposureKeyGatewayDto source, TemporaryExposureKeyGatewayDtoProto destination, ResolutionContext context)
            {
                var protoObject = new TemporaryExposureKeyGatewayDtoProto()
                {
                    KeyData = ByteString.CopyFrom(source.KeyData),
                    RollingStartIntervalNumber = source.RollingStartIntervalNumber,
                    RollingPeriod = source.RollingPeriod,
                    TransmissionRiskLevel = source.TransmissionRiskLevel,
                    DaysSinceOnsetOfSymptoms = source.DaysSinceOnsetOfSymptoms,
                    ReportType = MapReportType(source.ReportType),
                    Origin = source.Origin
                };
                protoObject.VisitedCountries.AddRange(source.VisitedCountries);
                return protoObject;
            }

            private TemporaryExposureKeyGatewayDtoProto.Types.ReportType MapReportType(string reportType)
            {
                switch (reportType)
                {
                    case "CONFIRMED_TEST":
                        return TemporaryExposureKeyGatewayDtoProto.Types.ReportType.ConfirmedTest;
                    case "CONFIRMED_CLINICAL_DIAGNOSIS":
                        return TemporaryExposureKeyGatewayDtoProto.Types.ReportType.ConfirmedClinicalDiagnosis;
                    case "SELF_REPORT":
                        return TemporaryExposureKeyGatewayDtoProto.Types.ReportType.SelfReport;
                    case "RECURSIVE":
                        return TemporaryExposureKeyGatewayDtoProto.Types.ReportType.Recursive;
                    case "REVOKED":
                        return TemporaryExposureKeyGatewayDtoProto.Types.ReportType.Revoked;
                    default:
                        return TemporaryExposureKeyGatewayDtoProto.Types.ReportType.Unknown;
                }
            }

            
        }
    }
}
