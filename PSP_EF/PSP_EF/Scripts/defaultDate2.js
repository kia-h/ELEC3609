function defaultDate(txtBoxName,txtBoxName2) { 
   var oDate = new Date();
   oDate.setDate(oDate.getDate()-1);
   var sDate = (oDate.getMonth()+1)+ "/" + 
       oDate.getDate() + "/" + oDate.getYear()
   document.forms[0][txtBoxName].value=sDate
   document.forms[0][txtBoxName2].value=sDate
}