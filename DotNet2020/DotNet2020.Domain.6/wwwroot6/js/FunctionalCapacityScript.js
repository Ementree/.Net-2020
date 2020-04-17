document.addEventListener("DOMContentLoaded", function () {
    console.log("doom загрузился");
    var selector = document.getElementById("changeYearSelector");
    selector.addEventListener("change", function () {
        var form = document.getElementById("changeYearForm");
        console.log("Кто-то сменил год...");
        var value = selector.value;
        console.log(value);
        console.log(window.location.search);
        window.location.search = ("?currentYear=" + value);
        console.log(window.location.search);
    });
});
//# sourceMappingURL=FunctionalCapacityScript.js.map