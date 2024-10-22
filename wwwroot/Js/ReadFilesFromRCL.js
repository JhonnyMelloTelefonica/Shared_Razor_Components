export function readTextFile(file, dotNetObject) {
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", file, false);
    rawFile.onreadystatechange = function () {
        if (rawFile.readyState === 4) {
            if (rawFile.status === 200 || rawFile.status == 0) {
                var allText = rawFile.responseText;
                dotNetObject.invokeMethodAsync('FetchData', allText);
                console.log(allText);
            }
        }
    }

    rawFile.send(null);
}