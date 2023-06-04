$('.slide').slick({
    autoplay: true,
    dots: true,
    infinite: true,
    speed: 500,
    fade: true,
    cssEase: 'linear'
});

const tabs = document.querySelectorAll('[data-tab-target]')
const tabContents = document.querySelectorAll('[data-tab-content]')

tabs.forEach(tab => {
    tab.addEventListener('click', () => {
        const target = document.querySelector(tab.dataset.tabTarget)
        tabContents.forEach(tabContent => {
            tabContent.classList.remove('active')
        });
        tabs.forEach(tab => {
            tab.classList.remove('active')
        });
        tab.classList.add('active')
        target.classList.add('active')
    });
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
    });
    var divs = $("div.movie").get().sort(function () {
        return Math.round(Math.random()) - 0.5;

    }).slice(0, 4)
$(divs).appendTo(divs[0].parentNode).show();

$('#btn_c').click(function () {
    var tValue = $('.textC').val();
    var tId = $('.textC').val();
    var url = '@Url.Action("SetComment","User")';

    url += '?text=' + encodeURIComponent(tValue) + '&id=' + encodeURIComponent(tId);
    window.location.href = url;
});

