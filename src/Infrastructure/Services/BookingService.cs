﻿using Application.DTOs.BookingDTO;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Pagination;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(AddBookingDTO bookingDTO)
        {
            try
            {
                Booking booking = _mapper.Map<Booking>(bookingDTO);
                await _unitOfWork.Bookings.AddAsync(booking);
                await _unitOfWork.CompleteAsync();
                return booking.Id;
            }
            catch (Exception ex)
            {
                _logger.LogWarn($"Non correct values in the {nameof(AddAsync)} action {ex}");
                return Guid.Empty;
            }
        }

        public async Task<PagedList<GetBookingDTO>> GetPagedAsync(PagedQueryBase query)
        {
            try
            {
                var bookings = await _unitOfWork.Bookings.GetPagedAsync(query);
                var mapBookings = _mapper.Map<List<GetBookingDTO>>(bookings);
                var bookingsDTO = new PagedList<GetBookingDTO>(mapBookings, bookings.TotalCount, bookings.CurrentPage, bookings.PageSize);
                return bookingsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogWarn($"Non correct values in the {nameof(GetPagedAsync)} action {ex}");
                return null;
            }
        }

        public async Task<GetBookingDTO> GetByIdAsync(Guid Id)
        {
            try
            {
                var booking = await _unitOfWork.Bookings.GetByIdAsync(Id);
                return _mapper.Map<GetBookingDTO>(booking);
            }
            catch (Exception ex)
            {
                _logger.LogWarn($"Non correct values in the {nameof(GetByIdAsync)} action {ex}");
                return null;
            }
        }

        public async Task<bool> RemoveAsync(Guid Id)
        {
            try
            {
                var booking = await _unitOfWork.Bookings.GetByIdAsync(Id);
                if (booking == null)
                    return false;

                _unitOfWork.Bookings.Remove(booking);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarn($"Non correct values in the {nameof(RemoveAsync)} action {ex}");
                return false;
            }
        }

        public async Task<PagedList<GetBookingDTO>> SearchByUserIdAsync(Guid userId, PagedQueryBase query)
        {
            try
            {
                var bookings = await _unitOfWork.Bookings.GetPagedByUserIdAsync(userId, query);
                var bookingMaps = _mapper.Map<List<GetBookingDTO>>(bookings);
                var bookingsDTO = new PagedList<GetBookingDTO>(bookingMaps, bookings.TotalCount, bookings.CurrentPage, bookings.PageSize);
                return bookingsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogWarn($"Non correct values in the {nameof(SearchByUserIdAsync)} action {ex}");
                return null;
            }
        }

        public async Task<PagedList<GetBookingDTO>> SearchByWorkPlaceIdAsync(Guid workPlaceId, PagedQueryBase query)
        {
            try
            {
                var bookings = await _unitOfWork.Bookings.GetPagedByWorkPlaceIdAsync(workPlaceId, query);
                var bookingMaps = _mapper.Map<List<GetBookingDTO>>(bookings);
                var bookingsDTO = new PagedList<GetBookingDTO>(bookingMaps, bookings.TotalCount, bookings.CurrentPage, bookings.PageSize);
                return bookingsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogWarn($"Non correct values in the {nameof(SearchByWorkPlaceIdAsync)} action {ex}");
                return null;
            }
        }

        public async Task<GetBookingDTO> UpdateAsync(UpdateBookingDTO bookingDTO)
        {
            try
            {
                var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingDTO.Id);
                if (booking == null)
                    return null;

                _mapper.Map(bookingDTO, booking);
                _unitOfWork.Bookings.Update(booking);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<GetBookingDTO>(booking);
            }
            catch (Exception ex)
            {
                _logger.LogWarn($"Non correct values in the {nameof(UpdateAsync)} action {ex}");
                return null;
            }
        }
    }
}
