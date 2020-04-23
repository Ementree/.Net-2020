function getCurrentMonthName() {
    var currentMonthNumber = new Date().getMonth();
    console.log("текущий месяц " + currentMonthNumber);

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
    let currentMonthName = getCurrentMonthName();

    for (let i = 1; i < monthCellsCollection.length; i++) {
        let cellText = monthCellsCollection[i].innerText;
        let monthName = cellText.split(' ')[0].toLowerCase();

        if (monthName == currentMonthName) {

            monthCellsCollection[i].classList.add("month-highlight");
            console.log("добвился класс :" + currentMonthName);
            return;
        }
        console.log(monthName);
    }
}

function AddYearChangeEvent() {
    let selector = <HTMLSelectElement>document.getElementById("changeYearSelector");

    selector.addEventListener("change", function () {
        let form = <HTMLFormElement>document.getElementById("changeYearForm");
        console.log("Кто-то сменил год...");
        let newYear = selector.value;
        let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");
        let newAccuracy = accuracyInput.value;
        console.log(newAccuracy);
        console.log(newYear);
        console.log(window.location.search);
        window.location.search = ("?currentYear=" + newYear + "&currentAccuracy=" + newAccuracy);
        console.log(window.location.search);
    });
}

function AddAccuracyChangeEvent() {
    let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");
    console.log("v accuracy event");

    accuracyInput.addEventListener("change", function () {
        let input = this.value;
        console.log(input);

        if (input.length > 0 && input[0] == "0" || input.length == 0 || input[0] == "-") {
            this.value = "0";
        }

        HilightCells();
    });
}

function HilightCells() {
    let cells = <HTMLCollectionOf<HTMLTableDataCellElement>>document.getElementsByClassName("for-js-selecor");
    let accuracyInput = <HTMLInputElement>document.getElementById("accuracy-input");
    let accuracy = parseFloat(accuracyInput.value);

    for (let i = 0; i < cells.length; i+=2) {
        console.log(cells[i].innerText);
        cells[i].classList.remove("blue-highlight");
        cells[i + 1].classList.remove("blue-highlight");

        cells[i].classList.remove("red-highlight");
        cells[i + 1].classList.remove("red-highlight");



        let plannedCapacityCellText = cells[i].innerText;
        let plannedCapacity = parseFloat(plannedCapacityCellText.substring(0, plannedCapacityCellText.length - 1));

        let currentCapacityCellText = cells[i+1].innerText;
        let currentCapacity = parseFloat(currentCapacityCellText.substring(0, currentCapacityCellText.length - 1));

        let difference = plannedCapacity - currentCapacity;

        if (difference < 0)
            difference = difference * -1;

        if (difference <= accuracy) {

        } else if (currentCapacity > plannedCapacity) {
            cells[i].classList.add("blue-highlight");
            cells[i + 1].classList.add("blue-highlight");
        } else {
            cells[i].classList.add("red-highlight");
            cells[i + 1].classList.add("red-highlight");
        }
    }
}


document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился")
    HilightCells();
    AddAccuracyChangeEvent();
    PaintCurrentMonthColumn();
    AddYearChangeEvent();
    AddAccuracyChangeEvent();
});