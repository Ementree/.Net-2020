function getCurrentMonthName(currentMonthNumber:number) {
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

    var currentYear = new Date().getFullYear();
    var tableYear = parseInt((<HTMLSelectElement>document.getElementById("changeYearSelector")).value);
    //console.log(tableYear);
    //console.log("Ntreonq ujl");
    //console.log(currentYear);

    if (currentYear == tableYear) {
        for (let i = 1; i < monthCellsCollection.length; i++) {
            let cellText = monthCellsCollection[i].innerText;
            let monthName = cellText.split(' ')[0].toLowerCase();

            if (monthName == currentMonthName) {

                monthCellsCollection[i].classList.add("month-highlight");
                //console.log("добвился класс :" + currentMonthName);
                return;
            }
            //console.log(monthName);
        }
    }
}

function AddPaintCurrentMonthEvent() {
    var yearSelectorElement = <HTMLSelectElement>document.getElementById("changeYearSelector");
    yearSelectorElement.addEventListener("change", function () {
        PaintCurrentMonthColumn();
    })
}

function AddYearChangeEvent() {
    let selector = <HTMLSelectElement>document.getElementById("changeYearSelector");

    selector.addEventListener("change", function () {
        let form = <HTMLFormElement>document.getElementById("changeYearForm");
        //console.log("Кто-то сменил год...");
        let newYear = selector.value;
        let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");
        let newAccuracy = accuracyInput.value;
        //console.log(newAccuracy);
        //console.log(newYear);
        //console.log(window.location.search);
        window.location.search = ("?Year=" + newYear + "&Accuracy=" + newAccuracy);
        //console.log(window.location.search);
    });
}

function AddAccuracyChangeEvent() {
    let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");
    //console.log("v accuracy event");

    accuracyInput.addEventListener("change", function () {
        let input = this.value;
        //console.log(input);

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
        //console.log(cells[i].innerText);

        if (cells[i].classList.contains("total-sum-js") && !cells[i].classList.contains("total-sum-highlight")) {
            AddTotalSumHighlight(cells[i], cells[i + 1]);
        }

        AddVlaueDifferenceHighlight(cells[i], cells[i + 1]);
    }
}

function AbsenceResolver() {
    console.log("здесь")
    let request = new XMLHttpRequest();
    request.open("GET", "functionalcapacity/getAbsences?year=2008", true);
    request.onload = function () {
        console.log(request.responseText);

        let dict = JSON.parse(request.responseText);

        console.log(dict);
        console.log(dict["Артур Саттаров"]);
    }

    request.send();

    return request.responseText;
    
}

document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился");

    AbsenceResolver();

    HighlightCells();
    AddAccuracyChangeEvent();
    PaintCurrentMonthColumn();
    AddYearChangeEvent();
    AddAccuracyChangeEvent();
    AddPaintCurrentMonthEvent();
});