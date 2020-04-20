function handle(object){
    let inp = document.createElement("input");
    inp.setAttribute('size', '7');
    inp.type = "text";
    inp.value = object.innerText.slice(0, -1);

    object.innerText = "";
    object.appendChild(inp);

    inp.onkeydown = function(e){

        if(e.keyCode === 13){
            if(inp.value === "") inp.value = "0";
            if (!isNaN(parseInt(inp.value))) {
                inp.value = String(parseInt(inp.value));
            } else {
                inp.value = "0"
            }
            if(parseInt(inp.value) < 0) inp.value = "0";
            
            let xhr = new XMLHttpRequest();
            xhr.open('POST', '/changeCapacity', false);
            xhr.setRequestHeader('Content-type', 'application/json');
            xhr.send(JSON.stringify(object.id + ';' + inp.value));
            
            object.innerText = inp.value + '%';
            object.removeChild(inp);
        }
    };
}

