function getCurrentMonthName(currentMonthNumber) {
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
    var currentMonthName = getCurrentMonthName(new Date().getMonth());
    var currentYear = new Date().getFullYear();
    var tableYear = parseInt(document.getElementById("changeYearSelector").value);
    if (currentYear == tableYear) {
        for (var i = 1; i < monthCellsCollection.length; i++) {
            var cellText = monthCellsCollection[i].innerText;
            var monthName = cellText.split(' ')[0].toLowerCase();
            if (monthName == currentMonthName) {
                monthCellsCollection[i].classList.add("month-highlight");
                return;
            }
        }
    }
}
function AddPaintCurrentMonthEvent() {
    var yearSelectorElement = document.getElementById("changeYearSelector");
    yearSelectorElement.addEventListener("change", function () {
        PaintCurrentMonthColumn();
    });
}
function AddYearChangeEvent() {
    var selector = document.getElementById("changeYearSelector");
    selector.addEventListener("change", function () {
        var form = document.getElementById("changeYearForm");
        var newYear = selector.value;
        var accuracyInput = document.getElementById("accuracy-input");
        var newAccuracy = accuracyInput.value;
        window.location.search = ("?Year=" + newYear + "&Accuracy=" + newAccuracy);
    });
}
function AddAccuracyChangeEvent() {
    var accuracyInput = document.getElementById("accuracy-input");
    accuracyInput.addEventListener("change", function () {
        var input = this.value;
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
        if (cells[i].classList.contains("total-sum-js") && !cells[i].classList.contains("total-sum-highlight")) {
            AddTotalSumHighlight(cells[i], cells[i + 1]);
        }
        AddVlaueDifferenceHighlight(cells[i], cells[i + 1]);
    }
}
function AbsenceResolver(year) {
    console.log("здесь");
    var dict;
    var request = new XMLHttpRequest();
    request.open("GET", "functionalcapacity/getAbsences?year=" + year, false);
    request.onload = function () {
        dict = JSON.parse(request.responseText);
    };
    request.send();
    return JSON.parse(request.responseText);
}
function AbsenceCheckboxEvent() {
    var checkbox = document.getElementById("absence-checkbox");
    checkbox.addEventListener("change", function () {
        var tableYear = parseInt(document.getElementById("changeYearSelector").value);
        var absenceDict = AbsenceResolver(tableYear);
        var dif = 1;
        var userNameKey = "";
        var currentMonthName = "";
        if (checkbox.checked)
            dif = -1;
        var cells = document.getElementsByClassName("absence-for-js");
        for (var i = 0; i < cells.length; i++) {
            if (i % 13 == 0) {
                userNameKey = cells[i].innerText;
            }
            else {
                if (absenceDict[userNameKey] != undefined) {
                    currentMonthName = getCurrentMonthName(i % 13 - 1);
                    if (absenceDict[userNameKey][currentMonthName] != undefined) {
                        var str = (cells[i].innerText);
                        var str1 = str.substr(0, str.length);
                        var currentCapac = parseInt(str1);
                        var newCapac = currentCapac + dif * absenceDict[userNameKey][currentMonthName];
                        cells[i].innerText = newCapac + "%";
                    }
                }
            }
        }
        AmountResolver();
        HighlightCells();
    });
}
function AmountResolver() {
    var cells = document.getElementsByClassName("main-column-js");
    var indexList = [];
    var rangeList = [];
    for (var i = 0; i < cells.length; i++) {
        console.log(cells[i].innerText);
        if (cells[i].innerText.split(' ').length == 1) {
            indexList.push(i);
        }
    }
    for (var i = 0; i < indexList.length; i += 2) {
        rangeList.push(indexList[i + 1] - indexList[i] - 1);
    }
    cells = document.getElementsByClassName("for-js-selecor");
    var prev = 0;
    for (var _i = 0, rangeList_1 = rangeList; _i < rangeList_1.length; _i++) {
        var index = rangeList_1[_i];
        var array = InitYearAmountArray();
        for (var i = prev * 24; i < (prev + index) * 24; i++) {
            var value = GetNumberFromCell(cells[i]);
            array[i % 24] += value;
        }
        for (var i = (prev + index) * 24; i < (prev + index) * 24 + 24; i++) {
            WriteNumberToCell(cells[i], array[i % 24]);
        }
        prev = index + prev;
    }
}
function GetNumberFromCell(cell) {
    var str = (cell.innerText);
    var str1 = str.substr(0, str.length);
    return parseInt(str1);
}
function InitYearAmountArray() {
    var array = [];
    for (var i = 0; i < 24; i++) {
        array.push(0);
    }
    return array;
}
function WriteNumberToCell(cell, output) {
    cell.innerText = output + "%";
}
document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился");
    AbsenceCheckboxEvent();
    HighlightCells();
    AddAccuracyChangeEvent();
    PaintCurrentMonthColumn();
    AddYearChangeEvent();
    AddAccuracyChangeEvent();
    AddPaintCurrentMonthEvent();
});
//# sourceMappingURL=FunctionalCapacity.js.map