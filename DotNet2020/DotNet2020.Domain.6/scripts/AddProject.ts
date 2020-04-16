class Project {
    Name: string;
    Periods: Period[];
}

class Period {
    Date: Date;
    Resources: ResourceCapacity[];
}

class ResourceCapacity {
    constructor(id: number, name: string, capacity: number) {
        this.Id = id;
        this.Name = name;
        this.Capacity = capacity;
    }

    Id: number;
    Name: string;
    Capacity: number;
}

let lastYear: number = new Date(Date.now()).getFullYear();

function AddYear() {
    lastYear++;
    const localYear = lastYear;
    console.log(localYear);
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
        const monthBlock = GenerateMonth(localYear, i + 1);
        firstHalfYearDiv.appendChild(monthBlock);
    }

    //второе полугодие
    let secondHalfYearDiv = document.createElement('div');
    secondHalfYearDiv.classList.add('row', 'pb-3', 'halfYear');
    secondHalfYearDiv.id = `year${localYear}_2`;
    for (let i = 6; i < 12; i++) {
        const monthBlock = GenerateMonth(localYear, i + 1);
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

function GenerateMonth(localYear: number, monthNumber: number): Element {
    const monthBlock = <Element>(document
        .getElementById(`year${localYear - 1}Month${monthNumber}`)
        .cloneNode(true));
    monthBlock.id = `year${localYear}Month${monthNumber}`;
    console.log(monthBlock);
    let addRes = monthBlock.children[2];
    addRes.id = `addResourceYear${localYear}Month${monthNumber}`;
    while (addRes.childElementCount > 1) {
        addRes.removeChild(addRes.children[addRes.childElementCount - 1])
    }
    //смена айди у селекта
    let select = addRes.children[0].children[0].children[0];
    select.id = `selectYear${localYear}Month${monthNumber}`;

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
    console.log(deleteButton);
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

function SendProject() {
    let project = new Project();
    let projectName = (<HTMLInputElement>document.getElementById('projectName')).value;
    project.Name = projectName;
    project.Periods = [];
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
            let period = new Period();
            let date = monthBlock.id.replace(/\D/g,'_').split('_').filter(d=>d!=='').map(d=>parseInt(d));
            period.Date = new Date(date[0], date[1] - 1);
            period.Resources = [];
            let selects = monthBlock.children[2].children;
            for (let rowNumber = 0; rowNumber < selects.length; rowNumber++) {
                let selectValuePair = selects[rowNumber];
                let select = <HTMLSelectElement>selectValuePair.firstElementChild.firstElementChild;
                let resourceId = select.options[select.selectedIndex].value;
                let resourceFullName = select.options[select.selectedIndex].text;
                let value = parseInt(selectValuePair.lastElementChild.textContent);
                if(resourceId.trim()!==""){
                    period.Resources.push(new ResourceCapacity(parseInt(resourceId), resourceFullName, value));
                }
            }
            //Todo: поговорить про пустые периоды
            project.Periods.push(period);
        }
    }
    console.log(project);
}