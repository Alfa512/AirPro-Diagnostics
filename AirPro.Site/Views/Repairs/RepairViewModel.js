var Vehicle = function (vin, makeId, model, year, trans, name, selected) {
    var self = this;

    self.manualEntryInd = makeId === 0;

    self.pointsOfImpact = [];
    self.vehicleVin = ko.observable(vin).extend({
        required: true,
        message: 'Vehicle Vin field is required'
    });

    self.vehicleId = ko.observable(makeId).extend({
        required: true,
        message: 'Vehicle Make field is required'
    });

    self.selectedVehicle = ko.observable(selected).extend({
        required: {
            message: 'Vehicle Make field is required'
        }
    });

    self.vehicleMakeName = ko.observable(name || '').extend({
        required: {
            message: 'Vehicle Make Name field is required'
        }
    });

    self.vehicleModel = ko.observable(model || '').extend({
        required: {
            message: 'Vehicle Model field is required'
        }
    });

    self.vehicleYear = ko.observable(year || '').extend({
        required: {
            message: 'Vehicle Year field is required'
        }
    });

    self.vehicleTransmission = ko.observable(trans || '').extend({
        required: {
            message: 'Vehicle Transmission field is required'
        }
    });

    self.isValid = ko.computed(function () {
        return self.vehicleYear.isValid() &&
            self.vehicleModel.isValid() &&
            self.selectedVehicle.isValid() &&
            self.vehicleVin.isValid() &&
            self.vehicleTransmission.isValid();
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        return self.isValid();
    };

    self.enabled = ko.observable(true);
};

var Repair = function (repairId, shopGuid, shopRoNumber, insuranceCompanyId,
    insuranceCompanyOther, insuranceReferenceNumber, odometer, airbagsDeployed, drivableInd, airbagsVisualDeployments) {
    var self = this;

    self.repairId = ko.observable(repairId);
    self.shopGuid = ko.observable(shopGuid || '').extend({
        required: {
            message: 'Shop field is required'
        }
    });
    self.shopRoNumber = ko.observable(shopRoNumber || '');
    self.insuranceCompanyId = ko.observable(insuranceCompanyId || '');
    self.insuranceCompanyOther = ko.observable(insuranceCompanyOther || '');
    self.insuranceReferenceNumber = ko.observable(insuranceReferenceNumber || '');
    self.odometer = ko.observable(odometer || '').extend({
        numeric: {
            max: 999999,
            precision: 0
        }
    });
    self.airbagsDeployed = ko.observable(airbagsDeployed || '');
    self.airbagsVisualDeployments = ko.observable(airbagsVisualDeployments || '');
    self.drivableInd = ko.observable(drivableInd || '');

    self.isValid = ko.computed(function () {
        return self.shopGuid.isValid();
    });

    self.shopGuid.subscribe(function (value) {
        if (value) {
            $.get('/Repairs/ValidateRepairVin',
                { vin: repairViewModel.vehicleVin(), shopGuid: value },
                function (response) {
                    if (response && response.Exists) {
                        repairViewModel.showWarning(true);
                        repairViewModel.warningMsg('Active Repair Exists for VIN: ' +
                            repairViewModel.vehicleVin());
                    } else {
                        repairViewModel.showWarning(false);
                    }
                });
            $.get('/Repairs/ValidateShop', { shopGuid: value }, function (response) {
                console.debug(repairViewModel);
                repairViewModel.canCreateRequest(response.CanCreateRequest);
            });
        }
    });

    self.validate = function () {
        var result = ko.validation.group(self, { deep: true });
        result.showAllMessages();

        return self.isValid();
    };
};

var RepairViewModel = function (options) {
    var self = this;
    var urlMethods = options.urlMethods;

    self.vehicles = ko.observableArray(options.vehicleMakes);
    self.selectedVehicle = ko.observable();

    self.showDialog = ko.observable(false);
    self.showDialog.subscribe(function (value) {
        if (value) {
            self.vehicleVin('');
            self.vehicle(null);
            self.repair(new Repair());
            self.showWarning(false);
        }
    });

    self.vehicleVin = ko.observable('');
    self.vehicle = ko.observable(null);
    self.repair = ko.observable(new Repair());


    self.search = function () {
        if (self.editMode() || self.readOnly()) {
            return;
        }

        if (self.vehicleVin().length < 17) {
            alert('Invalid Vin');
            return;
        }

        $.post(urlMethods.vehicleSearch,
            { vin: self.vehicleVin() },
            function (response) {
                if (response) {
                    self.showWarning(false);
                    var selectedVehicle = ko.utils.arrayFirst(window.repairViewModel.vehicles(),
                        function (item) {
                            return item.Value == response.VehicleMakeId;
                        });

                    var vehicle = new Vehicle(
                        response.VehicleVIN,
                        response.VehicleMakeId,
                        response.VehicleModel,
                        response.VehicleYear,
                        response.VehicleTransmission,
                        response.VehicleMakeTypeName,
                        selectedVehicle);

                    var repair = new Repair();

                    window.repairViewModel.vehicle(vehicle);
                    window.repairViewModel.repair(repair);

                    window.repairViewModel.vehicle().enabled(response.VehicleMakeId === 0);
                    window.repairViewModel.readOnly(false);
                }
            });
    };

    self.showRepair = ko.computed(function () {
        return self.vehicle() != null && self.vehicle().isValid();
    });

    self.saveAndRequest = function () {
        self.save(true);
    };

    self.save = function (redirect) {
        if (self.vehicle() && self.vehicle().validate() && self.repair() && self.repair().validate()) {
            if (!$("#frmCreateRepair").valid()) return;    

            var vehicle = ko.toJS(repairViewModel.vehicle());
            var repair = ko.toJS(repairViewModel.repair());

            vehicle.vehicleMakeId = Number(vehicle.selectedVehicle.Value);
            vehicle.vehicleMakeName = vehicle.selectedVehicle.Text;

            var model = {
                vehicle: vehicle,
                repairOrder: ko.utils.extend(repair, vehicle)
            };

            $.each($('.poi-imageV2[data-selected="1"]'),
                function () {
                    var value = $(this).attr('data-location-id');
                    vehicle.pointsOfImpact.push(value);
                });

            $.post(urlMethods.save,
                model,
                function (response) {
                    if (response && response.UpdateResult && response.UpdateResult.Success) {
                        if (redirect === true) {
                            location.href = urlMethods.requestScan + '/' + response.RepairId;
                            return;
                        }

                        repairViewModel.showDialog(false);

                        $('#alertRepair span.msg').text(response.UpdateResult.Message);
                        $('#alertRepair').fadeTo(3000, 500).slideUp(500,
                            function () {
                                $('#alertRepair').slideUp(500);
                                $('#alertRepair').remove();
                            });

                        $('#repairs-grid').bootgrid("reload");
                    } else {
                        self.showWarning(true);
                        self.warningMsg(response.UpdateResult.Message);
                    }
                });
        }
    }

    self.showWarning = ko.observable(false);
    self.warningMsg = ko.observable('');
    self.btnText = ko.observable('Create');
    self.btnText2 = ko.observable('Create And Request');
    self.readOnly = ko.observable(options.readonly);
    self.readOnly.subscribe(function (value) {
        $('.poi-imageV2').prop('readonly', value);
    });

    self.editMode = ko.observable(false);

    self.title = ko.computed(function () {
        if (self.readOnly()) {
            return 'View Details';
        }
        else if (!self.repair() || self.repair().repairId() === undefined) {
            return 'Create Repair';
        } else {
            return 'Edit Repair';
        }
    });

    self.vehicleVin.subscribe(function (newValue) {
        if (!self.editMode()) {
            var text = newValue;
            if (text.length === 17) {
                self.search();
            }
        }
    });

    self.canCreateRequest = ko.observable(false);
};