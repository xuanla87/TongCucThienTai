using Aspose.Cells;
using codeTongCucThienTai.Models;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace codeTongCucThienTai.Controllers
{
    public class PrintController : Controller
    {
        private readonly TongCucThienTaiEntities _db;
        private readonly ApplicationDbContext _userConText;
        private TongCucThienTaiServices _services;

        public PrintController()
        {
            _db = new TongCucThienTaiEntities();
            _userConText = new ApplicationDbContext();
            _services = new TongCucThienTaiServices();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrintKeHoach(bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _DonViDuToan, bool? _NamKeHoach, bool? _GiaTriDeXuat, bool? _GiaTriDeXuat2, bool? _GhiChu, string TenNhiemVu, int? DonViDeXuat, int? DonViDuToan, int? NguonVon, int? NamKeHoach)
        {
            IEnumerable<DanhMucNhiemVu> danhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == null);
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai == 0);
            if (!string.IsNullOrEmpty(TenNhiemVu))
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.nhiemVuName.ToLower().Contains(TenNhiemVu.ToLower().Trim()) && nv.trangThai == 0
                           select dm);
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(TenNhiemVu.ToLower().Trim()));
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.namKeHoach == NamKeHoach.Value && nv.trangThai == 0
                           select dm);
                model = model.Where(x => x.namKeHoach == NamKeHoach.Value);
            }

            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.donViDeXuatId == DonViDeXuat.Value && nv.trangThai == 0
                           select dm);
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat.Value);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.donViDuToanId == DonViDuToan.Value && nv.trangThai == 0
                           select dm);
                model = model.Where(x => x.donViDuToanId == DonViDuToan.Value);
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           join w in _db.NguonVonNhiemVus on nv.nhiemVuId equals w.nhiemVuId
                           where w.nguonVonId == NguonVon.Value && nv.trangThai == 0
                           select dm).Distinct();
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q).Distinct();
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            danhMuc = danhMuc.DistinctBy(x => x.danhMucNhiemVuId);
            danhMuc = danhMuc.OrderByDescending(x => x.danhMucNhiemVuId);
            string _html = "";
            _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 18px; \">Danh Sách Kế Hoạch Nhiệm Vụ</div>";
            _html += "<div style=\"text-align: right; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 16px; \">ĐVT: VNĐ</div>";
            _html += "<table style=\" width:100%;border-top: 1px solid #333;border-left: 1px solid #333;\">";
            _html += "<tr>";
            if (_No.HasValue && _No.Value)
            {
                _html += "<td style=\" border:1px solid #333; font-weight: bold; text-align:center;\">STT</td>";
            }
            if (_Name.HasValue && _Name.Value)
            {
                _html += "<td style=\"  border:1px solid #333; font-weight: bold; text-align:center;\">Tên nhiệm vụ</td>";
            }
            if (_NguonVon.HasValue && _NguonVon.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Nguồn vốn</td>";
            }
            if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Đơn vị đề xuất</td>";
            }
            if (_DonViDuToan.HasValue && _DonViDuToan.Value)
            {
                _html += "<td style=\" border:1px solid #333;font-weight: bold; text-align:center;\">Đơn vị dự toán</td>";
            }
            if (_NamKeHoach.HasValue && _NamKeHoach.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Năm kế hoạch</td>";
            }
            if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị đề xuất của đơn vị</td>";
            }
            if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
            {
                _html += "<td style=\" border:1px solid #333;font-weight: bold; text-align:center;\">Giá trị đề xuất của tổng cục</td>";
            }
            if (_GhiChu.HasValue && _GhiChu.Value)
            {
                _html += "<td style=\"border:1px solid #333;  font-weight: bold; text-align:center;\">Ghi chú</td>";
            }
            _html += "</tr>";
            int count = 0;
            foreach (DanhMucNhiemVu _dmNv in danhMuc)
            {
                count++;
                _html += "<tr>";
                if (_No.HasValue && _No.Value)
                {
                    _html += "<td style=\"border:1px solid #333;  \">" + count + "</td>";
                }
                if (_Name.HasValue && _Name.Value)
                {
                    _html += "<td style=\"border:1px solid #333; font-weight:bold; text-transform:uppercase;\">" + _dmNv.danhMucNhiemVuName + "</td>";
                }
                if (_NguonVon.HasValue && _NguonVon.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_DonViDuToan.HasValue && _DonViDuToan.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_NamKeHoach.HasValue && _NamKeHoach.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GhiChu.HasValue && _GhiChu.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                _html += "</tr>";
                IEnumerable<NhiemVu> _NV = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai == 0 && x.danhMucId == _dmNv.danhMucNhiemVuId);
                int countItem = 0;
                foreach (NhiemVu item in _NV)
                {
                    countItem++;
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + count + "." + countItem + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nguonVonName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.donViDeXuatName + "</td>";
                    }
                    if (_DonViDuToan.HasValue && _DonViDuToan.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.donViDutoanName + "</td>";
                    }
                    if (_NamKeHoach.HasValue && _NamKeHoach.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.namKeHoach + "</td>";
                    }
                    if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.giaTriDeXuat.ToString("N0") + "</td>";
                    }
                    if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.giaTriDeXuat2.ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.ghiChu + "</td>";
                    }
                    _html += "</tr>";
                }
                _html += ChildKeHoachCon(_dmNv.danhMucNhiemVuId, count.ToString(), 1, _No, _Name, _NguonVon, _DonViDeXuat, _DonViDuToan, _NamKeHoach, _GiaTriDeXuat, _GiaTriDeXuat2, _GhiChu, TenNhiemVu, DonViDeXuat, DonViDuToan, NguonVon, NamKeHoach);
            }
            _html += "</table>";
            return Json(_html, JsonRequestBehavior.AllowGet);
        }
        public string ChildKeHoachCon(int? _DanhMucCha, string count, int level, bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _DonViDuToan, bool? _NamKeHoach, bool? _GiaTriDeXuat, bool? _GiaTriDeXuat2, bool? _GhiChu, string TenNhiemVu, int? DonViDeXuat, int? DonViDuToan, int? NguonVon, int? NamKeHoach)
        {
            IEnumerable<DanhMucNhiemVu> _DanhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == _DanhMucCha);
            _DanhMuc = _DanhMuc.OrderByDescending(x => x.danhMucNhiemVuId);
            string _html = "";
            int _item = 0;
            foreach (DanhMucNhiemVu _dmNv in _DanhMuc)
            {
                IEnumerable<NhiemVu> _NV = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai == 0 && x.danhMucId == _dmNv.danhMucNhiemVuId);
                _item++;
                _html += "<tr>";
                if (_No.HasValue && _No.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\">" + count + "." + _item + "</td>";
                }
                if (_Name.HasValue && _Name.Value)
                {
                    if (level == 1)
                    {
                        _html += "<td style=\"border:1px solid #333; font-weight:bold;\">" + _dmNv.danhMucNhiemVuName + "</td>";
                    }
                    if (level == 2)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + _dmNv.danhMucNhiemVuName + "</td>";
                    }
                    if (level == 3)
                    {
                        _html += "<td style=\"border:1px solid #333; font-style:italic;\">" + _dmNv.danhMucNhiemVuName + "</td>";
                    }
                }
                if (_NguonVon.HasValue && _NguonVon.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_DonViDuToan.HasValue && _DonViDuToan.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_NamKeHoach.HasValue && _NamKeHoach.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GhiChu.HasValue && _GhiChu.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                _html += "</tr>";
                int countItem = 0;
                foreach (NhiemVu item in _NV)
                {
                    countItem++;
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + count + "." + countItem + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nguonVonName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.donViDeXuatName + "</td>";
                    }
                    if (_DonViDuToan.HasValue && _DonViDuToan.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.donViDutoanName + "</td>";
                    }
                    if (_NamKeHoach.HasValue && _NamKeHoach.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.namKeHoach + "</td>";
                    }
                    if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.giaTriDeXuat.ToString("N0") + "</td>";
                    }
                    if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.giaTriDeXuat2.ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.ghiChu + "</td>";
                    }
                    _html += "</tr>";
                }
                _html += ChildKeHoachCon(_dmNv.danhMucNhiemVuId, count + "." + _item, level++, _No, _Name, _NguonVon, _DonViDeXuat, _DonViDuToan, _NamKeHoach, _GiaTriDeXuat, _GiaTriDeXuat2, _GhiChu, TenNhiemVu, DonViDeXuat, DonViDuToan, NguonVon, NamKeHoach);
            }
            return _html;
        }
        public ActionResult PrintNhiemVu(bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _GiaTriGiao, bool? _GiaTriDuyet, bool? _GhiChu, bool? _TrangThai, string TenNhiemVu, int? DonViDeXuat, int? TrangThai, int? NguonVon, int? NamKeHoach, int? DonViDuToan)
        {
            IEnumerable<DanhMucNhiemVu> danhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == null);
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            if (!string.IsNullOrEmpty(TenNhiemVu))
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.nhiemVuName.ToLower().Contains(TenNhiemVu.ToLower().Trim()) && nv.trangThai > 0
                           select dm);
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(TenNhiemVu.ToLower().Trim()));
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.namKeHoach == NamKeHoach.Value && nv.trangThai > 0
                           select dm);
                model = model.Where(x => x.namKeHoach == NamKeHoach.Value);
            }

            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.donViDeXuatId == DonViDeXuat.Value && nv.trangThai > 0
                           select dm);
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat.Value);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.donViDuToanId == DonViDuToan.Value && nv.trangThai > 0
                           select dm).Distinct();
                model = model.Where(x => x.donViDuToanId == DonViDuToan.Value);
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           join w in _db.NguonVonNhiemVus on nv.nhiemVuId equals w.nhiemVuId
                           where w.nguonVonId == NguonVon.Value && nv.trangThai > 0
                           select dm).Distinct();
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q).Distinct();
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            danhMuc = danhMuc.DistinctBy(x => x.danhMucNhiemVuId);
            string _html = "";
            _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 18px; \">Danh Sách Nhiệm Vụ</div>";
            _html += "<div style=\"text-align: right; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 16px; \">ĐVT: VNĐ</div>";
            _html += "<table style=\" width:100%;border-top: 1px solid #333;border-left: 1px solid #333;\">";
            _html += "<tr>";
            if (_No.HasValue && _No.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">STT</td>";
            }
            if (_Name.HasValue && _Name.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Tên nhiệm vụ</td>";
            }
            if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Đơn vị thực hiện</td>";
            }
            if (_NguonVon.HasValue && _NguonVon.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Nguồn vốn</td>";
            }
            if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị giao</td>";
            }
            if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị duyệt</td>";
            }
            if (_GhiChu.HasValue && _GhiChu.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Ghi chú</td>";
            }
            if (_TrangThai.HasValue && _TrangThai.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Trạng thái</td>";
            }
            _html += "</tr>";
            int count = 0;
            foreach (DanhMucNhiemVu item in danhMuc)
            {
                count++;
                _html += "<tr>";
                if (_No.HasValue && _No.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\">" + count + "</td>";
                }
                if (_Name.HasValue && _Name.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\">" + item.danhMucNhiemVuName + "</td>";
                }
                if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_NguonVon.HasValue && _NguonVon.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GhiChu.HasValue && _GhiChu.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_TrangThai.HasValue && _TrangThai.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                _html += "</tr>";
                IEnumerable<NhiemVu> _NV = _db.NhiemVus.Where(x => x.danhMucId == item.danhMucNhiemVuId && x.isTrash == false && x.trangThai > 0);
                int _itemNV = 0;
                foreach (NhiemVu itemNV in _NV)
                {
                    _itemNV++;
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + count + "." + _itemNV + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.nhiemVuName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.donViDeXuatName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.nguonVonName + "</td>";
                    }
                    if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + (itemNV.giaTriGiao ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + (itemNV.giaTriDuyet ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.ghiChu + "</td>";
                    }
                    if (_TrangThai.HasValue && _TrangThai.Value)
                    {
                        if (itemNV.trangThai == 1)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Chưa triển khai</td>";
                        }

                        if (itemNV.trangThai == 2)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã trình đề cương dự toán</td>";
                        }

                        if (itemNV.trangThai == 3)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt đề cương dự toán</td>";
                        }

                        if (itemNV.trangThai == 4)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã trình kế hoạch lựa chọn nhà thầu</td>";
                        }

                        if (itemNV.trangThai == 5)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kế hoạch nhà thầu</td>";
                        }

                        if (itemNV.trangThai == 6)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kết quả lựa chọn nhà thầu</td>";
                        }

                        if (itemNV.trangThai == 7)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã ký hợp đồng</td>";
                        }

                        if (itemNV.trangThai == 8)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã kết thúc</td>";
                        }
                    }
                    _html += "</tr>";
                }
                _html += ChildQuanLyNhiemVuCon(item.danhMucNhiemVuId, count.ToString(), 1, _No, _Name, _NguonVon, _DonViDeXuat, _GiaTriGiao, _GiaTriDuyet, _GhiChu, _TrangThai, TenNhiemVu, DonViDeXuat, TrangThai, NguonVon, NamKeHoach, DonViDuToan);
            }
            _html += "</table>";
            return Json(_html, JsonRequestBehavior.AllowGet);
        }
        public string ChildQuanLyNhiemVuCon(int? _DanhMucCha, string count, int level, bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _GiaTriGiao, bool? _GiaTriDuyet, bool? _GhiChu, bool? _TrangThai, string TenNhiemVu, int? DonViDeXuat, int? TrangThai, int? NguonVon, int? NamKeHoach, int? DonViDuToan)
        {
            IEnumerable<DanhMucNhiemVu> _DanhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == _DanhMucCha);
            string _html = "";
            int _item = 0;
            foreach (DanhMucNhiemVu _dmNv in _DanhMuc)
            {
                _item++;
                IEnumerable<NhiemVu> _NV = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0 && x.danhMucId == _dmNv.danhMucNhiemVuId);
                _html += "<tr>";
                if (_No.HasValue && _No.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\">" + count + "." + _item + "</td>";
                }
                if (_Name.HasValue && _Name.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\">" + _dmNv.danhMucNhiemVuName + "</td>";
                }
                if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_NguonVon.HasValue && _NguonVon.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_GhiChu.HasValue && _GhiChu.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                if (_TrangThai.HasValue && _TrangThai.Value)
                {
                    _html += "<td style=\"border:1px solid #333;\"></td>";
                }
                _html += "</tr>";
                int _itemNV = 0;
                foreach (NhiemVu itemNV in _NV)
                {
                    _itemNV++;
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + count + "." + _item + "." + _itemNV + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.nhiemVuName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.donViDeXuatName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.nguonVonName + "</td>";
                    }
                    if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + (itemNV.giaTriGiao ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + (itemNV.giaTriDuyet ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + itemNV.ghiChu + "</td>";
                    }
                    if (_TrangThai.HasValue && _TrangThai.Value)
                    {
                        if (itemNV.trangThai == 1)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Chưa triển khai</td>";
                        }

                        if (itemNV.trangThai == 2)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã trình đề cương dự toán</td>";
                        }

                        if (itemNV.trangThai == 3)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt đề cương dự toán</td>";
                        }

                        if (itemNV.trangThai == 4)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã trình kế hoạch lựa chọn nhà thầu</td>";
                        }

                        if (itemNV.trangThai == 5)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kế hoạch nhà thầu</td>";
                        }

                        if (itemNV.trangThai == 6)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kết quả lựa chọn nhà thầu</td>";
                        }

                        if (itemNV.trangThai == 7)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã ký hợp đồng</td>";
                        }

                        if (itemNV.trangThai == 8)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã kết thúc</td>";
                        }
                    }
                    _html += "</tr>";
                }
                _html += ChildQuanLyNhiemVuCon(_dmNv.danhMucNhiemVuId, count + "." + _item, level++, _No, _Name, _NguonVon, _DonViDeXuat, _GiaTriGiao, _GiaTriDuyet, _GhiChu, _TrangThai, TenNhiemVu, DonViDeXuat, TrangThai, NguonVon, NamKeHoach, DonViDuToan);
            }
            return _html;
        }
        public ActionResult PrintThongKe(bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _GiaTriGiao, bool? _GiaTriDuyet, bool? _GhiChu, bool? _TrangThai, bool? _GiaTriGiaiNgan, bool? _GiaTriQuyetToan, bool? _GiaTriHopDong, int? DonViThucHien, int? DanhMucNhiemVu, int? NguonVon, int? NamKeHoach)
        {
            IEnumerable<DanhMucNhiemVu> danhMuc = _db.DanhMucNhiemVus.Where(x => x.isTrash == false);
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach.Value);
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.namKeHoach == NamKeHoach
                           select dm);
            }

            if (DonViThucHien.HasValue && DonViThucHien > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           where nv.donViDeXuatId == DonViThucHien
                           select dm);
                model = model.Where(x => x.donViDeXuatId == DonViThucHien.Value);
            }

            if (DanhMucNhiemVu.HasValue && DanhMucNhiemVu > 0)
            {
                danhMuc = danhMuc.Where(x => x.danhMucNhiemVuId == DanhMucNhiemVu);
                model = model.Where(x => x.danhMucId == DanhMucNhiemVu.Value);
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                danhMuc = (from dm in danhMuc
                           join nv in _db.NhiemVus on dm.danhMucNhiemVuId equals nv.danhMucId
                           join nvon in _db.NguonVonNhiemVus on nv.nhiemVuId equals nvon.nhiemVuId
                           where nvon.nguonVonId == NguonVon.Value
                           select dm);
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }

            model = model.OrderBy(x => x.namKeHoach);
            decimal _totalGiao = 0;
            decimal _totalDuyet = 0;
            decimal _totalHopDong = 0;
            decimal _totalGiaiNgan = 0;
            decimal _totalQuyetToan = 0;
            _totalDuyet = model.Select(x => x.giaTriTrungThau).DefaultIfEmpty(0).Sum() ?? 0;
            _totalGiao = model.Select(x => x.giaTriGiao).DefaultIfEmpty(0).Sum() ?? 0;
            _totalHopDong = model.Select(x => x.giaTriHopDong).DefaultIfEmpty(0).Sum() ?? 0;
            _totalQuyetToan = model.Select(x => x.giaTriQuyetToan).DefaultIfEmpty(0).Sum() ?? 0;
            _totalGiaiNgan = GetGiaiNgan2(model);
            ViewBag.TotalGiaiNgan = _totalGiaiNgan.ToString("N0");
            ViewBag.TotalDuyet = _totalDuyet.ToString("N0");
            ViewBag.TotalGiao = _totalGiao.ToString("N0");
            ViewBag.TotalHopDong = _totalHopDong.ToString("N0");
            ViewBag.TotalQuyetToan = _totalQuyetToan.ToString("N0");

            string _html = "";
            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; height: 40px; font-size: 18px; \">Báo Cáo Nhiệm Vụ Năm " + NamKeHoach.Value + "</div>";
            }
            else
            {
                _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; height: 40px; font-size: 18px; \">Báo Cáo Nhiệm Vụ</div>";
            }

            _html += "<div style=\"text-align: right; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 16px; \">ĐVT: VNĐ</div>";
            _html += "<table style=\" width:100%;\">";
            _html += "<tr>";
            if (_No.HasValue && _No.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">STT</td>";
            }
            if (_Name.HasValue && _Name.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Tên nhiệm vụ</td>";
            }
            if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Đơn vị thực hiện</td>";
            }
            if (_NguonVon.HasValue && _NguonVon.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Nguồn vốn</td>";
            }
            if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold;  text-align:center;\">Giá trị giao <div style=\"text-align:right; \">" + ViewBag.TotalGiao + "</div></td>";
            }
            if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị duyệt <div style=\"text-align:right; \">" + ViewBag.TotalDuyet + "</div></td>";
            }
            if (_TrangThai.HasValue && _TrangThai.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Trạng thái</td>";
            }
            if (_GiaTriHopDong.HasValue && _GiaTriHopDong.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị hợp đồng<div style=\"text-align:right; \">" + ViewBag.TotalHopDong + "</div></td>";
            }
            if (_GiaTriGiaiNgan.HasValue && _GiaTriGiaiNgan.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị giải ngân<div style=\"text-align:right; \">" + ViewBag.TotalGiaiNgan + "</div></td>";
            }
            if (_GiaTriQuyetToan.HasValue && _GiaTriQuyetToan.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold;  text-align:center;\">Giá trị quyết toán<div style=\"text-align:right; \">" + ViewBag.TotalQuyetToan + "</div></td>";
            }
            if (_GhiChu.HasValue && _GhiChu.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; max-width: 100px; text-align:center;\">Ghi chú</td>";
            }

            _html += "</tr>";

            int count = 0;
            foreach (DanhMucNhiemVu itemDm in danhMuc)
            {
                IEnumerable<NhiemVu> model2 = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0 && x.danhMucId == itemDm.danhMucNhiemVuId);
                if (NamKeHoach.HasValue && NamKeHoach > 0)
                {
                    model2 = model2.Where(x => x.namKeHoach == NamKeHoach.Value);
                }

                if (DonViThucHien.HasValue && DonViThucHien > 0)
                {
                    model2 = model2.Where(x => x.donViDeXuatId == DonViThucHien.Value);
                }
                if (NguonVon.HasValue && NguonVon > 0)
                {

                    model2 = (from q in model2 join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
                }
                model2 = model2.OrderBy(x => x.namKeHoach);
                decimal _totalGiao2 = 0;
                decimal _totalDuyet2 = 0;
                decimal _totalHopDong2 = 0;
                decimal _totalGiaiNgan2 = 0;
                decimal _totalQuyetToan2 = 0;

                if (model2 != null && model2.Count() > 0)
                {
                    _totalDuyet2 = model2.Select(x => x.giaTriTrungThau).DefaultIfEmpty(0).Sum() ?? 0;
                    _totalGiao2 = model2.Select(x => x.giaTriGiao).DefaultIfEmpty(0).Sum() ?? 0;
                    _totalHopDong2 = model2.Select(x => x.giaTriHopDong).DefaultIfEmpty(0).Sum() ?? 0;
                    _totalQuyetToan2 = model2.Select(x => x.giaTriQuyetToan).DefaultIfEmpty(0).Sum() ?? 0;
                    _totalGiaiNgan2 = GetGiaiNgan2(model2);
                }
                string _sttotalGiao2 = _totalGiao2.ToString("N0");
                string _sttotalDuyet2 = _totalDuyet2.ToString("N0");
                string _sttotalHopDong2 = _totalHopDong2.ToString("N0");
                string _sttotalGiaiNgan2 = _totalGiaiNgan2.ToString("N0");
                string _sttotalQuyetToan2 = _totalQuyetToan2.ToString("N0");
                _html += "<tr>";
                if (_No.HasValue && _No.Value)
                {
                    _html += "<td style=\"\"></td>";
                }
                if (_Name.HasValue && _Name.Value)
                {
                    _html += "<td style=\" font-weight: bold;\">" + itemDm.danhMucNhiemVuName + "</td>";
                }
                if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                {
                    _html += "<td style=\"\"></td>";
                }
                if (_NguonVon.HasValue && _NguonVon.Value)
                {
                    _html += "<td style=\"\"></td>";
                }
                if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                {
                    _html += "<td style=\" text-align:right;\">" + _sttotalGiao2 + "</td>";
                }
                if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                {
                    _html += "<td style=\" text-align:right;\">" + _sttotalDuyet2 + "</td>";
                }
                if (_TrangThai.HasValue && _TrangThai.Value)
                {
                    _html += "<td style=\"\"></td>";
                }
                if (_GiaTriHopDong.HasValue && _GiaTriHopDong.Value)
                {
                    _html += "<td style=\" text-align:right;\">" + _sttotalHopDong2 + "</td>";
                }
                if (_GiaTriGiaiNgan.HasValue && _GiaTriGiaiNgan.Value)
                {
                    _html += "<td style=\" text-align:right;\">" + _sttotalGiaiNgan2 + "</td>";
                }
                if (_GiaTriQuyetToan.HasValue && _GiaTriQuyetToan.Value)
                {
                    _html += "<td style=\" text-align:right;\">" + _sttotalQuyetToan2 + "</td>";
                }
                if (_GhiChu.HasValue && _GhiChu.Value)
                {
                    _html += "<td style=\"\"></td>";
                }
                _html += "</tr>";
                foreach (NhiemVu item in model2)
                {
                    count++;
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"\">" + count + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"\">" + item.nhiemVuName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"\">" + item.donViDeXuatName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"\">" + item.nguonVonName + "</td>";
                    }
                    if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                    {
                        _html += "<td style=\" text-align:right;\">" + (item.giaTriGiao ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                    {
                        _html += "<td style=\" text-align:right;\">" + (item.giaTriDuyet ?? 0).ToString("N0") + "</td>";
                    }

                    if (_TrangThai.HasValue && _TrangThai.Value)
                    {
                        if (item.trangThai == 1)
                        {
                            _html += "<td style=\"\">Chưa triển khai</td>";
                        }

                        if (item.trangThai == 2)
                        {
                            _html += "<td style=\"\">Đã trình đề cương dự toán</td>";
                        }

                        if (item.trangThai == 3)
                        {
                            _html += "<td style=\"\">Đã phê duyệt đề cương dự toán</td>";
                        }

                        if (item.trangThai == 4)
                        {
                            _html += "<td style=\"\">Đã trình kế hoạch lựa chọn nhà thầu</td>";
                        }

                        if (item.trangThai == 5)
                        {
                            _html += "<td style=\"\">Đã phê duyệt kế hoạch nhà thầu</td>";
                        }

                        if (item.trangThai == 6)
                        {
                            _html += "<td style=\"\">Đã phê duyệt kết quả lựa chọn nhà thầu</td>";
                        }

                        if (item.trangThai == 7)
                        {
                            _html += "<td style=\"\">Đã ký hợp đồng</td>";
                        }

                        if (item.trangThai == 8)
                        {
                            _html += "<td style=\"\">Đã kết thúc</td>";
                        }
                    }
                    if (_GiaTriHopDong.HasValue && _GiaTriHopDong.Value)
                    {
                        _html += "<td style=\" text-align:right;\">" + (item.giaTriHopDong ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GiaTriGiaiNgan.HasValue && _GiaTriGiaiNgan.Value)
                    {
                        _html += "<td style=\" text-align:right;\">" + GetGiaiNgan(item.nhiemVuId) + "</td>";
                    }
                    if (_GiaTriQuyetToan.HasValue && _GiaTriQuyetToan.Value)
                    {
                        _html += "<td style=\" text-align:right;\">" + (item.giaTriQuyetToan ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"\">" + item.ghiChu + "</td>";
                    }
                    _html += "</tr>";
                }
            }
            _html += "</table>";
            return Json(_html, JsonRequestBehavior.AllowGet);
        }

        public void ExportExcelThongKe(bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _GiaTriGiao, bool? _GiaTriDuyet, bool? _GhiChu, bool? _TrangThai, bool? _GiaTriGiaiNgan, bool? _GiaTriQuyetToan, bool? _GiaTriHopDong, int? DonViThucHien, int? DanhMucNhiemVu, int? NguonVon, int? NamKeHoach)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach.Value);
            }

            if (DonViThucHien.HasValue && DonViThucHien > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViThucHien.Value);
            }

            if (DanhMucNhiemVu.HasValue && DanhMucNhiemVu > 0)
            {
                model = model.Where(x => x.danhMucId == DanhMucNhiemVu.Value);
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }

            model = model.OrderBy(x => x.namKeHoach);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            ws.Cells["A1"].Value = "";
        }

        public ActionResult ImportData()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ImportData(HttpPostedFileBase fileAttach)
        {
            if (fileAttach == null || fileAttach.ContentLength == 0)
            {
                TempData["error"] = "Bạn chưa chọn tệp tin!";
                return View();
            }
            else
            {
                if (fileAttach.FileName.EndsWith("xls") || fileAttach.FileName.EndsWith("xlsx"))
                {
                    bool exists = System.IO.Directory.Exists(Server.MapPath("~/DuLieuImport"));
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/DuLieuImport"));
                    }

                    string path = Server.MapPath("~/DuLieuImport/" + fileAttach.FileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fileAttach.SaveAs(path);
                    }
                    DataTable _DT = ImportExcels(path);
                    List<NhiemVu> _listImport = new List<NhiemVu>();
                    List<NhiemVuModel> _listNoImport = new List<NhiemVuModel>();
                    for (int row = 1; row < _DT.Rows.Count; row++)
                    {
                        string _NhiemVuName = _DT.Rows[row][0].ToString();
                        string _danhMucName = _DT.Rows[row][1].ToString();
                        string _NguonVon = _DT.Rows[row][2].ToString();
                        string _DonViDeXuat = _DT.Rows[row][3].ToString();
                        string _DonViDuToan = _DT.Rows[row][4].ToString();
                        string _NamKeHoach = _DT.Rows[row][5].ToString();
                        string _GiaTriDeXuat = _DT.Rows[row][6].ToString();
                        string _GiaTriDeXuat2 = _DT.Rows[row][7].ToString();
                        string _GhiChu = _DT.Rows[row][8].ToString();
                        bool _chekName = _services.CheckExitsNameNhiemVu(null, _NhiemVuName, 0, 0, 0, _NguonVon);
                        if (!_chekName)
                        {
                            int _DanhMucId = 0;
                            int _DonViDeXuatId = 0;
                            int _DonViDuToanId = 0;
                            int _NamKeHoachId = 0;
                            List<int> _LIDNguonVon = new List<int>();
                            decimal _GiaDeXuat = 0;
                            decimal _GiaDeXuat2 = 0;
                            if (!string.IsNullOrEmpty(_GiaTriDeXuat))
                            {
                                decimal.TryParse(_GiaTriDeXuat, out _GiaDeXuat);
                            }
                            if (!string.IsNullOrEmpty(_GiaTriDeXuat2))
                            {
                                decimal.TryParse(_GiaTriDeXuat2, out _GiaDeXuat2);
                            }
                            if (!string.IsNullOrEmpty(_danhMucName))
                            {
                                _DanhMucId = _services.GetAndCreateDanhMucByName(_danhMucName);
                            }

                            if (!string.IsNullOrEmpty(_DonViDeXuat))
                            {
                                _DonViDeXuatId = _services.GetAndCreateDanhMucDonViByName(_DonViDeXuat);
                            }

                            if (!string.IsNullOrEmpty(_DonViDuToan))
                            {
                                _DonViDuToanId = _services.GetAndCreateDanhMucDonViByName(_DonViDuToan);
                            }

                            if (!string.IsNullOrEmpty(_NamKeHoach))
                            {
                                int.TryParse(_NamKeHoach, out _NamKeHoachId);
                            }
                            string[] _ListNguonvon = _NguonVon.Split(',');
                            foreach (string item in _ListNguonvon)
                            {
                                DanhMucNguonVon _enNguonVon = _db.DanhMucNguonVons.FirstOrDefault(x => x.danhMucNguonVonName.ToLower() == item.ToLower().Trim() && x.isTrash == false);
                                if (_enNguonVon != null)
                                {
                                    _LIDNguonVon.Add(_enNguonVon.danhMucNguonVonId);
                                }
                                else
                                {
                                    _LIDNguonVon.Add(0);
                                }
                            }
                            if (_DanhMucId > 0 && _LIDNguonVon.Contains(0) == false)
                            {
                                NhiemVu entity = new NhiemVu
                                {
                                    createTime = DateTime.Now,
                                    updateTime = DateTime.Now,
                                    userCreate = User.Identity.Name,
                                    userUpdate = User.Identity.Name,
                                    trangThai = 0,
                                    nhiemVuName = _NhiemVuName,
                                    namKeHoach = _NamKeHoachId,
                                    isTrash = false,
                                    giaTriDeXuat = _GiaDeXuat,
                                    giaTriDeXuat2 = _GiaDeXuat2,
                                    ghiChu = _GhiChu,
                                    danhMucId = _DanhMucId,
                                    donViDeXuatId = _DonViDeXuatId,
                                    donViDuToanId = _DonViDuToanId,
                                    donViDeXuatName = _DonViDeXuat,
                                    donViDutoanName = _DonViDuToan
                                };
                                _db.NhiemVus.Add(entity);
                                _db.SaveChanges();
                                _listImport.Add(entity);
                                foreach (int item in _LIDNguonVon)
                                {
                                    DanhMucNguonVon _enNguonVon = _db.DanhMucNguonVons.FirstOrDefault(x => x.danhMucNguonVonId == item && x.isTrash == false);
                                    if (_enNguonVon != null)
                                    {
                                        NguonVonNhiemVu entitys = new NguonVonNhiemVu
                                        {
                                            nguonVonId = _enNguonVon.danhMucNguonVonId,
                                            nhiemVuId = entity.nhiemVuId

                                        };
                                        entity.nguonVonName += "," + _enNguonVon.danhMucNguonVonName;
                                        _db.NguonVonNhiemVus.Add(entitys);
                                        _db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(_GiaTriDeXuat))
                                {
                                    decimal.TryParse(_GiaTriDeXuat, out _GiaDeXuat);
                                }
                                if (!string.IsNullOrEmpty(_GiaTriDeXuat2))
                                {
                                    decimal.TryParse(_GiaTriDeXuat2, out _GiaDeXuat2);
                                }
                                if (!string.IsNullOrEmpty(_NamKeHoach))
                                {
                                    int.TryParse(_NamKeHoach, out _NamKeHoachId);
                                }

                                NhiemVuModel entity = new NhiemVuModel
                                {
                                    createTime = DateTime.Now,
                                    updateTime = DateTime.Now,
                                    userCreate = User.Identity.Name,
                                    userUpdate = User.Identity.Name,
                                    trangThai = 0,
                                    nhiemVuName = _NhiemVuName,
                                    namKeHoach = _NamKeHoachId,
                                    isTrash = false,
                                    giaTriDeXuat = _GiaTriDeXuat,
                                    giaTriDeXuat2 = _GiaTriDeXuat2,
                                    danhMucName = _danhMucName,
                                    ghiChu = _GhiChu,
                                    danhMucId = 0,
                                    donViDeXuatId = 0,
                                    donViDuToanId = 0,
                                    donViDeXuatName = _DonViDeXuat,
                                    donViDutoanName = _DonViDuToan,
                                    nguonVonName = _NguonVon
                                };
                                _listNoImport.Add(entity);
                            }
                        }
                        else
                        {
                            int _NamKeHoachId = 0;
                            decimal _GiaDeXuat = 0;
                            decimal _GiaDeXuat2 = 0;
                            if (!string.IsNullOrEmpty(_GiaTriDeXuat))
                            {
                                decimal.TryParse(_GiaTriDeXuat, out _GiaDeXuat);
                            }
                            if (!string.IsNullOrEmpty(_GiaTriDeXuat2))
                            {
                                decimal.TryParse(_GiaTriDeXuat2, out _GiaDeXuat2);
                            }
                            if (!string.IsNullOrEmpty(_NamKeHoach))
                            {
                                int.TryParse(_NamKeHoach, out _NamKeHoachId);
                            }

                            NhiemVuModel entity = new NhiemVuModel
                            {
                                createTime = DateTime.Now,
                                updateTime = DateTime.Now,
                                userCreate = User.Identity.Name,
                                userUpdate = User.Identity.Name,
                                trangThai = 0,
                                nhiemVuName = _NhiemVuName,
                                namKeHoach = _NamKeHoachId,
                                isTrash = false,
                                giaTriDeXuat = _GiaTriDeXuat,
                                giaTriDeXuat2 = _GiaTriDeXuat2,
                                danhMucName = _danhMucName,
                                ghiChu = _GhiChu,
                                danhMucId = 0,
                                donViDeXuatId = 0,
                                donViDuToanId = 0,
                                donViDeXuatName = _DonViDeXuat,
                                donViDutoanName = _DonViDuToan,
                                nguonVonName = _NguonVon
                            };
                            _listNoImport.Add(entity);
                        }
                    }
                    ViewBag.ListSuccess = _listImport;
                    ViewBag.ListError = _listNoImport;
                    TempData["success"] = "Import kế hoạch thành công!";
                    return View();
                }
                else
                {
                    TempData["error"] = "Tệp tin không đúng định dạng!";
                    return View();
                }
            }
        }

        public DataSet ImportExcel(string filePath)
        {
            string file = filePath;
            DataSet ds = new DataSet();
            string fileExtension = System.IO.Path.GetExtension(file);

            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                string fileLocation = filePath;

                string excelConnectionString = string.Empty;
                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                //connection String for xls file format.
                if (fileExtension == ".xls")
                {
                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                //connection String for xlsx file format.
                else if (fileExtension == ".xlsx")
                {
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }
                //Create Connection to Excel work book and add oledb namespace
                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                excelConnection.Open();
                DataTable dt = new DataTable();

                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string[] excelSheets = new string[dt.Rows.Count];
                int t = 0;
                OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                //excel data saves in temp file here.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[t] = row["TABLE_NAME"].ToString();
                    t++;
                }
                try
                {
                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    excelConnection.Close();
                    excelConnection1.Close();
                }
            }

            if (fileExtension.ToString().ToLower().Equals(".xml"))
            {
                string fileLocation = filePath;
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                ds.ReadXml(xmlreader);
                xmlreader.Close();
            }
            return ds;
        }

        public ActionResult ChiTietGiaiNgan(int Id)
        {
            IEnumerable<GiaiNgan> model = _db.GiaiNgans.Where(x => x.nhiemVuId == Id);
            decimal _GiaTri = 0;
            NhiemVu entity = _db.NhiemVus.Find(Id);
            if (entity != null)
            {
                _GiaTri = entity.giaTriTrungThau.GetValueOrDefault();
            }
            ViewBag.GiaTri = _GiaTri;
            model = model.OrderByDescending(x => x.thoiGianThanhToan);
            return PartialView(model.ToList());
        }

        public decimal GetGiaiNgan2(IEnumerable<NhiemVu> _input)
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

        public string GetGiaiNgan(int Id)
        {
            IQueryable<GiaiNgan> model = _db.GiaiNgans.Where(x => x.nhiemVuId == Id);
            model = model.OrderByDescending(x => x.thoiGianThanhToan);
            string _Total = (model.Select(x => x.giaTriThanhToan).DefaultIfEmpty(0).Sum() ?? 0).ToString("N0");
            return _Total;
        }

        public DataTable ImportExcels(string filePath)
        {
            DataTable dt = new DataTable();
            Workbook wb = new Workbook(filePath);
            dt = wb.Worksheets[0].Cells.ExportDataTable(0, 0, wb.Worksheets[0].Cells.MaxDataRow + 2, wb.Worksheets[0].Cells.MaxDataColumn + 1, true);
            return dt;
        }

        public ActionResult PrintNhiemVu2(bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _GiaTriGiao, bool? _GiaTriDuyet, bool? _GhiChu, bool? _TrangThai, string TenNhiemVu, int? DonViDeXuat, int? TrangThai, int? NguonVon, int? NamKeHoach, int? DonViDuToan)
        {
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            if (!string.IsNullOrEmpty(TenNhiemVu))
            {
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(TenNhiemVu.ToLower().Trim()));
            }
            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {

                model = model.Where(x => x.namKeHoach == NamKeHoach.Value);
            }
            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {

                model = model.Where(x => x.donViDeXuatId == DonViDeXuat.Value);
            }
            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {

                model = model.Where(x => x.donViDuToanId == DonViDuToan.Value);
            }
            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q).Distinct();
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            decimal _totalGiaTriDuyet = 0;
            decimal _totalGiaTriGiao = 0;
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
                IOrderedEnumerable<NhiemVu> _listTem = _ListNV.OrderBy(x => x.createTime);
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
            string _html = "";
            _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 18px; \">Danh Sách Nhiệm Vụ</div>";
            _html += "<div style=\"text-align: right; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 16px; \">ĐVT: VNĐ</div>";
            _html += "<table style=\" width:100%;border-collapse: collapse;\">";
            _html += "<tr>";
            if (_No.HasValue && _No.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">STT</td>";
            }
            if (_Name.HasValue && _Name.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Tên nhiệm vụ</td>";
            }
            if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Đơn vị thực hiện</td>";
            }
            if (_NguonVon.HasValue && _NguonVon.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Nguồn vốn</td>";
            }
            if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị giao <br/>" + _totalGiaTriGiao.ToString("N0") + "</td>";
            }
            if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị duyệt<br/>" + _totalGiaTriDuyet.ToString("N0") + "</td>";
            }
            if (_GhiChu.HasValue && _GhiChu.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Ghi chú</td>";
            }
            if (_TrangThai.HasValue && _TrangThai.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Trạng thái</td>";
            }
            _html += "</tr>";
            foreach (NhiemVuView item in _tem3)
            {
                if (item.nhiemVuId > 0)
                {
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:center;\">" + item.STT + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.donViDeXuatName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nguonVonName + "</td>";
                    }
                    if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align: right;\">" + (item.giaTriGiao ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align: right;\">" + (item.giaTriTrungThau ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.ghiChu + "</td>";
                    }
                    if (_TrangThai.HasValue && _TrangThai.Value)
                    {
                        if (item.trangThai == 1)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Chưa triển khai</td>";
                        }

                        if (item.trangThai == 2)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã trình đề cương dự toán</td>";
                        }

                        if (item.trangThai == 3)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt đề cương dự toán</td>";
                        }

                        if (item.trangThai == 4)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã trình kế hoạch lựa chọn nhà thầu</td>";
                        }

                        if (item.trangThai == 5)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kế hoạch nhà thầu</td>";
                        }

                        if (item.trangThai == 6)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kết quả lựa chọn nhà thầu</td>";
                        }

                        if (item.trangThai == 7)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã ký hợp đồng</td>";
                        }

                        if (item.trangThai == 8)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã kết thúc</td>";
                        }
                    }
                    _html += "</tr>";
                }
                else
                {
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.STT + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalDuToanGiao(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalDuToanDuyet(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_TrangThai.HasValue && _TrangThai.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    _html += "</tr>";
                }
            }
            _html += "</table>";
            return Json(_html, JsonRequestBehavior.AllowGet);
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

        public ActionResult PrintKeHoach2(bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _DonViDuToan, bool? _NamKeHoach, bool? _GiaTriDeXuat, bool? _GiaTriDeXuat2, bool? _GhiChu, string TenNhiemVu, int? DonViDeXuat, int? DonViDuToan, int? NguonVon, int? NamKeHoach)
        {
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai == 0);
            if (!string.IsNullOrEmpty(TenNhiemVu))
            {
                model = model.Where(x => x.nhiemVuName.ToLower().Contains(TenNhiemVu.ToLower().Trim()));
            }

            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach.Value);
            }

            if (DonViDeXuat.HasValue && DonViDeXuat > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViDeXuat.Value);
            }

            if (DonViDuToan.HasValue && DonViDuToan > 0)
            {
                model = model.Where(x => x.donViDuToanId == DonViDuToan.Value);
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }
            model = model.DistinctBy(x => x.nhiemVuId);
            List<NhiemVu> _ListNV = model.ToList();
            List<NhiemVuView> _tem = new List<NhiemVuView>();
            decimal _TotalDeXuat = 0;
            decimal _TotalDeXuat2 = 0;
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
                    List<NhiemVuView> _new = GetCha(_item.danhMucId, _tem);
                    _tem = _new;
                }
                _TotalDeXuat += item.giaTriDeXuat;
                _TotalDeXuat2 += item.giaTriDeXuat2;
            }
            List<NhiemVuView> _tem2 = new List<NhiemVuView>();
            foreach (NhiemVuView itemTem in _tem)
            {
                _tem2.Add(itemTem);
                IOrderedEnumerable<NhiemVu> _listTem = _ListNV.OrderBy(x => x.createTime);
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
            string _html = "";

            _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 18px; \">Danh Sách Kế Hoạch Nhiệm Vụ</div>";
            _html += "<div style=\"text-align: right; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 16px; \">ĐVT: VNĐ</div>";
            _html += "<table style=\" width:100%;border-collapse: collapse;\">";
            _html += "<tr>";
            if (_No.HasValue && _No.Value)
            {
                _html += "<td style=\" border:1px solid #333; font-weight: bold; text-align:center;\">STT</td>";
            }
            if (_Name.HasValue && _Name.Value)
            {
                _html += "<td style=\"  border:1px solid #333; font-weight: bold; text-align:center;\">Tên nhiệm vụ</td>";
            }
            if (_NguonVon.HasValue && _NguonVon.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Nguồn vốn</td>";
            }
            if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Đơn vị đề xuất </td>";
            }
            if (_DonViDuToan.HasValue && _DonViDuToan.Value)
            {
                _html += "<td style=\" border:1px solid #333;font-weight: bold; text-align:center;\">Đơn vị dự toán</td>";
            }
            if (_NamKeHoach.HasValue && _NamKeHoach.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Năm kế hoạch</td>";
            }
            if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị đề xuất của đơn vị<br/>" + _TotalDeXuat.ToString("N0") + "</td>";
            }
            if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
            {
                _html += "<td style=\" border:1px solid #333;font-weight: bold; text-align:center;\">Giá trị đề xuất của tổng cục<br/>" + _TotalDeXuat2.ToString("N0") + "</td>";
            }
            if (_GhiChu.HasValue && _GhiChu.Value)
            {
                _html += "<td style=\"border:1px solid #333;  font-weight: bold; text-align:center;\">Ghi chú</td>";
            }
            _html += "</tr>";
            foreach (NhiemVuView item in _tem3)
            {
                if (item.nhiemVuId > 0)
                {
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:center;\">" + item.STT + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nguonVonName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.donViDeXuatName + "</td>";
                    }
                    if (_DonViDuToan.HasValue && _DonViDuToan.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.donViDutoanName + "</td>";
                    }
                    if (_NamKeHoach.HasValue && _NamKeHoach.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:center;\">" + item.namKeHoach + "</td>";
                    }
                    if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align: right;\">" + item.giaTriDeXuat.ToString("N0") + "</td>";
                    }
                    if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align: right;\">" + item.giaTriDeXuat2.ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.ghiChu + "</td>";
                    }
                    _html += "</tr>";
                }
                else
                {
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:center;\">" + item.STT + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_DonViDuToan.HasValue && _DonViDuToan.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_NamKeHoach.HasValue && _NamKeHoach.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_GiaTriDeXuat.HasValue && _GiaTriDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalGiaTriDeXuatDonVi(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GiaTriDeXuat2.HasValue && _GiaTriDeXuat2.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalGiaTriDeXuatTongCuc(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    _html += "</tr>";
                }
            }
            _html += "</table>";
            return Json(_html, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintThongKe2(bool? _No, bool? _Name, bool? _NguonVon, bool? _DonViDeXuat, bool? _GiaTriGiao, bool? _GiaTriDuyet, bool? _GhiChu, bool? _TrangThai, bool? _GiaTriGiaiNgan, bool? _GiaTriQuyetToan, bool? _GiaTriHopDong, int? DonViThucHien, int? DanhMucNhiemVu, int? NguonVon, int? NamKeHoach)
        {
            IEnumerable<NhiemVu> model = _db.NhiemVus.Where(x => x.isTrash == false && x.trangThai > 0);
            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                model = model.Where(x => x.namKeHoach == NamKeHoach.Value);
            }

            if (DonViThucHien.HasValue && DonViThucHien > 0)
            {
                model = model.Where(x => x.donViDeXuatId == DonViThucHien.Value);
            }

            if (DanhMucNhiemVu.HasValue && DanhMucNhiemVu > 0)
            {
                model = model.Where(x => x.danhMucId == DanhMucNhiemVu.Value);
            }

            if (NguonVon.HasValue && NguonVon > 0)
            {
                model = (from q in model join w in _db.NguonVonNhiemVus on q.nhiemVuId equals w.nhiemVuId where w.nguonVonId == NguonVon.Value select q);
            }

            model = model.DistinctBy(x => x.nhiemVuId);
            decimal _totalGiao = 0;
            decimal _totalDuyet = 0;
            decimal _totalHopDong = 0;
            decimal _totalGiaiNgan = 0;
            decimal _totalQuyetToan = 0;
            List<NhiemVuView> _tem = new List<NhiemVuView>();
            List<NhiemVu> _ListNV = model.ToList();
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
                IOrderedEnumerable<NhiemVu> _listTem = _ListNV.OrderBy(x => x.createTime);
                foreach (NhiemVu nhiemVu in _listTem)
                {
                    if (nhiemVu.danhMucId == itemTem.danhMucId)
                    {
                        NhiemVuView _itemNV = new NhiemVuView
                        {
                            createTime = nhiemVu.createTime,
                            danhMucId = nhiemVu.danhMucId,
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
            ViewBag.TotalGiaiNgan = _totalGiaiNgan.ToString("N0");
            ViewBag.TotalDuyet = _totalDuyet.ToString("N0");
            ViewBag.TotalGiao = _totalGiao.ToString("N0");
            ViewBag.TotalHopDong = _totalHopDong.ToString("N0");
            ViewBag.TotalQuyetToan = _totalQuyetToan.ToString("N0");
            string _html = "";
            if (NamKeHoach.HasValue && NamKeHoach > 0)
            {
                _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; height: 40px; font-size: 18px; \">Báo Cáo Nhiệm Vụ Năm " + NamKeHoach.Value + "</div>";
            }
            else
            {
                _html += "<div style=\"text-align: center; text-transform: uppercase; font-weight: bold; height: 40px; font-size: 18px; \">Báo Cáo Nhiệm Vụ</div>";
            }
            _html += "<div style=\"text-align: right; text-transform: uppercase; font-weight: bold; line-height: 40px; font-size: 16px; \">ĐVT: VNĐ</div>";
            _html += "<table style=\" width:100%;border-collapse: collapse;\">";
            _html += "<tr>";
            if (_No.HasValue && _No.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">STT</td>";
            }
            if (_Name.HasValue && _Name.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Tên nhiệm vụ</td>";
            }
            if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Đơn vị thực hiện</td>";
            }
            if (_NguonVon.HasValue && _NguonVon.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Nguồn vốn</td>";
            }
            if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold;  text-align:center;\">Dự toán giao <br/>" + ViewBag.TotalGiao + "</td>";
            }
            if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Dự toán được duyệt <br/>" + ViewBag.TotalDuyet + "</td>";
            }
            if (_TrangThai.HasValue && _TrangThai.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Trạng thái</td>";
            }
            if (_GiaTriHopDong.HasValue && _GiaTriHopDong.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị hợp đồng<div style=\"text-align:right; \">" + ViewBag.TotalHopDong + "</div></td>";
            }
            if (_GiaTriGiaiNgan.HasValue && _GiaTriGiaiNgan.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; text-align:center;\">Giá trị giải ngân<div style=\"text-align:right; \">" + ViewBag.TotalGiaiNgan + "</div></td>";
            }
            if (_GiaTriQuyetToan.HasValue && _GiaTriQuyetToan.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold;  text-align:center;\">Giá trị quyết toán<div style=\"text-align:right; \">" + ViewBag.TotalQuyetToan + "</div></td>";
            }
            if (_GhiChu.HasValue && _GhiChu.Value)
            {
                _html += "<td style=\"border:1px solid #333; font-weight: bold; max-width: 100px; text-align:center;\">Ghi chú</td>";
            }

            _html += "</tr>";
            foreach (NhiemVuView item in _tem3)
            {
                if (item.nhiemVuId > 0)
                {
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\" border:1px solid #333;text-align:center;\">" + item.STT + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\" border:1px solid #333;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\" border:1px solid #333;\">" + item.donViDeXuatName + "</td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\" border:1px solid #333;\">" + item.nguonVonName + "</td>";
                    }
                    if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                    {
                        _html += "<td style=\" border:1px solid #333; text-align:right;\">" + (item.giaTriGiao ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                    {
                        _html += "<td style=\" border:1px solid #333; text-align:right;\">" + (item.giaTriTrungThau ?? 0).ToString("N0") + "</td>";
                    }

                    if (_TrangThai.HasValue && _TrangThai.Value)
                    {
                        if (item.trangThai == 1)
                        {
                            _html += "<td style=\" border:1px solid #333;\">Chưa triển khai</td>";
                        }

                        if (item.trangThai == 2)
                        {
                            _html += "<td style=\" border:1px solid #333;\">Đã trình đề cương dự toán</td>";
                        }

                        if (item.trangThai == 3)
                        {
                            _html += "<td style=\" border:1px solid #333;\">Đã phê duyệt đề cương dự toán</td>";
                        }

                        if (item.trangThai == 4)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã trình kế hoạch lựa chọn nhà thầu</td>";
                        }

                        if (item.trangThai == 5)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kế hoạch nhà thầu</td>";
                        }

                        if (item.trangThai == 6)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã phê duyệt kết quả lựa chọn nhà thầu</td>";
                        }

                        if (item.trangThai == 7)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã ký hợp đồng</td>";
                        }

                        if (item.trangThai == 8)
                        {
                            _html += "<td style=\"border:1px solid #333;\">Đã kết thúc</td>";
                        }
                    }
                    if (_GiaTriHopDong.HasValue && _GiaTriHopDong.Value)
                    {
                        _html += "<td style=\"border:1px solid #333; text-align:right;\">" + (item.giaTriHopDong ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GiaTriGiaiNgan.HasValue && _GiaTriGiaiNgan.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + GetGiaiNgan(item.nhiemVuId) + "</td>";
                    }
                    if (_GiaTriQuyetToan.HasValue && _GiaTriQuyetToan.Value)
                    {
                        _html += "<td style=\" border:1px solid #333;text-align:right;\">" + (item.giaTriQuyetToan ?? 0).ToString("N0") + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\" border:1px solid #333;\">" + item.ghiChu + "</td>";
                    }
                    _html += "</tr>";
                }
                else
                {
                    _html += "<tr>";
                    if (_No.HasValue && _No.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:center;\">" + item.STT + "</td>";
                    }
                    if (_Name.HasValue && _Name.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;font-weight: bold;\">" + item.nhiemVuName + "</td>";
                    }
                    if (_DonViDeXuat.HasValue && _DonViDeXuat.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_NguonVon.HasValue && _NguonVon.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_GiaTriGiao.HasValue && _GiaTriGiao.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalDuToanGiao(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GiaTriDuyet.HasValue && _GiaTriDuyet.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalDuToanDuyet(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_TrangThai.HasValue && _TrangThai.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    if (_GiaTriHopDong.HasValue && _GiaTriHopDong.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalHopDong(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GiaTriGiaiNgan.HasValue && _GiaTriGiaiNgan.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalGiaiNgan(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GiaTriQuyetToan.HasValue && _GiaTriQuyetToan.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;text-align:right;\">" + _TotalQuyetToan(_tem3, item.danhMucId) + "</td>";
                    }
                    if (_GhiChu.HasValue && _GhiChu.Value)
                    {
                        _html += "<td style=\"border:1px solid #333;\"></td>";
                    }
                    _html += "</tr>";
                }
            }
            _html += "</table>";
            return Json(_html, JsonRequestBehavior.AllowGet);
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
    }
}