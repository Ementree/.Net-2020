function selectProject() {
    var select = document.getElementById('projectSelector');
    var projectId = parseInt(select.options[select.selectedIndex].value);
    getProject(projectId);
}
function getProject(id) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', "getProjectPlanById/" + id);
    xhr.send();
    console.log(xhr.responseText);
}
//# sourceMappingURL=EditProject.js.map