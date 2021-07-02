module Application.Controllers {
    export class CollateralQualityMatrixController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = "Loading";
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        headerFields: Array<Models.IField>;
        alerts: Array<string>;
        infos: Array<string>;
        selectedFund: Models.IFund;
        funds: Array<Models.IFund>;
        matrixPoint: Models.IMatrixPoint = <Models.IMatrixPoint>{};
        matrixPoints: Array<Models.IMatrixPoint>;
        selectedDiversity:number;
        matrix: Models.IMatrix = <Models.IMatrix>{ colGroups: [], rowGroups: [] }; 
        summaries: Models.ISummary[];
        selectedFundSummary: Models.ISummary;
        parameterValues: Models.IParameterValue[];

        paramWarfRecovery: number;
        paramSpread: number;
        paramWarf: number;
        paramDiversity: number;

        static $inject = ["application.services.dataService", "$rootScope", '$modal', '$filter', '$timeout'];
        constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'maintenance');
            vm.modalService = modalService;
            vm.filter = $filter;
            vm.loadFunds();
        }

        loadFunds = () => {
            var vm = this;
            vm.isLoading = true;

            vm.dataService.getFunds().then(funds => {
                vm.funds = funds;
                vm.isLoading = false;
            });

            vm.dataService.loadParameterValuesForParameterType('Matrix Point - Fund Restriction').then(params => {
                vm.parameterValues = params;
                vm.paramWarfRecovery = params.filter(p => p.parameterValueText == 'WARF RECOVERY')[0].parameterMaxValueNumber;
                vm.paramSpread = params.filter(p => p.parameterValueText == 'SPREAD')[0].parameterMaxValueNumber;
                vm.paramWarf = params.filter(p => p.parameterValueText == 'WARF')[0].parameterMaxValueNumber;
                vm.paramDiversity = params.filter(p => p.parameterValueText == 'DIVERSITY')[0].parameterMaxValueNumber;
            });

            vm.dataService.loadSummaryData().then(summs => {
                vm.summaries = summs;
            });

        }

        loadData = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Loading";
            if (vm.selectedFund)
            {
                vm.selectedFundSummary = vm.summaries.filter(f => f.fundId == vm.selectedFund.fundId)[0];
                vm.matrixPoint = <Models.IMatrixPoint>{};
                vm.dataService.getMatrixPoints(vm.selectedFund.fundId).then(points => {
                    vm.matrixPoints = points;

                    if (points && points.length) {
                        vm.matrixPoint = points[0];
                        vm.matrixPoint.showDetails = true;
                    }

                    vm.loadMajors();
                });
            }
        }

        loadMajors = () => {
            var vm = this;
            vm.dataService.getMajors(vm.selectedFund.fundId).then(majors => {
                vm.matrix = <Models.IMatrix>{ colGroups: [], rowGroups: [], colDictionary: {}, rowDictionary: {} };
                vm.selectedDiversity = 0;
                if (majors.length > 0) {
                    for (var i = 0; i < majors[0].length; i++) {
                        var colGroup = <Models.IMatrixGroup>{ data: majors[0][i].Diversity, title: majors[0][i].Diversity.toString(), isMinor: false, showMinors: false, };
                        vm.matrix.colGroups.push(colGroup);
                        //colGroupdictionary
                        vm.matrix.colDictionary[majors[0][i].Diversity] = colGroup;
                    }

                    vm.matrix.data = majors;

                    for (var i = 0; i < majors.length; i++) {
                        var rowGroup = <Models.IMatrixGroup>{ data: majors[i][0].Spread, title: (majors[i][0].Spread * 100).toFixed(2).toString(), isMinor: false, showMinors: false, };
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
        }

        showMenu = (matrixData: Models.IMatrixData,rowIndex:number,colIndex:number) => {
            var vm = this;
            
            var menus = [];

            if (matrixData.Warf != vm.matrixPoint.Warf) {
                menus.push(['Set as Matrix Point', () => {
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/confirmmatrixpoint.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.confirmMatrixPointController',
                        controllerAs: 'confirm',
                        size: 'sm',
                        resolve: {
                            sourcedata: () => {
                                return {
                                    fundCode: vm.selectedFund.fundCode,
                                    matrixPoint: matrixData.Warf
                                };
                            }
                        }
                    });

                    modalInstance.result.then((confirm: boolean) => {
                        if (confirm) {
                            vm.isLoading = true;
                            vm.statusText = "Saving";

                            if (rowIndex > 0)
                            {
                                matrixData.TopSpread = vm.matrix.rowGroups[rowIndex-1].data;
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
                                if (!vm.matrix.rowGroups[i].isMinor)
                                {
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

                            vm.dataService.addMatrixPoint(matrixData).then(points => {
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
                    }, () => { });


                    
                }]);
            }
            return menus;
        }

        hideMinors = () => {
            var needReload = false;
            var vm = this;
            for (var i = 0; i < vm.matrix.colGroups.length; i++) {
                needReload = needReload || (!vm.matrix.colGroups[i].isMinor && !vm.matrix.colGroups[i].showMinors);
                if (needReload) break;
            }

            if (!needReload)
            {
                for (var i = 0; i < vm.matrix.rowGroups.length; i++) {
                    needReload = needReload || (!vm.matrix.rowGroups[i].isMinor && !vm.matrix.rowGroups[i].showMinors);
                    if (needReload) break;
                }
            }

            if (needReload)
            {
                vm.loadMajors();
            }
        }

        showHideDiversityMinors = (index:number) => {
            var vm = this;
            vm.statusText = "Loading";
            
            if (index < vm.matrix.colGroups.length - 1) {
                var currentMajor = vm.matrix.colGroups[index];

                if (!currentMajor.isMinor) {

                    var nextMajor = vm.matrix.colGroups[index + 1];
                    currentMajor.showMinors = !currentMajor.showMinors;

                    if (currentMajor.showMinors) {
                        vm.isLoading = true;
                        vm.dataService.getMinorsByDiversity(vm.selectedFund.fundId, currentMajor.data, nextMajor.data).then(minors => {
                            for (var i = 0; i < minors[0].length; i++) {

                                //adding new col groups
                                var colGroup: Models.IMatrixGroup = <Models.IMatrixGroup>
                                    {
                                        data: minors[0][i].Diversity,
                                        title: minors[0][i].Diversity.toString(),
                                        isMinor: true
                                    }
                                vm.matrix.colGroups.splice(index + i + 1, 0, colGroup);

                                //adding to col-dictionary
                                vm.matrix.colDictionary[minors[0][i].Diversity] = colGroup;
                            }

                            //adding cols for each row
                            for (var i = 0; i < minors.length; i++) {
                                if (vm.matrix.rowDictionary[minors[i][0].Spread])
                                {
                                    for (var rowIndex = 0; rowIndex < vm.matrix.data.length; rowIndex++) {
                                        if (vm.matrix.data[rowIndex][0].Spread == minors[i][0].Spread)
                                        {
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
                            if (!vm.matrix.colGroups[i].isMinor)
                            {
                                lastMinorIndex = i-1;
                                break;
                            }
                        }
                        if (lastMinorIndex != -1)
                        {
                            //removing from cols dictionary
                            for (var i = index+1; i <= lastMinorIndex ; i++) {
                                delete vm.matrix.colDictionary[vm.matrix.colGroups[i].data]; 
                            }

                            //removing from colsGroup
                            vm.matrix.colGroups.splice(index + 1, lastMinorIndex - index)


                            //removing cols from data
                            for (var i = 0; i < vm.matrix.data.length; i++) {
                                vm.matrix.data[i].splice(index + 1, lastMinorIndex - index);
                            }
                        }
                    }
                }
            }
        }

        getRoundedUpNumber = (number) => {
            return Math.ceil(number);
        } 

        showHideSpreadMinors = (index) => {
            var vm = this;
            vm.statusText = "Loading";
            
            if (index < vm.matrix.rowGroups.length - 1) {
                var currentMajor = vm.matrix.rowGroups[index];
                if (!currentMajor.isMinor)
                {
                    var nextMajor = vm.matrix.rowGroups[index + 1];
                    currentMajor.showMinors = !currentMajor.showMinors;
                    if (currentMajor.showMinors)
                    {
                        vm.isLoading = true;
                        vm.dataService.getMinorsBySpread(vm.selectedFund.fundId, currentMajor.data, nextMajor.data).then(minors => {
                            
                            for (var i = 0; i < minors.length; i++) {

                                //adding new row groups
                                var rowGroup = <Models.IMatrixGroup>
                                    {
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
                                    if (!vm.matrix.colDictionary[minors[i][j].Diversity])
                                    {
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
                            vm.matrix.rowGroups.splice(index + 1, lastMinorIndex - index)

                            //removing cols from data
                            vm.matrix.data.splice(index + 1, lastMinorIndex - index);
                            
                        }

                        vm.matrix.data = JSON.parse(JSON.stringify(vm.matrix.data));
                    }
                }
            }
        }

        
        


    }

    angular.module("app").controller("application.controllers.collateralQualityMatrixController", CollateralQualityMatrixController);
} 