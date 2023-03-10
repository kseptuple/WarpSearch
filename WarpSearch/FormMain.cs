using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using WarpSearch.Games;
using System.Threading;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;
using WarpSearch.Lang;

namespace WarpSearch
{
    public partial class FormMain : Form
    {
        private float scale = 3;
        private float lineWidth => scale;
        private float gridSize => scale * 4;
        private Bitmap map = null;
        private GbaCv rom = null;
        private readonly Brush blueBrush = new SolidBrush(Color.FromArgb(0, 0, 224));
        private readonly Brush blackBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
        private readonly Brush cyanBrush = new SolidBrush(Color.FromArgb(0, 200, 200));
        private readonly Brush redBrush = new SolidBrush(Color.FromArgb(240, 0, 0));
        private readonly Brush yellowBrush = new SolidBrush(Color.FromArgb(248, 248, 8));
        private readonly Brush whiteBrush = new SolidBrush(Color.FromArgb(248, 248, 248));
        private readonly Brush transparentWhiteBrush = new SolidBrush(Color.FromArgb(192, 255, 255, 255));
        private readonly Brush transparentGreenBrush = new SolidBrush(Color.FromArgb(127, 0, 255, 0));
        private readonly Brush transparentBlueBrush = new SolidBrush(Color.FromArgb(127, 0, 0, 255));
        private readonly Brush transparentRedBrush = new SolidBrush(Color.FromArgb(127, 255, 0, 0));
        private readonly Brush transparentOrangeBrush = new SolidBrush(Color.FromArgb(127, 255, 127, 0));
        private readonly Brush transparentBlackBrush = new SolidBrush(Color.FromArgb(127, 0, 0, 0));
        private readonly Brush trueWhiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        private readonly Brush greenBrush = new SolidBrush(Color.FromArgb(0, 224, 0));

        private Pen redPen = new Pen(Color.FromArgb(192, 0, 0));
        private readonly Brush redBrush2 = new SolidBrush(Color.FromArgb(192, 0, 0));

        private Graphics bitmapGraphics = null;

        private List<RectangleToDraw> RoomToDraw = new List<RectangleToDraw>();
        private List<uint> SourceRoomPointers = new List<uint>();
        private List<RectangleToDraw> PositionToDraw = new List<RectangleToDraw>();
        private List<RectangleToDraw> PositionPreviewToDraw = new List<RectangleToDraw>();
        private List<LineToDraw> LinesToDraw = new List<LineToDraw>();
        private List<Point> GreenRooms = new List<Point>();

        private OperationMode opMode = OperationMode.FindDestination;

        private string defaultAosPath = null;
        private string defaultHodPath = null;

        private bool useHackSupport = false;

        private GameTypeEnum romType = GameTypeEnum.Null;

        private RoomInfo originalSelectedRoom = null;

        public RoomInfo selectedRoom = null;
        public Point selectedPos = default;

        public RoomStruct sourceRoom = null;
        public RoomStruct destRoom = null;

        private int currentSourceRoomInListId = -1;

        private int globalOffset = 2;

        private List<Label> labelOptionSearches = new List<Label>();

        private Dictionary<ROMPointer, Dictionary<uint, byte>> flagListForRoom = new Dictionary<ROMPointer, Dictionary<uint, byte>>();

        public string Language { get; set; }

        public FormMain()
        {
            InitializeComponent();
            redPen.Width = scale;
            labelOptionSearches.Add(labelSearchOption1);
            labelOptionSearches.Add(labelSearchOption2);
            labelOptionSearches.Add(labelSearchOption3);
            labelOptionSearches.Add(labelSearchOption4);
            labelOptionSearches.Add(labelSearchOption5);
        }

