var projectStatuses = getProjectStatuses();
var resources = [];
var currentYear;
function selectProject() {
    var select = document.getElementById('projectSelector');
    var projectId = parseInt(select.options[select.selectedIndex].value);
    var project = getProject(projectId);
    var mainContainer = select.parentElement.parentElement;
    try {
        mainContainer.removeChild(document.getElementById('editProjectDiv'));
    }
    catch (e) {
        console.log(e);
    }
    var years = project.periods
        .map(function (period) { return period.date.getFullYear(); })
        .filter(function (value, index, self) {
        return self.indexOf(value) === index;
    });
    if (years.length == 0) {
        var date = new Date(Date.now());
        currentYear = date.getFullYear() - 1;
    }
    else {
        currentYear = years[years.length - 1];
    }
    var projectDOM = generateProjectDOM(project);
    mainContainer.appendChild(projectDOM);
}
function getProjectStatuses() {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', "/plan/getProjectStatuses", false);
    xhr.send();
    var statuses = JSON.parse(xhr.responseText);
    return statuses;
}
function getProject(id) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', "/plan/getProjectPlanById/" + id, false);
    xhr.send();
    var projectWithOutPeriods = JSON.parse(xhr.responseText);
    projectWithOutPeriods.periods.forEach(function (value) {
        value.date = new Date(Date.parse(value.date));
    });
    var project = projectWithOutPeriods;
    return project;
}
function generateProjectDOM(project) {
    var rootDiv = document.createElement('div');
    var yearCount = project.periods
        .map(function (period) { return period.date.getFullYear(); })
        .filter(function (value, index, self) {
        return self.indexOf(value) === index;
    }).length;
    rootDiv.id = "editProjectDiv";
    rootDiv.appendChild(generateNameBlock(project.name));
    rootDiv.appendChild(generateProjectStatusSelect(project.statusId));
    rootDiv.appendChild(generateButtonsBlock(yearCount));
    rootDiv.appendChild(generateYearsContainer(project.periods));
    return rootDiv;
}
function generateNameBlock(projectName) {
    var nameForm = document.createElement('div');
    nameForm.classList.add('form-inline', 'mt-2');
    var nameLabel = document.createElement('label');
    nameLabel.textContent = 'Название проекта:';
    nameLabel.htmlFor = 'editProjectName';
    nameLabel.classList.add('mr-2');
    var nameInput = document.createElement('input');
    nameInput.type = 'text';
    nameInput.classList.add('form-control', 'w-75');
    nameInput.value = projectName;
    nameInput.id = 'editProjectName';
    nameForm.append(nameLabel, nameInput);
    return nameForm;
}
function generateProjectStatusSelect(projectStatusId) {
    var selectForm = document.createElement('div');
    selectForm.classList.add('form-inline', 'mt-2');
    var selectLabel = document.createElement('label');
    selectLabel.textContent = 'Статус проекта:';
    selectLabel.htmlFor = 'projectStatusEdit';
    selectLabel.classList.add('mr-2');
    var selector = document.createElement('select');
    selector.id = 'projectStatusEdit';
    projectStatuses.forEach(function (value) {
        var option = document.createElement('option');
        option.value = String(value.id);
        option.text = value.status;
        selector.options.add(option);
    });
    for (var i = 0; i < selector.options.length; i++) {
        if (selector.options[i].value === String(projectStatusId))
            selector.selectedIndex = i;
    }
    selectForm.append(selectLabel, selector);
    return selectForm;
}
function generateButtonsBlock(yearsCount) {
    var buttonBlock = document.createElement('div');
    buttonBlock.classList.add('form-inline', 'mt-2', 'mb-4');
    var addYearButton = document.createElement('button');
    addYearButton.classList.add('btn', 'btn-primary');
    addYearButton.textContent = 'Добавить год';
    var removeYearButton = document.createElement('button');
    removeYearButton.classList.add('btn', 'btn-primary');
    removeYearButton.textContent = 'Удалить последний год';
    removeYearButton.disabled = yearsCount <= 1;
    removeYearButton.id = 'editRemoveYearButton';
    addYearButton.addEventListener('click', function () {
        var newYearPeriods = [];
        var year = ++currentYear;
        for (var i = 0; i < 12; i++)
            newYearPeriods.push(new Period(Number.NaN, new Date(year, i), []));
        document.getElementById('yearsContainerEdit').appendChild(generateEditYear(newYearPeriods));
        document.getElementById('editRemoveYearButton').disabled = false;
    });
    removeYearButton.addEventListener('click', function () {
        var removedYear = --currentYear;
        var container = document.getElementById('yearsContainerEdit');
        container.removeChild(container.lastChild);
        if (container.childElementCount === 1)
            document.getElementById('editRemoveYearButton').disabled = true;
    });
    buttonBlock.append(addYearButton, removeYearButton);
    return buttonBlock;
}
function generateMonth(period) {
    var idBase = "Year" + period.date.getFullYear() + "Month" + period.date.getMonth();
    var container = document.createElement('div');
    container.id = "edit" + idBase;
    container.classList.add('col-2');
    var nameAndCapacity = document.createElement('div');
    nameAndCapacity.classList.add('row');
    var monthName = document.createElement('div');
    monthName.classList.add('col-7', 'font-weight-bold', 'pl-1', 'pr-1');
    monthName.textContent = GetMonthName(period.date.getMonth() + 1) + " - ";
    var monthCapacity = document.createElement('div');
    monthCapacity.classList.add('col-5', 'font-weight-bold', 'pl-1', 'pr-1');
    var monthCapacityInput = document.createElement('input');
    monthCapacityInput.value = String(isNaN(period.capacity) ? '' : period.capacity);
    monthCapacityInput.id = "editMonthCapacity" + idBase;
    monthCapacityInput.type = 'number';
    monthCapacityInput.min = String(0);
    monthCapacityInput.classList.add('w-100');
    monthCapacityInput.placeholder = 'Загрузка';
    monthCapacity.appendChild(monthCapacityInput);
    nameAndCapacity.append(monthName, monthCapacity);
    container.appendChild(nameAndCapacity);
    var resAndCap = document.createElement('div');
    resAndCap.classList.add('row');
    var res = document.createElement('div');
    res.classList.add('col-5', 'pl-1', 'pr-1', 'text-left');
    res.textContent = 'Ресурс';
    var cap = document.createElement('div');
    cap.classList.add('col-7', 'p-0', 'text-right');
    cap.textContent = 'Загрузка, %';
    resAndCap.append(res, cap);
    container.appendChild(resAndCap);
    container.appendChild(generateResourceSelectorWithValue(period));
    var btnContainer = document.createElement('div');
    container.appendChild(btnContainer);
    btnContainer.classList.add('row', 'pl-1', 'pr-1');
    var addResBtn = document.createElement('button');
    addResBtn.classList.add('btn', 'btn-sm', 'btn-outline-dark', 'w-100', 'p-1');
    var removeResBtn = addResBtn.cloneNode();
    addResBtn.textContent = 'Добавить человека';
    removeResBtn.textContent = 'Удалить последнего человека';
    removeResBtn.id = "editRemoveResourceButton" + idBase;
    removeResBtn.disabled = period.resources.length === 0;
    addResBtn.addEventListener('click', function () {
        var baseId = idBase;
        var container = document.getElementById('editAddResource' + baseId);
        var row = document.createElement('div');
        row.classList.add('row', 'mb-2');
        container.appendChild(row);
        var selectContainer = document.createElement('div');
        selectContainer.classList.add('col-9', 'p-0');
        var capacityContainer = document.createElement('div');
        capacityContainer.classList.add('col-3', 'p-0', 'text-center', 'overflow-hidden');
        container.append(selectContainer, capacityContainer);
        row.append(selectContainer, capacityContainer);
        var select = document.createElement('select');
        selectContainer.append(select);
        select.classList.add('custom-select');
        select.id = "editSelectResourceYear" + baseId;
        var capacityInput = document.createElement('input');
        capacityInput.classList.add('w-100', 'h-100');
        capacityInput.min = '0';
        capacityInput.placeholder = 'Загрузка';
        capacityInput.type = 'number';
        capacityInput.value = '';
        capacityContainer.appendChild(capacityInput);
        var emptyOption = document.createElement('option');
        emptyOption.disabled = true;
        emptyOption.selected = true;
        emptyOption.text = 'Выберите человека';
        select.options.add(emptyOption);
        resources.forEach(function (res) {
            var option = document.createElement('option');
            option.value = String(res.id);
            option.text = res.name;
            select.options.add(option);
        });
        document.getElementById('editRemoveResourceButton' + baseId).disabled = false;
    });
    removeResBtn.addEventListener('dblclick', function () {
        var baseId = idBase;
        var container = document.getElementById('editAddResource' + baseId);
        container.removeChild(container.lastChild);
        if (container.childElementCount === 0)
            document.getElementById('editRemoveResourceButton' + baseId).disabled = true;
    });
    btnContainer.append(addResBtn, removeResBtn);
    return container;
}
function generateResourceSelectorWithValue(period) {
    if (resources.length === 0) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/plan/getResources', false);
        xhr.send();
        var response = JSON.parse(xhr.responseText);
        response.forEach(function (value) {
            resources.push(value);
        });
    }
    var baseId = "Year" + period.date.getFullYear() + "Month" + period.date.getMonth();
    var resContainer = document.createElement('div');
    resContainer.id = "editAddResource" + baseId;
    period.resources.forEach(function (value) {
        var row = document.createElement('div');
        row.classList.add('row', 'mb-2');
        var selectContainer = document.createElement('div');
        selectContainer.classList.add('col-9', 'p-0');
        var capacityContainer = document.createElement('div');
        capacityContainer.classList.add('col-3', 'p-0', 'text-center', 'overflow-hidden');
        var capacityInput = document.createElement('input');
        capacityInput.classList.add('w-100', 'h-100');
        capacityInput.min = '0';
        capacityInput.placeholder = 'Загрузка';
        capacityInput.type = 'number';
        capacityInput.value = String(value.capacity);
        capacityContainer.appendChild(capacityInput);
        row.append(selectContainer, capacityContainer);
        var selectResource = document.createElement('select');
        selectResource.classList.add('custom-select');
        selectResource.id = "editSelectResourceYear" + period.date.getFullYear() + "Month" + period.date.getMonth();
        selectContainer.appendChild(selectResource);
        resources.forEach(function (res) {
            var option = document.createElement('option');
            option.value = String(res.id);
            option.text = res.name;
            if (value.id === res.id)
                option.selected = true;
            selectResource.options.add(option);
        });
        for (var i = 0; i < selectResource.options.length; i++) {
            if (String(value.id) === selectResource.options[i].value) {
                selectResource.selectedIndex = i;
            }
        }
        resContainer.appendChild(row);
    });
    return resContainer;
}
function generateEditYear(periods) {
    if (periods.length != 12) {
        throw 'exception in periods length';
    }
    var yearContainer = document.createElement('div');
    yearContainer.id = "editYear" + periods[1].date.getFullYear();
    yearContainer.classList.add('year');
    var yearName = document.createElement('p');
    yearName.textContent = String(periods[1].date.getFullYear());
    var firstHalfYear = document.createElement('div');
    var secondHalfYear = document.createElement('div');
    yearContainer.append(yearName, firstHalfYear, secondHalfYear);
    firstHalfYear.id = "year" + periods[1].date.getFullYear() + "_1";
    firstHalfYear.classList.add('row', 'pb-3', 'halfYear');
    for (var i = 0; i < 6; i++) {
        firstHalfYear.append(generateMonth(periods[i]));
    }
    secondHalfYear.id = "year" + periods[1].date.getFullYear() + "_2";
    secondHalfYear.classList.add('row', 'pb-3', 'halfYear');
    for (var i = 6; i < 12; i++) {
        secondHalfYear.append(generateMonth(periods[i]));
    }
    return yearContainer;
}
function generateYearsContainer(periods) {
    var periodsLocal = periods.slice();
    var yearsContainer = document.createElement('div');
    yearsContainer.id = 'yearsContainerEdit';
    var years = periodsLocal.map(function (period) { return period.date.getFullYear(); }).filter(function (value, index, self) {
        return self.indexOf(value) === index;
    });
    for (var i = 0; i < years.length; i++) {
        var _loop_1 = function (monthNumber) {
            var currentYear_1 = years[i];
            var arr = periodsLocal.filter(function (value) {
                return (value.date.getFullYear() === currentYear_1) && (value.date.getMonth() === monthNumber);
            });
            if (arr.length === 0) {
                var period = new Period();
                period.date = new Date(currentYear_1, monthNumber);
                period.resources = [];
                period.capacity = Number.NaN;
                periodsLocal.push(period);
            }
        };
        for (var monthNumber = 0; monthNumber < 12; monthNumber++) {
            _loop_1(monthNumber);
        }
    }
    periodsLocal.sort(function (per1, per2) {
        return per1.date > per2.date ? 1 : -1;
    });
    var sliceStart = 0;
    var sliceEnd = 12;
    for (var i = 0; i < years.length; i++) {
        yearsContainer.appendChild(generateEditYear(periodsLocal.slice(sliceStart, sliceEnd)));
        sliceStart = sliceEnd;
        sliceEnd = sliceEnd + 12;
    }
    return yearsContainer;
}
function getProjectEditedInfo() {
    var project = new Project();
    var projectIdSelect = document.getElementById('projectSelector');
    var projectId = parseInt(projectIdSelect.options[projectIdSelect.selectedIndex].value);
    project.id = projectId;
    var projectName = document.getElementById('editProjectName').value;
    var projectStatusSelect = document.getElementById('projectStatusEdit');
    var projectStatusId = parseInt(projectStatusSelect.options[projectStatusSelect.selectedIndex].value);
    if (!isNaN(projectStatusId))
        project.statusId = projectStatusId;
    project.name = projectName;
    project.periods = [];
    var yearDivs = document.getElementById('yearsContainerEdit').children;
    for (var i = 0; i < yearDivs.length; i++) {
        var yearDiv = yearDivs[i];
        var year = parseInt(yearDiv.firstElementChild.textContent);
        var halfYears = [yearDiv.children[1], yearDiv.children[2]];
        var firstHalfYear = halfYears[0];
        var firstHalfMonths = firstHalfYear.children;
        for (var j = 0; j < firstHalfMonths.length; j++) {
            var monthBlock = firstHalfMonths[j];
            var period = getEditedPeriodInfo(monthBlock);
            project.periods.push(period);
        }
        var secondHalfYear = halfYears[1];
        var secondHalfMonths = secondHalfYear.children;
        for (var j = 0; j < secondHalfMonths.length; j++) {
            var monthBlock = secondHalfMonths[j];
            var period = getEditedPeriodInfo(monthBlock);
            project.periods.push(period);
        }
    }
    return project;
}
function getEditedPeriodInfo(monthBlock) {
    var period = new Period();
    var date = monthBlock.id.replace(/\D/g, '_').split('_').filter(function (d) { return d !== ''; }).map(function (d) { return parseInt(d); });
    period.date = new Date(date[0], date[1] + 1);
    period.resources = [];
    var capacity = parseInt(document
        .getElementById("editMonthCapacityYear" + date[0] + "Month" + date[1])
        .value);
    if (isNaN(capacity)) {
        capacity = -1;
    }
    period.capacity = capacity;
    var selects = document.getElementById("editAddResourceYear" + date[0] + "Month" + date[1]).children;
    for (var rowNumber = 0; rowNumber < selects.length; rowNumber++) {
        var selectValuePair = selects[rowNumber];
        var select = selectValuePair.firstElementChild.firstElementChild;
        var resourceId = parseInt(select.options[select.selectedIndex].value);
        if (isNaN(resourceId)) {
            resourceId = -1;
        }
        var resourceFullName = select.options[select.selectedIndex].text;
        var value = parseInt(selectValuePair.lastElementChild.firstElementChild.value);
        if (isNaN(value)) {
            value = -1;
        }
        period.resources.push(new ResourceCapacity(resourceId, resourceFullName, value));
    }
    return period;
}
function sendEditedProject() {
    var project = getProjectEditedInfo();
    if (ValidateForm(project, 'edit')) {
        var btn = document.getElementById('editSendProject');
        btn.disabled = true;
        var xhr = new XMLHttpRequest();
        xhr.open('PUT', 'plan/editProject', false);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.send(JSON.stringify(project));
        var success = xhr.responseText;
        if (success === 'true')
            location.reload();
    }
    else {
        document.getElementById('editErrorHandler').style.display = 'block';
    }
}
//# sourceMappingURL=EditProject.js.map