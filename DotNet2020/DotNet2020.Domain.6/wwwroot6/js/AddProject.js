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
    console.log(lastYear);
    var yearContainer = document.getElementById('yearsContainer');
    var yearDiv = document.createElement('div');
    yearDiv.id = "year" + lastYear;
    yearDiv.classList.add('row', 'year');
    for (var i = 0; i < 12; i++) {
        var monthCol = document.createElement('div');
        monthCol.id = "month" + (i + 1);
        monthCol.classList.add('col-1');
    }
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
//# sourceMappingURL=AddProject.js.map