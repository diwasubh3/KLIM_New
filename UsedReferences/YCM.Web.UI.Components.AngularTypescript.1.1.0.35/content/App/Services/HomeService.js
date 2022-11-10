var Application;
(function (Application) {
    var Services;
    (function (Services) {
        var HomeService = (function () {
            function HomeService(httpWrapperFactory) {
                var _this = this;
                this.loadData = function () {
                    return _this.httpWrapperFactory.getData('/api/user');
                };
                this.updateData = function (userModel) {
                    return _this.httpWrapperFactory.postData('/api/user', userModel);
                };
                this.httpWrapperFactory = httpWrapperFactory;
            }
            return HomeService;
        }());
        HomeService.$inject = ["application.factories.httpWrapperFactory"];
        Services.HomeService = HomeService;
        angular.module("app").service("application.services.homeService", HomeService);
    })(Services = Application.Services || (Application.Services = {}));
})(Application || (Application = {}));
//# sourceMappingURL=HomeService.js.map