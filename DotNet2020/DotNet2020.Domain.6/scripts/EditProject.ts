﻿const projectStatuses: ProjectStatus[] = getProjectStatuses();
const resources: ResourceCapacity[] = [];
let currentYear: number;
let pastYear: number;
let firstProjectYear: number;

function selectProject() {
    let select = <HTMLSelectElement>document.getElementById('projectSelector');
    let projectId = parseInt(select.options[select.selectedIndex].value);
    let project: Project = getProject(projectId);
    let mainContainer = select.parentElement.parentElement;
    try {
        mainContainer.removeChild(document.getElementById('editProjectDiv'));
    } catch (e) {
        console.log(e);
    }
    let years = project.periods
        .map(period => period.date.getFullYear())
        .filter((value, index, self) => {
            return self.indexOf(value) === index;
        });
    if (years.length == 0) {
        let date = new Date(Date.now());
        currentYear = date.getFullYear() - 1;
        pastYear = currentYear;
        firstProjectYear = date.getFullYear();
    } else {
        currentYear = years[years.length - 1];
        pastYear = years[0] - 1;
        firstProjectYear = years[0];
    }
    let projectDOM = generateProjectDOM(project);
    mainContainer.appendChild(projectDOM);
}

function getProjectStatuses() {
    let xhr = new XMLHttpRequest();
    xhr.open('GET', `/plan/getProjectStatuses`, false);
    xhr.send();
    let statuses: ProjectStatus[] = <ProjectStatus[]>JSON.parse(xhr.responseText);
    return statuses;
}

function getProject(id: number): Project {
    let xhr = new XMLHttpRequest();
    xhr.open('GET', `/plan/getProjectPlanById/${id}`, false);
    xhr.send();
    let projectWithOutPeriods = JSON.parse(xhr.responseText);
    projectWithOutPeriods.periods.forEach(value => {
        value.date = new Date(Date.parse(value.date));
    })
    let project = <Project>projectWithOutPeriods;
    return project;
}

function generateProjectDOM(project: Project): HTMLDivElement {
    let rootDiv = <HTMLDivElement>document.createElement('div');
    let yearCount = project.periods
        .map(period => period.date.getFullYear())
        .filter((value, index, self) => {
            return self.indexOf(value) === index;
        }).length;
    rootDiv.id = `editProjectDiv`;
    rootDiv.appendChild(generateNameBlock(project.name));
    rootDiv.appendChild(generateProjectStatusSelect(project.statusId));
    rootDiv.appendChild(generateButtonsBlock(yearCount));
    rootDiv.appendChild(generateYearsContainer(project.periods));
    return rootDiv;
}

function generateNameBlock(projectName: string): HTMLDivElement {
    let nameForm = document.createElement('div');
    nameForm.classList.add('form-inline', 'mt-2');
    let nameLabel = <HTMLLabelElement>document.createElement('label');
    nameLabel.textContent = 'Название проекта:';
    nameLabel.htmlFor = 'editProjectName';
    nameLabel.classList.add('mr-2')
    let nameInput = <HTMLInputElement>document.createElement('input');
    nameInput.type = 'text';
    nameInput.classList.add('form-control', 'w-75');
    nameInput.value = projectName;
    nameInput.id = 'editProjectName';
    nameForm.append(nameLabel, nameInput);
    return nameForm;
}

function generateProjectStatusSelect(projectStatusId: number): HTMLDivElement {
    let selectForm = document.createElement('div');
    selectForm.classList.add('form-inline', 'mt-2');
    let selectLabel = <HTMLLabelElement>document.createElement('label');
    selectLabel.textContent = 'Статус проекта:';
    selectLabel.htmlFor = 'projectStatusEdit';
    selectLabel.classList.add('mr-2');
    let selector = <HTMLSelectElement>document.createElement('select');
    selector.id = 'projectStatusEdit';
    projectStatuses.forEach(value => {
        let option = <HTMLOptionElement>document.createElement('option');
        option.value = String(value.id);
        option.text = value.status;
        selector.options.add(option);
    });
    for (let i = 0; i < selector.options.length; i++) {
        if (selector.options[i].value === String(projectStatusId))
            selector.selectedIndex = i;
    }
    selectForm.append(selectLabel, selector);
    return selectForm;
}

