
$(document).ready(function() {
    $('[data-toggle="bootgrid"]').each(function (i, v) {
        if ($(v).attr('id') !== 'upload-grid') {
            var settings = new bootgridSettings($(v));
            if (settings) settings.load();
        }
    });
});

var bootgridSettings = function(bootgrid) {
    var self = this;
    var grid = bootgrid;
    var clientHub = $.connection.clientHub;

    self.gridId = grid.attr('id');
    self.loading = ko.observable(false);
    self.rowCount = ko.observable(grid.bootgrid('getRowCount'));
    self.currentPage = ko.observable(grid.bootgrid('getCurrentPage'));
    self.searchPhrase = ko.observable(grid.bootgrid('getSearchPhrase'));
    self.sortDictionary = ko.observable(grid.bootgrid('getSortDictionary'));
    self.columnSettings = ko.mapping.fromJS(grid.bootgrid('getColumnSettings'));

    self.rowCount.subscribe(function () { self.save(); });
    self.currentPage.subscribe(function() { self.save(); });
    self.searchPhrase.subscribe(function () { self.save(); });
    self.sortDictionary.subscribe(function () { self.save(); });
    self.columnSettings.subscribe(function() { self.save(); });
    self.loading.subscribe(function(value) { if (!value) grid.bootgrid('reload'); });

    grid.on("loaded.rs.jquery.bootgrid", function(e) { self.validate(); });
    $(document).ajaxSend(function (e, xhr) { if (self.loading()) xhr.abort(); });

    self.validate = function () {
        if (grid.bootgrid('getRowCount') !== self.rowCount()) {
            self.rowCount(grid.bootgrid('getRowCount'));
        }

        if (grid.bootgrid('getCurrentPage') !== self.currentPage()) {
            self.currentPage(grid.bootgrid('getCurrentPage'));
        }

        if (grid.bootgrid('getSearchPhrase') !== self.searchPhrase()) {
            self.searchPhrase(grid.bootgrid('getSearchPhrase'));
        }

        if (ko.toJSON(grid.bootgrid('getSortDictionary')) !== ko.toJSON(self.sortDictionary())) {
            self.sortDictionary(grid.bootgrid('getSortDictionary'));
        }

        if (ko.toJSON(grid.bootgrid('getColumnSettings')) !== ko.toJSON(self.columnSettings())) {
            ko.mapping.fromJS(grid.bootgrid('getColumnSettings'), self.columnSettings);
        }
    };

    self.load = function () {
        // Load Data.
        $.connection.hub.start().done(function () {
            clientHub.server.getUserSettings(self.gridId).done(function (data) {
                if (data) {
                    self.apply(data);
                }
            }).fail(function(e) {
                console.log(e);
            });
        });
    }

    self.apply = function (data) {
        try {
            // Set Loading.
            self.loading(true);

            // Map Data.
            ko.mapping.fromJSON(data, { 'ignore': ["loading"] }, self);

            // Get Header & Footer.
            var gridHeader = $('#' + self.gridId + '-header');
            var gridFooter = $('#' + self.gridId + '-footer');

            // Set Column Selection.
            if (ko.toJSON(grid.bootgrid('getColumnSettings')) !== ko.toJSON(self.columnSettings())) {
                self.columnSettings().forEach(function (item) {
                    var filter = item.visible() == true ? ":not(:checked)" : ":checked";
                    var col = gridHeader.find('[name="' + item.id() + '"]' + filter);
                    if (col) col.click();
                });
            }

            // Set Search Phrase.
            if (grid.bootgrid('getSearchPhrase') !== self.searchPhrase()) {
                grid.bootgrid('search', self.searchPhrase());
                gridHeader.find('.search-field').on('keyup',
                    function (e) {
                        var t = e.target;
                        if ($(t).val() === '' && $(t).val() !== grid.bootgrid('getSearchPhrase')) {
                            grid.bootgrid('search', $(t).val());
                        }
                    });
            }

            // Set Row Count.
            if (grid.bootgrid('getRowCount') !== self.rowCount()) {
                var rowCount = gridHeader.find('.dropdown-item[data-action="' + self.rowCount() + '"]');
                if (rowCount) rowCount.click();
            }

            // Set Sort Dictionary.
            if (ko.toJSON(grid.bootgrid('getSortDictionary')) !== ko.toJSON(self.sortDictionary())) {
                grid.bootgrid('sort', self.sortDictionary());
            }

            // Set Selected Page.
            if (grid.bootgrid('getCurrentPage') !== self.currentPage()) {
                var setPage = self.currentPage();
                grid.one("loaded.rs.jquery.bootgrid", function(e) {
                    var page = gridFooter.find('[data-page="' + setPage + '"]');
                    if (page) page.click();
                });
            }
        } catch (e) {
            console.log(e);
        } 

        // Finish Update.
        self.loading(false);
        console.log('Bootgrid Settings Loaded.');
    };

    self.save = function () {
        // Check Loading.
        if (self.loading()) return;

        // Validate.
        self.validate();

        // Get Data.
        var update = ko.mapping.toJSON(self, { 'ignore': ["loading"] });

        // Save Data.
        $.connection.hub.start().done(function () {
            clientHub.server.saveUserSettings(self.gridId, update).done(function () {
                console.log('Bootgrid Settings Updated.');
            }).fail(function (e) {
                console.log(e);
            });
        });
    };
}