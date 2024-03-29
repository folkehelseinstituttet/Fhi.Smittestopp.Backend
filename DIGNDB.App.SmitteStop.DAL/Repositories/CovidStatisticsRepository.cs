﻿using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain.Db;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public class CovidStatisticsRepository : GenericRepository<CovidStatistics>, ICovidStatisticsRepository
    {
        public CovidStatisticsRepository(DigNDB_SmittestopContext context) : base(context)
        {
        }

        public CovidStatistics GetEntryByDate(DateTime date)
        {
            return Get(filter: c => c.EntryDate.Date.CompareTo(date.Date) == 0).SingleOrDefault();
        }

        public async Task<CovidStatistics> GetEntryByDateAsync(DateTime date)
        {
            return (await GetAsync(filter: c => c.EntryDate.Date == date.Date)).SingleOrDefault();
        }

        public void CreateEntry(CovidStatistics entry)
        {
            Insert(entry);
            Save();
        }

        public async Task<CovidStatistics> GetNewestEntryAsync()
        {
            var newest = await GetAsync(orderBy: x => x.OrderByDescending(x => x.EntryDate));
            return newest?.FirstOrDefault();
        }

        public CovidStatistics GetNewestEntry()
        {
            var entries = Get(orderBy: x => x.OrderByDescending(x => x.EntryDate));
            return entries.FirstOrDefault();
        }
    }
}
