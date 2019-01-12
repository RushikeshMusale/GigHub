using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class GigRepository : IGigRepository
    {
        IApplicationDbContext _context;

        public GigRepository(IApplicationDbContext context)
        {
            _context = context;
        }   

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _context.Attendences
                            .Where(a => a.AttendeeId == userId & a.Gig.Datetime > DateTime.Now) 
                            .Select(g => g.Gig)
                            .Include(g => g.Artist)
                            .Include(g => g.Genre)
                            .ToList();
        }

        public Gig GetGig(int gigId)
        {
            //Trick here is to eager load Artist & Genre 
            //In this application, it's more likely to need both properties
            //Think like Software Engineer
            return _context.Gigs
                        .Include(g=>g.Artist)
                        .Include(g=>g.Genre)
                        .SingleOrDefault(g => g.Id == gigId);
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Attendences.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }

        public Gig GetGigWithArtists(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetUpcomingGigsByArtist(string artistId)
        {
            return _context.Gigs
                 .Where(g => g.ArtistId == artistId &&
                 g.Datetime > DateTime.Now &&
                 !g.IsCanceled)
                 .Include(g => g.Genre)
                 .ToList();
        }
       
        public IEnumerable<Gig> GetUpcomingGigsByQuery(string query)
        {
            var upcomingGigs = _context.Gigs
                 .Include(g => g.Artist)
                 .Include(g => g.Genre)
                 .Where(g => g.Datetime > DateTime.Now && !g.IsCanceled);

            if (!string.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs
                    .Where(g =>
                                g.Artist.Name.Contains(query) ||
                                g.Genre.Name.Contains(query) ||
                                g.Venue.Contains(query));
            }

            return upcomingGigs;
        }

        public void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }
    }
}