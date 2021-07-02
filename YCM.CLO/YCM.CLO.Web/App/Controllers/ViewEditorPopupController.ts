module Application.Controllers {
	export class ViewEditorPopupController {
		modalInstance: angular.ui.bootstrap.IModalServiceInstance;
		uiService: Application.Services.Contracts.IUIService;
		appBasePath: string = pageOptions.appBasePath;
		waitGifPath: string = pageOptions.appBasePath + "/Content/images/ajax-loader.gif";
		isLoading: boolean = false;
		statusText: string = "Loading";
		analystResearchHeader: Models.IAnalystResearchHeader;
		analystResearchHeaderId: number;
		columns: Array<Date>;
		fieldGroups: Array<Application.Models.IFieldGroup>;
		selectedFieldGroup: any;
		comments: string;
		analystResearchDetails: Array<Models.IAnalystResearchDetail>;
		dataService: Application.Services.Contracts.IDataService;
		modalService: angular.ui.bootstrap.IModalService;
		ngTableParams: any;
		tableParams: any;
		lastUpdatedOn: Date;
		scope: ng.IScope;
		excludedGridOptions: any;
		includedGridOptions: any;
		exGridApi: any;
		inGridApi: any;
		isNew: boolean;
		allFields: Array<Application.Models.ICustomViewField>;
		excludedFields: Array<Application.Models.ICustomViewField>;
		includedFields: Array<Application.Models.ICustomViewField>;
		isSuperUser: boolean = true;
		isPublic: boolean = false;
		isDefault: boolean = false;
		customView: Models.ICustomView;
		//canEdit: boolean;
		viewId: number;
		nameWasChanged: boolean = false;
		error: string;
		labelMap: Map<string, string> = { "asOfDate": "Quarter Ended", "spacer1": "", "seniorLeverage": "Senior Leverage", "totalLeverage": "Total Leverage", "netTotalLeverage": "Net Total Leverage", "fcfDebt": "FCF Debt", "enterpriseValue": "Enterprise Value", "spacer2": "", "ltmRevenues": "LTM Revenues", "ltmebitda": "LTM EBITDA", "ltmfcf": "LTM FCF", "revenues": "Revenues", "yoYGrowth": "YoY Growth", "organicGrowth": "Organic Growth", "cashEBITDA": "Cash EBITDA", "margin": "Margin", "transactionExpenses": "Transaction Expenses", "restructuringAndIntegration": "Restructuring & Integration", "other1": "Other", "pfebitda": "PF EBITDA", "ltmpfebitda": "LTM EBITDA", "pfCostSaves": "PF Cost Saves", "pfAcquisitionAdjustment": "PF Acquisition Adjustment", "covenantEBITDA": "Covenant EBITDA", "interest": "Interest", "cashTaxes": "Cash Taxes", "workingCapital": "Working Capital", "restructuringOneTime": "Restructuring/One-Time", "other2": "Other", "ocf": "OCF", "capitalExpenditures": "Capital Expenditures", "fcf": "FCF", "ablrcf": "ABL/RCF", "firstLienDebt": "First Lien Debt", "totalDebt": "Total Debt", "equityMarketCap": "Equity Market Cap", "cash": "Cash", "comments": "Comments" };
		windowService: ng.IWindowService;

		static $inject = ["application.services.uiService", "application.services.dataService", "$window", "$scope", '$uibModal', "$uibModalInstance", 'NgTableParams', 'viewId'];

		constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, modalService: angular.ui.bootstrap.IModalService, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
			ngTableParams: NgTableParams, viewId: number) {
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
				onRegisterApi(gridApi) {
					vm.exGridApi = gridApi;
					console.log('reg exc');
				},
				appScopeProvider: this,
				columnDefs: [],
			}
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
				onRegisterApi(gridApi) {
					vm.inGridApi = gridApi;
					console.log('reg inc');
				},
				appScopeProvider: this,
				columnDefs: [],
			}
		}

		refreshGrid = () => {
			var vm = this;
			vm.exGridApi.grid.refresh();
		}

		getDisplayText = (key: string) => {
			var vm = this;
			var displayText = vm.labelMap[key];
			return displayText;
		}

		setComments = (detail: Models.IAnalystResearchDetail) => {
			var vm = this;
			vm.comments = detail.comments;
		}

		applyGroupFilter = () => {
			var vm = this;
			var grpId = vm.selectedFieldGroup ? vm.selectedFieldGroup.fieldGroupId : -1;
			if (grpId == -1) {
				vm.excludedFields = vm.allFields
			} else {
				vm.excludedFields = vm.allFields.filter(x => x.fieldGroupId === grpId);
			}
			//vm.refreshGrid();
			vm.excludedGridOptions.data = vm.excludedFields;
		}

		getViewFields = (viewId: number) => {
			var vm = this;
		}

		getView = (viewId: number) => {
			var vm = this;
			vm.dataService.userIsASuperUser().then(isSuperUser => {
				vm.isSuperUser = isSuperUser;
			}).then(() => {
				vm.dataService.getCustomView(viewId).then(v => {
					vm.customView = v;
					//vm.canEdit = (vm.isSuperUser && (vm.isNew || vm.customView.isPublic)) || !vm.customView.isPublic;
					if (vm.isNew)
						vm.customView = <Models.ICustomView>{ viewId: 0 };
				}).then(() => vm.getFieldGroups());
			});
		}

		canSave = () => {
			var vm = this;
			return vm.canEdit() && vm.customView.viewId != 1;
		}

		getFieldGroups = () => {
			var vm = this;
			vm.isLoading = true;
			vm.dataService.getCustomPositionViewFieldGroups().then(groups => {
					vm.fieldGroups = groups.sort(function(a, b) {
						return a.fieldGroupName.toLowerCase().localeCompare(b.fieldGroupName.toLowerCase());
					});
					var x = <Models.IFieldGroup>{ fieldGroupId: -1, fieldGroupName: "All Fields" };
					vm.fieldGroups.unshift(x);
					vm.selectedFieldGroup = vm.fieldGroups[0];
				},
				crap => {
					alert(crap);
				}).then(() => {
				vm.dataService.getAllCustomViewFields().then(fields => {
					if (vm.isNew) {
						var includedFields = fields.filter(x => x.fieldTitle == "ISSUER");
						includedFields[0].sortOrder = 10;
						vm.populateIncludedFields(includedFields);
					} else {
						vm.customView.customViewFields.forEach(f => {
							var flds = fields.filter(x => x.fieldId == f.fieldId);
							if (flds.length) {
								var fld = flds[0];
								f.fieldTitle = fld.fieldTitle;
								f.isHidden = fld.isHidden;
							} else {
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
				})
			});
		}

		populateIncludedFields = (fields: Array<Models.ICustomViewField>) => {
			var vm = this;
			vm.includedFields = fields;
			var header = <Models.ICustomViewField>{ fieldGroupId: -1, sortOrder: 0, fieldTitle: "--- Freeze Frame Fields (*up to 4) ---" };
			vm.includedFields.unshift(header);
			vm.addEmptyFrozenRowsIfNeeded();
			vm.includedFields = vm.sortFieldArraySortOrderAsc(vm.includedFields);
		}

		populateExcludedFields = () => {

		}

		sortFieldArrayAlphaAsc = (arrayToSort: Array<Models.ICustomViewField>) => {
				return arrayToSort.sort(function (a, b) {
					return a.fieldTitle.toLowerCase().localeCompare(b.fieldTitle.toLowerCase());
				});
		}

		sortFieldArraySortOrderAsc = (arrayToSort: Array<Models.ICustomViewField>) => {
			return arrayToSort.sort(function (a, b) {
				return a.sortOrder - b.sortOrder;
			});
		}

		populateGrids = () => {
			var vm = this;
			vm.initializeGrid("excludedGridOptions", "EXCLUDED", false, vm.excludedFields);
			vm.initializeGrid("includedGridOptions", "INCLUDED", true, vm.includedFields);
		}

		initializeGrid = (gridOption: string, displayName: string, applyFreezeFrameBackground: boolean, data: any) => {
			var vm = this;
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
		}

		includeField = () => {
			var vm = this;
			var selectedFields = vm.exGridApi.selection.getSelectedRows();
			var sortOrder = vm.getMaxSortOrder(vm.includedFields);
			selectedFields.forEach(f => {
				sortOrder += 10;
				f.sortOrder = sortOrder;
			});
			vm.moveFields(selectedFields, vm.excludedFields, vm.includedFields
			, vm.excludedGridOptions, vm.includedGridOptions);
			vm.removeIncludedFromAllFields();
			vm.clearSelectedRows(vm.excludedFields, vm.exGridApi);
		}

		excludeField = () => {
			var vm = this;
			var selectedFields = vm.inGridApi.selection.getSelectedRows();
			vm.moveFields(selectedFields, vm.includedFields, vm.excludedFields
				, vm.includedGridOptions, vm.excludedGridOptions);
			if (vm.allFields.length != selectedFields.length) {
				selectedFields.forEach(fld => {
					var exists = vm.allFields.filter(x => x.fieldId == fld.fieldId);
					if (!exists)
						vm.allFields.push(fld);
				})
				vm.allFields = vm.sortFieldArrayAlphaAsc(vm.allFields);
				vm.removeIncludedFromAllFields();
			}
			vm.addEmptyFrozenRowsIfNeeded();
			vm.includedFields = vm.sortFieldArraySortOrderAsc(vm.includedFields);
			vm.includedGridOptions.data = vm.includedFields;
			vm.applyGroupFilter();
			vm.clearSelectedRows(vm.includedFields, vm.inGridApi);
		}

		clearSelectedRows = (data: Array<Models.ICustomViewField>, gridApi: any) => {
			if (!data.length)
				gridApi.selection.clearSelectedRows();
		}

		addEmptyFrozenRowsIfNeeded = () => {
			var vm = this;
			var frozen = vm.includedFields.filter(x => x.sortOrder <= 40);
			if (frozen.length < 5) {
				for (var i = 10; i < 50; i += 10) {
					var f = frozen.filter(x => x.sortOrder == i);
					if (!f.length) {
						var field = <Models.ICustomViewField>{ fieldGroupId: -1, sortOrder: i, fieldTitle: null };
						vm.includedFields.push(field);
					}
				}
			}
		}

		removeAllEmptyFrozenRows = () => {
			var vm = this;
			var frozen = vm.includedFields.filter(x => !x.fieldTitle);
			if (frozen.length) {
				for (var i = vm.includedFields.length - 1; i >= 0; i--) {
					if (!vm.includedFields[i].fieldTitle) {
						vm.includedFields.splice(i, 1);
					}
				}
			}
		}

		getMaxSortOrder = (list: Array<Models.ICustomViewField>) => {
			var regularFields = list.filter(x => x.sortOrder < 9999999);
			var sortOrder = Math.max.apply(null, regularFields.map(function (o) { return o.sortOrder; }));
			return sortOrder;
		}

		getMinSortOrder = (list: Array<Models.ICustomViewField>) => {
			var sortOrder = Math.min.apply(null, list.map(function (o) { return o.sortOrder; }));
			return sortOrder;
		}

		moveFields = (selectedFields: any, fromList: any
			, toList: any, fromGridOption: any
			, toGridOption: any) => {
			var vm = this;
			selectedFields.forEach(fld => {
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
		}

		moveDown = () => {
			var vm = this;
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
				var pinnedSelected = selectedFields.filter(f => f.sortOrder <= 40);
				var pinnedNotSelected = <any>vm.includedFields.filter(x => x.fieldGroupId != -1
					&& x.sortOrder > 10 && x.sortOrder <= 40 && !selectedFields.includes(x));
				itemsToMove = vm.includedFields.filter(f => f.sortOrder >= minSelectedSortOrder
					&& !pinnedNotSelected.includes(f));
				if (maxSelectedSortOrder < 40) {
					var fieldsToMoveUp = vm.includedFields.filter(f => f.sortOrder <= (maxSelectedSortOrder + 10) &&
						f.sortOrder > minSelectedSortOrder &&
						!selectedFields.includes(f));
					var sortOrder = minSelectedSortOrder - 10;
					fieldsToMoveUp.forEach(x => {
						sortOrder++;
						x.sortOrder = sortOrder;
					});
				}
				if (pinnedSelected.length == selectedFields.length)
					itemsToMove = selectedFields;

				vm.adjustSortOrderDown(minSelectedSortOrder, itemsToMove);
			} else {
				if (maxSelectedSortOrder < maxSortOrder) {
					var fieldsToMoveUp = vm.includedFields.filter(f => f.sortOrder <= (maxSelectedSortOrder + 10) &&
						f.sortOrder > minSelectedSortOrder &&
						!selectedFields.includes(f));
					var sortOrder = minSelectedSortOrder - 10;
					fieldsToMoveUp.forEach(x => {
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
		}

		getPinnedRows = () => {
			var vm = this;
			return vm.includedFields.filter(f => f.sortOrder > 0 && f.sortOrder <= 40);
		}

		getMinEmptyPinnedRowSortOrder = () => {
			var vm = this;
			var pinned = vm.getPinnedRows();
			var empties = pinned.filter(f => !f.fieldTitle);
			var min = vm.getMinSortOrder(empties);
			return min;
		}

		moveUp = () => {
			var vm = this;
			var selectedFields = vm.inGridApi.selection.getSelectedRows();

			var minSortOrderOfItemsToMove = vm.getMinSortOrder(selectedFields);
			var maxSortOrderOfItemsToMove = vm.getMaxSortOrder(selectedFields);
			if (minSortOrderOfItemsToMove > 20) {
				vm.removeAllEmptyFrozenRows();
				var positionToMoveUpTo = minSortOrderOfItemsToMove - 10;
				var prevElements = vm.includedFields.filter(f => f.sortOrder >= positionToMoveUpTo && f.sortOrder < maxSortOrderOfItemsToMove
				&& !selectedFields.includes(f));
				if (prevElements.length) {
					var so = maxSortOrderOfItemsToMove == 40 ? 30 : maxSortOrderOfItemsToMove;
					prevElements.forEach(f => {
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

		}

		reSortIncludedFields = () => {
			var vm = this;
			var sortOrder = 0;
			var itemsToSort = vm.sortFieldArraySortOrderAsc(vm.includedFields.filter(f => f.sortOrder > 0));
			itemsToSort.forEach(f => {
				sortOrder += 10;
				f.sortOrder = sortOrder;
			})
		}

		adjustSortOrderDown = (sortOrder: number, list: Array<Models.ICustomViewField>) => {
			list.forEach(f => {
				sortOrder += 10;
				if (sortOrder == 50)
					sortOrder--;
				f.sortOrder = sortOrder;
			});
		}

		adjustSortOrderUp = (sortOrder: number, list: Array<Models.ICustomViewField>) => {
			list.forEach(f => {
				f.sortOrder = f.sortOrder - 10;
			});
		}

		removeIncludedFromAllFields = () => {
			var vm = this;
			vm.includedFields.forEach(fld => {
				for (var i = vm.allFields.length - 1; i >= 0; i--) {
					if (vm.allFields[i].fieldId == fld.fieldId) {
						vm.allFields.splice(i, 1);
					}
				}
			})
		}

        deleteCustomView = () => {
           var vm = this;
			vm.dataService.deleteCustomView(vm.customView).then(views => {
				var crap = views;
				vm.error = null;
				vm.modalInstance.dismiss('delete');
			})
		}

		canEdit(): boolean {
			var vm = this;
			var editable = (vm.isSuperUser && (vm.isNew || vm.customView.isPublic)) || !vm.customView.isPublic;
			return editable;
		}
		
		cloneCustomView = () => {
			var vm = this;
			vm.customView.viewName += "_Clone";
			vm.customView.viewId = 0;
			vm.customView.isPublic = false;
			vm.customView.isDefault = false;
			vm.customView.customViewFields.filter(f => f.fieldGroupId != -1).forEach(f => {
				f.customViewFieldId = 0;
				f.viewId = 0;
			});
		}

		saveCustomView = () => {
			var vm = this;
			if (vm.nameWasChanged || vm.isNew) {
				vm.dataService.viewNameIsTaken(vm.customView.viewName).then((isTaken) => {
					if (isTaken)
						vm.error = "ERROR: There is already a custom view with this Name.  Please edit your View Name."
					else {
						vm.save();
					}
				});
			} else {
				vm.save();
			}
		}

		save = () => {
			var vm = this;
			vm.isLoading = true;
			vm.customView.customViewFields = vm.includedFields.filter(f => f.fieldGroupId != -1);
			vm.dataService.saveCustomView(vm.customView).then(views => {
				var crap = views;
				vm.error = null;
				vm.isLoading = false;
				vm.modalInstance.dismiss('save');
			})
		}

		cancel = () => {
			var vm = this;
			vm.statusText = "Closing";
			vm.modalInstance.dismiss('cancel');
			return vm.customView;
		}

		onNameChanged = () => {
			var vm = this;
			vm.nameWasChanged = vm.customView.viewId > 0;
		}

		confirmDelete = () => {
			var vm = this;
			var modalInstance = vm.modalService.open({
				templateUrl: pageOptions.appBasePath + 'app/views/confirmcustomviewdelete.html?v=' + pageOptions.appVersion,
				controller: 'application.controllers.confirmCustomViewDeleteController',
				controllerAs: 'confirmdelete',
				size: 'sm',
				resolve: {
					sourcedata: () => {
						return null;
					}
				}
			});

            modalInstance.result.then(({ confirm }:any) => {
                if (confirm) {
					vm.deleteCustomView();
				}
			}, () => { });
		}

	}

	angular.module("app").controller("application.controllers.viewEditorPopupController", ViewEditorPopupController);
}