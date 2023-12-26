using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    /// <summary>
    /// 專家基本資料
    /// </summary>
    [Table("BasicUser")]
    public class BasicUser
    {
        [Key]
        [Display(Name = "身分證字號")]
        [ColumnDef(ColSize = 3)]
        [StringLength(10)]
        public string PId { get; set; }
        
        [Display(Name = "姓名")]
        [ColumnDef(ColSize = 3)]
        [StringLength(40)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "性別")]
        [ColumnDef(EditType = EditType.Select, SelectItems = "{\"1\":\"男\",\"2\":\"女\"}",
            ColSize = 3)]        
        public int Sex { get; set; }

        [Display(Name = "人員類別")]
        [ColumnDef(Filter = true, 
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CategorySelectItems.AssemblyQualifiedName,
            ColSize = 3)]        
        public int CategoryId { get; set; }

        [Display(Name = "在職狀況")]
        [ColumnDef(EditType = EditType.Select, SelectItems = "{\"OJ1\":\"在職\",\"OJ2\":\"退休\",\"OJ3\":\"歿\"}",
            ColSize = 3)]
        [StringLength(10)]
        public string OnJob { get; set; }

        [Display(Name = "單位(全銜)")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string UnitName { get; set; }

        [Display(Name = "職稱")]
        [ColumnDef(ColSize = 3)]
        [StringLength(40)]
        public string Position { get; set; }
        
        [Display(Name = "辦公室電話")]
        [ColumnDef(ColSize = 3)]
        [StringLength(20)]
        public string OfficePhone { get; set; }

        [Display(Name = "傳真")]
        [ColumnDef(ColSize = 3)]
        [StringLength(20)]
        public string Fax { get; set; }

        [Display(Name = "(辦公)縣市")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CitySelectItems.AssemblyQualifiedName,
            SelectGearingWith = "ZIP,CityCode,true", ColSize = 3)]
        [StringLength(6)]
        public string CityCode { get; set; }        

        [Display(Name = "(辦公)鄉鎮市區")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.TownSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        [StringLength(5)]
        public string ZIP { get; set; }

        [Display(Name = "(辦公)地址")]
        [ColumnDef(ColSize = 3)]
        [StringLength(100)]
        public string OfficeAddress { get; set; }

        [Display(Name = "(辦公)Email")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string OfficeEmail { get; set; }

        [Display(Name = "(私人)Email")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string PrivateEmail { get; set; }
        
        [Display(Name = "證照")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.LicenseSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        public int LicenseId { get; set; }

        [Display(Name = "國籍")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string Nation { get; set; }

        [Display(Name = "擅長語言")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string Language { get; set; }

        [Display(Name = "建檔日期")]
        [ColumnDef(VisibleEdit = false, ColSize = 3)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(6)]
        public string BFno { get; set; }

        [Display(Name = "建檔人姓名")]
        [ColumnDef(VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string BName { get; set; }

        [Display(Name = "修改日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        public DateTime? UDate { get; set; }

        [Display(Name = "修改人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(6)]
        public string UFno { get; set; }

        [Display(Name = "修改人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string UName { get; set; }

        [Display(Name = "推薦人")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string Recommender { get; set; }

        [ColumnDef(Visible = false, VisibleEdit = false)]
        public virtual BasicUser_Private BasicUser_Private 
        { 
            get
            {
                return BasicUser_Private.GetAllDatas().Where(a => a.PId == this.PId).FirstOrDefault();
            }
        }

        [NotMapped]
        public virtual ICollection<Expertise> Expertises        
        {
            get
            {
                return Expertise.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        [NotMapped]
        public virtual ICollection<UserHistoryOpinion> UserHistoryOpinions
        {
            get
            {
                return UserHistoryOpinion.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        [NotMapped]
        public virtual ICollection<Resume> Resumes
        {
            get
            {
                return Resume.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        [NotMapped]
        public virtual ICollection<FTISUserHistory> FTISUserHistorys
        {
            get
            {
                return FTISUserHistory.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }
    }
}