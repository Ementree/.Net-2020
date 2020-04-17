document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился");
    var selector = document.getElementById("changeYearSelector");
    selector.addEventListener("change", function () {
        var form = document.getElementById("changeYearForm");
        console.log("Кто-то сменил год...");
        form.submit();
    });
});
//# sourceMappingURL=FunctionalCapacityScript.js.map