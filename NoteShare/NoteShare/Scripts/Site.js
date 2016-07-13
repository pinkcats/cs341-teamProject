$(document).ready(function () {
    $('.searchbox').autocomplete({
        delay: 300,
        minLength: 3,
        source: '/Home/SearchBar',
        select: function (event, ui) {
            window.location.href = "/Home/Document/" + ui.item.key;
        }
    });
});