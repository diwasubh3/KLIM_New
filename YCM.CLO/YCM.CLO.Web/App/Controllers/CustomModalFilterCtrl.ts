angular.module("app").controller("application.controllers.customModalFilterCtrl", ['$scope', '$compile', '$timeout', function ($scope, $compile, $timeout) {
    var $elm;

    $scope.show = function () {
        $scope.list = [];
        
        $scope.property = $scope.$parent.col.field;
        $scope.title = $scope.$parent.col.displayName;
        
		$scope.col.grid.options.data.forEach(function (row) {
			if ($scope.list.indexOf(row[$scope.col.field]) == -1)
				$scope.list.push(row[$scope.col.field]);
			//$scope.$parent.col.filter.selectOptions.forEach(c => {
			//    if ($scope.list.indexOf(c.label) === -1) {
			//        $scope.list.push(c.label);
			//    }
		//})
        });

        $scope.gridOptions = {
            data: [],
            enableColumnMenus: false,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

                if ($scope.colFilter && $scope.colFilter.listTerm) {
                    $timeout(function () {
                        $scope.colFilter.listTerm.forEach(function (prop) {
                            var entities = $scope.gridOptions.data.filter(function (row) {
                                return row[$scope.title]=== prop;
                            });
                            if (entities.length > 0) {
                                $scope.gridApi.selection.selectRow(entities[0]);
                            }
                        });
                    });
                }
            }
        };

	    var filterData = $scope.list.sort($scope.$parent.col.sortingAlgorithm);
	    filterData.forEach(function (prop) {
            var p = {};
            p[$scope.title] = prop;
            $scope.gridOptions.data.push(p);
        });

      // Finding FilterTypes of Numeric Columns--Start

        
        var filterColumnNumericResultCount =0, filterColumnVarcharResultCount =0, filterColumnBooleanResultCount = 0;
        
        var noOfResults = $scope.gridOptions.data.length;

        $scope.gridOptions.data.forEach(function (row)
        {
            
            if (typeof row[$scope.col.displayName] == 'string') {
                var isColumnValueNumeric = /^\d*\.?\d*$/.test(row[$scope.col.displayName].replace(/[^a-zA-Z0-9.-/]+/g, ''));
                if (isColumnValueNumeric)
                    filterColumnNumericResultCount += 1;
                else
                    filterColumnVarcharResultCount += 1;
            }
            else if (typeof row[$scope.col.displayName] == 'number') {
                filterColumnNumericResultCount += 1;
            }
            else if (typeof row[$scope.col.displayName] == 'boolean') {
                filterColumnBooleanResultCount += 1;
            }
                
            
        })

        var innerHTML = '';

        var varcharHTML = '<select id="customFilterType"  ng-change=onSelectFilterType() ng-model="customFilterType" ng-options="filter.type for filter in cusFilterTypeArray"></select><br><br/><input id="cusFilterInput1" type = "number" ng-model="cusFilterInput1" ng-blur="check(this)" style="display:none" /><br/>'
        var numericHTML = '<select id="customFilterType"  ng-change=onSelectFilterType() ng-model="customFilterType" ng-options="filter.type for filter in cusFilterTypeArray"></select><br><br/><input id="cusFilterInput1" type = "number" ng-model="cusFilterInput1" ng-blur="check(this)" style="display:none" /><input id="cusFilterInput2" type = "number" ng-model="cusFilterInput2" ng-blur="check(this)" style = "display:none" /><br/>'


        if (filterColumnNumericResultCount == noOfResults) {
            $scope.cusFilterTypeArray = [{ "id": 1, "type": 'Equals' }, { "id": 2, "type": 'NotEquals' }, { "id": 3, "type": 'Range' }];
            innerHTML = numericHTML;
        }
        else if (filterColumnVarcharResultCount > 0 && filterColumnVarcharResultCount <= noOfResults) {
            $scope.cusFilterTypeArray = [{ "id": 1, "type": 'Contains' }, { "id": 2, "type": 'NotContains' }];
            innerHTML = varcharHTML;
        }


        var html = '<div class="modal" ng-style="{display: \'block\'}"><div class="modal-dialog modal-dialog-sm"><div class="modal-content"><div class="modal-header">Filter ' + $scope.title + ': </div><div class="modal-body">' + innerHTML + '<div id="grid1" ui-grid="gridOptions" ui-grid-selection class="modalGrid"></div></div><div class="modal-footer"><button id="buttonClose" class="btn btn-warning" ng-click="clear()">Clear <i class="fa fa-times" aria-hidden="true"></i></button><button id="buttonClose" class="btn btn-primary" ng-click="close()">Filter <i class="fa fa-filter" aria-hidden="true"></i></button></div></div></div></div>';

        // Finding FilterTypes of Numeric Columns-End

        $elm = angular.element(html);
        angular.element(document.body).prepend($elm);

        $compile($elm)($scope);
    };

    $scope.close = function () {
        var datas = $scope.gridApi.selection.getSelectedRows();
        $scope.colFilter.listTerm = [];
        
        datas.forEach(function (prop) {
            $scope.colFilter.listTerm.push(prop[$scope.title]);
        });

        $scope.colFilter.term = $scope.colFilter.listTerm.join(', ');
        
        $scope.colFilter.condition =
            function (searchTerm, activityType) {
                return $scope.colFilter.listTerm.indexOf(activityType) > -1
            }
        
        if ($elm) {
            $elm.remove();
        }
    };

    $scope.clear = function () {
        $scope.colFilter.listTerm = [];
        $scope.colFilter.term = '';
        if ($elm) {
            $elm.remove();
        }
    };

    // Event for Filter Functionality
    $scope.check = function (e) {
        
        var cusFilterInput1, cusFilterInput2 = null;
        var customFilterType = this.customFilterType.type;
        if (customFilterType != undefined && customFilterType != null) {
            if (customFilterType == 'Equals' || customFilterType == 'NotEquals') {
                cusFilterInput1 = ($('#cusFilterInput1').val() * 1);
                $scope.gridApi.selection.clearSelectedRows();
            }
            else if (customFilterType == 'Range') {
                if ($('#cusFilterInput1').val() != '' && $('#cusFilterInput1').val() != undefined && $('#cusFilterInput2').val() != '' && $('#cusFilterInput2').val() != undefined && ($('#cusFilterInput1').val() * 1) < ($('#cusFilterInput2').val() * 1)) {
                    cusFilterInput1 = ($('#cusFilterInput1').val() * 1);
                    cusFilterInput2 = ($('#cusFilterInput2').val() * 1);
                    $scope.gridApi.selection.clearSelectedRows();
                }
                else
                    return
            }
            else if (customFilterType == 'Contains' || customFilterType == 'NotContains') {
                cusFilterInput1 = $('#cusFilterInput1').val();
                $scope.gridApi.selection.clearSelectedRows();
            }
        }
        
        var filterColumnName = $scope.col.displayName;
        e.gridOptions.data.forEach(function (row) {
            var typeOfColumnValue = typeof row[filterColumnName];
            var columnValue = null;
            if (typeOfColumnValue=='string')
                columnValue = (row[filterColumnName].replace(/[^0-9.-]+/g, '') * 1)
            else if (typeOfColumnValue == 'number')
                columnValue = (row[filterColumnName] * 1)

            if (customFilterType === "Equals") {
                if (columnValue === cusFilterInput1) {
                    $scope.gridApi.selection.toggleRowSelection(row);
                }
            }
            else if (customFilterType === "NotEquals") {
                if (columnValue != cusFilterInput1) {
                    $scope.gridApi.selection.toggleRowSelection(row);
                }
            }
            else if (customFilterType === "Range") {
                if (columnValue >= cusFilterInput1 && columnValue <= cusFilterInput2) {
                    $scope.gridApi.selection.toggleRowSelection(row);
                }
            }
            else if (customFilterType === "Contains") {
                if (row[filterColumnName].toString().toLowerCase().indexOf(cusFilterInput1.toLowerCase())> -1) {
                    $scope.gridApi.selection.toggleRowSelection(row);
                }
            }
            else if (customFilterType === "NotContains") {
                if (row[filterColumnName].toString().toLowerCase().indexOf(cusFilterInput1.toLowerCase()) === -1) {
                    $scope.gridApi.selection.toggleRowSelection(row);
                }
            }
        },
        
        )
        
        
    };

    // Event for selecting Filter Types from Dropdown
    $scope.onSelectFilterType = function () {
        
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
    }


    
}]);