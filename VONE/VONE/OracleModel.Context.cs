﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VONE
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ANSWER_TO_Q> ANSWER_TO_Q { get; set; }
        public virtual DbSet<COMPANY> COMPANY { get; set; }
        public virtual DbSet<DOWNLOAD_RESUME> DOWNLOAD_RESUME { get; set; }
        public virtual DbSet<FIELD> FIELD { get; set; }
        public virtual DbSet<POS_RELATED_FIELD> POS_RELATED_FIELD { get; set; }
        public virtual DbSet<POS_RELEASE> POS_RELEASE { get; set; }
        public virtual DbSet<PROMPT_Q> PROMPT_Q { get; set; }
        public virtual DbSet<Q_TO_FIELD> Q_TO_FIELD { get; set; }
        public virtual DbSet<QUESTION> QUESTION { get; set; }
        public virtual DbSet<RESUME> RESUME { get; set; }
        public virtual DbSet<SKILL> SKILL { get; set; }
        public virtual DbSet<SKILL_TO_STUDENT> SKILL_TO_STUDENT { get; set; }
        public virtual DbSet<STUDENT> STUDENT { get; set; }
        public virtual DbSet<WANTED_POS> WANTED_POS { get; set; }
    }
}