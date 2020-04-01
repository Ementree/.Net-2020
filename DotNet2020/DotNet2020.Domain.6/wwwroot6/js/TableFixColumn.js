document.querySelector('.table-scroll').addEventListener('scroll', function (e) {
    this.querySelector('.thead-col').style.left = this.scrollLeft + "px";
    this.querySelector('.thead-row').style.top = this.scrollTop + "px";
});