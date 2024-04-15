using System;
using Microsoft.VisualBasic;

namespace Announcements
{
    public class Announcement
    {
        public Guid Id {get; set;}
        public string? Title;
        public string? Body;
        public object? Image;
        public string? PostTime;
    }
}