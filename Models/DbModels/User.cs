using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    // the following classes are built specifically for insertion into the db. Models for validations are in ViewModels.cs
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // a user can have many rsvps therefore:
        public List<Rsvp> Rsvp { get; set; } // list to expect multiple rsvp objects
        // must create an empty list in the single side of a one to many
        public User()
        {
            Rsvp = new List<Rsvp>(); // new empty list of rsvp
        }
    }
}