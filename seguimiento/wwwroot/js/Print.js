$(document).ready(function () {
    $('.print').on('click', function () {
        //alert("click");
        var name = $(this).data("id");





        html2canvas(document.querySelector(name), { scale: 5 }).then(canvas => {

            canvas.style.width = "100%";
            canvas.style.height = "auto";

            var tWindow = window.open("");
            $(tWindow.document.body)

                .html(canvas);

            $(tWindow.document.body).ready(function () {

                tWindow.focus();
                tWindow.print();
                tWindow.close();
            });
        });




    });
});