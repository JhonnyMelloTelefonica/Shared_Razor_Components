

function GetInputs(id) {
    // Get the specific div within the form
    const specificDiv = document.getElementById(id);

    // Get all input elements inside the specific div
    const inputsInsideDiv = specificDiv.querySelectorAll('input.invalid');

    const inputNames = [];

    // Loop through each valid input element
    inputsInsideDiv.forEach((input) => {
        // Get the 'name' attribute of the input
        const inputName = input.getAttribute('name');
        if (inputName) {
            inputNames.push(inputName);
        }
    });

    // Return the array of input names
    return inputNames;
}

export function areInputsValid(id) {

    var inputsToCheck = GetInputs(id);

    if (inputsToCheck.length > 0) {
        return false;
    }

    return true;
}

