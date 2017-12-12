using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    // the following classes are built specifically for insertion into the db. Models for validations are in ViewModels.cs
    public class Wedding : BaseEntity
    {
        [Key]
        public int WeddingId { get; set; }
        public int CreatedBy { get; set; }
        public string WedderOne { get; set; }
        public string WedderTwo { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        // a wedding can have many rsvps therefore:
        public List<Rsvp> Rsvp { get; set; } // list to expect multiple rsvp objects
        // must create an empty list in the single side of a one to many
        public Wedding()
        {
            Rsvp = new List<Rsvp>(); // new empty list of rsvps
        }
    }
}