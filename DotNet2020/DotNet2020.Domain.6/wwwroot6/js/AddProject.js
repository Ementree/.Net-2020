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
    document.getElementById('removeYearButton').disabled = false;
}
function RemoveLastYear() {
    if (lastYear === new Date(Date.now()).getFullYear() + 1) {
        document.getElementById('removeYearButton').disabled = true;
    }
    var lastYearDiv = document.getElementById("year" + lastYear);
    document.getElementById('yearsContainer').removeChild(lastYearDiv);
    lastYear--;
}
function GenerateMonth(localYear, monthNumber) {
    var monthBlock = (document
        .getElementById("year" + (localYear - 1) + "Month" + monthNumber)
        .cloneNode(true));
    monthBlock.id = "year" + localYear + "Month" + monthNumber;
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
function GetProjectInfo() {
    var project = new Project();
    var projectName = document.getElementById('projectName').value;
    project.Name = projectName;
    project.Periods = [];
    var yearDivs = document.getElementById('yearsContainer').children;
    for (var i = 0; i < yearDivs.length; i++) {
        var yearDiv = yearDivs[i];
        var year = parseInt(yearDiv.firstElementChild.textContent);
        var halfYears = [yearDiv.children[1], yearDiv.children[2]];
        var firstHalfYear = halfYears[0];
        var firstHalfMonths = firstHalfYear.children;
        for (var j = 0; j < firstHalfMonths.length; j++) {
            var monthBlock = firstHalfMonths[j];
            var period = GetPeriodInfo(monthBlock);
            project.Periods.push(period);
        }
        var secondHalfYear = halfYears[1];
        var secondHalfMonths = secondHalfYear.children;
        for (var j = 0; j < secondHalfMonths.length; j++) {
            var monthBlock = secondHalfMonths[j];
            var period = GetPeriodInfo(monthBlock);
            project.Periods.push(period);
        }
    }
    console.log(project);
    return project;
}
function GetPeriodInfo(monthBlock) {
    var period = new Period();
    var date = monthBlock.id.replace(/\D/g, '_').split('_').filter(function (d) { return d !== ''; }).map(function (d) { return parseInt(d); });
    period.Date = new Date(date[0], date[1]);
    period.Resources = [];
    var selects = monthBlock.children[2].children;
    for (var rowNumber = 0; rowNumber < selects.length; rowNumber++) {
        var selectValuePair = selects[rowNumber];
        var select = selectValuePair.firstElementChild.firstElementChild;
        var resourceId = select.options[select.selectedIndex].value;
        var resourceFullName = select.options[select.selectedIndex].text;
        var value = parseInt(selectValuePair.lastElementChild.textContent);
        if (resourceId.trim() !== "") {
            period.Resources.push(new ResourceCapacity(parseInt(resourceId), resourceFullName, value));
        }
    }
    return period;
}
function SendProjectToDb() {
    var project = GetProjectInfo();
    var xhr = new XMLHttpRequest();
    xhr.open('PUT', 'plan/addProject', false);
    xhr.setRequestHeader('Content-type', 'application/json');
    xhr.send(JSON.stringify(project));
    var success = xhr.responseText;
}
//# sourceMappingURL=AddProject.js.map