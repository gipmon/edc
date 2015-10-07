<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="EDC_Trabalho1.JSON._default" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div class="container">
	        <div class="row">
                <h1>Books Author <small id="author"></small></h1>
                <div class="lead"><a href="../JSON/"><i class="fa fa-backward"></i> Back to Authors</a>
                    <button class="btn pull-right" type="button" id="changeOrder" data-toggle="az">Order by name (Z-A)</button>
                </div>
                <hr />
                <div class="row" id="demo"></div>
	        </div>
            
        </div>
        <script type="text/javascript">
            function processArrayZA(arr) {
                var out = "<table class=\"table table-striped table-bordered table-condensed\"><tr><th>Title</th><th>Authors</th>" +
                    "<th>Type</th><th>Price</th><th>Sold qty.</th>" +
                    "<th>Notes</th><th>Pub. date</th></tr>";
                var i;

                for (i = arr.length - 1; i >= 0; i--) {
                    out += '<tr><td>' + arr[i].title + '</td><td>';
                    for (var j = 0; j < arr[i].authors.length; j++) {
                        if (j > 0)
                            out += "<br/>";
                        out += "<i class=\"fa fa-user\"></i> " + arr[i].authors[j];
                    }
                    out += "</td><td>";
                    out += arr[i].type + '</td><td>' +
                    arr[i].price + '</td><td>' +
                    arr[i].ytd_sales + '</td><td>' +
                    arr[i].notes + '</td><td>' +
                    arr[i].pubdate + '</td></tr>';
                }
                out += "</table>";
                document.getElementById("demo").innerHTML = out;
            }

            function processArrayAZ(arr) {
                var out = "<table class=\"table table-striped table-bordered table-condensed\"><tr><th>Title</th><th>Authors</th>" +
                    "<th>Type</th><th>Price</th><th>Sold qty.</th>" +
                    "<th>Notes</th><th>Pub. date</th></tr>";
                var i;

                for (i = 0; i < arr.length; i++) {
                    out += '<tr><td>' + arr[i].title + '</td><td>';
                    for (var j = 0; j < arr[i].authors.length; j++) {
                        if (j > 0)
                            out += "<br/>";
                        out += "<i class=\"fa fa-user\"></i> " + arr[i].authors[j];
                    }
                    out += "</td><td>";
                    out += arr[i].type + '</td><td>' +
                    arr[i].price + '</td><td>' +
                    arr[i].ytd_sales + '</td><td>' +
                    arr[i].notes + '</td><td>' +
                    arr[i].pubdate + '</td></tr>';
                }
                out += "</table>";
                document.getElementById("demo").innerHTML = out;
            }

            // http://stackoverflow.com/questions/827368/using-the-get-parameter-of-a-url-in-javascript
            function getQueryVariable(variable) {
                var query = window.location.search.substring(1);
                var vars = query.split("&");
                for (var i = 0; i < vars.length; i++) {
                    var pair = vars[i].split("=");
                    if (pair[0] == variable) {
                        return pair[1];
                    }
                }
                alert('Query Variable ' + variable + ' not found');
            }

            var authorID = getQueryVariable("author_ID");

            function call(processFunction) {
                var myUrl = "http://192.168.160.36/JSON/getAuthorTitles.aspx?author_ID="+authorID;
                $.ajax({
                    type: "GET",
                    url: myUrl,
                    data: { numAuthors: 8 },
                    dataType: "jsonp",
                    success: processFunction,
                    error: function (xhr, status, err) {
                    }
                });
            }

            function processAuthor(arr) {
                var result = JSON.stringify(arr, null, 4).split("\n").join("<br/>");
                result = result.split(" ").join("&nbsp;");
                var i;

                for (i = 0; i < arr.length; i++) {
                    if (arr[i].ID == authorID) {
                        $("#author").html(arr[i].name);
                        break;
                    }
                }
                call(processArrayAZ);
            }


            function findAuthorDetails() {
                var myUrl = "http://192.168.160.36/JSON/getAuthors.aspx";
                $.ajax({
                    type: "GET",
                    url: myUrl,
                    data: { numAuthors: 8 },
                    dataType: "jsonp",
                    success: processAuthor,
                    error: function (xhr, status, err) {
                    }
                });
            }
            
            findAuthorDetails();

            $("#changeOrder").click(function () {
                if ($(this).attr('data-toggle') == 'az') {
                    call(processArrayZA);
                    $(this).attr('data-toggle', 'za');
                    $(this).text('Order by name (A-Z)');
                } else {
                    call(processArrayAZ);
                    $(this).attr('data-toggle', 'az');
                    $(this).text('Order by name (Z-A)');
                }
            });
        </script>
</asp:Content>
