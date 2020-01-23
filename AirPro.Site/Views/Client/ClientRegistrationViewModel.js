var UserDetails = function (registrationUserId, emailAddress, firstName, lastName, jobTitle, contactNumber, phoneNumber, timeZoneInfoId, shopBillingNotification, shopReportNotification, password, confirmPassword) {
    var self = this;

    self.registrationUserId = registrationUserId;

    self.emailAddress = ko.observable(emailAddress || '');

    self.firstName = ko.observable(firstName || '')
        .extend({
            required: {
                params: true,
                message: 'First Name field is required'
            }
        });

    self.lastName = ko.observable(lastName || '')
        .extend({
            required: {
                params: true,
                message: 'Last Name field is required'
            }
        });

    self.jobTitle = ko.observable(jobTitle || '');

    self.contactNumber = ko.observable(contactNumber || '');

    self.phoneNumber = ko.observable(phoneNumber || '');

    self.timeZoneInfoId = ko.observable(timeZoneInfoId || '')
        .extend({
            required: {
                params: true,
                message: 'Time Zone field is required'
            }
        });

    self.shopBillingNotification = ko.observable(shopBillingNotification || false);

    self.shopReportNotification = ko.observable(shopReportNotification || false);

    self.password = ko.observable(password || '')
        .extend({
            required: {
                params: true,
                message: 'Password field is required'
            }
        });

    self.confirmPassword = ko.observable(confirmPassword || '')
        .extend({
            required: {
                params: true,
                message: 'Confirm Password field is required'
            },
            equal: {
                params: self.password,
                message: 'Confirm Password should be equal to Password'
            }
        });

    self.isValid = ko.computed(function () {
        return self.firstName.isValid() &&
            self.lastName.isValid() &&
            self.timeZoneInfoId.isValid() &&
            self.password.isValid() &&
            self.confirmPassword.isValid();
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        console.log("UserDetails valid : " + self.isValid());
        return self.isValid();
    };
};

var AccountInformation = function (registrationAccountId, name, address1, address2, city, stateId, zip, phone, fax, billingCycleId, difShopInfo) {
    var self = this;

    self.registrationAccountId = registrationAccountId;

    self.name = ko.observable(name || '')
        .extend({
            required: {
                params: true,
                message: 'Account Name field is required'
            },
            validation: {
                async: true,
                validator: function (val, params, callback) {

                    var options = {
                        url: '/Client/IsAccountNameUnique',
                        type: 'POST',
                        data: { name: self.name() },
                        success: callback
                    };

                    $.ajax(options);
                },
                message: 'Account Name already exists'
            }
        });

    self.address1 = ko.observable(address1 || '')
        .extend({
            required: {
                params: true,
                message: 'Address field is required'
            }
        });

    self.address2 = ko.observable(address2 || '');

    self.city = ko.observable(city || '')
        .extend({
            required: {
                params: true,
                message: 'City field is required'
            }
        });

    self.stateId = ko.observable(stateId || '')
        .extend({
            required: {
                params: true,
                message: 'State field is required'
            }
        });

    self.zip = ko.observable(zip || '')
        .extend({
            required: {
                params: true,
                message: 'Zip field is required'
            }
        });

    self.phone = ko.observable(phone || '')
        .extend({
            required: {
                params: true,
                message: 'Phone field is required'
            }
        });

    self.fax = ko.observable(fax || '');

    self.billingCycleId = ko.observable(billingCycleId || '');

    self.validate = function (observable) {
        if (ko.validation.utils.isValidatable(observable))
            ko.validation.validateObservable(observable);
    };
    self.name.subscribe(function (val) {
        self.difShopInfo.valueHasMutated();
    });
    self.difShopInfo = ko.observable(difShopInfo || false).extend({
        validation: {
            async: true,
            validator: function (val, params, callback) {

                var options = {
                    url: '/Client/IsShopNameUnique',
                    type: 'POST',
                    data: { name: self.name(), differentShopInfo: val },
                    success: callback
                };

                $.ajax(options);
            },
            message: 'Shop Name already exists. Please, change Account Name or enter Different Shop Info.'
        }
    });

    self.isValid = ko.computed(function () {
        return self.name.isValid() &&
            self.stateId.isValid() &&
            self.phone.isValid() &&
            self.city.isValid() &&
            self.address1.isValid() &&
            self.zip.isValid() &&
            self.difShopInfo.isValid();
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        console.log("AccontInformation valid : " + self.isValid());
        return self.isValid();
    };
};

var ShopInformation = function (registrationShopId, name, address1, address2, city, stateId, zip, phone, fax) {
    var self = this;

    self.registrationShopId = registrationShopId;

    self.name = ko.observable(name || '')
        .extend({
            required: {
                params: true,
                message: 'Shop Name field is required'
            },
            validation: {
                async: true,
                validator: function (val, params, callback) {

                    var options = {
                        url: '/Client/IsShopNameUnique',
                        type: 'POST',
                        data: { name: self.name() },
                        success: callback
                    };

                    $.ajax(options);
                },
                message: 'Shop Name already exists'
            }
        });

    self.address1 = ko.observable(address1 || '')
        .extend({
            required: {
                params: true,
                message: 'Adress field is required'
            }
        });

    self.address2 = ko.observable(address2 || '');

    self.city = ko.observable(city || '')
        .extend({
            required: {
                params: true,
                message: 'City field is required'
            }
        });

    self.stateId = ko.observable(stateId || '')
        .extend({
            required: {
                params: true,
                message: 'State field is required'
            }
        });

    self.zip = ko.observable(zip || '')
        .extend({
            required: {
                params: true,
                message: 'Zip field is required'
            }
        });

    self.phone = ko.observable(phone || '')
        .extend({
            required: {
                params: true,
                message: 'Phone field is required'
            }
        });

    self.fax = ko.observable(fax || '');

    self.isValid = ko.computed(function () {
        return self.name.isValid() &&
            self.phone.isValid() &&
            self.city.isValid() &&
            self.address1.isValid() &&
            self.zip.isValid() &&
            self.stateId.isValid();
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        console.log("ShopInformation valid : " + self.isValid());
        return self.isValid();
    };
};

var RepairInformation = function (averageMoVolume, directRepairPartners, oemCertifications) {
    var self = this;

    self.averageMoVolume = ko.observable(averageMoVolume);

    self.directRepairPartners = ko.observableArray(directRepairPartners);

    self.oemCertifications = ko.observableArray(oemCertifications);

    self.isValid = ko.computed(function () {
        return true;
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        console.log("RepairInformation valid : " + self.isValid());
        return self.isValid();
    };
};

var ExternalServices = function (cccShopId, sendToMitchell) {
    var self = this;

    self.cccShopId = ko.observable(cccShopId);

    self.sendToMitchell = ko.observable(sendToMitchell);

    self.isValid = ko.computed(function () {
        return true;
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        console.log("ExternalServices valid : " + self.isValid());
        return self.isValid();
    };
};

var ClientRegistrationViewModel = function (options) {
    // Global
    var self = this;
    var urlMethods = options.urlMethods;
    self.showWarning = ko.observable(false);
    self.warningMsg = ko.observable('');
    self.states = ko.observableArray(options.dropdown.states);
    self.billingCycles = ko.observableArray(options.dropdown.billingCycles);
    self.directRepairPartners = ko.observableArray(options.dropdown.directRepairPartners);
    self.oemCertifications = ko.observableArray(options.dropdown.oemCertifications);
    self.registrationId = options.registrationModel.registrationId;

    // Form steps
    self.userDetails = ko.observable(new UserDetails(
        options.registrationModel.registrationUserModel.registrationUserId || 0,
        options.registrationModel.emailAddress || "",
        options.registrationModel.registrationUserModel.firstName || "",
        options.registrationModel.registrationUserModel.lastName || "",
        options.registrationModel.registrationUserModel.jobTitle || "",
        options.registrationModel.registrationUserModel.contactNumber || "",
        options.registrationModel.registrationUserModel.phoneNumber || "",
        options.registrationModel.registrationUserModel.timeZoneInfoId || "",
        options.registrationModel.registrationUserModel.shopBillingNotification || false,
        options.registrationModel.registrationUserModel.shopReportNotification || false,
        options.registrationModel.registrationUserModel.password || "",
        options.registrationModel.registrationUserModel.confirmPassword || ""
    ));
    self.accountInformation = ko.observable(new AccountInformation(
        options.registrationModel.registrationAccountModel.registrationAccountId || 0,
        options.registrationModel.registrationAccountModel.name || "",
        options.registrationModel.registrationAccountModel.address1 || "",
        options.registrationModel.registrationAccountModel.address2 || "",
        options.registrationModel.registrationAccountModel.city || "",
        options.registrationModel.registrationAccountModel.stateId || "",
        options.registrationModel.registrationAccountModel.zip || "",
        options.registrationModel.registrationAccountModel.phone || "",
        options.registrationModel.registrationAccountModel.fax || "",
        options.registrationModel.billingCycleId || "",
        options.registrationModel.registrationAccountModel.difShopInfo || false
    ));
    self.shopInformation = ko.observable(new ShopInformation(
        options.registrationModel.registrationShopModel.registrationShopId || 0,
        options.registrationModel.registrationShopModel.name || "",
        options.registrationModel.registrationShopModel.address1 || "",
        options.registrationModel.registrationShopModel.address2 || "",
        options.registrationModel.registrationShopModel.city || "",
        options.registrationModel.registrationShopModel.stateId || "",
        options.registrationModel.registrationShopModel.zip || "",
        options.registrationModel.registrationShopModel.phone || "",
        options.registrationModel.registrationShopModel.fax || ""
    ));
    self.repairInformation = ko.observable(new RepairInformation(
        options.registrationModel.registrationRepairModel.averageMoVolume || "",
        options.registrationModel.registrationRepairModel.directRepairPartners || [],
        options.registrationModel.registrationRepairModel.oemCertifications || []
    ));
    self.externalServices = ko.observable(new ExternalServices(
        options.registrationModel.registrationExternalServicesModel.cccShopId || "",
        options.registrationModel.registrationExternalServicesModel.sendToMitchell || false
    ));

    // Wizard behaviour
    self.currentStep = ko.observable(options.registrationModel.passedStep || 1);
    self.prevButtonVisible = ko.observable(self.currentStep() > 1);
    self.nextButtonVisible = ko.observable(true);
    self.prevButtonText = ko.observable("< Back");
    self.nextButtonText = ko.observable("Next >");

    if ((self.accountInformation().difShopInfo() && self.currentStep() === 5) || (!self.accountInformation().difShopInfo() && self.currentStep() === 4)) {
        self.nextButtonText("Complete");
    }

    self.getCurrentStepImage = ko.computed(function () {
        return self.accountInformation().difShopInfo() ?
            `url(/Images/Steps/WizardProgress-${self.currentStep()}of5.png)` :
            `url(/Images/Steps/WizardProgress-${self.currentStep()}of4.png)`;
    });

    self.getWizardType = ko.computed(function () {
        return self.accountInformation().difShopInfo() ? "wizard-5-step" : "wizard-4-step";
    });

    self.getActiveStep = function (step) {
        return self.currentStep() === step ? 'active' : '';
    };

    self.showStep = function (step) {
        if (self.accountInformation().difShopInfo())
            return self.currentStep() === step;
        else {
            if (step === 1 || step === 2)
                return self.currentStep() === step;
            if (step === 4 || step === 5)
                return self.currentStep() + 1 === step;
        }
    };

    self.incrementStepClick = function () {
        if (self.accountInformation().difShopInfo()) {
            if (self.currentStep() === 1 && self.userDetails().validate())
                self.incrementStep();
            else if (self.currentStep() === 2 && self.accountInformation().validate())
                self.incrementStep();
            else if (self.currentStep() === 3 && self.shopInformation().validate())
                self.incrementStep();
            else if (self.currentStep() === 4 && self.repairInformation().validate())
                self.incrementStep();
            else if (self.currentStep() === 5 && self.externalServices().validate())
                self.incrementStep();
        }
        else {
            if (self.currentStep() === 1 && self.userDetails().validate())
                self.incrementStep();
            else if (self.currentStep() === 2 && self.accountInformation().validate())
                self.incrementStep();
            else if (self.currentStep() === 3 && self.repairInformation().validate())
                self.incrementStep();
            else if (self.currentStep() === 4 && self.externalServices().validate())
                self.incrementStep();
        }
    };

    self.incrementStep = function () {
        if ((self.accountInformation().difShopInfo() && self.currentStep() < 5) || (!self.accountInformation().difShopInfo() && self.currentStep() < 4)) {
            self.prevButtonVisible(true);
            self.currentStep(self.currentStep() + 1);
            self.saveState(self.currentStep());
        }
        else {
            self.sendForm();
        }
        if ((self.accountInformation().difShopInfo() && self.currentStep() === 5) || (!self.accountInformation().difShopInfo() && self.currentStep() === 4))
            self.nextButtonText("Complete");

        enableInputMasking();
    };

    self.decrementStepClick = function () {
        self.nextButtonText("Next >");
        if (self.currentStep() > 1)
            self.currentStep(self.currentStep() - 1);
        if (self.currentStep() === 1)
            self.prevButtonVisible(false);

        self.saveState(self.currentStep());
        enableInputMasking();
    };

    // Form processing
    self.checkDirectRepairPartners = function (data) {
        return self.repairInformation().directRepairPartners.indexOf(parseInt(data)) !== -1;
    };

    self.changeDirectRepairPartners = function (data, event) {
        if (event.currentTarget.checked)
            self.repairInformation().directRepairPartners.push(parseInt(data.Key));
        else
            self.repairInformation().directRepairPartners.remove(parseInt(data.Key));
    };

    self.checkOEMCertifications = function (data) {
        return self.repairInformation().oemCertifications.indexOf(parseInt(data)) !== -1;
    };

    self.changeOEMCertifications = function (data, event) {
        if (event.currentTarget.checked)
            self.repairInformation().oemCertifications.push(parseInt(data.Key));
        else
            self.repairInformation().oemCertifications.remove(parseInt(data.Key));
    };

    self.isFormValid = function () {
        var uv = self.userDetails().validate();
        var av = self.accountInformation().validate();
        var sv = self.shopInformation().validate();
        var rv = self.repairInformation().validate();
        var ev = self.externalServices().validate();
        return uv && av && sv && rv && ev;
    };

    self.saveState = function (step, afterSaveFunc) {
        var mapping = {
            'ignore': ["validate", "isValid"]
        };
        var model = {
            userDetails: ko.mapping.toJS(self.userDetails(), mapping),
            accountInformation: ko.mapping.toJS(self.accountInformation(), mapping),
            shopInformation: ko.mapping.toJS(self.shopInformation(), mapping),
            repairInformation: ko.mapping.toJS(self.repairInformation(), mapping),
            externalServices: ko.mapping.toJS(self.externalServices(), mapping),
            RegistrationId: self.registrationId,
            email: self.userDetails().emailAddress(),
            passedStep: step
        };
        if (!afterSaveFunc) afterSaveFunc = function () { };

        $.post(urlMethods.create,
            model,
            afterSaveFunc);
    };

    self.sendForm = function () {
        if (self.isFormValid) {
            self.saveState(self.currentStep() + 1, function (response) {
                if (response && response.UpdateResult && response.UpdateResult.Success) {
                    window.location.reload();
                    return;
                } else {
                    self.showWarning(true);
                    self.warningMsg(response.UpdateResult.Message);
                }
            });
        }
    };
};