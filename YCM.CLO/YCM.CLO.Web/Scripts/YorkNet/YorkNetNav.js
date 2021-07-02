/// <reference path="../jquery-1.9.1.js" />
/// <reference path="../bootstrap.min.js" />
(function ($) {

    var yorkNavigationMenu = {};

    $.fn.YorkNetNav = function (options) {

        var defaults = {
            pageTitle: 'YorkNet',
            pageUri: 'javascript:;',
            ribbonIconVisible: false,
            onRibbonIconClick: function () {; },
            subnavVisible: false,
            activeMenuItem: 'Home',
            menuWebApiUri: 'http://localhost:61406/NavigationMenu/GetMenus',
            sharepointBasePath: 'http://nydshpweb01',
            webBasePath: 'http://localhost:64551',
            dwhBasePath: 'http://localhost',
            adjustPaddingTopDivId: '',
            brudCrumbTooltip: '',
            isSubNavBarContainerAvailable:false
        };


        if ($('#NavContainer').length === 0 ) {
            alert('Missing \'NavContainer\' Div, Please make sure that there is \'NavContainer\' Div in your HTML Dom');
        }
        if ($('#main-fixed-nav-bar').length === 0) {
            alert('Missing \'main-fixed-nav-bar\' Div, Please make sure that there is \'main-fixed-nav-bar\' Div in your HTML Dom');
        }
        if ($('#main-fixed-nav-bar-navbar-inner-container').length === 0) {
            alert('Missing \'main-fixed-nav-bar-navbar-inner-container\' Div, Please make sure that there is \'main-fixed-nav-bar-navbar-inner-container\' Div in your HTML Dom');
        }

        options = $.extend(defaults, options);
        options.activeMenuItem = decodeURIComponent(options.activeMenuItem).trim();

        yorkNavigationMenu.options = options;
        yorkNavigationMenu.menuWebApiUri = options.menuWebApiUri;

        yorkNavigationMenu.NavigationMenuBasePaths =
            [{ "Id": 1, "Type": "ParentMenu", "BasePath": null },
            { "Id": 2, "Type": "Web", "BasePath": options.webBasePath },
            { "Id": 3, "Type": "DWH", "BasePath": options.dwhBasePath },
            { "Id": 4, "Type": "Sharepoint", "BasePath": options.sharepointBasePath },
            { "Id": 5, "Type": "Complete Url", "BasePath": null }
            ];

        window.onresize = function () {
            $.fn.YorkNetNav.setBodyPaddingTopBasedOnSubMenu();
        };

        return this.each(function () {
            var el = $(this);

            yorkNavigationMenu.el = el;

            var html = [];
            renderNavbar(html, options);
            yorkNavigationMenu.el.find('#main-fixed-nav-bar-navbar-inner-container').append(html.join(''));

            if (!yorkNavigationMenu.isSubNavBarContainerAvailable) {
                html = [];
                renderSubnav(html, options);
                yorkNavigationMenu.el.find('#main-fixed-nav-bar').append(html.join(''));
            }
            
            html = [];
            renderFavoritesModalSecttion(html);
            yorkNavigationMenu.el.append(html.join(''));

            if (yorkNavigationMenu.options.subnavVisible)
                $('#subnav', yorkNavigationMenu.el).show();

            $.fn.YorkNetNav.setBodyPaddingTopBasedOnSubMenu();

            if (yorkNavigationMenu.options.ribbonIconVisible) {
                var ri = $('#ribbon-toggle');
                ri.bind('click', yorkNavigationMenu.options.onRibbonIconClick);
                $.fn.YorkNetNav.setBodyPaddingTopBasedOnSubMenu();
            }

            //wire up submenus
            $("#menu-button", yorkNavigationMenu.el).click(function (event) {
                $("#subnav", yorkNavigationMenu.el).toggle();
                event.preventDefault();
                $.fn.YorkNetNav.setBodyPaddingTopBasedOnSubMenu();
            });


            renderMenus(html, options);
        });

    };

    $.fn.YorkNetNav.BindMenu = function (data) {

        var menus = data.Menus;
        var html = [];

        yorkNavigationMenu.options.activeMenuItem = getActiveMenuTitle(data.Menus, yorkNavigationMenu.options);
        var strmenus = JSON.stringify(data);
        localStorage.YorkMenus = strmenus;
        html[html.length] = '<ul class="mainnav">';
        for (var i = 0; i < menus.length; i++) {
            renderMenu(html, yorkNavigationMenu.options, menus[i]);
        }

        html[html.length] = '</ul>';

        setupFavoriteLink(yorkNavigationMenu.options, menus);

        $('#york-menu-container').html(html.join(''));

        $('span:contains("' + yorkNavigationMenu.options.activeMenuItem + '")', yorkNavigationMenu.el).closest('li').addClass("active");

        setNavigationMenuHover();
        $.fn.YorkNetNav.setBodyPaddingTopBasedOnSubMenu();

        setEditFavoritesLink();

        window.setTimeout(setNavigationMenuHover, 2000);

        $("#york-navigationmenu-brand").tooltip();
    };

    function setNavigationMenuHover() {

        if (!yorkCore.isMobileDevice) {

            $('ul.mainnav li.dropdown').unbind('mouseenter');
            $('ul.mainnav li.dropdown').unbind('mouseleave');

            $('ul.mainnav li.dropdown').hover(function () {
                $(this).closest('.dropdown-menu').stop(true, true).show();
                $(this).addClass('open');
            }, function () {
                $(this).closest('.dropdown-menu').stop(true, true).hide();
                $(this).removeClass('open');
            });
        }

    }

    $.fn.YorkNetNav.AddOrRemoveFavoriteCallback = function (data) {
        if (data.AddOrRemoveResult) {

            if (data.AddOrRemove) {
                $('#linkaddremovefavorite').html('Remove Page From Favorites');
            } else {
                $('#linkaddremovefavorite').html('Add Page to Favorites');
            }

            setUpFavorites(data);
            assignFavoriteMenus(data.Menus[0]);
            $('#linkaddremovefavorite').removeAttr('disabled');
            localStorage.removeItem('YorkMenusCachedDateTime');
            localStorage.removeItem('YorkMenus');

        }
    };

    $.fn.YorkNetNav.GetFavoritesCallback = function (data) {
        setUpFavorites(data);
        setupFavoriteLink(yorkNavigationMenu.options, data.Menus);
        setNavigationMenuHover();
        setEditFavoritesLink();
    };

    $.fn.YorkNetNav.ShowFavoritesReorder = function () {

        generateFavoritesTable();

        $("#favorites-modal").modal("show");
    };

    function generateFavoritesTable() {

        $('#favorites-modal-main-body-table > tbody').empty();

        for (var i = 0; i < yorkNavigationMenu.FavoriteMenus.Children.length; i++) {

            var trtext = '<tr><td><input class="span2" maxlength="25" onchange="$.fn.YorkNetNav.UpdateFavoriteRowsTitle(this,ROW_NUM)" value="' + yorkNavigationMenu.FavoriteMenus.Children[i].Title +
                '"/> </td><td></td><td><a href="javascript:;" onclick="window.open(\'' + yorkNavigationMenu.FavoriteMenus.Children[i].Uri + '\')"><div>'
                + decodeURI(yorkNavigationMenu.FavoriteMenus.Children[i].Uri) + '</div></a></td><td></td><td><a  href="javascript:;" >ARROW_PLACEHOLDER </a></td></tr>';

            var arrowsection = '';

            if (i === 0) {
                if (yorkNavigationMenu.FavoriteMenus.Children.length > 1) {
                    arrowsection = '<a  href="javascript:;" title="Move Down" onclick="$.fn.YorkNetNav.swapFavoriteRows(0,1)" ><i class="icon-arrow-down"></i> </a>';
                }
            }
            else if (i !== 0 && i === (yorkNavigationMenu.FavoriteMenus.Children.length - 1)) {
                arrowsection = '<a title="Move Up" href="javascript:;" onclick="$.fn.YorkNetNav.swapFavoriteRows(' + (i - 1) + ',' + i + ')"  ><i class="icon-arrow-up"></i> </a>';
            } else {
                arrowsection = '<a  href="javascript:;" title="Move Down" onclick="$.fn.YorkNetNav.swapFavoriteRows(' + i + ',' + (i + 1)
                    + ')" ><i class="icon-arrow-down"></i> </a><a title="Move Up"  href="javascript:;" onclick="$.fn.YorkNetNav.swapFavoriteRows(' + (i - 1) + ',' + i
                    + ')" ><i class="icon-arrow-up"></i> </a>';
            }

            $('#favorites-modal-main-body-table > tbody').append(trtext.replace('ARROW_PLACEHOLDER', arrowsection).replace('ROW_NUM', i));
        }
    }

    $.fn.YorkNetNav.swapFavoriteRows = function (row1, row2) {

        if (yorkNavigationMenu.FavoriteMenus.Children.length > row1 && yorkNavigationMenu.FavoriteMenus.Children.length > row2) {
            var originalrow = yorkNavigationMenu.FavoriteMenus.Children[row1];
            yorkNavigationMenu.FavoriteMenus.Children[row1] = yorkNavigationMenu.FavoriteMenus.Children[row2];
            yorkNavigationMenu.FavoriteMenus.Children[row2] = originalrow;
        }

        generateFavoritesTable();
    };

    $.fn.YorkNetNav.UpdateFavoriteRowsTitle = function (txt, rownum) {
        if (yorkNavigationMenu.FavoriteMenus.Children.length >= rownum) {
            yorkNavigationMenu.FavoriteMenus.Children[rownum].Title = txt.value;
        }
    };

    $.fn.YorkNetNav.SaveFavoritesCallback = function (data) {
        try {

            $.fn.YorkNetNav.GetFavoritesCallback(data);

            $('#favorites-modal-main-save-button').button('reset');
            $('#favorites-modal-header-close-button').button('reset');
            $('#favorites-modal-main-close-button').button('reset');


        } catch (e) {
            console.log(e.toString());
        }
    };

    $.fn.YorkNetNav.saveFavoritesOrder = function () {
        try {
            var savefavoritesurl = yorkNavigationMenu.menuWebApiUri.toLowerCase().replace('getmenus', 'SavefavoriteMenus');
            $('#favorites-modal-main-save-button').button('loading');
            $('#favorites-modal-header-close-button').button('loading');
            $('#favorites-modal-main-close-button').button('loading');

            $.ajax(
                {
                    type: "GET",
                    url: savefavoritesurl,
                    dataType: "jsonp",
                    data: { favoritemenusstr: JSON.stringify(yorkNavigationMenu.FavoriteMenus) },
                    jsonpCallback: " $.fn.YorkNetNav.SaveFavoritesCallback",
                    error: function (request, status, error) {
                    }
                }
            );
        } catch (e) {
            console.log(e.toString());
        }

        return false;
    };


    function setUpFavorites(data) {
        try {
            var favoritemenu = $(yorkNavigationMenu.FavoriteMenuId);

            var favhtml = [];

            if (data.Menus[0].Children.length > 0) {
                favoritemenu.attr('class', 'dropdown');
            } else {
                favoritemenu.unbind('click');
            }

            renderMenuContent(favhtml, yorkNavigationMenu.options, data.Menus[0]);

            favhtml[favhtml.length] = '</ul>';

            favoritemenu.empty();

            var htmlcontent = favhtml.join('');
            favoritemenu.html(htmlcontent);

            setNavigationMenuHover();



        } catch (e) {
            console.log('error in setUpFavorites' + e.toString());
        }
    }

    function getFavorites(parameters) {
        var getfavoritesurl = yorkNavigationMenu.menuWebApiUri.toLowerCase().replace('getmenus', 'GetfavoriteMenus');
        $.ajax(
            {
                type: "GET",
                url: getfavoritesurl,
                dataType: "jsonp",
                jsonpCallback: " $.fn.YorkNetNav.GetFavoritesCallback",
                error: function (request, status, error) {
                }
            }
        );

    }

    $.fn.YorkNetNav.AddOrRemoveFavorite = function () {
        $('#linkaddremovefavorite').attr('disabled', 'disabled');
        var addremoveurl = yorkNavigationMenu.menuWebApiUri.toLowerCase().replace('getmenus', 'AddOrRemoveFavoriteNavigationMenu');

        $.ajax(
            {
                type: "GET",
                url: addremoveurl,
                dataType: "jsonp",
                data: {
                    'navigationurl': window.location.toString(),
                    'addorremove': $('#linkaddremovefavorite').text() === 'Add Page to Favorites',
                    title: $(document).find("title").text()
                },
                jsonpCallback: "$.fn.YorkNetNav.AddOrRemoveFavoriteCallback",
                error: function (request, status, error) {
                    $('#linkaddremovefavorite').removeAttr('disabled');
                }
            }
        );

    };

    function renderNavbar(html, options) {

        html[html.length] = '<div class="only-non-mobile-device">'; //nav-collapse
        html[html.length] = '<ul class="nav pull-right" style="padding-top:13px">'; //nav pull-right
        html[html.length] = '<li><a href="javascript:;"  style="visibility: ' + (options.ribbonIconVisible ? 'visible' : 'hidden') + '" id="ribbon-toggle" title="Toggle Ribbon"><i class="icon-bookmark"></i></a></li>';
        
        html[html.length] = '<li class="dropdown">'; //dropdown
        html[html.length] = '<a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" title="Settings"><i class="icon-cog"></i></a>';
        renderSettingsMenu(html);
        html[html.length] = '';
        html[html.length] = '';
        html[html.length] = '';
        html[html.length] = '</li>'; //dropdown
        html[html.length] = '</ul>'; //nav pull-right
        html[html.length] = '</div>'; //nav-collapse

    }

    function renderSettingsMenu(html) {
        html[html.length] = '<ul class="dropdown-menu">';
        html[html.length] = '<li id="addremovefavorite" name="addremovefavorite"><a href="javascript:;" id="linkaddremovefavorite" onclick="$.fn.YorkNetNav.AddOrRemoveFavorite()">Add Page to Favorites</a></li>';
        html[html.length] = '<li><a href="javascript:;" id="yorknavigatemenu-edit-favorites" >Edit Favorites</a></li>';
        html[html.length] = '<li class="divider"></li>';
        html[html.length] = '<li><a href="javascript:;" onclick="$.fn.YorkNetNav.ClearNavigationMenuCache()">Reset Menus</a></li>';
        html[html.length] = '</ul>';
    }

    function setEditFavoritesLink() {
        $('#yorknavigatemenu-edit-favorites').attr('onclick', '$.fn.YorkNetNav.ShowFavoritesReorder();');
    }

    function renderFavoritesModalSecttion(html) {
        html[html.length] = '<div id="favorites-modal" class="modal hide fade"  style="width:600px"';
        html[html.length] = 'role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        html[html.length] = '<div class="modal-header">';
        html[html.length] = '<button type="button" class="close" id="favorites-modal-header-close-button"  data-loading-text="×" data-dismiss="modal" aria-hidden="true">×</button>';
        html[html.length] = '<table class="table table-condensed table-hover" id="favorites-modal-header">';
        html[html.length] = '<tr>';
        html[html.length] = '<td><div><h4 id="favorites-modal-header-label">Favorites:</h4></div></td>';
        html[html.length] = '</tr>';
        html[html.length] = '</table>';
        html[html.length] = '</div>';
        html[html.length] = '<div class="modal-body">';
        html[html.length] = '<div id="favorites-modal-main-body">';
        html[html.length] = '<table style="width: 100%" id="favorites-modal-main-body-table" >';
        html[html.length] = '<thead>';
        html[html.length] = '<tr>';
        html[html.length] = '<td style="width:100px"><h6>Title</h6></td>';
        html[html.length] = '<td style="width:10px"></td>';
        html[html.length] = '<td><h6>Url</h6></td>';
        html[html.length] = '<td style="width:10px"></td>';
        html[html.length] = '<td style="width:100px"></td>';
        html[html.length] = '</tr>';
        html[html.length] = '</thead>';
        html[html.length] = '<tbody>';
        html[html.length] = '</tbody>';
        html[html.length] = '</table>';
        html[html.length] = '</div>';
        html[html.length] = '</div>';
        html[html.length] = '<div class="modal-footer">';
        html[html.length] = '<button class="btn" id="favorites-modal-main-close-button" data-loading-text="Close" data-dismiss="modal" aria-hidden="true">Close</button>';
        html[html.length] = '<button class="btn btn-primary" id="favorites-modal-main-save-button" onclick="return $.fn.YorkNetNav.saveFavoritesOrder();" data-loading-text="Saving..." >Save changes</button>';
        html[html.length] = '</div>';
        html[html.length] = '</div>';
    }

    $.fn.YorkNetNav.ClearNavigationMenuCache = function (parameters) {
        localStorage.removeItem('YorkMenusCachedDateTime');
        localStorage.removeItem('YorkMenus');
        window.location.href = window.location.href;
    };

    

    function renderSubnav(html, options, menus) {
        html[html.length] = '<div class="subnavbar" id="subnav" style="display : none">'; //subnavbar
        html[html.length] = '<div class="subnavbar-inner">'; //subnavbar-inner
        html[html.length] = '<div class="container" id="york-menu-container">'; //container

        html[html.length] = '</div>'; //container
        html[html.length] = '</div>'; //subnavbar-inner
        html[html.length] = '</div>'; //subnavbar

    }

    $.fn.YorkNetNav.setBodyPaddingTopBasedOnSubMenu = function () {

        if (yorkNavigationMenu.options.adjustPaddingTopDivId.length == 0 || yorkNavigationMenu.options.adjustPaddingTopDivId == undefined) {
            $("body").css('padding-top', $('#main-fixed-nav-bar').height());
        } else if ($('#' + yorkNavigationMenu.options.adjustPaddingTopDivId).length > 0) {
            $('#' + yorkNavigationMenu.options.adjustPaddingTopDivId).css('padding-top', $('#main-fixed-nav-bar').height());
            $(document).trigger('menuHeightChangedEvent', [$('#main-fixed-nav-bar').height()]);
        }


    };

    function renderMenus(html, options) {
        yorkNavigationMenu.loadedMenuFromServer = false;
        var getmenusfromserver = true;

        try {
            if (localStorage.getItem('YorkMenusCachedDateTime') !== null) {
                getmenusfromserver = (new Date(parseInt(localStorage.YorkMenusCachedDateTime))).addHours(24).valueOf()
                    < (new Date()).valueOf();
            }
        } catch (e) {
            console.log(e.toString());
            ClearNavigationMenuCache();
        }

        if (getmenusfromserver || (localStorage.getItem('YorkMenus') === null)) {

            localStorage.YorkMenusCachedDateTime = (new Date()).valueOf();
            yorkNavigationMenu.loadedMenuFromServer = true;

            $.ajax({
                type: "GET",
                url: yorkNavigationMenu.menuWebApiUri,
                dataType: "jsonp",
                jsonpCallback: "$.fn.YorkNetNav.BindMenu",
                error: function (request, status, error) {
                    if (request.status != 200) {
                        yorkNavigationMenu.loadedMenuFromServer = false;
                        localStorage.removeItem('YorkMenusCachedDateTime');

                        if (localStorage.getItem('YorkMenus') === null) {

                            localStorage.YorkMenus = '{"Menus":[{"$id":"1","Children":[],"Icon":"icon-home","Id":1,"Ordinal":1,"Parent":null,"ParentId":null,"Title":"Home","Uri":"","IsAvailableOnPhone":true,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":2},{"$id":"2","Children":[{"$id":"3","Children":[],"Icon":"","Id":25,"Ordinal":1,"Parent":{"$ref":"2"},"ParentId":2,"Title":"Emergency Info","Uri":"EmergencyInfo/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"4","Children":[],"Icon":"","Id":26,"Ordinal":2,"Parent":{"$ref":"2"},"ParentId":2,"Title":"Locations","Uri":"Facilities/SitePages/Locations.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"5","Children":[],"Icon":"","Id":27,"Ordinal":3,"Parent":{"$ref":"2"},"ParentId":2,"Title":"Staff News","Uri":"Lists/Staff%20News/AllItems.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"6","Children":[],"Icon":"","Id":28,"Ordinal":4,"Parent":{"$ref":"2"},"ParentId":2,"Title":"Staff Photos","Uri":"SitePages/Staff%20Photos.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"7","Children":[],"Icon":"","Id":29,"Ordinal":5,"Parent":{"$ref":"2"},"ParentId":2,"Title":"Travel","Uri":"Travel/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4}],"Icon":"icon-york","Id":2,"Ordinal":2,"Parent":null,"ParentId":null,"Title":"York Info","Uri":"javascript:;","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":1},{"$id":"8","Children":[{"$id":"9","Children":[],"Icon":"","Id":10,"Ordinal":1,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Client Advisory","Uri":"Client Advisory/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"10","Children":[],"Icon":"","Id":11,"Ordinal":2,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Compliance","Uri":"Compliance/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"11","Children":[],"Icon":"","Id":12,"Ordinal":3,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Corporate Accounting","Uri":"Corporate Accounting/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"12","Children":[],"Icon":"","Id":13,"Ordinal":4,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Reception","Uri":"Reception/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"13","Children":[],"Icon":"","Id":14,"Ordinal":5,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Facilities","Uri":"Facilities/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"14","Children":[],"Icon":"","Id":15,"Ordinal":6,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Fund Accounting","Uri":"Fund Accounting/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"15","Children":[],"Icon":"","Id":16,"Ordinal":7,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Human Resources","Uri":"Human Resources/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"16","Children":[],"Icon":"","Id":17,"Ordinal":8,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Legal","Uri":"Legal/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"17","Children":[],"Icon":"","Id":18,"Ordinal":9,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Operations","Uri":"Operations/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"18","Children":[],"Icon":"","Id":19,"Ordinal":10,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Research","Uri":"Research/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"19","Children":[],"Icon":"","Id":20,"Ordinal":11,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Risk Management","Uri":"Risk/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"20","Children":[],"Icon":"","Id":21,"Ordinal":12,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Tax","Uri":"Tax/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"21","Children":[],"Icon":"","Id":22,"Ordinal":13,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Technology","Uri":"Technology/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"22","Children":[],"Icon":"","Id":23,"Ordinal":14,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Treasury","Uri":"Treasury/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4},{"$id":"23","Children":[],"Icon":"","Id":24,"Ordinal":15,"Parent":{"$ref":"8"},"ParentId":3,"Title":"Valuation","Uri":"Valuation/SitePages/Home.aspx","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":4}],"Icon":"icon-folder-open-alt","Id":3,"Ordinal":3,"Parent":null,"ParentId":null,"Title":"Departments","Uri":"javascript:;","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":1},{"$id":"24","Children":[],"Icon":"icon-bar-chart","Id":4,"Ordinal":4,"Parent":null,"ParentId":null,"Title":"Applications","Uri":"javascript:;","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":1},{"$id":"25","Children":[],"Icon":"icon-user","Id":5,"Ordinal":5,"Parent":null,"ParentId":null,"Title":"Phone List","Uri":"Home/PhoneList","IsAvailableOnPhone":true,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":2},{"$id":"26","Children":[],"Icon":"icon-star","Id":6,"Ordinal":6,"Parent":null,"ParentId":null,"Title":"Favorites","Uri":"javascript:;","IsAvailableOnPhone":false,"IsAvailableOnDesktop":true,"NavigationMenuTypeId":1}]}';
                        }

                        $.fn.YorkNetNav.BindMenu(JSON.parse(localStorage.YorkMenus));
                    }
                }
            });
        } else {
            $.fn.YorkNetNav.BindMenu(JSON.parse(localStorage.YorkMenus));
        }

    }

    function renderMenu(html, options, menu) {
        //html[html.length] = '<li ' + ' id="lnkyorkmenu' + menu.Id + '"' + getMenuClass(options, menu) + '>';
        renderMenuContent(html, options, menu);
    }

    function breakWordInSpans(w) {
        var words = w.split(' ');
        var spans = '';
        for (var i = 0; i < words.length; i++) {
            spans += '<span class="cu">' + words[i].charAt(0) + '</span><span>' + words[i].substring(1, words[i].length) +'</span>';
        }
        return spans;
    }

    function renderMenuContent(html, options, menu) {
        if (menu.Title === 'Home') {
            html[html.length] = '<a href="' + formatUri(menu, Option) + '"' + ((menu.Children.length > 0) ? ' class="dropdown-toggle" data-toggle="dropdown"' : '') + ' style="padding-top:4px" ">';
            html[html.length] = '<img src="/content/images/Kennedy_Lewis_logo_image.png" style = "height :60px; "></i></span>';
        }
        else {
            //html[html.length] = '<a href="' + formatUri(menu, Option) + '"' + ((menu.Children.length > 0) ? ' class="dropdown-toggle"  data-toggle="dropdown"' : '') + '">';
            //html[html.length] = '<span>'+ breakWordInSpans(menu.Title) +'</span>';
        }

        html[html.length] = '</a>';

        if (menu.Children.length > 0)
            renderSubMenu(html, options, menu);
        html[html.length] = '</li>';

    }

    function assignFavoriteMenus(favoritemenu) {
        yorkNavigationMenu.FavoriteMenus = favoritemenu;
    }
    function setupFavoriteLink(options, menus) {
        var favoriteparentmenu = $.grep(menus, function (e) {
            return e.Title === 'Favorites';
        });

        if (favoriteparentmenu.length > 0) {
            assignFavoriteMenus(favoriteparentmenu[0]);
            yorkNavigationMenu.FavoriteMenuId = "#lnkyorkmenu" + favoriteparentmenu[0].Id;
            var checkifcurrentlocationinfavorite = $.grep(favoriteparentmenu[0].Children, function (e) {
                return window.location.toString().indexOf(formatUri(e, options)) >= 0;
            });

            if (favoriteparentmenu[0].Children.length > 0) {
                $(yorkNavigationMenu.FavoriteMenuId).attr('class', 'dropdown');
            } else {
                $(yorkNavigationMenu.FavoriteMenuId).unbind('click');
            }

            if (checkifcurrentlocationinfavorite.length > 0) {
                $('#linkaddremovefavorite').text('Remove Page from Favorites');
            } else {
                $('#linkaddremovefavorite').text('Add Page to Favorites');
            }
        }
    }

    function getActiveMenuTitle(menus, options) {

        var activemenu = yorkNavigationMenu.options.activeMenuItem;
        var currentlocation = decodeURI(window.location.toString().toLowerCase());



        for (var i = 0; i < menus.length; i++) {
            if (menus[i].Title != 'Favorites') {

                if (currentlocation.indexOf(decodeURI(formatUri(menus[i], options)).toLowerCase()) >= 0) {
                    activemenu = menus[i].Title;
                }

                for (var j = 0; j < menus[i].Children.length; j++) {

                    if (currentlocation.indexOf(decodeURI(formatUri(menus[i].Children[j], options)).toLowerCase()) >= 0) {
                        activemenu = menus[i].Title;
                    }
                }
            }
        }

        return activemenu;
    }

    function getMenuClass(options, menu) {
        var retval = '';
        var classes = [];

        if (options.activeMenuItem === menu.Title) {
            classes[classes.length] = 'active';
        }
        else {
            if (menu.Title !== 'Favorites' && menu.Children !== undefined && menu.Children !== null && menu.Children.length > 0) {
                var result = $.grep(menu.Children, function (e) { return e.Title == options.activeMenuItem; });
                if (result.length > 0) {
                    classes[classes.length] = 'active';
                }
            }
        }

        if (menu.Children.length > 0)
            classes[classes.length] = 'dropdown';

        if (classes.length > 0)
            retval = ' class="' + classes.join(' ') + '"';

        return retval;
    }

    function renderSubMenu(html, options, menu) {
        html[html.length] = '<ul class="dropdown-menu">';
        for (var i = 0; i < menu.Children.length; i++) {
            var subMenu = menu.Children[i];
            html[html.length] = '<li' + ((subMenu.Children.length > 0) ? ' class="dropdown-submenu"' : '') + '>';
            html[html.length] = '<a href="' + formatUri(subMenu, options) + '">' + subMenu.Title + '</a>';
            if (subMenu.Children.length > 0) renderSubMenu2(html, options, subMenu);
            html[html.length] = '</li>';
        }
        html[html.length] = '</ul>';
    }

    function renderSubMenu2(html, options, menu) {
        html[html.length] = '<ul class="dropdown-menu">';
        for (var i = 0; i < menu.Children.length; i++)
            html[html.length] = '<li><a href="' + formatUri(menu.Children[i], options) + '">' + menu.Children[i].Title + '</a></li>';
        html[html.length] = '</ul>';
    }

    function formatUri(menu, options) {
        if (menu.NavigationMenuTypeId === 1) {
            return "javascript:;";
        }

        var result = $.grep(yorkNavigationMenu.NavigationMenuBasePaths, function (e) { return e.Id == menu.NavigationMenuTypeId; });
        if (result[0].Id === 5) {
            return menu.Uri;
        } else {
            return (result[0].BasePath === null ? '' : result[0].BasePath) + '/' + menu.Uri;
        }
    }


})(jQuery);

$(document).on('menuHeightChangedEvent', function (e, eventInfo) {
    subscribers = $('.subscribers-menuHeightChanged');
    subscribers.trigger('menuHeightChangedEventHandler', [eventInfo]);
});
