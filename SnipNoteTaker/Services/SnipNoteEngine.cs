using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SnipNoteTaker.Services
{
    public static class SnipNoteEngine
    {
        private static readonly SnipNoteSnapRepository snapRepository = new();

        public static List<string> GetProjectNames()
        {
            var projects = snapRepository.GetAll();
            return projects.Select(x => x.ProjectName).ToList();
        }
        public static string GetProjectImage(string projectName)
        {
            var snipNote = snapRepository.Get(projectName);
            return Path.GetFullPath(snipNote.PathToPng);
        }

        public static void AddText(string projectName, string textToAdd)
        {
            var bitmap = TextToBitmap.CreateBitmapFromText(textToAdd);
            var snipNote = snapRepository.Get(projectName);
            snipNote.AddToBitmap(bitmap);
            snipNote.AddToTextFile(textToAdd);
        }

        public static void AddImage(string projectName, Bitmap bitmap)
        {
            var snipNote = snapRepository.Get(projectName);
            snipNote.AddToBitmap(bitmap);
        }

        public static void OpenFileExplorer(string projectName)
        {
            var snipNote = snapRepository.Get(projectName);
            var filePath = Path.GetFullPath(snipNote.PathToPng);
            var directory = Path.GetDirectoryName(filePath);
            Process.Start("explorer.exe", directory);
        }

        public static void OpenMsPaint(string projectName)
        {
            var snipNote = snapRepository.Get(projectName);
            var filePath = Path.GetFullPath(snipNote.PathToPng);
            Process.Start("mspaint.exe", filePath);
        }
    }
}
