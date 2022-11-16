module Application.Services.Contracts {
    export interface IHomeService {
        loadData: () => ng.IPromise<Array<Application.Models.UserModel>>;
        updateData: (userModel: Application.Models.UserModel) => ng.IPromise<boolean>;
    }
} 