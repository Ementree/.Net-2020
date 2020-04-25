function selectProject() {
    let select = <HTMLSelectElement>document.getElementById('projectSelector');
    let projectId = parseInt(select.options[select.selectedIndex].value);
    getProject(projectId);
}

function getProject(id: number) {
    let xhr = new XMLHttpRequest();
    xhr.open('GET', `getProjectPlanById/${id}`);
    xhr.send();
    console.log(xhr.responseText);
}