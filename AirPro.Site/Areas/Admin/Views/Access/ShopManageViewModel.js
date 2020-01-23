//Insurance Company Class
var InsuranceCompany = function (insuranceCompanyId, planId, insuranceCompaniesChoices) {
    var self = this;

    self.insuranceCompanyId = ko.observable(insuranceCompanyId).extend({ required: true });
    self.planId = ko.observable(planId).extend({ required: true });

    var insuranceCompanies = ko.utils.arrayMap(insuranceCompaniesChoices,
        function (item) {
            return {
                InsuranceCompanyId: item.Key,
                InsuranceCompanyName: item.Value
            }
        });

    self.insuranceCompanies = ko.observableArray(insuranceCompanies);

    self.isValid = ko.computed(function () {
        return self.insuranceCompanyId.isValid() && self.planId.isValid();
    });
};

var VehicleMakePricing = function (vehicleMakeId, pricingPlanId, vehicleMakeChoices) {
    var self = this;

    self.vehicleMakeId = ko.observable(vehicleMakeId).extend({ required: true });
    self.pricingPlanId = ko.observable(pricingPlanId).extend({ required: true });

    var vehicleMakes = ko.utils.arrayMap(vehicleMakeChoices,
        function (item) {
            return {
                VehicleMakeId: item.Value,
                VehicleMakeName: item.Text
            }
        });

    self.vehicleMakes = ko.observableArray(vehicleMakes);

    self.isValid = ko.computed(function () {
        return self.vehicleMakeId.isValid() && self.pricingPlanId.isValid();
    });
};

var ContactViewModel = function (shopContactGuid, firstName, lastName, phoneNumber, hasRequests) {
    var self = this;

    self.shopContactGuid = ko.observable(shopContactGuid);
    self.hasRequests = ko.observable(hasRequests);
    self.firstName = ko.observable(firstName).extend({ required: true });
    self.lastName = ko.observable(lastName).extend({ required: true });
    self.phoneNumber = ko.observable(phoneNumber).extend({
        required: true
    });

    self.isValid = ko.computed(function () {
        return self.firstName.isValid() && self.lastName.isValid() && self.phoneNumber.isValid();
    });
};

