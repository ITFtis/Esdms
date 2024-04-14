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
        
        [Display(Name = "性別")]
        [ColumnDef(EditType = EditType.Select, SelectItemsClassNamespace = Esdms.GetSexSelectItems.AssemblyQualifiedName,
            ColSize = 3)]        
        public int? Sex { get; set; }

        [Display(Name = "人員類別")]
        [ColumnDef(
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CategorySelectItems.AssemblyQualifiedName,
            ColSize = 3)]        
        public int? CategoryId { get; set; }

        [Display(Name = "在職狀況")]
        [ColumnDef(EditType = EditType.Select,
            SelectItemsClassNamespace = Esdms.GetOnJobSelectItems.AssemblyQualifiedName,
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

        [Display(Name = "手機號碼")]
        [ColumnDef(ColSize = 3)]
        [StringLength(30)]
        public string PrivatePhone { get; set; }

        [Display(Name = "辦公室電話")]
        [ColumnDef(ColSize = 3)]
        [StringLength(40)]
        public string OfficePhone { get; set; }

        [Display(Name = "辦公室電話2")]
        [ColumnDef(ColSize = 3)]
        [StringLength(40)]
        public string OfficePhone2 { get; set; }        

        [Display(Name = "傳真")]
        [ColumnDef(ColSize = 3)]
        [StringLength(40)]
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
        [ColumnDef(
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CitySelectItems.AssemblyQualifiedName,
            SelectGearingWith = "PZIP,PCityCode,true", ColSize = 3)]
        [StringLength(6)]
        public string PCityCode { get; set; }

        [Display(Name = "(住家)鄉鎮市區")]
        [ColumnDef(
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
                    str = o.Name1 + string.Format("：{0}", string.Join("<span class='text-primary'>； </span> ", c.OrderBy(a => a.Sort2).Select(a => a.Name2)))
                });

                return string.Join("</br>", tmp.Select(a => a.str));
            }
        }

        //虛擬欄位 vmOutCount
        [Display(Name = "會外評選")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string vmOutCount
        {
            get
            {
                //近3年資料
                int n = 3;
                int sYear = DateTime.Now.Year - 1911 - n;

                var acts = ActivityCategorySelectItems.ActivityCategorys.Where(a => a.Type == 2);
                var query = FTISUserHistory.GetAllDatas().Where(a => a.PId == this.PId && a.ActivityCategoryType == 2)
                            .Where(a => a.OutYear >= sYear)
                            .GroupJoin(acts, a => a.ActivityCategoryId, b => b.Id, (o, c) => new
                            {
                                o.Id,
                                o.OutYear,                                
                                ActName = c.FirstOrDefault() == null ? "" : c.FirstOrDefault().Name,
                                ActId = c.FirstOrDefault() == null ? int.MaxValue : c.FirstOrDefault().Id,
                            })
                            .GroupJoin(UserHistorySet.GetAllDatas(), a => a.Id, b => b.FtisUHId, (o, c) => new
                            {
                                o.Id, o.OutYear, o.ActName, o.ActId, c
                            })
                            .Where(a => a.c.Count() != 0)  //有會外會議組別資料
                            .SelectMany(b => b.c.DefaultIfEmpty(), (o, c) => new
                            {
                                o.OutYear, o.ActName, o.ActId,
                                BidCount = c == null ? 0 : c.BidCount,
                                SetName = c == null ? "" : c.Name,
                                bidNum = c == null ? 0 : c.UserHistorySetBids.Count()
                            });

                var datas = query
                            .GroupBy(a => new { OutYear = a.OutYear, ActName = a.ActName })
                            .Select(a => new
                            {
                                OutYear = a.Key.OutYear,
                                setCount = a.Sum(p => p.bidNum),
                                setTitle = a.Key.ActName + "(" + a.Sum(p => p.bidNum).ToString() + ")",
                                setDesc = string.Join(",", query.Where(p => p.OutYear == a.Key.OutYear && p.ActName == a.Key.ActName)
                                                            .Select(p => p.SetName + "(" + p.BidCount.ToString() + ")"))
                            });

                var datasGroup = datas.Select(a => a.OutYear).Distinct().ToList();

                var tmp = datasGroup.GroupJoin(datas, a => a, b => b.OutYear, (o, c) => new
                {
                    str = string.Format(@"{0}年：(共<span class='text-primary'>{1}</span>次)</br>{2}", 
                                          o, 
                                          c.Sum(a=>a.setCount),
                                          string.Join("</br>", c.Select(p => p.setTitle + "：" + p.setDesc))
                                       )
                });

                return string.Join("</br>", tmp.Select(a => a.str));
            }
        }

        //虛擬欄位 vmTotalOutCount
        [Display(Name = "會外評選(總)次數")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int vmTotalOutCount
        {
            get
            {
                //近3年資料
                int n = 3;
                int sYear = DateTime.Now.Year - 1911 - n;
                
                var query = FTISUserHistory.GetAllDatas().Where(a => a.PId == this.PId && a.ActivityCategoryType == 2)
                            .Where(a => a.OutYear >= sYear)
                            .GroupJoin(UserHistorySet.GetAllDatas(), a => a.Id, b => b.FtisUHId, (o, c) => new
                            {
                                o.Id, SetCount = c.Sum(p=>p.BidCount)
                            });

                return query.Sum(a => a.SetCount);
            }
        }

        //虛擬欄位 vmInCount
        [Display(Name = "會內參與")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string vmInCount
        {
            get
            {
                //近3年資料
                int n = 3;
                int sYear = DateTime.Now.Year - 1911 - n;
                var acts = ActivityCategorySelectItems.ActivityCategorys.Where(a => a.Type == 1);
                var datas = FTISUserHistory.GetAllDatas().Where(a => a.PId == this.PId && a.ActivityCategoryType == 1)
                            .Where(a => a.Year >= sYear)
                            .GroupJoin(acts, a => a.ActivityCategoryId, b => b.Id, (o, c) => new
                            {
                                o.Id, o.Year, o.DCode, o.ProjectId,
                                ActName = c.FirstOrDefault() == null ? "" : c.FirstOrDefault().Name,
                                ActId = c.FirstOrDefault() == null ? int.MaxValue : c.FirstOrDefault().Id,
                            })
                            .GroupJoin(Code.GetDepartment(), a => a.DCode, b => b.Key, (o, c) => new
                            { 
                                 o.Id, o.Year, o.DCode, o.ProjectId, o.ActName, o.ActId,
                                 DName = c == null ? "" : c.FirstOrDefault().Value
                            })
                            .GroupJoin(ProjectSelectItems.Projects, a => a.ProjectId, b => b.Id, (o, c) => new
                            { 
                                 o.Id, o.Year, o.DCode, o.ProjectId, o.ActName, o.ActId, o.DName,
                                 pjName = c == null ? "" : c.FirstOrDefault().Name
                            });

                ////var tt = query.ToList();

                var datasGroup = datas.Select(a => a.Year).Distinct().ToList();

                var tmp = datasGroup.GroupJoin(datas, a => a, b => b.Year, (o, c) => new
                {
                    str = string.Format(@"{0}年：(共<span class='text-primary'>{1}</span>次)</br>{2}",
                                          o,
                                          c.Count(),
                                          string.Join("</br>", c.Select((p, index) => (index + 1).ToString() + "." + p.DName + "：" + p.pjName + "(" + p.ActName + ")"))
                                       )
                });

                return string.Join("</br>", tmp.Select(a => a.str));
            }
        }

        //虛擬欄位 vmTotalInCount
        [Display(Name = "會內參與(總)次數")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int vmTotalInCount
        {
            get
            {
                //近3年資料
                int n = 3;
                int sYear = DateTime.Now.Year - 1911 - n;

                var query = FTISUserHistory.GetAllDatas().Where(a => a.PId == this.PId && a.ActivityCategoryType == 1)
                            .Where(a => a.Year >= sYear);                

                return query.Count();
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