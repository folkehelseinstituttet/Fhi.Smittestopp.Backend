using System;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IPackageBuilderService
    {
        CacheResult BuildPackage(DateTime packageDate);
        List<byte[]> BuildPackageContentV2(DateTime startDate, ZipFileOrigin origin);
    }
}