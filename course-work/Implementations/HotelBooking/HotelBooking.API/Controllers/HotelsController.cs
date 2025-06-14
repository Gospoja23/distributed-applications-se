using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Models;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HotelsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchHotels(
    string? name,
    string? address,
    string? sortBy = "Name",
    bool isDescending = false,
    int page = 1,
    int pageSize = 10
)
        {
            var query = _context.Hotels.AsQueryable();

            // Фильтрация
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(h => h.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(address))
                query = query.Where(h => h.Address.Contains(address));

            // Сортировка
            query = sortBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(h => h.Name) : query.OrderBy(h => h.Name),
                "address" => isDescending ? query.OrderByDescending(h => h.Address) : query.OrderBy(h => h.Address),
                "stars" => isDescending ? query.OrderByDescending(h => h.Stars) : query.OrderBy(h => h.Stars),
                _ => query.OrderBy(h => h.Id)
            };

            // Пагинация
            var totalItems = await query.CountAsync();
            var hotels = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalItems,
                page,
                pageSize,
                items = hotels
            });
        }



        // GET: api/Hotels
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            return await _context.Hotels.ToListAsync();
        }

        // GET: api/Hotels/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return hotel;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Stars = dto.Stars
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }


        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }
    }
}
