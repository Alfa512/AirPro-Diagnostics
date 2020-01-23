
// Prevent AJAX Caching.
//$.ajaxSetup({ cache: false });

function enableInputMasking() {
    $(".inputmask-phone").inputmask("(999) 999-9999 [x99999]");
    $(".inputmask-phone10").inputmask("(999) 999-9999");
    $(".inputmask-email").inputmask("email");
    $(".inputmask-vin").inputmask("vin");
    $(".inputmask-mileage").inputmask("[999999]");
    $(".inputmask-currency").inputmask("currency");
    $(".inputmask-requestId").inputmask("[999999]");
}

// Initialization.
$(document).ready(function () {
    enableInputMasking();

    // Bootstrap Tooltips.
    $('[data-toggle="tooltip"]').tooltip();

    $('#btnMute').click(function () {
        window.common.notifications.sound.toggleMute();
        var $span = $(this).find('span:first');
        if ($span) {
            $span.removeClass();
            $span.addClass(window.common.notifications.sound.cssClass);
        }
    }).hover(function () {
        $(this).addClass('muteHover');
    },
        function () {
            $(this).removeClass('muteHover');
        });

    $('#userNav').click(function (e) {
        if (e.target.tagName === 'SPAN') {
            $(this).blur();
            e.preventDefault();
        }
    });

    $('input[type="number"]').on('keyup change', numericLimit);
    $('#manageModal').on('keyup change', 'input[type="number"]:not(".ignore")', numericLimit);

    function numericLimit(e) {
        if (/\D/g.test(this.value)) {
            // Filter non-digits from input value.
            this.value = this.value.replace(/\D/g, '');
        }

        if (this.maxLength) {
            if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);
        }
    }

}).ajaxStop(function () {
    $('[data-submit-disable][data-submit-mode="ajax"]').button('reset');
    $('[data-cancel-disable][data-submit-mode="ajax"]').button('reset');
    $('[data-dismiss-disable][data-submit-mode="ajax"]').button('reset');
}).ajaxStart(function () {
    $('[data-submit-disable][data-submit-mode="ajax"]').button('loading');
    $('[data-cancel-disable][data-submit-mode="ajax"]').button('loading');
    $('[data-dismiss-disable][data-submit-mode="ajax"]').button('loading');
});

window.common = (function () {
    var common = {};

    common.notifications = {
        sound: {
            isMuted: false,
            cssClass: 'glyphicon plyphicon-volume-up',
            toggleMute: function () {
                this.isMuted = !this.isMuted;
                var volumeOn = 'glyphicon glyphicon-volume-up';
                var volumeOff = 'glyphicon glyphicon-volume-off';

                this.cssClass = this.isMuted ? volumeOff : volumeOn;

                return this.isMuted;
            }
        }
    };

    common.gridManage = {
        id: '',
        manageBody: 'manageBody',
        modal: {
            id: 'manageModal',
            bodyId: 'manageModalBody',
            labelId: 'manageModalLabel',
            title: 'Modal Title',
            showUpdateMessage: function (html) {
                $('#' + common.gridManage.manageBody).prepend(html);
                $('#' + common.gridManage.manageBody + ' > .alert').fadeTo(3000, 500).slideUp(500,
                    function () {
                        $('#' + common.gridManage.manageBody + ' > .alert').slideUp(500);
                        $('#' + common.gridManage.manageBody + ' > .alert').remove();
                    });
                $('#' + common.gridManage.modal.id).modal('hide');
            },
            load: function (url) {
                $.get(url,
                    function (data) {
                        $('#' + common.gridManage.modal.bodyId).html(data);
                    })
                    .fail(function (err) {
                        console.log(err.message);
                        alert('There was an error contacting the server, please refresh the page and try again.');
                        $('#' + common.gridManage.modal.id).modal('hide');
                    });
            },
            post: function () {
                var form = $('#' + this.bodyId + ' > form');
                $.ajax({
                    type: 'POST',
                    url: form.attr('action'),
                    data: form.serialize()
                })
                    .done(function (data) {
                        $('#' + common.gridManage.modal.bodyId).html(data);
                    })
                    .fail(function (err) {
                        console.log(err.message);
                        alert('There was an error contacting the server, please refresh the page and try again.');
                        $('#' + common.gridManage.modal.id).modal('hide');
                    });
            },
            show: function () {
                $('#' + common.gridManage.modal.labelId).text(this.title);
                $('#' + common.gridManage.modal.id).modal('show');
            }
        },
        setHeaders: function () {
            $('#' + common.gridManage.id + '-header').appendTo('#gridSearch');
            $('.panel-heading .btn-group').addClass('btn-group-sm');
            $('.panel-heading .input-group').addClass('input-group-sm');
        }
    }

    common.getFragment = function getFragment() {
        if (window.location.hash.indexOf("#") === 0) {
            return parseQueryString(window.location.hash.substr(1));
        } else {
            return {};
        }
    };

    function parseQueryString(queryString) {
        var data = {},
            pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

        if (queryString === null) {
            return data;
        }

        pairs = queryString.split("&");

        for (var i = 0; i < pairs.length; i++) {
            pair = pairs[i];
            separatorIndex = pair.indexOf("=");

            if (separatorIndex === -1) {
                escapedKey = pair;
                escapedValue = null;
            } else {
                escapedKey = pair.substr(0, separatorIndex);
                escapedValue = pair.substr(separatorIndex + 1);
            }

            key = decodeURIComponent(escapedKey);
            value = decodeURIComponent(escapedValue);

            data[key] = value;
        }

        return data;
    }

    return common;
})();

function parseCurrency(n) {
    var multiplicator = Math.pow(10, 2);
    n = parseFloat((n * multiplicator).toFixed(11));
    var result = (Math.round(n) / multiplicator);
    return +(result.toFixed(2));
}

//Disabling modal from closing
$.fn.modal.prototype.constructor.Constructor.DEFAULTS.backdrop = 'static';
$.fn.modal.prototype.constructor.Constructor.DEFAULTS.keyboard = false;

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function (m, n) { return args[n]; });
};

function playAudio(fileName) {
    var notify = '/content/' + fileName + '.mp3';
    var audio = new Audio(window.appPath + notify);
    audio.play().catch(function () {
        $('body').append('<embed src="' + notify + '" style="width:0px;height:0px"></embed>');
    });
}

function playNotify() {
    try {
        if (window.common.notifications.sound.isMuted === true) return;
        $(document).ready(function () {
            playAudio('notify');
        });
    } catch (e) {
        console.log(e);
    }
}