using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace HelpProgram
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 图片列表
        private List<string> imageList = new List<string>()
        {
            "https://nie.res.netease.com/r/pic/20241125/33578183-55ac-4a1c-9607-575b2ebb502d.jpg",
            "https://nie.res.netease.com/r/pic/20241125/13a9758f-b8a3-4cbc-83fc-c95ff9f431f8.jpg",
            "https://nie.res.netease.com/r/pic/20241125/73ce1e0c-b96a-4a28-8351-5faa77b9f09d.jpg"
            // 可以添加更多图片URL
        };

        private int currentIndex = 0;
        private bool isMusicPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            // 绑定按钮事件处理程序
            PreviousButton.Click += PreviousImageButton_Click;
            NextButton.Click += NextImageButton_Click;
            MusicButton.Click += MusicButton_Click;
            HelpButton.Click += HelpButton_Click;
            CloseButton.Click += CloseButton_Click;
        }

        // 上一张按钮点击事件
        private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageList.Count > 0)
            {
                currentIndex = (currentIndex - 1 + imageList.Count) % imageList.Count;
                UpdateCurrentImage();
            }
        }

        // 下一张按钮点击事件
        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageList.Count > 0)
            {
                currentIndex = (currentIndex + 1) % imageList.Count;
                UpdateCurrentImage();
            }
        }

        // 音乐按钮点击事件
        private void MusicButton_Click(object sender, RoutedEventArgs e)
        {
            isMusicPlaying = !isMusicPlaying;
            MusicButton.Content = isMusicPlaying ? "关闭音乐" : "开启音乐";
            // 此处可以添加实际的音乐播放/暂停逻辑
        }

        // 帮助按钮点击事件
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开帮助页面
            try
            {
                Process.Start("https://example.com/help");
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开帮助页面: " + ex.Message);
            }
        }

        // 关闭按钮点击事件
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // 更新当前显示的图片
        private void UpdateCurrentImage()
        {
            if (imageList.Count > 0 && currentIndex >= 0 && currentIndex < imageList.Count)
            {
                CurrentImage.Source = new BitmapImage(new Uri(imageList[currentIndex], UriKind.Absolute));
            }
        }
    }
}
