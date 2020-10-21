using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SanwaSecsDll
{
    public class SanwaSML
    {
        public string Name;
        public string Text;
        public bool ReplyExpected;
        public int S;
        public int F;
        public string MessageName;
    }
    
    public class SanwaSMLManager
    {
        public Dictionary<string, SanwaSML> _messageList = new Dictionary<string, SanwaSML>();

        public void ReadFile(string filePath)
        {
            string stringFile = "";
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Contains("//"))
                    {
                        if(line.Length > 0)
                        {
                            stringFile += line;
                            stringFile += "\r\n";
                        }

                    }
                }

                string[] separatingStrings = { ".\r\n" };
                string[] raw = stringFile.Split(separatingStrings, StringSplitOptions.None);

                for (int i = 0; i< raw.Length; i++)
                {
                    int lineUp = raw[i].IndexOf("\r\n");

                    if (lineUp == -1) continue;

                    string name = raw[i].Substring(0, lineUp);

                    SanwaSML samwaSML = new SanwaSML
                    {
                        ReplyExpected = name.Contains("\' W") ? true : false
                    };

                    if (raw[i].Length == name.Length + 2) //代表\r\n的長度
                    {
                        //Header Only
                        samwaSML.Text = "";
                    }
                    else
                    {
                        //samwaSML.Text = raw[i].Substring(name.Length + 2);
                        samwaSML.Text = "";
                        string smlText = raw[i].Substring(name.Length + 2);

                        using (StringReader reader = new StringReader(smlText))
                        {
                            string smlLine;
                            while ((smlLine = reader.ReadLine()) != null)
                            {
                                samwaSML.Text += smlLine.Trim();
                                samwaSML.Text += "\r\n";
                            }
                        }
                    }

                    //ex. AreYouThere:'S1F1' W
                    int colonIndex = name.IndexOf(":");
                    samwaSML.MessageName = name.Substring(0, colonIndex);

                    int firstQuotationIndex = name.IndexOf("\'", colonIndex);
                    int lastQuotationIndex = name.LastIndexOf("\'");
                    samwaSML.Name = name.Substring(firstQuotationIndex + 1, lastQuotationIndex - firstQuotationIndex - 1);

                    int sIndex = samwaSML.Name.IndexOf("S");
                    int fIndex = samwaSML.Name.IndexOf("F");
                    samwaSML.S = Convert.ToInt32(samwaSML.Name.Substring(sIndex+1, fIndex - sIndex - 1));
                    samwaSML.F = Convert.ToInt32(samwaSML.Name.Substring(fIndex + 1, samwaSML.Name.Length - fIndex - 1));
                    _messageList.Add(samwaSML.Name, samwaSML);
                }
            }
        }
    }
}
