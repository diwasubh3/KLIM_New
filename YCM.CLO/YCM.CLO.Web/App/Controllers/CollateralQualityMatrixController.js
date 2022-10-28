var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var CollateralQualityMatrixController = (function () {
            function CollateralQualityMatrixController(dataService, $rootScope, modalService, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.matrixPoint = {};
                this.matrix = { colGroups: [], rowGroups: [] };
                this.loadFunds = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getFunds().then(function (funds) {
                        vm.funds = funds;
                        vm.isLoading = false;
                    });
                    vm.dataService.loadParameterValuesForParameterType('Matrix Point - Fund Restriction').then(function (params) {
                        vm.parameterValues = params;
                        vm.paramWarfRecovery = params.filter(function (p) { return p.parameterValueText == 'WARF RECOVERY'; })[0].parameterMaxValueNumber;
                        vm.paramSpread = params.filter(function (p) { return p.parameterValueText == 'SPREAD'; })[0].parameterMaxValueNumber;
                        vm.paramWarf = params.filter(function (p) { return p.parameterValueText == 'WARF'; })[0].parameterMaxValueNumber;
                        vm.paramDiversity = params.filter(function (p) { return p.parameterValueText == 'DIVERSITY'; })[0].parameterMaxValueNumber;
                        vm.warfConstant = params.filter(function (p) { return p.parameterValueText == 'WARF CONSTANT'; })[0].parameterMaxValueNumber;
                    });
                    vm.dataService.loadSummaryData().then(function (summs) {
                        vm.summaries = summs;
                    });
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Loading";
                    if (vm.selectedFund) {
                        vm.selectedFundSummary = vm.summaries.filter(function (f) { return f.fundId == vm.selectedFund.fundId; })[0];
                        if (vm.selectedFundSummary.isNewCalc) {
                            vm.paramWarfRecovery = vm.parameterValues.filter(function (p) { return p.parameterValueText == 'WARF NEW RECOVERY'; })[0].parameterMaxValueNumber;
                        }
                        else {
                            vm.paramWarfRecovery = vm.parameterValues.filter(function (p) { return p.parameterValueText == 'WARF RECOVERY'; })[0].parameterMaxValueNumber;
                        }
                        vm.matrixPoint = {};
                        vm.dataService.getMatrixPoints(vm.selectedFund.fundId).then(function (points) {
                            vm.matrixPoints = points;
                            if (points && points.length) {
                                vm.matrixPoint = points[0];
                                vm.matrixPoint.showDetails = true;
                            }
                            vm.matrixPoints.forEach(function (mPoint) {
                                console.log(vm.warfConstant);
                                var moodyRecovery = vm.selectedFundSummary.moodyRecovery.valueOf();
                                var walwarfAdj = vm.selectedFund.walwarfAdj ? vm.selectedFund.walwarfAdj : 0;
                                var diffRecovery = (moodyRecovery - vm.paramWarfRecovery) * mPoint.WarfModifier;
                                if (vm.selectedFundSummary.isNewCalc) {
                                    diffRecovery = diffRecovery < vm.warfConstant ? vm.warfConstant : diffRecovery;
                                }
                                mPoint.cushion = vm.getRoundedUpNumber(((walwarfAdj + mPoint.Warf + diffRecovery) - vm.selectedFundSummary.warf.valueOf())) || 0;
                                mPoint.recovery = vm.getRoundedUpNumber(((walwarfAdj + mPoint.Warf + diffRecovery)));
                            });
                            vm.loadMajors();
                        });
                    }
                };
                this.loadMajors = function () {
                    var vm = _this;
                    vm.dataService.getMajors(vm.selectedFund.fundId).then(function (majors) {
                        vm.matrix = { colGroups: [], rowGroups: [], colDictionary: {}, rowDictionary: {} };
                        vm.selectedDiversity = 0;
                        if (majors.length > 0) {
                            for (var i = 0; i < majors[0].length; i++) {
                                var colGroup = { data: majors[0][i].Diversity, title: majors[0][i].Diversity.toString(), isMinor: false, showMinors: false, };
                                vm.matrix.colGroups.push(colGroup);
                                //colGroupdictionary
                                vm.matrix.colDictionary[majors[0][i].Diversity] = colGroup;
                            }
                            vm.matrix.data = majors;
                            for (var i = 0; i < majors.length; i++) {
                                var rowGroup = { data: majors[i][0].Spread, title: (majors[i][0].Spread * 100).toFixed(2).toString(), isMinor: false, showMinors: false, };
                                vm.matrix.rowGroups.push(rowGroup);
                                //rowGroupdictionary
                                vm.matrix.rowDictionary[majors[i][0].Spread] = rowGroup;
                            }
                            for (var i = 0; i < majors[0].length; i++) {
                                if (vm.matrixPoint.Diversity && vm.matrixPoint.Diversity == majors[0][i].Diversity) {
                                    vm.selectedDiversity = i;
                                }
                            }
                        }
                        vm.isLoading = false;
                    });
                };
                this.showMenu = function (matrixData, rowIndex, colIndex) {
                    var vm = _this;
                    var menus = [];
                    var moodyRecovery = vm.selectedFundSummary.moodyRecovery;
                    if (matrixData.Warf != vm.matrixPoint.Warf) {
                        menus.push(['Set as Matrix Point', function () {
                                var modalInstance = vm.modalService.open({
                                    templateUrl: pageOptions.appBasePath + 'app/views/confirmmatrixpoint.html?v=' + pageOptions.appVersion,
                                    controller: 'application.controllers.confirmMatrixPointController',
                                    controllerAs: 'confirm',
                                    size: 'sm',
                                    resolve: {
                                        sourcedata: function () {
                                            return {
                                                fundCode: vm.selectedFund.fundCode,
                                                matrixPoint: matrixData.Warf
                                            };
                                        }
                                    }
                                });
                                modalInstance.result.then(function (confirm) {
                                    if (confirm) {
                                        vm.isLoading = true;
                                        vm.statusText = "Saving";
                                        if (rowIndex > 0) {
                                            matrixData.TopSpread = vm.matrix.rowGroups[rowIndex - 1].data;
                                        }
                                        if (rowIndex < (vm.matrix.rowGroups.length - 1)) {
                                            matrixData.BottomSpread = vm.matrix.rowGroups[rowIndex + 1].data;
                                        }
                                        if (colIndex > 0) {
                                            matrixData.LeftDiversity = vm.matrix.colGroups[colIndex - 1].data;
                                        }
                                        if (colIndex < (vm.matrix.colGroups.length - 1)) {
                                            matrixData.RightDiversity = vm.matrix.colGroups[colIndex + 1].data;
                                        }
                                        for (var i = rowIndex - 1; i >= 0; i--) {
                                            if (!vm.matrix.rowGroups[i].isMinor) {
                                                matrixData.TopMajorSpread = vm.matrix.rowGroups[i].data;
                                                break;
                                            }
                                        }
                                        for (var i = rowIndex + 1; i < vm.matrix.rowGroups.length; i++) {
                                            if (!vm.matrix.rowGroups[i].isMinor) {
                                                matrixData.BottomMajorSpread = vm.matrix.rowGroups[i].data;
                                                break;
                                            }
                                        }
                                        for (var i = colIndex - 1; i >= 0; i--) {
                                            if (!vm.matrix.colGroups[i].isMinor) {
                                                matrixData.LeftMajorDiversity = vm.matrix.colGroups[i].data;
                                                break;
                                            }
                                        }
                                        for (var i = colIndex + 1; i < vm.matrix.colGroups.length; i++) {
                                            if (!vm.matrix.colGroups[i].isMinor) {
                                                matrixData.RightMajorDiversity = vm.matrix.colGroups[i].data;
                                                break;
                                            }
                                        }
                                        var moodyRecovery = vm.selectedFundSummary.moodyRecovery;
                                        if (vm.selectedFundSummary.isNewCalc && moodyRecovery > vm.paramWarfRecovery) {
                                            matrixData.WarfModifier = matrixData.WarfModifier2 ? matrixData.WarfModifier2 : 0;
                                        }
                                        vm.dataService.addMatrixPoint(matrixData).then(function (points) {
                                            vm.matrixPoints = points;
                                            if (points && points.length) {
                                                vm.matrixPoint = points[0];
                                                vm.matrixPoint.showDetails = true;
                                            }
                                            for (var i = 0; i < vm.matrix.data[0].length; i++) {
                                                if (vm.matrixPoint.Diversity && vm.matrixPoint.Diversity == vm.matrix.data[0][i].Diversity) {
                                                    vm.selectedDiversity = i;
                                                }
                                            }
                                            vm.isLoading = false;
                                        });
                                    }
                                }, function () { });
                            }]);
                    }
                    return menus;
                };
                this.hideMinors = function () {
                    var needReload = false;
                    var vm = _this;
                    for (var i = 0; i < vm.matrix.colGroups.length; i++) {
                        needReload = needReload || (!vm.matrix.colGroups[i].isMinor && !vm.matrix.colGroups[i].showMinors);
                        if (needReload)
                            break;
                    }
                    if (!needReload) {
                        for (var i = 0; i < vm.matrix.rowGroups.length; i++) {
                            needReload = needReload || (!vm.matrix.rowGroups[i].isMinor && !vm.matrix.rowGroups[i].showMinors);
                            if (needReload)
                                break;
                        }
                    }
                    if (needReload) {
                        vm.loadMajors();
                    }
                };
                this.showHideDiversityMinors = function (index) {
                    var vm = _this;
                    vm.statusText = "Loading";
                    if (index < vm.matrix.colGroups.length - 1) {
                        var currentMajor = vm.matrix.colGroups[index];
                        if (!currentMajor.isMinor) {
                            var nextMajor = vm.matrix.colGroups[index + 1];
                            currentMajor.showMinors = !currentMajor.showMinors;
                            if (currentMajor.showMinors) {
                                vm.isLoading = true;
                                vm.dataService.getMinorsByDiversity(vm.selectedFund.fundId, currentMajor.data, nextMajor.data).then(function (minors) {
                                    for (var i = 0; i < minors[0].length; i++) {
                                        //adding new col groups
                                        var colGroup = {
                                            data: minors[0][i].Diversity,
                                            title: minors[0][i].Diversity.toString(),
                                            isMinor: true
                                        };
                                        vm.matrix.colGroups.splice(index + i + 1, 0, colGroup);
                                        //adding to col-dictionary
                                        vm.matrix.colDictionary[minors[0][i].Diversity] = colGroup;
                                    }
                                    //adding cols for each row
                                    for (var i = 0; i < minors.length; i++) {
                                        if (vm.matrix.rowDictionary[minors[i][0].Spread]) {
                                            for (var rowIndex = 0; rowIndex < vm.matrix.data.length; rowIndex++) {
                                                if (vm.matrix.data[rowIndex][0].Spread == minors[i][0].Spread) {
                                                    for (var k = 0; k < minors[i].length; k++) {
                                                        vm.matrix.data[rowIndex].splice(index + 1 + k, 0, minors[i][k]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    vm.isLoading = false;
                                });
                            }
                            else {
                                var lastMinorIndex = -1;
                                for (var i = index + 1; i < vm.matrix.colGroups.length; i++) {
                                    if (!vm.matrix.colGroups[i].isMinor) {
                                        lastMinorIndex = i - 1;
                                        break;
                                    }
                                }
                                if (lastMinorIndex != -1) {
                                    //removing from cols dictionary
                                    for (var i = index + 1; i <= lastMinorIndex; i++) {
                                        delete vm.matrix.colDictionary[vm.matrix.colGroups[i].data];
                                    }
                                    //removing from colsGroup
                                    vm.matrix.colGroups.splice(index + 1, lastMinorIndex - index);
                                    //removing cols from data
                                    for (var i = 0; i < vm.matrix.data.length; i++) {
                                        vm.matrix.data[i].splice(index + 1, lastMinorIndex - index);
                                    }
                                }
                            }
                        }
                    }
                };
                this.getRoundedUpNumber = function (number) {
                    return Math.ceil(number);
                };
                this.showHideSpreadMinors = function (index) {
                    var vm = _this;
                    vm.statusText = "Loading";
                    if (index < vm.matrix.rowGroups.length - 1) {
                        var currentMajor = vm.matrix.rowGroups[index];
                        if (!currentMajor.isMinor) {
                            var nextMajor = vm.matrix.rowGroups[index + 1];
                            currentMajor.showMinors = !currentMajor.showMinors;
                            if (currentMajor.showMinors) {
                                vm.isLoading = true;
                                vm.dataService.getMinorsBySpread(vm.selectedFund.fundId, currentMajor.data, nextMajor.data).then(function (minors) {
                                    for (var i = 0; i < minors.length; i++) {
                                        //adding new row groups
                                        var rowGroup = {
                                            data: minors[i][0].Spread,
                                            title: (minors[i][0].Spread * 100).toFixed(2).toString(),
                                            isMinor: true
                                        };
                                        vm.matrix.rowGroups.splice(index + i + 1, 0, rowGroup);
                                        //rowGroupdictionary
                                        vm.matrix.rowDictionary[minors[i][0].Spread] = rowGroup;
                                    }
                                    for (var i = 0; i < minors.length; i++) {
                                        //remove unwanted columns
                                        for (var j = 0; j < minors[i].length; j++) {
                                            if (!vm.matrix.colDictionary[minors[i][j].Diversity]) {
                                                minors[i].splice(j, 1);
                                                j--;
                                            }
                                        }
                                        vm.matrix.data.splice(index + i + 1, 0, minors[i]);
                                    }
                                    vm.matrix.data = JSON.parse(JSON.stringify(vm.matrix.data));
                                    vm.isLoading = false;
                                });
                            }
                            else {
                                var lastMinorIndex = -1;
                                for (var i = index + 1; i < vm.matrix.rowGroups.length; i++) {
                                    if (!vm.matrix.rowGroups[i].isMinor) {
                                        lastMinorIndex = i - 1;
                                        break;
                                    }
                                }
                                if (lastMinorIndex != -1) {
                                    //removing from rows dictionary
                                    for (var i = index + 1; i <= lastMinorIndex; i++) {
                                        delete vm.matrix.rowDictionary[vm.matrix.rowGroups[i].data];
                                    }
                                    //removing from rowGroup
                                    vm.matrix.rowGroups.splice(index + 1, lastMinorIndex - index);
                                    //removing cols from data
                                    vm.matrix.data.splice(index + 1, lastMinorIndex - index);
                                }
                                vm.matrix.data = JSON.parse(JSON.stringify(vm.matrix.data));
                            }
                        }
                    }
                };
                var vm = this;
                vm.dataService = dataService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'maintenance');
                vm.modalService = modalService;
                vm.filter = $filter;
                vm.loadFunds();
            }
            return CollateralQualityMatrixController;
        }());
        CollateralQualityMatrixController.$inject = ["application.services.dataService", "$rootScope", '$modal', '$filter', '$timeout'];
        Controllers.CollateralQualityMatrixController = CollateralQualityMatrixController;
        angular.module("app").controller("application.controllers.collateralQualityMatrixController", CollateralQualityMatrixController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=CollateralQualityMatrixController.js.map