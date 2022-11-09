declare var pageOptions: any;

module Application.Directives {
    declare var pageOptions: any;

    //Example Template
    //<form id="frmFileUpload" style= "margin-bottom: 0px" ng- upload="entryupload.complete(content)" ng- upload - loading="entryupload.startUploading()" method= "post" enctype= "multipart/form-data" >
    //<input type="file" file- processor title= 'Upload Entries' id= "uploadedfile" name= "uploadedfile"
    //accept = ".xlsx" multiple= "" />
    //</form>    

    export class FileProcessor implements ng.IDirective {
        restrict = 'A';

        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {
            $(element).bootstrapFileInput();
            (<HTMLFormElement>$(element).parent().parent()[0]).action = pageOptions.webBasePath + "/" + pageOptions.appName + '/Data/UploadBalanceAmounts';
            $((<HTMLFormElement>$(element).parent().parent()[0])).on('change', (e:Event) => {
                window.setTimeout(() => {
                    $((<HTMLElement>e.target).parentNode.parentNode).submit();
                });
            });
        }

        static factory(): ng.IDirectiveFactory {
            var directive = () => new FileProcessor();
            directive.$inject = [];
            return directive;
        }
    }

    export class OnlyNumricKeys implements ng.IDirective {

        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {
            var keyCode = [8, 9, 37, 39, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 110, 188, 190, 189];
            element.bind("keydown", event => {
                if ($.inArray(event.which, keyCode) == -1) {
                    scope.$apply(() => {
                        scope.$eval(attrs['onlynumrickeys']);
                        event.preventDefault();
                    });
                    event.preventDefault();
                }

            });
        }
        static factory(): ng.IDirectiveFactory {
            var directive = () => new OnlyNumricKeys();
            directive.$inject = [];
            return directive;
        }
    }

    export class ValidateWeekendDate implements ng.IDirective {
        require = "ngModel";

        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {

            ctrl.$validators.validateweekenddate = (modelValue, viewValue) => {
                var isvalid = true;
                if (ctrl.$isEmpty(modelValue)) {
                    // consider empty models to be valid
                    isvalid = false;
                } else {
                    var momentDay = moment(scope['bal'].Date);
                    isvalid = (momentDay.day() <= 5 && momentDay.day() > 0) && moment().diff(momentDay) >= 0;
                }

                if (!isvalid) {
                    element.children('input').addClass('ng-invalid');
                } else {
                    element.children('input').removeClass('ng-invalid');
                }

                return isvalid;

            }
        }

        static factory(): ng.IDirectiveFactory {
            var directive = () => new ValidateWeekendDate();
            directive.$inject = [];
            return directive;
        }
    }

    export class FormatTextAsCurrency implements ng.IDirective {
        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {

            function commaSeparateNumber(val) {
                if (val != null && val.length > 0) {
                    return parseFloat(val.replace(/,/g, ""))
                        .toFixed(2)
                        .toString()
                        .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                } else {
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
        }
        static factory(): ng.IDirectiveFactory {
            var directive = () => new FormatTextAsCurrency();
            directive.$inject = [];
            return directive;
        }
    }

    angular.module("app").directive("fileProcessor", FileProcessor.factory());
    angular.module("app").directive("onlyNumericKeys", OnlyNumricKeys.factory());
    angular.module("app").directive("formatTextAsCurrency", FormatTextAsCurrency.factory());
    angular.module("app").directive("validateWeekendDate", ValidateWeekendDate.factory());
}