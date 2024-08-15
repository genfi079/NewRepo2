using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;






/* 
 * En este código, hemos desarrollado una aplicación de gestión de nómina con varias funcionalidades clave:
 
 -Agregar: Permite ingresar datos de empleados a través de TextBoxes y los agrega al DataGridView.
       Aquí nos enfocamos en validar que todos los campos estén llenos y en el formato correcto antes de añadir los datos.

 -Modificar: Permite seleccionar una fila del DataGridView, cargar sus datos en los TextBoxes para ser editados, 
      y luego actualizar la fila con los datos modificados. En esta sección, cambiamos el botón de "Modificar" a "Guardar" 
      para distinguir entre los modos de edición y guardado.

 -Exportar: Permite guardar los datos del DataGridView en un archivo de texto. 
     Aquí nos enfocamos en formatear el archivo de manera que cada campo de los empleados se guarde en una 
     línea separada, con dos líneas en blanco entre cada registro para mejorar la legibilidad.

 -Importar: Permite leer datos desde un archivo de texto y cargarlos en el DataGridView. 
     Procesamos cada línea del archivo para extraer y agregar la información al DataGridView, 
     asegurando que la importación se realice de manera estructurada.

 -Salir: Permite cerrar la aplicación después de confirmar la intención del usuario 
     con un mensaje de confirmación.

 Validaciones: Implementamos validaciones para asegurar que los campos numéricos solo
     acepten números y que los TextBoxes para nombre y apellido capitalicen correctamente cada palabra.

Además, hemos añadido funcionalidades adicionales como la limpieza de TextBoxes después
     de cada operación y la conversión de texto en mayúsculas para los campos de nombre y apellido

Nos enfocamos que que sea una interfaz facil de utilizar y amigable con los usuarios.
    
*/
namespace Gestor_de_Nomina
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Angel David Santos 23-EISN-2-004
            // Configuramos eventos para evitar que se escriban letras en ciertos campos, para mejorar la estetica del programa y evitar errores.
            txtSalarioBase.KeyPress += new KeyPressEventHandler(txtNumeros_KeyPress);
            txtID.KeyPress += new KeyPressEventHandler(txtNumeros_KeyPress);
            txtHorasTrabajadas.KeyPress += new KeyPressEventHandler(txtNumeros_KeyPress);
            txtHorasExtras.KeyPress += new KeyPressEventHandler(txtNumeros_KeyPress);
            txtDeducciones.KeyPress += new KeyPressEventHandler(txtNumeros_KeyPress);
            txtBonos.KeyPress += new KeyPressEventHandler(txtNumeros_KeyPress);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificamos que todos los campos estén llenos
                if (string.IsNullOrWhiteSpace(txtID.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtSalarioBase.Text) ||
                    string.IsNullOrWhiteSpace(txtHorasTrabajadas.Text) ||
                    string.IsNullOrWhiteSpace(txtHorasExtras.Text) ||
                    string.IsNullOrWhiteSpace(txtBonos.Text) ||
                    string.IsNullOrWhiteSpace(txtDeducciones.Text))
                {
                    MessageBox.Show("Por favor, completa todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Creamos un nuevo empleado con los datos del formulario
                Empleado nuevoEmpleado = new Empleado()
                {
                    ID = int.Parse(txtID.Text),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    SalarioBase = decimal.Parse(txtSalarioBase.Text),
                    HorasTrabajadas = int.Parse(txtHorasTrabajadas.Text),
                    HorasExtras = decimal.Parse(txtHorasExtras.Text),
                    Bonos = decimal.Parse(txtBonos.Text),
                    Deducciones = decimal.Parse(txtDeducciones.Text)
                };

                // Agregamos el empleado al DataGridView
                dataGridView1.Rows.Add(nuevoEmpleado.ID, nuevoEmpleado.Nombre, nuevoEmpleado.Apellido,
                                       nuevoEmpleado.SalarioBase, nuevoEmpleado.HorasTrabajadas,
                                       nuevoEmpleado.HorasExtras, nuevoEmpleado.Bonos,
                                       nuevoEmpleado.Deducciones);

                // Limpiamos los TextBoxes después de agregar
                LimpiarTextBoxes();

                MessageBox.Show("Empleado añadido con éxito.");
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Asegúrate de que todos los números estén en el formato correcto.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Algo salió mal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Genfi Bencosme Polanco 23-SISN-2-023
        private int filaSeleccionada = -1;
        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (btnModificar.Text == "Modificar")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    filaSeleccionada = dataGridView1.SelectedRows[0].Index;

                    // Rellenamos los TextBoxes con los datos de la fila seleccionada
                    txtID.Text = dataGridView1.Rows[filaSeleccionada].Cells[0].Value.ToString();
                    txtNombre.Text = dataGridView1.Rows[filaSeleccionada].Cells[1].Value.ToString();
                    txtApellido.Text = dataGridView1.Rows[filaSeleccionada].Cells[2].Value.ToString();
                    txtSalarioBase.Text = dataGridView1.Rows[filaSeleccionada].Cells[3].Value.ToString();
                    txtHorasTrabajadas.Text = dataGridView1.Rows[filaSeleccionada].Cells[4].Value.ToString();
                    txtHorasExtras.Text = dataGridView1.Rows[filaSeleccionada].Cells[5].Value.ToString();
                    txtBonos.Text = dataGridView1.Rows[filaSeleccionada].Cells[6].Value.ToString();
                    txtDeducciones.Text = dataGridView1.Rows[filaSeleccionada].Cells[7].Value.ToString();

                    btnModificar.Text = "Guardar";
                }
                else
                {
                    MessageBox.Show("Selecciona un registro para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (btnModificar.Text == "Guardar")
            {
                try
                {
                    // Actualizamos los datos de la fila seleccionada
                    dataGridView1.Rows[filaSeleccionada].Cells[0].Value = txtID.Text;
                    dataGridView1.Rows[filaSeleccionada].Cells[1].Value = txtNombre.Text;
                    dataGridView1.Rows[filaSeleccionada].Cells[2].Value = txtApellido.Text;
                    dataGridView1.Rows[filaSeleccionada].Cells[3].Value = txtSalarioBase.Text;
                    dataGridView1.Rows[filaSeleccionada].Cells[4].Value = txtHorasTrabajadas.Text;
                    dataGridView1.Rows[filaSeleccionada].Cells[5].Value = txtHorasExtras.Text;
                    dataGridView1.Rows[filaSeleccionada].Cells[6].Value = txtBonos.Text;
                    dataGridView1.Rows[filaSeleccionada].Cells[7].Value = txtDeducciones.Text;

                    // Limpiamos los TextBoxes después de guardar
                    LimpiarTextBoxes();

                    // Cambiamos el texto del botón de vuelta a "Modificar"
                    btnModificar.Text = "Modificar";
                    filaSeleccionada = -1;

                    MessageBox.Show("Datos actualizados con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Algo salió mal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Josias Martinez 23-SISN-2-054
        private void btnExportar_Click(object sender, EventArgs e)
        {
            // Abre un cuadro de diálogo para elegir dónde guardar el archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
            saveFileDialog.Title = "Guardar como archivo de texto";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Lista para guardar las líneas del archivo
                    List<string> lines = new List<string>();

                    // Recorremos cada fila del DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            // Añadimos los datos de la fila en el formato deseado
                            lines.Add($"ID: {row.Cells["ID"].Value}");
                            lines.Add($"Nombre: {row.Cells["Nombre"].Value}");
                            lines.Add($"Apellido: {row.Cells["Apellido"].Value}");
                            lines.Add($"Salario: {row.Cells["Salario"].Value}");
                            lines.Add($"Horas Trabajadas: {row.Cells["HorasTrabajadas"].Value}");
                            lines.Add($"Horas Extras: {row.Cells["HorasExtras"].Value}");
                            lines.Add($"Bonos: +{row.Cells["Bonos"].Value}");
                            lines.Add($"Deducciones: -{row.Cells["Deducciones"].Value}");
                            lines.Add(""); // Espacio entre registros
                            lines.Add(""); // Espacio entre registros
                        }
                    }

                    // Guardamos todas las líneas en el archivo seleccionado
                    System.IO.File.WriteAllLines(saveFileDialog.FileName, lines);
                    MessageBox.Show("Datos exportados correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //Genfi Bencosme Polanco 23-SISN-2-023
        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Seguro que quieres salir?", "Confirmar salida", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void LimpiarTextBoxes()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtSalarioBase.Clear();
            txtHorasTrabajadas.Clear();
            txtHorasExtras.Clear();
            txtBonos.Clear();
            txtDeducciones.Clear();
        }
        //Christian Rollin 23-SISN-2-053
        //Creamos este metodo para crear estetica al programa a la hora de utilizarlo
        private void CapitalizarCadaPalabra(TextBox textBox)
        {
            if (textBox.Text.Length > 0)
            {
                // Convierte el texto en una lista de palabras
                string[] palabras = textBox.Text.Split(' ');
                for (int i = 0; i < palabras.Length; i++)
                {
                    if (palabras[i].Length > 0)
                    {
                        // Convierte la primera letra en mayúscula y el resto en minúscula
                        palabras[i] = char.ToUpper(palabras[i][0]) + palabras[i].Substring(1).ToLower();
                    }
                }

                // Une las palabras y actualiza el texto del TextBox
                textBox.Text = string.Join(" ", palabras);

                // Coloca el cursor al final del texto
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            CapitalizarCadaPalabra((TextBox)sender);
        }

        private void txtApellido_TextChanged(object sender, EventArgs e)
        {
            CapitalizarCadaPalabra((TextBox)sender);
        }

        private void txtNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificamos si se presionó una tecla que no sea un número o la tecla de retroceso
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Mostramos un mensaje de error
                MessageBox.Show("Este campo solo acepta números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Bloqueamos el ingreso del carácter
                e.Handled = true;
            }
        }
        // //Genfi Bencosme Polanco 23-SISN-2-023. Decidi hacer el programa mas completo para que permita importar datos tambien.
        private void btnImportar_Click(object sender, EventArgs e)
        {
            // Creamos un cuadro de diálogo para seleccionar el archivo a importar
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
            openFileDialog.Title = "Seleccionar archivo de texto";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Leemos todas las líneas del archivo
                    string[] lines = System.IO.File.ReadAllLines(openFileDialog.FileName);

                    // Limpiamos el DataGridView antes de importar nuevos datos
                    dataGridView1.Rows.Clear();

                    // Lista temporal para almacenar los datos de una fila
                    List<string> rowData = new List<string>();

                    // Procesamos cada línea del archivo
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            // Añadimos la línea a la lista de datos de la fila
                            rowData.Add(line.Split(':')[1].Trim());

                            // Si tenemos todos los campos de una fila, agregamos al DataGridView
                            if (rowData.Count == 8)
                            {
                                dataGridView1.Rows.Add(rowData.ToArray());
                                rowData.Clear();
                            }
                        }
                    }

                    MessageBox.Show("Datos importados con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo importar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //Ariel Alexander Mejia 23-SISN-2-048
        //Este es el metodo que vamos a utilizar para buscar registros.
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Limpiamos cualquier selección previa, asegurándonos de empezar la búsqueda sin interferencias.
            dataGridView1.ClearSelection();

            // Verificamos que el usuario haya ingresado un valor de búsqueda. Esto es esencial para evitar búsquedas vacías que nos llevarían a resultados incorrectos.
            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                // Recorremos cada fila del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Aquí nos centramos en evaluar cada celda para ver si contiene el valor buscado. Este enfoque cumple con los requerimientos de búsqueda solicitados.
                    if (row.Cells.Cast<DataGridViewCell>().Any(c => c.Value != null && c.Value.ToString().Equals(txtBuscar.Text)))
                    {
                        // Si encontramos el valor, lo seleccionamos y notificamos al usuario.
                        row.Selected = true;
                        MessageBox.Show("Registro encontrado y seleccionado.", "Búsqueda exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                // Si el valor no se encuentra, lo notificamos, cumpliendo con la necesidad de mantener al usuario informado sobre los resultados de sus acciones.
                MessageBox.Show("Registro no encontrado.", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Manejamos el caso en que el usuario no ingresa ningún valor de búsqueda, asegurando la robustez de nuestra aplicación.
                MessageBox.Show("Por favor, introduce un valor para buscar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Jonathan Rijo 23-SISN-2-043 
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Primero, nos aseguramos de que el usuario haya seleccionado al menos una fila para eliminar.
                // Esto es importante porque el profesor nos pidió que validáramos todas las posibles interacciones.
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Antes de eliminar cualquier cosa, mostramos una ventana de confirmación.
                    // Esto ayuda a prevenir que se borren datos por accidente, lo que es algo que el profesor nos hizo hincapié.
                    DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar los registros seleccionados?",
                                                          "Confirmar eliminación",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Si el usuario confirma, entonces procedemos a eliminar todas las filas seleccionadas.
                        // Esto se hace recorriendo cada fila seleccionada en el DataGridView.
                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            // Removemos cada fila de la lista. Nos aseguramos de que esto solo ocurra si hay una confirmación previa.
                            dataGridView1.Rows.Remove(row);
                        }
                        // Informamos al usuario que los registros se eliminaron correctamente, cumpliendo con el ciclo de retroalimentación que también fue solicitado.
                        MessageBox.Show("Registros eliminados correctamente.");
                    }
                }
                else
                {
                    // Si no se ha seleccionado ninguna fila, mostramos un mensaje de error.
                    // Esto es para guiar al usuario y evitar confusiones, tal como se discutió en clase.
                    MessageBox.Show("Por favor, selecciona al menos un registro para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Manejamos cualquier error inesperado que pueda ocurrir durante el proceso.
                // El manejo de excepciones es crucial para asegurarnos de que el programa no falle de forma inesperada, como lo mencionó el profesor.
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
//Programador y diseñador de la interfaz: Genfi Bencosme 23-SISN-2-023, cada quien hizo su parte y dieron sus ideas para que el programa funcione sin errores y 
//sea comodo de usar el programa, tambien se trato de hacer que el programa tenga buena estetica, con imagenes y le creamos un logo.

//Esperamos que haya cumplido los estandares exigidos, y estamos dispuestos a mejorar, Bendicione!!!
