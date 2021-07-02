angular.module("app").factory('exportUiGridService', ['uiGridExporterService', exportUiGridService]);
function exportUiGridService(uiGridExporterService) {

    var service = {
        exportToExcel: exportToExcel
    };

    return service;

    function Workbook() {
        if (!(this instanceof Workbook)) return new Workbook();
        this.SheetNames = [];
        this.Sheets = {};
    }

    function exportToExcel(sheetName, gridApi, rowTypes, colTypes, positionFilter) {

        console.log(gridApi.grid);

        var columns = gridApi.grid.options.showHeader
            ? uiGridExporterService.getColumnHeaders(gridApi.grid, colTypes)
            : [];

        console.log(columns);

        var data = uiGridExporterService.getData(gridApi.grid, rowTypes, colTypes);


        var fileName = gridApi.grid.options.exporterExcelFilename
            ? gridApi.grid.options.exporterExcelFilename
            : 'dokuman';

        fileName += '.xlsx';

        var wb = new Workbook(), ws = sheetFromArrayUiGrid(data, columns);

        wb.SheetNames.push(sheetName);       
        wb.Sheets[sheetName] = ws;
        
        if (gridApi.grid.columns.some(c => c.filters[0].term)) {
            wb.SheetNames.push("Filters");
            wb.Sheets["Filters"] = getFilterWs(gridApi, positionFilter);
        }


        var wbout = XLSX.write(wb, {
            bookType: 'xlsx',
            bookSST: true,
            type: 'binary'
        });

        saveAs(new Blob([s2ab(wbout)], {
            type: 'application/octet-stream'
        }), fileName);

    }
    
    function getFilterWs(gridApi, positionFilter) {

        let filterData = gridApi.grid.columns
            .filter(c => c.filters[0].term && c.displayName != 'Filtered')
            .map(c => [{ value: c.displayName }, { value: c.filters[0].term }])
            .concat(getTopLevelFilterData(positionFilter));

        return sheetFromArrayUiGrid(filterData, [{ name: 'Field' }, { name: 'FilterTerm' }]);
    }
    
    function sheetFromArrayUiGrid(data, columns) {
        var ws = {};
        var range = {
            s: {
                c: 10000000,
                r: 10000000
            },
            e: {
                c: 0,
                r: 0
            }
        };
        var C = 0;
        columns.forEach(function (c) {
            var v = c.displayName || c.value || c.name;
            addCell(range, v, 0, C, ws);
            C++;
        }, this);
        var R = 1;
        data.forEach(function (ds) {
            C = 0;
            ds.forEach(function (d) {
                var v = d.value;
                addCell(range, v, R, C, ws);
                C++;
            });
            R++;
        }, this);
        if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
        return ws;
    }
    /**
     * 
     * @param {*} data 
     * @param {*} columns 
     */

    function datenum(v, date1904) {
        if (date1904) v += 1462;
        var epoch = Date.parse(v);
        return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
    }

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    function addCell(range, value, row, col, ws) {
        if (range.s.r > row) range.s.r = row;
        if (range.s.c > col) range.s.c = col;
        if (range.e.r < row) range.e.r = row;
        if (range.e.c < col) range.e.c = col;
        var cell = {
            v: value
        };
        if (cell.v == null) cell.v = '-';
        var cell_ref = XLSX.utils.encode_cell({
            c: col,
            r: row
        });

        if (typeof cell.v === 'number') cell.t = 'n';
        else if (typeof cell.v === 'boolean') cell.t = 'b';
        else if (cell.v instanceof Date) {
            cell.t = 'n';
            cell.z = XLSX.SSF._table[14];
            cell.v = datenum(cell.v);
        } else cell.t = 's';

        ws[cell_ref] = cell;
    }

    function getTopLevelFilterData(positionFilter) {

        if (!(positionFilter.analyst
            && positionFilter.fixedfields
            && positionFilter.ratings
            && positionFilter.fixedfields))
            return [[]];

        return positionFilter.analyst
            .concat(positionFilter.fixedfields)
            .concat(positionFilter.ratings)
            .concat(positionFilter.security)
            .filter(f => f.upperBound.value || f.lowerBound.value)
            .map(getFilterInfo)
            .map(i => [{ value: i.field.fieldName }, { value: i.filterDesc }]);
    }

    function getFilterInfo(filter) {

        let getValue = value => filter.sourceCollection && value
            ? filter.sourceCollection.filter(i => i.value === value)[0].label
            : value;

        let lowerBoundDesc = filter.lowerBound.value ?
            `Lowerbound Operator: '${filter.lowerBound.operator.operatorCode}', ` +
            `Lowerbound Value: '${getValue(filter.lowerBound.value)}'`             
            : '';

        let upperBoundDesc = filter.upperBound.value ?
            `Upperbound Operator: '${filter.upperBound.operator.operatorCode}', `+
            `Upperbound Value: '${getValue(filter.upperBound.value)}'`            
            : '';
        
        return {
            field: filter.field,
            filterDesc: [lowerBoundDesc, upperBoundDesc].filter(d => d).join(', ')
            
        };
    }

}