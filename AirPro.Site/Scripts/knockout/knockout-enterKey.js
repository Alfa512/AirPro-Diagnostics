//Usage: <input type="text" data-bind="enterKey: callBackFunc" />
ko.bindingHandlers.enterKey = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var callback = valueAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                event.target.blur();
                callback.call(viewModel);
                return false;
            }
            return true;
        });
    }
};