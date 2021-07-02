
import { Component, OnInit, NgModule } from '@angular/core';

import { ISummary } from './models/summary';
import { CLOService } from './services/closervice';

@Component({
    selector: 'pm-app',
    templateUrl:'app/index.html' 
})
export class AppComponent implements OnInit {

    public constructor(private _cloService: CLOService) {

    }

    public rows: Array<any> = [];

    public columns: Array<any> = [
        { title: 'CLO NAME',            name: 'fundCode'       , sort:false  ,className: ''},
        { title: 'PAR',                 name: 'par'            , sort:false  ,className: ['text-right']},
        { title: 'SPREAD',              name: 'spread'         , sort:false  ,className: 'text-right'},
        { title: 'TOTAL COUPON',        name: 'totalCoupon'    , sort:false  ,className: 'text-right'},
        { title: 'WARF',                name: 'warf'           , sort:false  ,className: 'text-right'},
        { title: 'MOODY\'S RECOVERY',   name: 'moodyRecovery'  , sort:false  ,className: 'text-right'},
        { title: 'WA BID',              name: 'bid'            , sort:false  ,className: 'text-right'},
        { title: 'PRICIPAL CASH',       name: 'principalCash'  , sort: false ,className: 'text-right'},
    ];

    public config: any = {
        paging: true,
        sorting: { columns: this.columns },
        filtering: { filterString: '' },
        className: ['table-bordered']
    };

    summaries: ISummary[];
    errorMessage: string;
    public ngOnInit(): void {
        var vm = this;
        this._cloService.getSummaries().subscribe(summaries => {
        this.summaries = summaries;
            vm.onChangeTable(vm.config);
        }, error => vm.errorMessage = <any>error);
        
    }
    public changeFilter(data: any, config: any): any {

        let filteredData: Array<any> = data;
        this.columns.forEach((column: any) => {
            if (column.filtering) {
                filteredData = filteredData.filter((item: any) => {
                    return item[column.name].match(column.filtering.filterString);
                });
            }
        });

        if (!config.filtering) {
            return filteredData;
        }

        if (config.filtering.columnName) {
            return filteredData.filter((item: any) =>
                item[config.filtering.columnName].match(this.config.filtering.filterString));
        }

        let tempArray: Array<any> = [];
        filteredData.forEach((item: any) => {
            let flag = false;

            this.columns.forEach((column: any) => {
                if (item[column.name] && this.config.filtering.filterString &&
                    (!item[column.name].toString().match(this.config.filtering.filterString))) {
                    flag = false;
                }
                else    
                {
                    flag = true;
                }
            });
            if (flag) {
                tempArray.push(item);
            }
        });
        filteredData = tempArray;

        return filteredData;
    }

    public changeSort(data: any, config: any): any {
        if (!config.sorting) {
            return data;
        }

        let columns = this.config.sorting.columns || [];
        let columnName: string = void 0;
        let sort: string = void 0;

        for (let i = 0; i < columns.length; i++) {
            if (columns[i].sort !== '' && columns[i].sort !== false) {
                columnName = columns[i].name;
                sort = columns[i].sort;
            }
        }

        if (!columnName) {
            return data;
        }

        // simple sorting
        return data.sort((previous: any, current: any) => {
            if (previous[columnName] > current[columnName]) {
                return sort === 'desc' ? -1 : 1;
            } else if (previous[columnName] < current[columnName]) {
                return sort === 'asc' ? -1 : 1;
            }
            return 0;
        });
    }

    public onChangeTable(config: any): any {

        if (config.filtering) {
            Object.assign(this.config.filtering, config.filtering);
        }

        if (config.sorting) {
            Object.assign(this.config.sorting, config.sorting);
        }

        let filteredData = this.changeFilter(this.summaries, this.config);
        let sortedData = this.changeSort(filteredData, this.config);
        this.rows = sortedData;

    }

    pageTitle: string = 'Home';
}
