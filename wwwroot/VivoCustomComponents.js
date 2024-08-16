

function GetInputs(id) {
    // Get the specific div within the form
    const specificDiv = document.getElementById(id);

    // Get all input elements inside the specific div
    const inputsInsideDiv = specificDiv.querySelectorAll('.invalid');

    // Return the array of input names
    return inputsInsideDiv;
}

export function areInputsValid(id) {

    var inputsToCheck = GetInputs(id);

    if (inputsToCheck.length > 0) {
        return false;
    }

    return true;
}

export function CheckEveryValidation(isvalid, id) {
    // Get the specific div within the form
    const specificDiv = document.getElementById(id);
    if (isvalid) {
        specificDiv.classList.add('all-valid');
    } else {
        specificDiv.classList.remove('all-valid');
    }
}

export function Executedotnetfunction(dotNetObject, value) {
    dotNetObject.invokeMethodAsync('ChangeBooleanValue', value);
}
