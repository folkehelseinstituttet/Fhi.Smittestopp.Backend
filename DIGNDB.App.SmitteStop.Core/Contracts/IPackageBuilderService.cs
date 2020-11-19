using DIGNDB.App.SmitteStop.Domain.Dto;
using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IPackageBuilderService
    {
        CacheResult BuildPackage(DateTime packageDate);
        List<byte[]> BuildPackageContentV2(DateTime startDate, string originPostfix);
    }
}