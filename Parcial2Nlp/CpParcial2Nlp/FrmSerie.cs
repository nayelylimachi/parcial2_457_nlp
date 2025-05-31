using CadParcial2Nlp;
using ClnParcial2Nlp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Nlp
{
    public partial class FrmSerie : Form
    {
        private bool esNuevo = false;
        public FrmSerie()
        {
            InitializeComponent();
            listar();
        }

        private void listar()
        {
            var lista = SerieCln.listarPa(txtParametro.Text.Trim());
            dgvLista.DataSource = lista;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["titulo"].HeaderText = "Título";
            dgvLista.Columns["sinopsis"].HeaderText = "Sinopsis";
            dgvLista.Columns["director"].HeaderText = "Director";
            dgvLista.Columns["episodios"].HeaderText = "Episodios";
            dgvLista.Columns["fechaEstreno"].HeaderText = "Fecha de Estreno";
            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["titulo"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
            Size = new Size(846, 362);
            listar();
        }

        private void limpiar()
        {
            txtTitulo.Clear();
            txtDirector.Clear();
            txtSinopsis.Clear();
            nudEpisodios.Value = 0;
            dtpFechaEstreno.Value = DateTime.Today;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(846, 362);
            limpiar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(846, 521);
            txtTitulo.Focus();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(846, 521);

            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            var serie = SerieCln.obtenerUno(id);
            txtTitulo.Text = serie.titulo;
            txtSinopsis.Text = serie.sinopsis;
            txtDirector.Text = serie.director;
            txtTitulo.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private bool validar()
        {
            bool esValido = true;
            erpTitulo.SetError(txtTitulo, "");
            erpSinopsis.SetError(txtSinopsis, "");
            erpDirector.SetError(txtDirector, "");
            erpEpisodio.SetError(nudEpisodios, "");
            erpFechaEstreno.SetError(dtpFechaEstreno, "");
            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                erpTitulo.SetError(txtTitulo, "El campo título es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtSinopsis.Text))
            {
                erpSinopsis.SetError(txtSinopsis, "El campo sinopsis es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtDirector.Text))
            {
                erpDirector.SetError(txtDirector, "El campo director es obligatorio");
                esValido = false;
            }
            if (nudEpisodios.Value < 0)
            {
                erpEpisodio.SetError(nudEpisodios, "El campo episodio no puede ser menor a 0");
                esValido = false;
            }
            if (dtpFechaEstreno.Value > DateTime.Today)
            {
                erpFechaEstreno.SetError(dtpFechaEstreno, "La fecha de estreno no puede ser futura");
                esValido = false;
            }
            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.episodios = Convert.ToInt32(nudEpisodios.Value);
                serie.fechaEstreno = dtpFechaEstreno.Value;
                if (esNuevo)
                {
                    serie.estado = 1;
                    SerieCln.insertar(serie);
                }
                else
                {
                    int index = dgvLista.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    SerieCln.actualizar(serie);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Serie guardada correctamente", "::: Serie - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string titulo = dgvLista.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar la serie {titulo}?",
                "::: Serie - Mensaje :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                SerieCln.eliminar(id);
                listar();
                MessageBox.Show("Serie dado de baja correctamente", "::: Serie - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
