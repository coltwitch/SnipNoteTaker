using Newtonsoft.Json;
using SnipNoteTaker.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnipNoteTaker.Models
{
    public class SnipNote
    {
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }
        [JsonProperty("pathToPng")]
        public string PathToPng { get; set; }
        [JsonProperty("pathToTxt")]
        public string PathToTxt { get; set; }
        [JsonIgnore]
        public Image Image { get; set; }
        [JsonIgnore]
        private string notesDirectory = "notes";

        public SnipNote(string projectName)
        {
            ProjectName = projectName;
            PathToPng = GetImagePathFromProjectName();
            PathToTxt = GetTextPathFromProjectName();
            Image = LoadImageFromPath();
            SaveImageToPath();
        }

        public void AddToBitmap(Image newImage)
        {
            Image = BitmapCombiner.CombineImage(Image, newImage);
            SaveImageToPath();
        }

        public void AddToTextFile(string text)
        {
            SaveTextToPath(text);
        }

        private void SaveTextToPath(string text)
        {
            if (string.IsNullOrEmpty(PathToTxt))
            {
                return;
            }
            if (!File.Exists(PathToTxt))
            {
                if (!Directory.Exists(notesDirectory))
                {
                    Directory.CreateDirectory(notesDirectory);
                }
                if (!Directory.Exists($"{notesDirectory}/{ProjectName}"))
                {
                    Directory.CreateDirectory($"{notesDirectory}/{ProjectName}");
                }
                var txt = File.Create(PathToTxt);
                txt.Close();
            }
            using (StreamWriter sw = File.AppendText(PathToTxt))
            {
                sw.WriteLine(text);
            }
        }

        private void SaveImageToPath()
        {
            if (!File.Exists(PathToPng))
            {
                if (!Directory.Exists(notesDirectory))
                {
                    Directory.CreateDirectory(notesDirectory);
                }
                if (!Directory.Exists($"{notesDirectory}/{ProjectName}"))
                {
                    Directory.CreateDirectory($"{notesDirectory}/{ProjectName}");
                }
                var png = File.Create(PathToPng);
                png.Close();
            }

            using (FileStream stream = new FileStream(PathToPng, FileMode.Open, FileAccess.ReadWrite))
            {
                Image.Save(stream, ImageFormat.Png);
            }
        }

        private string GetImagePathFromProjectName()
        {
            return $"{notesDirectory}/{ProjectName}/{ProjectName}.png";
        }

        private string GetTextPathFromProjectName()
        {
            return $"{notesDirectory}/{ProjectName}/{ProjectName}.txt";
        }

        private Image LoadImageFromPath()
        {
            if (File.Exists(PathToPng))
            {
                Image img;
                using (var bmpTemp = new Bitmap(PathToPng))
                {
                    img = new Bitmap(bmpTemp);
                }
                return img;
            }
            return new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
    }
}
