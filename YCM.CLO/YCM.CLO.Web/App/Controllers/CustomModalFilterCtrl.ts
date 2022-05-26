
module Application.Controllers {

    export class customModalFilterCtrl {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;
        isLoading: boolean;
        appBasePath: string = pageOptions.appBasePath;
        statusText: string = "Loading";
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeout: ng.ITimeoutService;
        isRunningAnalysis: boolean;
        hideDefinitions: boolean;
        selectedFund: Models.ISummary;
        operators: Array<Models.IOperator>;
        filterCollections: any;
        windowService: ng.IWindowService;
        compile: ng.ICompileService;
        Rank: Array<number>;
        ratingId: number;


        moodyRatings: Array<Models.IRating>;

        static $inject = ["application.services.dataService", '$scope', '$compile', '$timeout'];
        //   static $inject = ["application.controllers.customModalFilterCtrl", "application.services.dataService", ['$scope', '$compile', '$timeout', function ($scope, $compile, $timeout)]]

        constructor(dataService: Application.Services.Contracts.IDataService, $scope: ng.IRootScopeService, $compile: ng.ICompileService, $timeout: ng.ITimeoutService, moodyRatings: Array<Models.IRating>) {

            var vm = this;
            vm.dataService = dataService;
            vm.timeout = $timeout;
            vm.compile = $compile;
            vm.rootScope = $scope;
            vm.moodyRatings = moodyRatings;


            var $elm;

            dataService.getMoodyRatings().then(ratings => {
                vm.moodyRatings = ratings;
            });

            vm.rootScope.show = function () {
                vm.rootScope.list = [];

                $scope.property = vm.rootScope.$parent.col.field;
                vm.rootScope.title = vm.rootScope.$parent.col.displayName;

                vm.rootScope.col.grid.options.data.forEach(function (row) {
                    if (vm.rootScope.list.indexOf(row[vm.rootScope.col.field]) == -1)
                        vm.rootScope.list.push(row[vm.rootScope.col.field]);
                    //vm.rootScope.$parent.col.filter.selectOptions.forEach(c => {
                    //    if (vm.rootScope.list.indexOf(c.label) === -1) {
                    //        vm.rootScope.list.push(c.label);
                    //    }
                    //})
                });

                vm.rootScope.gridOptions = {
                    data: [],
                    enableColumnMenus: false,
                    onRegisterApi: function (gridApi) {
                        vm.rootScope.gridApi = gridApi;

                        if (vm.rootScope.colFilter && vm.rootScope.colFilter.listTerm) {
                            $timeout(function () {
                                vm.rootScope.colFilter.listTerm.forEach(function (prop) {
                                    var entities = vm.rootScope.gridOptions.data.filter(function (row) {
                                        return row[vm.rootScope.title] === prop;
                                    });
                                    if (entities.length > 0) {
                                        vm.rootScope.gridApi.selection.selectRow(entities[0]);
                                    }
                                });
                            });
                        }
                    }
                };

                var filterData = vm.rootScope.list.sort(vm.rootScope.$parent.col.sortingAlgorithm);
                filterData.forEach(function (prop) {
                    var p = {};
                    p[vm.rootScope.title] = prop;
                    vm.rootScope.gridOptions.data.push(p);
                });

                // Finding FilterTypes of Numeric Columns--Start


                var filterColumnNumericResultCount = 0, filterColumnVarcharResultCount = 0, filterColumnBooleanResultCount = 0;

                var noOfResults = vm.rootScope.gridOptions.data.length;

                vm.rootScope.gridOptions.data.forEach(function (row) {

                    if (typeof row[vm.rootScope.col.displayName] == 'string') {
                        var isColumnValueNumeric = /^\d*\.?\d*$/.test(row[vm.rootScope.col.displayName].replace(/[^a-zA-Z0-9.-/]+/g, ''));
                        if (isColumnValueNumeric)
                            filterColumnNumericResultCount += 1;
                        else
                            filterColumnVarcharResultCount += 1;
                    }
                    else if (typeof row[vm.rootScope.col.displayName] == 'number') {
                        filterColumnNumericResultCount += 1;
                    }
                    else if (typeof row[vm.rootScope.col.displayName] == 'boolean') {
                        filterColumnBooleanResultCount += 1;
                    }


                })

                var innerHTML = '';
                // onKeyPress="edValueKeyPress()" 
                var varcharHTML = '<select id="customFilterType"   ng-change=onSelectFilterType()  ng-model="customFilterType" ng-options="filter.type for filter in cusFilterTypeArray"></select><br><br/><input id="cusFilterInput1" type = "number" ng-model="cusFilterInput1" ng-blur="check(this)"   style="display:none"  /><br/>'
                var numericHTML = '<select id="customFilterType" ng-change=onSelectFilterType() ng-model="customFilterType" ng-options="filter.type for filter in cusFilterTypeArray"></select><br><br/><input id="cusFilterInput1"  type = "number" ng-model="cusFilterInput1" ng-blur="check(this)"   style="display:none" /><input id="cusFilterInput2" type = "number" ng-model="cusFilterInput2" ng-blur="check(this)" style = "display:none" /><br/>'


                if (filterColumnNumericResultCount == noOfResults) {
                    vm.rootScope.cusFilterTypeArray = [{ "id": 1, "type": 'Equals' }, { "id": 2, "type": 'NotEquals' }, { "id": 3, "type": 'Range' }, { "id": 4, "type": 'GreaterAndEqual' }, { "id": 5, "type": 'LessAndEqual' }];
                    innerHTML = numericHTML;
                }
                else if (filterColumnVarcharResultCount > 0 && filterColumnVarcharResultCount <= noOfResults) {
                    vm.rootScope.cusFilterTypeArray = [{ "id": 1, "type": 'Contains' }, { "id": 2, "type": 'NotContains' }, { "id": 3, "type": 'GreaterAndEqual' }, { "id": 4, "type": 'LessAndEqual' }],
                        innerHTML = varcharHTML;
                }
                //Hide greater and less tha from dropdown
                //check columns for moody and type should be string only
                if (!((vm.rootScope.col.displayName.toString().toLowerCase().includes("moody facility") ||
                    vm.rootScope.col.displayName.toString().toLowerCase().includes("moody\'s cfr") ||
                    vm.rootScope.col.displayName.toString().toLowerCase().includes('moody\'s crf adj')))
                    && (filterColumnVarcharResultCount > 0 && filterColumnVarcharResultCount <= noOfResults)) {


                    vm.rootScope.cusFilterTypeArray.splice(2, 1)
                    vm.rootScope.cusFilterTypeArray.splice(2, 1)
                }
                var html = '<div class="modal" ng-style="{display: \'block\'}"><div class="modal-dialog modal-dialog-sm"><div class="modal-content"><div class="modal-header">Filter ' + vm.rootScope.title + ': </div><div class="modal-body">' + innerHTML + '<div id="grid1" ui-grid="gridOptions" ui-grid-selection  class="modalGrid"></div></div><div class="modal-footer"><button id="buttonClose" class="btn btn-warning" ng-click="clear()">Clear <i class="fa fa-times" aria-hidden="true"></i></button><button id="buttonClose" class="btn btn-primary" ng-click="close()">Filter <i class="fa fa-filter" aria-hidden="true"></i></button></div></div></div></div>';

                // Finding FilterTypes of Numeric Columns-End

                $elm = angular.element(html);
                angular.element(document.body).prepend($elm);

                $compile($elm)(vm.rootScope);
            };

            vm.rootScope.close = function () {
                var datas = vm.rootScope.gridApi.selection.getSelectedRows();
                vm.rootScope.colFilter.listTerm = [];

                datas.forEach(function (prop) {
                    vm.rootScope.colFilter.listTerm.push(prop[vm.rootScope.title]);
                });

                vm.rootScope.colFilter.term = vm.rootScope.colFilter.listTerm.join(', ');

                vm.rootScope.colFilter.condition =

                    function (searchTerm, activityType) {
                        return vm.rootScope.colFilter.listTerm.indexOf(activityType) > -1
                    }

                if ($elm) {
                    $elm.remove();
                }
            };

            vm.rootScope.clear = function () {
                vm.rootScope.colFilter.listTerm = [];
                vm.rootScope.colFilter.term = '';
                if ($elm) {
                    $elm.remove();
                }
            };

            // Event for Filter Functionality
            vm.rootScope.check = function (e) {

                var cusFilterInput1, cusFilterInput2 = null;
                var customFilterType = this.customFilterType.type;

                if (customFilterType != undefined && customFilterType != null) {
                    if (customFilterType == 'Equals' || customFilterType == 'NotEquals') {
                        cusFilterInput1 = ($('#cusFilterInput1').val() * 1);
                        vm.rootScope.gridApi.selection.clearSelectedRows();
                    }
                    else if (customFilterType == 'Range') {
                        if ($('#cusFilterInput1').val() != '' && $('#cusFilterInput1').val() != undefined && $('#cusFilterInput2').val() != '' && $('#cusFilterInput2').val() != undefined && ($('#cusFilterInput1').val() * 1) < ($('#cusFilterInput2').val() * 1)) {
                            cusFilterInput1 = ($('#cusFilterInput1').val() * 1);
                            cusFilterInput2 = ($('#cusFilterInput2').val() * 1);
                            vm.rootScope.gridApi.selection.clearSelectedRows();
                        }
                        else
                            return
                    }
                    else if (customFilterType == 'Contains' || customFilterType == 'NotContains') {
                        cusFilterInput1 = $('#cusFilterInput1').val();
                        vm.rootScope.gridApi.selection.clearSelectedRows();
                    }
                    else if (customFilterType == 'GreaterAndEqual' || customFilterType == 'LessAndEqual') {
                        cusFilterInput1 = $('#cusFilterInput1').val();
                        vm.rootScope.gridApi.selection.clearSelectedRows();
                    }

                }
                var finalrank;
                var filterColumnName = vm.rootScope.col.displayName;

                //for geting the column type for filters
                e.gridOptions.data.forEach(function (row) {
                    var filterColumnNumericResultCount = 0, filterColumnVarcharResultCount = 0, filterColumnBooleanResultCount = 0;

                    var noOfResults = vm.rootScope.gridOptions.data.length;

                    vm.rootScope.gridOptions.data.forEach(function (row) {

                        if (typeof row[vm.rootScope.col.displayName] == 'string') {
                            var isColumnValueNumeric = /^\d*\.?\d*$/.test(row[vm.rootScope.col.displayName].replace(/[^a-zA-Z0-9.-/]+/g, ''));
                            if (isColumnValueNumeric)
                                filterColumnNumericResultCount += 1;
                            else
                                filterColumnVarcharResultCount += 1;
                        }
                        else if (typeof row[vm.rootScope.col.displayName] == 'number') {
                            filterColumnNumericResultCount += 1;
                        }
                        else if (typeof row[vm.rootScope.col.displayName] == 'boolean') {
                            filterColumnBooleanResultCount += 1;
                        }


                    })


                    var typeOfColumnValue = typeof row[filterColumnName];
                    var columnValue = null;
                    if (typeOfColumnValue == 'string')
                        columnValue = (row[filterColumnName].replace(/[^0-9.-]+/g, '') * 1)
                    else if (typeOfColumnValue == 'number')
                        columnValue = (row[filterColumnName] * 1)

                    if (customFilterType === "Equals") {
                        if (columnValue === cusFilterInput1) {
                            vm.rootScope.gridApi.selection.toggleRowSelection(row);
                        }
                    }
                    else if (customFilterType === "NotEquals") {
                        if (columnValue != cusFilterInput1) {
                            vm.rootScope.gridApi.selection.toggleRowSelection(row);
                        }
                    }
                    else if (customFilterType === "Range") {
                        if (columnValue >= cusFilterInput1 && columnValue <= cusFilterInput2) {
                            vm.rootScope.gridApi.selection.toggleRowSelection(row);
                        }
                    }
                    else if (customFilterType === "Contains") {


                        if (row[filterColumnName].toString().toLowerCase().indexOf(cusFilterInput1.toLowerCase()) > -1) {
                            vm.rootScope.gridApi.selection.toggleRowSelection(row);
                        }
                    }
                    else if (customFilterType === "NotContains") {

                        if (row[filterColumnName].toString().toLowerCase().indexOf(cusFilterInput1.toLowerCase()) === -1) {
                            vm.rootScope.gridApi.selection.toggleRowSelection(row);
                        }
                    }

                    else if (customFilterType === "GreaterAndEqual") {
                        //check column is numeric or string type for filters
                        //Numbers
                        if (filterColumnNumericResultCount == noOfResults) {
                            if (cusFilterInput1 != 0) {
                                if (columnValue >= cusFilterInput1) {
                                    vm.rootScope.gridApi.selection.toggleRowSelection(row);
                                }
                            }

                        }
                        //strings
                        else if (filterColumnVarcharResultCount > 0 && filterColumnVarcharResultCount <= noOfResults) {
                            finalrank = [];
                            var cusFilter1 = $('#cusFilterInput1').val();
                            finalrank = vm.moodyRatings.filter(s => s.ratingDesc.toLowerCase() == cusFilter1.toLowerCase()).map(t => t.rank);
                            //AS per the Rank get the list of data from service
                            if (finalrank.length != 0) {
                                var RatingDesc = vm.moodyRatings.filter(x => x.rank >= finalrank).map(t => t.ratingDesc).sort((a, b) => (b > a) ? -1 : 1);
                                for (var i = 0; i < RatingDesc.length; i++) {
                                    if (row[filterColumnName].toString().toLowerCase() === RatingDesc[i].toLowerCase()) {
                                        vm.rootScope.gridApi.selection.toggleRowSelection(row);
                                    }

                                }
                            }
                        }

                    }
                    else if (customFilterType === "LessAndEqual") {

                        //check column is numeric or string type for filters
                        //Numbers
                        if (filterColumnNumericResultCount == noOfResults) {
                            if (cusFilterInput1 != 0) {
                                if (columnValue <= cusFilterInput1) {
                                    vm.rootScope.gridApi.selection.toggleRowSelection(row);
                                }
                            }

                        }
                        else {

                            finalrank = [];
                            var cusFilter2 = $('#cusFilterInput1').val();
                            finalrank = vm.moodyRatings.filter(s => s.ratingDesc.toLowerCase() == cusFilter2.toLowerCase()).map(t => t.rank);
                            if (finalrank.length != 0) {
                                var RatingDescless = vm.moodyRatings.filter(x => x.rank <= finalrank).map(t => t.ratingDesc).sort((a, b) => (b > a) ? -1 : 1);
                                // console.log("Test2  Value : " + RatingDesc);
                                for (var j = 0; j < RatingDescless.length; j++) {
                                    if (row[filterColumnName].toString().toLowerCase() === RatingDescless[j].toLowerCase()) {
                                        vm.rootScope.gridApi.selection.toggleRowSelection(row);
                                    }
                                }

                            }
                        }



                    }
                },

                )




            };

            // Event for selecting Filter Types from Dropdown
            vm.rootScope.onSelectFilterType = function () {
                //start .. checked for the columns type Numeric or string 
                var filterColumnNumericResultCount = 0, filterColumnVarcharResultCount = 0, filterColumnBooleanResultCount = 0;

                var noOfResults = vm.rootScope.gridOptions.data.length;

                vm.rootScope.gridOptions.data.forEach(function (row) {

                    if (typeof row[vm.rootScope.col.displayName] == 'string') {
                        var isColumnValueNumeric = /^\d*\.?\d*$/.test(row[vm.rootScope.col.displayName].replace(/[^a-zA-Z0-9.-/]+/g, ''));
                        if (isColumnValueNumeric)
                            filterColumnNumericResultCount += 1;
                        else
                            filterColumnVarcharResultCount += 1;
                    }
                    else if (typeof row[vm.rootScope.col.displayName] == 'number') {
                        filterColumnNumericResultCount += 1;
                    }
                    else if (typeof row[vm.rootScope.col.displayName] == 'boolean') {
                        filterColumnBooleanResultCount += 1;
                    }


                })
                //End..

                if (this.customFilterType.type == 'Equals') {
                    this.cusFilterInput1 = '';
                    this.cusFilterInput2 = '';
                    $('#cusFilterInput1').attr('placeholder', 'Equals');
                    $('#cusFilterInput1').show();
                    $('#cusFilterInput2').hide();




                }
                else if (this.customFilterType.type == 'NotEquals') {
                    this.cusFilterInput1 = '';
                    this.cusFilterInput2 = '';
                    $('#cusFilterInput1').attr('placeholder', 'NotEquals');
                    $('#cusFilterInput1').show();
                    $('#cusFilterInput2').hide();
                }
                else if (this.customFilterType.type == 'Contains') {
                    this.cusFilterInput1 = '';
                    $('#cusFilterInput1').attr('placeholder', 'Contains');
                    $('#cusFilterInput1').attr('type', 'text');
                    $('#cusFilterInput1').show();

                }
                else if (this.customFilterType.type == 'NotContains') {
                    this.cusFilterInput1 = '';
                    $('#cusFilterInput1').attr('placeholder', 'NotContains');
                    $('#cusFilterInput1').attr('type', 'text');
                    $('#cusFilterInput1').show();

                }
                else if (this.customFilterType.type == 'Range') {
                    this.cusFilterInput1 = '';
                    this.cusFilterInput2 = '';
                    $('#cusFilterInput1').attr('placeholder', 'From');
                    $('#cusFilterInput2').attr('placeholder', 'To');
                    $('#cusFilterInput1').show();
                    $('#cusFilterInput2').show();
                }
                else if (this.customFilterType.type == 'GreaterAndEqual') {
                    //checked for the Number Columns for Numeric textbox
                    if (filterColumnNumericResultCount == noOfResults) {

                        this.cusFilterInput1 = '';
                        this.cusFilterInput2 = '';
                        $('#cusFilterInput1').attr('placeholder', 'GreaterAndEqual');
                        $('#cusFilterInput1').show();
                        $('#cusFilterInput2').hide();

                    }
                    else {

                        this.cusFilterInput1 = '';
                        this.cusFilterInput2 = '';
                        $('#cusFilterInput1').attr('placeholder', 'GreaterAndEqual');
                        $('#cusFilterInput1').attr('type', 'text');
                        $('#cusFilterInput1').show();
                        $('#cusFilterInput2').hide();
                    }
                }
                else if (this.customFilterType.type == 'LessAndEqual') {

                    if (filterColumnNumericResultCount == noOfResults) {

                        this.cusFilterInput1 = '';
                        this.cusFilterInput2 = '';
                        $('#cusFilterInput1').attr('placeholder', 'LessAndEqual');
                        $('#cusFilterInput1').show();
                        $('#cusFilterInput2').hide();

                    }
                    else {
                        this.cusFilterInput1 = '';
                        this.cusFilterInput2 = '';
                        $('#cusFilterInput1').attr('placeholder', 'LessAndEqual');
                        $('#cusFilterInput1').attr('type', 'text');
                        $('#cusFilterInput1').show();
                        $('#cusFilterInput2').hide();

                    }

                }

                $("[class*='ui-grid-icon-ok']").addClass('disablegrid');
                $("[class*='modalGrid']").addClass('FilterGridcolor');
                
               
            }



        }

    }

    angular.module("app").controller("application.controllers.customModalFilterCtrl", customModalFilterCtrl);

   

}
