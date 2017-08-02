$(document).ready(function () {
    obtenerUltimoValor('#txPrecioEntrada', '.textEmpresa', calcularCantidad);
    obtenerUltimoValor('.textSalida', '.textEmpresa');

    $('.textSalida').change(function () {
        actualizarGanancia($(this));
    });

    $('#txPrecioEntrada').change(function () {
        calcularCantidad();
        actualizarGanancia($(this).parent().parent().find('.textSalida'));
    }); 

    $('#txCantidad').change(function () {
        actualizarGanancia($(this).parent().parent().find('.textSalida'));
        calcularInversion();
    });

    $('#txEmpresa').change(function () {
        obtenerUltimoValor('#txPrecioEntrada', '#txEmpresa');
        obtenerUltimoValor('#txPrecioSalida', '#txEmpresa',calcularCantidad);       
    }); 

    $('#recargarPrecios').click(function () {
        obtenerUltimoValor('#txPrecioEntrada', '#txEmpresa');
        obtenerUltimoValor('#txPrecioSalida', '#txEmpresa', calcularCantidad);
    });

    actualizarColorGanancia();
    $('.campoNumerico').keydown(function (event) {
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 16 || event.keyCode == 39 || event.keyCode == 37 || event.keyCode == 46 || event.keyCode == 13) {
        } else {
            if (event.keyCode == 190 || event.keyCode == 188 || event.keyCode == 110) {
                $(this).val($(this).val() + '.');
                event.preventDefault();
            }
            else if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105))) {
                event.preventDefault();
            }
        }
    });

    $("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
        $("#success-alert").alert('close');
    });
});

function obtenerUltimoValor(textboxSalida, empresa,func) {
        $(textboxSalida).each(function () {
            var salida = $(this);
            $.getJSON('Home/ObtenerUltimoValor', { empresa: gval(salida.parent().parent().find(empresa)) }, function (data) {
                salida.val(data.data);
                if (func != null) func();
            }).complete(function (data) {
                actualizarGanancia(salida);
            });
        });
}

function actualizarGanancia(campo) {
    var textGanancia = campo.parent().parent().find('.textGanancia');
    var textPorcentaje = campo.parent().parent().find('.textPorcentaje');

    var entrada = gval(campo.parent().parent().find('.textEntrada'));
    var salida = campo.val();
    var cantidad = gval(campo.parent().parent().find('.textCantidad'));

    var inversion = gval(campo.parent().parent().find('.textInversion'));

    var impuestos = 100;
    if (inversion < 3000) {
        impuestos += 30;
    } else {
        impuestos += inversion * 2 / 100;
    }


    textGanancia.html(Math.round(cantidad * (salida - entrada) - impuestos));
    textPorcentaje.html(((cantidad * (salida - entrada) - impuestos) * 100 / (cantidad * (entrada) + impuestos)).toFixed(2) + '%');
    actualizarColorGanancia();
}

function actualizarColorGanancia() {
    $('.textGanancia').each(function () {
        var ganancia = $(this);
        if ($.isNumeric(ganancia.html()) && ganancia.html() > 40) {
            ganancia.removeClass('warning');
            ganancia.removeClass('danger');
            ganancia.addClass('success');
        }
        if ($.isNumeric(ganancia.html()) && ganancia.html() > 0 && ganancia.html() <= 40) {
            ganancia.addClass('warning');
            ganancia.removeClass('danger');
            ganancia.removeClass('success');
        }
        if ($.isNumeric(ganancia.html()) && ganancia.html() <= 0) {
            ganancia.removeClass('warning');
            ganancia.addClass('danger');
            ganancia.removeClass('success');
        }
    });
}

function calcularCantidad() {
    $('#txCantidad').val(Math.round($('#txInversion').val() / $('#txPrecioEntrada').val()));
}
function calcularInversion() {
    $('#txInversion').val(Math.round($('#txCantidad').val() * $('#txPrecioEntrada').val()));
}

function gval(x) {
    var y = x.html();
    if (y === '') y = x.val();
    return y;
}