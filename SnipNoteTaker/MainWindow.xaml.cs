using Microsoft.Win32;
using SnipNoteTaker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnipNoteTaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> ProjectsList;
        public MainWindow()
        {
            InitializeComponent();
            LoadProjectList();
        }

        private void LoadProjectList()
        {
            ProjectsList = new ObservableCollection<string>();
            var projects = SnipNoteEngine.GetProjectNames();
            foreach(var project in projects)
            {
                ProjectsList.Add(project);
            }
            ComboBoxProjectName.ItemsSource = ProjectsList;
        }

        private void ButtonLoadStartProject_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxProjectName.Text))
            {
                return;
            }
            if (!ProjectsList.Contains(ComboBoxProjectName.Text))
            {
                ProjectsList.Add(ComboBoxProjectName.Text);
            }
            LoadImage(ComboBoxProjectName.Text);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonAddText_Click(null, null);
                TextTextToAdd.Focus();
            }
        }

        private void LoadImage(string projectName)
        {
            LabelProjectName.Content = projectName;
            var filePath = SnipNoteEngine.GetProjectImage(projectName);
            Bitmap img;
            using (var bmpTemp = new Bitmap(filePath))
            {
                img = new Bitmap(bmpTemp);
            }
            var bitmapImage = BitmapCombiner.ToBitmapImage(img);
            ImageViewer.Source = bitmapImage;
        }

        private void ButtonPaste_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxProjectName.Text))
            {
                return;
            }
            if (Clipboard.ContainsImage())
            {
                var bitmapSource = Clipboard.GetImage();
                var bitmap = BitmapCombiner.BitmapFromSource(bitmapSource);
                SnipNoteEngine.AddImage(ComboBoxProjectName.Text, bitmap);
            }
            LoadImage(ComboBoxProjectName.Text);
        }

        private void ButtonFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxProjectName.Text))
            {
                return;
            }
            SnipNoteEngine.OpenFileExplorer(ComboBoxProjectName.Text);
        }

        private void ButtonAddText_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxProjectName.Text))
            {
                return;
            }
            if (string.IsNullOrEmpty(ComboBoxProjectName.Text))
            {
                return;
            }
            SnipNoteEngine.AddText(ComboBoxProjectName.Text, TextTextToAdd.Text);
            LoadImage(ComboBoxProjectName.Text);
            TextTextToAdd.Text = string.Empty;
        }

        private void ButtonOpenInPaint_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxProjectName.Text))
            {
                return;
            }
            SnipNoteEngine.OpenMsPaint(ComboBoxProjectName.Text);
        }
    }
}
