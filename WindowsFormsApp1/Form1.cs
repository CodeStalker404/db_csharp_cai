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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        DataSet dsProfesores;
        SqlDataAdapter da;
        //variable que indica en qué registro estamos situados.

        private int pos;
        private int maxRegistros;


        public Form1()
        {
            InitializeComponent();
        }


        private void mostrarRegistro (int pos)
        {
            //Objeto que nos permite recoger un registro de la tabla.
            DataRow dRegistro;

            // Cogemos el registro de la posición pos en la tabla Profesores
            dRegistro = dsProfesores.Tables["Profesores"].Rows[pos];

            //Cogemos el valor de cada una de las columnas del registro y lo ponemos en el Txt correspondiente,

            txtDni.Text = dRegistro[0].ToString();
            txtNombre.Text = dRegistro[1].ToString();   
            txtApellidos.Text = dRegistro[2].ToString();
            txtTelefono.Text = dRegistro[3].ToString();
            txtEmail.Text = dRegistro[4].ToString();

            this.lblRegistros.Text = "Registro " + (pos+1) + " de " + maxRegistros;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string cadenaConexion = "Data Source = (LocalDB)\\MSSQLLocalDB;" +
               "AttachDbFilename = C:\\Users\\Cai\\OneDrive\\Escritorio\\DAW\\PROGRAMACIÓN\\tema 9\\ejercicios\\Instituto.mdf;" +
               "Integrated Security = True; Connect Timeout = 30";

            SqlConnection con = new SqlConnection(cadenaConexion);

            //abrimos la conexión
            con.Open();

            dsProfesores = new DataSet();
            string cadenaSql = "SELECT * From Profesores";

            da = new SqlDataAdapter(cadenaSql, con);

            da.Fill(dsProfesores, "Profesores");

            //Obtenemos el número de registros
            maxRegistros = dsProfesores.Tables["Profesores"].Rows.Count;

            //Situamos la primera posición.
            pos = 0;
            mostrarRegistro(pos);
             
            //cerramos la conexión
            con.Close();

            

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
            if(pos >= maxRegistros-1)
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
            pos = maxRegistros - 1;
            mostrarRegistro(pos);
            btn.Enabled = false;
            this.bSiguiente.Enabled = false;
            this.bAnterior.Enabled = true;
            this.bPrimero.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

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
            //Creamos un nuevo registro.
            DataRow dRegistro = dsProfesores.Tables["Profesores"].NewRow();

            //Metemos los datos en el nuevo registro
            dRegistro[0] = txtDni.Text;
            dRegistro[1] = txtNombre.Text;
            dRegistro[2] = txtApellidos.Text;
            dRegistro[3] = txtTelefono.Text;
            dRegistro[4] = txtEmail.Text;


            //Añadimos el registro al Dataset.
            dsProfesores.Tables["Profesores"].Rows.Add(dRegistro);

            //Reconectamos con el dataAdapter y acutalizamos la Base de Datos.
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.Update(dsProfesores, "Profesores");

            //Actualizamos el número de registros y la posición en la tabla.
            maxRegistros++;
            pos = maxRegistros - 1;
        }

        private void bActualizar_Click(object sender, EventArgs e)
        {
            //Cogemos el registro situado en la posición actual.
            DataRow dRegistro = dsProfesores.Tables["Profesores"].Rows[pos];

            //Metemos los datos en el registro
            dRegistro[0] = txtDni.Text;
            dRegistro[1] = txtNombre.Text;
            dRegistro[2] = txtApellidos.Text;
            dRegistro[3] = txtTelefono.Text;
            dRegistro[4] = txtEmail.Text;

            //Reconectamos con el dataAdapter y actualizamos la Base de Datos.
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.Update(dsProfesores, "Profesores");
        }

        private void bEliminar_Click(object sender, EventArgs e)
        {
            //Eliminamos el registro situado en la posición actual.
            dsProfesores.Tables["Profesores"].Rows[pos].Delete();

            //Tenemos un registro menos.
            maxRegistros--;

            //Vamos al primer registro y lo mostramos
            pos = 0;
            mostrarRegistro(pos);

            //Reconectamos con el dataAdapter y actualizamos la Base de Datos.
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.Update(dsProfesores, "Profesores");

        }
    }
}
