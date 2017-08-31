using System;
using System.Collections.Generic;

namespace ModNotes.DbModels
{
    public class Note : IEquatable<Note>
    {
        public int Id { get; set; }

        public string Title { get; set; }
 
        public string Content { get; set; }

        public bool Equals(Note other)
        {
            return Id.Equals(other.Id) && Title.Equals(other.Title) && Content.Equals(other.Content);
        }
    }  	
}