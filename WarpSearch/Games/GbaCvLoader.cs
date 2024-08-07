﻿using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using WarpSearch.Lang;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public static class GbaCvLoader
    {
        public static GbaCv LoadGame(string fileName, FormMain formMain, bool forceCustom)
        {
            int fileLength = 0;
            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Length > 33554432)
                {
                    MessageBox.Show(formMain, L10N.GetText("FileTooLarge"));
                    return null;
                }
                fileLength = (int)fileInfo.Length;
            }
            byte[] data = new byte[fileLength];

            var fs = File.Open(fileName, FileMode.Open);
            fs.Read(data, 0, fileLength);
            byte[] gameSign = new byte[18];
            fs.Seek(0xa0, SeekOrigin.Begin);
            fs.Read(gameSign, 0, 18);
            fs.Flush();
            fs.Close();

            if (forceCustom)
            {
                new FormCustomGame(data, formMain, fileName).ShowDialog(formMain);
                return null;
            }
            else
            {
                GbaCv result = null;
                //先检查是否有已保存的ROM设置
                RomSettings romSettings = formMain.GetRomSettings(fileName);
                if (romSettings != null)
                {
                    if (romSettings.GameType == GameTypeEnum.Hod)
                    {
                        result = new HoDCustom(data, romSettings.RoomPointer, romSettings.MapPointer, romSettings.MapLinePointer, romSettings.GameVersion);
                    }
                    else if (romSettings.GameType == GameTypeEnum.Aos)
                    {
                        result = new AoSCustom(data, romSettings.RoomPointer, romSettings.MapPointer, romSettings.MapLinePointer, romSettings.GameVersion);
                    }
                    if (result != null)
                    {
                        result.FileName = fileName;
                        result.GameVersion = romSettings.GameVersion;
                        result.IsCustom = true;
                        //每次打开都重新插入，排到最后
                        formMain.InsertOrUpdateRomSettings(romSettings);
                        return result;
                    }
                }
                string gameSignStr = Encoding.ASCII.GetString(gameSign);
                GameVersionEnum gameVersion = GameVersionEnum.USA;
                switch (gameSignStr)
                {
                    case "CASTLEVANIA2A2CEA4":
                        result = new AoSUSA(data);
                        gameVersion = GameVersionEnum.USA;
                        break;
                    case "CASTLEVANIA2A2CPA4":
                        result = new AoSEUR(data);
                        gameVersion = GameVersionEnum.EUR;
                        break;
                    case "CASTLEVANIA2A2CJEM":
                        result = new AoSJPN(data);
                        gameVersion = GameVersionEnum.JPN;
                        break;
                    case "CASTLEVANIA1ACHEA4":
                        result = new HoDUSA(data);
                        gameVersion = GameVersionEnum.USA;
                        break;
                    case "CASTLEVANIA1ACHPA4":
                        result = new HoDEUR(data);
                        gameVersion = GameVersionEnum.EUR;
                        break;
                    case "CASTLEVANIA1ACHJEM":
                        result = new HoDJPN(data);
                        gameVersion = GameVersionEnum.JPN;
                        break;
                    default:
                        var confirmResult = MessageBox.Show(formMain, L10N.GetText("UnknownRom"), formMain.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (confirmResult == DialogResult.OK)
                        {
                            new FormCustomGame(data, formMain, fileName).ShowDialog(formMain);
                        }
                        return null;
                }
                if (result != null)
                {
                    result.FileName = fileName;
                    result.GameVersion = gameVersion;
                }
                return result;
            }
        }

        public static void ResetCustomRom(GbaCv game, FormMain formMain)
        {
            new FormCustomGame(game, formMain).ShowDialog(formMain);
        }

        public static List<SpecialRoomData> getSpecialRooms(GameTypeEnum gameType, List<RomPointer> romPointers)
        {
            List<SpecialRoomData> result = null;
            if (gameType == GameTypeEnum.Aos)
            {
                result = new List<SpecialRoomData>()
                {
                    new SpecialRoomData(45, 51, 45, 51, 3, 3, 3, 3, romPointers[0]),

                    new SpecialRoomData(18, 53, 18, 53, 3, 3, 3, 2, romPointers[1]),

                    new SpecialRoomData(19, 53, 19, 53, 3, 3, 2, 0, romPointers[2]),
                    new SpecialRoomData(20, 53, 19, 53, 3, 3, 0, 0, romPointers[2]),
                    new SpecialRoomData(21, 53, 19, 53, 3, 3, 0, 2, romPointers[2]),

                    new SpecialRoomData(22, 53, 22, 53, 3, 3, 2, 0, romPointers[3]),
                    new SpecialRoomData(23, 53, 22, 53, 3, 3, 0, 2, romPointers[3]),

                    new SpecialRoomData(24, 53, 24, 53, 3, 3, 2, 0, romPointers[4]),
                    new SpecialRoomData(25, 53, 24, 53, 3, 3, 0, 2, romPointers[4]),

                    new SpecialRoomData(26, 53, 26, 53, 3, 3, 2, 0, romPointers[5]),
                    new SpecialRoomData(27, 53, 26, 53, 3, 3, 0, 2, romPointers[5]),

                    new SpecialRoomData(28, 53, 28, 53, 3, 3, 2, 0, romPointers[6]),
                    new SpecialRoomData(29, 53, 28, 53, 3, 3, 0, 2, romPointers[6]),

                    new SpecialRoomData(30, 53, 30, 53, 3, 3, 2, 0, romPointers[7]),
                    new SpecialRoomData(31, 53, 30, 53, 3, 3, 0, 2, romPointers[7]),

                    new SpecialRoomData(32, 53, 32, 53, 3, 3, 2, 0, romPointers[8]),
                    new SpecialRoomData(33, 53, 32, 53, 3, 3, 0, 2, romPointers[8]),

                    new SpecialRoomData(34, 53, 34, 52, 0, 3, 2, 0, romPointers[9]),
                    new SpecialRoomData(35, 53, 34, 52, 0, 3, 0, 2, romPointers[9]),
                    new SpecialRoomData(34, 52, 34, 52, 3, 0, 3, 0, romPointers[9]),
                    new SpecialRoomData(35, 52, 34, 52, 3, 0, 0, 3, romPointers[9]),

                    new SpecialRoomData(36, 53, 36, 53, 3, 3, 2, 0, romPointers[10]),
                    new SpecialRoomData(37, 53, 36, 53, 3, 3, 0, 2, romPointers[10]),

                    new SpecialRoomData(38, 53, 38, 53, 3, 3, 2, 2, romPointers[11]),

                    new SpecialRoomData(39, 53, 39, 53, 3, 3, 2, 3, romPointers[12]),

                    new SpecialRoomData(18, 50, 18, 50, 3, 3, 3, 2, romPointers[13]),

                    new SpecialRoomData(19, 50, 19, 50, 3, 3, 2, 0, romPointers[14]),
                    new SpecialRoomData(20, 50, 19, 50, 3, 3, 0, 2, romPointers[14]),

                    new SpecialRoomData(21, 50, 21, 50, 3, 3, 2, 0, romPointers[15]),
                    new SpecialRoomData(22, 50, 21, 50, 3, 3, 0, 2, romPointers[15]),

                    new SpecialRoomData(23, 50, 23, 50, 3, 3, 2, 0, romPointers[16]),
                    new SpecialRoomData(24, 50, 23, 50, 3, 3, 0, 2, romPointers[16]),

                    new SpecialRoomData(25, 50, 25, 50, 3, 3, 2, 0, romPointers[17]),
                    new SpecialRoomData(26, 50, 25, 50, 3, 3, 0, 2, romPointers[17]),

                    new SpecialRoomData(27, 50, 27, 50, 3, 3, 2, 0, romPointers[18]),
                    new SpecialRoomData(28, 50, 27, 50, 3, 3, 0, 2, romPointers[18]),

                    new SpecialRoomData(29, 50, 29, 50, 3, 3, 2, 0, romPointers[19]),
                    new SpecialRoomData(30, 50, 29, 50, 3, 3, 0, 2, romPointers[19]),

                    new SpecialRoomData(31, 50, 31, 50, 3, 3, 2, 0, romPointers[20]),
                    new SpecialRoomData(32, 50, 31, 50, 3, 3, 0, 2, romPointers[20]),

                    new SpecialRoomData(33, 50, 33, 50, 3, 3, 2, 0, romPointers[21]),
                    new SpecialRoomData(34, 50, 33, 50, 3, 3, 0, 2, romPointers[21]),

                    new SpecialRoomData(35, 50, 35, 50, 3, 3, 2, 0, romPointers[22]),
                    new SpecialRoomData(36, 50, 35, 50, 3, 3, 0, 2, romPointers[22]),

                    new SpecialRoomData(37, 50, 37, 50, 3, 3, 2, 0, romPointers[23]),
                    new SpecialRoomData(38, 50, 37, 50, 3, 3, 0, 2, romPointers[23]),

                    new SpecialRoomData(39, 50, 39, 50, 3, 3, 2, 3, romPointers[24]),
                };

            }
            else if (gameType == GameTypeEnum.Hod)
            {

                result = new List<SpecialRoomData>()
                {
                    new SpecialRoomData(69, 37, 67, 37, 3, 0, 3, 2, romPointers[0]),
                    new SpecialRoomData(69, 38, 67, 37, 0, 3, 0, 3, romPointers[0]),
                    new SpecialRoomData(67, 38, 67, 37, 3, 3, 3, 0, romPointers[0]),
                    new SpecialRoomData(68, 38, 67, 37, 3, 3, 0, 0, romPointers[0]),

                    new SpecialRoomData(69, 41, 67, 41, 3, 0, 3, 2, romPointers[1]),
                    new SpecialRoomData(69, 42, 67, 41, 0, 3, 0, 3, romPointers[1]),
                    new SpecialRoomData(67, 42, 67, 41, 3, 3, 2, 0, romPointers[1]),
                    new SpecialRoomData(68, 42, 67, 41, 3, 3, 0, 0, romPointers[1]),

                    new SpecialRoomData(72, 36, 72, 36, 3, 3, 2, 0, romPointers[2]),
                    new SpecialRoomData(73, 36, 72, 36, 3, 3, 0, 2, romPointers[2]),

                    new SpecialRoomData(72, 38, 72, 38, 3, 3, 2, 0, romPointers[3]),
                    new SpecialRoomData(73, 38, 72, 38, 3, 3, 0, 2, romPointers[3]),

                    new SpecialRoomData(72, 40, 72, 40, 3, 3, 2, 0, romPointers[4]),
                    new SpecialRoomData(73, 40, 72, 40, 3, 3, 0, 2, romPointers[4]),

                    new SpecialRoomData(72, 42, 72, 42, 3, 3, 2, 0, romPointers[5]),
                    new SpecialRoomData(73, 42, 72, 42, 3, 3, 0, 3, romPointers[5]),
                };


            }
            return result;
        }
    }
}