//Shop Manage ViewModel
var ShopManageViewModel = function (model, insuranceCompaniesChoices, insuranceCompaniesDrpChoices, pricingPlansChoices, estimatePlansChoices, vehicleMakeChoices) {
    var self = this;
    var insuranceCompanies = insuranceCompaniesChoices;

    self.allInsuranceCompanies = ko.utils.arrayMap(insuranceCompaniesChoices,
        function (item) {
            var entry = ko.utils.arrayFirst(model.insuranceCompanies,
                function (item2) {
                    return item2 == item.Key;
                });

            return {
                Key: item.Key,
                Value: item.Value,
                Selected: entry != null
            }
        });

    self.allInsuranceDrpCompanies = ko.utils.arrayMap(insuranceCompaniesDrpChoices,
        function (item) {
            var entry = ko.utils.arrayFirst(model.insuranceCompanies,
                function (item2) {
                    return item2 == item.Key;
                });

            return {
                Key: item.Key,
                Value: item.Value,
                Selected: entry != null
            }
        });

    //Map the pricing plans into a new array
    var pricingPlans = ko.utils.arrayMap(pricingPlansChoices,
        function (item) {
            return {
                PricingPlanId: item.Key,
                PricingPlanName: item.Value,
            }
        });

    self.pricingPlans = ko.observableArray(pricingPlans);

    var estimatePlans = ko.utils.arrayMap(estimatePlansChoices,
        function (item) {
            return {
                EstimatePlanId: item.Key,
                EstimatePlanName: item.Value
            }
        });
    self.estimatePlans = ko.observableArray(estimatePlans);

    var shopInsuranceCompanies = ko.utils.arrayMap(model.shopInsuranceCompaniesPricingPlans,
        function (item) {
            return new InsuranceCompany(item.InsuranceCompanyId, item.PlanId, insuranceCompanies);
        });

    self.shopInsuranceCompanies = ko.observableArray(shopInsuranceCompanies);

    var contacts = ko.utils.arrayMap(model.contacts,
        function (item) {
            return new ContactViewModel(item.ShopContactGuid, item.FirstName, item.LastName, item.PhoneNumber, item.HasRequests);
        });

    self.contacts = ko.observableArray(contacts);

    self.errors = ko.observable();
    self.contacts.subscribe(function () {
        self.errors(ko.validation.group(self.contacts(), { deep: true }));
        self.errors().showAllMessages(true);
    });

    var shopEstimateInsuranceCompanies = ko.utils.arrayMap(model.shopInsuranceCompaniesEstimatePlans,
        function (item) {
            return new InsuranceCompany(item.InsuranceCompanyId, item.PlanId, insuranceCompanies);
        });

    self.shopEstimateInsuranceCompanies = ko.observableArray(shopEstimateInsuranceCompanies);

    var shopVehicleMakes = ko.utils.arrayMap(model.shopVehicleMakes,
        function (item) {
            return new VehicleMakePricing(item.VehicleMakeId, item.PricingPlanId, vehicleMakeChoices);
        });

    self.shopVehicleMakes = ko.observableArray(shopVehicleMakes);

    self.vehicleMakeChoices = ko.observableArray(vehicleMakeChoices);
    self.allVehicleMakes = ko.utils.arrayMap(vehicleMakeChoices,
        function (item) {
            var entry = ko.utils.arrayFirst(model.vehicleMakes,
                function (item2) {
                    return item2 == item.Value;
                });

            return {
                Text: item.Text,
                Value: item.Value,
                Selected: entry !== null
            };
        });

    //Add Event
    self.addInsuranceCompany = function () {
        //Get the current selection list from all insurance companies into array
        var selectedItems = ko.utils.arrayMap(self.shopInsuranceCompanies(),
            function (item) {
                return item.insuranceCompanyId();
            });

        //if none set to empty array
        if (!selectedItems) {
            selectedItems = [];
        }

        //Filter all the insurance companies catalog excluding the selected items
        var filtered = ko.utils.arrayFilter(insuranceCompanies, function (item) {
            return selectedItems.indexOf(item.Key) === -1;
        });

        //push a new instance to the array
        var insuranceCompany = new InsuranceCompany(0, 0, filtered);
        self.shopInsuranceCompanies.push(insuranceCompany);
    };

    self.addVehicleMake = function () {
        //Get the current selection list from all vehicles into array
        var selectedItems = ko.utils.arrayMap(self.shopVehicleMakes(),
            function (item) {
                return item.vehicleMakeId();
            });

        //if none set to empty array
        if (!selectedItems) {
            selectedItems = [];
        }

        //Filter all the insurance companies catalog excluding the selected items
        var filtered = ko.utils.arrayFilter(vehicleMakeChoices, function (item) {
            return selectedItems.indexOf(item.Value) === -1;
        });

        //push a new instance to the array
        var vehicleMakePricing = new VehicleMakePricing(0, 0, filtered);
        self.shopVehicleMakes.push(vehicleMakePricing);
    };

    //Remove Event
    self.removeInsuranceCompany = function (insuranceCompany) {
        self.shopInsuranceCompanies.remove(insuranceCompany);
    };

    self.addEstimatePlanInsuranceCompany = function () {
        //Get the current selection list from all insurance companies into array
        var selectedItems = ko.utils.arrayMap(self.shopEstimateInsuranceCompanies(),
            function (item) {
                return item.insuranceCompanyId();
            });

        //if none set to empty array
        if (!selectedItems) {
            selectedItems = [];
        }

        //Filter all the insurance companies catalog excluding the selected items
        var filtered = ko.utils.arrayFilter(insuranceCompanies, function (item) {
            return selectedItems.indexOf(item.Key) === -1;
        });

        //push a new instance to the array
        var insuranceCompany = new InsuranceCompany(0, 0, filtered);
        self.shopEstimateInsuranceCompanies.push(insuranceCompany);
    };

    //Remove Event
    self.removeEstimatePlanInsuranceCompany = function (insuranceCompany) {
        self.shopEstimateInsuranceCompanies.remove(insuranceCompany);
    };

    //Remove Event
    self.removeVehicleMake = function (vm) {
        self.shopVehicleMakes.remove(vm);
    };

    self.isValid = ko.computed(function () {
        var entry1 = ko.utils.arrayFirst(self.shopInsuranceCompanies(),
            function (item) {
                return item.isValid() === false;
            });

        var entry2 = ko.utils.arrayFirst(self.shopVehicleMakes(),
            function (item) {
                return item.isValid() === false;
            });

        var entry3 = ko.utils.arrayFirst(self.contacts(),
            function (item) {
                return item.isValid() === false;
            });

        return entry1 === null && entry2 === null && entry3 === null;
    });

    self.isEstimatePlanAddValid = ko.computed(function () {
        var entry1 = ko.utils.arrayFirst(self.shopEstimateInsuranceCompanies(),
            function (item) {
                return item.isValid() === false;
            });

        return entry1 === null;
    });

    self.addContact = function () {
        var newContact = new ContactViewModel();
        self.contacts.push(newContact);
        enableInputMasking();
    };

    self.removeContact = function (contact) {
        if (contact.hasRequests()) {
            alert('Please, unassign this contact from scan requests before removing from shop.');
            return;
        }

        self.contacts.remove(contact);
    };
};