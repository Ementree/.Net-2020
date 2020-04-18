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

            monthCellsCollection[i].classList.add("current-month");
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
        window.location.search = ("?currentYear=" + newYear + "&currentAccuracy="+newAccuracy);
        console.log(window.location.search);
    });
}

function AddAccuracyChangeEvent() {

} 


document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился")

    PaintCurrentMonthColumn();
    AddYearChangeEvent();
    AddAccuracyChangeEvent();
});