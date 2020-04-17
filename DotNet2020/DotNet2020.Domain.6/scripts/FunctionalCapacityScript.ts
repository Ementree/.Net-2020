document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился")

    let selector = document.getElementById("changeYearSelector");

    selector.addEventListener("change", function () {
        let form = <HTMLFormElement>document.getElementById("changeYearForm");
        console.log("Кто-то сменил год...");
        form.submit();
    });
});