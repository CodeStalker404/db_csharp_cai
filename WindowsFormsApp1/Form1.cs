using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Net;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        //DataSet dsProfesores;
        //SqlDataAdapter da;
        ////variable que indica en qué registro estamos situados.
        ///////private int maxRegistros;

        // Instancia del objeto que maneja la BD.
        SqlDBHelper sqlDBHelper;

        // Variable que indica en qué registro estamos situados
        private int pos;


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            ////string cadenaConexion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\JRJ_1\\Projects\\db_csharp_cai\\Instituto.mdf;Integrated Security=True;Connect Timeout=30";
            //string cadenaConexion = "data source = (localdb)\\mssqllocaldb;" +
            //   "attachdbfilename = c:\\users\\cai\\onedrive\\escritorio\\daw\\programación\\tema 9\\ejercicios\\instituto.mdf;" +
            //   "integrated security = true; connect timeout = 30";

            //SqlConnection con = new SqlConnection(cadenaConexion);

            ////abrimos la conexión
            //con.Open();

            //dsProfesores = new DataSet();
            //string cadenaSql = "SELECT * From Profesores";

            //da = new SqlDataAdapter(cadenaSql, con);

            //da.Fill(dsProfesores, "Profesores");

            ////Obtenemos el número de registros
            //maxRegistros = dsProfesores.Tables["Profesores"].Rows.Count;

            ////Situamos la primera posición.
            //pos = 0;
            //mostrarRegistro(pos);

            ////cerramos la conexión
            //con.Close();



            // Creamos el objeto BD
            sqlDBHelper = new SqlDBHelper();

            // Situamos la primera posición
            // y mostramos el registro
            pos = 0;
            mostrarRegistro(pos);
        }


        private void mostrarRegistro(int pos)
        {
            Profesor profesor;

            profesor = sqlDBHelper.devuelveProfesor(pos);
            
            //Cogemos el valor de cada una de las columnas del registro y lo ponemos en el txtBox correspondiente
            txtDni.Text = profesor.Dni;
            txtNombre.Text = profesor.Nombre;
            txtApellidos.Text = profesor.Apellidos;
            txtTelefono.Text = profesor.Tlf;
            txtEmail.Text = profesor.eMail;

            //TODO revisar número profesores
            this.lblRegistros.Text = "Registro " + (pos + 1) + " de " + sqlDBHelper.NumProfesores;//maxRegistros;
        }


        private void bPrimero_Click(object sender, EventArgs e)
        {
            //Para saber qué tipo de objetco es sender
            Console.WriteLine(sender.GetType().Name);

            //He cambiado el tipo del objeto sender a botón (castear)
            Button btn = sender as Button;

            // Ponemos la primera posición
            pos = 0;
            mostrarRegistro(pos);
            btn.Enabled = false;
            this.bAnterior.Enabled = false;
            this.bSiguiente.Enabled = true;
            this.bUltimo.Enabled = true;
        }

        private void bAnterior_Click(object sender, EventArgs e)
        {
            //He cambiado el tipo del objeto sender a botón (castear)
            Button btn = sender as Button;

            // Vamos a la posición anterior.
            pos--;

            //activamos el botón
            this.bSiguiente.Enabled = true;
            this.bUltimo.Enabled = true;
            mostrarRegistro(pos);
            if (pos <= 0)
            {   //descativamos el botón
                btn.Enabled = false;
                this.bPrimero.Enabled = false;
            }

        }

        private void bSiguiente_Click(object sender, EventArgs e)
        {

            //He cambiado el tipo del objeto sender a botón (castear)
            Button btn = sender as Button;

            // Vamos a la posición siguiente
            pos++;
            Console.WriteLine(pos.ToString());

            this.bAnterior.Enabled = true;
            this.bPrimero.Enabled = true;
            mostrarRegistro(pos);
            if (pos >= sqlDBHelper.NumProfesores - 1)
            {
                btn.Enabled = false;
                this.bUltimo.Enabled = false;
            }

        }

        private void bUltimo_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            // Vamos a la última posición.
            // Los registros van del 0 al numero de registros - 1
            pos = sqlDBHelper.NumProfesores - 1;
            mostrarRegistro(pos);
            btn.Enabled = false;
            this.bSiguiente.Enabled = false;
            this.bAnterior.Enabled = true;
            this.bPrimero.Enabled = true;
        }


        private bool EsDniValido(string dni)
        {
            bool esDniValido = false;

            if(dni.Length == 9)
            {
                char ultimoCaracter = dni.ElementAt(dni.Length - 1);
                esDniValido = Char.IsLetter(ultimoCaracter);
            }
            return esDniValido;
        }

        private bool SonTodoLetras(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (char c in input)
            {
                if (!Char.IsLetter(c) && c != ' ')
                {
                    Console.WriteLine(c + " apesta");
                    return false;
                }
            }
            return true;
        }

        private bool SonTodoNumeros(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (char c in input)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool EsEmailValido(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            return email.Contains("@") && email.Contains(".");
        }


        

        private void bAnyadir_Click(object sender, EventArgs e)
        {
            txtDni.Clear();
            txtNombre.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
        }

        private void bguardar_Click(object sender, EventArgs e)
        {
            //Creamos el profesor con los datos del formulario
            Profesor profesor = new Profesor(txtDni.Text, txtNombre.Text,
                txtApellidos.Text, txtTelefono.Text, txtEmail.Text);

            sqlDBHelper.anyadirProfesor(profesor);

            //Actualizamos la posición en la tabla.
            pos = sqlDBHelper.NumProfesores - 1;
        }

        private void bActualizar_Click(object sender, EventArgs e)
        {
            //Creamos el profesor con los datos del formulario
            Profesor profesor = new Profesor(txtDni.Text, txtNombre.Text,
                txtApellidos.Text, txtTelefono.Text, txtEmail.Text);

            sqlDBHelper.actualizarProfesor(profesor, pos);
        }

        private void bEliminar_Click(object sender, EventArgs e)
        {
            sqlDBHelper.eliminarProfesor(pos);

            // Nos vamos al primer registro y lo mostramos
            pos = 0;
            mostrarRegistro(pos);
        }

        private void txtDni_TextChanged(object sender, EventArgs e)
        {
            TextBox txtDni = sender as TextBox;
            if (!EsDniValido(txtDni.Text))
            {
                this.lblValidacionDNI.Text = "DNI inválido. Introduzca un DNI válido.";
                this.lblValidacionDNI.Visible = true;
                MessageBox.Show("DNI inválido. Introduzca un DNI válido.");
            }
            else
            {
                this.Visible = false;
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            TextBox txtNombre = sender as TextBox;
            Console.WriteLine(txtNombre.Text);
            if (!SonTodoLetras(txtNombre.Text))
            {
                MessageBox.Show("Nombre inválido. Un nombre solo puede contener letras y espacios.");
            }
        }

        private void txtApellidos_TextChanged(object sender, EventArgs e)
        {
            TextBox txtApellidos = sender as TextBox;
            if (!SonTodoLetras(txtApellidos.Text))
            {
                MessageBox.Show("Apellidos inválidos. Los apellidos solo pueden contener letras y espacios.");
            }
        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            TextBox txtTelefono = sender as TextBox;
            if (!SonTodoNumeros(txtTelefono.Text))
            {
                MessageBox.Show("Teléfono inválido. Un teléfono solo puede contener números.");
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            TextBox txtEmail = sender as TextBox;
            if (!EsEmailValido(txtEmail.Text))
            {
                MessageBox.Show("Teléfono inválido. Un teléfono solo puede contener números.");
            }
        }
    }
}
