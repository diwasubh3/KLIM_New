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
        var DatePicker = (function () {
            function DatePicker() {
                this.link = function (scope, element, attrs, ctrl) {
                    $(element).datepicker();
                };
            }
            DatePicker.factory = function () {
                var directive = function () { return new DatePicker(); };
                directive.$inject = [];
                return directive;
            };
            return DatePicker;
        }());
        Directives.DatePicker = DatePicker;
        var AutoRefreshButton = (function () {
            function AutoRefreshButton() {
                this.link = function (scope, element, attrs, ctrl) {
                    if (window.localStorage.getItem($(element).attr('id')) == null) {
                        window.localStorage.setItem($(element).attr('id'), "true");
                    }
                    if (window.localStorage.getItem($(element).attr('id')) == "false") {
                        $(element).removeAttr("checked");
                        angular.element("#autoRefresh").scope()['topNav'].isAutoRefresh = false;
                    }
                    else {
                        $(element).attr("checked", "checked");
                        angular.element("#autoRefresh").scope()['topNav'].isAutoRefresh = true;
                    }
                    $(element).bootstrapToggle();
                    $(element).change(function () {
                        window.localStorage.setItem($(element).attr('id'), $(element).prop('checked'));
                        angular.element("#autoRefresh").scope()['topNav'].isAutoRefresh = $(element).prop('checked');
                    });
                };
            }
            AutoRefreshButton.factory = function () {
                var directive = function () { return new AutoRefreshButton(); };
                directive.$inject = [];
                return directive;
            };
            return AutoRefreshButton;
        }());
        Directives.AutoRefreshButton = AutoRefreshButton;
        var ToggleButton = (function () {
            function ToggleButton() {
                this.link = function (scope, element, attrs, ctrl) {
                    $(element).bootstrapToggle();
                };
            }
            ToggleButton.factory = function () {
                var directive = function () { return new ToggleButton(); };
                directive.$inject = [];
                return directive;
            };
            return ToggleButton;
        }());
        Directives.ToggleButton = ToggleButton;
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
        var OptionsClass = (function () {
            function OptionsClass($parse) {
                var _this = this;
                this.$parse = $parse;
                this.link = function (scope, elem, attrs, ngSelect) {
                    try {
                        // get the source for the items array that populates the select.
                        var optionsSourceStr = attrs.ngOptions.split(' ').pop(), 
                        // use $parse to get a function from the options-class attribute
                        // that you can use to evaluate later.
                        getOptionsClass = _this.$parse(attrs.optionsClass);
                        scope.$watch(optionsSourceStr, function (items) {
                            // when the options source changes loop through its items.
                            angular.forEach(items, function (item, index) {
                                // evaluate against the item to get a mapping object for
                                // for your classes.
                                var classes = getOptionsClass(item), 
                                // also get the option you're going to need. This can be found
                                // by looking for the option with the appropriate index in the
                                // value attribute.
                                option = elem.find('option[value=' + index + ']');
                                var crappier = angular.element(option);
                                //if (item["isDefault"]) {
                                //	option.context.style.color = "blue";
                                //} else {
                                //	option.context.style.color = "green";
                                //}
                                var crap = option.context["0"];
                                //// now loop through the key/value pairs in the mapping object
                                //// and apply the classes that evaluated to be truthy.
                                //angular.forEach(classes, function (add, className) {
                                //	if (add) {
                                //		//angular.element(option).addClass(className);
                                //		var el = angular.element(option);
                                //		el.addClass(className);
                                //		//el.css("color", "blue");
                                //		var crap = el.context;
                                //	}
                                //});
                            });
                        });
                    }
                    catch (e) {
                        alert(e);
                    }
                };
            }
            OptionsClass.factory = function () {
                var directive = function ($parse) { return new OptionsClass($parse); };
                directive.$inject = ['$parse'];
                return directive;
            };
            return OptionsClass;
        }());
        Directives.OptionsClass = OptionsClass;
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
                    //$(element[0]).load(function () {
                    //	console.log("bind");
                    //	$(this).val(commaSeparateNumber($(this).val()));
                    //});
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
        var CsvDataService = (function () {
            function CsvDataService() {
            }
            CsvDataService.prototype.exportToCsv = function (filename, rows) {
                if (!rows || !rows.length) {
                    return;
                }
                var separator = ',';
                var keys = Object.keys(rows[0]);
                var csvContent = keys.join(separator) +
                    '\n' +
                    rows.map(function (row) {
                        return keys.map(function (k) {
                            var cell = row[k] === null || row[k] === undefined ? '' : row[k];
                            cell = cell instanceof Date
                                ? cell.toLocaleString()
                                : cell.toString().replace(/"/g, '""');
                            if (cell.search(/("|,|\n)/g) >= 0) {
                                cell = "\"" + cell + "\"";
                            }
                            return cell;
                        }).join(separator);
                    }).join('\n');
                var blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
                if (navigator.msSaveBlob) {
                    navigator.msSaveBlob(blob, filename);
                }
                else {
                    var link = document.createElement('a');
                    if (link.download !== undefined) {
                        var url = URL.createObjectURL(blob);
                        link.setAttribute('href', url);
                        link.setAttribute('download', filename);
                        link.style.visibility = 'hidden';
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    }
                }
                return true;
            };
            CsvDataService.factory = function () {
                var directive = function () { return new CsvDataService(); };
                directive.$inject = [];
                return directive;
            };
            return CsvDataService;
        }());
        Directives.CsvDataService = CsvDataService;
        var FormatTextAsCurrency4 = (function () {
            function FormatTextAsCurrency4() {
                this.link = function (scope, element, attrs, ctrl) {
                    function commaSeparateNumber(val) {
                        if (val != null && val.length > 0) {
                            var options = { style: 'currency', currency: 'USD', minimumFractionDigits: 4 };
                            return (new Intl.NumberFormat('en-US', options).format(val)).replace("$", "");
                            //return parseFloat(val.replace(/,/g, ""))
                            //	.toFixed(4)
                            //	.toString()
                            //	.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
            FormatTextAsCurrency4.factory = function () {
                var directive = function () { return new FormatTextAsCurrency4(); };
                directive.$inject = [];
                return directive;
            };
            return FormatTextAsCurrency4;
        }());
        Directives.FormatTextAsCurrency4 = FormatTextAsCurrency4;
        var ScrollGroup = (function () {
            function ScrollGroup() {
                this.link = function (scope, element, attrs, ctrl) {
                    element.bind("scroll", function (e) {
                        e.preventDefault();
                        if ($('.child').length) {
                            $('.parent').scrollLeft($('.child').scrollLeft());
                        }
                        $('.childX').scrollLeft($('.parent').scrollLeft());
                        $('.childY').scrollTop($('.parent').scrollTop());
                    });
                };
            }
            ScrollGroup.factory = function () {
                var directive = function () { return new ScrollGroup(); };
                directive.$inject = [];
                return directive;
            };
            return ScrollGroup;
        }());
        Directives.ScrollGroup = ScrollGroup;
        function sumOfValue() {
            return function (data, key) {
                if (angular.isUndefined(data) || angular.isUndefined(key))
                    return 0;
                var sum = 0;
                angular.forEach(data, function (value) {
                    sum = sum + parseFloat(value[key] ? value[key].toString().replace(/[^\d\.]/g, '') : '0');
                });
                return sum;
            };
        }
        Directives.sumOfValue = sumOfValue;
        function sumOfDifferenceValue() {
            return function (data, key1, key2) {
                if (angular.isUndefined(data) || angular.isUndefined(key1) || angular.isUndefined(key2))
                    return 0;
                var sum = 0;
                angular.forEach(data, function (value) {
                    sum = sum + (parseFloat(value[key1] ? value[key1].toString().replace(/[^\d\.]/g, '') : '0') - parseFloat(value[key2] ? value[key2].toString().replace(/[^\d\.]/g, '') : 0));
                });
                return sum;
            };
        }
        Directives.sumOfDifferenceValue = sumOfDifferenceValue;
        function dynamicFilter($filter) {
            return function (value, key1) {
                try {
                    if (key1 == null || key1 == "" || value == null || value == "" || (!value))
                        return value;
                    var newValue = value;
                    var filterArgs = key1.split(":");
                    if (filterArgs.length > 1 && !isNaN(Number(filterArgs[1])))
                        newValue = $filter(filterArgs[0])(value, filterArgs[1]);
                    else if (filterArgs.length == 1) {
                        newValue = $filter(key1)(value);
                    }
                    return newValue;
                }
                catch (e) {
                    console.log(e);
                    return value;
                }
            };
        }
        Directives.dynamicFilter = dynamicFilter;
        function sumOfValueMultiplyNum() {
            return function (data, key, num1) {
                if (angular.isUndefined(data) || angular.isUndefined(key) || angular.isUndefined(num1))
                    return 0;
                var sum = 0;
                angular.forEach(data, function (value) {
                    sum = sum + (parseFloat(value[key] ? value[key].toString().replace(/[^\d\.]/g, '') : '0') * num1);
                });
                return sum;
            };
        }
        Directives.sumOfValueMultiplyNum = sumOfValueMultiplyNum;
        function CustomModalFilterDirective() {
            return {
                template: '<div style="" class="p-0"><div class="pull-right p-0" ><a style="margin:0 15px 0 15px" ng-show="colFilter.term" ng-click="clear()"><i class="fa fa-times" aria-hidden="true"></i></a><a ng-click="show()"><i class="fa fa-filter" aria-hidden="true"></i></a></div></div>',
                controller: 'application.controllers.customModalFilterCtrl'
            };
        }
        Directives.CustomModalFilterDirective = CustomModalFilterDirective;
        //export function addCommasToInteger(val): ng.IDirective {
        //	var commas, decimals, wholeNumbers;
        //	decimals = val.indexOf('.') == -1 ? '' : val.replace(/^\d+(?=\.)/, '');
        //	wholeNumbers = val.replace(/(\.\d+)$/, '');
        //	commas = wholeNumbers.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
        //	return "" + commas + decimals;
        //   }
        angular.module("app").directive("fileProcessor", FileProcessor.factory());
        angular.module("app").directive("onlyNumericKeys", OnlyNumricKeys.factory());
        angular.module("app").directive("datePicker", DatePicker.factory());
        angular.module("app").directive("formatTextAsCurrency", FormatTextAsCurrency.factory());
        angular.module("app").directive("formatTextAsCurrency4", FormatTextAsCurrency4.factory());
        angular.module("app").directive("scrollGroup", ScrollGroup.factory());
        angular.module("app").directive("validateWeekendDate", ValidateWeekendDate.factory());
        angular.module("app").filter("dynamicFilter", dynamicFilter);
        angular.module("app").filter("sumOfValue", sumOfValue);
        angular.module("app").filter("sumOfValueMultiplyNum", sumOfValueMultiplyNum);
        angular.module("app").filter("sumOfDifferenceValue", sumOfDifferenceValue);
        angular.module("app").directive("autoRefreshButton", AutoRefreshButton.factory());
        angular.module("app").directive("toggleButton", ToggleButton.factory());
        angular.module("app").directive("customModalFilter", CustomModalFilterDirective);
        angular.module("app").directive("optionsClass", OptionsClass.factory());
        angular.module("app").directive("csvDataService", CsvDataService.factory());
        //angular.module("app", []).directive('fcsaNumber', addCommasToInteger);
    })(Directives = Application.Directives || (Application.Directives = {}));
})(Application || (Application = {}));
//# sourceMappingURL=Directives.js.map