<%@ Page Title="Phiếu thu" Language="C#" ValidateRequest="false" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="rpPhieuthu.aspx.cs" Inherits="PrintsFile_rpPhieuthu" %>

<%@ Register TagPrefix="uc1" TagName="FormHeader" Src="~/PrintsFile/FormHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Source/ndbstyle.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        function PrintAllDataSys(idmaindata) {
            var contents = $("#" + idmaindata).html();
            var frame1 = $('<iframe />');
            frame1[0].name = "frame1";
            frame1.css({ "position": "absolute", "top": "-1000000px" });
            $("body").append(frame1);
            var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
            frameDoc.document.open();
            frameDoc.document.write('<html><head><title>Phần Mềm Quản lý Sản Xuất & Kinh Doanh - Design  By Niemtin.JSC - Email :   Support@niemtinvn.com 0912991369</title>');
            frameDoc.document.write('</head><body>');
            frameDoc.document.write('<link href="Source/ndbstyle.css" rel="stylesheet" />');
            frameDoc.document.write(contents);
            frameDoc.document.write('</body></html>');
            frameDoc.document.close();
            setTimeout(function () {
                window.frames["frame1"].focus();
                window.frames["frame1"].print();
                frame1.remove();
            }, 500);
        }
    </script>
    <div id="mainContain" style="background-color: #ffffff; font-size: 14px; font-family: tahoma;margin-bottom: 50px">
        <div id="IDmainData" style="width: 100%; clear: both">
            <uc1:FormHeader runat="server" ID="FormHeader" />
            <div style="width: 750px; text-align: center; font-weight: bold; clear: both; font-size: 30px; padding-top: 20px; padding-bottom: 10px; font-family: tahoma">
                PHIẾU THU
            </div>
            <div style="width: 750px; text-align: center; font-weight: bold; clear: both; font-size: 14px">
                <asp:Label ID="txtStarDate" runat="server" Text=""></asp:Label>
            </div>
            <div style="width: 100%; clear: both; background-color: #ffffff">
                <table width="100%" border="0" style="font-size: 14px; font-weight: bold; font-family: tahoma">
                    <tr>
                        <td colspan="2" style="width: 30%">Họ‎, ‎tên người nộp tiền ‎: </td>
                        <td colspan="4">
                            <asp:Label ID="lblHoTen" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Địa chỉ ‎:  </td>
                        <td colspan="4">
                            <asp:Label ID="lblDiaChi" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Lý do nộp ‎ ‎: </td>
                        <td colspan="4">
                            <asp:Label ID="lblLyDoNop" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Số tiền ‎ ‎: </td>
                        <td colspan="4">
                            <asp:Label ID="lblSoTien" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Viết bằng chữ ‎ ‎: </td>
                        <td colspan="4">
                            <asp:Label ID="lblVietBangChu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Kèm theo ‎ ‎: </td>
                        <td colspan="4">
                            <asp:Label ID="lblKemTheo" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Chứng từ kế toán ‎ ‎: </td>
                        <td colspan="4">
                            <asp:Label ID="lblChungTuKeToan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: right; padding-right: 50px; font-style: italic">
                            <asp:Label ID="lblNgayThangNam" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                        <td colspan="4"></td>
                    </tr>
                </table>
                <table width="100%" border="0" style="font-size: 14px; font-weight: bold; font-family: tahoma">
                    <tr style="text-align: center">
                        <td>Thủ trưởng đơn vị</td>
                        <td>Kế toán trưởng</td>
                        <td>Người lập</td>
                        <td>Người nộp</td>
                        <td>Thủ quỹ‎</td>
                    </tr>
                    <tr style="text-align: center">
                        <td colspan="6" style="height: 40px"></td>
                    </tr>
                    <tr style="text-align: center; height: 40px">
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>‎
                        <asp:Label ID="lblThuQuy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="fixed">
          <div style="width: 100%; clear: both">
        <div style="margin-top: 25px; float: right">
            <input type="button" value="In Phiếu Thu" title="Nhấn vào đây để in" class="btnprint" onclick="javascript: PrintAllDataSys('IDmainData');" />
        </div>
        <div style="margin-right: 20px; margin-top: 25px; float: right">
            <asp:HiddenField ID="hdfhtml" runat="server" />
            <asp:Button ID="btnExport" CssClass="exportbtn" ToolTip="Nhấn vào đây để lưu file excel" runat="server" Text="Lưu File Excel" OnClick="ExportToExcel" />
        </div>
    </div>
    </div>
  
    <script type="text/javascript">
        $(function () {
            $("[id*=btnExport]").click(function () {
                $("[id*=hdfhtml]").val($("#mainContain").html());
            });
        });
    </script>
</asp:Content>

