

function GetInputs() {
    // Get the form element
    const form = document.querySelector('form');

    // Get the specific div within the form
    const specificDiv = form.querySelector('.apply-component');

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

export function areInputsValid(formId) {
    var form = document.getElementById(formId);
    if (!form || !form.checkValidity) {
        return false;
    }

    var inputsToCheck = GetInputs();

    if (inputsToCheck.length > 0) {
        return false;
    }

    return true;
}

