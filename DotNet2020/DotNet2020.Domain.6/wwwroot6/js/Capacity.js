function handle(object) {
    var inp = document.createElement("input");
    inp.setAttribute('size', '7');
    inp.type = "text";
    inp.value = object.innerText.slice(0, -1);
    object.innerText = "";
    object.appendChild(inp);
    var _event = object.onclick;
    object.onclick = null;
    inp.onkeydown = function (e) {
        if (e.keyCode === 13) {
            if (inp.value === "")
                inp.value = "0";
            if (!isNaN(parseInt(inp.value))) {
                inp.value = String(parseInt(inp.value));
            }
            else {
                inp.value = "0";
            }
            if (parseInt(inp.value) > 100)
                inp.value = "100";
            if (parseInt(inp.value) < 0)
                inp.value = "0";
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/changeCapacity', false);
            xhr.setRequestHeader('Content-type', 'application/json');
            xhr.send(JSON.stringify(object.id + ';' + inp.value));
            object.innerText = inp.value + '%';
            object.onclick = _event;
            object.removeChild(inp);
        }
    };
}
//# sourceMappingURL=Capacity.js.map