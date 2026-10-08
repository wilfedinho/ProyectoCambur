function aplicarMascaraDni(input) {

    var digitos = input.value.replace(/\D/g, "").substring(0, 8);
    var resultado = digitos;

    if (digitos.length > 5) {
        resultado = digitos.substring(0, 2) + "." + digitos.substring(2, 5) + "." + digitos.substring(5);
    } else if (digitos.length > 2) {
        resultado = digitos.substring(0, 2) + "." + digitos.substring(2);
    }

    input.value = resultado;
}

function aplicarMascaraTelefono(input) {
    var digitos = input.value.replace(/\D/g, "").substring(0, 10);
    var resultado = digitos;

    if (digitos.length > 6) {
        resultado = digitos.substring(0, 2) + "-" + digitos.substring(2, 6) + "-" + digitos.substring(6);
    } else if (digitos.length > 2) {
        resultado = digitos.substring(0, 2) + "-" + digitos.substring(2);
    }

    input.value = resultado;
}