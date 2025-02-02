﻿using Application.Extentions;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Pagination;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OfficeRepository : GenericRepository<Office>, IOfficeRepository
    {
        public OfficeRepository(ApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }

        public async Task<PagedList<Office>> GetPagedAsync(PagedQueryBase query, bool tracking = false)
        {
            try
            {
                return await GetAll(tracking)
                            .Sort(query.SortOn, query.SortDirection)
                            .ToPagedListAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetPagedAsync)} action {ex}");
                return new PagedList<Office>(new List<Office>(), 0, 0, 0);
            }
        }

        public async Task<Office> GetByAddressAsync(string address, PagedQueryBase query, bool tracking = false)
        {
            try
            {
                return await Search(x => x.Address == address,
                                    tracking)
                            .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetByAddressAsync)} action {ex}");
                return new Office();
            }
        }

        public async Task<PagedList<Office>> GetPagedByCityAsync(string city, PagedQueryBase query, bool tracking = false)
        {
            try
            {
                return await Search(x => x.City == city,
                                    tracking)
                            .Sort(query.SortOn, query.SortDirection)
                            .ToPagedListAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetPagedByCityAsync)} action {ex}");
                return new PagedList<Office>(new List<Office>(), 0, 0, 0);
            }
        }

        public async Task<PagedList<Office>> GetPagedByCountryAsync(string country, PagedQueryBase query, bool tracking = false)
        {
            try
            {
                return await Search(x => x.Country == country,
                                    tracking)
                            .Sort(query.SortOn, query.SortDirection)
                            .ToPagedListAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetPagedByCountryAsync)} action {ex}");
                return new PagedList<Office>(new List<Office>(), 0, 0, 0);
            }
        }

        public async Task<PagedList<string>> GetPagedCountriesAsync(PagedQueryBase query, bool tracking = false)
        {
            try
            {
                return await GetAll(tracking)
                            .Sort(query.SortOn, query.SortDirection)
                            .Select(x => x.Country)
                            .Distinct()
                            .ToPagedListAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetPagedCountriesAsync)} action {ex}");
                return new PagedList<string>(new List<string>(), 0, 0, 0);
            }
        }

        public async Task<PagedList<string>> GetPagedCitiesByCountryAsync(string country, PagedQueryBase query, bool tracking = false)
        {
            try
            {
                return await Search(x => x.Country == country,
                                    tracking)
                            .Sort(query.SortOn, query.SortDirection)
                            .Select(x => x.City)
                            .Distinct()
                            .ToPagedListAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetPagedCitiesByCountryAsync)} action {ex}");
                return new PagedList<string>(new List<string>(), 0, 0, 0);
            }
        }
    }
}
