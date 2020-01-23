ko.bindingHandlers.timePickerMoment = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize timepicker with some optional options
        var options = allBindingsAccessor().timePickerOptions || {};
        var valueOfElement = ko.unwrap(valueAccessor());
        if (valueOfElement) {
            $(element).val(valueOfElement.format("HH:mm")); //Converting moment object to HH:mm
        }

        var picker = $(element).timepicker(options);
        allBindingsAccessor().widget(picker);

        //when a user changes the date, update the view model
        ko.utils.registerEventHandler(element, "changeTime.timepicker", function (event) {
            var value = valueAccessor();
            if (ko.isObservable(value)) {
                value(moment(event.time.value, 'HH:mm')); //Converting HH:mm to moment
            }
        });
    },
    update: function (element, valueAccessor) {
        var widget = $(element).val();
        //when the view model is updated, update the widget
        if (widget) {
            var date = ko.utils.unwrapObservable(valueAccessor());
            if (date) {
                $(element).val(date.format("HH:mm")); //Converting moment object to HH:mm
            }
        }
    }
};