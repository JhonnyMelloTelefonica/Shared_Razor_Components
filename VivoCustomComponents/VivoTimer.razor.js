export function UpdateTimerValue(id, value, background) {
    console.log(value, background);
    let element = document.querySelector("#" + id + " .filtering #donut");
    element.style.background = 'conic-gradient(' + background + ' 0deg ' + value + 'deg, #f7f7f7 ' + value + 'deg 360deg)'
}