(function () {
    var highlight = $.validator.defaults.highlight;
    var unhighlight = $.validator.defaults.unhighlight;

    $.validator.setDefaults({
        highlight: function (element, errorClass, validClass) {
            $(element).parents('.control-group').each(function () {
                var $this = $(this);
                if (!$this.hasClass('highlight-none')) {
                    $this.removeClass('success')
                         .addClass('error');
                }
            });
            highlight(element, errorClass, validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents('.control-group').each(function () {
                var $this = $(this);
                if (!$this.hasClass('highlight-none')) {
                    $this.removeClass('error')
                         .addClass('success');
                }
            });
            unhighlight(element, errorClass, validClass);
        }
    });
})();

$(document).ready(function () {

    $('span.field-validation-valid, span.field-validation-error').each(function () {
        $(this).addClass('help-inline');
    });

    $('form').each(function () {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('error');
            }
        });
    });

});