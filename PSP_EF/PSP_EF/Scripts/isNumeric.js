function isNumeric(elem, helperMsg){
	var numericExpression = /^[0-9]+$/;
	if(elem.value.match(numericExpression) || elem.value == ""){
		return true;
	}else{
		alert(helperMsg);
		return false;
	}
}

function isNumeric2(elem, helperMsg) {
    var numericExpression = /^[0-9]+$/;
    if (elem.value.match(numericExpression)) {
        return true;
    } else {
        alert(helperMsg);
        return false;
    }
}
