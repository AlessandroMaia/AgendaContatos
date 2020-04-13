using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaContatos
{
    public partial class FrmAgenda : Form
    {
        public FrmAgenda()
        {
            InitializeComponent();
        }
        private OperacaoEnum acao;

        private void FrmAgenda_Shown(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesAlterarIncluirExcluir(true);
            AlterarEstadoCampos(false);
            CarregarListaContatos();
        }
        private void AlterarBotoesSalvarECancelar(bool estado)
        {
            btnSalvar.Enabled = estado;
            btnCancelar.Enabled = estado;
        }
        private void AlterarBotoesAlterarIncluirExcluir(bool estado)
        {
            btnIncluir.Enabled = estado;
            btnAlterar.Enabled = estado;
            btnExcluir.Enabled = estado;
        }
        private void AlterarEstadoCampos(bool estado)
        {
            txbNome.Enabled = estado;
            txbEmail.Enabled = estado;
            txbTelefone.Enabled = estado;
        }
        private void LimparCampos()
        {
            txbNome.Text = "";
            txbEmail.Text = "";
            txbTelefone.Text = "";
        }


        private void CarregarListaContatos()
        {
            lsbContatos.Items.Clear();
            lsbContatos.Items.AddRange(ManipuladorArquivos.LerArquivo().ToArray());
        }
        private void lsbContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contato contato = (Contato)lsbContatos.Items[lsbContatos.SelectedIndex];
            txbNome.Text = contato.Nome;
            txbEmail.Text = contato.Email;
            txbTelefone.Text = contato.NumeroTelefone;
        }


        private void btnIncluir_Click(object sender, EventArgs e)
        {
            LimparCampos();
            AlterarEstadoCampos(true);
            AlterarBotoesSalvarECancelar(true);
            AlterarBotoesAlterarIncluirExcluir(false);
            acao = OperacaoEnum.INCLUIR;
        }
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            AlterarEstadoCampos(true);
            AlterarBotoesSalvarECancelar(true);
            AlterarBotoesAlterarIncluirExcluir(false);
            acao = OperacaoEnum.ALTERAR;
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tem certeza? ", "Pergunta", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int indiceExcluido = lsbContatos.SelectedIndex;
                lsbContatos.SelectedIndex = 0;
                lsbContatos.Items.RemoveAt(indiceExcluido);
                List<Contato> contatosList = new List<Contato>();
                foreach (Contato contato in lsbContatos.Items)
                {
                    contatosList.Add(contato);
                }
                ManipuladorArquivos.EscreverArquivo(contatosList);
                CarregarListaContatos();
                LimparCampos();
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos();
            AlterarEstadoCampos(false);
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesAlterarIncluirExcluir(true);
        }
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Contato contato = new Contato
            {
                Nome = txbNome.Text,
                Email = txbEmail.Text,
                NumeroTelefone = txbTelefone.Text
            };

            List<Contato> contatosList = new List<Contato>();
            foreach (Contato contatoDaLista in lsbContatos.Items)
            {
                contatosList.Add(contatoDaLista);
            }
            if (acao == OperacaoEnum.INCLUIR)
            {
                contatosList.Add(contato);
            }
            else if (acao == OperacaoEnum.ALTERAR)
            {
                int indice = lsbContatos.SelectedIndex;
                contatosList.RemoveAt(indice);
                contatosList.Insert(indice, contato);
            }
            ManipuladorArquivos.EscreverArquivo(contatosList);
            CarregarListaContatos();
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesAlterarIncluirExcluir(true);
            LimparCampos();
            AlterarEstadoCampos(false);
        }

    }
}
