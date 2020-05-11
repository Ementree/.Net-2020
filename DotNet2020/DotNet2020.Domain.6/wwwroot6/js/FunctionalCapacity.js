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
    var monthRow = document.getElementById("monthRow");
    var monthCellsCollection = monthRow.cells;
    var currentMonthName = getCurrentMonthName();
    var currentYear = new Date().getFullYear();
    var tableYear = parseInt(document.getElementById("changeYearSelector").value);
    console.log(tableYear);
    console.log("Ntreonq ujl");
    console.log(currentYear);
    if (currentYear == tableYear) {
        for (var i = 1; i < monthCellsCollection.length; i++) {
            var cellText = monthCellsCollection[i].innerText;
            var monthName = cellText.split(' ')[0].toLowerCase();
            if (monthName == currentMonthName) {
                monthCellsCollection[i].classList.add("month-highlight");
                console.log("добвился класс :" + currentMonthName);
                return;
            }
            console.log(monthName);
        }
    }
}
function AddYearChangeEvent() {
    var selector = document.getElementById("changeYearSelector");
    selector.addEventListener("change", function () {
        var form = document.getElementById("changeYearForm");
        console.log("Кто-то сменил год...");
        var newYear = selector.value;
        var accuracyInput = document.getElementById("accuracy-input");
        var newAccuracy = accuracyInput.value;
        console.log(newAccuracy);
        console.log(newYear);
        console.log(window.location.search);
        window.location.search = ("?Year=" + newYear + "&Accuracy=" + newAccuracy);
        console.log(window.location.search);
    });
}
function AddAccuracyChangeEvent() {
    var accuracyInput = document.getElementById("accuracy-input");
    console.log("v accuracy event");
    accuracyInput.addEventListener("change", function () {
        var input = this.value;
        console.log(input);
        if (input.length > 0 && input[0] == "0" || input.length == 0 || input[0] == "-") {
            this.value = "0";
        }
        HighlightCells();
    });
}
function AddVlaueDifferenceHighlight(cell1, cell2) {
    var accuracyInput = document.getElementById("accuracy-input");
    var accuracy = parseFloat(accuracyInput.value);
    cell1.classList.remove("blue-highlight");
    cell2.classList.remove("blue-highlight");
    cell1.classList.remove("red-highlight");
    cell2.classList.remove("red-highlight");
    var plannedCapacityCellText = cell1.innerText;
    var plannedCapacity = parseFloat(plannedCapacityCellText.substring(0, plannedCapacityCellText.length - 1));
    var currentCapacityCellText = cell2.innerText;
    var currentCapacity = parseFloat(currentCapacityCellText.substring(0, currentCapacityCellText.length - 1));
    var difference = plannedCapacity - currentCapacity;
    if (difference < 0)
        difference = difference * -1;
    if (difference <= accuracy) {
    }
    else if (currentCapacity > plannedCapacity) {
        cell1.classList.add("blue-highlight");
        cell2.classList.add("blue-highlight");
    }
    else {
        cell1.classList.add("red-highlight");
        cell2.classList.add("red-highlight");
    }
}
function AddTotalSumHighlight(cell1, cell2) {
    cell1.classList.add("total-sum-highlight");
    cell2.classList.add("total-sum-highlight");
}
function HighlightCells() {
    var cells = document.getElementsByClassName("for-js-selecor");
    for (var i = 0; i < cells.length; i += 2) {
        console.log(cells[i].innerText);
        if (cells[i].classList.contains("total-sum-js") && !cells[i].classList.contains("total-sum-highlight")) {
            AddTotalSumHighlight(cells[i], cells[i + 1]);
        }
        AddVlaueDifferenceHighlight(cells[i], cells[i + 1]);
    }
}
document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился");
    HighlightCells();
    AddAccuracyChangeEvent();
    PaintCurrentMonthColumn();
    AddYearChangeEvent();
    AddAccuracyChangeEvent();
});
//# sourceMappingURL=FunctionalCapacity.js.map