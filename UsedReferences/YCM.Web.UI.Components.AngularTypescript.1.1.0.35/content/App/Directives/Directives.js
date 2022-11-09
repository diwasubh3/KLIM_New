var Application;
(function (Application) {
    var Directives;
    (function (Directives) {
        //Example Template
        //<form id="frmFileUpload" style= "margin-bottom: 0px" ng- upload="entryupload.complete(content)" ng- upload - loading="entryupload.startUploading()" method= "post" enctype= "multipart/form-data" >
        //<input type="file" file- processor title= 'Upload Entries' id= "uploadedfile" name= "uploadedfile"
        //accept = ".xlsx" multiple= "" />
        //</form>    
        var FileProcessor = (function () {
            function FileProcessor() {
                this.restrict = 'A';
                this.link = function (scope, element, attrs, ctrl) {
                    $(element).bootstrapFileInput();
                    $(element).parent().parent()[0].action = pageOptions.webBasePath + "/" + pageOptions.appName + '/Data/UploadBalanceAmounts';
                    $($(element).parent().parent()[0]).on('change', function (e) {
                        window.setTimeout(function () {
                            $(e.target.parentNode.parentNode).submit();
                        });
                    });
                };
            }
            FileProcessor.factory = function () {
                var directive = function () { return new FileProcessor(); };
                directive.$inject = [];
                return directive;
            };
            return FileProcessor;
        }());
        Directives.FileProcessor = FileProcessor;
        var OnlyNumricKeys = (function () {
            function OnlyNumricKeys() {
                this.link = function (scope, element, attrs, ctrl) {
                    var keyCode = [8, 9, 37, 39, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 110, 188, 190, 189];
                    element.bind("keydown", function (event) {
                        if ($.inArray(event.which, keyCode) == -1) {
                            scope.$apply(function () {
                                scope.$eval(attrs['onlynumrickeys']);
                                event.preventDefault();
                            });
                            event.preventDefault();
                        }
                    });
                };
            }
            OnlyNumricKeys.factory = function () {
                var directive = function () { return new OnlyNumricKeys(); };
                directive.$inject = [];
                return directive;
            };
            return OnlyNumricKeys;
        }());
        Directives.OnlyNumricKeys = OnlyNumricKeys;
        var ValidateWeekendDate = (function () {
            function ValidateWeekendDate() {
                this.require = "ngModel";
                this.link = function (scope, element, attrs, ctrl) {
                    ctrl.$validators.validateweekenddate = function (modelValue, viewValue) {
                        var isvalid = true;
                        if (ctrl.$isEmpty(modelValue)) {
                            // consider empty models to be valid
                            isvalid = false;
                        }
                        else {
                            var momentDay = moment(scope['bal'].Date);
                            isvalid = (momentDay.day() <= 5 && momentDay.day() > 0) && moment().diff(momentDay) >= 0;
                        }
                        if (!isvalid) {
                            element.children('input').addClass('ng-invalid');
                        }
                        else {
                            element.children('input').removeClass('ng-invalid');
                        }
                        return isvalid;
                    };
                };
            }
            ValidateWeekendDate.factory = function () {
                var directive = function () { return new ValidateWeekendDate(); };
                directive.$inject = [];
                return directive;
            };
            return ValidateWeekendDate;
        }());
        Directives.ValidateWeekendDate = ValidateWeekendDate;
        var FormatTextAsCurrency = (function () {
            function FormatTextAsCurrency() {
                this.link = function (scope, element, attrs, ctrl) {
                    function commaSeparateNumber(val) {
                        if (val != null && val.length > 0) {
                            return parseFloat(val.replace(/,/g, ""))
                                .toFixed(2)
                                .toString()
                                .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                        }
                        else {
                            return "";
                        }
                    }
                    $(element[0]).focusout(function () {
                        $(this).val(commaSeparateNumber($(this).val()));
                    });
                    $(element[0]).focusin(function () {
                        if ($(this).val().length > 0) {
                            $(this).val($(this).val().replaceAll(",", ""));
                        }
                    });
                };
            }
            FormatTextAsCurrency.factory = function () {
                var directive = function () { return new FormatTextAsCurrency(); };
                directive.$inject = [];
                return directive;
            };
            return FormatTextAsCurrency;
        }());
        Directives.FormatTextAsCurrency = FormatTextAsCurrency;
        angular.module("app").directive("fileProcessor", FileProcessor.factory());
        angular.module("app").directive("onlyNumericKeys", OnlyNumricKeys.factory());
        angular.module("app").directive("formatTextAsCurrency", FormatTextAsCurrency.factory());
        angular.module("app").directive("validateWeekendDate", ValidateWeekendDate.factory());
    })(Directives = Application.Directives || (Application.Directives = {}));
})(Application || (Application = {}));
//# sourceMappingURL=Directives.js.map