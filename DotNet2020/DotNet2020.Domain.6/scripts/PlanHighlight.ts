function HighlightProjectsResource_Capacity() {
    let blocks = document.getElementsByClassName('resource_capacity');
    for (let i = 0; i < blocks.length; i++) {
        let pair = blocks[i];
        let plannedCell = <HTMLTableDataCellElement>pair.firstChild;
        let currentCell = <HTMLTableDataCellElement>pair.lastChild;
        let planned = parseInt(plannedCell.textContent);
        let current = parseInt(currentCell.textContent);
        if (!isNaN(planned) && !isNaN(current)) {
            if (planned === current) {
                plannedCell.style.backgroundColor
            }
        }
    }
}