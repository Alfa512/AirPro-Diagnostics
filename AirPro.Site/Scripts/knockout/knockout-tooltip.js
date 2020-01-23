ko.bindingHandlers.tooltip = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var valueUnwrapped = ko.unwrap(valueAccessor());
        var options = allBindingsAccessor().tooltipOptions || {};
        options.title = valueUnwrapped;
        $(element).tooltip(options);
    }
};