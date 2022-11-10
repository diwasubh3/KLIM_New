(function () {
    if (!window.console) {
        window.console = {};
    }
    // union of Chrome, FF, IE, and Safari console methods
    var m = [
      "log", "info", "warn", "error", "debug", "trace", "dir", "group",
      "groupCollapsed", "groupEnd", "time", "timeEnd", "profile", "profileEnd",
      "dirxml", "assert", "count", "markTimeline", "timeStamp", "clear"
    ];
    // define undefined methods as noops to prevent errors
    for (var i = 0; i < m.length; i++) {
        if (!window.console[m[i]]) {
            window.console[m[i]] = function () { };
        }
    }
})();

Date.prototype.addHours = function(h) {
    this.setTime(this.getTime() + (h * 60 * 60 * 1000));
    return this;
};

Date.prototype.getCurrentTime = function() {
    var dTime = new Date();
    var hours = dTime.getHours();
    var minute = dTime.getMinutes();
    return (hours + ":" + minute);
};

String.prototype.replaceAll = function(character, replaceChar) {
    var word = this.valueOf();

    while (word.indexOf(character) != -1)
        word = word.replace(character, replaceChar);

    return word;
};

String.prototype.compareTime = function (endTime) {
    var startTime = this.valueOf();
    
    var regExp = /(\d{1,2})\:(\d{1,2})/;

    if (parseInt(endTime.replace(regExp, "$1$2")) > parseInt(startTime.replace(regExp, "$1$2"))) {
        return 1;
    } else if (parseInt(endTime.replace(regExp, "$1$2")) < parseInt(startTime.replace(regExp, "$1$2"))) {
        return -1;
    } else {
        return 0;
    }
};


     var yorkCore = {
        isMsIe: false,
        isMobileDevice: true,
        configHelper: { }
    };

var isMobile = {
    Android: function () {
        return ($('#userAgent').length > 0 ? $('#userAgent').val():navigator.userAgent).match(/Android/i);
    },
    BlackBerry: function () {
        return ($('#userAgent').length > 0 ? $('#userAgent').val() : navigator.userAgent).match(/BlackBerry/i);
    },
    iPhone: function () {
        return ($('#userAgent').length > 0 ? $('#userAgent').val() : navigator.userAgent).match(/iPhone|iPod/i);
    },
    iPad: function () {
        return ($('#userAgent').length > 0 ? $('#userAgent').val() : navigator.userAgent).match(/iPad/i);
    },
    Opera: function () {
        return ($('#userAgent').length > 0 ? $('#userAgent').val() : navigator.userAgent).match(/Opera Mini/i);
    },
    Windows: function () {
        return ($('#userAgent').length > 0 ? $('#userAgent').val() : navigator.userAgent).match(/IEMobile/i);
    },
    Desktop: function () {
        return ($('#userAgent').length > 0 ? $('#userAgent').val() : navigator.userAgent).match(/Windows/);
    },
    any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iPad() || isMobile.iPhone() || isMobile.Opera() || isMobile.Windows());
    }
};


yorkCore.isMsIe = navigator.appName.indexOf("Microsoft") >= 0;
yorkCore.isMobileDevice = isMobile.any();
yorkCore.isPhone = isMobile.Android() || isMobile.BlackBerry() || isMobile.iPhone();
yorkCore.isTablet = isMobile.iPad();
yorkCore.isDeskTop = isMobile.Desktop();

$(function() {
    $.ajaxSetup({
        error: function(jqXHR, exception) {
            if (jqXHR.status === 0) {
                console.log('Not connect.\n Verify Network.');
            } else if (jqXHR.status == 404) {
                console.log('Requested page not found. [404]');
            } else if (jqXHR.status == 500) {
                console.log('Internal Server Error [500].');
            } else if (exception === 'parsererror') {
                console.log('Requested JSON parse failed.');
            } else if (exception === 'timeout') {
                console.log('Time out error.');
            } else if (exception === 'abort') {
                console.log('Ajax request aborted.');
            } else {
                console.log('Uncaught Error.\n' + jqXHR.responseText);
            }
        }
    });
});

checkIfNetworkIsConnected = function(parameters) {
    try {

        if (typeof(resetSweetGreenButton) === typeof(Function)) {
            resetSweetGreenButton();
        }
        
        if ($("body").length === 0) {
            console.log("Permission Denied error has occured.");
        } else {
            setTimeout("checkIfNetworkIsConnected();", 30000);
        }
    } catch (e) {
        console.log('An error occured' + e.toString());
        window.location.href = window.location.href;
    } 
};

$(function() {
    checkIfNetworkIsConnected();
});


yorkCore.ServerPath = window.location.hostname == "localhost" ? '/yorknet' : '';