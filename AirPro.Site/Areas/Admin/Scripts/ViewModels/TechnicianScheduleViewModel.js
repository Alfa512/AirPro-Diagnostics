var DayOfWeek = function (day, dayName, start, end, breakStart, breakEnd) {
    var self = this;

    self.dayOfWeek = day;
    self.name = dayName;

    self.start = ko.observable(null);
    if (start) self.start(new moment(start, 'HH:mm'));

    self.end = ko.observable(null);
    if (end) self.end(new moment(end, 'HH:mm'));

    self.breakStart = ko.observable(null);
    if (breakStart && breakStart.length) self.breakStart(new moment(breakStart, 'HH:mm'));

    self.breakEnd = ko.observable(null);
    if (breakEnd && breakEnd.length) self.breakEnd(new moment(breakEnd, 'HH:mm'));

    self.enableBreak = ko.observable(self.breakStart() != null && self.breakEnd() != null);

    self.validBreak = ko.pureComputed(function() {
        if (!self.enableBreak()) return true;

        var s = self.start();
        var e = self.end();
        var bs = self.breakStart();
        var be = self.breakEnd();

        if (bs && be) {
            var result = (moment.duration(bs.diff(s)).asMinutes() >= 0) &&
                (moment.duration(e.diff(be)).asMinutes() >= 0);

            return result && moment.duration(be.diff(bs)).asMinutes() >= 0;
        }

        return false;
    });

    //Total hours computed for the entire work day
    self.totalHours = ko.computed(function () {
        var start = self.start();
        var end = self.end();
        var breatkStart = self.breakStart();
        var breakEnd = self.breakEnd();

        if (start && end) {
            //parse moment object and get the total hours
            var duration = moment.duration(end.diff(start));
            var result = duration.asHours();

            if (breatkStart && breakEnd && self.validBreak()) {
                var breakDuration = moment.duration(breakEnd.diff(breatkStart));
                if (breakDuration.asMinutes() > 0)
                    result = duration.asHours() - breakDuration.asHours();
            }

            return parseFloat(result, 10).toFixed(2);
        } else {
            return 0;
        }
    });

    //Clear function for the entire row hours
    self.clear = function () {
        this.enableBreak(false);

        this.eWidget().data('timepicker').setTime(null);
        this.sWidget().data('timepicker').setTime(null);
        this.b1SWidget().data('timepicker').setTime(null);
        this.b1EWidget().data('timepicker').setTime(null);

        this.end(null);
        this.start(null);
        this.breakStart(null);
        this.breakEnd(null);
    };

    self.addBreak = function() {
        self.enableBreak(true);
    };

    self.removeBreak = function () {
        this.b1SWidget().data('timepicker').setTime(null);
        this.b1EWidget().data('timepicker').setTime(null);
        this.breakStart(null);
        this.breakEnd(null);

        self.enableBreak(false);
    };

    //Widget references to timepicker, these are set via data-bind="widget: sWidget"
    self.sWidget = ko.observable();
    self.eWidget = ko.observable();

    self.b1SWidget = ko.observable();
    self.b1EWidget = ko.observable();
};

var TechnicianScheduleViewModel = function (model) {
    var self = this;
    
    //Map the model into a DayOfWeek object
    var daysArr = ko.utils.arrayMap(model,
        function (item) {
            return new DayOfWeek(item.DayOfWeek, item.Name, item.StartTime, item.EndTime, item.BreakStart, item.BreakEnd);
        });

    //isValid Model
    self.isValid = ko.observable(true);

    //Validation object statuses, only isValid will be observable for two way databinding
    self.validation = {
        totalHoursNegative: {
            isValid: ko.observable(true),
            message: 'Total Daily Hours must be a higher or equal to zero.'
        },
        breakOutOfRange: {
            isValid: ko.observable(true),
            message: 'Break can NOT occur outside of scheduled hours.'
        }
    };

    self.scheduleDays = ko.observableArray(daysArr);

    //Validate function
    self.validate = function () {
        //Set Defaults
        self.isValid(true);
        self.validation.totalHoursNegative.isValid(true);

        //Iterate through all scheduleDays in array
        ko.utils.arrayForEach(self.scheduleDays(),
            function (item) {
                //Validate negative hours
                if (item.totalHours() < 0) {
                    self.isValid(false);
                    self.validation.totalHoursNegative.isValid(false);
                }

                // Validate Break Times.
                if (!item.validBreak()) {
                    self.isValid(false);
                    self.validation.breakOutOfRange.isValid(false);
                }
            });
    };

    //Copy to next element in array
    self.copyNext = function (item) {
        //Get the next element in the array
        var obj = self.scheduleDays()[item.dayOfWeek + 1];
        if (obj) {
            obj.clear();
            obj.sWidget().data('timepicker').setTime(item.start().format('HH:mm'));
            obj.eWidget().data('timepicker').setTime(item.end().format('HH:mm'));
            obj.b1SWidget().data('timepicker').setTime(item.breakStart().format('HH:mm'));
            obj.b1EWidget().data('timepicker').setTime(item.breakEnd().format('HH:mm'));
            obj.enableBreak(item.enableBreak());
        }
    };

    //Copy to previous element in array
    self.copyPrevious = function (item) {
        //Get the next element in the array
        var obj = self.scheduleDays()[item.dayOfWeek - 1];
        if (obj) {
            obj.clear();
            obj.sWidget().data('timepicker').setTime(item.start().format('HH:mm'));
            obj.eWidget().data('timepicker').setTime(item.end().format('HH:mm'));
            obj.b1SWidget().data('timepicker').setTime(item.breakStart().format('HH:mm'));
            obj.b1EWidget().data('timepicker').setTime(item.breakEnd().format('HH:mm'));
            obj.enableBreak(item.enableBreak());
        }
    };

    //Total Hours computed value from all total hours in the scheduleDays array
    self.totalHours = ko.computed(function () {
        var sum = 0;
        //iterate and sum total hours for each row
        ko.utils.arrayForEach(self.scheduleDays(),
            function (item) {
                sum += parseFloat(item.totalHours());
            });

        return sum;
    });

    //totalHours change event handler
    self.totalHours.subscribe(function (value) {
        //Trigger validate as soon as the totalHours computed value changes
        if (value) {
            self.validate();
        }
    });

    //Total hours css computed.
    //Whenever totalHours changes the computed will be executed and evaluate the corresponding css class to be used
    self.totalHoursCss = ko.pureComputed(function () {
        var total = self.totalHours();
        var result = 'label-info';

        if (total > 50) {
            result = 'label-danger';
        }

        if (total < 40) {
            result = 'label-warning';
        }

        if (total >= 40 && total <= 50) {
            result = 'label-success';
        }

        return result;
    });
};