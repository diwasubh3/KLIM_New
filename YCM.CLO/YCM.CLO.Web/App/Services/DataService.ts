module Application.Services {
    export class DataService implements Contracts.IDataService {
        private httpWrapperFactory: Application.Factories.HttpWrapperFactory;
        static $inject = ["application.factories.httpWrapperFactory"];
        constructor(httpWrapperFactory: Application.Factories.HttpWrapperFactory) {
            this.httpWrapperFactory = httpWrapperFactory;
        }

	    userIsAnAdmin = () => {
			return <ng.IPromise<boolean>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/userIsAnAdmin');
	    }

	    userIsASuperUser = () => {
			return <ng.IPromise<boolean>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/userIsASuperUser');
		}

		deleteCustomView = (view: Models.ICustomView) => {
			return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/deleteCustomView', view);
		}

		saveCustomView = (view: Models.ICustomView) => {
			//return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/savefund', fund);
			return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/saveCustomView', view);
		}

	    getAnalystResearchDetails = (headerId: number) => {
			return <ng.IPromise<Array<Application.Models.IAnalystResearchDetail>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getanalystresearchdetails?headerId=' + headerId);
		}

	    downloadSummaries = () => {
			var hurl = pageOptions.appBasePath + 'data/downloadSummaries';
			window.open(hurl);
		}

	    downloadLoanPositions = (fundId: number) => {
			var hurl = pageOptions.appBasePath + 'data/downloadloanpositionsfile?fundId=' + fundId;
			window.open(hurl);
        }

        downloadReInvestCash = (Url: string) => {
            var durl = pageOptions.appBasePath + 'data/downloadReInvestCash?filePath=' + Url;
            window.open(durl);
        }

        loadData = () => {
            return <ng.IPromise<Array<Application.Models.UserModel>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/api/user');
        }

	    getAnalystResearchIssuerIds = () => {
		    return <ng.IPromise<Array<number>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getanalystresearchissuerids');
		}

	    getCustomViews = () => {
			return <ng.IPromise<Array<Models.ICustomView>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getcustomviews');
        }

        getPerson = () => {
            return <ng.IPromise<Array<Models.ICustomView>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/GetPerson');
        }

		getCustomView = (viewId: number) => {
			return <ng.IPromise<Models.ICustomView>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getcustomview?viewId=' + viewId);
		}

	    viewNameIsTaken = (viewName: string) => {
			return <ng.IPromise<boolean>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/viewnameistaken?viewName=' + viewName);
		}

	    updateData = (userModel: Application.Models.UserModel) => {
			return <ng.IPromise<boolean>>this.httpWrapperFactory.postData(pageOptions.appBasePath + '/api/user', userModel);
        }

	    getAnalystResearchHeader = (issuerId: number) => {
			return <ng.IPromise<Models.IAnalystResearchHeader>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getanalystresearchheader?issuerId=' + issuerId);
	    }

        loadSummaryData = () => {
            return <ng.IPromise<Array<Application.Models.ISummary>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getsummaries');
        }

		getFieldsForCustomView = (viewId: number) => {
			return <ng.IPromise<Array<Application.Models.IField>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getFieldsForCustomView?viewId=' + viewId);
		}

        loadFixedFields = () => {
            return <ng.IPromise<Array<Application.Models.IField>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/GetFixedFields');
        }

        loadParameterValues = () => {
            return <ng.IPromise<Array<Application.Models.IParameterValue>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/parameterdata/GetParameterValues');
        }

        loadParameterValuesForParameterType = (parameterType: string) => {
            return <ng.IPromise<Array<Application.Models.IParameterValue>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/parameterdata/GetParameterValuesForParameterType?parameterTypeName='+parameterType);
        }

        loadParameterTypes = () => {
            return <ng.IPromise<Array<Application.Models.IParameterType>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + 'parameterdata/GetParameterTypes');
        }

        loadRules = () => {
            return <ng.IPromise<Array<Application.Models.IRule>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getrules');
        }

        loadPositionViewFieldGroups = () => {
            return <ng.IPromise<Array<Application.Models.IFieldGroup>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getpositionviewfieldgroups');
        }

	    getCustomPositionViewFieldGroups = () => {
			return <ng.IPromise<Array<Application.Models.IFieldGroup>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getCustomPositionViewFieldGroups');
		}

	    getAllFieldGroups = () => {
			return <ng.IPromise<Array<Application.Models.IFieldGroup>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getfieldgroups');
		}

	    getAllCustomViewFields = () => {
			return <ng.IPromise<Array<Application.Models.ICustomViewField>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getallcustomviewfields');
		}

        loadFundRestrictionFieldGroup = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getpositionviewfieldgroups?fundgroupname=');
        }

        loadTop10Bottom10 = (fund:Models.ISummary,ruleId:number) => {
            return <ng.IPromise<Models.ITop10Bottom10Positions>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/positiondata/GetTopBottomPositions?fundCode=' + (fund && fund.fundCode ? fund.fundCode: "CLO-1") +"&ruleId="+ruleId);
        }

        loadPositions = (fund: Models.ISummary, onlyWithExposures: boolean) => {
            return <ng.IPromise<Array<Models.IPosition>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/positiondata/getallpositions?fundCode=' + fund.fundCode + (onlyWithExposures ? '&onlyWithExposures=true' : ''));
        }

        loadFundRestrictionFields = () => {
            return <ng.IPromise<Array<Models.IField>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getfundrestrictionfields');
        }

        loadFundRestrictionsTypes = () => {
            return <ng.IPromise<Array<Models.IFundRestrictionsTypes>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fundrestrictiondata/getfundrestrictionstypes');
        }

        loadFundRestrictions = (fundId:number) => {
            return <ng.IPromise<Array<Models.IFundRestriction>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fundrestrictiondata/getfundrestrictions' + (fundId ?'?fundId='+fundId: ''));
        }

        loadOperators = () => {
            return <ng.IPromise<Array<Models.IOperator>>>this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/GetOperators');
        }

        saveFundRestrictions = (fundRestrictions: Array<Models.IFundRestriction>) => {
            return <ng.IPromise<Array<Models.IFundRestriction>>>this.httpWrapperFactory.postData(pageOptions.appBasePath + '/fundrestrictiondata/SaveFundRestrictions', fundRestrictions);
        }

		updateWatch = (watch: Models.IWatch, fundCode: string) => {
			return <ng.IPromise<Models.IWatch>>this.httpWrapperFactory.postData(pageOptions.appBasePath + '/watchdata/savewatch', { watch: watch, fundCode: fundCode });
        }

        updateParameterValue = (parameterValue: Models.IParameterValue) => {
            return <ng.IPromise<Models.IParameterValue>>this.httpWrapperFactory.postData(pageOptions.appBasePath + '/parameterdata/saveparametervalue', parameterValue);
        }

        deleteWatch = (watch: Models.IWatch) => {
            return this.httpWrapperFactory.deleteData(pageOptions.appBasePath + '/watchdata/deletewatch?watchId=' +  watch.watchId);
        }
        
        getSecurityOverrides = (loadType: number, securityId: number) => {
            var url = pageOptions.appBasePath + '/securitydata/getsecurityoverrides?';
            if (loadType != null) {
                url += 'loadType=' + loadType + (securityId? '&':'');
            }
            if (securityId) {
                url += 'securityId=' + securityId;
            }

            return this.httpWrapperFactory.getData(url);
        }

        getGroupedSecurityOverrides = (loadType: number, securityId: number) => {
            var url = pageOptions.appBasePath + '/securitydata/getgroupedsecurityoverrides?';
            if (loadType != null) {
                url += 'loadType=' + loadType + (securityId ? '&' : '');
            }
            if (securityId) {
                url += 'securityId=' + securityId;
            }

            return this.httpWrapperFactory.getData(url);
        }

        getSecurityOverrideHeaderFields = () =>{
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getsecurityoverrideheaderfields');
        }

        getSaveSecurityOverrideHeaderFields = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getsavesecurityoverrideheaderfields');
        }

        getSecurityOverrideableFields = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getsecurityoverrideablefields');
        }

        getSecurities = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getsecurities');
        }

        getCurrentSecurities = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getcurrentsecurities');
        }

        saveSecurityOverides = (securityOverrides: Array<Models.ISecurityOverride>) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/SaveSecurityOverrides', securityOverrides);
        }

        getAnalystResearches = (loadType: number,issuerId:number) => {
            var url = pageOptions.appBasePath + '/analystdata/getanalystrefresh?';
            if (loadType != null) {
                url += 'loadType=' + loadType + (issuerId ? '&' : '');
            }
            if (issuerId) {
                url += 'issuerId=' + issuerId;
            }
            return this.httpWrapperFactory.getData(url);
        }

        getAnalystResearchHeaderFields= () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getanalystresearchheaderfields');
        };

        getIssuers = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getissuers');
        }

        getAnalysts= () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/analystdata/getanalysts');
        }

        saveAnalystResearches = (analystResearches: Array<Models.IAnalystResearch>) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/analystdata/SaveAnalystResearches', analystResearches);
        }

        getBidOfferHeaderFields = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getbidofferheaderfields');
        }

        savePricings = (pricings: Array<Models.IPricing>) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/bidofferdata/saveprices', pricings);
        }

        getSecurityReconHeaderFields = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getnewsecurityreconheaderfields');
        }

        getSecuritiesForRecon = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getsecuritiesforrecon');
        }

        reconcileSecurities = (securities: Array<Models.ISecurityRecon>) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/reconcilesecurities', securities);
        }

        getLoanAttributeOverrideReconHeaderFields = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/getloanattributeoverridereconheaderfields');
        }

        getLoanAttributeOverrides = (securityId: number) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/securitydata/getloanattributesoverridesforrecon' + (securityId?'?securityId='+securityId:''));
        }

        endSecurityOverride = (loanAttributeOverride:Models.ILoanAttributeDto) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/endsecurityoverride', loanAttributeOverride);
        }

        resetSecurityOverrideConflict = (loanAttributeOverride: Models.ILoanAttributeDto) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/resetsecurityoverrideconflict', loanAttributeOverride);
        }

        getTradeAllocations = (securityId: number) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/gettradeallocations' + ('?securityId=' + securityId ));
        }

        saveTrades = (trade: Models.ITrade, processSaveTrade: boolean) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradedata/savetrade', { trade: trade, processSavedTrade: processSaveTrade });
        };
        

        getPositions = (securityCode: string, fundCode: string) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/positiondata/getallpositionsbasedonsecuritycodeandfundcode' + (securityCode ? '?securityCode=' + securityCode : '') + (fundCode?'&fundCode='+fundCode:''));
        }

        getPositionsForSecurities = (securityIds: Array<number>, fundCode: string) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/positiondata/getallpositionsforsecurities', { securityIds: securityIds, fundCode: fundCode} );
        }

        getTradesHeaderFields = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/fielddata/gettradesheaderfields');
        }

        getTrades = (includeCancelled: boolean, fundCode:string ) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/gettrades?includeCancelled=' + includeCancelled + '&fundCode=' + fundCode);
        }

        getTradeSourceData = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/getsourcedata');
        }

        getTradeBookingData = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/getsourcedata');
        }

        generateTradeXML = (data: Models.ITradeBooking) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradebookingdata/GenerateTradeXML', data);
        }

        getIssuerSecurities = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/GetIssuerSecurities');
        }

        getTradeBooking = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/GetTradeBooking');
        }

        getTradeFundAllocation = (totalQty: number, ruleName: string) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/GetTradeFundAllocation?totalQty=' + totalQty + '&ruleName=' + ruleName);
        }

        getCalculatedData = (data: Array<Models.ITradeBookingDetail>) => { //, totalQty: number, ruleName: string
            /*return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradebookingdata/getCalculatedData?data=' + JSON.stringify(data) + '&totalQty=' + totalQty + '&ruleName=' + ruleName);*/
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradebookingdata/getCalculatedData',data);
        }

        getBloombergData = (securityCode: string) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradedata/getbloombergsecurityinfo?securityCode='+securityCode);
        }

        saveTempSecurity = (tempsecurity: Models.ITempSecurity) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradedata/SaveSecurity', tempsecurity);
        }

        getFunds = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getfunds');
        }

        getTradeSnapshots = (fundid: number, tradeswapid: number, groupby: number) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + 'tradeswapdata/GetTradeSwaps?fundid=' + fundid + '&tradeswapid=' + tradeswapid +'&groupby='+groupby);
        }

        saveFund = (fund: Models.IFund) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/savefund', fund);
        }

        getLastTradeSwap= () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/tradeswapdata/getlastsavedtradeswap');
        }

        startTradeSwap = (tradeSwapParam: Models.ITradeSwapParam) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/tradeswapdata/starttradeswap', tradeSwapParam);
        }

        process = (url) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath+url);
        }

        getMoodyRatings = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getmoodyratings');
        }

        getRatings = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getratings');
        }

        getFilterFieldGroups = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + 'fielddata/GetFilterFields');
		}

	    updateSecurity = (security: Models.ISecurity) => {
			return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/updateBbgId', { dto: security });
		};

	    updateIsPrivate = (issuer: Models.IIssuer) => {
			return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/securitydata/updateIsPrivate', { dto: issuer });
        };


        saveReportingData = (data: Models.ReportingData) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/data/savereportingdata', data);
        }


        getMajors = (fundId:number) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/majors?fundid='+fundId);
        }

        getMinorsByDiversity= (fundId: number, fromDiversity: number, toDiversity: number) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/diversityminors?fundid=' + fundId + '&fromDiversity=' + fromDiversity + '&toDiversity=' + toDiversity);
        }

        getMinorsBySpread = (fundId: number, fromSpread: number, toSpread: number) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/spreadminors?fundid=' + fundId + '&fromSpread=' + fromSpread + '&toSpread=' + toSpread);
        }

        getMatrixPoints= (fundId: number) => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + 'matrixdata/matrixpoints?fundid=' + fundId);
        }


        addMatrixPoint = (matrixData: Models.IMatrixData) => {
            return this.httpWrapperFactory.postData(pageOptions.appBasePath + '/matrixdata/addmatrixpoint', matrixData);
        };


        getReportingData = () => {
            return this.httpWrapperFactory.getData(pageOptions.appBasePath + '/data/getreportingdata');
        }

        getTradeHistory = (securityCode: string) => {
            var url = pageOptions.appBasePath + '/tradedata/GetTradeHistory?' + 'securityCode=' + securityCode;
            return this.httpWrapperFactory.getData(url);
        }

        updatePaydown = (paydown: Models.IPaydown, fundCode: string) => {
            return <ng.IPromise<Models.IPaydown>>this.httpWrapperFactory.postData(pageOptions.appBasePath + '/paydowndata/savepaydown', { paydown: paydown, fundCode: fundCode });
        }

        deletePaydown = (paydown: Models.IPaydown) => {
            return this.httpWrapperFactory.deleteData(pageOptions.appBasePath + '/paydowndata/deletepaydown?paydownId=' + paydown.paydownId);
        }


    }

    angular.module("app").service("application.services.dataService", DataService); 
}
