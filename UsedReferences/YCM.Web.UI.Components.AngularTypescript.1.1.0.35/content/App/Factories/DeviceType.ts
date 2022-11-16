module Application.Factories {
    export class DeviceType {
        constructor() {
            var t: string = "Phone", n: string = "Desktop";
            return /BlackBerry/.test(navigator.userAgent) ? n = "BB10" : /IEMobile/.test(navigator.userAgent) ? n = "IEMobile" : /iPhone|iPod/.test(navigator.userAgent) ? n = "iPhone" : /iPad/.test(navigator.userAgent) ? (t = "Tablet", n = "iPad") : /Android/.test(navigator.userAgent) ? (/Mobile/.test(navigator.userAgent) || (t = "Tablet"), n = "Android") : t = n, { formFactor: t, type: n } 
        }
    }

    angular.module("app").factory("application.Factories.DeviceType", DeviceType);
}