function generateButtonsBlock(yearsCount: number): HTMLDivElement {
    let buttonBlock = <HTMLDivElement>document.createElement('div');
    buttonBlock.classList.add('form-inline', 'mt-2', 'mb-4');
    let addYearButton = <HTMLButtonElement>document.createElement('button');
    addYearButton.classList.add('btn', 'btn-primary');
    addYearButton.textContent = 'Добавить год';
    let removeYearButton = <HTMLButtonElement>document.createElement('button');
    removeYearButton.classList.add('btn', 'btn-primary');
    removeYearButton.textContent = 'Удалить последний год';
    removeYearButton.disabled = yearsCount <= 1;
    removeYearButton.id = 'editRemoveYearButton';
    addYearButton.addEventListener('click', function () {
        let newYearPeriods = [];
        let year = ++currentYear;
        for (let i = 0; i < 12; i++)
            newYearPeriods.push(new Period(Number.NaN, new Date(year, i), []));
        document.getElementById('yearsContainerEdit').appendChild(generateEditYear(newYearPeriods));
        (<HTMLButtonElement>document.getElementById('editRemoveYearButton')).disabled = false;
    });
    removeYearButton.addEventListener('click', function () {
        let removedYear = --currentYear;
        let yearsContainer = document.getElementById('yearsContainerEdit');
        yearsContainer.removeChild(yearsContainer.lastChild);
        if (yearsContainer.childElementCount <= 1)
            (<HTMLButtonElement>document.getElementById('editRemoveYearButton')).disabled = true;
    })


    let addPastYearBtn = <HTMLButtonElement>document.createElement('button');
    addPastYearBtn.classList.add('btn', 'btn-primary');
    addPastYearBtn.textContent = `Добавить ${pastYear} год`;
    addPastYearBtn.id = 'addPastYearBtn';
    addPastYearBtn.addEventListener('click', function () {
        let newYearPeriods = [];
        let year = pastYear;
        pastYear--;
        for (let i = 0; i < 12; i++)
            newYearPeriods.push(new Period(Number.NaN, new Date(year, i), []));
        let yearBlock = generateEditYear(newYearPeriods);
        let yearsContainer = document.getElementById('yearsContainerEdit');
        yearsContainer.insertBefore(yearBlock, yearsContainer.firstChild);
        addPastYearBtn.textContent = `Добавить ${year - 1} год`;
        removePastYearBtn.textContent = `Удалить ${year} год`
        removePastYearBtn.style.display = 'block';
        removePastYearBtn.disabled = false;
    });

    let removePastYearBtn = <HTMLButtonElement>document.createElement('button');
    removePastYearBtn.classList.add('btn', 'btn-primary');
    removePastYearBtn.textContent = `Удалить ${pastYear} год`;
    removePastYearBtn.id = 'removePastYearBtn';
    removePastYearBtn.disabled = true;
    removePastYearBtn.style.display = 'none';
    removePastYearBtn.addEventListener('click', function () {
        let yearsContainer = document.getElementById('yearsContainerEdit');
        yearsContainer.removeChild(yearsContainer.firstChild);
        let pastYearLocal = pastYear;
        console.log(pastYearLocal, '    ', firstProjectYear);
        if (pastYearLocal + 2 === firstProjectYear) {
            pastYear++;
            removePastYearBtn.style.display = 'none';
            removePastYearBtn.disabled = true;
        } else {
            pastYear++;
        }
        addPastYearBtn.textContent = `Добавить ${pastYear} год`;
        removePastYearBtn.textContent = `Удалить ${pastYear + 1} год`;
    });

    buttonBlock.append(addYearButton, removeYearButton, addPastYearBtn, removePastYearBtn);
    return buttonBlock;
}

