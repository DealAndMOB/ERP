let alertaActual = null;
function AlertaAGC(titulo, mensaje, tipo) {
    //Condicion si esta una alerta en pantalla para no sobre poner otra
    if (alertaActual) {
        alertaActual.remove();
    }
    //Crea contenedor de la alerta
    const alerta = document.createElement("div");
    alerta.classList.add("alerta", tipo);

    //Icono de la alerta
    const icono = document.createElement("div");
    const contenedor = document.createElement("div");
    icono.classList.add("icono")
    icono.appendChild(contenedor);
    if (tipo === "error") {
        contenedor.classList.add("equis", "rounded-circle", "mb-2", "iconos");
    }
    else if (tipo === "success") {
        contenedor.classList.add("exit", "rounded-circle", "mb-2", "iconos");
    }
    else {
        contenedor.classList.add("exclamation", "rounded-circle", "mb-2", "iconos");
    }
    alerta.appendChild(icono);


    //Titulo

    const tituloAlerta = document.createElement("h1");
    tituloAlerta.innerText = titulo;
    alerta.appendChild(tituloAlerta);

    //mensaje
    const mensajeAlerta = document.createElement("p");
    mensajeAlerta.classList.add("mt-2", "mb-4");
    mensajeAlerta.innerText = mensaje;
    alerta.appendChild(mensajeAlerta);

    const cont = document.createElement("div");
    cont.classList.add("text-end");
    alerta.appendChild(cont);

    const btnOk = document.createElement("button");
    btnOk.classList.add("btnOk");
    btnOk.textContent = "Ok";

    btnOk.addEventListener("click", () => {
        alerta.remove();
    });
    cont.appendChild(btnOk);
    //Se agrega la alerta al body
    document.body.appendChild(alerta);
    alertaActual = alerta;
}

// Errores - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
function RemisionSinFolio() {
    AlertaAGC("Error", "¡Genera el folio de la remisión!", "error");
    return true;
}

function ErrorCaracteres() {
    AlertaAGC("Error", "¡Solo se aceptan números y letras!", "error");
    return true;
}

function RegistroCotizacion() {
    AlertaAGC("Error", "No existen productos en la cotización", "error");
    return true;
}

function SinCliente() {
    AlertaAGC("Error", "Seleccione un cliente para realizar la cotización", "error");
    return true;
}

function SinProveedor() {
    AlertaAGC("Error", "Seleccione un proveedor para realizar la cotización", "error");
    return true;
}

function SinCondicionesVenta() {
    AlertaAGC("Error", "¡Las condiciones de venta son obligatorias!", "error");
    return true;
}

function SinCondicionesCompra() {
    AlertaAGC("Error", "¡Las condiciones de compra son obligatorias!", "error");
    return true;
}
function ErrorInicio() {
    AlertaAGC("Error", "Usuario y/o contraseña incorrectos", "error");
    return true;
}

function SinImagen() {
    AlertaAGC("Error", "¡No se ha seleccionado niguna imagen!", "error");
    return true;
}

function DosBusquedas() {
    AlertaAGC("Error", "¡La busqueda se debe realizar con solo un criterio!", "error");
    return true;
}

function PDF() {
    AlertaAGC("Error", "¡La cotización no ha sido confirmada!", "error");
    return true;
}

function DobleConfirmacion() {
    AlertaAGC("Error", "¡Este folio ya esta registrado, Genere uno nuevo!", "error");
    return true;
}

function LimitePartidas() {
    AlertaAGC("Error", "¡No se pueden eliminar mas partidas de esta Cotización!", "error");
    return true;
}

function UnidadesVacias() {
    AlertaAGC("Error", "¡Los Campos de unidades no pueden estar vacíos!", "error");
    return true;
}

function UnidadesVacias() {
    AlertaAGC("Error", "¡Los Campos de unidades no pueden estar vacíos!", "error");
    return true;
}

function SinPartidasSeleccionadas() {
    AlertaAGC("Error", "¡Ninguna partida se ha seleccionado!", "error");
    return true;
}

function SinFecha() {
    AlertaAGC("Error", "¡Ingresa una fecha de entrega!", "error");
    return true;
}

function FechaInferior() {
    AlertaAGC("Error", "¡La fecha ingresada es anterior a la actual!", "error");
    return true;
}

// PROVEEDORES - - - - - - - - - - - - - - - - - - - - - - -

function RFCinvalido() {
    AlertaAGC("Error", "El RFC ingresado no es válido.", "error");
    return true;
}

function TelefonoInvalido() {
    AlertaAGC("Error", "El Telefono solo debe llevar Numeros", "error");
    return true;
}

function RFCProveedorDuplicado() {
    AlertaAGC("Error", "¡Ya existe un proveedor con el mismo RFC registrado!", "error");
    return true;
}

// CLIENTES - - - - - - - - - - - - - - - - - - - - - - -
function ErrorRFC() {
    AlertaAGC("Error", "¡El RFC debe tener 12 o 13 caracteres!", "error");
    return true;
}

function ErrorTelefono() {
    AlertaAGC("Error", "¡El número de teléfono debe tener 10 caracteres!", "error");
    return true;
}

function RFCClienteDuplicado() {
    AlertaAGC("Error", "¡Ya existe un cliente con el mismo RFC registrado!", "error");
    return true;
}



