using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EjemploDataGridView
{
    public partial class Form1 : Form
    {
        #region Atributos
        // Creo e instancio la lista de clases
        private List<Productos> productos = new List<Productos>();
        // Variable global para guardar el indice del producto seleccionado
        private int id;
        #endregion

        #region Eventos
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {          
            // Verifico que el texto dentro de txtNombre sea nulo o vacio
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                mensajeError(txtNombre);
                return;
            }
            // Verifico que el valor del txtPrecio no sea numerico
            if (!isNumeric(txtPrecio.Text))
            {
                mensajeError(txtPrecio);
                return;
            }
            // Verifico que el valor del txtStock no sea numerico
            if (!isNumeric(txtStock.Text))
            {
                mensajeError(txtStock);
                return;
            }

            switch (btnGuardar.Text)
            {
                /* Si el boton tiene escrito en la propiedad "Text" "Guardar" agrego un elemento nuevo en la lista
                 * Si dice "Guardar Cambios" actualizo el registro correspondiente de la lista 
                 */
                case "Guardar":

                    /* agrego en la lista un nuevo registro creando un nuevo objeto de la clase productos 
                     * y asignandole los valores a sus respectivas propiedades 
                     */
                    productos.Add(new Productos()
                    {
                        Nombre = txtNombre.Text,
                        Descripcion = txtDescripcion.Text,
                        Marca = txtMarca.Text,
                        Precio = Convert.ToDouble(txtPrecio.Text),
                        Stock = Convert.ToDouble(txtStock.Text)
                    });
                    break;

                case "Guardar Cambios":

                    /* Reemplazo, en la lista de productos, en la posicion del id, los datos
                     * correspondientes y cambio el texto del boton nuevamente
                     */
                    productos[id].Nombre = txtNombre.Text;
                    productos[id].Descripcion = txtDescripcion.Text;
                    productos[id].Marca = txtMarca.Text;
                    productos[id].Precio = Convert.ToDouble(txtPrecio.Text);
                    productos[id].Stock = Convert.ToDouble(txtStock.Text);

                    btnGuardar.Text = "Guardar";
                    break;

                default:
                    break;
            }
            actualizarDatos();
            limpiarCampos(pnlMantenimiento);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                productos.RemoveAt(id); // Elimino los valores guardados en la lista del indice que obtenemos de la variable global "id"
                actualizarDatos();
                limpiarCampos(pnlMantenimiento);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Eliminar", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            /* Cargo en los TextBox's correspondientes lo guardado en la lista de la fila seleccionada
             * de la grilla (utilizando el valor guardado en la variable global "id") 
             */
            txtNombre.Text = productos[id].Nombre.ToString();
            txtDescripcion.Text = productos[id].Descripcion.ToString();
            txtMarca.Text = productos[id].Marca.ToString();
            txtPrecio.Text = productos[id].Precio.ToString();
            txtStock.Text = productos[id].Stock.ToString();
            btnGuardar.Text = "Guardar Cambios";
        }

        private void dtgProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dtgProductos.CurrentCell.RowIndex; // Guardo el Indice de la celda seleccionada en la variable global
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e); // Permito escribir solo numeros
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e); // Permito escribir solo numeros
        }
        #endregion

        #region Metodos

        // Metodo de mensaje de error
        private void mensajeError(Control control)
        {
            MessageBox.Show(
                    "Error al Guardar: \n Debe completar los campos obligatorios",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            control.Focus();
        }

        // Metodo para actualizar la grilla (DataGridView)
        private void actualizarDatos()
        {
            dtgProductos.DataSource = null;      // Limpio la grilla
            dtgProductos.DataSource = productos; // Vuelvo a cargar la grilla
        }

        // Metodo para limpiar los campos de un control (por ejemplo un panel)
        private void limpiarCampos(Control control)
        {
            //Recorro los controles del del control recibido por parametro
            foreach (object item in control.Controls)
            {
                // Si el control es un TextBox lo limpio
                if (item is TextBox)
                {
                    ((TextBox)item).Clear();
                }
            }
        }

        // Metodo para permitir solo ingresar numero o simbolos exceptuados como el "." en este caso
        private KeyPressEventArgs soloNumeros(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar) |
                char.ToString(e.KeyChar) == ".")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            return e;
        }
        // Metodo para validar si un valor dado es numerico o no
        private bool isNumeric(string valor)
        {
            try
            {
                // Si puede convertilo a double devuelve verdadero (es numerico)
                Convert.ToDouble(valor);
                return true;
            }
            catch (Exception)
            {
                // Al no poder convertirse y generarse una excepcion devuelve falso (no es un numero)
                return false;
            }
        } 
        #endregion
    }
}