function generateMonth(period: Period): HTMLDivElement {
    let idBase = `Year${period.date.getFullYear()}Month${period.date.getMonth()}`;
    let container = <HTMLDivElement>document.createElement('div');
    container.id = `edit` + idBase;
    container.classList.add('col-2');

    let nameAndCapacity = <HTMLDivElement>document.createElement('div');
    nameAndCapacity.classList.add('row');

    let monthName = <HTMLDivElement>document.createElement('div');
    monthName.classList.add('col-7', 'font-weight-bold', 'pl-1', 'pr-1');
    monthName.textContent = `${GetMonthName(period.date.getMonth() + 1)} - `;

    let monthCapacity = <HTMLDivElement>document.createElement('div');
    monthCapacity.classList.add('col-5', 'font-weight-bold', 'pl-1', 'pr-1');

    let monthCapacityInput = <HTMLInputElement>document.createElement('input');
    monthCapacityInput.value = String(isNaN(period.capacity) ? '' : period.capacity);
    monthCapacityInput.id = `editMonthCapacity` + idBase;
    monthCapacityInput.type = 'number';
    monthCapacityInput.min = String(0);
    monthCapacityInput.classList.add('w-100');
    monthCapacityInput.placeholder = 'Загрузка';

    monthCapacity.appendChild(monthCapacityInput);
    nameAndCapacity.append(monthName, monthCapacity);

    container.appendChild(nameAndCapacity);

    let resAndCap = <HTMLDivElement>document.createElement('div');
    resAndCap.classList.add('row');
    let res = <HTMLDivElement>document.createElement('div');
    res.classList.add('col-5', 'pl-1', 'pr-1', 'text-left');
    res.textContent = 'Ресурс';
    let cap = <HTMLDivElement>document.createElement('div');
    cap.classList.add('col-7', 'p-0', 'text-right');
    cap.textContent = 'Загрузка, %';
    resAndCap.append(res, cap);

    container.appendChild(resAndCap);
    container.appendChild(generateResourceSelectorWithValue(period));

    let btnContainer = <HTMLDivElement>document.createElement('div');
    container.appendChild(btnContainer);
    btnContainer.classList.add('row', 'pl-1', 'pr-1');

    let addResBtn = <HTMLButtonElement>document.createElement('button');
    addResBtn.classList.add('btn', 'btn-sm', 'btn-outline-dark', 'w-100', 'p-1');
    let removeResBtn = <HTMLButtonElement>addResBtn.cloneNode();

    addResBtn.textContent = 'Добавить человека';
    removeResBtn.textContent = 'Удалить последнего человека';
    removeResBtn.id = `editRemoveResourceButton` + idBase;
    removeResBtn.disabled = period.resources.length === 0;

    addResBtn.addEventListener('click', function () {
        let baseId = idBase;
        let container = document.getElementById('editAddResource' + baseId);
        let row = document.createElement('div');
        row.classList.add('row', 'mb-2');
        container.appendChild(row);
        let selectContainer = <HTMLDivElement>document.createElement('div');
        selectContainer.classList.add('col-9', 'p-0');
        let capacityContainer = <HTMLDivElement>document.createElement('div');
        capacityContainer.classList.add('col-3', 'p-0', 'text-center', 'overflow-hidden');
        container.append(selectContainer, capacityContainer);
        row.append(selectContainer, capacityContainer);
        let select = <HTMLSelectElement>document.createElement('select');
        selectContainer.append(select);
        select.classList.add('custom-select');
        select.id = `editSelectResourceYear` + baseId;
        let capacityInput = <HTMLInputElement>document.createElement('input');
        capacityInput.classList.add('w-100', 'h-100');
        capacityInput.min = '0';
        capacityInput.placeholder = 'Загрузка';
        capacityInput.type = 'number';
        capacityInput.value = '';
        capacityContainer.appendChild(capacityInput);


        let emptyOption = <HTMLOptionElement>document.createElement('option');
        emptyOption.disabled = true;
        emptyOption.selected = true;
        emptyOption.text = 'Выберите человека';
        select.options.add(emptyOption);
        resources.forEach(res => {
            let option = <HTMLOptionElement>document.createElement('option');
            option.value = String(res.id);
            option.text = res.name;
            select.options.add(option);
        });
        (<HTMLButtonElement>document.getElementById('editRemoveResourceButton' + baseId)).disabled = false;
    })

    removeResBtn.addEventListener('click', function () {
        let baseId = idBase;
        let container = document.getElementById('editAddResource' + baseId);
        container.removeChild(container.lastChild);
        if (container.childElementCount === 0)
            (<HTMLButtonElement>document.getElementById('editRemoveResourceButton' + baseId)).disabled = true;
    })
    btnContainer.append(addResBtn, removeResBtn);

    return container;
}

