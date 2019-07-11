$(document).ready(function () {
    //FROM

    $("#from-select-country").change(function () {
        $.ajax({
            type: "GET",
            url: "/FormSearch/GetFlightByParent",
            data: { parentId: $("#from-select-country").val() },
            success: function (data) {
                $("#from-select-city-or-airport").html(data.op);
            }
        });
    });

    $("#to-select-country").change(function () {
        $.ajax({
            type: "GET",
            url: "/FormSearch/GetFlightByParent",
            data: { parentId: $("#to-select-country").val() },
            success: function (data) {
                $("#to-select-city-or-airport").html(data.op);
            }
        });
    });

    $('#from-btnselect').click(function () {
        $('#frmSearchHome #from-id').val($('#from-select-city-or-airport').val());
        $('.sub-menu').hide();
        $("#to").click();
        $("#CheckFromInternational").val(1);
    });
    $('#to-btnselect').click(function () {
        $('#frmSearchHome #to-id').val($('#to-select-city-or-airport').val());
        $('.sub-menu').hide();
        $("#CheckToInternational").val(1);
    });


    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = (day) + "-" + (month) + "-" + now.getFullYear();
    $('#departuredate').val(today);
});

jQuery(function ($) {
    $.datepicker.regional['vi'] = {
        closeText: 'Đóng',
        prevText: '&#x3c;Trước',
        nextText: 'Tiếp&#x3e;',
        currentText: 'Hôm nay',
        monthNames: ['Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu',
		'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai'],
        monthNamesShort: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
		'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
        dayNames: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
        dayNamesShort: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
        dayNamesMin: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
        weekHeader: 'Tu',
        dateFormat: 'dd/mm/yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['vi']);

    $("#departuredate").datepicker
		({
		    numberOfMonths: 2
		, firstDay: 1
		, changeMonth: false
		, changeYear: false
		, dateFormat: 'dd-mm-yy'
		, closeText: 'X'
		, closeAtTop: false
		    //,showOn:'both'
		    //,buttonImageOnly:true
		, clearText: ''
		, nextText: '&gt;&gt;'
		, prevText: '&lt;&lt;'
		, maxDate: 330
		, minDate: 0
		, onClose: departuredate_OnClose
		    //,buttonImage: '/images/calendar.png'
		});
    $("#returndate").datepicker
		({
		    numberOfMonths: 2
		, firstDay: 1
		, changeMonth: false
		, changeYear: false
		, dateFormat: 'dd-mm-yy'
		, closeText: 'X'
		, closeAtTop: false
		    //,showOn:'both'
		    //,buttonImage: '/images/calendar.png'
		, buttonImageOnly: true
		, clearText: ''
		, nextText: '&gt;&gt;'
		, prevText: '&lt;&lt;'
		, maxDate: 365
		, minDate: new Date($('#departuredate').val())
		, beforeShow: returndate_BeforeShow
		});
    function getNextDate(date, incrementDay) {
        var year = (date.getYear() > 1900) ? date.getYear() : date.getYear() + 1900;
        var nextDate = new Date(year, (date.getMonth() + 1) - 1, date.getDate() + incrementDay);
        return nextDate;
    }
    function departuredate_OnClose(date) {
        var date = $('#departuredate').datepicker('getDate');
        var nextDate = getNextDate(date, 3);
        $("#returndate").val($.datepicker.formatDate("dd-mm-yy", nextDate));
        $("#returndate").focus();
    }
    // Customize two date pickers to work as a date range
    function returndate_BeforeShow(input) {
        var date = $('#departuredate').datepicker('getDate');
        //var nextDate = getNextDate(date, 1);
        //return { minDate: nextDate };
        return { minDate: date };
    }
});
$(document).ready(function () {
    if ($("#oneway").attr("checked")) {
        $('#frmSearchHome #returndate').attr('disabled', true);
        $('#boxdateof').addClass("displaynone");
        $('#boxdateon').removeClass("col-xs-6 col-md-6");
        $('#boxdateon').addClass("col-xs-12 col-md-12");
    } else {
        $('#frmSearchHome #returndate').attr('disabled', false);
        $('#boxdateof').removeClass("displaynone");
        $('#boxdateon').removeClass("col-xs-12 col-md-12");
        $('#boxdateon').addClass("col-xs-6 col-md-6");
    }

    $('input[name=way]').click(function () {
        var value_select = $(this).val();
        if (value_select == '1') {
            $('#frmSearchHome #returndate').attr('disabled', true);
            $('#boxdateof').addClass("displaynone");
            $('#boxdateon').removeClass("col-xs-6 col-md-6");
            $('#boxdateon').addClass("col-xs-12 col-md-12");
        } else {
            $('#frmSearchHome #returndate').attr('disabled', false);
            $('#boxdateof').removeClass("displaynone");
            $('#boxdateon').removeClass("col-xs-12 col-md-12");
            $('#boxdateon').addClass("col-xs-6 col-md-6");
        }
    });


    //$("select, input, textarea").uniform();
    $('#from,#to').click(function (event) {
        event.stopPropagation();
    });
    $("#from").click(function () {
        $(".sub-menu-from").show();
        $(".sub-menu-to").hide();
        $(".sub-menu-from ul li").click(function () {
            $("#from-id").val($(this).text());
            $("#CheckFromInternational").val(0);
            $(".sub-menu-from").hide();
            $("#to").click();
        });
    });
    $("#to").click(function () {
        $(".sub-menu-to").show();
        $(".sub-menu-from").hide();
        $(".sub-menu-to ul li").click(function () {
            $("#to-id").val($(this).text());
            $("#CheckToInternational").val(0);
            $(".sub-menu-to").hide();
            $("#departuredate").focus();
        });
    });
    $('body').click(function () {
        $(".sub-menu-from,.sub-menu-to").hide();
    });
    $('a.accordion-toggle').click(function () {
        var strs = $(this).attr('rel');
        if (strs != "") {
            var arr = strs.split('_');
            var type = arr[0];
            var air = arr[1];
            $.ajaxq('flight_time', {
                type: 'POST',
                url: '/ajax/flight/ticket',
                data: { type: type, airline: air },
                success: function (response) {
                    $('div.' + strs).html(response);
                }
            });

            $('a.' + strs).attr('rel', '');
        }
    });
});