var Project = (function () {
    function Project() {
    }
    return Project;
}());
var Period = (function () {
    function Period(capacity, date, resources) {
        this.capacity = capacity;
        this.date = date;
        this.resources = resources;
    }
    return Period;
}());
var ResourceCapacity = (function () {
    function ResourceCapacity(id, name, capacity) {
        this.id = id;
        this.name = name;
        this.capacity = capacity;
    }
    return ResourceCapacity;
}());
var ProjectStatus = (function () {
    function ProjectStatus(id, status) {
        this.id = id;
        this.status = status;
    }
    return ProjectStatus;
}());
//# sourceMappingURL=PlanModels.js.map