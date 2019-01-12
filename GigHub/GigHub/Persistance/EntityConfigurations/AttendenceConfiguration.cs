using GigHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace GigHub.Persistance.EntityConfigurations
{
    public class AttendenceConfiguration : EntityTypeConfiguration<Attendence>
    {
        public AttendenceConfiguration()
        {
            //I think this will automatically set orders of column as sequence in new{}
            HasKey(a => new { a.GigId, a.AttendeeId });            
        }
    }
}