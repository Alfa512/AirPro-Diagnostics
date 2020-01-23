var ScanRequest = function (requestId, repairId, requestTypeId, requestTypeName,
    categoryId, categoryName, notes, problem, shopName, requestCreateDtUtc,
    requestDate, uploadDate, assignedTech, assignedTechContactNumber, assignedTechMobileNumber, vehicle, slaTimeoutMinutes) {
    var self = this;

    var r1 = moment.utc(requestCreateDtUtc);
    var u1 = uploadDate != null ? moment.utc(uploadDate) : null;

    self.currentUtc = ko.observable(moment.utc());
    setInterval(function () { self.currentUtc(moment.utc()); }, 1000);

    self.requestId = ko.observable(requestId);
    self.requestTypeId = ko.observable(requestTypeId);
    self.requestTypeName = ko.observable(requestTypeName);
    self.repairId = ko.observable(repairId);
    self.categoryId = ko.observable(categoryId);
    self.categoryName = ko.observable(categoryName);
    self.notes = ko.observable(notes);
    self.problem = ko.observable(problem);
    self.vehicle = vehicle;
    self.shop = shopName;
    self.requestDate = ko.observable(r1.utcOffset(userOffset));
    self.requestCreateDtUtc = ko.observable(moment.utc(requestCreateDtUtc));
    self.assignedTech = ko.observable(assignedTech || 'Unassigned');
    self.assignedTechContactNumber = ko.observable(assignedTechContactNumber);
    self.assignedTechMobileNumber = ko.observable(assignedTechMobileNumber);
    self.assignedTechPhone = ko.computed(function () {
        var result = '';
        if (self.assignedTechMobileNumber()) {
            result += 'Mobile:  ' + self.assignedTechMobileNumber();
        }
        if (self.assignedTechContactNumber()) {
            result += '\nContact:  ' + self.assignedTechContactNumber();
        }
        return result;
    });

    if (u1 == null) {
        self.uploadDate = ko.observable(null);
    } else {
        self.uploadDate = ko.observable(u1.local());
    }

    self.title = ko.computed(function () {
        return self.requestTypeName() + ' - ' + self.requestId();
    });

    self.aging = ko.computed(function () {
        var dur = moment.duration(self.currentUtc().diff(self.requestDate()));
        var min = parseInt(dur.asMinutes());
        var sec = parseInt(dur.asSeconds());
        if (min === 0) return sec + ' sec ago';
        return min + ' min ago';
    });

    self.slaTimeoutMinutes = ko.observable(slaTimeoutMinutes);
    self.cardClass = ko.computed(function () {
        if (self.assignedTech() != 'Unassigned' && self.uploadDate()) {
            return 'card-green';
        }
        else if (self.assignedTech() != 'Unassigned') {
            return '';
        }

        var startTime = self.currentUtc();
        var duration = moment.duration(startTime.diff(self.requestDate()));
        var minutes = duration.asMinutes();
        if (self.assignedTech() === 'Unassigned' && minutes > self.slaTimeoutMinutes()) {
            return 'card-red';
        }

        return 'card-yellow';
    });
};

var ScanRequestViewModel = function (model, slaTimeoutMinutes) {
    var self = this;

    var scans = ko.utils.arrayMap(model.scans,
        function (scan) {
            return new ScanRequest(scan.RequestId,
                scan.RepairId,
                scan.RequestTypeId,
                scan.RequestTypeName,
                scan.RequestCategoryId,
                scan.RequestCategoryName,
                scan.Notes,
                scan.ProblemDescription,
                scan.ShopName,
                scan.RequestCreateDt,
                scan.ScanUploadDt,
                scan.TechnicianName,
                {
                    vin: scan.VehicleVin,
                    makeName: scan.VehicleMakeName,
                    model: scan.VehicleModelName,
                    year: scan.VehicleYear
                },
                slaTimeoutMinutes);
        });

    self.searchQuery = ko.observable();
    self.scans = ko.observableArray(scans).groupBy('assignedTech');
    self.filteredScans = ko.computed(function () {
        var query = self.searchQuery();
        if (self.searchQuery()) {
            query = self.searchQuery().toLowerCase();
        }
        if (!query) return self.scans;

        var result = ko.observableArray(self.scans().filter(function (scan) {
            return scan.shop.toLowerCase().indexOf(query) > -1 || scan.title().toLowerCase().indexOf(query) > -1 || scan.vehicle.vin.toLowerCase().indexOf(query) > -1 || scan.vehicle.year.indexOf(query) > -1 || scan.vehicle.makeName.toLowerCase().indexOf(query) > -1 || scan.vehicle.model.toLowerCase().indexOf(query) > -1 || scan.assignedTech().toLowerCase().indexOf(query) > -1; 
        })).groupBy('assignedTech');

        return result;
    });
    self.scanGroups = ko.computed(function (item) {
        var arr = [];
        ko.utils.arrayForEach(self.filteredScans()(),
            function (item) {
                if (arr.filter(function (group) { return group.name() == item.assignedTech(); }).length === 0) {
                    arr.push({ name: item.assignedTech, phone: item.assignedTechPhone });
                }
            });

        arr.sort(function (a, b) {
            return a.name() === 'Unassigned' ? -1 : 1;
        });

        return arr;
    });

    self.alertScans = ko.pureComputed(function () {
        return self.scans().filter(function (scan) {
            return scan.assignedTech() == "Unassigned" &&
                moment.duration(scan.currentUtc().diff(scan.requestDate())).asMinutes() > slaTimeoutMinutes;
        });
    });

    self.alertScanCountLast = ko.observable(0);

    
    var inDebounce;
    self.alertScans.subscribe(function (value) {
        if (window.common.notifications.sound.isMuted) return;
        if (value && value.length > 0) {
            var scanCount = value.length;
            if (scanCount > self.alertScanCountLast()) {
                try {
                    clearTimeout(inDebounce);
                    inDebounce = setTimeout(function () {
                        playAudio('alert');
                    }, 100);
                }
                catch (e) {
                    console.log(e);
                }
            } 
            self.alertScanCountLast(scanCount);
        } else {
            self.alertScanCountLast(0);
        }
    });

    self.sortDescending = function () {
        self.scans.sort(function (a, b) {
            return b.requestDate().valueOf() - a.requestDate().valueOf();
        });
    };

    self.sortAscending = function () {
        self.scans.sort(function (a, b) {
            return a.requestDate().valueOf() - b.requestDate().valueOf();
        });
    };

    self.sortAscending();
};