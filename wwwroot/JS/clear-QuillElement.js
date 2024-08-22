export function ClearQuill() {
    var elements = document.querySelector(".Answer-problema .ql-editor")
    elements.children[0].innerHTML = '';
}
export function FocusBadgeElement(element, id) {
    var element = document.getElementById("elementbadge-" + id)
    element.classList.add("badge-focused")
}
export function RemoveFocusBadgeElement(element, id) {
    var element = document.getElementById("elementbadge-" + id)
    element.classList.remove("badge-focused")
}