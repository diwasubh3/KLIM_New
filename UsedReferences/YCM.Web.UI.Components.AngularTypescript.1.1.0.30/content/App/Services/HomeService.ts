module Application.Services {
    export class HomeService implements Contracts.IHomeService {
        private httpWrapperFactory: Application.Factories.HttpWrapperFactory;
        static $inject = ["application.factories.httpWrapperFactory"];
        constructor(httpWrapperFactory: Application.Factories.HttpWrapperFactory) {
            this.httpWrapperFactory = httpWrapperFactory;
        }

        loadData = () => {
            return this.httpWrapperFactory.getData('/api/user');
        }

        updateData = (userModel: Application.Models.UserModel) => {
            return this.httpWrapperFactory.postData('/api/user', userModel);
        }
    }

    angular.module("app").service("application.services.homeService", HomeService); 
}
