using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Media;

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
        private MediaPlayer mediaPlayer; // 添加MediaPlayer实例
        private Uri musicUri; // 音频文件路径

        public MainWindow()
        {
            InitializeComponent();
            InitializeEvents();
            InitializeMediaPlayer();
        }

        private void InitializeMediaPlayer()
        {
            // 初始化MediaPlayer
            mediaPlayer = new MediaPlayer();
            
            // 使用相对路径引用音频文件，文件会被复制到输出目录
            try
            {
                // 使用相对路径，文件会被复制到输出目录的Resources文件夹
                musicUri = new Uri("Resources/music.mp3", UriKind.Relative);
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化音频资源失败: " + ex.Message);
            }
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
            try
            {
                isMusicPlaying = !isMusicPlaying;
                
                if (isMusicPlaying && musicUri != null)
                {
                    mediaPlayer.Open(musicUri);
                    mediaPlayer.Play();
                    mediaPlayer.MediaEnded += MediaPlayer_MediaEnded; // 添加循环播放
                }
                else
                {
                    mediaPlayer.Pause();
                }
                
                MusicButton.Content = isMusicPlaying ? "关闭音乐" : "开启音乐";
            }
            catch (Exception ex)
            {
                MessageBox.Show("播放音频时出错: " + ex.Message);
                isMusicPlaying = false;
                MusicButton.Content = "开启音乐";
            }
        }

        // 音频播放结束事件，用于循环播放
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (isMusicPlaying && musicUri != null)
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
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
            // 关闭前停止播放
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
                mediaPlayer.Close();
            }
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
