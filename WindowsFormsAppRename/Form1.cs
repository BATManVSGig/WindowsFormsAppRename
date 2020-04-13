using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WindowsFormsAppRename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            string name;

        }
        private string ReaderLinesFromFile(string filename, int startLine, int linecount)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();

            StreamReader reader = new StreamReader(filename.ToString());
            while (!reader.EndOfStream)
            {
                if (i >= startLine)
                {
                    if (linecount < 1)
                        sb.Append(reader.ReadToEnd());
                    else
                        sb.Append(reader.ReadLine());
                    sb.Append("\r");
                }
                else
                    reader.ReadLine();
                i++;
                if (i >= linecount + startLine) break;
            }
            reader.Close();
            reader.Dispose();
            return sb.ToString();
        }

        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]

        public static extern int StrCmpLogicalW(string psz1, string psz2);

        public class FileComparer : IComparer<string>
        {
            [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string psz1, string psz2);
            public int Compare(string psz1, string psz2)
            {
                return StrCmpLogicalW(psz1, psz2);
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {  
            openFileDialog1.ShowDialog();

            string a;
            string strc = "";
            string file = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);

            string filepath = file + "\\" +openFileDialog1.SafeFileName;

            DirectoryInfo theFolder = new DirectoryInfo(file);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            dirInfo = dirInfo.OrderBy(x => x.Name, new FileComparer()).ToArray();
          
            foreach (DirectoryInfo NextFolder in dirInfo)
            {

                FileInfo[] fileInfo = NextFolder.GetFiles();

                fileInfo = fileInfo.OrderBy(y => y.Name, new FileComparer()).ToArray();

                int i = 0;
                foreach (FileInfo NextFile in fileInfo)  //遍历文件
                {
                   
                    strc = fileInfo[i].FullName;
                    i++;
                    a = ReaderLinesFromFile(strc, 0, 2);
                    int index = strc.LastIndexOf('\\');
                    string s2 = string.Empty;
                    s2 = strc.Substring(index + 1);
                    string s3 = string.Empty;
                    s3 = s2.Substring(0, s2.LastIndexOf("_"));
                    
                    StringBuilder str = new StringBuilder();
                    string str1 = string.Empty;
                    str1 = str.Append(a + s3).ToString();


                    filepath = file + "\\" + openFileDialog1.SafeFileName;

                    if (File.Exists(filepath))
                    {
                        if (filepath == @"D://220kv_GT_航线//220kv_GT_航线//220kV丰铜一线//丰铜二线.txt") ;
                        FileStream fs = new FileStream(filepath, FileMode.Append);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(str1);
                        sw.Close();
                        fs.Close();

                    }
                    else
                    {
                        FileStream fs = new FileStream(filepath, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(str1);
                        sw.Close();
                        fs.Close();
                    }
                }
            }
        }
    }
}
 
