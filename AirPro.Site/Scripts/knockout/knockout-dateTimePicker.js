ko.bindingHandlers.dateTimePicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().dateTimePickerOptions || {};
        var widget = options.widget;

        // Unwrap observable dates
        Object.keys(options).forEach(function (key) {
            options[key] = ko.unwrap(options[key]);
        });

        $(element).datetimepicker(options);
        if (widget) {
            widget($(element).data('DateTimePicker'));
        }

        //when a user changes the date, update the view model
        ko.utils.registerEventHandler(element,
            "dp.change",
            function (event) {
                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    if (event.date === false) {
                        value(new Date());
                    }
                    else if (event.date != null && !(event.date instanceof Date)) {
                        value(event.date.toDate());
                    } else {
                        value(event.date);
                    }
                }
            });

        ko.utils.domNodeDisposal.addDisposeCallback(element,
            function () {
                var picker = $(element).data("DateTimePicker");
                if (picker) {
                    picker.destroy();
                }
            });
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var picker = $(element).data("DateTimePicker");
        //when the view model is updated, update the widget
        if (picker) {
            var koDate = ko.utils.unwrapObservable(valueAccessor());

            //in case return from server datetime i am get in this form for example /Date(93989393)/ then fomat this
            koDate = (typeof (koDate) !== 'object') ? new Date(parseFloat(koDate.replace(/[^0-9]/g, ''))) : koDate;

            picker.date(koDate);

            var options = allBindings().dateTimePickerOptions || {};

            // Unwrap observable dates
            Object.keys(options).forEach(function (key) {
                var val = ko.unwrap(options[key]);
                if (key === 'minDate') {
                    picker.minDate(val);
                }

                if (key === 'maxDate') {
                    picker.maxDate(val);
                }
            });
        }
    }
};