using codeTongCucThienTai.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace codeTongCucThienTai.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TongCucThienTaiEntities _db;
        private readonly ApplicationDbContext _userConText;
        private ApplicationUserManager _userManager;
        private TongCucThienTaiServices _services;

        public HomeController()
        {
            _db = new TongCucThienTaiEntities();
            _userConText = new ApplicationDbContext();
            _services = new TongCucThienTaiServices();
        }

        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("KeHoachNhiemVu3");
            }
            else
            {
                return RedirectToAction("QuanLyNhiemVu3");
            }
            //return View();
        }

        public ActionResult KeHoachNhiemVu(string SearchKey, string SearchKeyNC, int? NguonVon, int? DonViDeXuat, int? DonViDeXuatNC, int? DonViDuToan, int? NamKeHoach, int? PageIndex)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai == 0);

            ViewBag.DonViDuToan = _services.DropDownListDanhMucDonVi(DonViDuToan);
            ViewBag.NguonVon = _services.DropDownListDanhMucNguonVon(new List<int>());
            ViewBag.NamKeHoach = _services.DropDownListNamKeHoach(NamKeHoach);
            ViewBag.SearchKey = SearchKey;
            ViewBag.SearchKeyNC = SearchKeyNC;
            if (!string.IsNullOrEmpty(SearchKeyNC))
            {
                SearchKey = SearchKeyNC;
            }

            if (DonViDeXuatNC.HasValue && DonViDeXuatNC > 0)
            {
                DonViDeXuat = DonViDeXuatNC;
            }

            ViewBag.SearchKey = SearchKey;
            ViewBag.SearchKeyNC = SearchKeyNC;
            ViewBag.DonViDeXuat = _services.DropDownListDanhMucDonVi(DonViDeXuat);
            ViewBag.DonViDeXuatNC = _services.DropDownListDanhMucDonVi(DonViDeXuatNC);
            decimal _TotalDeXuat = 0;
            decimal _TotalDeXuat2 = 0;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }
            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                model = model.Where(x => x.donViDuToanId == DonViDuToan);
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach);
            }

            int totalRecord = model.Count();
            model = model.OrderByDescending(x => x.createTime);
            _TotalDeXuat = model.Select(x => x.giaTriDeXuat).DefaultIfEmpty(0).Sum();
            _TotalDeXuat2 = model.Select(x => x.giaTriDeXuat2).DefaultIfEmpty(0).Sum();
            int totalPage = 0;
            int _pageSize = 20;
            if (PageIndex != null)
            {
                model = model.Skip((PageIndex.Value - 1) * _pageSize);
            }
            totalPage = (int)Math.Ceiling(1.0 * totalRecord / _pageSize);
            model = model.Take(_pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = PageIndex ?? 1;
            ViewBag.TotalDeXuat = _TotalDeXuat.ToString("N0");
            ViewBag.TotalDeXuat2 = _TotalDeXuat2.ToString("N0");
            return View(model.ToList());
        }

        public ActionResult KeHoachNhiemVu2(string SearchKey, string SearchKeyNC, int? NguonVon, int? DonViDeXuat, int? DonViDeXuatNC, int? DonViDuToan, int? NamKeHoach, int? PageIndex)
        {
            IEnumerable<DanhMucNhiemVu> _DanhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == null);
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai == 0);
            ViewBag.DonViDuToan = _services.DropDownListDanhMucDonVi(DonViDuToan);
            ViewBag.NguonVon = _services.DropDownListDanhMucNguonVon(new List<int>());
            ViewBag.NamKeHoach = _services.DropDownListNamKeHoach(NamKeHoach);
            ViewBag.SearchKey = SearchKey;
            ViewBag.SearchKeyNC = SearchKeyNC;
            if (!string.IsNullOrEmpty(SearchKeyNC))
            {
                SearchKey = SearchKeyNC;
            }

            if (DonViDeXuatNC.HasValue && DonViDeXuatNC > 0)
            {
                DonViDeXuat = DonViDeXuatNC;
            }

            ViewBag.SearchKey = SearchKey;
            ViewBag.SearchKeyNC = SearchKeyNC;
            ViewBag.DonViDeXuat = _services.DropDownListDanhMucDonVi(DonViDeXuat);
            ViewBag.DonViDeXuatNC = _services.DropDownListDanhMucDonVi(DonViDeXuatNC);
            decimal _TotalDeXuat = 0;
            decimal _TotalDeXuat2 = 0;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()) && nv.trangThai == 0
                            select dm);
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            join nvon in _db.NguonVonNhiemVus on nv.nhiemVuId equals nvon.nhiemVuId
                            where nvon.nguonVonId == NguonVon.Value && nv.trangThai == 0
                            select dm);
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q).Distinct();
            }
            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.donViDeXuatId == DonViDeXuat && nv.trangThai == 0
                            select dm);

                model = model.Where(x => x.donViDeXuatId == DonViDeXuat);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.donViDuToanId == DonViDuToan && nv.trangThai == 0
                            select dm);
                model = model.Where(x => x.donViDuToanId == DonViDuToan);
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.namKeHoach == NamKeHoach && nv.trangThai == 0
                            select dm);
                model = model.Where(x => x.namKeHoach == NamKeHoach);
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            _DanhMuc = _DanhMuc.DistinctBy(x => x.danhMucNhiemVuId);
            _DanhMuc = _DanhMuc.OrderByDescending(p => p.danhMucNhiemVuId);
            foreach (NhiemVu item in model)
            {
                _TotalDeXuat += item.giaTriDeXuat;
                _TotalDeXuat2 += item.giaTriDeXuat2;
            }
            //_TotalDeXuat = model.Select(x => x.giaTriDeXuat).DefaultIfEmpty(0).Sum();
            //_TotalDeXuat2 = model.Select(x => x.giaTriDeXuat2).DefaultIfEmpty(0).Sum();
            ViewBag.TotalDeXuat = _TotalDeXuat.ToString("N0");
            ViewBag.TotalDeXuat2 = _TotalDeXuat2.ToString("N0");
            return View(_DanhMuc);
        }

        public ActionResult KeHoachNhiemVu3(string SearchKey, string SearchKeyNC, int? NguonVon, int? DonViDeXuat, int? DonViDeXuatNC, int? DonViDuToan, int? NamKeHoach, int? PageIndex)
        {
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai == 0);
            ViewBag.DonViDuToan = _services.DropDownListDanhMucDonVi(DonViDuToan);
            ViewBag.NguonVon = _services.DropDownListDanhMucNguonVon(new List<int>());
            ViewBag.NamKeHoach = _services.DropDownListNamKeHoach(NamKeHoach);
            ViewBag.SearchKey = SearchKey;
            ViewBag.SearchKeyNC = SearchKeyNC;
            if (!string.IsNullOrEmpty(SearchKeyNC))
            {
                SearchKey = SearchKeyNC;
            }

            if (DonViDeXuatNC.HasValue && DonViDeXuatNC > 0)
            {
                DonViDeXuat = DonViDeXuatNC;
            }

            ViewBag.SearchKey = SearchKey;
            ViewBag.SearchKeyNC = SearchKeyNC;
            ViewBag.DonViDeXuat = _services.DropDownListDanhMucDonVi(DonViDeXuat);
            ViewBag.DonViDeXuatNC = _services.DropDownListDanhMucDonVi(DonViDeXuatNC);
            decimal _TotalDeXuat = 0;
            decimal _TotalDeXuat2 = 0;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }
            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }
            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                model = model.Where(x => x.donViDuToanId == DonViDuToan);
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach);
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            List<NhiemVu> _ListNV = model.ToList();
            List<NhiemVuView> _tem = new List<NhiemVuView>();
            foreach (NhiemVu item in _ListNV)
            {
                NhiemVuView _item = new NhiemVuView
                {
                    createTime = item.createTime,
                    danhMucId = item.danhMucId,
                    danhMucCha = item.danhMucId,
                    donViDeXuatId = item.donViDeXuatId,
                    donViDeXuatName = item.donViDeXuatName,
                    donViDuToanId = item.donViDuToanId,
                    donViDutoanName = item.donViDutoanName,
                    ghiChu = item.ghiChu,
                    giaTriDeXuat = item.giaTriDeXuat,
                    giaTriDeXuat2 = item.giaTriDeXuat2,
                    giaTriDuyet = item.giaTriDuyet,
                    giaTriGiao = item.giaTriGiao,
                    giaTriHopDong = item.giaTriHopDong,
                    giaTriQuyetToan = item.giaTriQuyetToan,
                    giaTriThanhToan = item.giaTriThanhToan,
                    giaTriTrungThau = item.giaTriTrungThau,
                    isTrash = item.isTrash,
                    Level = 0,
                    namKeHoach = item.namKeHoach,
                    nguonVonName = item.nguonVonName,
                    nhaThauTrung = item.nhaThauTrung,
                    nhiemVuId = item.nhiemVuId,
                    Code = item.nhiemVuId.ToString(),
                    nhiemVuName = item.nhiemVuName,
                    trangThai = item.trangThai,
                    updateTime = item.updateTime,
                    userCreate = item.userCreate,
                    userUpdate = item.userUpdate
                };

                if (_item.danhMucId > 0)
                {
                    _tem = GetCha(_item.danhMucId, _tem);
                }
                _TotalDeXuat += item.giaTriDeXuat;
                _TotalDeXuat2 += item.giaTriDeXuat2;
            }
            List<NhiemVuView> _tem2 = new List<NhiemVuView>();
            foreach (NhiemVuView itemTem in _tem)
            {
                _tem2.Add(itemTem);
                IOrderedEnumerable<NhiemVu> _listTem = _ListNV.OrderByDescending(x => x.createTime);
                foreach (NhiemVu nhiemVu in _listTem)
                {
                    if (nhiemVu.danhMucId == itemTem.danhMucId)
                    {
                        NhiemVuView _itemNV = new NhiemVuView
                        {
                            createTime = nhiemVu.createTime,
                            danhMucId = nhiemVu.danhMucId,
                            danhMucCha = nhiemVu.danhMucId,
                            donViDeXuatId = nhiemVu.donViDeXuatId,
                            donViDeXuatName = nhiemVu.donViDeXuatName,
                            donViDuToanId = nhiemVu.donViDuToanId,
                            donViDutoanName = nhiemVu.donViDutoanName,
                            ghiChu = nhiemVu.ghiChu,
                            giaTriDeXuat = nhiemVu.giaTriDeXuat,
                            giaTriDeXuat2 = nhiemVu.giaTriDeXuat2,
                            giaTriDuyet = nhiemVu.giaTriDuyet,
                            giaTriGiao = nhiemVu.giaTriGiao,
                            giaTriHopDong = nhiemVu.giaTriHopDong,
                            giaTriQuyetToan = nhiemVu.giaTriQuyetToan,
                            giaTriThanhToan = nhiemVu.giaTriThanhToan,
                            giaTriTrungThau = nhiemVu.giaTriTrungThau,
                            isTrash = nhiemVu.isTrash,
                            Level = itemTem.Level + 1,
                            namKeHoach = nhiemVu.namKeHoach,
                            nguonVonName = nhiemVu.nguonVonName,
                            nhaThauTrung = nhiemVu.nhaThauTrung,
                            nhiemVuId = nhiemVu.nhiemVuId,
                            Code = nhiemVu.nhiemVuId.ToString(),
                            nhiemVuName = nhiemVu.nhiemVuName,
                            trangThai = nhiemVu.trangThai,
                            updateTime = nhiemVu.updateTime,
                            userCreate = nhiemVu.userCreate,
                            userUpdate = nhiemVu.userUpdate
                        };
                        NhiemVuView checkExits = _tem2.FirstOrDefault(x => x.Code == _itemNV.Code);
                        if (checkExits == null)
                        {
                            _tem2.Add(_itemNV);
                        }
                    }
                }
            }
            List<NhiemVuView> _tem3 = new List<NhiemVuView>();
            List<NhiemVuView> _tem4 = _tem2;
            int count = 0;
            foreach (NhiemVuView itemTem in _tem4)
            {
                if (itemTem.Level == 1)
                {
                    count++;
                    itemTem.STT = _services.ChuyenSangSoLaMa(count);
                    _tem3.Add(itemTem);
                    _tem3 = AddCon(_tem3, itemTem.danhMucId, itemTem.STT, _tem4);
                }
            }
            ViewBag.TotalDeXuat = _TotalDeXuat.ToString("N0");
            ViewBag.TotalDeXuat2 = _TotalDeXuat2.ToString("N0");
            return View(_tem3);
        }

        public List<NhiemVuView> AddCon(List<NhiemVuView> _curent, int Id, string _STT, List<NhiemVuView> _data)
        {
            int count = 0;
            foreach (NhiemVuView item in _data)
            {
                if (item.nhiemVuId <= 0)
                {
                    if (item.danhMucCha == Id && Id > 0)
                    {
                        count++;
                        item.STT = _STT + "." + count;
                        _curent.Add(item);
                        AddCon(_curent, item.danhMucId, item.STT, _data);
                    }
                }
                else
                {
                    if (item.danhMucId == Id && Id > 0)
                    {
                        count++;
                        item.STT = _STT + "." + count;
                        _curent.Add(item);
                    }
                }
            }
            return _curent;
        }

        public List<NhiemVuView> GetCha(int? Id, List<NhiemVuView> _curent)
        {
            DanhMucNhiemVu dmNV = _db.DanhMucNhiemVus.Find(Id);
            if (dmNV != null)
            {
                NhiemVuView _item = new NhiemVuView
                {
                    danhMucId = dmNV.danhMucNhiemVuId,
                    Code = "DM" + dmNV.danhMucNhiemVuId,
                    Level = dmNV.capDanhMuc,
                    nhiemVuName = dmNV.danhMucNhiemVuName,
                    danhMucCha = dmNV.danhMucCha.GetValueOrDefault()
                };
                if (_item.danhMucCha > 0)
                {
                    GetCha(_item.danhMucCha, _curent);
                }
                NhiemVuView checkExits = _curent.FirstOrDefault(x => x.Code == _item.Code);
                if (checkExits == null)
                {
                    _curent.Add(_item);
                }
            }
            return _curent;
        }

        public ActionResult DanhMucCon(int Id, string Count, int Level)
        {
            ViewBag.Count = Count;
            ViewBag.Level = Level;
            IEnumerable<DanhMucNhiemVu> _DanhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == Id);
            _DanhMuc = _DanhMuc.OrderByDescending(p => p.danhMucNhiemVuId);
            return PartialView(_DanhMuc);
        }
        public ActionResult DanhMucCon2(int Id, int Count, int Level, string STT)
        {
            ViewBag.Count = Count;
            ViewBag.Level = Level;
            ViewBag.STT = STT;
            IEnumerable<DanhMucNhiemVu> _DanhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == Id);
            _DanhMuc = _DanhMuc.OrderByDescending(p => p.danhMucNhiemVuId);
            return PartialView(_DanhMuc);
        }
        public ActionResult NhiemVuThuocChuyenMuc(int Id, string Count)
        {
            ViewBag.Count = Count;
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.danhMucId == Id && x.trangThai == 0);
            model = model.OrderByDescending(x => x.namKeHoach);
            return PartialView(model);
        }
        public ActionResult NhiemVuThuocChuyenMuc2(int Id, int Count, string STT)
        {
            ViewBag.Count = Count;
            ViewBag.STT = STT;
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.danhMucId == Id && x.trangThai > 0);
            model = model.OrderByDescending(x => x.namKeHoach);
            return PartialView(model);
        }
        /// <summary>
        /// Thêm mới nhiệm vụ
        /// </summary>
        /// <returns></returns>
        public ActionResult ThemMoiNhiemVu()
        {
            NhiemVuModel model = new NhiemVuModel
            {
                createTime = DateTime.Now,
                updateTime = DateTime.Now,
                isTrash = false,
                userCreate = User.Identity.Name,
                userUpdate = User.Identity.Name,
                trangThai = 0,
                namKeHoach = DateTime.Now.Year
            };
            ViewBag.danhMucId = _services.DropDownListNhiemVu2(model.danhMucId, null).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            ViewBag.donViDeXuatId = _services.DropDownListDanhMucDonVi(model.donViDeXuatId).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            ViewBag.donViDuToanId = _services.DropDownListDanhMucDonVi(model.donViDuToanId).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            ViewBag.namKeHoach = _services.DropDownListNamKeHoach(model.namKeHoach).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            List<int> _listId = new List<int>();
            ViewBag.NguonVon = _services.DropDownListNguonVonMultile(_listId);
            ViewBag.nguonVonName = _services.DropDownListDanhMucNguonVon(_listId).Select(x => new SelectListItem { Text = x.Text, Value = x.Value, Selected = x.IsSelect });
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiNhiemVu(NhiemVuModel model)
        {
            int[] _nguonvon = model.nguonVonName2;
            StringBuilder _nguonvonName = new StringBuilder();
            foreach (int item in _nguonvon)
            {
                DanhMucNguonVon _enNguonVon = _db.DanhMucNguonVons.Find(item);
                if (_enNguonVon != null)
                {
                    _nguonvonName.Append(_enNguonVon.danhMucNguonVonName);
                    _nguonvonName.Append(",");
                }
            }
            if (!_services.CheckExitsNameNhiemVu(null, model.nhiemVuName, model.namKeHoach, model.donViDeXuatId, model.donViDuToanId, _nguonvonName.ToString().TrimEnd(',')))
            {
                decimal.TryParse(model.giaTriDeXuat, out decimal _gia);
                decimal.TryParse(model.giaTriDeXuat2, out decimal _gia2);
                NhiemVu entity = new NhiemVu
                {
                    createTime = DateTime.Now,
                    updateTime = DateTime.Now,
                    userCreate = model.userCreate,
                    userUpdate = model.userUpdate,
                    trangThai = 0,
                    nhiemVuName = model.nhiemVuName,
                    namKeHoach = model.namKeHoach,
                    isTrash = false,
                    giaTriDeXuat = _gia,
                    giaTriDeXuat2 = _gia2,
                    ghiChu = model.ghiChu,
                    danhMucId = model.danhMucId,
                    donViDeXuatId = model.donViDeXuatId,
                    donViDuToanId = model.donViDuToanId,
                    donViDeXuatName = _db.DanhMucDonVis.Find(model.donViDeXuatId).danhMucDonViName,
                    donViDutoanName = _db.DanhMucDonVis.Find(model.donViDuToanId).danhMucDonViName
                };
                entity.nguonVonName = _nguonvonName.ToString().TrimEnd(',');
                _db.NhiemVus.Add(entity);
                _db.SaveChanges();
                foreach (int item in _nguonvon)
                {
                    DanhMucNguonVon _enNguonVon = _db.DanhMucNguonVons.FirstOrDefault(x => x.danhMucNguonVonId == item);
                    if (_enNguonVon != null)
                    {
                        NguonVonNhiemVu entitys = new NguonVonNhiemVu
                        {
                            nguonVonId = _enNguonVon.danhMucNguonVonId,
                            nhiemVuId = entity.nhiemVuId
                        };
                        _db.NguonVonNhiemVus.Add(entitys);
                        _db.SaveChanges();
                    }
                }
                TempData["success"] = "Thêm mới thành công!";
                return RedirectToAction("KeHoachNhiemVu3");
            }
            else
            {
                TempData["error"] = "Tên nhiệm vụ đã tồn tại!";
                return RedirectToAction("KeHoachNhiemVu3");
            }
        }

        /// <summary>
        /// Cập nhật nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CapNhapNhiemVu(int Id)
        {
            NhiemVu model = _db.NhiemVus.Find(Id);
            List<int> _listId = new List<int>();
            IEnumerable<NguonVonNhiemVu> _listNguonVon = _db.NguonVonNhiemVus.Where(x => x.nhiemVuId == Id);
            foreach (NguonVonNhiemVu item in _listNguonVon)
            {
                DanhMucNguonVon _nguonvon = _db.DanhMucNguonVons.FirstOrDefault(x => x.danhMucNguonVonId == item.nguonVonId);
                if (_nguonvon != null)
                {
                    _listId.Add(_nguonvon.danhMucNguonVonId);
                }
            }
            NhiemVuModel entity = new NhiemVuModel
            {
                createTime = model.createTime,
                danhMucId = model.danhMucId,
                donViDuToanId = model.donViDuToanId,
                donViDeXuatId = model.donViDeXuatId,
                donViDeXuatName = model.donViDeXuatName,
                donViDutoanName = model.donViDutoanName,
                ghiChu = model.ghiChu,
                giaTriDeXuat = model.giaTriDeXuat.ToString("N0"),
                giaTriDeXuat2 = model.giaTriDeXuat2.ToString("N0"),
                giaTriDuyet = model.giaTriDuyet.GetValueOrDefault().ToString("N0"),
                giaTriGiao = model.giaTriGiao.GetValueOrDefault().ToString("N0"),
                giaTriQuyetToan = model.giaTriQuyetToan.GetValueOrDefault().ToString("N0"),
                giaTriTrungThau = model.giaTriTrungThau.GetValueOrDefault().ToString("N0"),
                isTrash = model.isTrash,
                namKeHoach = model.namKeHoach,
                nhaThauTrung = model.nhaThauTrung,
                nhiemVuId = model.nhiemVuId,
                nhiemVuName = model.nhiemVuName,
                trangThai = model.trangThai,
                updateTime = model.updateTime,
                userCreate = model.userCreate,
                userUpdate = model.userUpdate
            };
            ViewBag.danhMucId = _services.DropDownListNhiemVu2(null, null).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            ViewBag.donViDeXuatId = _services.DropDownListDanhMucDonVi(model.donViDeXuatId).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            ViewBag.donViDuToanId = _services.DropDownListDanhMucDonVi(model.donViDuToanId).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            ViewBag.namKeHoach = _services.DropDownListNamKeHoach(model.namKeHoach).Select(x => new SelectListItem { Text = x.Text, Value = x.Value });
            ViewBag.nguonVonName = _services.DropDownListDanhMucNguonVon(_listId).Select(x => new SelectListItem { Text = x.Text, Value = x.Value, Selected = x.IsSelect });
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhapNhiemVu(NhiemVuModel model)
        {
            int[] _nguonvon = model.nguonVonName2;
            StringBuilder _nguonvonName = new StringBuilder();
            if (_nguonvon.Count() > 0)
            {
                foreach (int item in _nguonvon)
                {
                    DanhMucNguonVon _enNguonVon = _db.DanhMucNguonVons.Find(item);
                    if (_enNguonVon != null)
                    {
                        _nguonvonName.Append(_enNguonVon.danhMucNguonVonName);
                        _nguonvonName.Append(",");
                    }
                }
            }
            if (!_services.CheckExitsNameNhiemVu(model.nhiemVuId, model.nhiemVuName, model.namKeHoach, model.donViDeXuatId, model.donViDuToanId, _nguonvonName.ToString().TrimEnd(',')))
            {
                decimal.TryParse(model.giaTriDeXuat, out decimal _gia);
                decimal.TryParse(model.giaTriDeXuat2, out decimal _gia2);
                NhiemVu entity = _db.NhiemVus.Find(model.nhiemVuId);
                entity.nhiemVuName = model.nhiemVuName;
                entity.danhMucId = model.danhMucId;
                entity.donViDeXuatId = model.donViDeXuatId;
                entity.donViDuToanId = model.donViDuToanId;
                entity.donViDeXuatName = _db.DanhMucDonVis.Find(model.donViDeXuatId).danhMucDonViName;
                entity.donViDutoanName = _db.DanhMucDonVis.Find(model.donViDuToanId).danhMucDonViName;
                entity.nguonVonName = model.nguonVonName;
                entity.ghiChu = model.ghiChu;
                entity.updateTime = DateTime.Now;
                entity.userUpdate = User.Identity.Name;
                entity.giaTriDeXuat = _gia;
                entity.giaTriDeXuat2 = _gia2;
                entity.namKeHoach = model.namKeHoach;
               
                entity.nguonVonName = _nguonvonName.ToString().TrimEnd(',');
                _db.SaveChanges();
                List<NguonVonNhiemVu> _nguonVonNV = _db.NguonVonNhiemVus.Where(x => x.nhiemVuId == entity.nhiemVuId).ToList();
                _db.NguonVonNhiemVus.RemoveRange(_nguonVonNV);
                _db.SaveChanges();
                if (_nguonvon.Count() > 0)
                {
                    foreach (int item in _nguonvon)
                    {
                        DanhMucNguonVon _enNguonVon = _db.DanhMucNguonVons.FirstOrDefault(x => x.danhMucNguonVonId == item);
                        if (_enNguonVon != null)
                        {
                            NguonVonNhiemVu entitys = new NguonVonNhiemVu
                            {
                                nguonVonId = _enNguonVon.danhMucNguonVonId,
                                nhiemVuId = entity.nhiemVuId
                            };
                            _db.NguonVonNhiemVus.Add(entitys);
                            _db.SaveChanges();
                        }
                    }
                }
                TempData["success"] = "Cập nhập thành công!";
            }
            else
            {
                TempData["error"] = "Tên nhiệm vụ đã tồn tại!";
            }

            return RedirectToAction("KeHoachNhiemVu3");
        }

        /// <summary>
        /// Xóa nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult XoaNhiemVu(int Id)
        {
            try
            {
                NhiemVu entity = _db.NhiemVus.Find(Id);
                entity.isTrash = true;
                _db.SaveChanges();
                TempData["success"] = "Xóa thành công!";
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Duyệt nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DuyetNhiemVu(int Id)
        {
            NhiemVu model = _db.NhiemVus.Find(Id);
            DuyetNhiemVuModel entity = new DuyetNhiemVuModel
            {
                createTime = model.createTime,
                isTrash = model.isTrash,
                danhMucId = model.danhMucId,
                donViDeXuatId = model.donViDeXuatId,
                donViDeXuatName = model.donViDeXuatName,
                donViDuToanId = model.donViDuToanId,
                donViDutoanName = model.donViDutoanName,
                fileAttachment = "",
                ghiChu = model.ghiChu,
                giaTriDeXuat = model.giaTriDeXuat,
                danhMucName = _services.GetNameDanhMucById(model.danhMucId),
                giaTriDuyet = model.giaTriDuyet,
                giaTriQuyetToan = model.giaTriQuyetToan,
                giaTriTrungThau = model.giaTriTrungThau,
                namKeHoach = model.namKeHoach,
                nguonVonName = model.nguonVonName,
                nhaThauTrung = model.nhaThauTrung,
                nhiemVuId = model.nhiemVuId,
                nhiemVuName = model.nhiemVuName,
                trangThai = model.trangThai,
                updateTime = model.updateTime,
                userCreate = model.userCreate,
                userUpdate = model.userUpdate
            };
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DuyetNhiemVu(DuyetNhiemVuModel model, string _hiddenFlieD)
        {
            decimal.TryParse(model.giaTriGiao, out decimal _gia);
            NhiemVu _nhiemvu = _db.NhiemVus.Find(model.nhiemVuId);
            _nhiemvu.trangThai = 1;
            _nhiemvu.updateTime = DateTime.Now;
            _nhiemvu.userUpdate = User.Identity.Name;
            _nhiemvu.giaTriGiao = _gia;
            try
            {
                if (!string.IsNullOrEmpty(model.hanTrinh))
                {
                    _nhiemvu.hanTrinh = DateTime.ParseExact(model.hanTrinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            catch
            {
            }
            _db.SaveChanges();
            _services.LogTrangThai(_nhiemvu.nhiemVuId, _nhiemvu.nhiemVuName, User.Identity.Name, 0, 1);
            List<string> _listFile = _hiddenFlieD.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu _NVTL = new NhiemVuTaiLieu
                    {
                        nhiemVuId = _nhiemvu.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(_NVTL);
                    _db.SaveChanges();
                }
            }
            TempData["success"] = "Duyệt thành công!";
            return RedirectToAction("KeHoachNhiemVu3");
        }

        [HttpPost]
        public ActionResult LayTaiLieu(string searchKey)
        {
            var model = (from c in _db.TaiLieux
                         where c.taiLieuName.StartsWith(searchKey)
                         select new { c.taiLieuName, c.taiLieuId });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Xem chi tiết nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ChiTietNhiemVu(int Id)
        {
            NhiemVu entity = _db.NhiemVus.Find(Id);
            TaiLieuNhiemVuModel _taiLieuNV = _services.GetTaiLieuTheoNhiemVu(Id);
            ViewBag.TaiLieu = _taiLieuNV;
            return PartialView(entity);
        }

        /// <summary>
        /// Cập nhật danh mục nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CapNhatDanhMucNhiemVu(int Id)
        {
            DanhMucNhiemVu model = _db.DanhMucNhiemVus.Find(Id);
            ViewBag.danhMucCha = _services.DropDownListNhiemVu2(model.danhMucNhiemVuId, model.danhMucCha).Select(x => new SelectListItem { Text = x.Text, Value = x.Value.ToString() });
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatDanhMucNhiemVu(DanhMucNhiemVu model)
        {
            if (ModelState.IsValid)
            {
                if (!_services.CheckExitsNameDanhMucNhiemVu(model.danhMucNhiemVuId, model.danhMucNhiemVuName))
                {
                    DanhMucNhiemVu entity = _db.DanhMucNhiemVus.Find(model.danhMucNhiemVuId);
                    entity.danhMucNhiemVuName = model.danhMucNhiemVuName;
                    entity.danhMucCha = model.danhMucCha;
                    entity.capDanhMuc = model.capDanhMuc;
                    _db.SaveChanges();
                    TempData["success"] = "Cập nhật thành công!";
                }
                else
                {
                    TempData["error"] = "Tên danh mục nhiệm vụ đã tồn tại!";
                }
            }
            return RedirectToAction("DanhMucNhiemVu");
        }

        /// <summary>
        /// Thêm mới danh mục nhiệm vụ
        /// </summary>
        /// <returns></returns>
        public ActionResult ThemMoiDanhMucNhiemVu()
        {
            DanhMucNhiemVu model = new DanhMucNhiemVu
            {
                isTrash = false
            };
            ViewBag.danhMucCha = _services.DropDownListNhiemVu2(model.danhMucNhiemVuId, model.danhMucCha).Select(x => new SelectListItem { Text = x.Text, Value = x.Value.ToString() });
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiDanhMucNhiemVu(DanhMucNhiemVu model)
        {
            if (ModelState.IsValid)
            {
                if (!_services.CheckExitsNameDanhMucNhiemVu(null, model.danhMucNhiemVuName))
                {
                    _db.DanhMucNhiemVus.Add(model);
                    _db.SaveChanges();
                    TempData["success"] = "Thêm mới thành công!";
                }
                else
                {
                    TempData["error"] = "Tên danh mục nhiệm vụ đã tồn tại!";
                }
            }
            return RedirectToAction("DanhMucNhiemVu");
        }

        /// <summary>
        /// Xóa danh mục nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult XoaDanhMucNhiemVu(int Id)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.danhMucId == Id && x.isTrash == false);
            if (model != null && model.Count() > 0)
            {
                TempData["error"] = "Danh mục nhiệm vụ đã được sử dụng!";
                return Json(TempData["error"], JsonRequestBehavior.AllowGet);
            }
            else
            {
                DanhMucNhiemVu entity = _db.DanhMucNhiemVus.Find(Id);
                entity.isTrash = true;
                _db.SaveChanges();
                TempData["success"] = "Xóa thành công!";
                return Json(TempData["success"], JsonRequestBehavior.AllowGet);
            }
        }

        public string GetDanhMucNhiemVuCha(int? Id)
        {
            if (Id.HasValue && Id.GetValueOrDefault() > 0)
            {
                DanhMucNhiemVu model = _db.DanhMucNhiemVus.Find(Id.GetValueOrDefault());
                if (model != null)
                {
                    return model.danhMucNhiemVuName;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Thêm mới danh mục đơn vị
        /// </summary>
        /// <returns></returns>
        public ActionResult ThemMoiDanhMucDonVi()
        {
            DanhMucDonVi model = new DanhMucDonVi
            {
                isTrash = false
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiDanhMucDonVi(DanhMucDonVi model)
        {
            if (ModelState.IsValid)
            {
                if (!_services.CheckExitsNameDanhMucDonVi(null, model.danhMucDonViName))
                {
                    _db.DanhMucDonVis.Add(model);
                    _db.SaveChanges();
                    TempData["success"] = "Thêm mới thành công!";
                }
                else
                {
                    TempData["error"] = "Tên đơn vị đã tồn tại!";
                }
            }
            return RedirectToAction("DanhMucDonVi");
        }

        /// <summary>
        /// Cập nhật danh mục đơn vi
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CapNhapDanhMucDonVi(int Id)
        {
            DanhMucDonVi model = _db.DanhMucDonVis.Find(Id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhapDanhMucDonVi(DanhMucDonVi model)
        {
            if (ModelState.IsValid)
            {
                if (!_services.CheckExitsNameDanhMucDonVi(model.danhMucDonViId, model.danhMucDonViName))
                {
                    DanhMucDonVi entity = _db.DanhMucDonVis.Find(model.danhMucDonViId);
                    entity.danhMucDonViName = model.danhMucDonViName;
                    _db.SaveChanges();
                    TempData["success"] = "Cập nhật thành công!";
                }
                else
                {
                    TempData["error"] = "Tên đơn vị đã tồn tại!";
                }
            }
            return RedirectToAction("DanhMucDonVi");
        }

        /// <summary>
        /// Xóa danh mục đơn vị
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult XoaDanhMucDonVi(int Id)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => (x.donViDeXuatId == Id || x.donViDuToanId == Id) && x.isTrash == false);
            if (model != null && model.Count() > 0)
            {
                TempData["error"] = "Danh mục đơn vị đã được sử dụng!";
                return Json(TempData["error"], JsonRequestBehavior.AllowGet);
            }
            else
            {
                DanhMucDonVi entity = _db.DanhMucDonVis.Find(Id);
                entity.isTrash = true;
                _db.SaveChanges();
                TempData["success"] = "Xóa thành công!";
                return Json(TempData["success"], JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Thêm mới danh mục nguồn vốn
        /// </summary>
        /// <returns></returns>
        public ActionResult ThemMoiDanhMucNguonVon()
        {
            DanhMucNguonVon model = new DanhMucNguonVon
            {
                isTrash = false
            };
            ViewBag.danhMucCha = _services.DropDownListNguonVon2(null, null).Select(x => new SelectListItem { Text = x.Text, Value = x.Value.ToString() });
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiDanhMucNguonVon(DanhMucNguonVon model)
        {
            if (ModelState.IsValid)
            {
                if (!_services.CheckExitsNameDanhMucNguonVon(null, model.danhMucNguonVonName))
                {
                    _db.DanhMucNguonVons.Add(model);
                    _db.SaveChanges();
                    TempData["success"] = "Thêm mới thành công!";
                }
                else
                {
                    TempData["error"] = "Tên nguồn vốn đã tồn tại!";
                }
            }
            return RedirectToAction("DanhMucNguonVon");
        }

        /// <summary>
        /// Cập nhật danh mục nguồn vốn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CapNhapDanhMucNguonVon(int Id)
        {
            DanhMucNguonVon model = _db.DanhMucNguonVons.Find(Id);
            ViewBag.danhMucCha = _services.DropDownListNguonVon2(model.danhMucNguonVonId, model.danhMucCha).Select(x => new SelectListItem { Text = x.Text, Value = x.Value.ToString() });
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhapDanhMucNguonVon(DanhMucNguonVon model)
        {
            if (ModelState.IsValid)
            {
                if (!_services.CheckExitsNameDanhMucNguonVon(model.danhMucNguonVonId, model.danhMucNguonVonName))
                {
                    DanhMucNguonVon entity = _db.DanhMucNguonVons.Find(model.danhMucNguonVonId);
                    entity.danhMucNguonVonName = model.danhMucNguonVonName;
                    entity.danhMucCha = model.danhMucCha;
                    entity.capDanhMuc = model.capDanhMuc;
                    _db.SaveChanges();
                    TempData["success"] = "Cập nhật thành công!";
                }
                else
                {
                    TempData["error"] = "Tên nguồn vốn đã tồn tại!";
                }
            }
            return RedirectToAction("DanhMucNguonVon");
        }

        /// <summary>
        /// Xóa danh mục nguồn vốn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult XoaDanhMucNguonVon(int Id)
        {
            DanhMucNguonVon entity = _db.DanhMucNguonVons.Find(Id);
            //IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.nguonVonName.Contains(Id.ToString()) && x.isTrash == false);
            //if (model != null && model.Count() > 0)
            //{
            //    TempData["error"] = "Danh mục nguồn vốn đã được sử dụng!";
            //    return Json(TempData["error"], JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
                entity.isTrash = true;
                _db.SaveChanges();
                TempData["success"] = "Xóa thành công!";
                return Json(TempData["success"], JsonRequestBehavior.AllowGet);
            //}
        }

        public string GetNguonVonCha(int? Id)
        {
            if (Id.HasValue && Id.GetValueOrDefault() > 0)
            {
                DanhMucNguonVon model = _db.DanhMucNguonVons.Find(Id.GetValueOrDefault());
                if (model != null)
                {
                    return model.danhMucNguonVonName;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public ActionResult QuanLyNhiemVu(string SearchKey, int? NguonVon, int? TrangThai, int? DonViDeXuat, int? DonViDuToan, int? NamKeHoach, int? PageIndex)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            ViewBag.DonViDeXuat = _services.DropDownListDanhMucDonVi(DonViDeXuat);
            ViewBag.DonViDuToan = _services.DropDownListDanhMucDonVi(DonViDuToan);
            ViewBag.NguonVon = _services.DropDownListDanhMucNguonVon(new List<int>());
            ViewBag.NamKeHoach = _services.DropDownListNamKeHoach(NamKeHoach);
            ViewBag.TrangThai = _services.DropDownListTrangThai(TrangThai);
            ViewBag.SearchKey = SearchKey;
            ViewBag.TrangThaiNC = TrangThai;
            ViewBag.DonViDeXuatNC = DonViDeXuat;
            decimal _totalGiaTriGiao = 0;
            decimal _totalGiaTriDuyet = 0;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }
            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                model = model.Where(x => x.donViDuToanId == DonViDuToan);
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach);
            }

            if (TrangThai.HasValue && TrangThai > 0)
            {
                model = model.Where(x => x.trangThai == TrangThai);
            }

            int totalRecord = model.Count();
            _totalGiaTriGiao = model.Select(x => x.giaTriGiao).DefaultIfEmpty(0).Sum() ?? 0;
            _totalGiaTriDuyet = model.Select(x => x.giaTriTrungThau).DefaultIfEmpty(0).Sum() ?? 0;
            model = model.OrderByDescending(x => x.createTime);
            int totalPage = 0;
            int _pageSize = 20;
            if (PageIndex != null)
            {
                model = model.Skip((PageIndex.Value - 1) * _pageSize);
            }
            totalPage = (int)Math.Ceiling(1.0 * totalRecord / _pageSize);
            model = model.Take(_pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = PageIndex ?? 1;
            ViewBag.TotalGiao = _totalGiaTriGiao.ToString("N0");
            ViewBag.TotalDuyet = _totalGiaTriDuyet.ToString("N0");
            return View(model.ToList());
        }

        public ActionResult QuanLyNhiemVu2(string SearchKey, int? NguonVon, int? TrangThai, int? DonViDeXuat, int? DonViDuToan, int? NamKeHoach, int? PageIndex)
        {
            IEnumerable<DanhMucNhiemVu> _DanhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == null);
            _DanhMuc = (from dm in _DanhMuc
                        join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                        where nv.trangThai > 0
                        select dm);
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            ViewBag.DonViDeXuat = _services.DropDownListDanhMucDonVi(DonViDeXuat);
            ViewBag.DonViDuToan = _services.DropDownListDanhMucDonVi(DonViDuToan);
            ViewBag.NguonVon = _services.DropDownListDanhMucNguonVon(new List<int>());
            ViewBag.NamKeHoach = _services.DropDownListNamKeHoach(NamKeHoach);
            ViewBag.TrangThai = _services.DropDownListTrangThai(TrangThai);
            ViewBag.SearchKey = SearchKey;
            ViewBag.TrangThaiNC = TrangThai;
            ViewBag.DonViDeXuatNC = DonViDeXuat;
            decimal _totalGiaTriGiao = 0;
            decimal _totalGiaTriDuyet = 0;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()) && nv.trangThai > 0
                            select dm);
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            join w in _db.NguonVonNhiemVus on nv.nhiemVuId equals w.nhiemVuId
                            where w.nguonVonId == NguonVon.Value && nv.trangThai > 0
                            select dm);
            }
            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.donViDeXuatId == DonViDeXuat && nv.trangThai > 0
                            select dm);
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.donViDuToanId == DonViDuToan && nv.trangThai > 0
                            select dm);
                model = model.Where(x => x.donViDuToanId == DonViDuToan);
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.namKeHoach == NamKeHoach && nv.trangThai > 0
                            select dm);
                model = model.Where(x => x.namKeHoach == NamKeHoach);
            }

            if (TrangThai.HasValue && TrangThai > 0)
            {
                _DanhMuc = (from dm in _DanhMuc
                            join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                            where nv.trangThai == TrangThai
                            select dm);
                model = model.Where(x => x.trangThai == TrangThai);
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            _DanhMuc = _DanhMuc.DistinctBy(x => x.danhMucNhiemVuId);
            _DanhMuc = _DanhMuc.OrderByDescending(x => x.danhMucNhiemVuId);
            foreach (NhiemVu item in model)
            {
                _totalGiaTriDuyet += item.giaTriTrungThau.GetValueOrDefault();
                _totalGiaTriGiao += item.giaTriGiao.GetValueOrDefault();
            }
            //_totalGiaTriGiao = model.Select(x => x.giaTriGiao).DefaultIfEmpty(0).Sum() ?? 0;
            //_totalGiaTriDuyet = model.Select(x => x.giaTriTrungThau).DefaultIfEmpty(0).Sum() ?? 0;
            model = model.OrderByDescending(x => x.createTime);
            ViewBag.TotalGiao = _totalGiaTriGiao.ToString("N0");
            ViewBag.TotalDuyet = _totalGiaTriDuyet.ToString("N0");
            return View(_DanhMuc);
        }

        public ActionResult QuanLyNhiemVu3(string SearchKey, int? NguonVon, int? TrangThai, int? DonViDeXuat, int? DonViDuToan, int? NamKeHoach, int? PageIndex)
        {
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            ViewBag.DonViDeXuat = _services.DropDownListDanhMucDonVi(DonViDeXuat);
            ViewBag.DonViDuToan = _services.DropDownListDanhMucDonVi(DonViDuToan);
            ViewBag.NguonVon = _services.DropDownListDanhMucNguonVon(new List<int>());
            ViewBag.NamKeHoach = _services.DropDownListNamKeHoach(NamKeHoach);
            ViewBag.TrangThai = _services.DropDownListTrangThai(TrangThai);
            ViewBag.SearchKey = SearchKey;
            ViewBag.TrangThaiNC = TrangThai;
            ViewBag.DonViDeXuatNC = DonViDeXuat;
            decimal _totalGiaTriGiao = 0;
            decimal _totalGiaTriDuyet = 0;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }
            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                model = model.Where(x => x.donViDuToanId == DonViDuToan);
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach);
            }

            if (TrangThai.HasValue && TrangThai > 0)
            {
                model = model.Where(x => x.trangThai == TrangThai);
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            List<NhiemVu> _ListNV = model.ToList();
            List<NhiemVuView> _tem = new List<NhiemVuView>();
            foreach (NhiemVu item in _ListNV)
            {
                _totalGiaTriDuyet += item.giaTriTrungThau.GetValueOrDefault();
                _totalGiaTriGiao += item.giaTriGiao.GetValueOrDefault();
                NhiemVuView _item = new NhiemVuView
                {
                    createTime = item.createTime,
                    danhMucId = item.danhMucId,
                    danhMucCha = item.danhMucId,
                    donViDeXuatId = item.donViDeXuatId,
                    donViDeXuatName = item.donViDeXuatName,
                    donViDuToanId = item.donViDuToanId,
                    donViDutoanName = item.donViDutoanName,
                    ghiChu = item.ghiChu,
                    giaTriDeXuat = item.giaTriDeXuat,
                    giaTriDeXuat2 = item.giaTriDeXuat2,
                    giaTriDuyet = item.giaTriDuyet,
                    giaTriGiao = item.giaTriGiao,
                    giaTriHopDong = item.giaTriHopDong,
                    giaTriQuyetToan = item.giaTriQuyetToan,
                    giaTriThanhToan = item.giaTriThanhToan,
                    giaTriTrungThau = item.giaTriTrungThau,
                    isTrash = item.isTrash,
                    Level = 0,
                    namKeHoach = item.namKeHoach,
                    nguonVonName = item.nguonVonName,
                    nhaThauTrung = item.nhaThauTrung,
                    nhiemVuId = item.nhiemVuId,
                    Code = item.nhiemVuId.ToString(),
                    nhiemVuName = item.nhiemVuName,
                    trangThai = item.trangThai,
                    updateTime = item.updateTime,
                    userCreate = item.userCreate,
                    userUpdate = item.userUpdate,
                    hanTrinh = item.hanTrinh.HasValue ? item.hanTrinh.GetValueOrDefault().ToString("dd/MM/yyyy") : null
                };
                if (_item.danhMucId > 0)
                {
                    List<NhiemVuView> _new = GetCha(_item.danhMucId, _tem);
                    _tem = _new;
                }
            }
            List<NhiemVuView> _tem2 = new List<NhiemVuView>();
            foreach (NhiemVuView itemTem in _tem)
            {
                _tem2.Add(itemTem);
                IOrderedEnumerable<NhiemVu> _listTem = _ListNV.OrderByDescending(x => x.createTime);
                foreach (NhiemVu nhiemVu in _listTem)
                {
                    if (nhiemVu.danhMucId == itemTem.danhMucId)
                    {
                        NhiemVuView _itemNV = new NhiemVuView
                        {
                            createTime = nhiemVu.createTime,
                            danhMucId = nhiemVu.danhMucId,
                            danhMucCha = nhiemVu.danhMucId,
                            donViDeXuatId = nhiemVu.donViDeXuatId,
                            donViDeXuatName = nhiemVu.donViDeXuatName,
                            donViDuToanId = nhiemVu.donViDuToanId,
                            donViDutoanName = nhiemVu.donViDutoanName,
                            ghiChu = nhiemVu.ghiChu,
                            giaTriDeXuat = nhiemVu.giaTriDeXuat,
                            giaTriDeXuat2 = nhiemVu.giaTriDeXuat2,
                            giaTriDuyet = nhiemVu.giaTriDuyet,
                            giaTriGiao = nhiemVu.giaTriGiao,
                            giaTriHopDong = nhiemVu.giaTriHopDong,
                            giaTriQuyetToan = nhiemVu.giaTriQuyetToan,
                            giaTriThanhToan = nhiemVu.giaTriThanhToan,
                            giaTriTrungThau = nhiemVu.giaTriTrungThau,
                            isTrash = nhiemVu.isTrash,
                            Level = itemTem.Level + 1,
                            namKeHoach = nhiemVu.namKeHoach,
                            nguonVonName = nhiemVu.nguonVonName,
                            nhaThauTrung = nhiemVu.nhaThauTrung,
                            nhiemVuId = nhiemVu.nhiemVuId,
                            Code = nhiemVu.nhiemVuId.ToString(),
                            nhiemVuName = nhiemVu.nhiemVuName,
                            trangThai = nhiemVu.trangThai,
                            updateTime = nhiemVu.updateTime,
                            userCreate = nhiemVu.userCreate,
                            userUpdate = nhiemVu.userUpdate,
                            hanTrinh = nhiemVu.hanTrinh.HasValue ? nhiemVu.hanTrinh.GetValueOrDefault().ToString("dd/MM/yyyy") : null
                        };
                        NhiemVuView checkExits = _tem2.FirstOrDefault(x => x.Code == _itemNV.Code);
                        if (checkExits == null)
                        {
                            _tem2.Add(_itemNV);
                        }
                    }
                }
            }
            List<NhiemVuView> _tem3 = new List<NhiemVuView>();
            List<NhiemVuView> _tem4 = _tem2;
            int count = 0;
            foreach (NhiemVuView itemTem in _tem4)
            {
                if (itemTem.Level == 1)
                {
                    count++;
                    itemTem.STT = _services.ChuyenSangSoLaMa(count);
                    _tem3.Add(itemTem);
                    _tem3 = AddCon(_tem3, itemTem.danhMucId, itemTem.STT, _tem4);
                }
            }
            ViewBag.TotalGiao = _totalGiaTriGiao.ToString("N0");
            ViewBag.TotalDuyet = _totalGiaTriDuyet.ToString("N0");
            return View(_tem3);
        }
        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult DanhMucDonVi(string SearchKey, int? PageIndex)
        {
            IQueryable<DanhMucDonVi> model = _db.DanhMucDonVis.Where(x => x.isTrash == false);
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.danhMucDonViName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }
            ViewBag.SearchKey = SearchKey;
            return View(model.ToList());
        }

        /// <summary>
        /// Danh sách danh mục nhiệm vụ
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult DanhMucNhiemVu(string SearchKey, int? PageIndex)
        {
            IEnumerable<DanhMucNhiemVu> model = _db.DanhMucNhiemVus.Where(x => x.isTrash == false);
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.danhMucNhiemVuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }
            model = model.OrderBy(x => x.danhMucNhiemVuId);
            List<DanhMucNhiemVu> _listDanhMucNhiemVu = model.ToList();
            List<DanhMucNhiemVu> _listTem = new List<DanhMucNhiemVu>();
            foreach (DanhMucNhiemVu item in _listDanhMucNhiemVu)
            {
                _listTem = GetChaDanhMucNhiemVu(item.danhMucCha, _listTem);
                DanhMucNhiemVu checkExits = _listTem.FirstOrDefault(x => x.danhMucNhiemVuId == item.danhMucNhiemVuId);
                if (checkExits == null)
                {
                    _listTem.Add(item);
                }
            }
            List<DanhMucNhiemVu> _listTem2 = new List<DanhMucNhiemVu>();
            _listTem = _listTem.OrderBy(x => x.danhMucCha).ToList();
            foreach (DanhMucNhiemVu item in _listTem)
            {
                DanhMucNhiemVu checkExit1 = _listTem2.FirstOrDefault(x => x.danhMucNhiemVuId == item.danhMucNhiemVuId);
                if (checkExit1 == null)
                {
                    _listTem2.Add(item);
                }
                List<DanhMucNhiemVu> _Child = _listTem.Where(x => x.danhMucCha == item.danhMucNhiemVuId).ToList();
                _Child = _Child.OrderBy(x => x.danhMucCha).ToList();
                foreach (DanhMucNhiemVu itemChild in _Child)
                {
                    DanhMucNhiemVu checkExits = _listTem2.FirstOrDefault(x => x.danhMucNhiemVuId == itemChild.danhMucNhiemVuId);
                    if (checkExits == null)
                    {
                        _listTem2.Add(itemChild);
                    }
                    AddConDanhMucNhiemVu(_listTem2, _listTem, itemChild.danhMucNhiemVuId);
                }
            }
            ViewBag.SearchKey = SearchKey;
            return View(_listTem2);
        }
        public List<DanhMucNhiemVu> GetChaDanhMucNhiemVu(int? Id, List<DanhMucNhiemVu> _curent)
        {
            DanhMucNhiemVu dmNV = _db.DanhMucNhiemVus.Find(Id);
            if (dmNV != null)
            {
                DanhMucNhiemVu _item = dmNV;
                if (_item.danhMucCha > 0)
                {
                    GetChaDanhMucNhiemVu(_item.danhMucCha, _curent);
                }
                DanhMucNhiemVu checkExits = _curent.FirstOrDefault(x => x.danhMucNhiemVuId == _item.danhMucNhiemVuId);
                if (checkExits == null)
                {
                    _curent.Add(_item);
                }
            }
            return _curent;
        }
        public ActionResult _DanhMucNhiemVuCon(int Id)
        {
            IEnumerable<DanhMucNhiemVu> model = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == Id);
            model = model.OrderBy(x => x.danhMucNhiemVuId);
            return PartialView(model);
        }

        /// <summary>
        /// Danh sách nguồn vốn
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult DanhMucNguonVon(string SearchKey, int? PageIndex)
        {
            IEnumerable<DanhMucNguonVon> model = _db.DanhMucNguonVons.Where(x => x.isTrash == false);
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.danhMucNguonVonName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }
            ViewBag.SearchKey = SearchKey;
            List<DanhMucNguonVon> _listNguonVon = model.ToList();
            List<DanhMucNguonVon> _listTem = new List<DanhMucNguonVon>();
            foreach (DanhMucNguonVon item in _listNguonVon)
            {
                _listTem = GetChaNguonVon(item.danhMucCha, _listTem);
                DanhMucNguonVon checkExits = _listTem.FirstOrDefault(x => x.danhMucNguonVonId == item.danhMucNguonVonId);
                if (checkExits == null)
                {
                    _listTem.Add(item);
                }
            }
            _listTem = _listTem.OrderBy(x => x.danhMucCha).ToList();
            List<DanhMucNguonVon> _listTem2 = new List<DanhMucNguonVon>();
            foreach (DanhMucNguonVon item in _listTem)
            {
                DanhMucNguonVon checkExit1 = _listTem2.FirstOrDefault(x => x.danhMucNguonVonId == item.danhMucNguonVonId);
                if (checkExit1 == null)
                {
                    _listTem2.Add(item);
                }
                List<DanhMucNguonVon> _Child = _listTem.Where(x => x.danhMucCha == item.danhMucNguonVonId).ToList();
                _Child = _Child.OrderBy(x => x.danhMucCha).ToList();
                foreach (DanhMucNguonVon itemChild in _Child)
                {
                    DanhMucNguonVon checkExits = _listTem2.FirstOrDefault(x => x.danhMucNguonVonId == itemChild.danhMucNguonVonId);
                    if (checkExits == null)
                    {
                        _listTem2.Add(itemChild);
                    }
                    AddConNguonVon(_listTem2, _listTem, itemChild.danhMucNguonVonId);
                }
            }
            return View(_listTem2);
        }
        public List<DanhMucNguonVon> AddConNguonVon(List<DanhMucNguonVon> _curent, List<DanhMucNguonVon> _data, int Id)
        {
            List<DanhMucNguonVon> _Child = _data.Where(x => x.danhMucCha == Id).ToList();
            _Child = _Child.OrderBy(x => x.danhMucCha).ToList();
            foreach (DanhMucNguonVon itemChild in _Child)
            {
                DanhMucNguonVon checkExits = _curent.FirstOrDefault(x => x.danhMucNguonVonId == itemChild.danhMucNguonVonId);
                if (checkExits == null)
                {
                    _curent.Add(itemChild);
                }
            }
            return _curent;
        }
        public List<DanhMucNhiemVu> AddConDanhMucNhiemVu(List<DanhMucNhiemVu> _curent, List<DanhMucNhiemVu> _data, int Id)
        {
            List<DanhMucNhiemVu> _Child = _data.Where(x => x.danhMucCha == Id).ToList();
            _Child = _Child.OrderBy(x => x.danhMucCha).ToList();
            foreach (DanhMucNhiemVu itemChild in _Child)
            {
                DanhMucNhiemVu checkExits = _curent.FirstOrDefault(x => x.danhMucNhiemVuId == itemChild.danhMucNhiemVuId);
                if (checkExits == null)
                {
                    _curent.Add(itemChild);
                }
            }
            return _curent;
        }
        public List<DanhMucNguonVon> GetChaNguonVon(int? Id, List<DanhMucNguonVon> _curent)
        {
            DanhMucNguonVon dmNV = _db.DanhMucNguonVons.Find(Id);
            if (dmNV != null)
            {
                DanhMucNguonVon _item = dmNV;
                if (_item.danhMucCha > 0)
                {
                    GetChaNguonVon(_item.danhMucCha, _curent);
                }
                DanhMucNguonVon checkExits = _curent.FirstOrDefault(x => x.danhMucNguonVonId == _item.danhMucNguonVonId);
                if (checkExits == null)
                {
                    _curent.Add(_item);
                }
            }
            return _curent;
        }
        public ActionResult _DanhMucNguonVonCon(int Id)
        {
            IEnumerable<DanhMucNguonVon> model = _db.DanhMucNguonVons.Where(x => x.isTrash == false && x.danhMucCha == Id);
            model = model.OrderBy(x => x.danhMucNguonVonId);
            return PartialView(model);
        }

        /// <summary>
        /// Danh sách người dùng
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult DanhSachNguoiDung(string SearchKey, int? PageIndex)
        {
            List<ApplicationUser> _listUser = _userConText.Users.ToList();
            if (!string.IsNullOrEmpty(SearchKey))
            {
                _listUser = _listUser.Where(x => x.Email.Contains(SearchKey) || x.UserName.Contains(SearchKey)).ToList();
            }
            ViewBag.Role = _services.DropDownListRole(null);
            return View(_listUser);
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DoiMatKhau(string Id)
        {
            ApplicationUser _user = _userConText.Users.Find(Id);
            ChangePass model = new ChangePass { FullName = _user.FullName, UserName = _user.UserName, UserId = _user.Id };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMatKhau(ChangePass model)
        {
            ApplicationUser entity = _userConText.Users.Find(model.UserId);
            if (!string.IsNullOrEmpty(model.PasswordNew) && model.PasswordNew.Length >= 6 && model.PasswordNew.Length <= 100)
            {
                IdentityResult resultRemovePass = UserManager.RemovePassword(model.UserId);
                if (resultRemovePass.Succeeded)
                {
                    IdentityResult result = UserManager.AddPassword(model.UserId, model.PasswordNew);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "Đổi mật khẩu thành công!";
                    }
                    else
                    {
                        TempData["error"] = result.Errors.FirstOrDefault();
                    }
                }
                else
                {
                    TempData["error"] = resultRemovePass.Errors.FirstOrDefault();
                }
            }
            else
            {
                TempData["error"] = "Mật khẩu phải lơn hơn 5 và nhỏ hơn 100 ký tự!";
            }
            return RedirectToAction("DanhSachNguoiDung");
        }

        /// <summary>
        /// Cập nhật người dùng
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CapNhatNguoiDung(string Id)
        {
            ApplicationUser _user = _userConText.Users.Find(Id);
            UpdateUser model = new UpdateUser { FullName = _user.FullName, RoleNameOld = getRoleName(_user.Roles.FirstOrDefault()), Email = _user.Email, UserName = _user.UserName, UserId = _user.Id, RoleName = getRoleName(_user.Roles.FirstOrDefault()) };
            ViewBag.RoleName = _services.DropDownListRole(_user.Roles.FirstOrDefault()?.RoleId ?? null).Select(x => new SelectListItem { Text = x.Text, Value = x.Text });
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatNguoiDung(UpdateUser model)
        {
            ApplicationUser _user = UserManager.FindById(model.UserId);
            _user.FullName = model.FullName;
            IdentityResult result = UserManager.Update(_user);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.RoleNameOld))
                {
                    UserManager.RemoveFromRole(_user.Id, model.RoleNameOld);
                }

                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    UserManager.AddToRole(_user.Id, model.RoleName);
                }

                TempData["success"] = "Cập nhật thành công!";
            }
            else
            {
                TempData["error"] = result.Errors.FirstOrDefault();
            }

            return RedirectToAction("DanhSachNguoiDung");
        }

        /// <summary>
        /// Khóa người dùng
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult XoaNguoiDung(string Id)
        {
            IdentityResult result = UserManager.SetLockoutEnabled(Id, true);
            UserManager.SetLockoutEndDate(Id, DateTime.Now.AddYears(1));
            if (result.Succeeded)
            {
                TempData["success"] = "Khóa thành công!";
            }
            else
            {
                TempData["error"] = result.Errors.FirstOrDefault();
            }

            return Json(result.Succeeded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenNguoiDung(string Id)
        {
            IdentityResult result = UserManager.SetLockoutEnabled(Id, false);
            UserManager.SetLockoutEndDate(Id, DateTime.Now);
            if (result.Succeeded)
            {
                TempData["success"] = "Mở khóa thành công!";
            }
            else
            {
                TempData["error"] = result.Errors.FirstOrDefault();
            }

            return Json(result.Succeeded, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thêm quyền
        /// </summary>
        /// <param name="_roleName"></param>
        /// <returns></returns>
        public ActionResult ThemQuyen(string _roleName)
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (!roleManager.RoleExists(_roleName))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = _roleName
                };
                roleManager.Create(role);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thêm người dùng
        /// </summary>
        /// <param name="_fullName"></param>
        /// <param name="_userName"></param>
        /// <param name="_email"></param>
        /// <param name="_passWord"></param>
        /// <param name="_roleName"></param>
        /// <returns></returns>
        public async Task<ActionResult> ThemNguoiDung(string _fullName, string _userName, string _email, string _passWord, string _roleName)
        {
            ApplicationUser user = new ApplicationUser { UserName = _userName, Email = _email, FullName = _fullName };
            IdentityResult result = await UserManager.CreateAsync(user, _passWord);
            if (result.Succeeded && !string.IsNullOrEmpty(_roleName))
            {
                IdentityResult roleresult = UserManager.AddToRole(user.Id, _roleName);
                return Json(roleresult, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy tên quyền
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public string getRoleName(IdentityUserRole _id)
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (_id != null)
            {
                IdentityRole _role = roleManager.FindById(_id.RoleId);
                if (_role != null)
                {
                    return _role.Name;
                }
            }
            return "";
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        /// <summary>
        /// Danh sách dữ liệu
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult KhoDuLieu(string SearchKey, int? PageIndex)
        {
            IQueryable<TaiLieu> model = _db.TaiLieux.Where(x => x.isTrash == false);
            if (!string.IsNullOrEmpty(SearchKey))
            {
                model = model.Where(x => x.taiLieuName.ToLower().Contains(SearchKey.ToLower().Trim()));
            }
            ViewBag.SearchKey = SearchKey;
            return View(model.ToList());
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <returns></returns>
        public ActionResult ThemMoiDuLieu()
        {
            TaiLieu model = new TaiLieu
            {
                isTrash = false
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiDuLieu(TaiLieu model, HttpPostedFileBase fileAttach)
        {
            try
            {
                if (fileAttach.ContentLength > 0)
                {
                    string _fileName = Path.GetFileName(fileAttach.FileName);
                    bool exists = System.IO.Directory.Exists(Server.MapPath("~/KhoDuLieu"));
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/KhoDuLieu"));
                    }

                    string _path = Path.Combine(Server.MapPath("~/KhoDuLieu"), _fileName);
                    bool existFile = System.IO.File.Exists(_path);
                    if (!existFile)
                    {
                        fileAttach.SaveAs(_path);
                        if (!_services.CheckExitsNameTaiLieu(null, _fileName))
                        {
                            TaiLieu entity = new TaiLieu
                            {
                                taiLieuName = _fileName,
                                taiLieuLink = "KhoDuLieu/" + _fileName,
                                isTrash = false
                            };
                            _db.TaiLieux.Add(entity);
                            _db.SaveChanges();
                            //TempData["success"] = "Thêm mới thành công!";
                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //TempData["error"] = "Tài liệu đã tồn tại tồn tại!";
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //TempData["error"] = "Tệp đã tồn tại tồn tại!";
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //TempData["error"] = "Không thể thêm khi chưa chọn tệp!";
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("KhoDuLieu");
        }

        /// <summary>
        /// Cập nhập dữ liệu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CapNhapDuLieu(int Id)
        {
            TaiLieu model = _db.TaiLieux.Find(Id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhapDuLieu(TaiLieu model, HttpPostedFileBase fileAttach)
        {
            try
            {
                if (fileAttach.ContentLength > 0)
                {
                    string _fileName = Path.GetFileName(fileAttach.FileName);
                    bool exists = System.IO.Directory.Exists(Server.MapPath("~/KhoDuLieu"));
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/KhoDuLieu"));
                    }

                    string _path = Path.Combine(Server.MapPath("~/KhoDuLieu"), _fileName);
                    bool existFile = System.IO.File.Exists(_path);
                    if (!existFile)
                    {
                        fileAttach.SaveAs(_path);
                        if (!_services.CheckExitsNameTaiLieu(model.taiLieuId, model.taiLieuName))
                        {
                            TaiLieu entity = _db.TaiLieux.Find(model.taiLieuId);
                            entity.taiLieuName = _fileName;
                            entity.taiLieuLink = "KhoDuLieu/" + _fileName; ;
                            _db.SaveChanges();
                            TempData["success"] = "Cập nhập thành công!";
                        }
                        else
                        {
                            TempData["error"] = "Tài liệu đã tồn tại tồn tại!";
                        }
                    }
                    else
                    {
                        TempData["error"] = "Tệp đã tồn tại tồn tại!";
                    }
                }
                else
                {
                    TempData["error"] = "Không thể thêm khi chưa chọn tệp!";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("KhoDuLieu");
        }

        /// <summary>
        /// Xóa tài liệu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult XoaTaiLieu(int Id)
        {
            IQueryable<NhiemVuTaiLieu> model = _db.NhiemVuTaiLieux.Where(x => x.taiLieuId == Id);
            if (model != null && model.Count() > 0)
            {
                TempData["error"] = "Tài liệu đã được sử dụng!";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TaiLieu entity = _db.TaiLieux.Find(Id);
                entity.isTrash = true;
                _db.SaveChanges();
                TempData["success"] = "Xóa thành công!";
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Danh sách sản phầm
        /// </summary>
        /// <param name="NhiemVu"></param>
        /// <param name="SanPham"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult SanPham(string NhiemVu, string SanPham, int? PageIndex)
        {
            IQueryable<SanPham> model = _db.SanPhams.Where(x => x.isTrash == false);
            if (!string.IsNullOrEmpty(NhiemVu))
            {
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(NhiemVu.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(SanPham))
            {
                model = model.Where(x => x.sanPhamName.ToLower().Contains(SanPham.ToLower().Trim()));
            }

            int totalRecord = model.Count();
            model = model.OrderByDescending(x => x.nhiemVuId);
            int totalPage = 0;
            int _pageSize = 20;
            if (PageIndex != null)
            {
                model = model.Skip((PageIndex.Value - 1) * _pageSize);
            }
            totalPage = (int)Math.Ceiling(1.0 * totalRecord / _pageSize);
            model = model.Take(_pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.PageIndex = PageIndex ?? 1;
            ViewBag.NhiemVu = NhiemVu;
            ViewBag.SanPham = SanPham;
            return View(model.ToList());
        }

        /// <summary>
        /// Thống kê
        /// </summary>
        /// <param name="DonViThucHien"></param>
        /// <param name="NamKeHoach"></param>
        /// <param name="NguonVon"></param>
        /// <param name="DanhMucNhiemVu"></param>
        /// <returns></returns>
        public ActionResult ThongKe(int? DonViThucHien, int? NamKeHoach, int? NguonVon, int? DanhMucNhiemVu, int? PageIndex)
        {
            decimal _totalGiao = 0;
            decimal _totalDuyet = 0;
            decimal _totalHopDong = 0;
            decimal _totalGiaiNgan = 0;
            decimal _totalQuyetToan = 0;
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            ViewBag.DonViThucHien = _services.DropDownListDanhMucDonVi(DonViThucHien);
            ViewBag.NguonVon = _services.DropDownListDanhMucNguonVon(new List<int>());
            ViewBag.NamKeHoach = _services.DropDownListNamKeHoach(NamKeHoach);
            ViewBag.DanhMucNhiemVu = _services.DropDownListDanhMucNhiemVu(DanhMucNhiemVu);
            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }
            if (DonViThucHien.HasValue && DonViThucHien > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViThucHien);
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach);
            }

            if (DanhMucNhiemVu.HasValue && DanhMucNhiemVu > 0)
            {
                model = model.Where(x => x.danhMucId == DanhMucNhiemVu);
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            List<NhiemVu> _ListNV = model.ToList();
            List<NhiemVuView> _tem = new List<NhiemVuView>();
            foreach (NhiemVu item in _ListNV)
            {
                _totalDuyet += item.giaTriTrungThau.GetValueOrDefault();
                _totalGiao += item.giaTriGiao.GetValueOrDefault();
                _totalHopDong += item.giaTriHopDong.GetValueOrDefault();
                _totalQuyetToan += item.giaTriQuyetToan.GetValueOrDefault();
                IEnumerable<GiaiNgan> giaiNgan = _db.GiaiNgans.Where(x => x.nhiemVuId == item.nhiemVuId);
                giaiNgan = giaiNgan.OrderByDescending(x => x.thoiGianThanhToan);
                _totalGiaiNgan += (giaiNgan.Select(x => x.giaTriThanhToan).DefaultIfEmpty(0).Sum() ?? 0);
                NhiemVuView _item = new NhiemVuView
                {
                    createTime = item.createTime,
                    danhMucId = item.danhMucId,
                    danhMucCha = item.danhMucId,
                    donViDeXuatId = item.donViDeXuatId,
                    donViDeXuatName = item.donViDeXuatName,
                    donViDuToanId = item.donViDuToanId,
                    donViDutoanName = item.donViDutoanName,
                    ghiChu = item.ghiChu,
                    giaTriDeXuat = item.giaTriDeXuat,
                    giaTriDeXuat2 = item.giaTriDeXuat2,
                    giaTriDuyet = item.giaTriDuyet,
                    giaTriGiao = item.giaTriGiao,
                    giaTriHopDong = item.giaTriHopDong,
                    giaTriQuyetToan = item.giaTriQuyetToan,
                    giaTriThanhToan = item.giaTriThanhToan,
                    giaTriTrungThau = item.giaTriTrungThau,
                    isTrash = item.isTrash,
                    Level = 0,
                    namKeHoach = item.namKeHoach,
                    nguonVonName = item.nguonVonName,
                    nhaThauTrung = item.nhaThauTrung,
                    nhiemVuId = item.nhiemVuId,
                    Code = item.nhiemVuId.ToString(),
                    nhiemVuName = item.nhiemVuName,
                    trangThai = item.trangThai,
                    updateTime = item.updateTime,
                    userCreate = item.userCreate,
                    userUpdate = item.userUpdate
                };
                if (_item.danhMucId > 0)
                {
                    List<NhiemVuView> _new = GetCha(_item.danhMucId, _tem);
                    _tem = _new;
                }
            }
            List<NhiemVuView> _tem2 = new List<NhiemVuView>();
            foreach (NhiemVuView itemTem in _tem)
            {
                _tem2.Add(itemTem);
                IOrderedEnumerable<NhiemVu> _listTem = _ListNV.OrderByDescending(x => x.createTime);
                foreach (NhiemVu nhiemVu in _listTem)
                {
                    if (nhiemVu.danhMucId == itemTem.danhMucId)
                    {
                        NhiemVuView _itemNV = new NhiemVuView
                        {
                            createTime = nhiemVu.createTime,
                            danhMucId = nhiemVu.danhMucId,
                            danhMucCha = nhiemVu.danhMucId,
                            donViDeXuatId = nhiemVu.donViDeXuatId,
                            donViDeXuatName = nhiemVu.donViDeXuatName,
                            donViDuToanId = nhiemVu.donViDuToanId,
                            donViDutoanName = nhiemVu.donViDutoanName,
                            ghiChu = nhiemVu.ghiChu,
                            giaTriDeXuat = nhiemVu.giaTriDeXuat,
                            giaTriDeXuat2 = nhiemVu.giaTriDeXuat2,
                            giaTriDuyet = nhiemVu.giaTriDuyet,
                            giaTriGiao = nhiemVu.giaTriGiao,
                            giaTriHopDong = nhiemVu.giaTriHopDong,
                            giaTriQuyetToan = nhiemVu.giaTriQuyetToan,
                            giaTriThanhToan = nhiemVu.giaTriThanhToan,
                            giaTriTrungThau = nhiemVu.giaTriTrungThau,
                            isTrash = nhiemVu.isTrash,
                            Level = itemTem.Level + 1,
                            namKeHoach = nhiemVu.namKeHoach,
                            nguonVonName = nhiemVu.nguonVonName,
                            nhaThauTrung = nhiemVu.nhaThauTrung,
                            nhiemVuId = nhiemVu.nhiemVuId,
                            Code = nhiemVu.nhiemVuId.ToString(),
                            nhiemVuName = nhiemVu.nhiemVuName,
                            trangThai = nhiemVu.trangThai,
                            updateTime = nhiemVu.updateTime,
                            userCreate = nhiemVu.userCreate,
                            userUpdate = nhiemVu.userUpdate
                        };
                        NhiemVuView checkExits = _tem2.FirstOrDefault(x => x.Code == _itemNV.Code);
                        if (checkExits == null)
                        {
                            _tem2.Add(_itemNV);
                        }
                    }
                }
            }
            ViewBag.TotalPage = 0;
            ViewBag.PageIndex = PageIndex ?? 1;
            ViewBag.TotalGiaiNgan = _totalGiaiNgan.ToString("N0");
            ViewBag.TotalDuyet = _totalDuyet.ToString("N0");
            ViewBag.TotalGiao = _totalGiao.ToString("N0");
            ViewBag.TotalHopDong = _totalHopDong.ToString("N0");
            ViewBag.TotalQuyetToan = _totalQuyetToan.ToString("N0");
            List<NhiemVuView> _tem3 = new List<NhiemVuView>();
            List<NhiemVuView> _tem4 = _tem2;
            int count = 0;
            foreach (NhiemVuView itemTem in _tem4)
            {
                if (itemTem.Level == 1)
                {
                    count++;
                    itemTem.STT = _services.ChuyenSangSoLaMa(count);
                    _tem3.Add(itemTem);
                    _tem3 = AddCon(_tem3, itemTem.danhMucId, itemTem.STT, _tem4);
                }
            }
            return View(_tem3);
        }

        public string _TotalGiaTriDeXuatDonVi(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriDeXuat).DefaultIfEmpty(0).Sum();
            }
            return _Total.ToString("N0");
        }
        public string _TotalGiaTriDeXuatTongCuc(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriDeXuat2).DefaultIfEmpty(0).Sum();
            }
            return _Total.ToString("N0");
        }


        public string _TotalDuToanGiao(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriGiao).DefaultIfEmpty(0).Sum() ?? 0;
            }
            return _Total.ToString("N0");
        }

        public string _TotalDuToanDuyet(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriTrungThau).DefaultIfEmpty(0).Sum() ?? 0;
            }
            return _Total.ToString("N0");
        }

        public string _TotalHopDong(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriHopDong).DefaultIfEmpty(0).Sum() ?? 0;
            }
            return _Total.ToString("N0");
        }

        public string _TotalGiaiNgan(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriThanhToan).DefaultIfEmpty(0).Sum() ?? 0;
            }
            return _Total.ToString("N0");
        }

        public string _TotalQuyetToan(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriQuyetToan).DefaultIfEmpty(0).Sum() ?? 0;
            }
            return _Total.ToString("N0");
        }

        /// <summary>
        /// Lấy giải ngân
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public decimal GetGiaiNgan(IEnumerable<NhiemVu> _input)
        {
            decimal _Total = 0;
            foreach (NhiemVu item in _input)
            {
                IQueryable<GiaiNgan> model = _db.GiaiNgans.Where(x => x.nhiemVuId == item.nhiemVuId);
                model = model.OrderByDescending(x => x.thoiGianThanhToan);
                _Total += (model.Select(x => x.giaTriThanhToan).DefaultIfEmpty(0).Sum() ?? 0);
            }
            return _Total;
        }

        /// <summary>
        /// Lấy tên danh mục
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string LayDanhMucById(int Id)
        {
            DanhMucNhiemVu model = _db.DanhMucNhiemVus.Find(Id);
            if (model != null)
            {
                return model.danhMucNhiemVuName;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult TrinhDeCuongDuToan(int _hiddenId, string _hiddenFiles)
        {
            NhiemVu entity = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuId == _hiddenId);
            entity.trangThai = 2;
            _db.SaveChanges();
            _services.LogTrangThai(entity.nhiemVuId, entity.nhiemVuName, User.Identity.Name, 1, 2);
            List<string> _listFile = _hiddenFiles.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu model = new NhiemVuTaiLieu
                    {
                        nhiemVuId = entity.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(model);
                    _db.SaveChanges();
                }
            }
            TempData["success"] = "Trình đề cương dự toán thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Kiểm tra tệp tin đã có hay chưa
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public ActionResult KiemTraFile(string FileName)
        {
            bool result = _services.CheckExitsNameTaiLieu(null, FileName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thêm tệp tin vào danh sách
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ListFile"></param>
        /// <returns></returns>
        public ActionResult AddFileToList(string FileName, string ListFile)
        {
            List<string> _list = ListFile.Split(';').ToList();
            if (_list.Count > 0)
            {
                if (!_list.Any(x => x == FileName))
                {
                    TaiLieu result = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == FileName.Trim());
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TaiLieu result = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == FileName.Trim());
                if (result != null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tài liệu theo nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult TaiLieuNhiemVu(int Id)
        {
            IQueryable<NhiemVuTaiLieu> model = _db.NhiemVuTaiLieux.Where(x => x.nhiemVuId == Id);
            IQueryable<TaiLieu> entity = (from q in _db.TaiLieux join e in model on q.taiLieuId equals e.taiLieuId select q);
            return PartialView(entity.ToList());
        }

        /// <summary>
        /// Phê duyệt đề cương dự án
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult PheDuyetDeCuong(int Id)
        {
            NhiemVu model = _db.NhiemVus.Find(Id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PheDuyetDeCuong(int _hiddenIdPDDC, string _hiddenFilesPDDC, string _SanPham, string _GiaTriDuocDuyet)
        {
            decimal.TryParse(_GiaTriDuocDuyet, out decimal _gia);
            NhiemVu entity = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuId == _hiddenIdPDDC);
            entity.trangThai = 3;
            entity.giaTriTrungThau = _gia;
            _db.SaveChanges();
            _services.LogTrangThai(entity.nhiemVuId, entity.nhiemVuName, User.Identity.Name, 2, 3);
            List<string> _listFile = _hiddenFilesPDDC.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu model = new NhiemVuTaiLieu
                    {
                        nhiemVuId = entity.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(model);
                    _db.SaveChanges();
                }
            }
            if (!string.IsNullOrEmpty(_SanPham))
            {
                SanPham _enSanPham = new SanPham
                {
                    nhiemVuId = entity.nhiemVuId,
                    isTrash = false,
                    nhiemVuName = entity.nhiemVuName,
                    sanPhamName = _SanPham
                };
                _db.SanPhams.Add(_enSanPham);
                _db.SaveChanges();
            }
            TempData["success"] = "Phê duyệt đề cương dự toán thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Trình kế hoạch lựa chọn nhà thầu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult TrinhKeHoachLuaChoNhaThau(int Id)
        {
            NhiemVu model = _db.NhiemVus.Find(Id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult TrinhKeHoachLuaChoNhaThau(int _hiddenIdTKHLCNT, string _hiddenFilesTKHLCNT)
        {
            NhiemVu entity = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuId == _hiddenIdTKHLCNT);
            entity.trangThai = 4;
            _db.SaveChanges();
            _services.LogTrangThai(entity.nhiemVuId, entity.nhiemVuName, User.Identity.Name, 3, 4);
            List<string> _listFile = _hiddenFilesTKHLCNT.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu model = new NhiemVuTaiLieu
                    {
                        nhiemVuId = entity.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(model);
                    _db.SaveChanges();
                }
            }
            TempData["success"] = "Trình đề kế hoạch lựa chọn nhà thầu thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Phê duyệt kế hoạch nhà thầu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult PheDuyetKeHoachNhaThau(int Id)
        {
            NhiemVu model = _db.NhiemVus.Find(Id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PheDuyetKeHoachNhaThau(int _hiddenIdPDKHNT, string _hiddenSelectPDKHNT, string _hiddenFilePDKHNT)
        {
            NhiemVu entity = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuId == _hiddenIdPDKHNT);
            entity.trangThai = 5;
            _db.SaveChanges();
            _services.LogTrangThai(entity.nhiemVuId, entity.nhiemVuName, User.Identity.Name, 4, 5);
            //Tạm đóng
            //string[] _listSelect = _hiddenSelectPDKHNT.Split('|');
            //if (_listSelect.Count() > 0)
            //{
            //    foreach (string item in _listSelect)
            //    {
            //        string[] _item = item.Split(';');
            //        if (_item.Count() > 1)
            //        {
            //            decimal.TryParse(_item[1], out decimal _gia);
            //            GoiThau enThau = new GoiThau
            //            {
            //                giaThau = _gia,
            //                nhaThauName = _item[0],
            //                nhiemVuId = _hiddenIdPDKHNT
            //            };
            //            _db.GoiThaus.Add(enThau);
            //            _db.SaveChanges();
            //        }
            //    }
            //}
            List<string> _listFile = _hiddenFilePDKHNT.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu _NVTL = new NhiemVuTaiLieu
                    {
                        nhiemVuId = entity.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(_NVTL);
                    _db.SaveChanges();
                }
            }
            TempData["success"] = "Phê duyệt kế hoạch nhà thầu thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Xóa nhà thầu
        /// </summary>
        /// <param name="_Name"></param>
        /// <param name="_Gia"></param>
        /// <param name="_ListSelect"></param>
        /// <returns></returns>
        public ActionResult XoaNhaThau(string _Name, string _Gia, string _ListSelect)
        {
            decimal.TryParse(_Gia, out decimal Gia);
            _ListSelect = _ListSelect.Replace(_Name + ";" + _Gia, "");
            _ListSelect = _ListSelect.Replace("||", "|");
            _ListSelect = _ListSelect.TrimStart('|');
            string[] _Select = _ListSelect.Split('|');
            string _html = "";
            if (_Select.Count() > 0)
            {
                _html += "<div class=\"nha-thau\">";
                _html += "Danh sách nhà thầu:";
                _html += "<table class=\"table table-bordered\" >";
                _html += "<tr><th>Tên nhà thầu</th><th>Giá trúng thầu</th><th>Thao tác</th></tr>";
                int count = 0;
                foreach (string item in _Select)
                {
                    count++;
                    string[] _item = item.Split(';');
                    if (_item.Count() > 1)
                    {
                        _html += "<tr>";
                        _html += "<td>";
                        _html += _item[0];
                        _html += "</td>";
                        _html += "<td>";
                        _html += _item[1];
                        _html += "</td>";
                        _html += "<td><button type=\"button\" onclick=\"xoaNhaThau('" + _item[0] + "','" + _item[1] + "')\"><i class=\"fa fa-trash\"></i> Xóa</button>";
                        _html += "</td>";
                        _html += "</tr>";
                    }
                }
                _html += "</table>";
                _html += "</div>";
            }
            GoiThauView result = new GoiThauView
            {
                GoiThaus = _ListSelect.TrimEnd('|'),
                _Html = _html
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thêm nhà thầu vào danh sách
        /// </summary>
        /// <param name="_Name"></param>
        /// <param name="_Gia"></param>
        /// <param name="_ListSelect"></param>
        /// <param name="_NhiemVuId"></param>
        /// <returns></returns>
        public ActionResult ThemNhaThau(string _Name, string _Gia, string _ListSelect, int _NhiemVuId)
        {
            decimal.TryParse(_Gia, out decimal Gia);
            _ListSelect += "|";
            _ListSelect += _Name;
            _ListSelect += ";";
            _ListSelect += _Gia;
            _ListSelect += "|";
            string[] _Select = _ListSelect.Split('|');
            string _html = "";
            if (_Select.Count() > 0)
            {
                _html += "<div class=\"nha-thau\">";
                _html += "Danh sách nhà thầu:";
                _html += "<table class=\"table table-bordered\" >";
                _html += "<tr><th>Tên nhà thầu</th><th>Giá trúng thầu</th><th>Thao tác</th></tr>";
                int count = 0;
                foreach (string item in _Select)
                {
                    count++;
                    string[] _item = item.Split(';');
                    if (_item.Count() > 1)
                    {
                        _html += "<tr>";
                        _html += "<td>";
                        _html += _item[0];
                        _html += "</td>";
                        _html += "<td>";
                        _html += _item[1];
                        _html += "</td>";
                        _html += "<td><button type=\"button\" onclick=\"xoaNhaThau('" + _item[0] + "','" + _item[1] + "')\"><i class=\"fa fa-trash\"></i> Xóa</button>";
                        _html += "</td>";
                        _html += "</tr>";
                    }
                }
                _html += "</table>";
                _html += "</div>";
            }
            GoiThauView result = new GoiThauView
            {
                GoiThaus = _ListSelect.TrimEnd('|'),
                _Html = _html
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Phê duyệt kết quá lựa chọn nhà thầu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult PheDuyetKetQuaLuaChonNhaThau(int Id)
        {
            IQueryable<GoiThau> NhaThau = _db.GoiThaus.Where(x => x.nhiemVuId == Id);
            NhiemVu model = _db.NhiemVus.Find(Id);
            NhiemVuModel entity = new NhiemVuModel { createTime = model.createTime, nhiemVuId = model.nhiemVuId };
            ViewBag.ListNhaThau = NhaThau.ToList();
            return PartialView(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PheDuyetKetQuaLuaChonNhaThau(NhiemVuModel model, string _hiddenFliePDKQLCNT, string _hiddenSelectPDKQLCNT)
        {
            //decimal.TryParse(model.giaTriTrungThau, out decimal _gia);
            NhiemVu entity = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuId == model.nhiemVuId);
            entity.trangThai = 6;
            _db.SaveChanges();
            string[] _listSelect = _hiddenSelectPDKQLCNT.Split('|');
            if (_listSelect.Count() > 0)
            {
                foreach (string item in _listSelect)
                {
                    string[] _item = item.Split(';');
                    if (_item.Count() > 1)
                    {
                        decimal.TryParse(_item[1], out decimal _gia);
                        GoiThau enThau = new GoiThau
                        {
                            giaThau = _gia,
                            nhaThauName = _item[0],
                            nhiemVuId = model.nhiemVuId
                        };
                        _db.GoiThaus.Add(enThau);
                        _db.SaveChanges();
                    }
                }
            }
            _services.LogTrangThai(entity.nhiemVuId, entity.nhiemVuName, User.Identity.Name, 5, 6);
            List<string> _listFile = _hiddenFliePDKQLCNT.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu _NVTL = new NhiemVuTaiLieu
                    {
                        nhiemVuId = entity.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(_NVTL);
                    _db.SaveChanges();
                }
            }
            TempData["success"] = "Phê duyệt kết quả lựa chọn nhà thầu thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Ký hợp đồng
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult KyHopDong(int Id)
        {
            NhiemVu entity = _db.NhiemVus.Find(Id);
            NhiemVuModel model = new NhiemVuModel { nhiemVuId = entity.nhiemVuId, nhaThauTrung = entity.nhaThauTrung };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult KyHopDong(NhiemVuModel model, string _hiddenFlieKHD)
        {
            decimal.TryParse(model.giaTriHopDong, out decimal _gia);
            NhiemVu entity = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuId == model.nhiemVuId);
            entity.trangThai = 7;
            entity.giaTriHopDong = _gia;
            _db.SaveChanges();
            _services.LogTrangThai(entity.nhiemVuId, entity.nhiemVuName, User.Identity.Name, 6, 7);
            List<string> _listFile = _hiddenFlieKHD.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu _NVTL = new NhiemVuTaiLieu
                    {
                        nhiemVuId = entity.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(_NVTL);
                    _db.SaveChanges();
                }
            }
            TempData["success"] = "Ký hợp đồng thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Giải ngân
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult GiaiNgan(int Id)
        {
            NhiemVu entity = _db.NhiemVus.Find(Id);
            GiaiNganModel model = new GiaiNganModel { nhiemVuId = entity.nhiemVuId, nhiemVuName = entity.nhiemVuName };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GiaiNgan(GiaiNganModel model)
        {
            DateTime _date = DateTime.Now;
            _date = DateTime.ParseExact(model.thoiGianThanhToan, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            decimal.TryParse(model.giaTriThanhToan, out decimal _gia);
            NhiemVu nhiemvu = _db.NhiemVus.Find(model.nhiemVuId);
            decimal dutoanduocduyet = 1;
            if (nhiemvu != null && nhiemvu.giaTriTrungThau.HasValue)
            {
                dutoanduocduyet = nhiemvu.giaTriTrungThau.Value;
            }

            GiaiNgan entity = new GiaiNgan
            {
                giaTriThanhToan = _gia,
                tiLeGiaiNgan = (double)(_gia / dutoanduocduyet),
                thoiGianThanhToan = _date,
                nhiemVuId = model.nhiemVuId,
                nhiemVuName = model.nhiemVuName
            };
            _db.GiaiNgans.Add(entity);
            _db.SaveChanges();
            TempData["success"] = "Giải ngân thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Kết thúc nhiệm vụ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult KetThuc(int Id)
        {
            NhiemVu entity = _db.NhiemVus.Find(Id);
            NhiemVuModel model = new NhiemVuModel { nhiemVuId = entity.nhiemVuId, nhiemVuName = entity.nhiemVuName, ghiChu = entity.ghiChu };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult KetThuc(NhiemVuModel model, string _hiddenFlieKT)
        {
            decimal.TryParse(model.giaTriThanhToan, out decimal _giaThanhToan);
            decimal.TryParse(model.giaTriQuyetToan, out decimal _giaQuyetToan);
            NhiemVu entity = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuId == model.nhiemVuId);
            entity.trangThai = 8;
            entity.giaTriThanhToan = _giaThanhToan;
            entity.giaTriQuyetToan = _giaQuyetToan;
            entity.ghiChu = model.ghiChu;
            entity.updateTime = DateTime.Now;
            _db.SaveChanges();
            _services.LogTrangThai(entity.nhiemVuId, entity.nhiemVuName, User.Identity.Name, 7, 8);
            List<string> _listFile = _hiddenFlieKT.Split(';').ToList();
            foreach (string item in _listFile)
            {
                TaiLieu _model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == item);
                if (_model != null)
                {
                    NhiemVuTaiLieu _NVTL = new NhiemVuTaiLieu
                    {
                        nhiemVuId = entity.nhiemVuId,
                        taiLieuId = _model.taiLieuId
                    };
                    _db.NhiemVuTaiLieux.Add(_NVTL);
                    _db.SaveChanges();
                }
            }
            TempData["success"] = "Đóng nhiệm vụ thành công!";
            return RedirectToAction("QuanLyNhiemVu3");
        }

        /// <summary>
        /// Lấy tên nhiệm vụ
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LayTenNhiemVu(string searchKey)
        {
            var model = (from c in _db.NhiemVus
                         where c.nhiemVuName.StartsWith(searchKey)
                         select new { c.nhiemVuName });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Nhiệm vụ liên quan
        /// </summary>
        /// <param name="nhiemVuId"></param>
        /// <param name="danhMucId"></param>
        /// <returns></returns>
        public ActionResult NhiemVuLienQuan(int nhiemVuId, int danhMucId)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.nhiemVuId != nhiemVuId && x.danhMucId == danhMucId);
            return PartialView(model);
        }

        /// <summary>
        /// Lấy tên nhà thầu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetNhaThau(int Id)
        {
            IEnumerable<GoiThau> model = _db.GoiThaus.Where(x => x.nhiemVuId == Id);
            string _NameGia = "";
            foreach (GoiThau item in model)
            {
                _NameGia += item.nhaThauName + " - <b class=\"red\">" + item.giaThau.ToString("N0") + "</b> vnđ; ";
            }
            return _NameGia;
        }

        public List<NhiemVuView> GetAllNhiemVu(List<NhiemVuView> _curent, List<NhiemVuView> Data, int Id)
        {
            if (Id > 0)
            {
                IEnumerable<NhiemVuView> _model = Data.Where(x => x.nhiemVuId > 0 && x.danhMucId == Id);
                _curent.AddRange(_model);
                IEnumerable<NhiemVuView> entity = Data.Where(x => x.nhiemVuId == 0 && x.danhMucCha == Id);
                foreach (NhiemVuView item in entity)
                {
                    _curent = GetAllNhiemVu(_curent, Data, item.danhMucId);
                }
            }
            return _curent;
        }

        public string _TotalT(List<NhiemVuView> Data, int Id)
        {
            decimal _Total = 0;
            if (Id > 0)
            {
                List<NhiemVuView> _curent = new List<NhiemVuView>();
                _curent = GetAllNhiemVu(_curent, Data, Id);
                _curent = _curent.DistinctBy(x => x.nhiemVuId).ToList();
                _Total += _curent.Select(x => x.giaTriGiao).DefaultIfEmpty(0).Sum() ?? 0;
            }
            return _Total.ToString("N0");
        }
    }
}