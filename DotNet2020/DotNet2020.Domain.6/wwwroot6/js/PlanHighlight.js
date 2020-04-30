function HighlightProjectsResource_Capacity() {
    var blocks = document.getElementsByClassName('resource_capacity');
    for (var i = 0; i < blocks.length; i++) {
        var pair = blocks[i];
        var plannedCell = pair.firstChild;
        var currentCell = pair.lastChild;
        var planned = parseInt(plannedCell.textContent);
        var current = parseInt(currentCell.textContent);
        if (!isNaN(planned) && !isNaN(current)) {
            if (planned === current) {
                plannedCell.style.backgroundColor;
            }
        }
    }
}
//# sourceMappingURL=PlanHighlight.js.map