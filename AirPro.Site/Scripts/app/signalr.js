// SignalR Hub Connection.
var hub = $.connection.clientHub;

if (hub) {
    // Initialize Client Events.
    hub.client.resetButton = function (btn) {
        $('#' + btn).button('reset');
    };

    // Status Indicator.
    function updateStatus(log, css, text) {
        console.log(log);
        var status = $("#onlineStatusIndicator");
        status.attr('title', text);
        status.attr('data-original-title', text);
        status.removeClass().addClass("label " + css);
    }

    // Connection Events.
    $.connection.hub
        .starting(function () { updateStatus("Connection Starting...", "label-info", "Connecting..."); })
        .reconnecting(function () { updateStatus("Reconnecting...", "label-info", "Reconnecting...") })
        .reconnected(function () { updateStatus("Connection Established.", "label-success", "Site Connected"); })
        .connectionSlow(function() { updateStatus("Connection Degraded...", "label-warning", "Slow Connection"); })
        .disconnected(function() { updateStatus("Disconnected!", "label-danger", "Site Disconnected"); });

    // Start Connection.
    $.connection.hub.qs = { "page" : window.location.pathname };
    $.connection.hub.start()
        .done(function() { updateStatus("Connection Established.", "label-success", "Connected"); })
        .fail(function (e) {
            console.log(e);
            updateStatus("Disconnected!", "label-danger", "Disconnected");
        });
}