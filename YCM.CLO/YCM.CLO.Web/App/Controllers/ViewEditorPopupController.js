var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ViewEditorPopupController = (function () {
            function ViewEditorPopupController(uiService, dataService, $window, $scope, modalService, $modalInstance, ngTableParams, viewId) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.waitGifPath = pageOptions.appBasePath + "/Content/images/ajax-loader.gif";
                this.isLoading = false;
                this.statusText = "Loading";
                this.isSuperUser = true;
                this.isPublic = false;
                this.isDefault = false;
                this.nameWasChanged = false;
                this.labelMap = { "asOfDate": "Quarter Ended", "spacer1": "", "seniorLeverage": "Senior Leverage", "totalLeverage": "Total Leverage", "netTotalLeverage": "Net Total Leverage", "fcfDebt": "FCF Debt", "enterpriseValue": "Enterprise Value", "spacer2": "", "ltmRevenues": "LTM Revenues", "ltmebitda": "LTM EBITDA", "ltmfcf": "LTM FCF", "revenues": "Revenues", "yoYGrowth": "YoY Growth", "organicGrowth": "Organic Growth", "cashEBITDA": "Cash EBITDA", "margin": "Margin", "transactionExpenses": "Transaction Expenses", "restructuringAndIntegration": "Restructuring & Integration", "other1": "Other", "pfebitda": "PF EBITDA", "ltmpfebitda": "LTM EBITDA", "pfCostSaves": "PF Cost Saves", "pfAcquisitionAdjustment": "PF Acquisition Adjustment", "covenantEBITDA": "Covenant EBITDA", "interest": "Interest", "cashTaxes": "Cash Taxes", "workingCapital": "Working Capital", "restructuringOneTime": "Restructuring/One-Time", "other2": "Other", "ocf": "OCF", "capitalExpenditures": "Capital Expenditures", "fcf": "FCF", "ablrcf": "ABL/RCF", "firstLienDebt": "First Lien Debt", "totalDebt": "Total Debt", "equityMarketCap": "Equity Market Cap", "cash": "Cash", "comments": "Comments" };
                this.refreshGrid = function () {
                    var vm = _this;
                    vm.exGridApi.grid.refresh();
                };
                this.getDisplayText = function (key) {
                    var vm = _this;
                    var displayText = vm.labelMap[key];
                    return displayText;
                };
                this.setComments = function (detail) {
                    var vm = _this;
                    vm.comments = detail.comments;
                };
                this.applyGroupFilter = function () {
                    var vm = _this;
                    var grpId = vm.selectedFieldGroup ? vm.selectedFieldGroup.fieldGroupId : -1;
                    if (grpId == -1) {
                        vm.excludedFields = vm.allFields;
                    }
                    else {
                        vm.excludedFields = vm.allFields.filter(function (x) { return x.fieldGroupId === grpId; });
                    }
                    //vm.refreshGrid();
                    vm.excludedGridOptions.data = vm.excludedFields;
                };
                this.getViewFields = function (viewId) {
                    var vm = _this;
                };
                this.getView = function (viewId) {
                    var vm = _this;
                    vm.dataService.userIsASuperUser().then(function (isSuperUser) {
                        vm.isSuperUser = isSuperUser;
                    }).then(function () {
                        vm.dataService.getCustomView(viewId).then(function (v) {
                            vm.customView = v;
                            //vm.canEdit = (vm.isSuperUser && (vm.isNew || vm.customView.isPublic)) || !vm.customView.isPublic;
                            if (vm.isNew)
                                vm.customView = { viewId: 0 };
                        }).then(function () { return vm.getFieldGroups(); });
                    });
                };
                this.canSave = function () {
                    var vm = _this;
                    return vm.canEdit() && vm.customView.viewId != 1;
                };
                this.getFieldGroups = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getCustomPositionViewFieldGroups().then(function (groups) {
                        vm.fieldGroups = groups.sort(function (a, b) {
                            return a.fieldGroupName.toLowerCase().localeCompare(b.fieldGroupName.toLowerCase());
                        });
                        var x = { fieldGroupId: -1, fieldGroupName: "All Fields" };
                        vm.fieldGroups.unshift(x);
                        vm.selectedFieldGroup = vm.fieldGroups[0];
                    }, function (crap) {
                        alert(crap);
                    }).then(function () {
                        vm.dataService.getAllCustomViewFields().then(function (fields) {
                            if (vm.isNew) {
                                var includedFields = fields.filter(function (x) { return x.fieldTitle == "ISSUER"; });
                                includedFields[0].sortOrder = 10;
                                vm.populateIncludedFields(includedFields);
                            }
                            else {
                                vm.customView.customViewFields.forEach(function (f) {
                                    var flds = fields.filter(function (x) { return x.fieldId == f.fieldId; });
                                    if (flds.length) {
                                        var fld = flds[0];
                                        f.fieldTitle = fld.fieldTitle;
                                        f.isHidden = fld.isHidden;
                                    }
                                    else {
                                        f.isHidden = true;
                                    }
                                });
                                vm.populateIncludedFields(vm.customView.customViewFields);
                            }
                            vm.allFields = vm.sortFieldArrayAlphaAsc(fields);
                            vm.removeIncludedFromAllFields();
                            vm.excludedFields = vm.allFields;
                            vm.populateGrids();
                            vm.isDefault = vm.customView.isDefault;
                            vm.isLoading = false;
                        });
                    });
                };
                this.populateIncludedFields = function (fields) {
                    var vm = _this;
                    vm.includedFields = fields;
                    var header = { fieldGroupId: -1, sortOrder: 0, fieldTitle: "--- Freeze Frame Fields (*up to 4) ---" };
                    vm.includedFields.unshift(header);
                    vm.addEmptyFrozenRowsIfNeeded();
                    vm.includedFields = vm.sortFieldArraySortOrderAsc(vm.includedFields);
                };
                this.populateExcludedFields = function () {
                };
                this.sortFieldArrayAlphaAsc = function (arrayToSort) {
                    return arrayToSort.sort(function (a, b) {
                        return a.fieldTitle.toLowerCase().localeCompare(b.fieldTitle.toLowerCase());
                    });
                };
                this.sortFieldArraySortOrderAsc = function (arrayToSort) {
                    return arrayToSort.sort(function (a, b) {
                        return a.sortOrder - b.sortOrder;
                    });
                };
                this.populateGrids = function () {
                    var vm = _this;
                    vm.initializeGrid("excludedGridOptions", "EXCLUDED", false, vm.excludedFields);
                    vm.initializeGrid("includedGridOptions", "INCLUDED", true, vm.includedFields);
                };
                this.initializeGrid = function (gridOption, displayName, applyFreezeFrameBackground, data) {
                    var vm = _this;
                    var columnDefs = vm[gridOption]['columnDefs'];
                    var columnDef = {
                        field: "fieldTitle",
                        name: "FieldTitle",
                        visible: true,
                        displayName: displayName,
                        enableFiltering: false,
                        //filter: {
                        //	noTerm: true,
                        //	condition: function (searchTerm, cellValue) {
                        //		return cellValue != "ISSUER";
                        //	}
                        //},
                        cellClass: applyFreezeFrameBackground ? function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.sortOrder <= 40) {
                                return 'yellow';
                            }
                        } : null
                    };
                    columnDefs.push(columnDef);
                    var filterColumnDef = {
                        field: "isHidden",
                        name: "Hidden",
                        visible: false,
                        displayName: "Hide Me",
                        enableFiltering: false,
                        filter: {
                            noTerm: true,
                            condition: function (searchTerm, cellValue) {
                                return !cellValue;
                            }
                        },
                        //filter: {
                        //	noTerm: true,
                        //	condition: function (searchTerm, cellValue) {
                        //		return cellValue != true;
                        //	}
                        //},
                        cellClass: applyFreezeFrameBackground ? function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.sortOrder <= 40) {
                                return 'yellow';
                            }
                        } : null
                    };
                    columnDefs.push(filterColumnDef);
                    vm[gridOption].data = data;
                };
                this.includeField = function () {
                    var vm = _this;
                    var selectedFields = vm.exGridApi.selection.getSelectedRows();
                    var sortOrder = vm.getMaxSortOrder(vm.includedFields);
                    selectedFields.forEach(function (f) {
                        sortOrder += 10;
                        f.sortOrder = sortOrder;
                    });
                    vm.moveFields(selectedFields, vm.excludedFields, vm.includedFields, vm.excludedGridOptions, vm.includedGridOptions);
                    vm.removeIncludedFromAllFields();
                    vm.clearSelectedRows(vm.excludedFields, vm.exGridApi);
                };
                this.excludeField = function () {
                    var vm = _this;
                    var selectedFields = vm.inGridApi.selection.getSelectedRows();
                    vm.moveFields(selectedFields, vm.includedFields, vm.excludedFields, vm.includedGridOptions, vm.excludedGridOptions);
                    if (vm.allFields.length != selectedFields.length) {
                        selectedFields.forEach(function (fld) {
                            var exists = vm.allFields.filter(function (x) { return x.fieldId == fld.fieldId; });
                            if (!exists)
                                vm.allFields.push(fld);
                        });
                        vm.allFields = vm.sortFieldArrayAlphaAsc(vm.allFields);
                        vm.removeIncludedFromAllFields();
                    }
                    vm.addEmptyFrozenRowsIfNeeded();
                    vm.includedFields = vm.sortFieldArraySortOrderAsc(vm.includedFields);
                    vm.includedGridOptions.data = vm.includedFields;
                    vm.applyGroupFilter();
                    vm.clearSelectedRows(vm.includedFields, vm.inGridApi);
                };
                this.clearSelectedRows = function (data, gridApi) {
                    if (!data.length)
                        gridApi.selection.clearSelectedRows();
                };
                this.addEmptyFrozenRowsIfNeeded = function () {
                    var vm = _this;
                    var frozen = vm.includedFields.filter(function (x) { return x.sortOrder <= 40; });
                    if (frozen.length < 5) {
                        for (var i = 10; i < 50; i += 10) {
                            var f = frozen.filter(function (x) { return x.sortOrder == i; });
                            if (!f.length) {
                                var field = { fieldGroupId: -1, sortOrder: i, fieldTitle: null };
                                vm.includedFields.push(field);
                            }
                        }
                    }
                };
                this.removeAllEmptyFrozenRows = function () {
                    var vm = _this;
                    var frozen = vm.includedFields.filter(function (x) { return !x.fieldTitle; });
                    if (frozen.length) {
                        for (var i = vm.includedFields.length - 1; i >= 0; i--) {
                            if (!vm.includedFields[i].fieldTitle) {
                                vm.includedFields.splice(i, 1);
                            }
                        }
                    }
                };
                this.getMaxSortOrder = function (list) {
                    var regularFields = list.filter(function (x) { return x.sortOrder < 9999999; });
                    var sortOrder = Math.max.apply(null, regularFields.map(function (o) { return o.sortOrder; }));
                    return sortOrder;
                };
                this.getMinSortOrder = function (list) {
                    var sortOrder = Math.min.apply(null, list.map(function (o) { return o.sortOrder; }));
                    return sortOrder;
                };
                this.moveFields = function (selectedFields, fromList, toList, fromGridOption, toGridOption) {
                    var vm = _this;
                    selectedFields.forEach(function (fld) {
                        toList.push(fld);
                        for (var i = fromList.length - 1; i >= 0; i--) {
                            if (fromList[i].fieldId == fld.fieldId) {
                                fromList.splice(i, 1);
                            }
                        }
                    });
                    vm.allFields = vm.sortFieldArrayAlphaAsc(vm.allFields);
                    vm.includedFields = vm.sortFieldArraySortOrderAsc(vm.includedFields);
                    fromGridOption.data = fromList;
                    toGridOption.data = toList;
                };
                this.moveDown = function () {
                    var vm = _this;
                    var selectedFields = vm.inGridApi.selection.getSelectedRows();
                    var maxSortOrder = vm.getMaxSortOrder(vm.includedFields);
                    var minSelectedSortOrder = vm.getMinSortOrder(selectedFields);
                    var maxSelectedSortOrder = vm.getMaxSortOrder(selectedFields);
                    if (minSelectedSortOrder < 50) {
                        vm.removeAllEmptyFrozenRows();
                        var itemsToMove = selectedFields;
                        //if (maxSelectedSortOrder == 40) {
                        //	var unPinned = vm.includedFields.filter(x => x.sortOrder > 40);
                        //	if (unPinned.length)
                        //		itemsToMove = selectedFields.concat(unPinned);
                        //} else {
                        //	var pinnedSelected = selectedFields.filter(f => f.sortOrder <= 40);
                        //	var pinnedNotSelected = <any>vm.includedFields.filter(x => x.fieldGroupId != -1
                        //		&& x.sortOrder > 10 && x.sortOrder <= 40 && !selectedFields.includes(x));
                        //	itemsToMove = vm.includedFields.filter(f => f.sortOrder >= minSelectedSortOrder
                        //		&& !pinnedNotSelected.includes(f));
                        //	if (pinnedSelected.length == selectedFields.length)
                        //		itemsToMove = selectedFields;
                        //	//var maxPinnedSelectedSortOrder = vm.getMaxSortOrder(pinnedSelected);
                        //	//var minPinnedNotSelectedSortOrder = vm.getMinSortOrder(pinnedNotSelected);
                        //}
                        var pinnedSelected = selectedFields.filter(function (f) { return f.sortOrder <= 40; });
                        var pinnedNotSelected = vm.includedFields.filter(function (x) { return x.fieldGroupId != -1
                            && x.sortOrder > 10 && x.sortOrder <= 40 && !selectedFields.includes(x); });
                        itemsToMove = vm.includedFields.filter(function (f) { return f.sortOrder >= minSelectedSortOrder
                            && !pinnedNotSelected.includes(f); });
                        if (maxSelectedSortOrder < 40) {
                            var fieldsToMoveUp = vm.includedFields.filter(function (f) { return f.sortOrder <= (maxSelectedSortOrder + 10) &&
                                f.sortOrder > minSelectedSortOrder &&
                                !selectedFields.includes(f); });
                            var sortOrder = minSelectedSortOrder - 10;
                            fieldsToMoveUp.forEach(function (x) {
                                sortOrder++;
                                x.sortOrder = sortOrder;
                            });
                        }
                        if (pinnedSelected.length == selectedFields.length)
                            itemsToMove = selectedFields;
                        vm.adjustSortOrderDown(minSelectedSortOrder, itemsToMove);
                    }
                    else {
                        if (maxSelectedSortOrder < maxSortOrder) {
                            var fieldsToMoveUp = vm.includedFields.filter(function (f) { return f.sortOrder <= (maxSelectedSortOrder + 10) &&
                                f.sortOrder > minSelectedSortOrder &&
                                !selectedFields.includes(f); });
                            var sortOrder = minSelectedSortOrder - 10;
                            fieldsToMoveUp.forEach(function (x) {
                                sortOrder++;
                                x.sortOrder = sortOrder;
                            });
                            vm.adjustSortOrderDown(minSelectedSortOrder, selectedFields);
                        }
                    }
                    vm.addEmptyFrozenRowsIfNeeded();
                    vm.reSortIncludedFields();
                    vm.includedFields = vm.sortFieldArraySortOrderAsc(vm.includedFields);
                    vm.includedGridOptions.data = vm.includedFields;
                };
                this.getPinnedRows = function () {
                    var vm = _this;
                    return vm.includedFields.filter(function (f) { return f.sortOrder > 0 && f.sortOrder <= 40; });
                };
                this.getMinEmptyPinnedRowSortOrder = function () {
                    var vm = _this;
                    var pinned = vm.getPinnedRows();
                    var empties = pinned.filter(function (f) { return !f.fieldTitle; });
                    var min = vm.getMinSortOrder(empties);
                    return min;
                };
                this.moveUp = function () {
                    var vm = _this;
                    var selectedFields = vm.inGridApi.selection.getSelectedRows();
                    var minSortOrderOfItemsToMove = vm.getMinSortOrder(selectedFields);
                    var maxSortOrderOfItemsToMove = vm.getMaxSortOrder(selectedFields);
                    if (minSortOrderOfItemsToMove > 20) {
                        vm.removeAllEmptyFrozenRows();
                        var positionToMoveUpTo = minSortOrderOfItemsToMove - 10;
                        var prevElements = vm.includedFields.filter(function (f) { return f.sortOrder >= positionToMoveUpTo && f.sortOrder < maxSortOrderOfItemsToMove
                            && !selectedFields.includes(f); });
                        if (prevElements.length) {
                            var so = maxSortOrderOfItemsToMove == 40 ? 30 : maxSortOrderOfItemsToMove;
                            prevElements.forEach(function (f) {
                                so++;
                                f.sortOrder = so;
                            });
                        }
                        vm.adjustSortOrderUp(minSortOrderOfItemsToMove, selectedFields);
                        vm.addEmptyFrozenRowsIfNeeded();
                        vm.reSortIncludedFields();
                        vm.includedFields = vm.sortFieldArraySortOrderAsc(vm.includedFields);
                        vm.includedGridOptions.data = vm.includedFields;
                    }
                };
                this.reSortIncludedFields = function () {
                    var vm = _this;
                    var sortOrder = 0;
                    var itemsToSort = vm.sortFieldArraySortOrderAsc(vm.includedFields.filter(function (f) { return f.sortOrder > 0; }));
                    itemsToSort.forEach(function (f) {
                        sortOrder += 10;
                        f.sortOrder = sortOrder;
                    });
                };
                this.adjustSortOrderDown = function (sortOrder, list) {
                    list.forEach(function (f) {
                        sortOrder += 10;
                        if (sortOrder == 50)
                            sortOrder--;
                        f.sortOrder = sortOrder;
                    });
                };
                this.adjustSortOrderUp = function (sortOrder, list) {
                    list.forEach(function (f) {
                        f.sortOrder = f.sortOrder - 10;
                    });
                };
                this.removeIncludedFromAllFields = function () {
                    var vm = _this;
                    vm.includedFields.forEach(function (fld) {
                        for (var i = vm.allFields.length - 1; i >= 0; i--) {
                            if (vm.allFields[i].fieldId == fld.fieldId) {
                                vm.allFields.splice(i, 1);
                            }
                        }
                    });
                };
                this.deleteCustomView = function () {
                    var vm = _this;
                    vm.dataService.deleteCustomView(vm.customView).then(function (views) {
                        var crap = views;
                        vm.error = null;
                        vm.modalInstance.dismiss('delete');
                    });
                };
                this.cloneCustomView = function () {
                    var vm = _this;
                    vm.customView.viewName += "_Clone";
                    vm.customView.viewId = 0;
                    vm.customView.isPublic = false;
                    vm.customView.isDefault = false;
                    vm.customView.customViewFields.filter(function (f) { return f.fieldGroupId != -1; }).forEach(function (f) {
                        f.customViewFieldId = 0;
                        f.viewId = 0;
                    });
                };
                this.saveCustomView = function () {
                    var vm = _this;
                    if (vm.nameWasChanged || vm.isNew) {
                        vm.dataService.viewNameIsTaken(vm.customView.viewName).then(function (isTaken) {
                            if (isTaken)
                                vm.error = "ERROR: There is already a custom view with this Name.  Please edit your View Name.";
                            else {
                                vm.save();
                            }
                        });
                    }
                    else {
                        vm.save();
                    }
                };
                this.save = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.customView.customViewFields = vm.includedFields.filter(function (f) { return f.fieldGroupId != -1; });
                    vm.dataService.saveCustomView(vm.customView).then(function (views) {
                        var crap = views;
                        vm.error = null;
                        vm.isLoading = false;
                        vm.modalInstance.dismiss('save');
                    });
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                    return vm.customView;
                };
                this.onNameChanged = function () {
                    var vm = _this;
                    vm.nameWasChanged = vm.customView.viewId > 0;
                };
                this.confirmDelete = function () {
                    var vm = _this;
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/confirmcustomviewdelete.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.confirmCustomViewDeleteController',
                        controllerAs: 'confirmdelete',
                        size: 'sm',
                        resolve: {
                            sourcedata: function () {
                                return null;
                            }
                        }
                    });
                    modalInstance.result.then(function (_a) {
                        var confirm = _a.confirm;
                        if (confirm) {
                            vm.deleteCustomView();
                        }
                    }, function () { });
                };
                var vm = this;
                vm.uiService = uiService;
                vm.viewId = viewId;
                vm.isNew = viewId < 1;
                //if (vm.isNew)
                //	vm.customView = <Models.ICustomView>{ viewId: 0 };
                vm.includedFields = [];
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.windowService = $window;
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.lastUpdatedOn = new Date();
                vm.isLoading = true;
                vm.getView(viewId);
                //vm.getFieldGroups();
                this.excludedGridOptions = {
                    rowHeight: 20,
                    enableFiltering: true,
                    enablePinning: true,
                    enableSorting: true,
                    onRegisterApi: function (gridApi) {
                        vm.exGridApi = gridApi;
                        console.log('reg exc');
                    },
                    appScopeProvider: this,
                    columnDefs: [],
                };
                this.includedGridOptions = {
                    rowHeight: 20,
                    enableFiltering: true,
                    enablePinning: true,
                    enableSorting: true,
                    isRowSelectable: function (row) {
                        return (row.entity.sortOrder > 10 && row.entity.fieldGroupId != -1);
                        //if (row.entity.sortOrder <= 10) return false;
                        //return true;
                    },
                    onRegisterApi: function (gridApi) {
                        vm.inGridApi = gridApi;
                        console.log('reg inc');
                    },
                    appScopeProvider: this,
                    columnDefs: [],
                };
            }
            ViewEditorPopupController.prototype.canEdit = function () {
                var vm = this;
                var editable = (vm.isSuperUser && (vm.isNew || vm.customView.isPublic)) || !vm.customView.isPublic;
                return editable;
            };
            return ViewEditorPopupController;
        }());
        ViewEditorPopupController.$inject = ["application.services.uiService", "application.services.dataService", "$window", "$scope", '$uibModal', "$uibModalInstance", 'NgTableParams', 'viewId'];
        Controllers.ViewEditorPopupController = ViewEditorPopupController;
        angular.module("app").controller("application.controllers.viewEditorPopupController", ViewEditorPopupController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ViewEditorPopupController.js.map