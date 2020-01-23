var ScanReportViewModel = function (options, data) {
    var self = this;
    var mappings = {
        'troubleCodeRecommendations': {
            create: function (tc) {
                return new ControllerTroubleCodeViewModel(options, tc.data, self.allowEdit);
            }
        }
    };
    ko.mapping.fromJS(data, mappings, self);

    self.loadingData = ko.observable(true);
    self.loadingData.subscribe(function () {
        $('.tooltip').hide();
    });

    self.requestCategoryDisplayName = ko.pureComputed(function () {
        return self.requestCategorySelectionDisplay().filter(function (v, i, s) {
            return v.requestCategoryId() === (self.requestCategoryId() ? self.requestCategoryId() : 0);
        }).map(t => t.requestCategoryName())[0];
    });

    self.requestCategorySelectionDisplay = ko.pureComputed(function () {
        return self.requestTypeSelections().filter(function (v, i, s) {
            return s.map(t => t.requestCategoryId()).indexOf(v.requestCategoryId()) === i;
        });
    });

    self.requestCategorySelectionChange = function (d, e) {
        if (d && d.requestCategoryId() > -1) {
            self.requestCategoryId(d.requestCategoryId());
            self.requestTypeId(null);
        }
    };

    self.requestTypeDisplayName = ko.pureComputed(function () {
        if (!self.requestTypeId()) return 'Selection Required';
        return self.requestTypeSelectionDisplay().filter(function (v, i, s) {
            return v.requestTypeId() === self.requestTypeId();
        }).map(t => t.requestTypeName())[0];
    });

    self.requestTypeSelectionDisplay = ko.pureComputed(function () {
        return self.requestTypeSelections().filter(function (v, i, s) {
            return v.requestCategoryId() === (self.requestCategoryId() ? self.requestCategoryId() : 0);
        });
    });

    self.requestTypeSelectionChange = function (d, e) {
        if (d && d.requestTypeId()) {
            self.requestTypeId(d.requestTypeId());
            self.save();
        }
    };

    self.headerVisible = ko.observable(self.reportHeaderHTML() && self.reportHeaderHTML().length > 0);
    self.toggleHeader = function () { self.headerVisible(!self.headerVisible()); };

    self.notesVisible = ko.observable(self.technicianNotes() && self.technicianNotes().length > 0);
    self.toggleNotes = function () { self.notesVisible(!self.notesVisible()); };

    self.footerVisible = ko.observable(self.reportFooterHTML() && self.reportFooterHTML().length > 0);
    self.toggleFooter = function () { self.footerVisible(!self.footerVisible()); };

    self.airProToolId.subscribe(function (nv) {
        var idx = self.airProToolSelections().map(x => x.airProToolId()).indexOf(nv);
        var tool = self.airProToolSelections()[idx];
        if (!tool) return;

        self.airProToolName(tool.airProToolName());
        self.airProToolPassword(tool.airProToolPassword());
    });

    self.workTypeSearch = ko.observable('');
    self.workTypeSelectionsDisplay = ko.pureComputed(function () {
        if (self.workTypeSelections) {
            return self.workTypeSelections().filter(function (type) {
                var search = self.workTypeSearch().toLowerCase();
                return search.length === 0 ||
                    type.workTypeGroupName().toLowerCase().indexOf(search) >= 0 ||
                    type.workTypeName().toLowerCase().indexOf(search) >= 0;
            });
        }
        return self.workTypeSelections;
    });
    self.workTypeGroups = ko.pureComputed(function () {
        var groups = ko.utils.arrayMap(self.workTypeSelectionsDisplay(),
            function (item) { return item.workTypeGroupName() });
        return ko.utils.arrayGetDistinctValues(groups).sort();
    }, self);

    self.decisionSearch = ko.observable('');
    self.decisionSelectionsDisplay = ko.pureComputed(function () {
        if (self.decisionSelections) {
            return self.decisionSelections().filter(function (decision) {
                var search = self.decisionSearch().toLowerCase();
                return search.length === 0 || decision.decisionText().toLowerCase().indexOf(search) >= 0;
            });
        }
        return self.decisionSelections;
    });

    self.decisionAddText = ko.observable('').extend({ rateLimit: 600 });
    self.decisionAddText.subscribe(function (newVal) {
        self.decisionAddSelections.removeAll();
        if (self.decisionAddText().length > 0) {
            var request = {
                RequestId: self.requestId(),
                Search: newVal
            };
            $.post(options.searchDecisionsUrl,
                request,
                function (d) {
                    ko.mapping.fromJS(d, {}, self.decisionAddSelections);
                });
        }
    });
    self.decisionAddSelections = ko.observableArray();
    self.decisionAddSelectionsDisplay = ko.pureComputed(function () {
        if (self.decisionAddSelections && self.decisionAddSelections().length > 0) {
            return self.decisionAddSelections().filter(function (decision) {
                return !ko.utils.arrayFirst(self.decisionSelections(),
                    function (sel) {
                        return sel.decisionId() === decision.decisionId();
                    });
            });
        }
        return self.decisionAddSelections;
    });
    self.decisionSeverityClick = function (d) {
        if (self.allowEdit() && d.decisionSelected()) {
            d.decisionTextSeverity(d.decisionTextSeverity() + 1);
            if (d.decisionTextSeverity() > 3)
                d.decisionTextSeverity(0);
        }
    };
    self.decisionAddSelect = function (e) {
        var decision = {
            decisionId: e.decisionId,
            decisionText: e.decisionText,
            decisionSelected: ko.observable(true),
            decisionTextSeverity: ko.observable(e.defaultTextSeverity ? e.defaultTextSeverity : 0)
        };
        self.decisionSelections.push(decision);
        self.decisionAddText('');
    };
    self.decisionAddSelectText = function (e) {
        var decision = {
            decisionId: ko.observable(),
            decisionText: ko.observable(self.decisionAddText()),
            decisionSelected: ko.observable(true),
            decisionTextSeverity: ko.observable(0)
        };
        self.decisionSelections.push(decision);
        self.decisionAddText('');
    };
    self.decisionAddTextEnter = function (d, e) {
        if (e.keyCode === 13 && self.decisionAddText()) {
            self.decisionAddSelectText();
        }
        return true;
    };
    self.decisionPercentageDisplay = function (d) {
        if (d) {
            var t = formatPercentage(d.requestTypeUsage());
            var c = formatPercentage(d.requestCategoryUsage());
            var v = formatPercentage(d.vehicleMakeUsage());
            return '(T' + t + '|C' + c + '|V' + v + ')';
        }
        return '';
    };

    self.diagnosticResultSearch = ko.observable('');
    self.diagnosticResultIncludeQueueSelections = ko.observable(false);
    self.diagnosticResultIncludeQueueToggle = function () {
        self.diagnosticResultIncludeQueueSelections(!self.diagnosticResultIncludeQueueSelections());
    };
    self.diagnosticResultSelectionsDisplay = ko.pureComputed(function () {
        return self.diagnosticResultSelections().filter(function (result) {
            return self.diagnosticResultIncludeQueueSelections() === true ||
                result.assignedToRequestInd() === true;
        });
    });
    self.diagnosticResultAssignmentToggle = function (d) {
        $('.tooltip').hide();
        d.assignedToRequestInd(!d.assignedToRequestInd());
    };

    self.diagnosticResultQueueCount = ko.pureComputed(function () {
        return self.diagnosticResultSelections().filter(function (result) {
            return result.assignedToRequestInd() === false;
        }).length;
    });

    self.diagnosticResultDelete = function (d) {
        $.post(options.deleteDiagnosticUrl,
            { resultId: d.resultId() },
            function (r) {
                if (r === true) {
                    self.diagnosticResultSelections.remove(d);
                }
            });
    };

    self.loadedDtDisplay = ko.observable(formatDateDisplay(new Date()));
    self.updatedDtDisplay = ko.pureComputed(function () {
        if (self.updatedDt())
            return formatDateDisplay(self.updatedDt());
        if (self.createdDt() && self.createdDt() !== '0001-01-01T00:00:00')
            return formatDateDisplay(self.createdDt());
        return 'Not Saved';
    });

    self.allowEdit.subscribe(function (nv) {
        if (nv === false)
            self.diagnosticResultIncludeQueueSelections(nv);
    });

    self.displayReport = ko.observable(false);

    self.responsibilityToggle = function (d, e) {
        if (!self.responsibleTechUserId() || self.responsibleTechUserId() !== options.userGuid)
            self.responsibleTechUserId(options.userGuid);
        else
            self.responsibleTechUserId(null);
        self.save();
    };

    self.completeReportToggle = function () {
        $('.tooltip').hide();
        if (self.completeReport() || confirm('This will Complete the Report and Send Notifications when Saved, are you sure?')) {
            self.completeReport(!self.completeReport());
            self.userCompletedInd(self.completeReport());
            if (!self.completeReport()) {
                self.cancelReport(false);
                self.cancelNotes(null);
            }
        }
    };

    self.cancelReportToggle = function () {
        $('.tooltip').hide();
        self.cancelReport(!self.cancelReport());
        if (!self.cancelReport()) {
            self.cancelNotes(null);
            self.cancelReasonTypeId('');
            self.userCompletedInd(self.completeReport());
        }
    };

    self.allowSave = ko.pureComputed(function () {
        return !self.loadingData()
            && (!self.cancelReport() || self.cancelNotes() && self.cancelNotes().length > 0 && self.cancelReasonTypeId())
            && self.requestTypeId();
    });

    var propertyIgnore = { ignore: ["origState", "hasChanges", "updateResult", "responsibilityHistory", "internalNoteHistory", "airProToolSelections", "requestTypeSelections", "reportPreviewHtml", "frequentRecommendationSelections", "currentControllerTroubleCode", "currentTroubleCodeRecommendation", "possibleMissingControllers", "cancelReasonTypes"] };

    self.save = function () {
        self.loadingData(true);
        var update = ko.mapping.toJSON(self, propertyIgnore);
        $.ajax({
            type: "POST",
            url: options.saveReportUrl,
            data: update,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (d) {
            self.load(d);
            console.log('Saved!');
        }).fail(function (e) {
            console.log(e);
        }).always(function () {
            self.loadingData(false);
        });
    };

    self.refresh = function () {
        if (!self.hasChanges() || !self.isValid() || confirm('Any changes you have made will be LOST, are you sure?')) {
            self.loadingData(true);
            $.post(options.loadReportUrl,
                { id: self.requestId() },
                function (d) {
                    self.load(d);
                    console.log('Refreshed!');
                    self.loadingData(false);
                });
        }
    };

    self.load = function (d) {
        ko.mapping.fromJS(d, mappings, self);
        self.loadedDtDisplay(formatDateDisplay(new Date()));
        self.origState(ko.mapping.toJSON(self, propertyIgnore));

        syncTinyMce();
        $('.tooltip').hide();
        setTimeout(function () {
            $('[data-toggle="tooltip"]').tooltip();
        }, 300);

        if (d.updateResult) {
            $('.update-result-alert').show();
            if (d.updateResult.success) {
                $('.update-result-alert').fadeTo(2000, 500).slideUp(500,
                    function () {
                        $('.update-result-alert').slideUp(500);
                    });
            }
        }
    };

    self.viewHistory = ko.observable(true);
    self.viewHistoryToggle = function () {
        self.viewHistory(!self.viewHistory());
    };

    self.isValid = ko.observable(true);
    self.origState = ko.observable(ko.mapping.toJSON(self, propertyIgnore));
    self.hasChanges = ko.pureComputed(function () {
        return self.allowEdit() && (self.origState() !== ko.mapping.toJSON(self, propertyIgnore))
            && self.anyToolSelected()
            && self.versionEnteredForSelectedTools();
    });

    self.loadingData(false);
    self.loadingData.subscribe(function (newVal) {
        if (newVal) self.allowEdit(false);
    });

    self.reportStateTitle = ko.pureComputed(function () {
        if (self.loadingData()) return 'Loading';
        if (self.hasChanges()) return 'Changed';
        if (self.allowEdit()) return 'Ready';
        return 'Disabled';
    });
    self.reportStateCss = ko.pureComputed(function () {
        if (self.loadingData()) return 'fa fa-spinner fa-spin';
        if (self.hasChanges()) return 'glyphicon glyphicon-asterisk warning';
        if (self.allowEdit()) return 'glyphicon glyphicon-ok success';
        return 'glyphicon glyphicon-ban-circle danger';
    });

    self.newDiagnosticTroubleCode = function (controllerId, controllerName) {
        return {
            reportOrderTroubleCodeId: null,
            controllerId: controllerId,
            controllerIdOrig: null,
            controllerName: controllerName,
            controllerNameOrig: null,
            troubleCodeId: null,
            troubleCodeIdOrig: null,
            troubleCode: null,
            troubleCodeOrig: null,
            troubleCodeDescription: null,
            troubleCodeDescriptionOrig: null,
            recommendations: [
                {
                    currentRequestInd: true
                }
            ]
        };
    };

    self.addMissingController = function (controllerId, controllerName) {
        if (self.allowEdit()) {
            var dtc = self.newDiagnosticTroubleCode(controllerId, controllerName);
            var vm = new ControllerTroubleCodeViewModel(options, dtc, self.allowEdit);
            self.selectControllerTroubleCode(vm);
        }
    };

    self.addControllerTroubleCode = function () {
        var dtc = self.newDiagnosticTroubleCode();
        var vm = new ControllerTroubleCodeViewModel(options, dtc, self.allowEdit);
        self.selectControllerTroubleCode(vm);
    };

    self.saveControllerTroubleCode = function () {
        self.currentControllerTroubleCode().troubleCodeTextEdit(false);
        if (self.currentControllerTroubleCode()
            && !self.currentControllerTroubleCode().reportOrderTroubleCodeId()
            && self.currentControllerTroubleCode().troubleCodeUpdateIsValid()) {
            self.troubleCodeRecommendations().push(self.currentControllerTroubleCode());
            self.save();
        }
    };
    self.currentControllerTroubleCode = ko.observable(null);
    self.selectControllerTroubleCode = function (d) {
        if (self.allowEdit()) {
            self.currentControllerTroubleCode(d);
            $('#controller-edit').modal('show');
        }
    };

    self.frequentRecommendationSelectionDisplay = ko.pureComputed(function () {
        return self.frequentRecommendationSelections().filter(function (f) {
            return f.controllerId() === self.currentTroubleCodeRecommendation().controllerId()
                && f.troubleCodeId() === self.currentTroubleCodeRecommendation().troubleCodeId()
                && f.troubleCodeRecommendationId() !== self.currentTroubleCodeRecommendation().troubleCodeRecommendationId();
        }).sort(function (l, r) { return l.troubleCodeRecommendationRank() > r.troubleCodeRecommendationRank() ? 1 : -1; });
    });
    self.frequentRecommendationSelect = function (d) {
        self.currentTroubleCodeRecommendation().troubleCodeRecommendationId(d.troubleCodeRecommendationId());
        self.currentTroubleCodeRecommendation().troubleCodeRecommendationText(d.troubleCodeRecommendationText());
        $('#recommendation-edit').modal('hide');
    };

    self.currentTroubleCodeRecommendation = ko.observable(null);
    self.selectTroubleCodeRecommendation = function (d) {
        if (self.allowEdit() && d.currentRequestInd()) {
            self.currentTroubleCodeRecommendation(d);
            $('#recommendation-edit').modal('show');
        }
    };

    var vehicleMakeToolsMappings = {
        'vehicleMakeTools': {
            create: function (r) {
                return new VehicleMakeToolViewModel(r.data, self.allowEdit);
            }
        }
    };
    ko.mapping.fromJS(data, vehicleMakeToolsMappings, self);

    self.anyToolSelected = ko.pureComputed(function() {
        return self.vehicleMakeTools().filter(function (f) {
            return f.checkedInd();
        }).length > 0 || self.cancelReport() || !self.allowEdit() || self.vehicleMakeTools().length == 0;
    });

    self.versionEnteredForSelectedTools = ko.pureComputed(function () {
        return self.vehicleMakeTools().filter(function(f) {
                return f.checkedInd() && f.toolVersion() && f.toolVersion().length > 0;
        }).length > 0 || self.cancelReport() || !self.allowEdit() || self.vehicleMakeTools().length == 0;
    });
};

var VehicleMakeToolViewModel = function (data, allowEdit) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);
    self.allowEdit = ko.pureComputed(function () {
        return allowEdit();
    });

    self.versionHasError = ko.pureComputed(function() {
        return self.checkedInd() && (!self.toolVersion() || self.toolVersion().length == 0);
    });

    self.checkedIndCss = ko.pureComputed(function () {
        return formatCheckDisplay(self.checkedInd(), self.allowEdit());
    });

    self.checkedIndToggle = function () {
        if (self.allowEdit()) {
            self.checkedInd(!self.checkedInd());
            if (!self.checkedInd()) {
                self.toolVersion('');
            }
        }
    }
}

