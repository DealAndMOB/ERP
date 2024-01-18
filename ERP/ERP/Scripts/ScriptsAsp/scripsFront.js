function Viewformulario() {
    document.querySelector('.aumentos').classList.toggle('activeView');
    return true;
}

function closeformulario() {
    document.querySelector('.aumentos').classList.toggle('closeView');
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
function ViewUsuario() {
    document.querySelector('.usuario').classList.toggle('UsuarioView');

    return true;
}

function CloseUsuario() {
    document.querySelector('.usuario').classList.toggle('productos');
    return true;
}

function UpdateViewUsuario() {
    document.querySelector('.UpdateUsuario').classList.toggle('UsuarioView');

    return true;
}

function UpdateCloseUsuario() {
    document.querySelector('.usuario').classList.toggle('productos');
    return true;
}
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
function ViewNivel() {
    document.querySelector('.nivel').classList.toggle('NivelView');
    return true;
}

function CloseNivel() {
    document.querySelector('.nivel').classList.toggle('productos');
    return true;
}

function UpdateViewNivel() {
    document.querySelector('.UpdateNivel').classList.toggle('NivelView');
    return true;
}

function CloseNivel() {
    document.querySelector('.UpdateNivel').classList.toggle('productos');
    return true;
}
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                            //Productos
//Subir
function productos() {
    document.querySelector('.ProductoC').classList.remove('close');
    document.querySelector('.ProductoC').classList.add('open');
    return true;
}

function closeP() {
    document.querySelector('.ProductoC').classList.remove('open');
    document.querySelector('.ProductoC').classList.add('close');
    return true;
}
//Actualizar
function Updateproductos() {
    document.querySelector('.UpdateProductosC').classList.remove('close');
    document.querySelector('.UpdateProductosC').classList.add('open');
    return true;
}

function UpdateCloseP() {
    document.querySelector('.UpdateProductosC').classList.remove('open');
    document.querySelector('.UpdateProductosC').classList.add('close');
    return true;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                           //Proveedores
function proveedores() {
    document.querySelector('.ProveedorC').classList.remove('close');
    document.querySelector('.ProveedorC').classList.add('open');
    return true;
}

function closeProvee() {
    document.querySelector('.ProveedorC').classList.remove('open');
    document.querySelector('.ProveedorC').classList.add('close');
    return true;
}

//Actualizar

function Updateproveedores() {
    document.querySelector('.UpdateProveedorC').classList.remove('close');
    document.querySelector('.UpdateProveedorC').classList.add('open');
    return true;
}

function UpdateCloseProvee() {
    document.querySelector('.UpdateProveedorC').classList.remove('open');
    document.querySelector('.UpdateProveedorC').classList.add('close');
    return true;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
//Zona
function Zona() {
    document.querySelector('.fomularioZona').classList.remove('close');
    document.querySelector('.fomularioZona').classList.add('open');
    return true;
}
function closeZona() {
    document.querySelector('.fomularioZona').classList.remove('open');
    document.querySelector('.fomularioZona').classList.add('close');
    return true;
}
//Actualizar
function UpdateZona() {
    document.querySelector('.UpdateZona').classList.remove('close');
    document.querySelector('.UpdateZona').classList.add('open');
    return true;
}
function UpdateCloseZona() {
    document.querySelector('.UpdateZona').classList.remove('open');
    document.querySelector('.UpdateZona').classList.add('close');
    return true;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
//Estado
function Estado() {
    document.querySelector('.fomularioEstado').classList.remove('close');
    document.querySelector('.fomularioEstado').classList.add('open');
    return true;
}
function closeEstado() {
    document.querySelector('.fomularioEstado').classList.remove('open');
    document.querySelector('.fomularioEstado').classList.add('close');
    return true;
}
//Actualizar

function UpdateEstado() {
    document.querySelector('.UpdateEstado').classList.remove('close');
    document.querySelector('.UpdateEstado').classList.add('open');
    return true;
}
function UpdatecloseEstado() {
    document.querySelector('.UpdateEstado').classList.remove('open');
    document.querySelector('.UpdateEstado').classList.add('close');
    return true;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

function actializar() {
    document.querySelector('.actualizar').classList.toggle('open');
    document.querySelector('.subir').classList.toggle('close');

    return true;
}

function subir() {
    document.querySelector('.actualizar').classList.toggle('close');
    document.querySelector('.actualizar').classList.toggle('open');
    return true;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
function actualizarBTNs() {
    document.querySelector('.ContenedorAgregar').classList.add('close');

    document.querySelector('.ContenedorActualizar').classList.remove('close');
    document.querySelector('.ContenedorCancelar').classList.remove('close');
    return true;
}

function ReiniciarBTNs() {
    document.querySelector('.ContenedorAgregar').classList.remove('close');

    document.querySelector('.ContenedorActualizar').classList.add('close');
    document.querySelector('.ContenedorCancelar').classList.add('close');
    return true;
}

function MostrarDateConfig() {
    document.querySelector('.Calendario').classList.remove('close');
    return true;
}

function validarAccesos(accesos) {
    let cadena = accesos;
    var modulos = {
        1: ["compras"],
        2: ["ventas"],
        3: ["catalogos"],
        4: ["sistema"]
    }
    for (let i = 0; i < cadena.length; i++) {
        permisos(i, cadena);
    }
    function permisos(i, cadena) {
        if (cadena[i] == '0') {
            // alert("Acceso a " + modulos[i + 1]);
            let mostrar = document.getElementById(modulos[i + 1]);
            mostrar.classList.add("close");
        }
        else if (cadena[i] == '1') {
            // alert("Error en " + modulos[i + 1]);
            console.log("Hola mundo")
        }
    }
}

//AlertaConfirmación
function alertConfirmar() {
    document.querySelector('.alert').classList.add('open');
    document.querySelector('.alert').classList.remove('close');
    return true;
}

function alertConfirmar2() {
    document.querySelector('.alert2').classList.add('open');
    document.querySelector('.alert2').classList.remove('close');
    return true;
}

function EliminarUsuario() {
    document.querySelector('.usu').classList.remove('close');
    return true;
}

function Eliminarperfil() {
    document.querySelector('.per').classList.remove('close');
    return true;
}

function closeAlertUsu() {
    document.querySelector('.usu').classList.add('close');
    return true;
}

function closeAlertPer() {
    document.querySelector('.per').classList.add('close');
    return true;
}