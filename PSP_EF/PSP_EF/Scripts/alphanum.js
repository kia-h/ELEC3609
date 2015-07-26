function isAlphanumeric(elem,message){
	var alphaExp = /^[a-zA-Z0-9- ]+$/;
	if(elem.value.match(alphaExp)){
		return true;
	}else{
		alert(message);
		elem.focus();
		return false;
	}
}