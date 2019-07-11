using System;

namespace codeTongCucThienTai.Models
{
    public class DuyetNhiemVuModel
    {
        public int nhiemVuId { get; set; }
        public string nhiemVuName { get; set; }
        public int danhMucId { get; set; }
        public string danhMucName { get; set; }
        public int namKeHoach { get; set; }
        public decimal giaTriDeXuat { get; set; }
        public string giaTriGiao { get; set; }
        public int donViDeXuatId { get; set; }
        public string donViDeXuatName { get; set; }
        public int donViDuToanId { get; set; }
        public string donViDutoanName { get; set; }
        public int nguonVonId { get; set; }
        public string nguonVonName { get; set; }
        public string ghiChu { get; set; }
        public System.DateTime createTime { get; set; }
        public bool isTrash { get; set; }
        public int trangThai { get; set; }
        public Nullable<decimal> giaTriDuyet { get; set; }
        public Nullable<decimal> giaTriTrungThau { get; set; }
        public string nhaThauTrung { get; set; }
        public Nullable<decimal> giaTriQuyetToan { get; set; }
        public string userCreate { get; set; }
        public string userUpdate { get; set; }
        public System.DateTime updateTime { get; set; }

        public string fileAttachment { get; set; }

        public string hanTrinh { get; set; }
    }
}