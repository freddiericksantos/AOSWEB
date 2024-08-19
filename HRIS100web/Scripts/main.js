$('#menugrid').ready(function () {

    var currentPath = window.location.pathname;
    var currentQuery = window.location.search;
    var currentRoute = currentPath + currentQuery;
    if (currentRoute == '/Login.aspx') {
        $('#menugrid').attr("hidden", true);
    }
})