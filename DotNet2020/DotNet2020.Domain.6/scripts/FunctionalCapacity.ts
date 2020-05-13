﻿function getCurrentMonthName(currentMonthNumber:number) {
    //var currentMonthNumber = new Date().getMonth();
    //console.log("текущий месяц " + currentMonthNumber);

    switch (currentMonthNumber) {
        case 0:
            return "январь";
        case 1:
            return "февраль";
        case 2:
            return "март";
        case 3:
            return "апрель";
        case 4:
            return "май";
        case 5:
            return "июнь";
        case 6:
            return "июль";
        case 7:
            return "август";
        case 8:
            return "сентябрь";
        case 9:
            return "октябрь";
        case 10:
            return "ноябрь";
        case 11:
            return "декабрь";
    }
}


function PaintCurrentMonthColumn() {
    let monthRow = <HTMLTableRowElement>document.getElementById("monthRow");
    let monthCellsCollection = monthRow.cells;
    let currentMonthName = getCurrentMonthName(new Date().getMonth());

    let currentYear = new Date().getFullYear();
    let tableYear = parseInt((<HTMLSelectElement>document.getElementById("changeYearSelector")).value);
    
    if (currentYear == tableYear) {
        for (let i = 1; i < monthCellsCollection.length; i++) {
            let cellText = monthCellsCollection[i].innerText;
            let monthName = cellText.split(' ')[0].toLowerCase();

            if (monthName == currentMonthName) {

                monthCellsCollection[i].classList.add("month-highlight");
                return;
            }
        }
    }
}

function AddPaintCurrentMonthEvent() {
    let yearSelectorElement = <HTMLSelectElement>document.getElementById("changeYearSelector");
    yearSelectorElement.addEventListener("change", function () {
        PaintCurrentMonthColumn();
    })
}

function AddYearChangeEvent() {
    let selector = <HTMLSelectElement>document.getElementById("changeYearSelector");

    selector.addEventListener("change", function () {
        let form = <HTMLFormElement>document.getElementById("changeYearForm");
        let newYear = selector.value;
        let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");
        let newAccuracy = accuracyInput.value;
        window.location.search = ("?Year=" + newYear + "&Accuracy=" + newAccuracy);
    });
}

function AddAccuracyChangeEvent() {
    let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");

    accuracyInput.addEventListener("change", function () {
        let input = this.value;

        if (input.length > 0 && input[0] == "0" || input.length == 0 || input[0] == "-") {
            this.value = "0";
        }

        HighlightCells();
    });
}

function AddVlaueDifferenceHighlight(cell1: HTMLTableDataCellElement, cell2: HTMLTableDataCellElement) {
    let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");
    let accuracy = parseFloat(accuracyInput.value);

    cell1.classList.remove("blue-highlight");
    cell2.classList.remove("blue-highlight");

    cell1.classList.remove("red-highlight");
    cell2.classList.remove("red-highlight");

    let plannedCapacityCellText = cell1.innerText;
    let plannedCapacity = parseFloat(plannedCapacityCellText.substring(0, plannedCapacityCellText.length - 1));

    let currentCapacityCellText = cell2.innerText;
    let currentCapacity = parseFloat(currentCapacityCellText.substring(0, currentCapacityCellText.length - 1));

    let difference = plannedCapacity - currentCapacity;

    if (difference < 0)
        difference = difference * -1;

    if (difference <= accuracy) {

    } else if (currentCapacity > plannedCapacity) {
        cell1.classList.add("blue-highlight");
        cell2.classList.add("blue-highlight");
    } else {
        cell1.classList.add("red-highlight");
        cell2.classList.add("red-highlight");
    }
}

function AddTotalSumHighlight(cell1: HTMLTableDataCellElement, cell2: HTMLTableDataCellElement) {
    cell1.classList.add("total-sum-highlight");
    cell2.classList.add("total-sum-highlight");
}

