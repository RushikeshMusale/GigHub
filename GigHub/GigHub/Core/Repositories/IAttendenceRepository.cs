using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IAttendenceRepository
    {
        Attendence GetAttendence(string userId, int gigId);
        IEnumerable<Attendence> GetFutureAtendnences(string userId);

        void Add(Attendence attendence);

        void Remove(Attendence attendence);
    }
}