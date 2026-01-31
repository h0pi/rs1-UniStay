using UniStay.API.Data;
using UniStay.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace UniStay.API.Services
{
    public class HallsService
    {
        private readonly ApplicationDbContext _context;

        public HallsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Halls>> GetAllAsync()
        {
            return await _context.Hall.ToListAsync();
        }

        public async Task<Halls?> GetByIdAsync(int id)
        {
            return await _context.Hall.FindAsync(id);
        }

        public async Task<Halls> CreateAsync(Halls hall)
        {
            _context.Hall.Add(hall);
            await _context.SaveChangesAsync();
            return hall;
        }

        public async Task<Halls?> UpdateAsync(Halls hall)
        {
            var existing = await _context.Hall.FindAsync(hall.HallID);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(hall);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hall = await _context.Hall.FindAsync(id);
            if (hall == null) return false;

            _context.Hall.Remove(hall);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}