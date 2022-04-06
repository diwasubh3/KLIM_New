var Application;
(function (Application) {
    var Services;
    (function (Services) {
        var DataService = (function () {
            function DataService(httpWrapperFactory) {
                var _this = this;
                this.userIsAnAdmin = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/userIsAnAdmin');
                };
                this.userIsASuperUser = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/userIsASuperUser');
                };
                this.deleteCustomView = function (view) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/deleteCustomView', view);
                };
                this.saveCustomView = function (view) {
                    //return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/savefund', fund);
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/saveCustomView', view);
                };
                this.getAnalystResearchDetails = function (headerId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getanalystresearchdetails?headerId=' + headerId);
                };
                this.downloadSummaries = function () {
                    var hurl = pageOptions.appBasePath + 'data/downloadSummaries';
                    window.open(hurl);
                };
                this.downloadLoanPositions = function (fundId) {
                    var hurl = pageOptions.appBasePath + 'data/downloadloanpositionsfile?fundId=' + fundId;
                    window.open(hurl);
                };
                this.downloadReInvestCash = function (Url) {
                    var durl = pageOptions.appBasePath + 'data/downloadReInvestCash?filePath=' + Url;
                    window.open(durl);
                };
                this.loadData = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/api/user');
                };
                this.getAnalystResearchIssuerIds = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getanalystresearchissuerids');
                };
                this.getCustomViews = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getcustomviews');
                };
                this.getPerson = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/GetPerson');
                };
                this.getCustomView = function (viewId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getcustomview?viewId=' + viewId);
                };
                this.viewNameIsTaken = function (viewName) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/viewnameistaken?viewName=' + viewName);
                };
                this.updateData = function (userModel) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/api/user', userModel);
                };
                this.getAnalystResearchHeader = function (issuerId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getanalystresearchheader?issuerId=' + issuerId);
                };
                this.loadSummaryData = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getsummaries');
                };
                this.getFieldsForCustomView = function (viewId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getFieldsForCustomView?viewId=' + viewId);
                };
                this.loadFixedFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/GetFixedFields');
                };
                this.loadParameterValues = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/parameterdata/GetParameterValues');
                };
                this.loadParameterValuesForParameterType = function (parameterType) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/parameterdata/GetParameterValuesForParameterType?parameterTypeName=' + parameterType);
                };
                this.loadParameterTypes = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + 'parameterdata/GetParameterTypes');
                };
                this.loadRules = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getrules');
                };
                this.loadPositionViewFieldGroups = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getpositionviewfieldgroups');
                };
                this.getCustomPositionViewFieldGroups = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getCustomPositionViewFieldGroups');
                };
                this.getAllFieldGroups = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getfieldgroups');
                };
                this.getAllCustomViewFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getallcustomviewfields');
                };
                this.loadFundRestrictionFieldGroup = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getpositionviewfieldgroups?fundgroupname=');
                };
                this.loadTop10Bottom10 = function (fund, ruleId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/positiondata/GetTopBottomPositions?fundCode=' + (fund && fund.fundCode ? fund.fundCode : "CLO-1") + "&ruleId=" + ruleId);
                };
                this.loadPositions = function (fund, onlyWithExposures) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/positiondata/getallpositions?fundCode=' + fund.fundCode + (onlyWithExposures ? '&onlyWithExposures=true' : ''));
                };
                this.loadFundRestrictionFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getfundrestrictionfields');
                };
                this.loadFundRestrictionsTypes = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fundrestrictiondata/getfundrestrictionstypes');
                };
                this.loadFundRestrictions = function (fundId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fundrestrictiondata/getfundrestrictions' + (fundId ? '?fundId=' + fundId : ''));
                };
                this.loadOperators = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/GetOperators');
                };
                this.saveFundRestrictions = function (fundRestrictions) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/fundrestrictiondata/SaveFundRestrictions', fundRestrictions);
                };
                this.updateWatch = function (watch, fundCode) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/watchdata/savewatch', { watch: watch, fundCode: fundCode });
                };
                this.updateParameterValue = function (parameterValue) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/parameterdata/saveparametervalue', parameterValue);
                };
                this.deleteWatch = function (watch) {
                    return _this.httpWrapperFactory.deleteData(pageOptions.appBasePath + '/watchdata/deletewatch?watchId=' + watch.watchId);
                };
                this.getSecurityOverrides = function (loadType, securityId) {
                    var url = pageOptions.appBasePath + '/securitydata/getsecurityoverrides?';
                    if (loadType != null) {
                        url += 'loadType=' + loadType + (securityId ? '&' : '');
                    }
                    if (securityId) {
                        url += 'securityId=' + securityId;
                    }
                    return _this.httpWrapperFactory.getData(url);
                };
                this.getGroupedSecurityOverrides = function (loadType, securityId) {
                    var url = pageOptions.appBasePath + '/securitydata/getgroupedsecurityoverrides?';
                    if (loadType != null) {
                        url += 'loadType=' + loadType + (securityId ? '&' : '');
                    }
                    if (securityId) {
                        url += 'securityId=' + securityId;
                    }
                    return _this.httpWrapperFactory.getData(url);
                };
                this.getSecurityOverrideHeaderFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getsecurityoverrideheaderfields');
                };
                this.getSaveSecurityOverrideHeaderFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getsavesecurityoverrideheaderfields');
                };
                this.getSecurityOverrideableFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getsecurityoverrideablefields');
                };
                this.getSecurities = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getsecurities');
                };
                this.getCurrentSecurities = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getcurrentsecurities');
                };
                this.saveSecurityOverides = function (securityOverrides) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/SaveSecurityOverrides', securityOverrides);
                };
                this.getAnalystResearches = function (loadType, issuerId) {
                    var url = pageOptions.appBasePath + '/analystdata/getanalystrefresh?';
                    if (loadType != null) {
                        url += 'loadType=' + loadType + (issuerId ? '&' : '');
                    }
                    if (issuerId) {
                        url += 'issuerId=' + issuerId;
                    }
                    return _this.httpWrapperFactory.getData(url);
                };
                this.getAnalystResearchHeaderFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getanalystresearchheaderfields');
                };
                this.getIssuers = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getissuers');
                };
                this.getAnalysts = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/analystdata/getanalysts');
                };
                this.saveAnalystResearches = function (analystResearches) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/analystdata/SaveAnalystResearches', analystResearches);
                };
                this.getBidOfferHeaderFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getbidofferheaderfields');
                };
                this.savePricings = function (pricings) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/bidofferdata/saveprices', pricings);
                };
                this.getSecurityReconHeaderFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getnewsecurityreconheaderfields');
                };
                this.getSecuritiesForRecon = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getsecuritiesforrecon');
                };
                this.reconcileSecurities = function (securities) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/reconcilesecurities', securities);
                };
                this.getLoanAttributeOverrideReconHeaderFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getloanattributeoverridereconheaderfields');
                };
                this.getLoanAttributeOverrides = function (securityId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getloanattributesoverridesforrecon' + (securityId ? '?securityId=' + securityId : ''));
                };
                this.endSecurityOverride = function (loanAttributeOverride) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/endsecurityoverride', loanAttributeOverride);
                };
                this.resetSecurityOverrideConflict = function (loanAttributeOverride) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/resetsecurityoverrideconflict', loanAttributeOverride);
                };
                this.getTradeAllocations = function (securityId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/gettradeallocations' + ('?securityId=' + securityId));
                };
                this.saveTrades = function (trade, processSaveTrade) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradedata/savetrade', { trade: trade, processSavedTrade: processSaveTrade });
                };
                this.getPositions = function (securityCode, fundCode) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/positiondata/getallpositionsbasedonsecuritycodeandfundcode' + (securityCode ? '?securityCode=' + securityCode : '') + (fundCode ? '&fundCode=' + fundCode : ''));
                };
                this.getPositionsForSecurities = function (securityIds, fundCode) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/positiondata/getallpositionsforsecurities', { securityIds: securityIds, fundCode: fundCode });
                };
                this.getTradesHeaderFields = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/gettradesheaderfields');
                };
                this.getTrades = function (includeCancelled, fundCode) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/gettrades?includeCancelled=' + includeCancelled + '&fundCode=' + fundCode);
                };
                this.getTradeSourceData = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/getsourcedata');
                };
                this.getTradeBookingData = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/getsourcedata');
                };
                this.generateTradeXML = function (data) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradebookingdata/GenerateTradeXML', data);
                };
                this.getFundAllocations = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/GetFundAllocations');
                };
                this.getIssuerSecurities = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/GetIssuerSecurities');
                };
                this.getBloombergData = function (securityCode) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/getbloombergsecurityinfo?securityCode=' + securityCode);
                };
                this.saveTempSecurity = function (tempsecurity) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradedata/SaveSecurity', tempsecurity);
                };
                this.getFunds = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getfunds');
                };
                this.getTradeSnapshots = function (fundid, tradeswapid, groupby) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + 'tradeswapdata/GetTradeSwaps?fundid=' + fundid + '&tradeswapid=' + tradeswapid + '&groupby=' + groupby);
                };
                this.saveFund = function (fund) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/savefund', fund);
                };
                this.getLastTradeSwap = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradeswapdata/getlastsavedtradeswap');
                };
                this.startTradeSwap = function (tradeSwapParam) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradeswapdata/starttradeswap', tradeSwapParam);
                };
                this.process = function (url) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + url);
                };
                this.getMoodyRatings = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getmoodyratings');
                };
                this.getRatings = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getratings');
                };
                this.getFilterFieldGroups = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + 'fielddata/GetFilterFields');
                };
                this.updateSecurity = function (security) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/updateBbgId', { dto: security });
                };
                this.updateIsPrivate = function (issuer) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/updateIsPrivate', { dto: issuer });
                };
                this.saveReportingData = function (data) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/savereportingdata', data);
                };
                this.getMajors = function (fundId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/majors?fundid=' + fundId);
                };
                this.getMinorsByDiversity = function (fundId, fromDiversity, toDiversity) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/diversityminors?fundid=' + fundId + '&fromDiversity=' + fromDiversity + '&toDiversity=' + toDiversity);
                };
                this.getMinorsBySpread = function (fundId, fromSpread, toSpread) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/spreadminors?fundid=' + fundId + '&fromSpread=' + fromSpread + '&toSpread=' + toSpread);
                };
                this.getMatrixPoints = function (fundId) {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/matrixpoints?fundid=' + fundId);
                };
                this.addMatrixPoint = function (matrixData) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/matrixdata/addmatrixpoint', matrixData);
                };
                this.getReportingData = function () {
                    return _this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getreportingdata');
                };
                this.getTradeHistory = function (securityCode) {
                    var url = pageOptions.appBasePath + '/tradedata/GetTradeHistory?' + 'securityCode=' + securityCode;
                    return _this.httpWrapperFactory.getData(url);
                };
                this.updatePaydown = function (paydown, fundCode) {
                    return _this.httpWrapperFactory.postData(pageOptions.appBasePath + '/paydowndata/savepaydown', { paydown: paydown, fundCode: fundCode });
                };
                this.deletePaydown = function (paydown) {
                    return _this.httpWrapperFactory.deleteData(pageOptions.appBasePath + '/paydowndata/deletepaydown?paydownId=' + paydown.paydownId);
                };
                this.httpWrapperFactory = httpWrapperFactory;
            }
            return DataService;
        }());
        DataService.$inject = ["application.factories.httpWrapperFactory"];
        Services.DataService = DataService;
        angular.module("app").service("application.services.dataService", DataService);
    })(Services = Application.Services || (Application.Services = {}));
})(Application || (Application = {}));
//# sourceMappingURL=DataService.js.map