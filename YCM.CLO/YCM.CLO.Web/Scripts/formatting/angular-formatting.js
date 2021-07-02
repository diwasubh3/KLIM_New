angular.module("app").constant('formatFactory', {
    currency: {
        pattern: /^[\(\-\+]?\d*,?\d*\.?\d+[\)]?$/,
        patternError: 'This field must be formatted as Currency.<br/>A sample valid input looks like: 1000.00',
        replace: /[,\$]/g,
        symbol:''
    },
    currency2: {
        pattern: /^[\(\-\+]?\d*,?\d*\.?\d+[\)]?$/,
        patternError: 'This field must be formatted as Currency.<br/>A sample valid input looks like: 1000.00',
        replace: /[,\$]/g,
        symbol: ''
    },
    percent: {
        pattern: /^\d+(\.\d+)?$/,
        patternError: 'The value you entered is not a valid percentage.',
        replace: /[,%]/g,
        symbol: '%'
    },
    phone: {
        pattern: /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/,
        patternError: 'This field must be formatted as a Phone Number.<br/>A sample valid input looks like 123-456-7890',
        replace: /[-\.\(\)\sa-zA-Z]/g
    }
})
        .filter('percent', function () {
            return function (input) {
                if (!input || isNaN(input)) {
                    return;
                } else {
                    return input + '%';
                }
            };
        })
        .filter('phone', function () {
            'use strict';

            return function (input, parens, separator) {
                var out,
                    arrOut,
                    i;

                if (!input) {
                    return;
                } else {
                    if (!isNaN(input)) {
                        input = input.toString();
                    }
                    separator = separator || '-';
                    input = input.replace(' ', '');
                    input = input.replace('(', '');
                    input = input.replace(')', '');
                    input = input.replace('.', '');

                    arrOut = input.split('');

                    // Loop through the array and remove anything that's NaN
                    for (i = 0; i < arrOut.length; i++) {
                        if (isNaN(parseInt(arrOut[i], 10))) {
                            arrOut.splice(i, 1);
                        }
                    }

                    // Put all the separators back
                    arrOut.splice(0, 0, '(');
                    arrOut.splice(4, 0, ') ');
                    arrOut.splice(8, 0, separator);
                    out = arrOut.join('');

                    return out;
                }
            };
        })
        .directive('quickInput', function () {
            return {
                scope: true,
                replace: true,
                transclude: true,
                template: '<div class="form-group"><div ng-transclude></div><ul ng-if="modelCtrl.$invalid && modelCtrl.$touched"><li class="error" ng-if="modelCtrl.$error.required || modelCtrl.$error.parse">This field is required.</li><li class="error" ng-show="modelCtrl.$error.pattern && format.patternError">{{format.patternError}}</li></ul></div>',
                controller: ['$scope',function ($scope) {
                    this.setFormatting = function (format) {
                        $scope.format = format;
                    };

                    this.setLabel = function (label) {
                        $scope.label = label;
                    };

                    this.setId = function (id) {
                        $scope.id = id;
                    };

                    this.setModelCtrl = function (modelCtrl) {
                        $scope.modelCtrl = modelCtrl;
                    };
                }]
            };
        })
        .directive('field', function () {
            return {
                require: ['ngModel', '?^quickInput', '?^form'],
                link: function (scope, element, attrs, ctrls) {
                    var ngModelCtrl = ctrls[0],
                        quickInputCtrl = ctrls[1],
                        ngFormCtrl = ctrls[2];
                    // Give the input its name
                    ngModelCtrl.$name = attrs.id;
                    // Tell the form all about it
                    if (ngFormCtrl) {
                        ngFormCtrl.$addControl(ngModelCtrl);
                    }
                    if (quickInputCtrl) {
                        quickInputCtrl.setLabel(attrs.title);
                        quickInputCtrl.setId(attrs.id);
                        quickInputCtrl.setModelCtrl(ngModelCtrl);
                    }
                }
            };
        })
        .directive('format', ['$filter', 'formatFactory', function ($filter, formatFactory) {
            return {
                scope: true,
                restrict: 'A',
                require: ['ngModel', '?^quickInput'],
                link: function (scope, element, attrs, ctrls) {
                    var ngModelCtrl = ctrls[0],
                        quickInputCtrl = ctrls[1],
                        thisFormat = formatFactory[attrs.format];

                    // This is the toModel routine
                    var parser = function (value) {
                        var removeParens;
                        if (value == null || value.toString().length==0) {
                            return '';
                        }
                        // get rid of currency indicators
                        value = value.toString().replace(thisFormat.replace, '');
                        // Check for parens, currency filter (5) is -5
                        removeParens = value.replace(/[\(\)]/g, '');
                        // having parens indicates the number is negative
                        if (value.length !== removeParens.length) {
                            value = -removeParens;
                        }
                        return value || undefined;
                    },
                        // This is the toView routine
                        formatter = function (value) {
                            if (value != null && value.toString().length) {
                                // the currency filter returns undefined if parse error
                                if (attrs.format == 'currency') {
                                    return $filter('currency')(parser(value), thisFormat.symbol, 0) || thisFormat.symbol || '';
                                }if (attrs.format == 'currency2') {
                                    return $filter('currency')(parser(value), thisFormat.symbol, 2) || thisFormat.symbol || '';
                                }
                                else {
                                    return $filter(attrs.format)(parser(value)) || thisFormat.symbol || '';
                                }
                            } else {
                                return "";
                            }
                        };

                 

                    // This sets the format/parse to happen on blur/focus
                    element.on("blur", function () {
                        ngModelCtrl.$setViewValue(formatter(this.value));
                        ngModelCtrl.$render();
                    }).on("focus", function () {
                        ngModelCtrl.$setViewValue(parser(this.value));
                        ngModelCtrl.$render();
                    });

                    // Model Formatter
                    ngModelCtrl.$formatters.push(formatter);
                    // Model Parser
                    ngModelCtrl.$parsers.push(parser);

                   
                    if (quickInputCtrl) {
                        quickInputCtrl.setFormatting(thisFormat);
                    }
                }
            }
        }])
;
