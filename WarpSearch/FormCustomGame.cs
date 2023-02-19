using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarpSearch.Games;
using WarpSearch.Lang;

namespace WarpSearch
{
    public partial class FormCustomGame : Form
    {
        private byte[] data;

        private GbaCv game;

        private FormMain formMain;

        private bool isChangingRadio = false;

        public FormCustomGame()
        {
            InitializeComponent();
        }

        public FormCustomGame(byte[] data, FormMain formMain) : this()
        {
            this.data = data;
            this.game = new GbaCv(data, formMain);
            this.formMain = formMain;
            L10N.SetLang(this, formMain.Language);
        }

        private void textBoxRoomPointer_TextChanged(object sender, EventArgs e)
        {
            textBoxChangedCommon(textBoxRoomPointer);
        }

        private uint textBoxChangedCommon(TextBox textBox)
        {
            if (isChangingRadio) return 0;
            string hexText = textBox.Text;
            if (hexText.StartsWith("0x") || hexText.StartsWith("0X"))
            {
                hexText = hexText.Substring(2);
            }
            uint number = 0;
            try
            {
                number = Convert.ToUInt32(hexText, 16);
            }
            catch
            {
                textBox.ForeColor = Color.Red;
                return 0;
            }
            if (number < 0x8_00_00_00 || number >= data.Length + 0x8_00_00_00)
            {
                textBox.ForeColor = Color.Red;
                return 0;
            }
            textBox.ForeColor = SystemColors.WindowText;
            return number;
        }

        private void textBoxRoomPointer_Validated(object sender, EventArgs e)
        {
            textBoxChangedCommon(textBoxRoomPointer);
        }

        private void textBoxMapPointer_Validated(object sender, EventArgs e)
        {
            textBoxChangedCommon(textBoxMapPointer);
        }

        private void textBoxLinePointer_Validated(object sender, EventArgs e)
        {
            textBoxChangedCommon(textBoxLinePointer);
        }

        private void radioButtonGame_CheckedChanged(object sender, EventArgs e)
        {
            isChangingRadio = true;
            if (radioButtonHoDU.Checked)
            {
                textBoxRoomPointer.Text = "0x8494D48";
                textBoxMapPointer.Text = "0x80DAD94";
                textBoxLinePointer.Text = "0x80DC194";
            }
            else if (radioButtonHoDJ.Checked)
            {
                textBoxRoomPointer.Text = "0x848C3C8";
                textBoxMapPointer.Text = "0x80D24A4";
                textBoxLinePointer.Text = "0x80D38A4";
            }
            else if (radioButtonAoSU.Checked)
            {
                textBoxRoomPointer.Text = "0x850EF08";
                textBoxMapPointer.Text = "0x8116650";
                textBoxLinePointer.Text = "0x8117DD0";
            }
            else if (radioButtonAoSJ.Checked)
            {
                textBoxRoomPointer.Text = "0x84E5808";
                textBoxMapPointer.Text = "0x80F58D8";
                textBoxLinePointer.Text = "0x80F7058";
            }
            isChangingRadio = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            GbaCv game = null;
            uint firstRoomPointer = textBoxChangedCommon(textBoxRoomPointer);
            uint mapPointer = textBoxChangedCommon(textBoxMapPointer);
            uint mapLinePointer = textBoxChangedCommon(textBoxLinePointer);

            if (firstRoomPointer == 0 || mapPointer == 0 || mapLinePointer == 0)
            {
                return;
            }

            if (radioButtonHoDU.Checked)
            {
                game = new HoDCustom(data, formMain, new ROMPointer(firstRoomPointer), new ROMPointer(mapPointer), new ROMPointer(mapLinePointer), GameVersion.USA);
            }
            else if (radioButtonHoDJ.Checked)
            {
                game = new HoDCustom(data, formMain, new ROMPointer(firstRoomPointer), new ROMPointer(mapPointer), new ROMPointer(mapLinePointer), GameVersion.JPN);
            }
            else if (radioButtonAoSU.Checked)
            {
                game = new AoSCustom(data, formMain, new ROMPointer(firstRoomPointer), new ROMPointer(mapPointer), new ROMPointer(mapLinePointer), GameVersion.USA);
            }
            else if (radioButtonAoSJ.Checked)
            {
                game = new AoSCustom(data, formMain, new ROMPointer(firstRoomPointer), new ROMPointer(mapPointer), new ROMPointer(mapLinePointer), GameVersion.JPN);
            }

            formMain.OpenCustomRom(game);
            this.Close();
        }
    }
}
