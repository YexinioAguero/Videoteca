
$('.slide').slick({
    autoplay: true,
    dots: true,
    infinite: true,
    speed: 500,
    fade: true,
    cssEase: 'linear'
});

$('.carrousel').each(function () {
        $(this).slick({
            infinite: true,
            centerMode: true,
            centerPadding: '60px',
            slidesToShow: 3,
            autoplay: true,
            responsive: [
                {
                    breakpoint: 900,
                    settings: {
                        arrows: false,
                        centerMode: true,
                        centerPadding: '60px',
                        slidesToShow: 2
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        arrows: false,
                        centerMode: true,
                        centerPadding: '40px',
                        slidesToShow: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        arrows: false,
                        centerMode: true,
                        centerPadding: '40px',
                        slidesToShow: 1
                    }
                }
            ]
        });
    var divs = $("div.movie").get().sort(function () {
        return Math.round(Math.random()) - 0.5;

    }).slice(0, 4)
    $(divs).appendTo(divs[0].parentNode).show();
});


// Escucha el envío del formulario y oculta el pop-up al enviarlo
    // Manejar el evento de clic del botón

$('.btnC').click(function () {
    var commp = document.querySelector(".comment-post");
    var widget = document.querySelector(".comment-widget");
    var editBtn = document.querySelector(".edit");
    var texto = $('.textC').val();

        // Obtener los datos del formulario o crear un objeto de datos
    var datos = {
            text: $('.textC').val(),
            id: $('.textC').attr("id")
    };
    var datos1 = { id: $('.textC').attr("id") };
        // Realizar la solicitud Ajax
    const regText = /\S+/g;
 
    if (regText.test(texto)) {
        $.ajax({
            url: '/User/SetComment',
            type: 'POST',
            data: datos,
            success: function (response) {
                // Manejar la respuesta del ActionResult
                widget.style.display = "none";
                commp.style.display = "block";
                editBtn.onclick = () => {
                    widget.style.display = "block";
                    commp.style.display = "none";
                }
                $('.textC').val("");
                $.ajax({
                    url: '/User/GetComment',
                    type: 'GET',
                    data: datos1,
                    success: function (result) {
                        console.log(result);
                        $("#getC").html(result);
                    },
                    error: function () {
                        // Manejar errores en la solicitud Ajax
                        alert('Error al obtener la vista parcial');
                    }
                });

            },
            error: function (error) {
                // Manejar errores en la solicitud Ajax
                console.log(error);
            }
        });

    } else {
        alert("El campo esta vacio!!");
    }

    
});

$('#comments-tab').click(function () {
    var datos1 = { id: $('.textC').attr("id") };


    $.ajax({
        url: '/User/GetComment',
        type: 'GET',
        data: datos1,
        success: function (result) {
            $("#getC").html(result);
        },
        error: function () {
            // Manejar errores en la solicitud Ajax
            alert('Error al obtener la vista parcial');
        }
    });
    
    
});
$('#episodes-tab').click(function () {
    var datos = { id: $('.textC').attr("id") };


    $.ajax({
        url: '/User/GetEpisodes',
        type: 'GET',
        data: datos,
        success: function (result) {
            $("#getE").html(result);
        },
        error: function () {
            // Manejar errores en la solicitud Ajax
            alert('Error al obtener la vista parcial');
        }
    });


});


var modal = document.getElementById("modal1");
var btn = document.getElementById("openModal1");
var span = document.getElementsByClassName("closeModal1")[0];

btn.onclick = function () {
    modal.style.display = "block";
}
span.onclick = function () {
    modal.style.display = "none";
}
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

var modal1 = document.getElementById("modal2");
var btn1 = document.getElementById("rate-tab");
var span1 = document.getElementsByClassName("closeModal2")[0];

btn1.onclick = function () {
    modal1.style.display = "block";
}

span1.onclick = function () {
    modal1.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal1.style.display = "none";
    }
}
var rate = null;

$("#rate-5, #rate-4, #rate-3, #rate-2, #rate-1").click(function () {
    rate = $(this).val();
})


$('.btn-rate').click(function () {
    var ratep = document.querySelector(".rate-post");
    var widget = document.querySelector(".star-widget");
    var editBtn = document.querySelector(".edit");
    // Obtener los datos del formulario o crear un objeto de datos
      
    var datos = {
        value: rate,
        id: $('.star-widget').attr("id")
    };
    var datos2 = { id: $('.textC').attr("id") };

    // Realizar la solicitud Ajax
    $.ajax({
        url: '/User/SetRate',
        type: 'POST',
        data: datos,
        success: function (response) {
            // Manejar la respuesta del ActionResult
            widget.style.display = "none";
            ratep.style.display = "block";
            editBtn.onclick = () => {
                widget.style.display = "block";
                ratep.style.display = "none";
            }
            $.ajax({
                url: '/User/GetRate',
                type: 'GET',
                data: datos,
                success: function (result) {
                    $("#rate-prom").html(result.replace(/['"]/g, ""));
                },
                error: function () {
                    // Manejar errores en la solicitud Ajax
                    alert('Error al obtener el dato');
                }
            });
        },
        error: function (error) {
            // Manejar errores en la solicitud Ajax
            console.log(error);
        }
    });

});

$('.slideE').slick({
    autoplay: true,
    dots: true,
    infinite: true,
    speed: 500,
    fade: true,
    cssEase: 'linear'
});

// Write your JavaScript code.

function previewProfilePicture(event) {
    var input = event.target;
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var ProfilePicture = document.getElementById("profile-picture");
            ProfilePicture.src = e.target.result;
            ProfilePicture.alt = 'Profile picture';
        }

        reader.readAsDataURL(input.files[0]);
    }
}