using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class AttendenceRepository : IAttendenceRepository
    {
        ApplicationDbContext _context;
        public AttendenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Attendence> GetFutureAtendnences(string userId)
        {
            return _context.Attendences
                           .Where(a => a.AttendeeId == userId && a.Gig.Datetime > DateTime.Now)
                           .ToList();
        }

        //public bool IsUserAttendingGig(string userId, int gigId)
        //{
        //    return _context.Attendences
        //                .Any(a => a.AttendeeId == userId && a.GigId == gigId);
        //}
        
        //Instead of creating IsUserAttendingGig create a method GetAttendence
        //it will be more usable
        public Attendence GetAttendence(string userId, int gigId)
        {
            return _context.Attendences
                        .SingleOrDefault(a => a.AttendeeId == userId && a.GigId == gigId);
        }

        public void Add(Attendence attendence)
        {
            _context.Attendences.Add(attendence);
        }

        public void Remove(Attendence attendence)
        {
            _context.Attendences.Remove(attendence);
        }
    }
}