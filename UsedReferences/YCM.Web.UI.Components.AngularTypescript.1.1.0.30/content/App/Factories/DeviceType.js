var Application;
(function (Application) {
    var Factories;
    (function (Factories) {
        var DeviceType = (function () {
            function DeviceType() {
                var t = "Phone", n = "Desktop";
                return /BlackBerry/.test(navigator.userAgent) ? n = "BB10" : /IEMobile/.test(navigator.userAgent) ? n = "IEMobile" : /iPhone|iPod/.test(navigator.userAgent) ? n = "iPhone" : /iPad/.test(navigator.userAgent) ? (t = "Tablet", n = "iPad") : /Android/.test(navigator.userAgent) ? (/Mobile/.test(navigator.userAgent) || (t = "Tablet"), n = "Android") : t = n, { formFactor: t, type: n };
            }
            return DeviceType;
        }());
        Factories.DeviceType = DeviceType;
        angular.module("app").factory("application.Factories.DeviceType", DeviceType);
    })(Factories = Application.Factories || (Application.Factories = {}));
})(Application || (Application = {}));
//# sourceMappingURL=DeviceType.js.map