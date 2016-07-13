$(document).ready(function () {
    var results = [];
    var numStart = 0;
    var numEnd = 0;

    var showResults = 20;
    $('#SearchButton').on('click', function () {
        $('.contentArea').block({
            message: '<h1>Searching</h1>'
        });
        $('#SearchOptions').block({ message: null });
        var model = {
            university: $('#UniversityId').val(),
            className: $('#ClassName').val(),
            professor: $('#Professor').val(),
            description: $('#Description').val(),
            department: $('#Department').val(),
            fileType: $('#FileType').val()
        };
        $.post('/Home/Search', { model: model }, function (data) {
            if (data.length == 0) {
                $('#SearchResults').html("No notes found");
            } else {
                results = data;

                numEnd = (results.length < showResults) ? results.length : (showResults);

                updateGrid();
            }

            $('.contentArea,#SearchOptions').unblock();
        })
    });

    $('#SortByDate').on('click', function () {
        results.sort(function (a, b) {
            var myDateA = new Date(a.dateAdded.match(/\d+/)[0] * 1);
            var myDateB = new Date(b.dateAdded.match(/\d+/)[0] * 1);
            return myDateB - myDateA;
        });

        updateGrid();
    });

    $("input#University").autocomplete({
        source: universityNames,
        minLength: 5,
        change: function (event, ui) {
            //this code implements mustmatch that prevents user from entering a university not in our repository//
            //This code must be in front of the code to put the id in the hidden window//
            var source = $(this).val();
            var found = $('.ui-autocomplete li').text().search(source);
            if (found < 0) {
                $(this).val('');
            }
            //this part puts the id in the hidden 
            $(this).val(ui.item.value);
            $('#UniversityId').val(ui.item.key);
        }
    });

    $('#SortByRating').on('click', function () {
        results.sort(function (a, b) {
            return b.rating - a.rating;
        });

        updateGrid();
    });

    $('#ResultsPagingPrev').on('click', function () {
        if (numStart < showResults) {
            numStart = 0;
            numEnd = (results.length <= showResults) ? results.length : showResults
        } else {
            numStart -= showResults;
            numEnd -= showResults;
        }

        updateGrid();
    });

    $('#ResultsPagingNext').on('click', function () {
        if ((numEnd + showResults) > results.length) {
            numEnd = results.length;
            numStart = (results.length - showResults > 0) ? results.length - showResults : 0;
        } else {
            numEnd += showResults;
            numStart += showResults
        }

        updateGrid();
    });

    function updateGrid() {
        var lines = "";
        $.each(results.slice(numStart, numEnd), function (i, e) {
            lines += htmlTemplate(e);
        });

        $('#SearchResults').html(lines);

        $('.resultRating').each(function (i, e) {
            var score = parseFloat($(e).text());
            $(e).text('');
            $(e).raty({
                'score': score,
                'readOnly': true,
                'half': true
            });
        });

        $('#ResultsPagingNumber').html("Showing " + (numStart + 1) + "-" + numEnd + " of " + results.length + " records");
    };

    function htmlTemplate(data) {
        var myDate = new Date(data.dateAdded.match(/\d+/)[0] * 1);
        return '<div><div class="resultName">' + data.name + '</div>' +
               '<div class="resultDate">' + myDate.formatMMDDYYYY() + '</div>' +
               '<div class="resultRating">' + data.rating + '</div>' +
               '<a class="resultLink" href="Document/' + data.noteId + '">View</a></div>';
    };
});

Date.prototype.formatMMDDYYYY = function () {
    return (this.getMonth() + 1) +
    "/" + this.getDate() +
    "/" + this.getFullYear();
}