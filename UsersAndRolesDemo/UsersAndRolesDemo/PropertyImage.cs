//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UsersAndRolesDemo
{
    using System;
    using System.Collections.Generic;
    
    public partial class PropertyImage
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public byte[] name { get; set; }
    
        public virtual Property Property { get; set; }
    }
}