// PRODUCTOS - - - - - - - - - - - - - - - - - - - - - - -
function NombreDuplicado() {
    AlertaAGC("Error", "¡Ya existe un producto con el mismo nombre registrado!", "error");
    return true;
}

function ErrorCostoPrecio() {
    AlertaAGC("Error", "¡El costo de adqusición no puede ser mayor o igual al precio de venta!", "error");
    return true;
}

function ImagenIncompatible(mensaje) {
    AlertaAGC("Error", mensaje, "error");
    return true;
}

function MismoCodigoProducto() {
    AlertaAGC("Error", "El código generado ha sido registrado, vuelve a subir el producto.", "error");
    return true;
}

//Dependencias - - - - - - - - - - - - - - - - - - - - - -

function CategoriaDuplicada() {
    AlertaAGC("Error", "¡Ya existe una categoria con el mismo nombre registrado!", "error");
    return true;
}

function ZonaDuplicada() {
    AlertaAGC("Error", "¡Ya existe una zona con el mismo nombre registrado!", "error");
    return true;
}

function EstadoDuplicado() {
    AlertaAGC("Error", "¡Ya existe un estado con el mismo nombre registrado!", "error");
    return true;
}

// Usuarios
function UsuarioReplicado() {
    AlertaAGC("Error", "¡Los datos ingresados corresponden a otro usuario!", "error");
    return true;
}

function PerfilReplicado() {
    AlertaAGC("Error", "¡El nombre ingresado corresponde a otro perfil!", "error");
    return true;
}

function NoEliminarPerfil() {
    AlertaAGC("Error", "¡El perfil de Super Admin no puede ser eliminado!", "error");
    return true;
}

function NoActualizarPerfil() {
    AlertaAGC("Error", "¡El perfil de Super Admin no puede ser actualizado!", "error");
    return true;
}

function NoActualizarUsuario() {
    AlertaAGC("Error", "¡El usuario Super Admin no puede cambiar de perfil!", "error");
    return true;
}

function NoEliminarUsuario() {
    AlertaAGC("Error", "¡El usuario Super Admin no puede ser eliminado!", "error");
    return true;
}

function SuperAdminUnico() {
    AlertaAGC("Error", "¡Solo un usuario puede ser Super Admin!", "error");
    return true;
}

// GENERALES - - - - - - - - - - - - - - - - - - - - - - -

function SesionExpirada() {
    AlertaAGC("¡Sesión Expirada!", "Vuelva a iniciar sesión.", "error");
    var contenido = document.getElementById('pantallaBloqueo');
    contenido.classList.add('contenido-inhabilitado');
    return true;
}

function PerfilDenegado() {
    AlertaAGC("Error", "El nivel de usuario no dispone de los permisos necesarios.", "error");
    return true;
}

function ErrorDatos() {
    AlertaAGC("Error", "Se ha producido un error al intentar realizar esta acción.", "error");
    return true;
}

// Procesos - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
function Subido() {
    AlertaAGC("Éxito", "Datos registrados correctamente", "success");
    return true;
}

function Actualizado() {
    AlertaAGC("Éxito", "Datos actualizados correctamente", "success");
    return true;
}

function Borrado() {
    AlertaAGC("Éxito", "Producto eliminado correctamente", "success");
    return true;
}

function VaciarCorizacion() {
    AlertaAGC("Éxito", "Valores de cotización reiniciados", "success");
}


function InicioSesion(Nombre) {
    AlertaAGC("Éxito", "Bienvenido usuario: " + Nombre, "success");
}

function Eliminado() {
    AlertaAGC("Éxito", "¡Datos eliminados Correctamente!", "success");
    return true;
}

function ErrorCorreo() {
    AlertaAGC("Error", "¡El correo es invalido!", "error");
    return true;
}

function test(criterio) {
    AlertaAGC("CriterioAgregado", "Criterio: " + criterio, "success");
}

function NombrePerfilInvalido() {
    AlertaAGC("Error", "¡Solo se debe ingresar letras!", "error");
    return true;
}

function BloquearEnter(event) {
    if (event.keyCode === 13) {
        event.preventDefault(); // Cancela la acción predeterminada del Enter
        return false; // Evita que el evento se propague
    }
}


// Pausar animacion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
const elementos = document.querySelectorAll('.iconos');
let animacionPausada = false;

// Función para pausar la animación
function pausarAnimacion() {
    if (!animacionPausada) {
        elementos.forEach((elemento) => {
            elemento.style.animationPlayState = 'paused';
        });
        animacionPausada = true;

        // Reanudar la animación después de 2 segundos
        setTimeout(() => {
            reanudarAnimacion();
        }, 1000);
    }
}

// Función para reanudar la animación
function reanudarAnimacion() {
    elementos.forEach((elemento) => {
        elemento.style.animationPlayState = 'running';
    });
    animacionPausada = false;

    // Pausar la animación después de 4 segundos
    setTimeout(() => {
        pausarAnimacion();
    }, 6000);
}

// Reiniciar la animación a la posición inicial
function reiniciarAnimacion() {
    elementos.forEach((elemento) => {
        elemento.style.animation = 'none';
        void elemento.offsetWidth; // Forzar un reflow
        elemento.style.animation = null;
    });
}

// Iniciar la animación
reanudarAnimacion();
