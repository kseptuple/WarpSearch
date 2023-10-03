using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace WarpSearch.Lang
{
    public static class L10N
    {
        static L10N()
        {
            InitLang();
        }

        private static Dictionary<string, string> texts = new Dictionary<string, string>();

        public static Dictionary<string, string> Texts = new Dictionary<string, string>();

        public static List<LanguageModel> LanguageModels = new List<LanguageModel>();
        public static void SetLang(Form form, string areaCode)
        {
            var currentLanguageModel = LanguageModels.FirstOrDefault(l => l.AreaCode == areaCode);
            if (currentLanguageModel != null)
            {
                var currentFormLanguageModel = currentLanguageModel.FormData.FirstOrDefault(l => l.Name == form.Name);
                if (currentFormLanguageModel != null)
                {
                    if (currentFormLanguageModel.Text != null)
                    {
                        form.Text = currentFormLanguageModel.Text;
                    }
                    getControls(form.Controls, currentFormLanguageModel.ChildControl);
                }
                var currentTextLanguageModel = currentLanguageModel.FormData.FirstOrDefault(l => l.Name.ToLower() == "text");
                if (currentTextLanguageModel != null)
                {
                    texts.Clear();
                    foreach (var textItem in currentTextLanguageModel.ChildControl)
                    {
                        if (!texts.ContainsKey(textItem.Name))
                        {
                            texts.Add(textItem.Name, textItem.Text);
                        }
                        else
                        {
                            texts[textItem.Name] = textItem.Text;
                        }
                    }
                }
            }

            void getControls(Control.ControlCollection controls, List<ControlLanguageModel> currentControlLanguageModelList)
            {
                foreach (Control control in controls)
                {
                    var currentControlLanguageModel = currentControlLanguageModelList.FirstOrDefault(l => l.Name == control.Name);
                    if (currentControlLanguageModel != null)
                    {
                        control.Text = currentControlLanguageModel.Text;
                    }
                    if (control is MenuStrip)
                    {
                        setMenuText(((MenuStrip)control).Items, currentControlLanguageModelList);
                    }
                    if (control.HasChildren)
                    {
                        getControls(control.Controls, currentControlLanguageModelList);
                    }
                }
            }

            void setMenuText(ToolStripItemCollection menuItemList, List<ControlLanguageModel> languageModelList)
            {
                foreach (object item in menuItemList)
                {
                    var menuItem = item as ToolStripMenuItem;
                    if (menuItem == null) 
                        continue; 
                    var childLanguageModel = languageModelList.FirstOrDefault(l => l.Name == menuItem.Name);
                    if (childLanguageModel != null)
                    {
                        menuItem.Text = childLanguageModel.Text;
                    }
                    if (menuItem.DropDownItems.Count > 0)
                    {
                        setMenuText(menuItem.DropDownItems, languageModelList);
                    }
                }
            }
        }

        public static void InitLang()
        {
            if (Directory.Exists(".\\Lang\\"))
            {
                var langFiles = Directory.GetFiles(".\\Lang\\", "*.xml");
                foreach (var langFile in langFiles)
                {
                    XmlDocument doc = new XmlDocument();
                    LanguageModel languageModel = new LanguageModel();
                    doc.Load(langFile);
                    XmlNode root = doc.SelectSingleNode("Language");
                    languageModel.Name = root.Attributes["Name"]?.Value?.Trim();
                    languageModel.AreaCode = Path.GetFileNameWithoutExtension(langFile);
                    languageModel.FormData = readNodes(root.ChildNodes);

                    if (languageModel.FormData != null)
                    {
                        LanguageModels.Add(languageModel);
                    }
                }
            }

            List<ControlLanguageModel> readNodes(XmlNodeList langDataList)
            {
                try
                {
                    List<ControlLanguageModel> controlLanguageModelList = new List<ControlLanguageModel>();
                    foreach (XmlNode langData in langDataList)
                    {
                        ControlLanguageModel controlLanguageModel = new ControlLanguageModel();
                        if (langData.NodeType == XmlNodeType.Element)
                        {
                            var langName = langData.Name.Trim();
                            var langValue = langData.Attributes["Text"]?.Value?.Trim();

                            bool textNodeOnly = false;
                            if (langData.ChildNodes.Count == 1)
                            {
                                if (langData.FirstChild.NodeType == XmlNodeType.Text)
                                {
                                    if (string.IsNullOrEmpty(langValue))
                                    {
                                        langValue = langData.FirstChild.Value?.Trim();
                                    }
                                    textNodeOnly = true;
                                }
                            }
                            controlLanguageModel.Name = langName;
                            controlLanguageModel.Text = langValue;
                            if (!textNodeOnly)
                            {
                                controlLanguageModel.ChildControl = readNodes(langData.ChildNodes);
                            }
                        }
                        controlLanguageModelList.Add(controlLanguageModel);
                    }
                    return controlLanguageModelList;
                }
                catch
                {
                    return null;
                }

            }
        }

        public static string GetLang(string langToGet)
        {
            CultureInfo cultureInfo = null;
            if (string.IsNullOrEmpty(langToGet))
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }
            else
            {
                cultureInfo = CultureInfo.GetCultureInfo(langToGet);
            }
            while (!string.IsNullOrEmpty(cultureInfo.Name))
            {
                if (LanguageModels.Exists(l => l.AreaCode == cultureInfo.Name))
                {
                    return cultureInfo.Name;
                }
                if (LanguageModels.Exists(l => l.AreaCode.StartsWith(cultureInfo.Name)))
                {
                    return cultureInfo.Name;
                }
                cultureInfo = cultureInfo.Parent;
            }
            //fallback
            if (LanguageModels.Exists(l => l.AreaCode == "en-US"))
            {
                return "en-US";
            }
            if (LanguageModels.Exists(l => l.AreaCode == "zh-CN"))
            {
                return "zh-CN";
            }
            return string.Empty;
        }

        public static string GetText(string textName)
        {
            if (texts != null && texts.ContainsKey(textName))
            {
                return texts[textName];
            }
            return string.Empty;
        }
    }

    public class LanguageModel
    {
        public string AreaCode { get; set; }
        public string Name { get; set; }
        public List<ControlLanguageModel> FormData { get; set; }
    }

    public class ControlLanguageModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public List<ControlLanguageModel> ChildControl { get; set; }
    }
}
