﻿document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился")

    let selector = <HTMLSelectElement> document.getElementById("changeYearSelector");

    selector.addEventListener("change", function () {
        let form = <HTMLFormElement>document.getElementById("changeYearForm");
        console.log("Кто-то сменил год...");
        let value = selector.value;
        console.log(value);
        console.log(window.location.search);
        window.location.search = ("?currentYear=" + value);
        console.log(window.location.search);
    });
});