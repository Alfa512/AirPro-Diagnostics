var TimeOffEntry = function(id, start, end, reason) {
    var self = this;

    self.timeOffEntryId = id;
    self.startDate = ko.observable(moment(start || '').toDate());
    self.endDate = ko.observable(moment(end || '').toDate());
    self.reason = ko.observable(reason).extend({
        required: {
            message: 'Reason field is required'
        }
    });
    self.deleteInd = ko.observable(false);
    self.readOnly = ko.observable(id > 0);

    self.startDateTxt = ko.computed(function(item) {
        var mDate = moment(self.startDate());
        return mDate.format("MMM Do HH:mm");
    });
    self.endDateTxt = ko.computed(function (item) {
        var mDate = moment(self.endDate());
        return mDate.format("MMM Do HH:mm");
    });

    self.start = ko.computed(function (item) {
        var ticks = new moment(self.startDate());
        return ticks.format("YYYY-MM-DD HH:mm");
    });
    self.end = ko.computed(function () {
        var mDate = moment(self.endDate());
        return mDate.format("YYYY-MM-DD HH:mm");
    });

    self.sWidget = ko.observable();
    self.eWidget = ko.observable();

    self.edit = function() {
        self.readOnly(false);
    };
};

var TechnicianTimeOffViewModel = function (model) {
    var self = this;

    var mapped = ko.utils.arrayMap(model,
        function(item) {
            return new TimeOffEntry(item.TimeOffEntryId, item.StartDate, item.EndDate, item.Reason);
        });

    self.timeOffEntries = ko.observableArray(mapped);

    self.addEntry = function () {
        self.timeOffEntries.unshift(new TimeOffEntry(0, new Date(), new Date(), ''));
    };

    self.isValid = ko.computed(function() {
        return ko.utils.arrayFirst(self.timeOffEntries(),
            function(item) {
                return item.reason.isValid() === false;
            }) == null;
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        return self.isValid();
    };

    self.remove = function (item) {
        if (item.timeOffEntryId > 0) {
            item.deleteInd(true);
        } else {
            self.timeOffEntries.remove(item);
        }
    };
};