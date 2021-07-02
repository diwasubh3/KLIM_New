module Application.Factories {
    export class HttpWrapperFactory {
        private httpService: ng.IHttpService;
        private $q: ng.IQService;

        static $inject = ["$http", "$q"];
        constructor($http: ng.IHttpService, $q: ng.IQService) {
            this.httpService = $http;
            this.$q = $q;
            return this;
        }

        getData = (url) => {
            var deferred = this.$q.defer();
            
			this.httpService.get(url, { timeout: 360000, headers: { 'Cache-Control': 'no-cache' } }).then(d => {
                deferred.resolve(d.data);
            }).catch(() => {
                deferred.reject();
            });
            return deferred.promise;
        }

        postData = (url, data) => {
            var deferred = this.$q.defer();
            this.httpService.post(url, data).then(d => {
                deferred.resolve(d.data);
            }).catch(() => {
                deferred.reject();
            });
            return deferred.promise;
        }

        deleteData = (url) => {
            var deferred = this.$q.defer();
            this.httpService.delete(url).then(d => {
                deferred.resolve(d.data);
            }).catch(() => {
                deferred.reject();
            });
            return deferred.promise;
        }
    }

    angular.module("app").factory("application.factories.httpWrapperFactory", HttpWrapperFactory);
} 