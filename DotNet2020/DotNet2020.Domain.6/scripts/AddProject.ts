let lastYear: number = new Date(Date.now()).getFullYear();

function AddYear() {
    lastYear++;
    const localYear = lastYear;
    let yearsContainer = document.getElementById('yearsContainer');
    let yearDiv: HTMLDivElement = document.createElement('div');
    yearDiv.id = `year${localYear}`;
    yearDiv.classList.add('year');

    let yearParagraph = document.createElement('p');
    yearParagraph.textContent = `${localYear}`;
    yearParagraph.classList.add('font-weight-bold');
    yearDiv.appendChild(yearParagraph);

    //первое полугодие
    let firstHalfYearDiv = document.createElement('div');
    firstHalfYearDiv.classList.add('row', 'pb-3', 'halfYear');
    firstHalfYearDiv.id = `year${localYear}_1`;
    for (let i = 0; i < 6; i++) {
        const monthBlock = generateMonthForNewYear(localYear, i + 1);
        firstHalfYearDiv.appendChild(monthBlock);
    }

    //второе полугодие
    let secondHalfYearDiv = document.createElement('div');
    secondHalfYearDiv.classList.add('row', 'pb-3', 'halfYear');
    secondHalfYearDiv.id = `year${localYear}_2`;
    for (let i = 6; i < 12; i++) {
        const monthBlock = generateMonthForNewYear(localYear, i + 1);
        secondHalfYearDiv.appendChild(monthBlock);
    }

    yearDiv.appendChild(firstHalfYearDiv);
    yearDiv.appendChild(secondHalfYearDiv);
    yearsContainer.appendChild(yearDiv);

    (<HTMLInputElement>document.getElementById('removeYearButton')).disabled = false;
}

function RemoveLastYear() {
    if (lastYear === new Date(Date.now()).getFullYear() + 1) {
        (<HTMLInputElement>document.getElementById('removeYearButton')).disabled = true;
    }
    let lastYearDiv = document.getElementById(`year${lastYear}`);
    document.getElementById('yearsContainer').removeChild(lastYearDiv);
    lastYear--;
}

