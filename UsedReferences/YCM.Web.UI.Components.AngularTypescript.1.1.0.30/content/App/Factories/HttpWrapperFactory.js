var Application;
(function (Application) {
    var Factories;
    (function (Factories) {
        var HttpWrapperFactory = (function () {
            function HttpWrapperFactory($http, $q) {
                var _this = this;
                this.getData = function (url) {
                    var deferred = _this.$q.defer();
                    _this.httpService.get(url, { timeout: 360000 }).success(function (d) {
                        deferred.resolve(d);
                    }).error(function () {
                        deferred.reject();
                    });
                    return deferred.promise;
                };
                this.postData = function (url, data) {
                    var deferred = _this.$q.defer();
                    _this.httpService.post(url, data).success(function (d) {
                        deferred.resolve(d);
                    }).error(function () {
                        deferred.reject();
                    });
                    return deferred.promise;
                };
                this.httpService = $http;
                this.$q = $q;
                return this;
            }
            return HttpWrapperFactory;
        }());
        HttpWrapperFactory.$inject = ["$http", "$q"];
        Factories.HttpWrapperFactory = HttpWrapperFactory;
        angular.module("app").factory("application.factories.httpWrapperFactory", HttpWrapperFactory);
    })(Factories = Application.Factories || (Application.Factories = {}));
})(Application || (Application = {}));
//# sourceMappingURL=HttpWrapperFactory.js.map