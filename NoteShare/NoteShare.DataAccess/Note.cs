//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoteShare.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Note
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsPrivate { get; set; }
        public string Title { get; set; }
        public string FileType { get; set; }
        public byte[] FileContents { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int CourseId { get; set; }
        public string Description { get; set; }
        public bool IsSuspended { get; set; }
    }
}