var ControllerTroubleCodeViewModel = function (options, data, allowEdit) {
    var self = this;
    var mappings = {
        'recommendations': {
            create: function (r) {
                return new RecommendationViewModel(self.controllerId, self.troubleCodeId, r.data, allowEdit);
            }
        }
    };
    ko.mapping.fromJS(data, mappings, self);
    self.allowEdit = allowEdit;

    self.troubleCodeRowCss = ko.pureComputed(function () {
        if (self.troubleCode() &&
            self.recommendations().filter(function (i) {
                return i.currentRequestInd() && !i.codeClearedInd();
            }).length >
            0) return 'danger';
        return self.troubleCodeDescription().toLowerCase().includes('no response') ? 'warning' : 'success';
    });

    self.troubleCodeRowChanged = ko.pureComputed(function () {
        return self.controllerId() !== self.controllerIdOrig()
            || self.troubleCodeId() !== self.troubleCodeIdOrig()
            || self.troubleCode() !== self.troubleCodeOrig()
            || self.troubleCodeDescription() !== self.troubleCodeDescriptionOrig();
    });

    self.troubleCodeRowOrigDisplay = ko.pureComputed(function () {
        return 'Original Value\n'
            + 'Controller: ' + self.controllerNameOrig() + '\n'
            + 'Trouble Code: ' + (self.troubleCodeOrig() ? self.troubleCodeOrig() : 'None') + '\n'
            + 'Description: ' + (self.troubleCodeDescriptionOrig() && self.troubleCodeDescriptionOrig().length > 0
                ? self.troubleCodeDescriptionOrig().substring(0, 100) : 'No Description')
            + (self.troubleCodeDescriptionOrig() && self.troubleCodeDescriptionOrig().length > 100 ? '...' : '');
    });

    self.troubleCodeSelections = ko.observableArray([]);
    self.troubleCodeSearchText = ko.observable('').extend({ rateLimit: 600 });
    self.troubleCodeSearchText.subscribe(function (search) {
        self.troubleCodeSelections.removeAll();
        if (self.troubleCodeSearchText().length > 0) {
            var request = {
                Search: search
            };
            $.post(options.searchTroubleCodesUrl,
                request,
                function (d) {
                    ko.mapping.fromJS(d, {}, self.troubleCodeSelections);
                });
        }
    });
    self.troubleCodeSelectDisplay = function (d) {
        return (d.troubleCode() ? d.troubleCode() : 'No Code') + ' - '
            + (d.troubleCodeDescription() && d.troubleCodeDescription().length > 0
                ? d.troubleCodeDescription() : 'No Description');
    };
    self.troubleCodeSelect = function (d) {
        self.troubleCode(d.troubleCode());
        self.troubleCodeId(d.troubleCodeId());
        self.troubleCodeDescription(d.troubleCodeDescription());
        self.troubleCodeSearchText('');
    };
    self.troubleCodeUpdate = function () {
        self.troubleCode(self.troubleCodeSearchText());
        self.troubleCodeDescription('');
        self.troubleCodeId(null);
        self.troubleCodeSearchText('');
        self.troubleCodeTextEdit(true);
    };
    self.troubleCodeTextEdit = ko.observable(false);
    self.troubleCodeTextEdit.subscribe(function (v) {
        if (v) self.troubleCodeId(null);
    });
    self.troubleCodeTextEditToggle = function () {
        self.troubleCodeTextEdit(!self.troubleCodeTextEdit());
    };
    self.troubleCodeUpdateIsValid = ko.pureComputed(function () {
        return self.controllerName() && self.controllerName().length > 0
            && self.troubleCodeDescription() && self.troubleCodeDescription().length > 0;
    });

    self.controllerSelections = ko.observableArray([]);
    self.controllerSearchText = ko.observable('').extend({ rateLimit: 600 });
    self.controllerSearchText.subscribe(function (newVal) {
        self.controllerSelections.removeAll();
        if (self.controllerSearchText().length > 0) {
            var request = {
                Search: newVal
            };
            $.post(options.searchControllersUrl,
                request,
                function (d) {
                    ko.mapping.fromJS(d, {}, self.controllerSelections);
                });
        }
    });
    self.controllerSelect = function (d) {
        self.controllerName(d.controllerName());
        self.controllerId(d.controllerId());
        self.controllerSearchText('');
    };
    self.controllerUpdate = function () {
        self.controllerName(self.controllerSearchText());
        self.controllerId(null);
        self.controllerSearchText('');
    };

    self.overrideUserDisplay = ko.pureComputed(function () {
        return formatHistoryDisplay('Trouble Code',
            self.overrideCreatedByUserDisplay(),
            self.overrideCreatedDt(),
            self.overrideUpdatedByUserDisplay(),
            self.overrideUpdatedDt());
    });
};

