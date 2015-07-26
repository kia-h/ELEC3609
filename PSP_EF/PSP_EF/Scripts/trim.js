function trim(argvalue) {
  var tmpstr = ltrim(argvalue);

  return rtrim(tmpstr);
}

function ltrim(argvalue) {

  while (1) {
    if (argvalue.substring(0, 1) != " ")
      break;
    argvalue = argvalue.substring(1, argvalue.length);
  }

  return argvalue;
}

function rtrim(argvalue) {

  while (1) {
    if (argvalue.substring(argvalue.length - 1, argvalue.length) != " ")
      break;
    argvalue = argvalue.substring(0, argvalue.length - 1);
  }

  return argvalue;
}