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
    for (var i = 1; i < monthCellsCollection.length; i++) {
        var cellText = monthCellsCollection[i].innerText;
        var monthName = cellText.split(' ')[0].toLowerCase();
        if (monthName == currentMonthName) {
            monthCellsCollection[i].classList.add("current-month");
            console.log("добвился класс :" + currentMonthName);
            return;
        }
        console.log(monthName);
    }
}
function AddYearChangeEvent() {
    var selector = document.getElementById("changeYearSelector");
    selector.addEventListener("change", function () {
        var form = document.getElementById("changeYearForm");
        console.log("Кто-то сменил год...");
        var value = selector.value;
        console.log(value);
        console.log(window.location.search);
        window.location.search = ("?currentYear=" + value);
        console.log(window.location.search);
    });
}
function AddAccuracyChangeEvent() {
}
document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился");
    PaintCurrentMonthColumn();
    AddYearChangeEvent();
});
//# sourceMappingURL=FunctionalCapacityScript.js.map