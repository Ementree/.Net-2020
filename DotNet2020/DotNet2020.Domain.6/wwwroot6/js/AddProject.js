var Project = (function () {
    function Project() {
    }
    return Project;
}());
var Period = (function () {
    function Period() {
    }
    return Period;
}());
var ResourceCapacity = (function () {
    function ResourceCapacity(id, name, capacity) {
        this.Id = id;
        this.Name = name;
        this.Capacity = capacity;
    }
    return ResourceCapacity;
}());
var lastYear = new Date(Date.now()).getFullYear();
function AddYear() {
    lastYear++;
    var localYear = lastYear;
    console.log(localYear);
    var yearsContainer = document.getElementById('yearsContainer');
    var yearDiv = document.createElement('div');
    yearDiv.id = "year" + localYear;
    yearDiv.classList.add('year');
    var yearParagraph = document.createElement('p');
    yearParagraph.textContent = "" + localYear;
    yearParagraph.classList.add('font-weight-bold');
    yearDiv.appendChild(yearParagraph);
    var firstHalfYearDiv = document.createElement('div');
    firstHalfYearDiv.classList.add('row', 'pb-3', 'halfYear');
    firstHalfYearDiv.id = "year" + localYear + "_1";
    for (var i = 0; i < 6; i++) {
        var monthBlock = GenerateMonth(localYear, i + 1);
        firstHalfYearDiv.appendChild(monthBlock);
    }
    var secondHalfYearDiv = document.createElement('div');
    secondHalfYearDiv.classList.add('row', 'pb-3', 'halfYear');
    secondHalfYearDiv.id = "year" + localYear + "_2";
    for (var i = 6; i < 12; i++) {
        var monthBlock = GenerateMonth(localYear, i + 1);
        secondHalfYearDiv.appendChild(monthBlock);
    }
    yearDiv.appendChild(firstHalfYearDiv);
    yearDiv.appendChild(secondHalfYearDiv);
    yearsContainer.appendChild(yearDiv);
}
function GenerateMonth(localYear, monthNumber) {
    var monthBlock = (document
        .getElementById("year" + (localYear - 1) + "Month" + monthNumber)
        .cloneNode(true));
    monthBlock.id = "year" + localYear + "Month" + monthNumber;
    console.log(monthBlock);
    var addRes = monthBlock.children[2];
    addRes.id = "addResourceYear" + localYear + "Month" + monthNumber;
    while (addRes.childElementCount > 1) {
        addRes.removeChild(addRes.children[addRes.childElementCount - 1]);
    }
    var select = addRes.children[0].children[0].children[0];
    select.id = "selectYear" + localYear + "Month" + monthNumber;
    var buttonBlock = monthBlock.children[monthBlock.childElementCount - 1];
    while (buttonBlock.childElementCount > 0) {
        buttonBlock.removeChild(buttonBlock.children[buttonBlock.childElementCount - 1]);
    }
    var addButton = document.createElement('button');
    addButton.classList.add('btn', 'btn-sm', 'btn-outline-dark', 'w-100', 'p-1');
    addButton.textContent = 'Добавить человека';
    var removeButton = addButton.cloneNode(true);
    removeButton.id = "RemoveButtonYear" + localYear + "Month" + monthNumber;
    removeButton.disabled = true;
    removeButton.textContent = 'Удалить последнего человека';
    addButton.addEventListener('click', function () {
        AddResource(localYear, monthNumber);
    }, false);
    removeButton.addEventListener('click', function () {
        RemoveResource(localYear, monthNumber);
    }, false);
    buttonBlock.appendChild(addButton);
    buttonBlock.appendChild(removeButton);
    return monthBlock;
}
function AddResource(year, month) {
    var deleteButton = document.getElementById("RemoveButtonYear" + year + "Month" + month);
    console.log(deleteButton);
    var monthElem = document.getElementById("addResourceYear" + year + "Month" + month);
    var select = monthElem.children[0].cloneNode(true);
    monthElem.appendChild(select);
    deleteButton.disabled = false;
}
function RemoveResource(year, month) {
    var monthElem = document.getElementById("addResourceYear" + year + "Month" + month);
    var select = monthElem.children[monthElem.children.length - 1];
    monthElem.removeChild(select);
    if (monthElem.childElementCount === 1) {
        var deleteButton = document.getElementById("RemoveButtonYear" + year + "Month" + month);
        deleteButton.disabled = true;
    }
}
function GetMonthName(number) {
    switch (number) {
        case 1:
            return "Январь";
        case 2:
            return "Февраль";
        case 3:
            return "Март";
        case 4:
            return "Апрель";
        case 5:
            return "Май";
        case 6:
            return "Июнь";
        case 7:
            return "Июль";
        case 8:
            return "Август";
        case 9:
            return "Сентябрь";
        case 10:
            return "Октябрь";
        case 11:
            return "Ноябрь";
        case 12:
            return "Декабрь";
    }
}
//# sourceMappingURL=AddProject.js.map