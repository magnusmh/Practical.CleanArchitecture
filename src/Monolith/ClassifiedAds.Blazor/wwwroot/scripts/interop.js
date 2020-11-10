var interop = interop || {};

interop.focusElementById = function (id) {
    setTimeout(function () {
        var element = document.getElementById(id);
        if (element) {
            element.focus();
        }
    }, 200)
};
