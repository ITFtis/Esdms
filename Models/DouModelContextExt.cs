using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    public class EsdmsModelContextExt : Dou.Models.ModelContextBase<User, Role>
    {
        public EsdmsModelContextExt() : base("name=EsdmsModelContextExt")
        {
            Database.SetInitializer<EsdmsModelContextExt>(null);
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Town> Town { get; set; }
        public virtual DbSet<Category> Category { get; set; }        
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<License> License { get; set; }
        public virtual DbSet<ActivityCategory> ActivityCategory { get; set; }        
        public virtual DbSet<ProjectUnit> ProjectUnit { get; set; }
        public virtual DbSet<Project> Project { get; set; }        
        public virtual DbSet<BasicUser> BasicUser { get; set; }       
        //與基本資料合併
        //public virtual DbSet<BasicUser_Private> BasicUser_Private { get; set; }
        public virtual DbSet<Expertise> Expertise { get; set; }       
        public virtual DbSet<UserHistoryOpinion> UserHistoryOpinion { get; set; }        
        public virtual DbSet<Resume> Resume { get; set; }        
        public virtual DbSet<FTISUserHistory> FTISUserHistory { get; set; }
        public virtual DbSet<BasicUser_License> BasicUser_License { get; set; }        
        public virtual DbSet<SubjectDetail> SubjectDetail { get; set; }
    }

}