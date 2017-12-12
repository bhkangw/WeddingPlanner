using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    // the following classes are built specifically for insertion into the db. Models for validations are in ViewModels.cs
    public class Rsvp
    {
        [Key]
        public int RsvpId { get; set; }
        public int UserId { get; set; } // foreign key goes in the multiple side of a one to many
        public User User { get; set; } // User objects created along with the foreign key
        public int WeddingId { get; set; } // foreign key goes in the multiple side of a one to many
        public Wedding Wedding { get; set; } // Wedding objects created along with the foreign key
    }
}