//Carrusel con bótones de navegación, con temporizador - - - - - - - - - - - - - - - - - - - - - - - - - 

$('.owl-carousel').owlCarousel({
    items: 2,
    margin: 10,

    nav: true,
    /*loop: true,*/

    autoplay: true,
    autoplayTimeout: 4000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
        },
        600: {
            items: 1,
        },
        960: {
            items: 1,
        },
        1500: {
            items: 2,
        },
        2000: {
            items: 2,
        },
        2900: {
            items: 4,
        }
    }
    
})


//Carrusel con bótones de navegación, con temporizador y con movimiento scroll - - - - - - - - - - - - -

//var owl = $('.owl-carousel');
//owl.owlCarousel({
//    //$('.owl-carousel').owlCarousel({
//    items: 1,
//    margin: 10,

//    nav: true,
//    loop: true,

//    autoplay: true,
//    autoplayTimeout: 4000,
//    autoplayHoverPause: true
//})
//owl.on("mousewheel", ".owl-stage", function (e) {
//    if (e.deltaY > 0) {
//        owl.trigger("next.owl");
//    } else {
//        owl.trigger("prev.owl");
//    }
//    e.preventDefault();
//});

//Carrusel con temporizador - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - -

//var owl = $('.owl-carousel');
//owl.owlCarousel({
//    items: 1,
//    loop: true,
//    margin: 10,
//    autoplay: true,
//    autoplayTimeout: 1000,
//    autoplayHoverPause: true
//});
//$('.play').on('click', function () {
//    owl.trigger('play.owl.autoplay', [1000])
//})
//$('.stop').on('click', function () {
//    owl.trigger('stop.owl.autoplay')
//})
//$(document).ready(function () {
//    $(".owl-carousel").owlCarousel();
//});

//Carrusel con bótones de navegación con opciones responsive y con movimiento scroll - - - - - - - - - -

//var owl = $(".owl-carousel");
//owl.owlCarousel({
//    loop: true,
//    nav: true,
//    margin: 10,
//    responsive: {
//        0: {
//            items: 1,
//        },
//        600: {
//            items: 1,
//        },
//        960: {
//            items:2,
//        },
//        1500: {
//            items: 4,
//        },
//        2000: {
//            items: 4,
//        }
//    },
//});
//owl.on("mousewheel", ".owl-stage", function (e) {
//    if (e.deltaY > 0) {
//        owl.trigger("next.owl");
//    } else {
//        owl.trigger("prev.owl");
//    }
//    e.preventDefault();
//});


//Carrusel con opciones responsive y temporizador - - - - - - - - - - - - - - - - - - - - - - - - - - -
//var owl = $('.owl-carousel');
//owl.owlCarousel({
//    items: 4,
//    loop: true,
//    margin: 10,
//    autoplay: true,
//    autoplayTimeout: 1500,
//    autoplayHoverPause: true,
//    responsive: {
//        0: {
//            items: 1,
//        },
//        600: {
//            items: 1,
//        },
//        960: {
//            items: 2,
//        },
//        1500: {
//            items: 4,
//        },
//        2000: {
//            items: 4,
//        }
//    }
//});
//$('.play').on('click', function () {
//    owl.trigger('play.owl.autoplay', [1000])
//})
//$('.stop').on('click', function () {
//    owl.trigger('stop.owl.autoplay')
//})