using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarpSearch.Common;
using WarpSearch.Lang;

namespace WarpSearch
{
    public partial class FormTechInfo : Form
    {
        private FormMain parentForm;

        public FormTechInfo(FormMain parent)
        {
            InitializeComponent();
            parentForm = parent;
            L10N.SetLang(this, parentForm.Language);
        }

        public void CleatText()
        {
            textBoxTechInfo.Text = string.Empty;
        }

        public void SetText(WarpTechInfo warpTechInfo, GameTypeEnum gameType, OperationMode operationMode)
        {
            CleatText();
            AppendText(warpTechInfo, gameType, operationMode);
        }

        public void AppendText(WarpTechInfo warpTechInfo, GameTypeEnum gameType, OperationMode operationMode)
        {
            StringBuilder techText = new StringBuilder();
            techText.AppendLine(string.Format(L10N.GetText("TechInfoExitAddr"), warpTechInfo.ExitPointerStart.ToString()));
            if (warpTechInfo.IsOutOfBound && operationMode == OperationMode.FindDestination)
            {
                techText.AppendLine(L10N.GetText("TechInfoExitOoB"));
            }
            else
            {
                techText.AppendLine(string.Format(L10N.GetText("TechInfoExitFound"),
                    warpTechInfo.IsNormalExit ? L10N.GetText("TechInfoNormalExit") : L10N.GetText("TechInfoOoBExit"),
                    warpTechInfo.ExitIndex,
                    warpTechInfo.StartX,
                    warpTechInfo.StartY
                ));

                foreach (var techInfoRoom in warpTechInfo.DestRooms)
                {
                    techText.AppendLine(string.Format(L10N.GetText("TechInfoRoomInfo"),
                        techInfoRoom.RoomPointer,
                        techInfoRoom.IsInvalidRoom ? L10N.GetText("TechInfoInvalidRoom") : L10N.GetText("TechInfoValidRoom")
                    ));

                    if (techInfoRoom.IsOutOfBoundRoom)
                    {
                        techText.Append(L10N.GetText("TechInfoOoBRoom"));
                    }
                    if (techInfoRoom.Flag != 0xFFFF)
                    {
                        var bit = techInfoRoom.Flag % 8;
                        var byteOffset = techInfoRoom.Flag / 8;
                        techText.Append(string.Format(L10N.GetText("TechInfoFlagInfo"),
                            techInfoRoom.Flag.ToString("X2"),
                            byteOffset,
                            bit
                        ));
                        if (gameType == GameTypeEnum.Hod)
                        {
                            techText.AppendLine(L10N.GetText("TechInfoFlagHoD"));
                        }
                        else
                        {
                            techText.AppendLine(L10N.GetText("TechInfoFlagAoS"));
                        }
                    }
                    else
                    {
                        techText.AppendLine(L10N.GetText("TechInfoNoNextRoom"));
                        break;
                    }
                }
                if (warpTechInfo.IsLoop && operationMode == OperationMode.FindDestination)
                {
                    techText.AppendLine(L10N.GetText("TechInfoRoomLoop"));
                }
            }
            techText.AppendLine();
            textBoxTechInfo.Text += techText.ToString();
        }

        public void ChangeLang()
        {
            L10N.SetLang(this, parentForm.Language);
            SetInitText();
        }

        public void SetInitText()
        {
            textBoxTechInfo.Text = L10N.GetText("TechInfoSelectExit");
        }

        private void FormTechInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.TechInfoForm = null;
        }

        private void FormTechInfo_Load(object sender, EventArgs e)
        {
            SetInitText();
            textBoxTechInfo.Select(0, 0);
        }
    }
}
