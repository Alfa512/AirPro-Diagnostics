var ToolDepositViewModel = function (toolDepositId, date, description, amount) {
    var self = this;

    self.toolDepositId = toolDepositId;

    var d = date instanceof Date ? date : new Date(date);
    self.date = ko.observable(d).extend({
        required: {
            message: 'Date field is required'
        }
    });
    self.description = ko.observable(description).extend({
        required: {
            message: 'Description field is required'
        }
    });
    self.amount = ko.observable(amount).extend({
        numeric: {
            max: 10000,
            precision: 2
        },
        required: {
            message: 'Amount field is required'
        }
    });
    self.deleteInd = ko.observable(false);
    self.readOnly = ko.observable(false);
    self.isValid = ko.computed(function() {
        return self.date.isValid() && self.description.isValid() && self.amount() > 0;
    });

    self.validate = function () {
        if (self.deleteInd()) {
            return true;
        }

        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        return self.isValid();
    };
};

var ToolDepositsTabViewModel = function (deposits) {
    var self = this;

    var mDeposits = ko.utils.arrayMap(deposits,
        function (item) {
            return new ToolDepositViewModel(item.toolDepositId, item.date, item.description, item.amount);
        });

    self.deposits = ko.observableArray(mDeposits);

    self.addDeposit = function () {
        self.deposits.unshift(new ToolDepositViewModel(0, new Date(), '', 0));
    };

    self.removeDeposit = function(item) {
        if (item.toolDepositId === 0) {
            self.deposits.remove(item);
        } else {
            item.deleteInd(true);
        }
    };

    self.total = ko.computed(function() {
        var sum = 0;

        ko.utils.arrayForEach(self.deposits(),
            function (item) {
                sum += item.amount();
            });

        return sum.toFixed(2);
    });

    self.validate = function () {
        var isValid = true;

        ko.utils.arrayForEach(self.deposits(),
            function(item) {
                var result = item.validate();
                if (result === false) {
                    isValid = false;
                }
            });

        return isValid;
    };
};

var ToolSubscriptionViewModel = function (toolSubscriptionId, vendor, userName, password) {
    var self = this;

    self.toolSubscriptionId = toolSubscriptionId;
    self.vendor = ko.observable(vendor);
    self.username = ko.observable(userName);
    self.password = ko.observable(password);
};

var ToolSubscriptionsTabViewModel = function ( hondaVersion, fjdsVersion, techStreamVersion, subscriptions) {
    var self = this;

    self.hondaVersion = ko.observable(hondaVersion);
    self.fjdsVersion = ko.observable(fjdsVersion);
    self.techstreamVersion = ko.observable(techStreamVersion);

    var subs = ko.utils.arrayMap(subscriptions,
        function(item) {
            return new ToolSubscriptionViewModel(item.toolSubscriptionId, item.vendor, item.username, item.password);
        });

    self.subscriptions = ko.observableArray(subs);

    self.addSubscription = function(item) {
        self.subscriptions.push(new ToolSubscriptionViewModel(0, '', '', '', ''));
    }
};

var AirProToolViewModel = function (modelDeviceTab, modelSubscriptionsTab, modelDepositsTab) {
    var self = this;

    ko.mapping.fromJS(modelDeviceTab, {}, self);

    //Subscriptions Tab
    var a = modelSubscriptionsTab;
    var subscriptionsTabViewModel = new ToolSubscriptionsTabViewModel(a.hondaVersion, a.fjdsVersion, a.techstreamVersion, a.subscriptions);
    var b = modelDepositsTab;
    var depositsTabViewModel = new ToolDepositsTabViewModel(b.deposits);

    self.subscriptionsTab = ko.observable(subscriptionsTabViewModel);
    self.depositsTab = ko.observable(depositsTabViewModel);
};