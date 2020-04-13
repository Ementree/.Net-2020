class Project {
    Id: number;
    Name: string;
    Periods: Period[];
}
class Period {
    Id: number;
    Date: Date;
    Resources: ResourceCapacity[]; 
}
class ResourceCapacity{
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
    console.log(lastYear);
    let yearContainer = document.getElementById('yearsContainer');
    let yearDiv: HTMLDivElement = document.createElement('div');
    yearDiv.id = `year${lastYear}`;
    yearDiv.classList.add('row', 'year');
    
    for(let i = 0; i<12;i++) {
        let monthCol = document.createElement('div');
        monthCol.id = `month${i+1}`;
        monthCol.classList.add('col-1');
        
    }
    
}

function AddResource(year: number, month:number) {
    console.log(year, month);
    let monthElem = document.getElementById(`year${year}month${month}`);
    
}