function generateMonthForNewYear(localYear: number, monthNumber: number): Element {
    const monthBlock = <Element>(document
        .getElementById(`year${localYear - 1}Month${monthNumber}`)
        .cloneNode(true));
    monthBlock.id = `year${localYear}Month${monthNumber}`;
    let addRes = monthBlock.children[2];
    addRes.id = `addResourceYear${localYear}Month${monthNumber}`;

    let monthCapacity = <HTMLInputElement>monthBlock.children[0].children[1].children[0];
    monthCapacity.id = `monthlyCapacityYear${localYear}Month${monthNumber}`;
    monthCapacity.value = '';

    while (addRes.childElementCount > 1) {
        addRes.removeChild(addRes.children[addRes.childElementCount - 1])
    }
    //смена айди у селекта
    let select = addRes.children[0].children[0].children[0];
    select.id = `selectYear${localYear}Month${monthNumber}`;
    (<HTMLInputElement>addRes.children[0].children[1].children[0]).value = '';
    let buttonBlock = monthBlock.children[monthBlock.childElementCount - 1];
    while (buttonBlock.childElementCount > 0) {
        buttonBlock.removeChild(buttonBlock.children[buttonBlock.childElementCount - 1]);
    }

    let addButton = document.createElement('button');
    addButton.classList.add('btn', 'btn-sm', 'btn-outline-dark', 'w-100', 'p-1');
    addButton.textContent = 'Добавить человека';
    let removeButton = <HTMLInputElement>addButton.cloneNode(true);
    removeButton.id = `RemoveButtonYear${localYear}Month${monthNumber}`;
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

function AddResource(year: number, month: number) {
    let deleteButton = <HTMLInputElement>document.getElementById(`RemoveButtonYear${year}Month${month}`);
    let monthElem = document.getElementById(`addResourceYear${year}Month${month}`);
    let select = (<HTMLElement>monthElem.children[0]).cloneNode(true);
    monthElem.appendChild(select);
    deleteButton.disabled = false;
}

function RemoveResource(year: number, month: number) {
    let monthElem = document.getElementById(`addResourceYear${year}Month${month}`);
    let select = (<HTMLElement>monthElem.children[monthElem.children.length - 1]);
    monthElem.removeChild(select);
    if (monthElem.childElementCount === 1) {
        let deleteButton = <HTMLInputElement>document.getElementById(`RemoveButtonYear${year}Month${month}`);
        deleteButton.disabled = true;
    }
}

function GetMonthName(number: number): string {
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


function GetPeriodInfo(monthBlock: Element): Period {
    let period = new Period();
    let date = monthBlock.id.replace(/\D/g, '_').split('_').filter(d => d !== '').map(d => parseInt(d));
    period.date = new Date(date[0], date[1]);
    period.resources = [];
    let capacity = parseInt((<HTMLInputElement>monthBlock.firstElementChild.lastElementChild.firstElementChild).value);
    if (isNaN(capacity)) {
        capacity = -1;
    }
    period.capacity = capacity;
    let selects = monthBlock.children[2].children;
    for (let rowNumber = 0; rowNumber < selects.length; rowNumber++) {
        let selectValuePair = selects[rowNumber];
        let select = <HTMLSelectElement>selectValuePair.firstElementChild.firstElementChild;
        let resourceId = select.options[select.selectedIndex].value;
        let resourceFullName = select.options[select.selectedIndex].text;
        let value = parseInt((<HTMLInputElement>selectValuePair.lastElementChild.firstElementChild).value);
        if (isNaN(value)) {
            value = -1;
        }
        if (resourceId.trim() !== "") {
            period.resources.push(new ResourceCapacity(parseInt(resourceId), resourceFullName, value));
        }
    }
    return period;
}


function GetProjectInfo(): Project {
    let project = new Project();
    let projectName = (<HTMLInputElement>document.getElementById('projectName')).value;

    let projectStatusSelect = <HTMLSelectElement>document.getElementById('projectStatus');
    let projectStatusId = parseInt(projectStatusSelect.options[projectStatusSelect.selectedIndex].value);
    if (!isNaN(projectStatusId))
        project.statusId = projectStatusId;
    project.name = projectName;
    project.periods = [];
    let yearDivs = document.getElementById('yearsContainer').children;
    for (let i = 0; i < yearDivs.length; i++) {
        let yearDiv = yearDivs[i];
        let year: number = parseInt(yearDiv.firstElementChild.textContent);
        let halfYears = [yearDiv.children[1], yearDiv.children[2]];
        //собираем первое полугодие
        let firstHalfYear = halfYears[0];
        let firstHalfMonths = firstHalfYear.children;
        for (let j = 0; j < firstHalfMonths.length; j++) {
            let monthBlock = firstHalfMonths[j];
            let period = GetPeriodInfo(monthBlock);
            project.periods.push(period);
        }

        //собираем второе полугодие
        let secondHalfYear = halfYears[1];
        let secondHalfMonths = secondHalfYear.children;
        for (let j = 0; j < secondHalfMonths.length; j++) {
            let monthBlock = secondHalfMonths[j];
            let period = GetPeriodInfo(monthBlock);
            project.periods.push(period);
        }
    }
    return project;
}

function SendProjectToDb() {
    let project = GetProjectInfo();
    if (ValidateForm(project)) {
        let xhr = new XMLHttpRequest();
        xhr.open('PUT', 'plan/addProject', false);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.send(JSON.stringify(project));
        let success = xhr.responseText;
        if (success === 'true')
            location.reload();
    }
    else{
        document.getElementById('errorHandler').style.display = 'block';
    }
}

function ValidateForm(project: Project, additionalPrefix = ''): boolean {
    let flag = true;
    project.periods.forEach(elem => {
        let length = elem.resources.length;
        let lengthDistinct = elem.resources.map(res => res.id).filter((value, index, self) => {
            return self.indexOf(value) === index;
        }).length;
        if (length > lengthDistinct) {
            flag = false;
            let addResBlock = document
                .getElementById(`${additionalPrefix}addResourceYear${elem.date.getFullYear()}Month${elem.date.getMonth()}`);
            let selects = addResBlock.children;
            for(let i = 0; i<selects.length;i++){
                let elem = <HTMLDivElement>selects[i];
                elem.style.border = '0.5px solid red';
                elem.style.padding = '1px';
            }
        }
    })
    return flag;
}