function generateResourceSelectorWithValue(period: Period): HTMLDivElement {
    if (resources.length === 0) {
        let xhr = new XMLHttpRequest();
        xhr.open('GET', '/plan/getResources', false);
        xhr.send();
        let response: ResourceCapacity[] = <ResourceCapacity[]>JSON.parse(xhr.responseText);
        response.forEach(value => {
            resources.push(value);
        })
    }
    let baseId = `Year${period.date.getFullYear()}Month${period.date.getMonth()}`;
    let resContainer = <HTMLDivElement>document.createElement('div');
    resContainer.id = `editAddResource` + baseId;

    period.resources.forEach(value => {
        let row = <HTMLDivElement>document.createElement('div');
        row.classList.add('row', 'mb-2');

        let selectContainer = <HTMLDivElement>document.createElement('div');
        selectContainer.classList.add('col-9', 'p-0');

        let capacityContainer = <HTMLDivElement>document.createElement('div');
        capacityContainer.classList.add('col-3', 'p-0', 'text-center', 'overflow-hidden');
        let capacityInput = <HTMLInputElement>document.createElement('input');
        capacityInput.classList.add('w-100', 'h-100');
        capacityInput.min = '0';
        capacityInput.placeholder = 'Загрузка';
        capacityInput.type = 'number';
        capacityInput.value = String(value.capacity);
        capacityContainer.appendChild(capacityInput);

        row.append(selectContainer, capacityContainer);

        let selectResource = <HTMLSelectElement>document.createElement('select');
        selectResource.classList.add('custom-select');
        selectResource.id = `editSelectResourceYear${period.date.getFullYear()}Month${period.date.getMonth()}`;
        selectContainer.appendChild(selectResource);

        resources.forEach(res => {
            let option = <HTMLOptionElement>document.createElement('option');
            option.value = String(res.id);
            option.text = res.name;
            if (value.id === res.id)
                option.selected = true;
            selectResource.options.add(option);
        });
        for (let i = 0; i < selectResource.options.length; i++) {
            if (String(value.id) === selectResource.options[i].value) {
                selectResource.selectedIndex = i;
            }
        }

        resContainer.appendChild(row);
    })
    return resContainer;
}

function generateEditYear(periods: Period[]): HTMLDivElement {
    if (periods.length != 12) {
        throw 'exception in periods length';
    }
    let yearContainer = <HTMLDivElement>document.createElement('div');
    yearContainer.id = `editYear${periods[1].date.getFullYear()}`;
    yearContainer.classList.add('year');
    let yearName = document.createElement('p');
    yearName.textContent = String(periods[1].date.getFullYear());
    let firstHalfYear = <HTMLDivElement>document.createElement('div');
    let secondHalfYear = <HTMLDivElement>document.createElement('div');
    yearContainer.append(yearName, firstHalfYear, secondHalfYear);

    //first half
    firstHalfYear.id = `year${periods[1].date.getFullYear()}_1`;
    firstHalfYear.classList.add('row', 'pb-3', 'halfYear');
    for (let i = 0; i < 6; i++) {
        firstHalfYear.append(generateMonth(periods[i]));
    }

    //second half
    secondHalfYear.id = `year${periods[1].date.getFullYear()}_2`;
    secondHalfYear.classList.add('row', 'pb-3', 'halfYear');
    for (let i = 6; i < 12; i++) {
        secondHalfYear.append(generateMonth(periods[i]));
    }

    return yearContainer;
}

function generateYearsContainer(periods: Period[]) {
    let periodsLocal = periods.slice();
    let yearsContainer = <HTMLDivElement>document.createElement('div');
    yearsContainer.id = 'yearsContainerEdit';
    let years = periodsLocal.map(period => period.date.getFullYear()).filter((value, index, self) => {
        return self.indexOf(value) === index;
    });
    for (let i = 0; i < years.length; i++) {
        for (let monthNumber = 0; monthNumber < 12; monthNumber++) {
            let currentYear = years[i];
            let arr = periodsLocal.filter(value =>
                (value.date.getFullYear() === currentYear) && (value.date.getMonth() === monthNumber));
            if (arr.length === 0) {
                let period = new Period();
                period.date = new Date(currentYear, monthNumber);
                period.resources = [];
                period.capacity = Number.NaN;
                periodsLocal.push(period);
            }
        }
    }
    periodsLocal.sort((per1, per2) => {
        return per1.date > per2.date ? 1 : -1;
    })
    let sliceStart = 0;
    let sliceEnd = 12;
    for (let i = 0; i < years.length; i++) {
        yearsContainer.appendChild(generateEditYear(periodsLocal.slice(sliceStart, sliceEnd)));
        sliceStart = sliceEnd;
        sliceEnd = sliceEnd + 12;
    }
    return yearsContainer;
}

