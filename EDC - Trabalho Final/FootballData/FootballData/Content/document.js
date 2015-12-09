var pagination_global = 0;
var number_of_pages_global = 0;

console.log("ok");

function downloadNews() {
    $.ajax({
        url: "/downloadNews",
        type: 'get',
        dataType: 'text',
        async: true,
        success: function (data) {
            console.log("ready");
            $("#smallAlert").hide();
            $("#showAlert").show();
        }
    });
}


$(function () {

    $('#show').on('click', function () {
        $('.card-reveal').slideToggle('slow');
    });

    $('.card-reveal .close').on('click', function () {
        $('.card-reveal').slideToggle('slow');
    });

    // pagination
    if ($(".pagination").length) {
        pagination_global = 1;

        var news_length = $(".pagination").attr("data-toggle");
        if (typeof news_length != 'undefined') {
            
            // hide news
            var id_active = $("li.active > a").attr("href").replace("#", "");
            //console.log(news_length);
            for (var i = parseInt(id_active) + 2; i <= news_length; i++) {
                //console.log(i);
                $("#new_id_" + i).hide();
            }

            var number_of_pages = Math.ceil(news_length / 2);
            number_of_pages_global = number_of_pages;

            for (var i = number_of_pages; i > 1; i--) {
                $(".pagination > li.active").after("<li id=\"li_page_" + i + "\"><a data-toggle=\"pagination\" href=\"#" + i + "\">" + i + "</a></li>");
            }

            // preview images
            for (var i = 1; i <= news_length; i++) {
                if ($("#img" + i).length != 0) {
                    var img = '<img src="' + $("#img" + i).attr("url-img") + '">';
                    //console.log(img);
                    $("#img" + i).popover({ trigger: "hover", placement: 'top', html: true, content: img });
                }
            }

        }
    }


    $("a[data-toggle=\"pagination\"]").click(function () {
        var id_active = $("li.active > a").attr("href").replace("#", "");
        for (var i = parseInt(id_active)*2-1 ; i <= parseInt(id_active)*2; i++) {
            //console.log("#new_id_" + i);
            $("#new_id_" + i).hide();
        }

        var id_new = $(this).attr("href").replace("#", "");
        for (var i = parseInt(id_new) * 2 - 1; i <= id_new * 2; i++) {
            //console.log("#new_id_" + i);
            $("#new_id_" + i).show();
        }

        pagination_global = parseInt(id_new);

        $("li.active").removeClass("active");
        $("#li_page_" + id_new).addClass("active");
    });

    $("a[aria-label='Previous']").click(function () {
        if (pagination_global > 1) {
            var id_active = $("li.active > a").attr("href").replace("#", "");
            for (var i = parseInt(id_active) * 2 - 1 ; i <= parseInt(id_active) * 2; i++) {
                //console.log("#new_id_" + i);
                $("#new_id_" + i).hide();
            }

            var id_new = pagination_global - 1;
            pagination_global = id_new;
            for (var i = parseInt(id_new) * 2 - 1; i <= id_new * 2; i++) {
                //console.log("#new_id_" + i);
                $("#new_id_" + i).show();
            }

            $("li.active").removeClass("active");
            $("#li_page_" + id_new).addClass("active");

        }
        return false;
    });

    $("a[aria-label='Next']").click(function () {
        if (pagination_global < number_of_pages_global) {
            var id_active = $("li.active > a").attr("href").replace("#", "");
            for (var i = parseInt(id_active) * 2 - 1 ; i <= parseInt(id_active) * 2; i++) {
                //console.log("#new_id_" + i);
                $("#new_id_" + i).hide();
            }

            var id_new = pagination_global + 1;
            pagination_global = id_new;

            for (var i = parseInt(id_new) * 2 - 1; i <= id_new * 2; i++) {
                //console.log("#new_id_" + i);
                $("#new_id_" + i).show();
            }

            $("li.active").removeClass("active");
            $("#li_page_" + id_new).addClass("active");

        }
        return false;
    });

});
