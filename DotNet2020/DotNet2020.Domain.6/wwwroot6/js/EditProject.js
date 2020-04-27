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
    currentYear = years[years.length - 1];
    var projectDOM = generateProjectDOM(project);
    mainContainer.appendChild(projectDOM);
}
function getProjectStatuses() {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', "/plan/getProjectStatuses", false);
    xhr.send();
    console.log(xhr.responseText);
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
    rootDiv.id = "editProjectDiv";
    rootDiv.appendChild(generateNameBlock(project.name));
    rootDiv.appendChild(generateProjectStatusSelect(project.statusId));
    rootDiv.appendChild(generateButtonsBlock());
    rootDiv.appendChild(generateYearsContainer(project.periods));
    return rootDiv;
}
function generateNameBlock(projectName) {
    var nameForm = document.createElement('div');
    nameForm.classList.add('form-inline', 'mt-2');
    var nameLabel = document.createElement('label');
    nameLabel.textContent = 'Название проекта:';
    nameLabel.htmlFor = 'projectNameEdit';
    nameLabel.classList.add('mr-2');
    var nameInput = document.createElement('input');
    nameInput.type = 'text';
    nameInput.classList.add('form-control', 'w-75');
    nameInput.value = projectName;
    nameInput.id = 'projectNameEdit';
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
function generateButtonsBlock() {
    var buttonBlock = document.createElement('div');
    buttonBlock.classList.add('form-inline', 'mt-2', 'mb-4');
    var addYearButton = document.createElement('button');
    addYearButton.classList.add('btn', 'btn-primary');
    addYearButton.textContent = 'Добавить год';
    var removeYearButton = document.createElement('button');
    removeYearButton.classList.add('btn', 'btn-primary');
    removeYearButton.textContent = 'Удалить последний год';
    removeYearButton.disabled = true;
    removeYearButton.id = 'editRemoveYearButton';
    addYearButton.addEventListener('click', function () {
        var newYearPeriods = [];
        var year = ++currentYear;
        for (var i = 0; i < 12; i++)
            newYearPeriods.push(new Period(Number.NaN, new Date(year, i), []));
        document.getElementById('yearsContainerEdit').appendChild(generateEditYear(newYearPeriods));
        document.getElementById('editRemoveYearButton').disabled = false;
    });
    removeYearButton.addEventListener('dblclick', function () {
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
        console.log(container);
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
        console.log(xhr.responseText);
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
            console.log(value.id, value.name, ' - ', selectResource.options[i].value, selectResource.options[i].text);
            if (String(value.id) === selectResource.options[i].value) {
                selectResource.selectedIndex = i;
            }
        }
        resContainer.appendChild(row);
    });
    return resContainer;
}
function generateEditYear(periods) {
    console.log(periods);
    if (periods.length != 12) {
        throw 'exception in periods length';
    }
    var yearContainer = document.createElement('div');
    console.log(periods[1]);
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
//# sourceMappingURL=EditProject.js.map