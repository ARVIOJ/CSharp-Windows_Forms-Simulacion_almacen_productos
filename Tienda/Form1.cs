using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace Tienda
{
    public partial class frmPrincipal : Form
    {
        int m, mx, my;
        int poc;
        SoundPlayer sonido;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
      (
          int nLeftRect, // Coordenada x de la esquina superior izquierda
          int nTopRect, // Coordenada y de la esquina superior izquierda
          int nRightRect, // Coordenada x de la esquina inferior derecha
          int nBottomRect, // Coordenada y de la esquina inferior derecha
          int nWidthEllipse, // Altura de la elipse
          int nHeightEllipse // Ancho de la elipse
      );

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            m = 1;
            mx = e.X;
            my = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m == 1)
            {

                this.SetDesktopLocation(MousePosition.X - mx, MousePosition.Y - my);

            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            m = 0;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Minimized;
            }
            else if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog getPic = new OpenFileDialog();
            getPic.InitialDirectory = "C:\\User\\Descargas\\";
            
            getPic.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (getPic.ShowDialog() == DialogResult.OK)
            {
                ptb1.ImageLocation = getPic.FileName;


            }
            else
            {
                MessageBox.Show("No has ingresado ninguna imagen", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ptb1.Image = Properties.Resources.imgInicio;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbNombre.Text) || String.IsNullOrEmpty(txbCantidad.Text) || String.IsNullOrEmpty(txbPrecio.Text) || String.IsNullOrEmpty(txbTotal.Text))
            {

                MessageBox.Show("Debe completar la información");

                return;

            }

            try
            {
                dgvDato.Rows.Add(txbNombre.Text, txbCantidad.Text, txbPrecio.Text, txbTotal.Text, ptb1.Image);
                txbNombre.Text = "";
                txbCantidad.Text = "";
                txbPrecio.Text = "";
                txbTotal.Text = "";
                btnLimpiar.Enabled = false;
                this.ptb1.Image = Properties.Resources.imgInicio;
                MessageBox.Show("Se inserto correctamente");

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo insertar los datos por:\n " + ex);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txbNombre.Text) || String.IsNullOrEmpty(txbCantidad.Text) || String.IsNullOrEmpty(txbPrecio.Text) || String.IsNullOrEmpty(txbTotal.Text))
            {

                MessageBox.Show("Debe completar la información");

                return;

            }
            try {

                dgvDato[0, poc].Value = txbNombre.Text;
                dgvDato[1, poc].Value = txbCantidad.Text;
                dgvDato[2, poc].Value = txbPrecio.Text;
                dgvDato[3, poc].Value = txbTotal.Text;
                dgvDato[4, poc].Value = ptb1.Image;
                
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnGuardar.Enabled = true;
                btnLimpiar.Enabled = false;
                limpiar();
                sonido = new SoundPlayer(Application.StartupPath + @"\audios\edit.wav");
                sonido.Play();
                MessageBox.Show("Se edito correctamente");

            }


            catch {

                MessageBox.Show("No se puedo editar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        public frmPrincipal()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult msj = MessageBox.Show("¿Deseas eliminar el registro?", "Eliminar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (msj == DialogResult.OK)
                {
                    if (dgvDato.SelectedRows.Count > 0)
                    {
                        limpiar();
                        dgvDato.Rows.Remove(dgvDato.CurrentRow);
                        btnEliminar.Enabled = false;
                        btnEditar.Enabled = false;
                        btnGuardar.Enabled = true;
                        btnLimpiar.Enabled = false;
                        sonido = new SoundPlayer(Application.StartupPath + @"\audios\delete.wav");
                        sonido.Play();
                        MessageBox.Show("Eliminado Correctamente");

                    }
                    else
                    {
                        MessageBox.Show("Seleccione una fila por favor");
                        btnEliminar.Enabled = false;
                    }
                }
                else
                {

                    txbNombre.Focus();
                    btnEliminar.Enabled = false;
                    btnEditar.Enabled = false;
                    btnGuardar.Enabled = true;
                }
                

            }
            catch
            {
                MessageBox.Show("Seleccione una fila valida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnEliminar.Enabled = false;
                btnEditar.Enabled = false;
                btnGuardar.Enabled = true;
            }
        }

        private void dgvDato_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            try
            {
                    
                poc = dgvDato.CurrentRow.Index;
                if (dgvDato.SelectedRows.Count > 0)
                {
                    txbNombre.Focus();
                    btnGuardar.Enabled = false;
                    btnEditar.Enabled = true;
                    txbNombre.Text = dgvDato[0, poc].Value.ToString();
                    txbCantidad.Text = dgvDato[1, poc].Value.ToString();
                    txbPrecio.Text = dgvDato[2, poc].Value.ToString();
                    txbTotal.Text = dgvDato[3, poc].Value.ToString();
                    ptb1.Image = (Image)dgvDato[4, poc].Value;
                    

                }

                else
                {
                    MessageBox.Show("Seleccione una fila por favor");
                    btnEditar.Enabled = false;
                    txbNombre.Focus();
                }

            }
            catch
            {
                MessageBox.Show("Seleccione una fila valida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnEditar.Enabled = false;
                btnGuardar.Enabled = true;
                btnEliminar.Enabled = false;
                txbNombre.Focus();
            }

        }

        private void txbCantidad__TextChanged(object sender, EventArgs e)
        {
            btnLimpiar.Enabled = true;
            int v1 = 0;

            int.TryParse(txbCantidad.Text, out v1);

            double v2 = 0;
            Double.TryParse(txbPrecio.Text, out v2);

            txbTotal.Text = (v1 * v2).ToString("N2");
        }

        private void txbPrecio__TextChanged(object sender, EventArgs e)
        {
            btnLimpiar.Enabled = true;
            int v1 = 0;

            int.TryParse(txbCantidad.Text, out v1);

            double v2 = 0;
            Double.TryParse(txbPrecio.Text, out v2);

            txbTotal.Text = (v1 * v2).ToString("N2");
        }

        private void txbNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txbCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumerosEnteros(e);
        }

        private void txbPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumerosDecimal(e);
        }

        private void txbTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validar.SoloNumerosDecimal(e);
        }

        private void dgvDato_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (dgvDato.SelectedRows.Count > 0)
            {
                btnEliminar.Enabled = true;
            }

            else
            {
                btnEliminar.Enabled = false;
            }
        }

        private void dgvDato_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult msj = MessageBox.Show("¿Deseas salir del programa?", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (msj == DialogResult.OK)
                {
                    Close();
                }
                else
                {

                    txbNombre.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void txbNombre_TextChanged(object sender, EventArgs e)
        {
            btnLimpiar.Enabled = true;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txbNombre.Focus();

            txbNombre.Clear();
            txbCantidad.Clear();
            txbPrecio.Clear();
            txbTotal.Clear();

            this.ptb1.Image = Properties.Resources.imgInicio;

            btnLimpiar.Enabled = false;
            btnEliminar.Enabled = false;
            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;
        }

        private void txbTotal_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            dgvDato.Columns.Add("nombre", "Nombre");
            dgvDato.Columns.Add("cantidad", "Cantidad");
            dgvDato.Columns.Add("precio", "Precio");
            dgvDato.Columns.Add("total", "Total");
            DataGridViewImageColumn colum = new DataGridViewImageColumn();
            colum.Name = "Imagen";
            dgvDato.Columns.Add(colum);
            dgvDato.RowTemplate.Height = 120;
            colum.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;

        }

        public void limpiar()
        {
            txbNombre.Focus();
            txbNombre.Clear();
            txbCantidad.Clear();
            txbPrecio.Clear();
            txbTotal.Clear();
            this.ptb1.Image = Properties.Resources.imgInicio;
        }


    }
}