var RecommendationViewModel = function (controllerId, troubleCodeId, data, allowEdit) {
    var self = this;

    self.controllerId = controllerId;
    self.troubleCodeId = troubleCodeId;
    ko.mapping.fromJS(data, {}, self);

    self.allowEdit = ko.pureComputed(function () {
        return allowEdit() && self.currentRequestInd() === true;
    });

    self.requestCategoryDisplay = ko.pureComputed(function () {
        switch (self.requestCategoryId()) {
            case 1:
                return 'PRE';
            case 2:
                return 'PST';
            default:
                return 'OTR';
        }
    });

    self.requestTypeDisplay = ko.pureComputed(function () {
        switch (self.requestTypeId()) {
            case 1:
                return 'QCK';
            case 2:
                return 'DIA';
            case 3:
                return 'CMP';
            case 4:
                return 'FLW';
            case 5:
                return 'INS';
            case 6:
                return 'SLF';
            case 7:
                return 'SAS';
            case 8:
                return 'DMO';
            case 9:
                return 'CAL';
            case 10:
                return 'PGR';
            default:
                return 'UKN';
        }
    });

    self.informCustomerIndCss = ko.pureComputed(function () {
        return formatCheckDisplay(self.informCustomerInd(), self.currentRequestInd() && self.allowEdit());
    });
    self.informCustomerIndToggle = function () {
        if (self.allowEdit())
            self.informCustomerInd(!self.informCustomerInd());
    };

    self.accidentRelatedIndCss = ko.pureComputed(function () {
        return formatCheckDisplay(self.accidentRelatedInd(), self.currentRequestInd() && self.allowEdit());
    });
    self.accidentRelatedIndToggle = function () {
        if (self.allowEdit())
            self.accidentRelatedInd(!self.accidentRelatedInd());
    };

    self.excludeFromReportIndCss = ko.pureComputed(function () {
        return formatCheckDisplay(self.excludeFromReportInd(), self.currentRequestInd() && self.allowEdit());
    });
    self.excludeFromReportIndToggle = function () {
        if (self.allowEdit())
            self.excludeFromReportInd(!self.excludeFromReportInd());
    };

    self.codeClearedIndCss = ko.pureComputed(function () {
        return formatCheckDisplay(self.codeClearedInd(), self.currentRequestInd() && self.allowEdit());
    });
    self.codeClearedIndToggle = function () {
        if (self.allowEdit())
            self.codeClearedInd(!self.codeClearedInd());
    };

    self.diagnosticResultCss = ko.pureComputed(function () {
        return self.resultTroubleCodeId() ? 'glyphicon-ok-sign success' : 'glyphicon-remove-sign danger';
    });

    self.troubleCodeInformationCopy = function () {
        if (self.allowEdit())
            self.troubleCodeNoteText(self.troubleCodeInformationDisplay());
    };

    self.troubleCodeInformationDisplay = ko.pureComputed(function () {
        var i = JSON.parse(self.troubleCodeInformation());
        if (i) return i.join('\n');
        return '';
    });

    self.recommendationTextSeverityCss = ko.pureComputed(function () {
        if (self.recommendationTextSeverity() == 1)
            return 'text-severity-success';
        if (self.recommendationTextSeverity() == 2)
            return 'text-severity-warning';
        if (self.recommendationTextSeverity() == 3)
            return 'text-severity-danger';
        return '';
    });

    self.troubleCodeRecommendationTextEdit = ko.observable(false);
    self.troubleCodeRecommendationTextEdit.subscribe(function (v) {
        if (v) self.troubleCodeRecommendationId(null);
    });
    self.troubleCodeRecommendationTextEditToggle = function () {
        if (self.allowEdit()) {
            self.troubleCodeRecommendationTextEdit(!self.troubleCodeRecommendationTextEdit());
            $('#troubleCodeRecommendationText').focus();
        }
    };
    self.troubleCodeRecommendationTextEditCancel = function () {
        self.troubleCodeRecommendationSearchText('');
        self.troubleCodeRecommendationTextEdit(false);
    };
    self.troubleCodeRecommendationTextUpdate = function () {
        if (self.troubleCodeRecommendationSearchText() && self.troubleCodeRecommendationSearchText().length > 0) {
            self.troubleCodeRecommendationText(self.troubleCodeRecommendationSearchText());
            self.troubleCodeRecommendationId(null);
            self.troubleCodeRecommendationTextEditCancel();
        }
    };

    self.troubleCodeRecommendationSearchText = ko.observable();
    self.troubleCodeRecommendationSearchText.subscribe(function (newVal) {
        self.troubleCodeRecommendationTextSelections.removeAll();
        if (self.troubleCodeRecommendationSearchText().length > 0) {
            var request = {
                Search: newVal
            };
            $.post(options.searchRecommendationsUrl,
                request,
                function (d) {
                    ko.mapping.fromJS(d, {}, self.troubleCodeRecommendationTextSelections);
                });
        }
    });

    self.troubleCodeRecommendationTextSelections = ko.observableArray();
    self.troubleCodeRecommendationTextSelect = function (d) {
        self.troubleCodeRecommendationText(d.troubleCodeRecommendationText());
        self.troubleCodeRecommendationId(d.troubleCodeRecommendationId());
        self.troubleCodeRecommendationTextEditCancel();
    };
    self.troubleCodeRecommendationTextCss = ko.pureComputed(function () {
        var result = "";

        if (self.recommendationTextSeverity() == 1)
            result += "success";
        if (self.recommendationTextSeverity() == 2)
            result += "warning";
        if (self.recommendationTextSeverity() == 3)
            result += "danger";

        if (self.currentRequestInd() && self.allowEdit())
            result += ' clickable';

        return result;
    });
    self.troubleCodeRecommendationTextDisplay = ko.pureComputed(function () {
        if (!self.troubleCodeRecommendationText()) {
            return self.allowEdit()
                ? '<i class="glyphicon glyphicon-plus" style="font-size: 11px;"></i>&nbsp;Add Recommendation'
                : '<em>No Recommendation Entered.<em>';
        }
        return self.troubleCodeRecommendationText();
    });
    self.troubleCodeRecommendationIsValid = ko.pureComputed(function () {
        return !self.troubleCodeRecommendationTextEdit() || (self.troubleCodeRecommendationText() && self.troubleCodeRecommendationText().length > 0);
    });

    self.troubleCodeNoteTextEdit = ko.observable(false);
    self.troubleCodeNoteTextEditToggle = function () {
        if (self.allowEdit())
            self.troubleCodeNoteTextEdit(!self.troubleCodeNoteTextEdit());
    };
    self.troubleCodeNoteTextDisplay = ko.pureComputed(function () {
        if (!self.troubleCodeNoteText()) {
            return self.allowEdit()
                ? '<i class="glyphicon glyphicon-plus" style="font-size: 11px;"></i>&nbsp;Add Note'
                : '<em>No Note Entered.<em>';
        }
        return self.troubleCodeNoteText();
    });

    self.recommendationUserDisplay = ko.pureComputed(function () {
        return formatHistoryDisplay('Recommendation',
            self.recommendationCreatedByUserDisplay(),
            self.recommendationCreatedDt(),
            self.recommendationUpdatedByUserDisplay(),
            self.recommendationUpdatedDt());
    });
};

function formatHistoryDisplay(title, createdBy, createdDt, updatedBy, updatedDt) {
    var result = '';

    if (createdBy) {
        result += title +
            ':\n' +
            'Created By: ' +
            createdBy +
            '\n' +
            'Created Date: ' +
            formatDateDisplay(createdDt);
    }

    if (createdBy && updatedBy) {
        result += '\nUpdated By: ' +
            updatedBy +
            '\n' +
            'Updated Date: ' +
            formatDateDisplay(updatedDt);
    }

    return result;
}

function formatDateDisplay(date) {
    return moment(date, moment.HTML5_FMT.DATETIME_LOCAL_MS).format('MM/DD/YYYY h:mm:ss A');
}

function formatCheckDisplay(checked, enabled) {
    return (checked ? 'glyphicon glyphicon-check' : 'glyphicon glyphicon-unchecked') +
        (enabled ? ' clickable' : '');
}

function formatPercentage(val) {
    if (typeof (val) === 'undefined') return '';
    return (val * 100).toFixed(0) + '%';
}

function syncTinyMce() {
    $('.tinymce').each(function (i, v) {
        var e = $(v).tinymce();
        e.setContent($(e.targetElm).val());
    });
}