        public void DrawRoom(int x, int y, RoomType type, bool isHodCastleB = false)
        {
            Brush currentBrush = null;
            x += globalOffset;
            y += globalOffset;
            switch (type)
            {
                case RoomType.Normal:
                    if (isHodCastleB)
                    {
                        currentBrush = greenBrush;
                        GreenRooms.Add(new Point(x, y));
                    }
                    else
                    {
                        currentBrush = blueBrush;
                    }
                    break;
                case RoomType.Warp:
                    if (isHodCastleB)
                    {
                        GreenRooms.Add(new Point(x, y));
                    }
                    currentBrush = yellowBrush;
                    break;
                case RoomType.Save:
                case RoomType.Error:
                    if (isHodCastleB)
                    {
                        GreenRooms.Add(new Point(x, y));
                    }
                    currentBrush = redBrush;
                    break;
                default:
                    currentBrush = blackBrush;
                    break;
            }
            bitmapGraphics.FillRectangle(currentBrush, x * gridSize, y * gridSize, gridSize, gridSize);
        }

        public void DrawLine(int x, int y, bool isHorizonal, bool isSolid)
        {
            x += 2;
            y += 2;
            if (isHorizonal)
            {
                if (isSolid)
                {
                    bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, gridSize + lineWidth, lineWidth);
                }
                else
                {
                    if (romType == GameTypeEnum.Hod)
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, (gridSize - lineWidth) * 0.333333333f + lineWidth, lineWidth);
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize + (gridSize - lineWidth) * 0.666666667f + lineWidth, y * gridSize, (gridSize - lineWidth) * 0.333333333f + lineWidth, lineWidth);
                    }
                    else
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, gridSize + lineWidth, lineWidth);
                        bitmapGraphics.FillRectangle(cyanBrush, x * gridSize + lineWidth, y * gridSize, gridSize - lineWidth, lineWidth);
                    }
                }

            }
            else
            {
                if (isSolid)
                {
                    bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, lineWidth, gridSize + lineWidth);
                }
                else
                {
                    if (romType == GameTypeEnum.Hod)
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, lineWidth, (gridSize - lineWidth) * 0.333333333f + lineWidth);
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize + (gridSize - lineWidth) * 0.666666667f + lineWidth, lineWidth, (gridSize - lineWidth) * 0.333333333f);
                    }
                    else
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, lineWidth, gridSize + lineWidth);
                        bitmapGraphics.FillRectangle(cyanBrush, x * gridSize, y * gridSize + lineWidth, lineWidth, gridSize - lineWidth);
                    }
                }
            }
        }

        public void DrawText(string text, int x, int y, float size)
        {
            x += 2;
            y += 2;
            Font drawFont = new Font(SystemFonts.DefaultFont.FontFamily, size * scale);
            bitmapGraphics.DrawString(text, drawFont, trueWhiteBrush, x * gridSize, y * gridSize);
        }

        private void PictureMap_Click(object sender, EventArgs e)
        {
            if (rom == null) return;
            var mouseEvent = e as MouseEventArgs;
            if (mouseEvent != null)
            {
                var actualX = (int)Math.Floor((mouseEvent.X - gridSize * 2) / (float)gridSize);
                var actualY = (int)Math.Floor((mouseEvent.Y - gridSize * 2) / (float)gridSize);
                switch (opMode)
                {
                    //这里出城到哪里
                    case OperationMode.FindDestination:
                        {
                            if (mouseEvent.Button == MouseButtons.Right)
                            {
                                selectRoom(actualX, actualY);
                                pictureMap.Refresh();
                            }
                            else if (mouseEvent.Button == MouseButtons.Left)
                            {
                                PositionToDraw.Clear();
                                LinesToDraw.Clear();
                                var left = actualX;
                                var top = actualY;
                                selectedPos = new Point(left, top);
                                rom.FindWarpDestination();
                                if (sourceRoom != null && destRoom != null)
                                {
                                    textSrcRoomPointer.Text = sourceRoom.RoomPointer.ToString();
                                    textDestRoomPointer.Text = destRoom.RoomPointer.ToString();
                                    if (destRoom.EventFlag != -1)
                                    {
                                        textDestFlag.Text = destRoom.EventFlag.ToString("X2");
                                    }
                                }
                                else
                                {
                                    textSrcRoomPointer.Text = "";
                                    textDestRoomPointer.Text = "";
                                    textDestFlag.Text = "";
                                }
                                pictureMap.Refresh();
                            }
                        }
                        break;
                    //哪里出城到这里
                    case OperationMode.FindSource:
                        {
                            if (mouseEvent.Button == MouseButtons.Right)
                            {
                                currentSourceRoomInListId = -1;
                                selectRoom(actualX, actualY);
                                rom.FindWarpSource(trackBarSearchOption.Value);
                                pictureMap.Refresh();
                            }
                            else if (mouseEvent.Button == MouseButtons.Left)
                            {
                                if (selectedRoom == null || selectedRoom.Room == null) return;
                                PositionToDraw.Clear();
                                LinesToDraw.Clear();
                                if (currentSourceRoomInListId != -1)
                                {
                                    RoomToDraw[currentSourceRoomInListId + 1].Brush = transparentBlackBrush;
                                }
                                var point = new Point(actualX, actualY);
                                RoomInfo currentSourceRoom = null;
                                RoomAndExit currentSourceRoomInList = null;
                                textSrcRoomPointer.Text = "";
                                textDestRoomPointer.Text = "";
                                textDestFlag.Text = "";

                                if (rom.RoomsAtPositions.ContainsKey(point))
                                {
                                    currentSourceRoom = rom.RoomsAtPositions[point];
                                    if (currentSourceRoom != null)
                                    {
                                        List<RoomInfo> currentRoomInfoList = new List<RoomInfo>();
                                        currentRoomInfoList.Add(currentSourceRoom);
                                        if (rom.FlagRoomLists.ContainsKey(currentSourceRoom.Room.RoomPointer.Address))
                                        {
                                            foreach (var roomInfo in rom.FlagRoomLists[currentSourceRoom.Room.RoomPointer.Address])
                                            {
                                                if (roomInfo.Room.Left <= actualX && roomInfo.Room.Top <= actualY &&
                                                    roomInfo.Room.Left + roomInfo.Room.Width > actualX && roomInfo.Room.Top + roomInfo.Room.Height > actualY)
                                                    currentRoomInfoList.Add(roomInfo);
                                            }
                                        }
                                        for (int i = 0; i < listSourceRoom.Items.Count; i++)
                                        {
                                            foreach (var currentRoom in currentRoomInfoList)
                                            {
                                                currentSourceRoomInList = (RoomAndExit)listSourceRoom.Items[i];
                                                if (currentSourceRoomInList.Room.RoomPointer.Address == currentRoom.Room.RoomPointer.Address)
                                                {
                                                    displaySourceRoom(currentSourceRoomInList);
                                                }
                                            }
                                        }
                                    }
                                }
                                pictureMap.Refresh();
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void displaySourceRoom(RoomAndExit currentSourceRoomInList)
        {
            for (int j = 0; j < SourceRoomPointers.Count; j++)
            {
                if (SourceRoomPointers[j] == currentSourceRoomInList.Room.RoomPointer.Address)
                {
                    currentSourceRoomInListId = j;
                    RoomToDraw[currentSourceRoomInListId + 1].Brush = transparentWhiteBrush;
                }
            }
            var sourceX = currentSourceRoomInList.Room.Left + currentSourceRoomInList.Exit.SourceX;
            var sourceY = currentSourceRoomInList.Room.Top + currentSourceRoomInList.Exit.SourceY;
            var destX = selectedRoom.Room.Left + currentSourceRoomInList.Exit.DestX;
            var destY = selectedRoom.Room.Top + currentSourceRoomInList.Exit.DestY;
            AddRoomPos(sourceX, sourceY, false, false, currentSourceRoomInList.IsUncertain);
            AddRoomPos(destX, destY, false, false, currentSourceRoomInList.IsUncertain);

            if (sourceX != destX || sourceY != destY)
            {
                AddLine(sourceX, sourceY, destX, destY);
            }
            pictureMap.Refresh();
            textSrcRoomPointer.Text = currentSourceRoomInList.Room.RoomPointer.ToString();
            textDestRoomPointer.Text = selectedRoom.Room.RoomPointer.ToString();
            if (selectedRoom.Room.EventFlag != -1)
            {
                textDestFlag.Text = selectedRoom.Room.EventFlag.ToString("X2");
            }
            if (flagListForRoom.ContainsKey(currentSourceRoomInList.Room.RoomPointer))
            {
                listFlag.Items.Clear();
                var flagList = flagListForRoom[currentSourceRoomInList.Room.RoomPointer];
                foreach (var roomFlag in flagList)
                {
                    listFlag.Items.Add($"{roomFlag.Key:X8}:{roomFlag.Value:X2}");
                }
                panelFlag.Show();
            }
            else
            {
                panelFlag.Hide();
            }
        }

        public void SetFlagListForDestSearch(Dictionary<uint, byte> flagList)
        {
            if (flagList != null && flagList.Count != 0)
            {
                listFlag.Items.Clear();
                foreach (var roomFlag in flagList)
                {
                    listFlag.Items.Add($"{roomFlag.Key:X8}:{roomFlag.Value:X2}");
                }
                panelFlag.Show();
            }
            else
            {
                panelFlag.Hide();
            }
        }

        private void PictureMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (rom == null) return;
            var actualX = (int)Math.Floor((e.X - gridSize * 2) / (float)gridSize);
            var actualY = (int)Math.Floor((e.Y - gridSize * 2) / (float)gridSize);
            switch (opMode)
            {
                case OperationMode.FindDestination:
                    {
                        PositionPreviewToDraw.Clear();
                        var left = actualX;
                        var top = actualY;
                        selectedPos = new Point(left, top);
                        rom.FindWarpDestination(true);
                        pictureMap.Refresh();
                    }
                    break;
                case OperationMode.FindSource:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void ComboRoomList_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearAll();
            if (comboRoomList.SelectedIndex == 0)
            {
                selectedRoom = originalSelectedRoom;
            }
            else
            {
                selectedRoom = rom.FlagRoomLists[originalSelectedRoom.Room.RoomPointer.Address][comboRoomList.SelectedIndex - 1];
            }
            var room = selectedRoom.Room;
            RoomToDraw.Add(new RectangleToDraw(transparentWhiteBrush, room.Left, room.Top, room.Width, room.Height));
            if (opMode == OperationMode.FindSource)
            {
                rom.FindWarpSource(trackBarSearchOption.Value);
            }
            pictureMap.Refresh();
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuItemOpenRom_Click(object sender, EventArgs e)
        {
            if (openFileDialogMain.ShowDialog() == DialogResult.Cancel) return;
            if (OpenRom(openFileDialogMain.FileName))
            {
                if (romType == GameTypeEnum.Aos)
                {
                    defaultAosPath = openFileDialogMain.FileName;
                    toolStripMenuItemAosLast.Enabled = true;
                }
                else if (romType == GameTypeEnum.Hod)
                {
                    defaultHodPath = openFileDialogMain.FileName;
                    toolStripMenuItemHodLast.Enabled = true;
                }
            }
        }
        private void ToolStripMenuItemAosLast_Click(object sender, EventArgs e)
        {
            OpenRom(defaultAosPath);
        }

        private void ToolStripMenuItemHodLast_Click(object sender, EventArgs e)
        {
            OpenRom(defaultHodPath);
        }

        private bool OpenRom(string romPath)
        {
            GbaCv newRom = GbaCvLoader.LoadGame(romPath, this);
            if (newRom != null)
            {
                pictureMap.Visible = true;
                rom?.Dispose();
                clearAll();
                GreenRooms.Clear();
                comboRoomList.Items.Clear();
                rom = newRom;
                romType = rom.GameType;
                rom.UseHackSupport = ToolStripMenuItemHackSupport.CheckState == CheckState.Checked;
                map = new Bitmap((int)Math.Floor((rom.MapWidth + 4) * gridSize), (int)Math.Floor((rom.MapHeight + 4) * gridSize));
                bitmapGraphics = Graphics.FromImage(map);
                rom.LoadRooms();
                setSearchOptionText();
                pictureMap.Image = map;
                pictureMap.Width = map.Width;
                pictureMap.Height = map.Height;
                selectedRoom = null;
                originalSelectedRoom = null;
                return true;
            }
            return false;
        }

        public void OpenCustomRom(GbaCv newRom)
        {
            if (newRom != null)
            {
                pictureMap.Visible = true;
                rom?.Dispose();
                clearAll();
                GreenRooms.Clear();
                comboRoomList.Items.Clear();
                rom = newRom;
                romType = rom.GameType;
                rom.UseHackSupport = ToolStripMenuItemHackSupport.CheckState == CheckState.Checked;
                map = new Bitmap((int)Math.Floor((rom.MapWidth + 4) * gridSize), (int)Math.Floor((rom.MapHeight + 4) * gridSize));
                bitmapGraphics = Graphics.FromImage(map);
                rom.LoadRooms();
                setSearchOptionText();
                pictureMap.Image = map;
                pictureMap.Width = map.Width;
                pictureMap.Height = map.Height;
                selectedRoom = null;
                originalSelectedRoom = null;
            }
        }

        private void ReloadRom()
        {
            if (rom == null) return;
            clearAll();
            GreenRooms.Clear();
            comboRoomList.Items.Clear();
            rom.UseHackSupport = ToolStripMenuItemHackSupport.CheckState == CheckState.Checked;
            map = new Bitmap((int)Math.Floor((rom.MapWidth + 4) * gridSize), (int)Math.Floor((rom.MapHeight + 4) * gridSize));
            bitmapGraphics = Graphics.FromImage(map);
            rom.LoadRooms();
            pictureMap.Image = map;
            pictureMap.Width = map.Width;
            pictureMap.Height = map.Height;
            selectedRoom = null;
            originalSelectedRoom = null;
        }

        private void PictureMap_Paint(object sender, PaintEventArgs e)
        {
            foreach (var rect in RoomToDraw)
            {
                rect.Draw(e.Graphics, globalOffset, gridSize);
            }

            foreach (var rect in PositionToDraw)
            {
                rect.Draw(e.Graphics, globalOffset, gridSize);
            }

            foreach (var rect in PositionPreviewToDraw)
            {
                rect.Draw(e.Graphics, globalOffset, gridSize);
            }

            foreach (var line in LinesToDraw)
            {
                line.Draw(e.Graphics, globalOffset, gridSize);
            }
        }

        public void ClearRooms()
        {
            RoomToDraw.Clear();
        }
        public void ClearPos(bool previewOnly = false)
        {
            PositionPreviewToDraw.Clear();
            if (!previewOnly)
            {
                PositionToDraw.Clear();
            }
        }
        public void ClearLine()
        {
            LinesToDraw.Clear();
        }
        public void ClearSourceRoomList()
        {
            listSourceRoom.Items.Clear();
            SourceRoomPointers.Clear();
            if (RoomToDraw.Count > 1)
            {
                RoomToDraw.RemoveRange(1, RoomToDraw.Count - 1);
            }
        }
        public void AddSourceRoom(RoomStruct room, ExitInfo exit, bool isUncertain, Dictionary<uint, byte> flagList = null)
        {
            flagListForRoom.Clear();
            if (listSourceRoom.Items.Count == 0)
            {
                listSourceRoom.Items.Add(new RoomAndExit(room, exit, isUncertain));
            }
            else
            {
                for (int i = 0; i < listSourceRoom.Items.Count; i++)
                {
                    var roomAndExit = (RoomAndExit)listSourceRoom.Items[i];
                    if (roomAndExit.Room.RoomPointer.Address > room.RoomPointer.Address)
                    {
                        listSourceRoom.Items.Insert(i, new RoomAndExit(room, exit, isUncertain));
                        break;
                    }
                    if (roomAndExit.Room.RoomPointer.Address == room.RoomPointer.Address && roomAndExit.Exit.ExitPointer.Address == exit.ExitPointer.Address)
                    {
                        break;
                    }
                    if (i == listSourceRoom.Items.Count - 1)
                    {
                        listSourceRoom.Items.Add(new RoomAndExit(room, exit, isUncertain));
                        break;
                    }
                }
            }

            if (!SourceRoomPointers.Contains(room.RoomPointer.Address))
            {
                SourceRoomPointers.Add(room.RoomPointer.Address);
                AddRoom(room, true);
                if (flagList != null)
                {
                    flagListForRoom.Add(room.RoomPointer, flagList);
                }
            }
        }
        public void AddRoom(RoomStruct room, bool isBlack)
        {
            var brush = isBlack ? transparentBlackBrush : transparentWhiteBrush;
            RoomToDraw.Add(new RectangleToDraw(brush, room.Left, room.Top, room.Width, room.Height));
        }
        public void AddRoomPos(int x, int y, bool isBad, bool previewOnly = false, bool isUncertain = false)
        {
            var brush = transparentRedBrush;
            if (!isBad)
            {
                if (GreenRooms.Exists(p => p.X == x + globalOffset && p.Y == y + globalOffset))
                {
                    brush = transparentBlueBrush;
                }
                else
                {
                    brush = transparentGreenBrush;
                }
            }
            
            if (isUncertain)
            {
                brush = transparentOrangeBrush;
            }
            if (previewOnly)
            {
                PositionPreviewToDraw.Add(new RectangleToDraw(brush, x, y, 1, 1));
            }
            else
            {
                PositionToDraw.Add(new RectangleToDraw(brush, x, y, 1, 1));
            }
        }
        public void AddLine(int startX, int startY, int endX, int endY)
        {
            LinesToDraw.Add(new LineToDraw(redPen, redBrush2, startX, startY, endX, endY));
        }
        private void RadioButtonFindDest_CheckedChanged(object sender, EventArgs e)
        {
            panelTop.Enabled = true;
            panelBottom.Enabled = false;
            clearAll();
            selectedRoom = null;
            comboRoomList.Items.Clear();
            pictureMap.Refresh();
            opMode = OperationMode.FindDestination;
        }

        private void RadioButtonFindSource_CheckedChanged(object sender, EventArgs e)
        {
            panelBottom.Enabled = true;
            panelTop.Enabled = false;
            clearAll();
            selectedRoom = null;
            comboRoomList.Items.Clear();
            pictureMap.Refresh();
            opMode = OperationMode.FindSource;
        }

        private void TrackBarResize_Scroll(object sender, EventArgs e)
        {
            scale = trackBarResize.Value / 100.0f;
            if (rom == null) return;
            map = new Bitmap((int)Math.Floor((rom.MapWidth + 4) * gridSize), (int)Math.Floor((rom.MapHeight + 4) * gridSize));
            bitmapGraphics = Graphics.FromImage(map);
            rom.DrawRooms();
            pictureMap.Image = map;
            pictureMap.Width = map.Width;
            pictureMap.Height = map.Height;
            redPen.Width = scale;
            pictureMap.Refresh();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (File.Exists("setting.xml"))
            {
                Settings settings = null;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                using (var fs = new FileStream("setting.xml", FileMode.Open))
                using (var sr = new StreamReader(fs))
                {
                    settings = (Settings)xmlSerializer.Deserialize(sr);
                }
                if (settings != null)
                {
                    defaultAosPath = settings.AoSPath;
                    defaultHodPath = settings.HodPath;
                    useHackSupport = settings.useHackSupport;
                    toolStripMenuItemAosLast.Enabled = !string.IsNullOrEmpty(defaultAosPath);
                    toolStripMenuItemHodLast.Enabled = !string.IsNullOrEmpty(defaultHodPath);
                    ToolStripMenuItemHackSupport.CheckState = useHackSupport ? CheckState.Checked : CheckState.Unchecked;
                    Language = settings.Language;
                }
            }
            if (L10N.LanguageModels.Count > 1)
            {
                foreach (var language in L10N.LanguageModels)
                {
                    ToolStripMenuItem LanguageItem = new ToolStripMenuItem();
                    LanguageItem.Text = language.Name;
                    LanguageItem.Tag = language.AreaCode;
                    LanguageItem.Click += ToolStripMenuItemLanguageItem_Click;
                    ToolStripMenuItemLanguage.DropDownItems.Add(LanguageItem);
                }
                ToolStripMenuItemLanguage.Visible = true;
            }
            else
            {
                ToolStripMenuItemLanguage.Visible = false;
            }
            var actualLanguage = L10N.GetLang(Language);
            if (!string.IsNullOrEmpty(actualLanguage))
            {
                Language = actualLanguage;
                L10N.SetLang(this, Language);
                var languageMenuItems = ToolStripMenuItemLanguage.DropDownItems;
                if (languageMenuItems != null)
                {
                    foreach (ToolStripMenuItem languageMenuItem in languageMenuItems)
                    {
                        if (languageMenuItem.Tag.ToString() == actualLanguage)
                        {
                            languageMenuItem.Checked = true;
                            break;
                        }
                    }
                }
            }
        }

        private void ToolStripMenuItemLanguageItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem target = sender as ToolStripMenuItem;
            if (target != null)
            {
                var languageMenuItems = ToolStripMenuItemLanguage.DropDownItems;
                if (languageMenuItems != null)
                {
                    foreach (ToolStripMenuItem languageMenuItem in languageMenuItems)
                    {
                        languageMenuItem.Checked = false;
                    }
                }
                target.Checked = true;
                Language = target.Tag as string;
                L10N.SetLang(this, Language);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var fs = File.Open("setting.xml", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                Settings settings = new Settings();
                settings.AoSPath = defaultAosPath;
                settings.HodPath = defaultHodPath;
                settings.useHackSupport = useHackSupport;
                settings.Language = Language;
                xmlSerializer.Serialize(sw, settings);
                sw.Flush();
            }
        }

        private void clearAll()
        {
            RoomToDraw.Clear();
            PositionToDraw.Clear();
            PositionPreviewToDraw.Clear();
            LinesToDraw.Clear();
            listSourceRoom.Items.Clear();
            SourceRoomPointers.Clear();
        }

        //选择房间（两种模式通用）
        private void selectRoom(int actualX, int actualY)
        {
            clearAll();
            var point = new Point(actualX, actualY);
            selectedRoom = null;
            originalSelectedRoom = null;
            comboRoomList.Items.Clear();
            if (rom.RoomsAtPositions.ContainsKey(point))
            {
                selectedRoom = rom.RoomsAtPositions[point];
                if (selectedRoom != null && selectedRoom.Room != null)
                {
                    originalSelectedRoom = selectedRoom;
                    textRoomPointer.Text = selectedRoom.Room.RoomPointer.ToString();
                    textSector.Text = selectedRoom.MapSector.ToString("X2");
                    textRoomId.Text = selectedRoom.RoomId.ToString("X2");
                    var room = selectedRoom.Room;
                    comboRoomList.Items.Add(selectedRoom.Room.RoomPointer.ToString());
                    comboRoomList.SelectedIndex = 0;
                    if (rom.FlagRoomLists.ContainsKey(room.RoomPointer.Address))
                    {
                        foreach (var roomInfo in rom.FlagRoomLists[room.RoomPointer.Address])
                        {
                            if (roomInfo.Room.EventFlag != -1)
                            {
                                comboRoomList.Items.Add($"{roomInfo.Room.RoomPointer} (Flag={roomInfo.Room.EventFlag:X2})");
                            }
                            else
                            {
                                comboRoomList.Items.Add(roomInfo.Room.RoomPointer.ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                textRoomPointer.Text = "";
                textSector.Text = "";
                textRoomId.Text = "";
            }
        }

        //选择出城的源房间
        private void ListSourceRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedRoom == null || selectedRoom.Room == null) return;
            PositionToDraw.Clear();
            LinesToDraw.Clear();
            if (currentSourceRoomInListId != -1)
            {
                RoomToDraw[currentSourceRoomInListId + 1].Brush = transparentBlackBrush;
            }
            var currentSourceRoomInList = (RoomAndExit)listSourceRoom.SelectedItem;
            displaySourceRoom(currentSourceRoomInList);
        }

        private void ToolStripMenuItemHackSupport_Click(object sender, EventArgs e)
        {
            if (ToolStripMenuItemHackSupport.CheckState == CheckState.Checked)
            {
                ToolStripMenuItemHackSupport.CheckState = CheckState.Unchecked;
                useHackSupport = false;
            }
            else
            {
                ToolStripMenuItemHackSupport.CheckState = CheckState.Checked;
                useHackSupport = true;
            }
            if (rom != null)
            {
                rom.UseHackSupport = useHackSupport;
                ReloadRom();
            }
        }

        private void trackBarSearchOption_Scroll(object sender, EventArgs e)
        {
            setSearchOptionText();
            if (rom == null) return;
            PositionToDraw.Clear();
            PositionPreviewToDraw.Clear();
            LinesToDraw.Clear();
            listSourceRoom.Items.Clear();
            SourceRoomPointers.Clear();
            currentSourceRoomInListId = -1;
            rom.FindWarpSource(trackBarSearchOption.Value);
            pictureMap.Refresh();
        }

        private void setSearchOptionText()
        {
            for (int i = 0; i < labelOptionSearches.Count; i++)
            {
                if (i < trackBarSearchOption.Value)
                {
                    labelOptionSearches[i].Text = L10N.GetText("YesMark");
                }
                else
                {
                    labelOptionSearches[i].Text = L10N.GetText("NoMark");
                }
            }
            if (rom != null && rom.GameType == GameTypeEnum.Aos)
            {
                labelOptionSearches[0].Text = L10N.GetText("NoMark");
                labelOptionSearches[2].Text = L10N.GetText("NoMark");
            }
        }
    }

    public class Settings
    {
        public string AoSPath { get; set; }
        public string HodPath { get; set; }
        public bool useHackSupport { get; set; }
        public string Language { get; set; }
    }
    
    public class RectangleToDraw
    {
        public Brush Brush { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public RectangleToDraw(Brush brush, int left, int top, int width, int height)
        {
            Brush = brush;
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }

        public void Draw(Graphics g, int offset, float gridSize)
        {
            g?.FillRectangle(Brush, (Left + offset) * gridSize, (Top + offset) * gridSize, Width * gridSize, Height * gridSize);
        }
    }

    public class LineToDraw
    {
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }

        public LineToDraw(Pen pen, Brush brush, int startX, int startY, int endX, int endY)
        {
            Pen = pen;
            Brush = brush;
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }

        public void Draw(Graphics g, int offset, float gridSize)
        {
            var angle = Math.Atan2(EndY - StartY, EndX - StartX);
            var xDiff = (float)(0.5f * Math.Cos(angle));
            var yDiff = (float)(0.5f * Math.Sin(angle));

            var pointEnd = new PointF((EndX + offset + 0.5f - xDiff) * gridSize, (EndY + offset + 0.5f - yDiff) * gridSize);
            var pointStart = new PointF((StartX + offset + 0.5f) * gridSize, (StartY + offset + 0.5f) * gridSize);
            g?.DrawLine(Pen, pointStart, pointEnd);

            var pointEndCenter = new PointF((EndX + offset + 0.5f) * gridSize, (EndY + offset + 0.5f) * gridSize);
            var maxLength = ((EndY - StartY) * (EndY - StartY) + (EndX - StartX) * (EndX - StartX)) / 4f;
            var length = 0f;
            var angle1 = 0d;
            var angle2 = 0d;
            if (maxLength < 3.732051f)
            {
                length = (float)Math.Sqrt(maxLength);
                if (maxLength < 0.4f)
                {
                    length *= 1.41421354f;
                    angle1 = angle + Math.PI / 4;
                    angle2 = angle - Math.PI / 4;
                }
                else if (maxLength < 1.4f)
                {
                    length *= 1.15470052f;
                    angle1 = angle + Math.PI / 6;
                    angle2 = angle - Math.PI / 6;
                }
                else if (maxLength < 2.6f)
                {
                    length *= 1.08239222f;
                    angle1 = angle + Math.PI / 8;
                    angle2 = angle - Math.PI / 8;
                }
                else
                {
                    length *= 1.03527617f;
                    angle1 = angle + Math.PI / 12;
                    angle2 = angle - Math.PI / 12;
                }
            }
            else
            {
                length = 2f;
                angle1 = angle + Math.PI / 12;
                angle2 = angle - Math.PI / 12;
            }
            var point1 = new PointF((EndX + offset + 0.5f - (float)Math.Cos(angle1) * length) * gridSize, (EndY + offset + 0.5f - (float)Math.Sin(angle1) * length) * gridSize);
            var point2 = new PointF((EndX + offset + 0.5f - (float)Math.Cos(angle2) * length) * gridSize, (EndY + offset + 0.5f - (float)Math.Sin(angle2) * length) * gridSize);
            g?.FillPolygon(Brush, new PointF[] { pointEndCenter, point1, point2 });
        }
    }

    public enum OperationMode
    {
        FindDestination,
        FindSource
    }
}
