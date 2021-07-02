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
                                    return row[$scope.title] === prop;
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
            var html = '<div class="modal" ng-style="{display: \'block\'}"><div class="modal-dialog modal-dialog-sm"><div class="modal-content"><div class="modal-header">Filter ' + $scope.title + ': </div><div class="modal-body"><div id="grid1" ui-grid="gridOptions" ui-grid-selection class="modalGrid"></div></div><div class="modal-footer"><button id="buttonClose" class="btn btn-warning" ng-click="clear()">Clear <i class="fa fa-times" aria-hidden="true"></i></button><button id="buttonClose" class="btn btn-primary" ng-click="close()">Filter <i class="fa fa-filter" aria-hidden="true"></i></button></div></div></div></div>';
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
                    return $scope.colFilter.listTerm.indexOf(activityType) > -1;
                };
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
    }]);
//# sourceMappingURL=CustomModalFilterCtrl.js.map