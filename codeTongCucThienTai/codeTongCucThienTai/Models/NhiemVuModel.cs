using System;

namespace codeTongCucThienTai.Models
{
    public class NhiemVuModel
    {
        public int nhiemVuId { get; set; }
        public string nhiemVuName { get; set; }
        public int danhMucId { get; set; }
        public string danhMucName { get; set; }
        public int namKeHoach { get; set; }
        public string giaTriDeXuat { get; set; }
        public string giaTriDeXuat2 { get; set; }
        public string giaTriGiao { get; set; }
        public int donViDeXuatId { get; set; }
        public string donViDeXuatName { get; set; }
        public int donViDuToanId { get; set; }
        public string donViDutoanName { get; set; }
        public string nguonVonName { get; set; }
        public int[] nguonVonName2 { get; set; }
        public string ghiChu { get; set; }
        public System.DateTime createTime { get; set; }
        public bool isTrash { get; set; }
        public int trangThai { get; set; }
        public string giaTriDuyet { get; set; }
        public string giaTriTrungThau { get; set; }
        public string nhaThauTrung { get; set; }
        public string giaTriQuyetToan { get; set; }
        public string giaTriThanhToan { get; set; }
        public string userCreate { get; set; }
        public string userUpdate { get; set; }
        public System.DateTime updateTime { get; set; }
        public string giaTriHopDong { get; set; }
    }
    public class NhiemVuView
    {
        public string Code { get; set; }
        public string STT { get; set; }
        public int nhiemVuId { get; set; }
        public string nhiemVuName { get; set; }
        public int danhMucId { get; set; }
        public int namKeHoach { get; set; }
        public decimal giaTriDeXuat { get; set; }
        public Nullable<decimal> giaTriGiao { get; set; }
        public int donViDeXuatId { get; set; }
        public string donViDeXuatName { get; set; }
        public int donViDuToanId { get; set; }
        public string donViDutoanName { get; set; }
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
        public Nullable<decimal> giaTriHopDong { get; set; }
        public Nullable<decimal> giaTriThanhToan { get; set; }
        public decimal giaTriDeXuat2 { get; set; }
        public int? Level { get; set; }
        public int danhMucCha { get; set; }
        public string hanTrinh { get; set; }
    }
    public partial class GiaiNganModel
    {
        public int giaiNganId { get; set; }
        public int nhiemVuId { get; set; }
        public string nhiemVuName { get; set; }
        public string giaTriThanhToan { get; set; }
        public string thoiGianThanhToan { get; set; }
        public double tiLeGiaiNgan { get; set; }
    }
}