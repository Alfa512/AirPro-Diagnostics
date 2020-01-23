var TechStatViewModel = function () {
    var self = this;

    self.online = ko.observableArray([]);
    self.schedules = ko.observableArray([]);

    self.onlineBadge = ko.pureComputed(function () {
        var emails = self.online().map(function (obj) { return obj.userEmail(); });
        emails = emails.filter(function (v, i) { return emails.indexOf(v) == i; });
        return emails.length;
    });

    self.refreshOnline = ko.observable(null);
    self.loadOnline = function() {
        $.connection.hub.start().done(function() {
            scanRequestHub.server.getConnections().done(function(data) {
                ko.mapping.fromJS(data, {}, self.online);
                self.refreshOnline = setTimeout(self.loadOnline, 10000); // 10 Sec Refresh.
            });
        });
    };

    self.refreshSchedules = ko.observable(null);
    self.loadSchedules = function() {
        $.connection.hub.start().done(function() {
            scanRequestHub.server.getSchedules().done(function(data) {
                ko.mapping.fromJS(data, {}, self.schedules);
                self.refreshSchedules = setTimeout(self.loadSchedules, 60000); // 1 Min Refresh.
            });
        });
    };
};