function HighlightCells() {
    let cells = <HTMLCollectionOf<HTMLTableDataCellElement>>document.getElementsByClassName("for-js-selecor");


    for (let i = 0; i < cells.length; i+=2) {

        if (cells[i].classList.contains("total-sum-js") && !cells[i].classList.contains("total-sum-highlight")) {
            AddTotalSumHighlight(cells[i], cells[i + 1]);
        }

        AddVlaueDifferenceHighlight(cells[i], cells[i + 1]);
    }
}

function AbsenceResolver(year:number) {
    console.log("здесь")
    let dict;
    let request = new XMLHttpRequest();
    request.open("GET", "functionalcapacity/getAbsences?year=" + year, false);
    request.onload = function () {
        //console.log(request.responseText);
        dict = JSON.parse(request.responseText);
    };

    request.send();
    
    return JSON.parse(request.responseText);
}

function AbsenceCheckboxEvent(){
    let checkbox = <HTMLInputElement> document.getElementById("absence-checkbox");
    
    checkbox.addEventListener("change",function () {
        let tableYear = parseInt((<HTMLSelectElement>document.getElementById("changeYearSelector")).value);
        let absenceDict = AbsenceResolver(tableYear);
        let dif = 1;
        let userNameKey = "";
        let currentMonthName = "";
        
        if(checkbox.checked)
            dif = -1;

        let cells = <HTMLCollectionOf<HTMLTableDataCellElement>>document.getElementsByClassName("absence-for-js");

        for (let i = 0; i < cells.length; i++) {
            if(i%13 == 0){
                
                userNameKey = cells[i].innerText;
            }
            else{
                if(absenceDict[userNameKey] != undefined){
                    currentMonthName = getCurrentMonthName(i%13-1);
                    
                    if(absenceDict[userNameKey][currentMonthName] != undefined){
                        let str = (cells[i].innerText);
                        let str1 = str.substr(0,str.length);
                        let currentCapac = parseInt(str1);
                        let newCapac = currentCapac + dif*absenceDict[userNameKey][currentMonthName];
                        cells[i].innerText = newCapac + "%";
                    }
                }
            }
        }
        
        AmountResolver();
        HighlightCells();
    })
    
}

function AmountResolver(){
    //main-column-js
    let cells = <HTMLCollectionOf<HTMLTableDataCellElement>>document.getElementsByClassName("main-column-js");
    let indexList: number[] = [];
    let rangeList: number[] = [];
    
    for (let i = 0; i < cells.length; i++){
        console.log(cells[i].innerText);
        
        if(cells[i].innerText.split(' ').length == 1){
            indexList.push(i);
        }
    }
    
    for(let i = 0; i < indexList.length; i+=2){
        rangeList.push(indexList[i+1] - indexList[i] - 1);
    }

    cells = <HTMLCollectionOf<HTMLTableDataCellElement>>document.getElementsByClassName("for-js-selecor");
    
    let prev = 0;
    
    for (let index of rangeList){
        let array = InitYearAmountArray();
        console.log(index);
        
        for(let i = prev*24; i < (prev + index)*24; i++){
            let value = GetNumberFromCell(cells[i]);
            array[i%24] += value;
        }   
        
        for(let i =(prev + index)*24; i < (prev + index)*24+24;i++){
            WriteNumberToCell(cells[i],array[i%24]);
        }    
        console.log("perv  bilo " + prev);
        prev = index + prev +1;
        console.log("perv stalo "  + prev);
    }
}

function GetNumberFromCell(cell : HTMLTableDataCellElement){
    let str = (cell.innerText);
    let str1 = str.substr(0,str.length);
    return parseInt(str1);
}

function InitYearAmountArray() {
    let array: number[] = [];
    
    for (let i =0; i < 24; i++){
        array.push(0);
    }
    
    return array;
}

function WriteNumberToCell(cell : HTMLTableDataCellElement,output : number){
    cell.innerText = output + "%";
}

document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился");
    //AmountResolver();
    
    AbsenceCheckboxEvent();
    HighlightCells();
    AddAccuracyChangeEvent();
    PaintCurrentMonthColumn();
    AddYearChangeEvent();
    AddAccuracyChangeEvent();
    AddPaintCurrentMonthEvent();
});