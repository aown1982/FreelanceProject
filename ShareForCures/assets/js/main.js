/*
	Arcana by HTML5 UP
	html5up.net | @n33co
	Free for personal and commercial use under the CCA 3.0 license (html5up.net/license)
*/

(function ($) {

    skel.breakpoints({
        wide: '(max-width: 1680px)',
        normal: '(max-width: 1280px)',
        narrow: '(max-width: 980px)',
        narrower: '(max-width: 840px)',
        mobile: '(max-width: 736px)',
        mobilep: '(max-width: 480px)'
    });

    $(function () {

        var $window = $(window),
			$body = $('body');
        var isLogin = $('#hIsUserLoggedIn');
        var loggedIn = false;
        if (isLogin !== undefined && isLogin!==null && isLogin.val()==="yes") {
            loggedIn = true;
        }

        // Disable animations/transitions until the page has loaded.
        $body.addClass('is-loading');

        $(document).ready(function () {
            $body.removeClass('is-loading');
        });

        // Fix: Placeholder polyfill.
        $('form').placeholder();

        // Prioritize "important" elements on narrower.
        skel.on('+narrower -narrower', function () {
            $.prioritize(
                '.important\\28 narrower\\29',
                skel.breakpoint('narrower').active
            );
        });

        // Dropdowns.
        $('#nav > ul').dropotron({
            offsetY: -15,
            hoverDelay: 0,
            alignment: 'center'
        });

        // Off-Canvas Navigation.

        // Title Bar.
        if (loggedIn === false) {
            $(
                    '<div id="titleBar">' +
                    '<div class="LoginSignupMobile">' +
                    '<a id="btnLoginMobile" href="#animatedModal" class="button">Login</a>' +
                    '<a id="btnSignupMobile" href="#animatedModal" class="button">Signup</a>' +
                    '</div> ' +
                    '<a href="#navPanel" class="toggle"></a>' +
                    '</div>'
                )
                .appendTo($body);
        } else {
            $(
                    '<div id="titleBar">' +
                    '<div class="LoginSignupMobile">' +
                    '</div> ' +
                    '<a href="#navPanel" class="toggle"></a>' +
                    '</div>'
                )
                .appendTo($body);
        }
        // Navigation Panel.
        $(
            '<div id="navPanel">' +
                '<nav>' +
                    $('#nav').navList() +
                '</nav>' +
            '</div>'
        )
            .appendTo($body)
            .panel({
                delay: 500,
                hideOnClick: true,
                hideOnSwipe: true,
                resetScroll: true,
                resetForms: true,
                side: 'left',
                target: $body,
                visibleClass: 'navPanel-visible'
            });

        // Fix: Remove navPanel transitions on WP<10 (poor/buggy performance).
        if (skel.vars.os == 'wp' && skel.vars.osVersion < 10)
            $('#titleBar, #navPanel, #page-wrapper')
                .css('transition', 'none');

    });

})(jQuery);