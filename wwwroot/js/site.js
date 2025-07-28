$(document).ready(function () {
    const body = $('body');
    const sidebarToggle = $('#sidebarToggle');
    const sidebarOverlay = $('#sidebarOverlay');
    const SIDEBAR_COLLAPSED_KEY = 'sidebar_collapsed';

    function handleSidebarToggle() {
        if ($(window).width() < 992) {
            body.toggleClass('sidebar-mobile-show');
        } else {
            body.toggleClass('sidebar-collapsed');
            localStorage.setItem(SIDEBAR_COLLAPSED_KEY, body.hasClass('sidebar-collapsed'));
        }
    }

    sidebarToggle.on('click', function (e) {
        e.preventDefault();
        handleSidebarToggle();
    });

    sidebarOverlay.on('click', function () {
        body.removeClass('sidebar-mobile-show');
    });

    if ($(window).width() >= 992) {
        if (localStorage.getItem(SIDEBAR_COLLAPSED_KEY) === 'true') {
            body.addClass('sidebar-collapsed');
        }
    }

    $('.submenu-link.active').closest('.submenu').addClass('show');
    $('.submenu-link.active').closest('.nav-item').find('.has-submenu').attr('aria-expanded', 'true');
});