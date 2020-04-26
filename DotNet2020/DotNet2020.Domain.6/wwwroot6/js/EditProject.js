var projectStatuses = getProjectStatuses();
var resources = [];
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
    monthCapacityInput.value = String(period.capacity);
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
    removeResBtn.disabled = period.resources.length <= 1;
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
    var resContainer = document.createElement('div');
    resContainer.id = "editAddResourceYear" + period.date.getFullYear() + "Month" + period.date.getMonth();
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
        var emptyOption = document.createElement('option');
        emptyOption.disabled = true;
        emptyOption.selected = true;
        emptyOption.text = 'Выберите человека';
        for (var i = 0; i < selectResource.options.length; i++) {
            if (String(value.id) === selectResource.options[i].value)
                selectResource.selectedIndex = i;
        }
        resContainer.appendChild(row);
    });
    return resContainer;
}
function generateEditYear(periods) {
    if (periods.length != 12)
        throw 'exception in periods length';
    var yearContainer = document.createElement('div');
    console.log(periods[1]);
    yearContainer.id = "year" + periods[1].date.getFullYear();
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
    var yearsContainer = document.createElement('div');
    yearsContainer.id = 'yearsContainer';
    var yearsCount = periods.length / 12;
    var sliceStart = 0;
    var sliceEnd = 12;
    for (var i = 0; i < yearsCount; i++) {
        yearsContainer.appendChild(generateEditYear(periods.slice(sliceStart, sliceEnd)));
        sliceStart = sliceEnd;
        sliceEnd = sliceEnd + 12;
    }
    return yearsContainer;
}
//# sourceMappingURL=EditProject.js.map