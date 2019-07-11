$('.number').keypress(function (event) {
    var keycode = event.which;
    if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
        event.preventDefault();
    }
});
function moveToPreviousPage(currentPage, formId) {
    moveToPage(currentPage - 1, formId);
}
function moveToNextPage(currentPage, formId) {
    moveToPage(currentPage + 1, formId);
}
function moveToPage(page, formId) {
    debugger;
    $(".page-to-move").val(page);
    $("#" + formId).submit();
}