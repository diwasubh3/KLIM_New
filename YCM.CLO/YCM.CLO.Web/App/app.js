(function () {
    var mainApp = angular.module("app", ['ngAnimate', 'ngRoute', 'ngUpload', 'ui.bootstrap', 'ngTable', 'ui.bootstrap.contextMenu', 'ui.grid', 'ui.grid.pinning', 'ngSanitize', 'ui.grid.exporter', 'ui.grid.treeView', 'ui.grid.autoResize', 'ui.grid.selection']);
    mainApp.config(['$locationProvider', '$routeProvider', Application.Routes.configureRoutes]);
    mainApp.run(['$rootScope', '$location', "application.services.dataService", function ($rootScope, $location, dataSvc) {
            $rootScope.$on('$routeChangeStart', function (event) {
                var isSuperUser = dataSvc.userIsASuperUser();
                if (!isSuperUser) {
                    $location.path('/positions');
                }
            });
        }]);
    //mainApp.directive('optionsClass', function ($parse) {
    //	return {
    //		require: 'select',
    //		link: function (scope, elem, attrs, ngSelect) {
    //			// get the source for the items array that populates the select.
    //			var optionsSourceStr = attrs.ngOptions.split(' ').pop(),
    //				// use $parse to get a function from the options-class attribute
    //				// that you can use to evaluate later.
    //				getOptionsClass = $parse(attrs.optionsClass);
    //			scope.$watch(optionsSourceStr, function (items) {
    //				// when the options source changes loop through its items.
    //				angular.forEach(items, function (item, index) {
    //					// evaluate against the item to get a mapping object for
    //					// for your classes.
    //					var classes = getOptionsClass(item),
    //						// also get the option you're going to need. This can be found
    //						// by looking for the option with the appropriate index in the
    //						// value attribute.
    //						option = elem.find('option[value=' + index + ']');
    //					// now loop through the key/value pairs in the mapping object
    //					// and apply the classes that evaluated to be truthy.
    //					angular.forEach(classes, function (add, className) {
    //						if (add) {
    //							angular.element(option).addClass(className);
    //						}
    //					});
    //				});
    //			});
    //		}
    //	};
    //});
})();
//# sourceMappingURL=app.js.map