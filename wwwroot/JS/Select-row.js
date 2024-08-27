$(document).ready(function () {
    debugger
    var elements = document.querySelectorAll('fluent-data-grid-row');

    elements.forEach((item) => {
        item.addEventListener("click", function () {
            if (this.classList.contains("selected-row")) {
                this.classList.remove("selected-row");
            } else {
                this.classList.add("selected-row");
            }
        });
    });
});
//function Addlistener() {
//    var elements = document.querySelectorAll('fluent-data-grid-row');

//    elements.forEach((item) => {
//        item.addEventListener('click', function () {
//            if (this.classList.contains('selected-row')) {
//                this.classList.remove('selected-row');
//            } else {
//                this.classList.add('selected-row');
//            }
//        });
//    });
//}
