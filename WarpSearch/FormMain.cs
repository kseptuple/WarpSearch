using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WarpSearch.Games;
using System.Xml.Serialization;
using WarpSearch.Lang;
using WarpSearch.Common;

namespace WarpSearch
{
    public partial class FormMain : Form
    {
        private float scale = 3;
        private float lineWidth => scale;
        private float gridSize => scale * 4;
        private Bitmap map = null;
        private GbaCv rom = null;

        private string originalTitle = null;

        private Graphics bitmapGraphics = null;

        private List<uint> SourceRoomPointers = new List<uint>();

        private List<RectangleToDraw> RectanglesToDraw = new List<RectangleToDraw>();
        private List<RectangleToDraw> SquaresToDraw = new List<RectangleToDraw>();
        private List<RectangleToDraw> SquarePreviewsToDraw = new List<RectangleToDraw>();
        private List<LineToDraw> LinesToDraw = new List<LineToDraw>();
        private List<Point> GreenRooms = new List<Point>();

        private OperationMode opMode = OperationMode.FindDestination;

        private string defaultAosPath = null;
        private string defaultHodPath = null;

        private GameTypeEnum romType = GameTypeEnum.Null;

        private RoomStruct selectedRootRoom = null;

        public RoomStruct selectedRoom = null;
        public Point selectedPos = default;

        public RoomStruct sourceRoom = null;
        public RoomStruct destRoom = null;

        private int globalOffset = 2;

        private List<Label> labelOptionSearches = new List<Label>();

        private Dictionary<RoomAndExit, Dictionary<uint, byte>> flagListForRoom = new Dictionary<RoomAndExit, Dictionary<uint, byte>>();

        public string Language { get; set; }

        private List<RomSettings> romSettings = new List<RomSettings>();
        private const int maxRomSettings = 1000;

        private bool isChangingNumericSize = false;
        private bool isAddingToRoomList = false;

        public FormTechInfo TechInfoForm { get; set; }

        public FormMain()
        {
            InitializeComponent();
            redPen.Width = scale;
            orangePen.Width = scale;
            labelOptionSearches.Add(labelSearchOption1);
            labelOptionSearches.Add(labelSearchOption2);
            labelOptionSearches.Add(labelSearchOption3);
            labelOptionSearches.Add(labelSearchOption4);
            labelOptionSearches.Add(labelSearchOption5);
            toolStripStatusRomType.Text = string.Empty;
        }

