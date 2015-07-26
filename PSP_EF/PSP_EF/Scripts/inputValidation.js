function trimAll() {
    document.getElementById('FirstName').value = trim(document.getElementById('FirstName').value);
    document.getElementById('LastName').value = trim(document.getElementById('LastName').value);
    document.getElementById('DateOfBirth').value = trim(document.getElementById('DateOfBirth').value);
    document.getElementById('Suburb').value = trim(document.getElementById('Suburb').value);
    document.getElementById('Address').value = trim(document.getElementById('Address').value);
    document.getElementById('Description').value = trim(document.getElementById('Description').value);
    document.getElementById('Email').value = trim(document.getElementById('Email').value);
    document.getElementById('Phone').value = trim(document.getElementById('Phone').value);
}

function trimEdit() {
    document.getElementById('FirstName').value = trim(document.getElementById('FirstName').value);
    document.getElementById('LastName').value = trim(document.getElementById('LastName').value);
    document.getElementById('DateOfBirth').value = trim(document.getElementById('DateOfBirth').value);
    document.getElementById('Suburb').value = trim(document.getElementById('Suburb').value);
    document.getElementById('Address').value = trim(document.getElementById('Address').value);
    document.getElementById('Description').value = trim(document.getElementById('Description').value);
    document.getElementById('Phone').value = trim(document.getElementById('Phone').value);
}

function validate() {
    trimAll();
    var FirstName = document.getElementById('FirstName');
    var LastName = document.getElementById('LastName');
    var DateOfBirth = document.getElementById('DateOfBirth');
    var Suburb = document.getElementById('Suburb');
    var Address = document.getElementById('Address');
    var Description = document.getElementById('Description');
    var Email = document.getElementById('Email');
    var Password = document.getElementById('Password');
    var PasswordConfirmation = document.getElementById('PasswordConfirmation');
    var Phone = document.getElementById('Phone');

    if (Password.value != PasswordConfirmation.value) {
        alert("Password and password confirmation does not match.")
        return false;
    }
    //alert("DOB : " + DateOfBirth.value);
    if (isAlphanumeric(FirstName, "Numbers and letters only for your first name")) {
        if (isAlphanumeric(LastName, "Numbers and letters Only for your last name")) {
            if (isEmpty(Email, "Please enter your email adrress")) {
                if (isValidEmail(Email, "Please enter your valid email adrress")) {
                    if (isEmpty(Password, "Please enter your password")) {
                        if (isNumeric(Phone, "Numbers only for phone number")) {
                            return true;
                        }
                    }
                }
            }
        }
    }
    return false;
}

function validateEditForm() {
    trimEdit();
    var FirstName = document.getElementById('FirstName');
    var LastName = document.getElementById('LastName');
    var Phone = document.getElementById('Phone');
    //alert("DOB : " + DateOfBirth.value);
    if (isAlphanumeric(FirstName, "Numbers and letters only for your first name")) {
        if (isAlphanumeric(LastName, "Numbers and letters only for your last name")) {
            if (isNumeric(Phone, "Numbers only for phone number")) {
                return true;
            }
        }
    }
    return false;
}

function confirmation() {
    var r = confirm("Are you sure you want to unsubscribe?");
    if (r == true)
    { return true; }
    else
    { return false; }
}

function validateNumber() {
    document.getElementById('Price').value = trim(document.getElementById('Price').value);
    var Price = document.getElementById('Price');
    if (isEmpty(Price, "Please fill in the price.")) {
        if (isNumeric2(Price, "Numbers only for price")) {
            return true;
        }
    }
    return false;
}

function validateNumberRange() {
    document.getElementById('fromPrice').value = trim(document.getElementById('fromPrice').value);
    document.getElementById('toPrice').value = trim(document.getElementById('toPrice').value);
    var fromPrice = document.getElementById('fromPrice');
    var toPrice = document.getElementById('toPrice');

    if (isNumeric(fromPrice, "Numbers only for minimum price")) {
        if (isNumeric(toPrice, "Numbers only for maximum price")) {
            return true;
        }
    }

    return false;
}

function validatePetComment() {
    document.getElementById('Comment').value = trim(document.getElementById('Comment').value);
    var comment = document.getElementById('Comment');
    if (isEmpty(comment, "Please fill in the description.")) {
        return true;
    }
    return false;
}

function validateTrackReport() {
    var reportId = document.getElementById('LostFoundId');
    if (isNumeric2(reportId, "Numbers only for Report ID.")) {
        return true;
    }
    return false;
}

function transferEditorToDescription() {
    document.getElementById('Description').value = $('#editor').cleanHtml();
    return true;
}