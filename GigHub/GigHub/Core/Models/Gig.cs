using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GigHub.Core.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public ApplicationUser Artist { get; set; }
        
        public string ArtistId { get; set; }

        public DateTime Datetime { get; set; }
       
        public string Venue { get; set; }
        
        public byte GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<Attendence> Attendences  { get; private set; }
        
        //For testing purpose making in public
        //protected Gig()
        public Gig()
        {
            Attendences = new Collection<Attendence>();
        }

        public Gig(ApplicationUser artist,  DateTime dateTime, String venue, byte genreId)
        {
            if (artist.Followers == null)
                throw new ArgumentNullException("artist.followers");

            Artist = artist;
            Datetime = dateTime;
            Venue = venue;
            GenreId = genreId;

            Notification notification = Notification.GigCreated(this);
            foreach (var follower in artist.Followers.Select(f=>f.Follower))
            {
                follower.Notify(notification);
            }

        }

        public void Cancel()
        {
            IsCanceled = true;

            Notification notification = Notification.GigCancel(this);

            foreach (var attendee in Attendences.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        public void Update(string venue, byte genre, DateTime dateTime)
        {
            Notification notification = Notification.GigUpdated(this,Datetime,Venue) ;
            //notification.OriginalDateTime = Datetime;
            //notification.OriginalVenue = Venue;

            //now update values
            Venue = venue;
            GenreId = genre;
            Datetime = dateTime;

            //Notify attendees
            foreach (var attendee in Attendences.Select(a=>a.Attendee))
            {
                attendee.Notify(notification);                
            }
        }
    }

}