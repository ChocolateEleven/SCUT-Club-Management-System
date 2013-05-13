// Modified from http://buildinternet.com/2009/01/how-to-make-a-smooth-animated-menu-with-jQuery/
$(document).ready(function () {

    //When mouse rolls over
    $("li.NavigationbarItem").mouseover(function () {
        $(this).stop().animate({ height: '150px' }, { queue: false, duration: 600, easing: 'easeOutBounce' })
    });

    //When mouse is removed
    $("li.NavigationbarItem").mouseout(function () {
        $(this).stop().animate({ height: '50px' }, { queue: false, duration: 600, easing: 'easeOutBounce' })
    });

});