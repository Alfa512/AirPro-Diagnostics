var RequestTypeDashboardViewModel = function (options) {
    var self = this;
    self.requestTypes = ko.observableArray();

    self.validationRules = ko.observableArray();
    $.post(options.rulesUrl,
        function (data) {
            ko.mapping.fromJS(data, {}, self.validationRules);
        });

    self.requestTypeEdit = ko.observable(null);
    self.editRequestType = function (d) {
        self.requestTypeEdit(d);
    };

    self.saveRequestType = function () {
        $.post(options.saveUrl,
            ko.mapping.toJS(self.requestTypeEdit),
            function (data) {
                var result = new RequestTypeViewModel(data, self.validationRules);
                self.requestTypeEdit(result);

                if (result.updateResult && result.updateResult.success())
                    setTimeout(function () { $('.modal').modal('hide'); }, 2500);
            });
    };

    self.loadRequestTypes = function () {
        self.requestTypeEdit(null);
        $.post(options.loadUrl,
            function (data) {
                self.requestTypes.removeAll();
                for (var i = 0; i < data.length; i++) {
                    var type = new RequestTypeViewModel(data[i], self.validationRules);
                    self.requestTypes.push(type);
                }
            });
    };
};

var RequestTypeViewModel = function (data, rules) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);

    self.rules = rules;

    if (!self.validationRuleIds())
        self.validationRuleIds = ko.observableArray();

    self.validationRulesDisplay = ko.pureComputed(function() {
        return self.rules().map(r => new ValidationRuleViewModel(r.key(), r.value(), self.validationRuleIds()));
    });
    self.toggleValidationRule = function (d) {
        var id = parseInt(d.validationRuleId());
        if (self.validationRuleIds.indexOf(id) >= 0)
            self.validationRuleIds.remove(id);
        else
            self.validationRuleIds.push(id);
    };

    self.preScanEnabled = ko.pureComputed(function () {
        return self.requestCategoryIds() && self.requestCategoryIds.indexOf(1) >= 0;
    });
    self.togglePreScan = function () {
        if (self.preScanEnabled())
            self.requestCategoryIds.remove(1);
        else
            self.requestCategoryIds.push(1);
    };

    self.postScanEnabled = ko.pureComputed(function () {
        return self.requestCategoryIds() && self.requestCategoryIds.indexOf(2) >= 0;
    });
    self.togglePostScan = function () {
        if (self.postScanEnabled())
            self.requestCategoryIds.remove(2);
        else
            self.requestCategoryIds.push(2);
    };

    self.toggleActive = function () {
        self.activeFlag(!self.activeFlag());
    };

    self.toggleBillable = function () {
        self.billableFlag(!self.billableFlag());
    };

    self.updatedByDisplayName = ko.pureComputed(function () {
        return self.updatedByUserDisplay() && self.updatedByUserDisplay().length > 0
            ? self.updatedByUserDisplay() : self.createdByUserDisplay();
    });

    self.updatedDisplayDate = ko.pureComputed(function () {
        var date = self.updatedDt() ? self.updatedDt() : self.createdDt();
        return formatDate(date);
    });
};

var ValidationRuleViewModel = function (id, text, selected) {
    var self = this;
    self.validationRuleId = ko.observable(id);
    self.validationRuleText = ko.observable(text);
    self.validationRuleSelected = ko.observable(selected && selected.indexOf(parseInt(id)) >= 0 || false);
};

function formatBool(value) {
    return value
        ? "<i class='glyphicon glyphicon-ok-circle' style='color: green;'></i>"
        : "<i class='glyphicon glyphicon-remove-circle' style='color: red;'></i>";
}

function formatCurrency(value) {
    return "$" + value.toFixed(2);
}

function formatDate(value) {
    return value ? moment(value, moment.HTML5_FMT.DATETIME_LOCAL_MS).format('MM/DD/YYYY h:mm:ss A') : '';
}