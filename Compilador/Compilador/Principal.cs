﻿using Rules;
using ScintillaNET;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Main
{
    public partial class Principal : Form
    {
        private Compiler Compiler { get; set; }
        private FileManager FileManager { get; set; }

        public Principal()
        {
            Compiler = new Compiler();
            FileManager = new FileManager();
            InitializeComponent();
            this.editor.Styles[Style.Default].Font = "Consolas";
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(OnKeyDown);
        }

        private void editor_TextChanged(object sender, EventArgs e)
        {
            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var _maxLineNumberCharLength = this.editor.Lines.Count.ToString().Length;
            if (_maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            editor.Margins[0].Width = editor.TextWidth(Style.LineNumber, new string('9', _maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = _maxLineNumberCharLength;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                ShowTeam();
            else if (e.KeyCode == Keys.F9)
                Compile();
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.N)
                NewFile();
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.O)
                OpenFile();
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
                SaveFile();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            editor.Copy();
        }

        private void btnColar_Click(object sender, EventArgs e)
        {
            editor.Paste();
        }

        private void btnRecortar_Click(object sender, EventArgs e)
        {
            editor.Cut();
        }

        private void btnCompilar_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void btnEquipe_Click(object sender, EventArgs e)
        {
            ShowTeam();
        }

        private void NewFile()
        {
            editor.Text = mensagens.Text = barraStatus.Text = string.Empty;
            FileManager = new FileManager();
        }

        private void OpenFile()
        {
            try
            {
                FileManager = new FileManager();

                if (FileManager.OpenFile())
                {
                    editor.Text = FileManager.fileContent;
                    barraStatus.Text = FileManager.filePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Não foi possível abrir o arquivo. Erro: {0}", ex.Message), "Erro");
            }
        }

        private void SaveFile()
        {
            try
            {
                FileManager.SaveFile(editor.Text);
                mensagens.Text = string.Empty;
                MessageBox.Show("Arquivo salvo com sucesso!");
                barraStatus.Text = FileManager.filePath;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("O arquivo não foi salvo, nenhum local selecionado.", "Aviso");
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Não foi possível salvar o arquivo: {0}", e.Message), "Atenção");
            }
        }

        private void ShowTeam()
        {
            mensagens.Text = "Alan Felipe Jantz, Caroline Belli Regalin e Matheus Manhke";
        }

        private void Compile()
        {
            try
            {
                if (Compiler.Compile(editor.Text))
                    mensagens.Text = "Programa compilado com sucesso.";
                else
                    mensagens.Text = "Nenhum programa para compilar.";
            }
            catch (Exception ex)
            {
                mensagens.Text = ex.Message;
            }
        }
    }
}
