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

        private FormMain formMain;

        private bool isChangingRadio = false;

        private string fileName;

        public FormCustomGame()
        {
            InitializeComponent();
        }

        public FormCustomGame(byte[] data, FormMain formMain, string fileName) : this()
        {
            this.data = data;
            this.formMain = formMain;
            this.fileName = fileName;
            L10N.SetLang(this, formMain.Language);
            radioButtonHoDU.Checked = true;
        }

        public FormCustomGame(GbaCv game,  FormMain formMain) : this()
        {
            this.data = game.GetData();
            this.formMain = formMain;
            this.fileName = game.FileName;
            L10N.SetLang(this, formMain.Language);
            Text = L10N.GetText("ChangeCustomRom");
            if (game.GameType == GameTypeEnum.Hod)
            {
                if (game.GameVersion == GameVersionEnum.USA)
                {
                    radioButtonHoDU.Checked = true;
                }
                else if (game.GameVersion == GameVersionEnum.JPN)
                {
                    radioButtonHoDJ.Checked = true;
                }
                else if (game.GameVersion == GameVersionEnum.EUR)
                {
                    radioButtonHoDE.Checked = true;
                }
            }
            else if (game.GameType == GameTypeEnum.Aos)
            {
                if (game.GameVersion == GameVersionEnum.USA)
                {
                    radioButtonAoSU.Checked = true;
                }
                else if (game.GameVersion == GameVersionEnum.JPN)
                {
                    radioButtonAoSJ.Checked = true;
                }
                else if (game.GameVersion == GameVersionEnum.EUR)
                {
                    radioButtonAoSE.Checked = true;
                }
            }
            textBoxRoomPointer.Text = "0x" + game.GetFirstRoomPointer().Address.ToString("X");
            textBoxMapPointer.Text = "0x" + game.GetMapPointer().Address.ToString("X");
            textBoxLinePointer.Text = "0x" + game.GetMapLinePointer().Address.ToString("X");
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
            else if (radioButtonHoDE.Checked)
            {
                textBoxRoomPointer.Text = "0x8494D48";
                textBoxMapPointer.Text = "0x80DAD94";
                textBoxLinePointer.Text = "0x80DC194";
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
            else if (radioButtonAoSE.Checked)
            {
                textBoxRoomPointer.Text = "0x850EF60";
                textBoxMapPointer.Text = "0x8116664";
                textBoxLinePointer.Text = "0x8117DE4";
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
            GameVersionEnum gameVersion = GameVersionEnum.USA;
            ROMPointer _firstRoomPointer = new ROMPointer(firstRoomPointer);
            ROMPointer _mapPointer = new ROMPointer(mapPointer);
            ROMPointer _mapLinePointer = new ROMPointer(mapLinePointer);
            if (radioButtonHoDU.Checked)
            {
                gameVersion = GameVersionEnum.USA;
                game = new HoDCustom(data, formMain, _firstRoomPointer, _mapPointer, _mapLinePointer, gameVersion);
            }
            else if (radioButtonHoDJ.Checked)
            {
                gameVersion = GameVersionEnum.JPN;
                game = new HoDCustom(data, formMain, _firstRoomPointer, _mapPointer, _mapLinePointer, gameVersion);
            }
            else if (radioButtonHoDE.Checked)
            {
                gameVersion = GameVersionEnum.EUR;
                game = new HoDCustom(data, formMain, _firstRoomPointer, _mapPointer, _mapLinePointer, gameVersion);
            }
            else if (radioButtonAoSU.Checked)
            {
                gameVersion = GameVersionEnum.USA;
                game = new AoSCustom(data, formMain, _firstRoomPointer, _mapPointer, _mapLinePointer, gameVersion);
            }
            else if (radioButtonAoSJ.Checked)
            {
                gameVersion = GameVersionEnum.JPN;
                game = new AoSCustom(data, formMain, _firstRoomPointer, _mapPointer, _mapLinePointer, gameVersion);
            }
            else if (radioButtonAoSE.Checked)
            {
                gameVersion = GameVersionEnum.EUR;
                game = new AoSCustom(data, formMain, _firstRoomPointer, _mapPointer, _mapLinePointer, gameVersion);
            }

            if (game != null)
            {
                game.GameVersion = gameVersion;
                game.FileName = fileName;
                game.IsCustom = true;
            }

            if (checkBoxSaveSetting.Checked)
            {
                RomSettings romSettings = new RomSettings();
                romSettings.RomPath = game.FileName;
                romSettings.GameVersion = game.GameVersion;
                romSettings.GameType = game.GameType;
                romSettings.RoomPointer = _firstRoomPointer + 0x8_00_00_00u;
                romSettings.MapPointer = _mapPointer + 0x8_00_00_00u;
                romSettings.MapLinePointer = _mapLinePointer + 0x8_00_00_00u;
                formMain.InsertOrUpdateRomSettings(romSettings);
            }

            formMain.OpenCustomRom(game);
            this.Close();
        }
    }
}