        private void PictureMap_Click(object sender, EventArgs e)
        {
            if (rom == null) return;
            ActiveControl = null;
            var mouseEvent = e as MouseEventArgs;
            if (mouseEvent != null)
            {
                var actualX = (int)Math.Floor((mouseEvent.X - gridSize * globalOffset) / (float)gridSize);
                var actualY = (int)Math.Floor((mouseEvent.Y - gridSize * globalOffset) / (float)gridSize);
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
                                SquaresToDraw.Clear();
                                LinesToDraw.Clear();
                                var left = actualX;
                                var top = actualY;
                                selectedPos = new Point(left, top);
                                FindAndDrawWarpDestination(selectedPos, out int destX, out int destY);
                                if (sourceRoom != null && destRoom != null)
                                {
                                    textSrcRoomPointer.Text = sourceRoom.RoomPointer.ToString();
                                    textDestRoomPointer.Text = destRoom.RoomPointer.ToString();
                                    textSrcRoomExit.Text = getPositionFormattedString(left - sourceRoom.Left, top - sourceRoom.Top);
                                    textDestRoomPos.Text = getPositionFormattedString(destX - destRoom.Left, destY - destRoom.Top);
                                    if (destRoom.EventFlag != -1)
                                    {
                                        textDestFlag.Text = destRoom.EventFlag.ToString("X2");
                                    }
                                    else
                                    {
                                        textDestFlag.Text = "";
                                    }
                                }
                                else
                                {
                                    textSrcRoomPointer.Text = "";
                                    textDestRoomPointer.Text = "";
                                    textSrcRoomExit.Text = "";
                                    textDestRoomPos.Text = "";
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
                                selectRoom(actualX, actualY);
                                //选择房间的时候已经找到了源房间（从ComboRoomList_SelectedIndexChanged调用），不需要再搜索了
                                pictureMap.Refresh();
                            }
                            else if (mouseEvent.Button == MouseButtons.Left)
                            {
                                if (selectedRoom == null || selectedRoom == null) return;
                                SquaresToDraw.Clear();
                                LinesToDraw.Clear();

                                var point = new Point(actualX, actualY);
                                RoomStruct currentSourceRoom = null;
                                textSrcRoomPointer.Text = "";
                                textDestRoomPointer.Text = "";
                                textSrcRoomExit.Text = "";
                                textDestRoomPos.Text = "";
                                textDestFlag.Text = "";
                                listSourceRoom.SelectedIndex = -1;
                                bool hasValidRoom = false;
                                if (rom.RoomsAtPositions.ContainsKey(point))
                                {
                                    currentSourceRoom = rom.RoomsAtPositions[point];
                                    if (currentSourceRoom != null)
                                    {
                                        List<RoomStruct> currentRoomList = new List<RoomStruct> { currentSourceRoom };
                                        if (currentSourceRoom.OverlappingRooms.Count > 0)
                                        {
                                            foreach (var room in currentSourceRoom.OverlappingRooms)
                                            {
                                                if (room.Left <= actualX && room.Top <= actualY &&
                                                    room.Left + room.Width > actualX && room.Top + room.Height > actualY)
                                                    currentRoomList.Add(room);
                                            }
                                        }
                                        isAddingToRoomList = true;
                                        listFlag.Items.Clear();
                                        int roomAndExitCount = 0;

                                        RoomAndExit currentSourceRoomInList = null;
                                        bool hasFlag = false;
                                        bool updateExitInfo = true;
                                        TechInfoForm?.CleatText();
                                        for (int i = 0; i < listSourceRoom.Items.Count; i++)
                                        {
                                            foreach (var currentRoom in currentRoomList)
                                            {
                                                currentSourceRoomInList = (RoomAndExit)listSourceRoom.Items[i];
                                                if (currentSourceRoomInList.Room.RoomPointer == currentRoom.RoomPointer)
                                                {
                                                    hasValidRoom = true;
                                                    displaySourceRoom(currentSourceRoomInList, out bool currentRoomHasFlag, updateExitInfo);
                                                    if (updateExitInfo)
                                                    {
                                                        listSourceRoom.SelectedIndex = i;
                                                        updateExitInfo = false;
                                                    }
                                                    roomAndExitCount++;
                                                    hasFlag |= currentRoomHasFlag;
                                                }
                                            }
                                        }
                                        panelFlag.Visible = hasFlag;
                                        if (roomAndExitCount == 1 && hasFlag)
                                        {
                                            listFlag.Items.RemoveAt(0);
                                        }
                                        isAddingToRoomList = false;
                                    }
                                }
                                if (!hasValidRoom)
                                {
                                    panelFlag.Visible = false;
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

        private void displaySourceRoom(RoomAndExit currentSourceRoomInList, out bool hasFlag, bool updateExitInfo = true)
        {
            hasFlag = false;
            for (int j = 0; j < SourceRoomPointers.Count; j++)
            {
                if (SourceRoomPointers[j] == currentSourceRoomInList.Room.RoomPointer)
                {
                    RectanglesToDraw[j + 1].Brush = transparentWhiteBrush;
                }
                else
                {
                    RectanglesToDraw[j + 1].Brush = transparentBlackBrush;
                }
            }
            bool isOutsideDest = currentSourceRoomInList.IsDestOutside;
            bool isUncertain = currentSourceRoomInList.IsUncertain;
            var sourceX = currentSourceRoomInList.Room.Left + currentSourceRoomInList.Exit.SourceX;
            var sourceY = currentSourceRoomInList.Room.Top + currentSourceRoomInList.Exit.SourceY;
            var destX = selectedRoom.Left + currentSourceRoomInList.Exit.DestX;
            var destY = selectedRoom.Top + currentSourceRoomInList.Exit.DestY;

            //if (currentSourceRoomInList.Exit.DestX < 0 || currentSourceRoomInList.Exit.DestX > selectedRoom.Width - 1
            //    || currentSourceRoomInList.Exit.DestY < 0 || currentSourceRoomInList.Exit.DestY > selectedRoom.Height - 1)
            //{
            //    Debug.Assert(currentSourceRoomInList.IsUncertain);
            //    Debug.Assert(currentSourceRoomInList.IsDestOutside);
            //    isOutsideDest = true;
            //    isUncertain = true;
            //}

            AddMapSquarePos(sourceX, sourceY, false, false, isUncertain);
            AddMapSquarePos(destX, destY, false, false, isUncertain);

            if (sourceX != destX || sourceY != destY)
            {
                AddLine(sourceX, sourceY, destX, destY, true);
            }
            if (isOutsideDest)
            {
                AddLine(selectedRoom.Left, selectedRoom.Top, destX, destY, false);
            }
            pictureMap.Refresh();
            if (updateExitInfo)
            {
                textSrcRoomPointer.Text = currentSourceRoomInList.Room.RoomPointer.ToString();
                textDestRoomPointer.Text = selectedRoom.RoomPointer.ToString();
                textSrcRoomExit.Text = getPositionFormattedString(currentSourceRoomInList.Exit.SourceX, currentSourceRoomInList.Exit.SourceY);
                textDestRoomPos.Text = getPositionFormattedString(currentSourceRoomInList.Exit.DestX, currentSourceRoomInList.Exit.DestY);
            }

            if (selectedRoom.EventFlag != -1)
            {
                textDestFlag.Text = selectedRoom.EventFlag.ToString("X2");
            }
            if (isAddingToRoomList)
            {
                //在地图点选
                if (listFlag.Items.Count != 0)
                {
                    listFlag.Items.Add("===========");
                }

                listFlag.Items.Add($"{currentSourceRoomInList.Room.RoomPointer}" +
                    $" ({currentSourceRoomInList.Exit.SourceX},{currentSourceRoomInList.Exit.SourceY}):");
                if (flagListForRoom.ContainsKey(currentSourceRoomInList))
                {
                    var flagList = flagListForRoom[currentSourceRoomInList];
                    addFlagToFlagList(flagList);
                    hasFlag = true;
                }
                else
                {
                    listFlag.Items.Add("-");
                }
            }
            else
            {
                //在房间列表点选
                if (flagListForRoom.ContainsKey(currentSourceRoomInList))
                {
                    listFlag.Items.Clear();

                    var flagList = flagListForRoom[currentSourceRoomInList];
                    addFlagToFlagList(flagList);
                    hasFlag = true;
                    panelFlag.Show();
                }
                else
                {
                    panelFlag.Hide();
                }
                textSrcRoomExit.Text = getPositionFormattedString(currentSourceRoomInList.Exit.SourceX, currentSourceRoomInList.Exit.SourceY);
                textDestRoomPos.Text = getPositionFormattedString(currentSourceRoomInList.Exit.DestX, currentSourceRoomInList.Exit.DestY);
            }
            TechInfoForm?.AppendText(currentSourceRoomInList.WarpTechInfo, romType, opMode);
        }

        public void SetFlagListForDestSearch(Dictionary<uint, byte> flagList)
        {
            if (flagList != null && flagList.Count != 0)
            {
                listFlag.Items.Clear();
                addFlagToFlagList(flagList);
                panelFlag.Show();
            }
            else
            {
                panelFlag.Hide();
            }
        }

        private void addFlagToFlagList(Dictionary<uint, byte> flagList)
        {
            if (flagList == null) return;
            foreach (var roomFlag in flagList)
            {
                listFlag.Items.Add($"{roomFlag.Key:X8}:{roomFlag.Value:X2}");
            }
        }

        private string getPositionFormattedString(int X, int Y)
        {
            return $"({X},{Y})";
        }

        public void FindAndDrawWarpDestination(Point selectedPos, out int DestX, out int DestY, bool previewOnly = false)
        {
            ClearPos(previewOnly);
            if (!previewOnly)
            {
                ClearLine();
                if (RectanglesToDraw.Count > 1)
                {
                    RectanglesToDraw.RemoveAt(1);
                }
            }

            int sourceX = 0, sourceY = 0;
            DestX = 0;
            DestY = 0;
            var warpResult = rom.FindWarpDestination(selectedRoom, selectedPos);
            if (warpResult?.WarpRooms?.Count > 0)
            {
                for (int i = 0; i < warpResult.WarpRooms.Count; i++)
                {
                    var roomToDraw = warpResult.WarpRooms[i];
                    if (roomToDraw.IsStartRoom)
                    {
                        sourceX = roomToDraw.X;
                        sourceY = roomToDraw.Y;
                        AddMapSquarePos(roomToDraw.X, roomToDraw.Y, warpResult.IsBadWarp, previewOnly, warpResult.IsUncertainWarp);
                    }
                    else
                    {
                        if (!previewOnly && !warpResult.IsBadWarp)
                        {
                            DestX = roomToDraw.X;
                            DestY = roomToDraw.Y;
                            AddMapSquarePos(roomToDraw.X, roomToDraw.Y, warpResult.IsBadWarp, previewOnly, warpResult.IsUncertainWarp);
                        }
                    }
                }

                if (!previewOnly)
                {
                    if (warpResult.IsBadWarp)
                    {
                        sourceRoom = null;
                        destRoom = null;
                    }
                    else
                    {
                        sourceRoom = selectedRoom;
                        destRoom = warpResult.DestRoom;
                        SetFlagListForDestSearch(warpResult.FlagList);

                        //画线
                        if (sourceX != DestX || sourceY != DestY)
                        {
                            AddLine(sourceX, sourceY, DestX, DestY, true);
                        }
                        if (warpResult.IsDestOutside)
                        {
                            AddLine(destRoom.Left, destRoom.Top, DestX, DestY, false);
                            AddRoomRectangleToDraw(destRoom, false);
                        }
                    }
                    TechInfoForm?.SetText(warpResult.TechInfo, romType, opMode);
                }
            }
        }

        public void FindAndDrawWarpSource(int searchLevel)
        {
            ClearPos();
            ClearLine();
            ClearSourceRoomList();
            panelFlag.Visible = false;
            var sourceRooms = rom.FindWarpSource(selectedRoom, searchLevel);

            for (int i = 0; i < sourceRooms.Count; i++)
            {
                var sourceRoom = sourceRooms[i];
                AddSourceRoom(sourceRoom.Room, sourceRoom.Exit, sourceRoom.IsUncertain, sourceRoom.IsDestOutside, sourceRoom.TechInfo, sourceRoom.FlagList);
            }
        }

        public void AddSourceRoom(RoomStruct room, ExitInfo exit, bool isUncertain, bool isDestOutside, WarpTechInfo warpTechInfo, Dictionary<uint, byte> flagList = null)
        {
            var roomAndExit = new RoomAndExit(room, exit, isUncertain, isDestOutside, warpTechInfo);
            if (listSourceRoom.Items.Count == 0)
            {
                listSourceRoom.Items.Add(roomAndExit);
            }
            else
            {
                for (int i = 0; i < listSourceRoom.Items.Count; i++)
                {
                    var currentRoomAndExit = (RoomAndExit)listSourceRoom.Items[i];
                    if (currentRoomAndExit.Room.RoomPointer > room.RoomPointer)
                    {
                        listSourceRoom.Items.Insert(i, roomAndExit);
                        break;
                    }
                    if (currentRoomAndExit.Room.RoomPointer == room.RoomPointer && currentRoomAndExit.Exit.ExitPointer == exit.ExitPointer)
                    {
                        break;
                    }
                    if (i == listSourceRoom.Items.Count - 1)
                    {
                        listSourceRoom.Items.Add(roomAndExit);
                        break;
                    }
                }
            }

            if (!SourceRoomPointers.Contains(room.RoomPointer))
            {
                SourceRoomPointers.Add(room.RoomPointer);
                AddRoomRectangleToDraw(room, true);
            }
            if (flagList != null && flagList.Count != 0 && !flagListForRoom.ContainsKey(roomAndExit))
            {
                flagListForRoom.Add(roomAndExit, flagList);
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
                        SquarePreviewsToDraw.Clear();
                        var left = actualX;
                        var top = actualY;
                        selectedPos = new Point(left, top);
                        FindAndDrawWarpDestination(selectedPos, out _, out _, true);
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
            clearDest();
            if (comboRoomList.SelectedIndex == 0)
            {
                selectedRoom = selectedRootRoom;
            }
            else
            {
                selectedRoom = selectedRootRoom.OverlappingRooms[comboRoomList.SelectedIndex - 1];
            }
            var room = selectedRoom;
            AddRoomRectangleToDraw(room, false);
            if (opMode == OperationMode.FindSource)
            {
                flagListForRoom.Clear();
                FindAndDrawWarpSource(trackBarSearchOption.Value);
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
                //if (!rom.IsCustom)
                //{
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
                //}
            }
        }

        private void ToolStripMenuItemOpenCustom_Click(object sender, EventArgs e)
        {
            if (openFileDialogMain.ShowDialog() == DialogResult.Cancel) return;
            OpenRom(openFileDialogMain.FileName, true);
        }

        private void ToolStripMenuItemAosLast_Click(object sender, EventArgs e)
        {
            OpenRom(defaultAosPath);
        }

        private void ToolStripMenuItemHodLast_Click(object sender, EventArgs e)
        {
            OpenRom(defaultHodPath);
        }

        private void toolStripStatusRomType_Click(object sender, EventArgs e)
        {
            GbaCvLoader.ResetCustomRom(rom, this);
        }

        private bool OpenRom(string romPath, bool forceCustom = false)
        {
            GbaCv newRom = GbaCvLoader.LoadGame(romPath, this, forceCustom);
            if (newRom != null)
            {
                LoadRom(newRom);
                return true;
            }
            return false;
        }

        public void OpenCustomRom(GbaCv newRom)
        {
            if (newRom != null)
            {
                LoadRom(newRom);
            }
        }

        private void LoadRom(GbaCv newRom)
        {
            pictureMap.Visible = true;
            if (newRom != rom)
            {
                rom?.Dispose();
                rom = newRom;
            }

            clearAll();
            GreenRooms.Clear();
            comboRoomList.Items.Clear();
            romType = rom.GameType;
            map = new Bitmap((int)Math.Floor((rom.MapWidth + 4) * gridSize), (int)Math.Floor((rom.MapHeight + 4) * gridSize));
            bitmapGraphics = Graphics.FromImage(map);
            rom.LoadRooms();
            DrawMap();
            setSearchOptionText();
            pictureMap.Image = map;
            pictureMap.Width = map.Width;
            pictureMap.Height = map.Height;
            selectedRoom = null;
            selectedRootRoom = null;
            string statusText = string.Empty;
            panelFlag.Visible = false;
            switch (rom.GameType)
            {
                case GameTypeEnum.Aos:
                    statusText = L10N.GetText("AoS");
                    break;
                case GameTypeEnum.Hod:
                    statusText = L10N.GetText("HoD");
                    break;
            }
            switch (rom.GameVersion)
            {
                case GameVersionEnum.USA:
                    statusText += L10N.GetText("USA");
                    break;
                case GameVersionEnum.JPN:
                    statusText += L10N.GetText("JPN");
                    break;
                case GameVersionEnum.EUR:
                    statusText += L10N.GetText("EUR");
                    break;
            }
            if (rom.IsCustom)
            {
                statusText += L10N.GetText("Custom");
            }

            Text = $"{originalTitle} - {newRom.FileName}";

            toolStripStatusRomType.Text = statusText;
            if (rom.GameType == GameTypeEnum.Aos)
            {
                labelOptionSearches[0].Enabled = false;
                labelSearchLevel1.Enabled = false;
                labelOptionSearches[2].Enabled = false;
                labelSearchLevel3.Enabled = false;
            }
            else
            {
                labelOptionSearches[0].Enabled = true;
                labelSearchLevel1.Enabled = true;
                labelOptionSearches[2].Enabled = true;
                labelSearchLevel3.Enabled = true;
            }
        }

        private void PictureMap_Paint(object sender, PaintEventArgs e)
        {
            foreach (var rect in RectanglesToDraw)
            {
                rect.Draw(e.Graphics, globalOffset, gridSize);
            }

            foreach (var rect in SquaresToDraw)
            {
                rect.Draw(e.Graphics, globalOffset, gridSize);
            }

            foreach (var rect in SquarePreviewsToDraw)
            {
                rect.Draw(e.Graphics, globalOffset, gridSize);
            }

            foreach (var line in LinesToDraw)
            {
                line.Draw(e.Graphics, globalOffset, gridSize);
            }
        }

        private void RadioButtonFindDest_CheckedChanged(object sender, EventArgs e)
        {
            panelTop.Enabled = true;
            panelBottom.Enabled = false;
            panelFlag.Visible = false;
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
            panelFlag.Visible = false;
            clearAll();
            selectedRoom = null;
            comboRoomList.Items.Clear();
            pictureMap.Refresh();
            opMode = OperationMode.FindSource;
        }

        private void TrackBarResize_Scroll(object sender, EventArgs e)
        {
            isChangingNumericSize = true;
            textBoxResize.Text = ((decimal)trackBarResize.Value / 100).ToString();
            formatNumericSize();
            resizeMap();
            isChangingNumericSize = false;
        }

        private void textBoxResize_TextChanged(object sender, EventArgs e)
        {
            if (!isChangingNumericSize)
            {
                checkNumericSize();
                resizeMap();
            }
        }

        private void textBoxResize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
            {
                e.KeyChar = '.';
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && textBoxResize.Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void textBoxResize_Validating(object sender, CancelEventArgs e)
        {
            formatNumericSize();
        }

        private void checkNumericSize()
        {
            if (!isChangingNumericSize)
            {
                isChangingNumericSize = true;
                decimal value = 0;
                if (decimal.TryParse(textBoxResize.Text, out value))
                {
                    decimal newValue = value;
                    if (value * 100 < trackBarResize.Minimum) newValue = (decimal)trackBarResize.Minimum / 100;
                    if (value * 100 > trackBarResize.Maximum) newValue = (decimal)trackBarResize.Maximum / 100;

                    if (newValue != value)
                    {
                        textBoxResize.Text = newValue.ToString();
                        formatNumericSize();
                    }
                    trackBarResize.Value = (int)(newValue * 100);
                    if (textBoxResize.Text.Length - textBoxResize.Text.IndexOf('.') > 3)
                    {
                        textBoxResize.Text = textBoxResize.Text.Substring(0, textBoxResize.Text.IndexOf('.') + 3);
                    }
                }
                else
                {
                    if (textBoxResize.Text != "")
                    {
                        textBoxResize.Text = ((decimal)trackBarResize.Minimum / 100).ToString();
                    }
                    trackBarResize.Value = trackBarResize.Minimum;
                }
                isChangingNumericSize = false;
            }
        }

        private void formatNumericSize()
        {
            decimal value = 0;
            if (decimal.TryParse(textBoxResize.Text, out value))
            {
                textBoxResize.Text = value.ToString("0.00");
            }
            else
            {
                textBoxResize.Text = ((decimal)trackBarResize.Minimum / 100).ToString();
            }
        }

        private void resizeMap()
        {
            scale = trackBarResize.Value / 100.0f;
            if (rom == null) return;
            map = new Bitmap((int)Math.Floor((rom.MapWidth + 4) * gridSize), (int)Math.Floor((rom.MapHeight + 4) * gridSize));
            bitmapGraphics = Graphics.FromImage(map);
            DrawMap();
            pictureMap.Image = map;
            pictureMap.Width = map.Width;
            pictureMap.Height = map.Height;
            redPen.Width = scale;
            orangePen.Width = scale;
            pictureMap.Refresh();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            bool settingError = false;
            if (File.Exists("setting.xml"))
            {
                Settings settings = null;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                using (var fs = new FileStream("setting.xml", FileMode.Open))
                using (var sr = new StreamReader(fs))
                {
                    try
                    {
                        settings = (Settings)xmlSerializer.Deserialize(sr);
                    }
                    catch
                    {
                        settingError = true;
                    }
                }
                if (settings != null)
                {
                    defaultAosPath = settings.AoSPath;
                    if (!File.Exists(defaultAosPath))
                    {
                        defaultAosPath = string.Empty;
                    }
                    defaultHodPath = settings.HodPath;
                    if (!File.Exists(defaultHodPath))
                    {
                        defaultHodPath = string.Empty;
                    }
                    toolStripMenuItemAosLast.Enabled = !string.IsNullOrEmpty(defaultAosPath);
                    toolStripMenuItemHodLast.Enabled = !string.IsNullOrEmpty(defaultHodPath);
                    Language = settings.Language;
                    if (settings.Scale >= trackBarResize.Minimum && settings.Scale <= trackBarResize.Maximum)
                    {
                        textBoxResize.Text = ((decimal)settings.Scale / 100).ToString();
                        formatNumericSize();
                    }
                    if (settings.SearchLevel >= trackBarSearchOption.Minimum + 1 && settings.SearchLevel <= trackBarSearchOption.Maximum + 1)
                    {
                        trackBarSearchOption.Value = settings.SearchLevel - 1;
                    }
                    if (settings.FormWidth != 0 && settings.FormHeight != 0)
                    {
                        Rectangle currentScreen = Screen.FromControl(this).WorkingArea;

                        Top = settings.FormTop;
                        Left = settings.FormLeft;
                        if (Top < 0) Top = 0;
                        if (Left < 0) Left = 0;
                        if (Top > currentScreen.Height - MaximumSize.Height) Top = currentScreen.Height - MaximumSize.Height;
                        if (Left > currentScreen.Width - MaximumSize.Width) Left = currentScreen.Width - MaximumSize.Width;

                        Height = settings.FormHeight;
                        Width = settings.FormWidth;
                        if (Top + Height > currentScreen.Height) Height = currentScreen.Height - Top;
                        if (Left + Width > currentScreen.Width) Width = currentScreen.Width - Left;
                    }
                }
                if (settingError)
                {
                    File.Delete("setting.xml");
                }
            }

            settingError = false;
            if (File.Exists("rom.xml"))
            {
                List<RomSettings> romSettings = null;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<RomSettings>));
                using (var fs = new FileStream("rom.xml", FileMode.Open))
                using (var sr = new StreamReader(fs))
                {
                    try
                    {
                        romSettings = (List<RomSettings>)xmlSerializer.Deserialize(sr);
                    }
                    catch
                    {
                        settingError = true;
                    }
                }
                if (romSettings != null)
                {
                    this.romSettings = romSettings;
                }
                if (settingError)
                {
                    File.Delete("rom.xml");
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
                originalTitle = Text;
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
            setSearchOptionText();
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
                originalTitle = Text;
                TechInfoForm?.ChangeLang();
                if (rom != null)
                {
                    LoadRom(rom);
                }
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
                settings.Language = Language;
                settings.Scale = trackBarResize.Value;
                settings.SearchLevel = trackBarSearchOption.Value + 1;
                settings.FormWidth = Width;
                settings.FormHeight = Height;
                settings.FormLeft = Left;
                settings.FormTop = Top;
                xmlSerializer.Serialize(sw, settings);
                sw.Flush();
            }

            using (var fs = File.Open("rom.xml", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<RomSettings>));
                xmlSerializer.Serialize(sw, romSettings);
                sw.Flush();
            }
        }

        private void clearAll()
        {
            RectanglesToDraw.Clear();
            SquaresToDraw.Clear();
            SquarePreviewsToDraw.Clear();
            LinesToDraw.Clear();
            listSourceRoom.Items.Clear();
            SourceRoomPointers.Clear();
            textRoomPointer.Text = "";
            textSector.Text = "";
            textRoomId.Text = "";
            textSrcRoomPointer.Text = "";
            textDestRoomPointer.Text = "";
            textSrcRoomExit.Text = "";
            textDestRoomPos.Text = "";
            textDestFlag.Text = "";
            TechInfoForm?.SetInitText();
        }

        private void clearDest()
        {
            RectanglesToDraw.Clear();
            SquaresToDraw.Clear();
            SquarePreviewsToDraw.Clear();
            LinesToDraw.Clear();
            listSourceRoom.Items.Clear();
            SourceRoomPointers.Clear();
            textSrcRoomPointer.Text = "";
            textDestRoomPointer.Text = "";
            textSrcRoomExit.Text = "";
            textDestRoomPos.Text = "";
            textDestFlag.Text = "";
            TechInfoForm?.SetInitText();
        }

        //选择房间（两种模式通用）
        private void selectRoom(int actualX, int actualY)
        {
            clearAll();
            var point = new Point(actualX, actualY);
            selectedRoom = null;
            selectedRootRoom = null;
            comboRoomList.Items.Clear();
            panelFlag.Visible = false;
            if (rom.RoomsAtPositions.ContainsKey(point))
            {
                selectedRoom = rom.RoomsAtPositions[point];
                if (selectedRoom != null && selectedRoom != null)
                {
                    selectedRootRoom = selectedRoom;
                    textRoomPointer.Text = selectedRoom.RoomPointer.ToString();
                    if (selectedRoom.MapSector >= 0)
                    {
                        textSector.Text = selectedRoom.MapSector.ToString("X2");
                    }
                    else
                    {
                        textSector.Text = "-";
                    }
                    if (selectedRoom.RoomId >= 0)
                    {
                        textRoomId.Text = selectedRoom.RoomId.ToString("X2");
                    }
                    else
                    {
                        textRoomId.Text = "-";
                    }

                    var room = selectedRoom;
                    comboRoomList.Items.Add(selectedRoom.RoomPointer.ToString());
                    comboRoomList.SelectedIndex = 0;
                    if (room.OverlappingRooms.Count > 0)
                    {
                        foreach (var roomInfo in room.OverlappingRooms)
                        {
                            if (roomInfo.EventFlag != -1)
                            {
                                comboRoomList.Items.Add($"{roomInfo.RoomPointer} (Flag={roomInfo.EventFlag:X2})");
                            }
                            else
                            {
                                comboRoomList.Items.Add(roomInfo.RoomPointer.ToString());
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
            if (isAddingToRoomList) return;
            if (selectedRoom == null || selectedRoom == null) return;
            if (listSourceRoom.SelectedItem == null) return;
            SquaresToDraw.Clear();
            LinesToDraw.Clear();
            var currentSourceRoomInList = (RoomAndExit)listSourceRoom.SelectedItem;
            TechInfoForm?.CleatText();
            displaySourceRoom(currentSourceRoomInList, out _);
        }

        private void trackBarSearchOption_Scroll(object sender, EventArgs e)
        {
            setSearchOptionText();
            if (rom == null) return;
            SquaresToDraw.Clear();
            SquarePreviewsToDraw.Clear();
            LinesToDraw.Clear();
            listSourceRoom.Items.Clear();
            SourceRoomPointers.Clear();
            flagListForRoom.Clear();
            FindAndDrawWarpSource(trackBarSearchOption.Value);
            pictureMap.Refresh();
            TechInfoForm?.SetInitText();
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

        public void InsertOrUpdateRomSettings(RomSettings romSetting)
        {
            int existingRomSettingIndex = romSettings.FindIndex(r => r.RomPath == romSetting.RomPath);
            if (existingRomSettingIndex != -1)
            {
                romSettings.RemoveAt(existingRomSettingIndex);
            }

            romSettings.Add(romSetting);
            if (romSettings.Count > maxRomSettings)
            {
                romSettings.RemoveAt(0);
            }
        }

        public RomSettings GetRomSettings(string romPath)
        {
            return romSettings.FirstOrDefault(r => r.RomPath == romPath);
        }

        private void linkLabelTechInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (TechInfoForm == null)
            {
                TechInfoForm = new FormTechInfo(this);
                TechInfoForm.Show(this);
            }
            else
            {
                TechInfoForm.Activate();
            }
        }
    }

    public class Settings
    {
        public string AoSPath { get; set; }
        public string HodPath { get; set; }
        public string Language { get; set; }
        public int Scale { get; set; }
        public int SearchLevel { get; set; }
        public int FormTop { get; set; }
        public int FormLeft { get; set; }
        public int FormWidth { get; set; }
        public int FormHeight { get; set; }
    }

    public class RomSettings
    {
        public string RomPath { get; set; }
        public GameTypeEnum GameType { get; set; }
        public GameVersionEnum GameVersion { get; set; }
        public uint RoomPointer { get; set; }
        public uint MapPointer { get; set; }
        public uint MapLinePointer { get; set; }
    }

    public enum OperationMode
    {
        FindDestination,
        FindSource
    }
}
