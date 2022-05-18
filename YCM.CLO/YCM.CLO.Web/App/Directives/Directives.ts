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
			$((<HTMLFormElement>$(element).parent().parent()[0])).on('change', (e: Event) => {
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

	export class DatePicker implements ng.IDirective {

		link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {
			$(element).datepicker();
		}
		static factory(): ng.IDirectiveFactory {
			var directive = () => new DatePicker();
			directive.$inject = [];
			return directive;
		}
	}

	export class AutoRefreshButton implements ng.IDirective {

		link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {
			if (window.localStorage.getItem($(element).attr('id')) == null) {
				window.localStorage.setItem($(element).attr('id'), "true");
			}
			if (window.localStorage.getItem($(element).attr('id')) == "false") {
				$(element).removeAttr("checked");
				angular.element("#autoRefresh").scope()['topNav'].isAutoRefresh = false;
			} else {
				$(element).attr("checked", "checked");
				angular.element("#autoRefresh").scope()['topNav'].isAutoRefresh = true;
			}

			$(element).bootstrapToggle();
			$(element).change(function () {
				window.localStorage.setItem($(element).attr('id'), $(element).prop('checked'));
				angular.element("#autoRefresh").scope()['topNav'].isAutoRefresh = $(element).prop('checked');
			});
		}
		static factory(): ng.IDirectiveFactory {
			var directive = () => new AutoRefreshButton();
			directive.$inject = [];
			return directive;
		}
	}


	export class ToggleButton implements ng.IDirective {

		link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {
			$(element).bootstrapToggle();
		}
		static factory(): ng.IDirectiveFactory {
			var directive = () => new ToggleButton();
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

	export class OptionsClass implements ng.IDirective {
		require: 'select';
		constructor(private $parse: ng.IParseService) {
		}

		link = (scope: ng.IScope, elem: any, attrs: ng.IAttributes, ngSelect: any) => {
			try {
				// get the source for the items array that populates the select.
				var optionsSourceStr = attrs.ngOptions.split(' ').pop(),
					// use $parse to get a function from the options-class attribute
					// that you can use to evaluate later.
					getOptionsClass = this.$parse(attrs.optionsClass);

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
			} catch
						(e) {
				alert(e);
			}
		}
		static factory(): ng.IDirectiveFactory {
			var directive = ($parse: ng.IParseService) => new OptionsClass($parse);
			directive.$inject = ['$parse'];
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
			//$(element[0]).load(function () {
			//	console.log("bind");
			//	$(this).val(commaSeparateNumber($(this).val()));
			//});
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

	export class CsvDataService implements ng.IDirective {
		exportToCsv(filename, rows) {
			if (!rows || !rows.length) {
				return;
			}
			const separator = ',';
			const keys = Object.keys(rows[0]);
			const csvContent =
				keys.join(separator) +
				'\n' +
				rows.map(row => {
					return keys.map(k => {
						let cell = row[k] === null || row[k] === undefined ? '' : row[k];
						cell = cell instanceof Date
							? cell.toLocaleString()
							: cell.toString().replace(/"/g, '""');
						if (cell.search(/("|,|\n)/g) >= 0) {
							cell = `"${cell}"`;
						}
						return cell;
					}).join(separator);
				}).join('\n');

			const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
			if (navigator.msSaveBlob) { // IE 10+
				navigator.msSaveBlob(blob, filename);
			} else {
				const link = document.createElement('a');
				if (link.download !== undefined) {
					const url = URL.createObjectURL(blob);
					link.setAttribute('href', url);
					link.setAttribute('download', filename);
					link.style.visibility = 'hidden';
					document.body.appendChild(link);
					link.click();
					document.body.removeChild(link);
				}
			}
			return true;
		}
		static factory(): ng.IDirectiveFactory {
			var directive = () => new CsvDataService();
			directive.$inject = [];
			return directive;
		}
	}

	export class FormatTextAsCurrency4 implements ng.IDirective {

		link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {
			function commaSeparateNumber(val) {
				if (val != null && val.length > 0) {
					var options = { style: 'currency', currency: 'USD', minimumFractionDigits: 4 };
					return (new Intl.NumberFormat('en-US', options).format(val)).replace("$","");
					//return parseFloat(val.replace(/,/g, ""))
					//	.toFixed(4)
					//	.toString()
					//	.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
			var directive = () => new FormatTextAsCurrency4();
			directive.$inject = [];
			return directive;
		}
	}


	export class ScrollGroup implements ng.IDirective {
		link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any) => {
			
			element.bind("scroll", function (e) {
                e.preventDefault();

                if ($('.child').length)
                {
                    $('.parent').scrollLeft($('.child').scrollLeft());
                }
                
                $('.childX').scrollLeft($('.parent').scrollLeft());
                $('.childY').scrollTop($('.parent').scrollTop());
			});
		}
		static factory(): ng.IDirectiveFactory {
			var directive = () => new ScrollGroup();
			directive.$inject = [];
			return directive;
		}
    }

	export function sumOfValue() {
		return (data: Array<any>, key: string): number => {
			if (angular.isUndefined(data) || angular.isUndefined(key))
				return 0;
			var sum = 0;
			angular.forEach(data, value => {
				sum = sum + parseFloat(value[key] ? value[key].toString().replace(/[^\d\.]/g, '') : '0');
			});
			return sum;
		}
	}

	export function sumOfDifferenceValue() {
		return (data: Array<any>, key1: string, key2: string): number => {
			if (angular.isUndefined(data) || angular.isUndefined(key1) || angular.isUndefined(key2))
				return 0;
			var sum = 0;
			angular.forEach(data, value => {
				sum = sum + (parseFloat(value[key1] ? value[key1].toString().replace(/[^\d\.]/g, '') : '0') - parseFloat(value[key2] ? value[key2].toString().replace(/[^\d\.]/g, '') : 0));
			});
			return sum;
		}
	}

    export function dynamicFilter($filter) {
		return (value: any, key1: string): any => {
			try {
				if (key1 == null || key1 == "" || value == null || value=="" || (!value))
					return value;
				var newValue = value;
				var filterArgs = key1.split(":");
				if (filterArgs.length > 1 && !isNaN(Number(filterArgs[1])))
					newValue = $filter(filterArgs[0])(value, filterArgs[1]);
				else if (filterArgs.length == 1) {
					newValue = $filter(key1)(value);
				}
				return newValue;
			} catch (e) {
				console.log(e);
				return value;
			}
		}
	}

	export function sumOfValueMultiplyNum() {
		return (data: Array<any>, key: string, num1: number): number => {
			if (angular.isUndefined(data) || angular.isUndefined(key) || angular.isUndefined(num1))
				return 0;
			var sum = 0;
			angular.forEach(data, value => {
				sum = sum + (parseFloat(value[key] ? value[key].toString().replace(/[^\d\.]/g, '') : '0') * num1);
			});
			return sum;
		}
	}

	export function CustomModalFilterDirective(): ng.IDirective {
		return {
			template: '<div style="" class="p-0"><div class="pull-right p-0" ><a style="margin:0 15px 0 15px" ng-show="colFilter.term" ng-click="clear()"><i class="fa fa-times" aria-hidden="true"></i></a><a ng-click="show()"><i class="fa fa-filter" aria-hidden="true"></i></a></div></div>',
			controller: 'application.controllers.customModalFilterCtrl'
		};
	}

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
}
