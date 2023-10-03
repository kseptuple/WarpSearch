using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using WarpSearch.Lang;

namespace WarpSearch.Games
{
    public static class GbaCvLoader
    {
        public static GbaCv LoadGame(string fileName, FormMain formMain)
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

            string gameSignStr = Encoding.ASCII.GetString(gameSign);
            switch (gameSignStr)
            {
                case "CASTLEVANIA2A2CEA4":
                    return new AoSUSA(data, formMain);
                case "CASTLEVANIA2A2CPA4":
                    return new AoSEUR(data, formMain);
                case "CASTLEVANIA2A2CJEM":
                    return new AoSJPN(data, formMain);
                case "CASTLEVANIA1ACHEA4":
                    return new HoDUSA(data, formMain);
                case "CASTLEVANIA1ACHPA4":
                    return new HoDEUR(data, formMain);
                case "CASTLEVANIA1ACHJEM":
                    return new HoDJPN(data, formMain);
                default:
                    var result = MessageBox.Show(formMain, L10N.GetText("UnknownRom"), formMain.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        new FormCustomGame(data, formMain).Show(formMain);
                    }
                    return null;
            }
        }

        public static List<SpecialRoomData> getSpecialRooms(GameTypeEnum gameType, bool useHackSupport, List<ROMPointer> romPointers)
        {
            List<SpecialRoomData> result = null;
            if (gameType == GameTypeEnum.Aos)
            {
                if (useHackSupport)
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
                else
                {
                    result = new List<SpecialRoomData>()
                    {
                        new SpecialRoomData(13, 37, 13, 37, 3, 3, 3, 3, romPointers[0]),

                        new SpecialRoomData(21, 42, 21, 42, 3, 3, 3, 2, romPointers[1]),

                        new SpecialRoomData(22, 42, 22, 42, 3, 3, 2, 0, romPointers[2]),
                        new SpecialRoomData(23, 42, 21, 42, 3, 3, 0, 0, romPointers[2]),
                        new SpecialRoomData(24, 42, 21, 42, 3, 3, 0, 2, romPointers[2]),

                        new SpecialRoomData(25, 42, 25, 42, 3, 3, 2, 0, romPointers[3]),
                        new SpecialRoomData(26, 42, 25, 42, 3, 3, 0, 2, romPointers[3]),

                        new SpecialRoomData(27, 42, 27, 42, 3, 3, 2, 0, romPointers[4]),
                        new SpecialRoomData(28, 42, 27, 42, 3, 3, 0, 2, romPointers[4]),

                        new SpecialRoomData(29, 42, 29, 42, 3, 3, 2, 0, romPointers[5]),
                        new SpecialRoomData(30, 42, 29, 42, 3, 3, 0, 2, romPointers[5]),

                        new SpecialRoomData(31, 42, 31, 42, 3, 3, 2, 0, romPointers[6]),
                        new SpecialRoomData(32, 42, 31, 42, 3, 3, 0, 2, romPointers[6]),

                        new SpecialRoomData(33, 42, 33, 42, 3, 3, 2, 0, romPointers[7]),
                        new SpecialRoomData(34, 42, 33, 42, 3, 3, 0, 2, romPointers[7]),

                        new SpecialRoomData(35, 42, 35, 42, 3, 3, 2, 0, romPointers[8]),
                        new SpecialRoomData(36, 42, 35, 42, 3, 3, 0, 2, romPointers[8]),

                        new SpecialRoomData(37, 42, 37, 41, 0, 3, 2, 0, romPointers[9]),
                        new SpecialRoomData(38, 42, 37, 41, 0, 3, 0, 2, romPointers[9]),
                        new SpecialRoomData(37, 41, 37, 41, 3, 0, 3, 0, romPointers[9]),
                        new SpecialRoomData(38, 41, 37, 41, 3, 0, 0, 3, romPointers[9]),

                        new SpecialRoomData(39, 42, 39, 42, 3, 3, 2, 0, romPointers[10]),
                        new SpecialRoomData(40, 42, 39, 42, 3, 3, 0, 2, romPointers[10]),

                        new SpecialRoomData(41, 42, 41, 42, 3, 3, 2, 2, romPointers[11]),

                        new SpecialRoomData(42, 42, 42, 42, 3, 3, 2, 3, romPointers[12]),

                        new SpecialRoomData(21, 39, 21, 39, 3, 3, 3, 2, romPointers[13]),

                        new SpecialRoomData(22, 39, 22, 39, 3, 3, 2, 0, romPointers[14]),
                        new SpecialRoomData(23, 39, 22, 39, 3, 3, 0, 2, romPointers[14]),

                        new SpecialRoomData(24, 39, 24, 39, 3, 3, 2, 0, romPointers[15]),
                        new SpecialRoomData(25, 39, 24, 39, 3, 3, 0, 2, romPointers[15]),

                        new SpecialRoomData(26, 39, 26, 39, 3, 3, 2, 0, romPointers[16]),
                        new SpecialRoomData(27, 39, 26, 39, 3, 3, 0, 2, romPointers[16]),

                        new SpecialRoomData(28, 39, 28, 39, 3, 3, 2, 0, romPointers[17]),
                        new SpecialRoomData(29, 39, 28, 39, 3, 3, 0, 2, romPointers[17]),

                        new SpecialRoomData(30, 39, 30, 39, 3, 3, 2, 0, romPointers[18]),
                        new SpecialRoomData(31, 39, 30, 39, 3, 3, 0, 2, romPointers[18]),

                        new SpecialRoomData(32, 39, 32, 39, 3, 3, 2, 0, romPointers[19]),
                        new SpecialRoomData(33, 39, 32, 39, 3, 3, 0, 2, romPointers[19]),

                        new SpecialRoomData(34, 39, 34, 39, 3, 3, 2, 0, romPointers[20]),
                        new SpecialRoomData(35, 39, 34, 39, 3, 3, 0, 2, romPointers[20]),

                        new SpecialRoomData(36, 39, 36, 39, 3, 3, 2, 0, romPointers[21]),
                        new SpecialRoomData(37, 39, 36, 39, 3, 3, 0, 2, romPointers[21]),

                        new SpecialRoomData(38, 39, 38, 39, 3, 3, 2, 0, romPointers[22]),
                        new SpecialRoomData(39, 39, 38, 39, 3, 3, 0, 2, romPointers[22]),

                        new SpecialRoomData(40, 39, 40, 39, 3, 3, 2, 0, romPointers[23]),
                        new SpecialRoomData(41, 39, 40, 39, 3, 3, 0, 2, romPointers[23]),

                        new SpecialRoomData(42, 39, 42, 39, 3, 3, 2, 3, romPointers[24]),
                    };
                }
            }
            else if (gameType == GameTypeEnum.Hod)
            {
                if (useHackSupport)
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
                else
                {
                    result = new List<SpecialRoomData>()
                    {
                        new SpecialRoomData(44, 37, 42, 37, 3, 0, 3, 2, romPointers[0]),
                        new SpecialRoomData(44, 38, 42, 37, 0, 3, 0, 3, romPointers[0]),
                        new SpecialRoomData(42, 38, 42, 37, 3, 3, 3, 0, romPointers[0]),
                        new SpecialRoomData(43, 38, 42, 37, 3, 3, 0, 0, romPointers[0]),

                        new SpecialRoomData(44, 41, 42, 41, 3, 0, 3, 2, romPointers[1]),
                        new SpecialRoomData(44, 42, 42, 41, 0, 3, 0, 3, romPointers[1]),
                        new SpecialRoomData(42, 42, 42, 41, 3, 3, 2, 0, romPointers[1]),
                        new SpecialRoomData(43, 42, 42, 41, 3, 3, 0, 0, romPointers[1]),

                        new SpecialRoomData(47, 36, 47, 36, 3, 3, 2, 0, romPointers[2]),
                        new SpecialRoomData(48, 36, 47, 36, 3, 3, 0, 2, romPointers[2]),

                        new SpecialRoomData(47, 38, 47, 38, 3, 3, 2, 0, romPointers[3]),
                        new SpecialRoomData(48, 38, 47, 38, 3, 3, 0, 2, romPointers[3]),

                        new SpecialRoomData(47, 40, 47, 40, 3, 3, 2, 0, romPointers[4]),
                        new SpecialRoomData(48, 40, 47, 40, 3, 3, 0, 2, romPointers[4]),

                        new SpecialRoomData(47, 42, 47, 42, 3, 3, 2, 0, romPointers[5]),
                        new SpecialRoomData(48, 42, 47, 42, 3, 3, 0, 3, romPointers[5]),
                    };
                }
            }
            return result;
        }
    }
}
