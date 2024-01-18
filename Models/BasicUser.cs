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
        [Display(Name = "身分代碼")]
        [ColumnDef(ColSize = 3)]
        [StringLength(13)]
        public string PId { get; set; }
        
        [Display(Name = "姓名")]
        [Required]
        [ColumnDef(EditType = EditType.TextList, SelectItemsClassNamespace = Esdms.Models.BasicUserNameSelectItems.AssemblyQualifiedName, 
            Filter = true, FilterAssign = FilterAssignType.Contains,
            ColSize = 3)]
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
        public int? CategoryId { get; set; }

        [Display(Name = "在職狀況")]
        [ColumnDef(EditType = EditType.Select, SelectItems = "{\"OJ1\":\"在職\",\"OJ2\":\"退休\"}",
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

        [Display(Name = "手機號碼")]
        [ColumnDef(ColSize = 3)]
        [StringLength(30)]
        public string PrivatePhone { get; set; }

        [Display(Name = "傳真")]
        [ColumnDef(ColSize = 3)]
        [StringLength(20)]
        public string Fax { get; set; }

        [Display(Name = "(辦公)Email")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string OfficeEmail { get; set; }

        [Display(Name = "(私人)Email")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string PrivateEmail { get; set; }

        [Display(Name = "(辦公)縣市")]
        [ColumnDef(EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CitySelectItems.AssemblyQualifiedName,
            SelectGearingWith = "ZIP,CityCode,true", ColSize = 3)]
        [StringLength(6)]
        public string CityCode { get; set; }        

        [Display(Name = "(辦公)鄉鎮市區")]
        [ColumnDef(EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.TownSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        [StringLength(5)]
        public string ZIP { get; set; }

        [Display(Name = "(辦公)地址")]
        [ColumnDef(ColSize = 3)]
        [StringLength(100)]
        public string OfficeAddress { get; set; }

        [Display(Name = "(住家)縣市")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CitySelectItems.AssemblyQualifiedName,
            SelectGearingWith = "PZIP,PCityCode,true", ColSize = 3)]
        [StringLength(6)]
        public string PCityCode { get; set; }

        [Display(Name = "(住家)鄉鎮市區")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.TownSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        [StringLength(5)]
        public string PZIP { get; set; }

        [Display(Name = "(住家)地址")]
        [ColumnDef(ColSize = 3)]
        [StringLength(100)]
        public string PAddress { get; set; }

        [Display(Name = "備註")]
        [ColumnDef(Visible = false, ColSize = 3)]
        [StringLength(1000)]
        public string Note { get; set; }

        [Display(Name = "國籍")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string Nation { get; set; }

        [Display(Name = "擅長語言")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
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
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string Recommender { get; set; }
        
        //專長
        [NotMapped]
        public virtual ICollection<Expertise> Expertises        
        {
            get
            {
                return Expertise.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        //意見(隱藏)
        [NotMapped]
        public virtual ICollection<UserHistoryOpinion> UserHistoryOpinions
        {
            get
            {
                return UserHistoryOpinion.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        //經歷(隱藏)
        [NotMapped]
        public virtual ICollection<Resume> Resumes
        {
            get
            {
                return Resume.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        //專家參與紀錄
        [NotMapped]
        public virtual ICollection<FTISUserHistory> FTISUserHistorys
        {
            get
            {
                return FTISUserHistory.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        //證照(隱藏)
        [NotMapped]
        public virtual ICollection<BasicUser_License> BasicUser_Licenses
        {
            get
            {
                return BasicUser_License.GetAllDatas().Where(a => a.PId == this.PId).ToList();
            }
        }

        //虛擬欄位 SubjectId
        [Display(Name = "專長類別")]
        [ColumnDef(VisibleEdit = false, EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.SubjectSelectItems.AssemblyQualifiedName,
            SelectGearingWith = "SubjectDetailId,SubjectId,true", ColSize = 3)]
        public int? SubjectId { get; }

        //虛擬欄位 SubjectDetailId
        [Display(Name = "專長領域")]
        [ColumnDef(VisibleEdit = false, EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.SubjectDetailSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        public int? SubjectDetailId { get; }

        //虛擬欄位 strExpertises
        [Display(Name = "專長")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string strExpertises
        {
            get
            {
                var vs = Expertise.GetAllDatas()
                        .Where(a => a.PId == this.PId)
                        .Select(a => new { a.SubjectId, a.SubjectDetailId });

                var G1 = vs.Join(Subject.GetAllDatas(), a => a.SubjectId, b => b.Id, (o, c) => new
                {
                    SubjectId = c.Id,
                    Name1 = c.Name
                }).Distinct();

                var G2 = vs.Join(SubjectDetail.GetAllDatas(), a => a.SubjectDetailId, b => b.Id, (o, c) => new
                {
                    SubjectId = c.SubjectId,
                    SubjectDetailId = c.Id,
                    Name2 = c.Name,
                    Sort2 = c.Sort
                });

                var tmp = G1.GroupJoin(G2, a => a.SubjectId, b => b.SubjectId, (o, c) => new
                {
                    o.SubjectId,
                    str = o.Name1 + string.Format("：{0}", string.Join(", ", c.OrderBy(a => a.Sort2).Select(a => a.Name2)))
                });

                return string.Join("</br>", tmp.Select(a => a.str));
            }
        }

        //虛擬欄位 DuplicateName
        [Display(Name = "重覆姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false,
                Filter = true, EditType = EditType.Select, SelectItems = "{\"Y\":\"是\",\"N\":\"否\"}")]
        public string DuplicateName { get; }
    }

    public class BasicUserNameSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.BasicUserNameSelectItems, Esdms";

        protected static IEnumerable<BasicUser> _basicUsers;
        internal static IEnumerable<BasicUser> BasicUsers
        {
            get
            {
                if (_basicUsers == null)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _basicUsers = db.BasicUser.ToArray();
                    }
                }
                return _basicUsers;
            }
        }


        public static void Reset()
        {
            _basicUsers = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return BasicUsers.Select(s => new KeyValuePair<string, object>(s.Name, s.Name));
        }
    }
}