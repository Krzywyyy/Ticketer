using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketer.Database.Context;
using Ticketer.Database.Domain;

namespace Ticketer.Database.Repositories
{
    class Repository
    {
        private DatabaseContext _context;

        public Repository()
        {
            _context = new DatabaseContext();
        }

        public async Task SaveReservation(Reservation reservation)
        {
            await _context.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public Spectacle FindSpectacleByName(string name)
        {
            return _context.Spectacles.Where(x => x.Name.Equals(name)).FirstOrDefault();
        }

        public List<string> GetSpectacles()
        {
            List<Spectacle> spectacles = _context.Set<Spectacle>().ToList();
            List<string> namesOfSpectacles = new List<string>();
            
            foreach(Spectacle spectacle in spectacles)
            {
                namesOfSpectacles.Add(spectacle.Name);
            }
            return namesOfSpectacles;
        }
    }
}