function getProjectEditedInfo(): Project {
    let project = new Project();
    let projectIdSelect = <HTMLSelectElement>document.getElementById('projectSelector');
    let projectId = parseInt(projectIdSelect.options[projectIdSelect.selectedIndex].value);
    project.id = projectId;

    let projectName = (<HTMLInputElement>document.getElementById('editProjectName')).value;

    let projectStatusSelect = <HTMLSelectElement>document.getElementById('projectStatusEdit');
    let projectStatusId = parseInt(projectStatusSelect.options[projectStatusSelect.selectedIndex].value);
    if (!isNaN(projectStatusId))
        project.statusId = projectStatusId;
    project.name = projectName;
    project.periods = [];
    let yearDivs = document.getElementById('yearsContainerEdit').children;
    for (let i = 0; i < yearDivs.length; i++) {
        let yearDiv = yearDivs[i];
        let year: number = parseInt(yearDiv.firstElementChild.textContent);
        let halfYears = [yearDiv.children[1], yearDiv.children[2]];
        //собираем первое полугодие
        let firstHalfYear = halfYears[0];
        let firstHalfMonths = firstHalfYear.children;
        for (let j = 0; j < firstHalfMonths.length; j++) {
            let monthBlock = firstHalfMonths[j];
            let period = getEditedPeriodInfo(monthBlock);
            project.periods.push(period);
        }

        //собираем второе полугодие
        let secondHalfYear = halfYears[1];
        let secondHalfMonths = secondHalfYear.children;
        for (let j = 0; j < secondHalfMonths.length; j++) {
            let monthBlock = secondHalfMonths[j];
            let period = getEditedPeriodInfo(monthBlock);
            project.periods.push(period);
        }
    }
    return project;
}

function getEditedPeriodInfo(monthBlock: Element): Period {
    let period = new Period();
    let date = monthBlock.id.replace(/\D/g, '_').split('_').filter(d => d !== '').map(d => parseInt(d));
    period.date = new Date(date[0], date[1] + 1);
    period.resources = [];
    let capacity = parseInt((<HTMLInputElement>document
        .getElementById(`editMonthCapacityYear${date[0]}Month${date[1]}`))
        .value);
    //let capacity = parseInt((<HTMLInputElement>monthBlock.firstElementChild.lastElementChild.firstElementChild).value);
    if (isNaN(capacity)) {
        capacity = -1;
    }
    period.capacity = capacity;
    let selects = (<HTMLDivElement>document.getElementById(`editAddResourceYear${date[0]}Month${date[1]}`)).children;
    for (let rowNumber = 0; rowNumber < selects.length; rowNumber++) {
        let selectValuePair = selects[rowNumber];
        let select = <HTMLSelectElement>selectValuePair.firstElementChild.firstElementChild;
        let resourceId = parseInt(select.options[select.selectedIndex].value);
        if (isNaN(resourceId)) {
            resourceId = -1;
        }
        let resourceFullName = select.options[select.selectedIndex].text;
        let value = parseInt((<HTMLInputElement>selectValuePair.lastElementChild.firstElementChild).value);
        if (isNaN(value)) {
            value = -1;
        }
        period.resources.push(new ResourceCapacity(resourceId, resourceFullName, value));

    }
    return period;
}

function sendEditedProject() {
    let project = getProjectEditedInfo();
    if (ValidateForm(project, 'edit')) {
        let btn = <HTMLButtonElement>document.getElementById('editSendProject');
        btn.disabled = true;
        let xhr = new XMLHttpRequest();
        xhr.open('PUT', 'plan/editProject', false);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.send(JSON.stringify(project));
        let success = xhr.responseText;
        if (success === 'true')
            location.reload();
    } else {
        document.getElementById('editErrorHandler').style.display = 'block';
    }
}