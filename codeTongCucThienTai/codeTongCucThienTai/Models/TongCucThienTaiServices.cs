using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace codeTongCucThienTai.Models
{
    public class TongCucThienTaiServices
    {
        private readonly TongCucThienTaiEntities _db;
        public TongCucThienTaiServices()
        {
            _db = new TongCucThienTaiEntities();
        }

        public IEnumerable<IdentityRole> ListRole()
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return roleManager.Roles.AsEnumerable();
        }
        public IEnumerable<DropDownModel> DropDownListRole(string _curentId)
        {
            try
            {
                IEnumerable<IdentityRole> entitys = ListRole();
                List<DropDownModel> result = new List<DropDownModel>();
                foreach (IdentityRole item in entitys)
                {
                    if (item.Id == _curentId)
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.Name,
                            Value = item.Id,
                            IsSelect = true
                        });
                    }
                    else
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.Name,
                            Value = item.Id
                        });
                    }

                }
                return result;
            }
            catch (Exception)
            {
                return new List<DropDownModel>();
            }
        }
        public IEnumerable<DropDownModel> DropDownListDanhMucNguonVon(List<int> _curentId)
        {
            try
            {
                IQueryable<DanhMucNguonVon> entitys = _db.DanhMucNguonVons.Where(x => x.isTrash == false);
                List<DropDownModel> result = new List<DropDownModel>();
                foreach (DanhMucNguonVon item in entitys)
                {
                    if (_curentId.Count > 0 && _curentId.Any(x => x == item.danhMucNguonVonId))
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.danhMucNguonVonName,
                            Value = item.danhMucNguonVonId.ToString(),
                            IsSelect = true
                        });
                    }
                    else
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.danhMucNguonVonName,
                            Value = item.danhMucNguonVonId.ToString(),
                            IsSelect = false
                        });
                    }

                }
                return result;
            }
            catch (Exception)
            {
                return new List<DropDownModel>();
            }
        }
        public IEnumerable<DropDownModel> DropDownListDanhMucDonVi(int? _curentId)
        {
            try
            {
                IQueryable<DanhMucDonVi> entitys = _db.DanhMucDonVis.Where(x => x.isTrash == false);
                List<DropDownModel> result = new List<DropDownModel>();
                foreach (DanhMucDonVi item in entitys)
                {
                    if (item.danhMucDonViId == _curentId)
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.danhMucDonViName,
                            Value = item.danhMucDonViId.ToString(),
                            IsSelect = true
                        });
                    }
                    else
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.danhMucDonViName,
                            Value = item.danhMucDonViId.ToString()
                        });
                    }

                }
                return result;
            }
            catch (Exception)
            {
                return new List<DropDownModel>();
            }
        }
        public IEnumerable<DropDownModel> DropDownListDanhMucNhiemVu(int? _curentId)
        {
            try
            {
                IQueryable<DanhMucNhiemVu> entitys = _db.DanhMucNhiemVus.Where(x => x.isTrash == false);
                List<DropDownModel> result = new List<DropDownModel>();
                foreach (DanhMucNhiemVu item in entitys)
                {
                    if (item.danhMucNhiemVuId == _curentId)
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.danhMucNhiemVuName,
                            Value = item.danhMucNhiemVuId.ToString(),
                            IsSelect = true
                        });
                    }
                    else
                    {
                        result.Add(new DropDownModel
                        {
                            Text = item.danhMucNhiemVuName,
                            Value = item.danhMucNhiemVuId.ToString()
                        });
                    }

                }
                return result;
            }
            catch (Exception)
            {
                return new List<DropDownModel>();
            }
        }
        public IEnumerable<DropDownModel> DropDownListNamKeHoach(int? _curentId)
        {
            try
            {
                int _yearCurent = DateTime.Now.Year;
                List<DropDownModel> result = new List<DropDownModel>();
                for (int i = _yearCurent - 10; i < _yearCurent + 11; i++)
                {
                    if (i == _curentId)
                    {
                        result.Add(new DropDownModel
                        {
                            Text = i.ToString(),
                            Value = i.ToString(),
                            IsSelect = true
                        });
                    }
                    else
                    {
                        result.Add(new DropDownModel
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        });
                    }

                }
                return result;
            }
            catch (Exception)
            {
                return new List<DropDownModel>();
            }
        }
        public bool CheckExitsNameDanhMucNhiemVu(int? Id, string _Name)
        {
            DanhMucNhiemVu model = _db.DanhMucNhiemVus.FirstOrDefault(x => x.danhMucNhiemVuName == _Name && x.danhMucNhiemVuId != Id && x.isTrash == false);
            return (model != null);
        }
        public bool CheckExitsNameDanhMucDonVi(int? Id, string _Name)
        {
            DanhMucDonVi model = _db.DanhMucDonVis.FirstOrDefault(x => x.danhMucDonViName == _Name && x.danhMucDonViId != Id && x.isTrash == false);
            return (model != null);
        }
        public bool CheckExitsNameDanhMucNguonVon(int? Id, string _Name)
        {
            DanhMucNguonVon model = _db.DanhMucNguonVons.FirstOrDefault(x => x.danhMucNguonVonName == _Name && x.danhMucNguonVonId != Id && x.isTrash == false);
            return (model != null);
        }
        public bool CheckExitsNameTaiLieu(int? Id, string _Name)
        {
            TaiLieu model = _db.TaiLieux.FirstOrDefault(x => x.taiLieuName == _Name && x.taiLieuId != Id && x.isTrash == false);
            return (model != null);
        }
        public bool CheckExitsNameNhiemVu(int? Id, string _Name, int _NamKeHoach, int _DonViDeXuat, int _DonViDuToan, string _NguonVon)
        {
            NhiemVu model = _db.NhiemVus.FirstOrDefault(x => x.nhiemVuName == _Name.Trim() && x.namKeHoach == _NamKeHoach && x.donViDuToanId == _DonViDuToan && x.donViDeXuatId == _DonViDeXuat && x.nhiemVuId != Id && x.isTrash == false && x.nguonVonName == _NguonVon);
            return (model != null);
        }

        public bool CheckUsingTaiLieu(int Id)
        {
            IQueryable<NhiemVuTaiLieu> model = _db.NhiemVuTaiLieux.Where(x => x.taiLieuId == Id);
            return (model.Count() > 0);
        }
        public bool CheckUsingDanhMucNhiemVu(int Id)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.danhMucId == Id);
            return (model.Count() > 0);
        }
        public bool CheckUsingNguonVon(int Id)
        {
            var _check = (from q in _db.NguonVonNhiemVus join p in _db.NhiemVus on q.nhiemVuId equals p.nhiemVuId where q.nguonVonId == Id && p.isTrash == false select q);
            return (_check.Count() > 0);
        }
        public bool CheckUsingDonVi(int Id)
        {
            IQueryable<NhiemVu> model = _db.NhiemVus.Where(x => x.donViDeXuatId == Id || x.donViDuToanId == Id);
            return (model.Count() > 0);
        }

        public string GetNameDanhMucById(int Id)
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

        public TaiLieuNhiemVuModel GetTaiLieuTheoNhiemVu(int Id)
        {
            IQueryable<NhiemVuTaiLieu> entitys = _db.NhiemVuTaiLieux.Where(x => x.nhiemVuId == Id);
            TaiLieuNhiemVuModel model = new TaiLieuNhiemVuModel();
            List<TaiLieu> _list = new List<TaiLieu>();
            foreach (NhiemVuTaiLieu item in entitys)
            {
                _list.Add(_db.TaiLieux.Find(item.taiLieuId));
            }
            model.NhiemVuId = Id;
            return model;
        }

        public string DropDownListNguonVonMultile(List<int> _listId)
        {
            IQueryable<DanhMucNguonVon> entitys = _db.DanhMucNguonVons.Where(x => x.isTrash == false);
            string _html = "";
            _html += "<div class=\"button-group nguon-von\">";
            _html += "<input id=\"txtNguonVon\" readonly=\"readonly\" type=\"text\" placeholder=\"Chọn nguồn vốn\" class=\"form-control input-sm dropdown-toggle\" data-toggle=\"dropdown\"/>";
            _html += "<ul class=\"dropdown-menu\">";
            foreach (DanhMucNguonVon item in entitys)
            {
                if (_listId.Count > 0 && _listId.Any(x => x == item.danhMucNguonVonId))
                {
                    _html += "<li>";
                    _html += "<a href=\"#\" data-value=\"" + item.danhMucNguonVonId + "\" tabIndex=\"-1\"><input class=\"_nguonvon\" value=\"" + item.danhMucNguonVonId + "\" checked=\"checked\" type=\"checkbox\"/>&nbsp; " + item.danhMucNguonVonName + "</a>";
                    _html += "</li>";
                }
                else
                {
                    _html += "<li>";
                    _html += "<a href=\"#\" data-value=\"" + item.danhMucNguonVonId + "\" tabIndex=\"-1\"><input class=\"_nguonvon\" value=\"" + item.danhMucNguonVonId + "\" type=\"checkbox\"/>&nbsp; " + item.danhMucNguonVonName + "</a>";
                    _html += "</li>";
                }
            }
            _html += "</ul>";
            _html += "</div>";
            return _html;
        }

        public IEnumerable<DropDownModel> DropDownListTrangThai(int? Id)
        {
            List<DropDownModel> result = new List<DropDownModel>
            {
                new DropDownModel { Text = "Chưa triển khai", Value = "1" },
                new DropDownModel { Text = "Đã trình đề cương dự toán", Value = "2" },
                new DropDownModel { Text = "Đã phê duyệt đề cương dự toán", Value = "3" },
                new DropDownModel { Text = "Đã trình kế hoạch lựa chọn nhà thầu", Value = "4" },
                new DropDownModel { Text = "Đã phê duyệt kế hoạch nhà thầu", Value = "5" },
                new DropDownModel { Text = "Đã phê duyệt kết quả lựa chọn nhà thầu", Value = "6" },
                new DropDownModel { Text = "Đã ký hợp đồng", Value = "7" },
                new DropDownModel { Text = "Đã kết thúc", Value = "8" }
            };
            return result;
        }

        public void LogTrangThai(int NhiemVuId, string NhiemVuName, string UserName, int TrangThaiOld, int TrangThaiNew)
        {
            LogTrangThai entity = new LogTrangThai
            {
                nhiemVuId = NhiemVuId,
                trangThaiTruoc = TrangThaiOld,
                trangThaiSau = TrangThaiNew,
                nhiemVuName = NhiemVuName,
                userUpdate = UserName,
                updateTime = DateTime.Now
            };
            _db.LogTrangThais.Add(entity);
            _db.SaveChanges();
        }

        public int GetAndCreateDanhMucByName(string _Name)
        {
            DanhMucNhiemVu entity = _db.DanhMucNhiemVus.FirstOrDefault(x => x.danhMucNhiemVuName.ToLower() == _Name.ToLower().Trim() && x.isTrash == false);
            if (entity != null)
            {
                return entity.danhMucNhiemVuId;
            }
            else
            {
                //entity = new DanhMucNhiemVu { danhMucNhiemVuName = _Name, isTrash = false };
                //_db.DanhMucNhiemVus.Add(entity);
                //_db.SaveChanges();
                //return entity.danhMucNhiemVuId;
                return 0;
            }
        }

        public int GetAndCreateDanhMucDonViByName(string _Name)
        {
            DanhMucDonVi entity = _db.DanhMucDonVis.FirstOrDefault(x => x.danhMucDonViName.ToLower() == _Name.ToLower().Trim() && x.isTrash == false);
            if (entity != null)
            {
                return entity.danhMucDonViId;
            }
            else
            {
                entity = new DanhMucDonVi { danhMucDonViName = _Name, isTrash = false };
                _db.DanhMucDonVis.Add(entity);
                _db.SaveChanges();
                return entity.danhMucDonViId;
            }
        }

        public int GetAndCreateDanhMucNguonVonByName(string _Name)
        {
            DanhMucNguonVon entity = _db.DanhMucNguonVons.FirstOrDefault(x => x.danhMucNguonVonName.ToLower() == _Name.ToLower().Trim() && x.isTrash == false);
            if (entity != null)
            {
                return entity.danhMucNguonVonId;
            }
            else
            {
                entity = new DanhMucNguonVon { danhMucNguonVonName = _Name, isTrash = false };
                _db.DanhMucNguonVons.Add(entity);
                _db.SaveChanges();
                return entity.danhMucNguonVonId;
            }
        }

        public IEnumerable<DropDownModel> DropDownListNguonVon2(int? _curentId, int? _parentId)
        {
            try
            {
                IEnumerable<DanhMucNguonVon> entitys = _db.DanhMucNguonVons.Where(x => x.isTrash == false && x.danhMucCha == null);
                entitys = entitys.Where(x => x.danhMucNguonVonId != _curentId);
                List<DropDownModel> result = new List<DropDownModel>();
                foreach (DanhMucNguonVon item in entitys)
                {
                    result.Add(new DropDownModel
                    {
                        Text = item.danhMucNguonVonName.ToString(),
                        Value = item.danhMucNguonVonId.ToString()
                    });
                    DropDownListNguonVonCon(result, _curentId, item.danhMucNguonVonId, "-");
                }
                return result;
            }
            catch (Exception)
            {
                return new List<DropDownModel>();
            }
        }

        private IEnumerable<DropDownModel> DropDownListNguonVonCon(List<DropDownModel> result, int? _curentId, int _parentId, string st)
        {
            try
            {
                IEnumerable<DanhMucNguonVon> entity = _db.DanhMucNguonVons.Where(x => x.isTrash == false && x.danhMucCha == _parentId && x.danhMucNguonVonId != _curentId);
                foreach (DanhMucNguonVon item in entity)
                {
                    result.Add(new DropDownModel { Text = st + item.danhMucNguonVonName, Value = item.danhMucNguonVonId.ToString() });
                    DropDownListNguonVonCon(result, _curentId, item.danhMucNguonVonId, st + st);
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public IEnumerable<DropDownModel> DropDownListNhiemVu2(int? _curentId, int? _parentId)
        {
            try
            {
                IEnumerable<DanhMucNhiemVu> entitys = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == null);
                entitys = entitys.Where(x => x.danhMucNhiemVuId != _curentId);
                List<DropDownModel> result = new List<DropDownModel>();
                foreach (DanhMucNhiemVu item in entitys)
                {
                    result.Add(new DropDownModel
                    {
                        Text = item.danhMucNhiemVuName.ToString(),
                        Value = item.danhMucNhiemVuId.ToString()
                    });
                    DropDownListNhiemVuCon(result, _curentId, item.danhMucNhiemVuId, "-");
                }
                return result;
            }
            catch (Exception)
            {
                return new List<DropDownModel>();
            }
        }

        private IEnumerable<DropDownModel> DropDownListNhiemVuCon(List<DropDownModel> result, int? _curentId, int _parentId, string st)
        {
            try
            {
                IEnumerable<DanhMucNhiemVu> entity = _db.DanhMucNhiemVus.Where(x => x.isTrash == false && x.danhMucCha == _parentId && x.danhMucNhiemVuId != _curentId);
                foreach (DanhMucNhiemVu item in entity)
                {
                    result.Add(new DropDownModel { Text = st + item.danhMucNhiemVuName, Value = item.danhMucNhiemVuId.ToString() });
                    DropDownListNhiemVuCon(result, _curentId, item.danhMucNhiemVuId, st + st);
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public string ChuyenSangSoLaMa(int Number)
        {
            if (Number <= 0)
            {
                return "";
            }
            else if (Number < 3000)
            {
                if (Number >= 1000)
                {
                    return "M" + ChuyenSangSoLaMa(Number - 1000);
                }
                else
                if (Number >= 900)
                {
                    return "CM" + ChuyenSangSoLaMa(Number - 900);
                }
                else
                if (Number >= 500)
                {
                    return "D" + ChuyenSangSoLaMa(Number - 500);
                }
                else
                if (Number >= 400)
                {
                    return "CD" + ChuyenSangSoLaMa(Number - 400);
                }
                else
                if (Number >= 100)
                {
                    return "C" + ChuyenSangSoLaMa(Number - 100);
                }
                else
                if (Number >= 90)
                {
                    return "XC" + ChuyenSangSoLaMa(Number - 90);
                }
                else
                if (Number >= 50)
                {
                    return "L" + ChuyenSangSoLaMa(Number - 50);
                }
                else
                if (Number >= 40)
                {
                    return "XL" + ChuyenSangSoLaMa(Number - 40);
                }
                else
                if (Number >= 10)
                {
                    return "X" + ChuyenSangSoLaMa(Number - 10);
                }
                else
                if (Number >= 9)
                {
                    return "IX" + ChuyenSangSoLaMa(Number - 9);
                }
                else
                if (Number >= 5)
                {
                    return "V" + ChuyenSangSoLaMa(Number - 5);
                }
                else
                if (Number >= 4)
                {
                    return "IV" + ChuyenSangSoLaMa(Number - 4);
                }
                else
                if (Number >= 1)
                {
                    return "I" + ChuyenSangSoLaMa(Number - 1);
                }
                else
                    return ChuyenSangSoLaMa(Number);
            }
            else
                return "";
            //else if (Number < 11)
            //{
            //    string _out = "";
            //    switch (Number)
            //    {
            //        case 1:
            //            _out = "I";
            //            break;
            //        case 2:
            //            _out = "II";
            //            break;
            //        case 3:
            //            _out = "III";
            //            break;
            //        case 4:
            //            _out = "IV";
            //            break;
            //        case 5:
            //            _out = "V";
            //            break;
            //        case 6:
            //            _out = "VI";
            //            break;
            //        case 7:
            //            _out = "VII";
            //            break;
            //        case 8:
            //            _out = "VIII";
            //            break;
            //        case 9:
            //            _out = "IX";
            //            break;
            //        case 10:
            //            _out = "X";
            //            break;
            //        default:
            //            _out = "";
            //            break;
            //    }
            //    return _out;
            //}
            //else
            //{
            //    return "";
            //}
        }
    }
}