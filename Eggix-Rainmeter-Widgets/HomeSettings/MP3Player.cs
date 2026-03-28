// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由栖小光编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using NAudio.Wave;

namespace HomeSettings;

/// <summary>
/// MP3音频播放器类
/// </summary>
public static class MP3Player
{
    private static WaveOutEvent? _waveOut;
    private static AudioFileReader? _audioFileReader;

    public static void Play(string filePath)
    {
        Stop();

        try
        {
            _audioFileReader = new AudioFileReader(filePath);
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_audioFileReader);
            _waveOut.Play();

            _waveOut.PlaybackStopped += WaveOutEvent_PlaybackStopped;
        }
        catch
        {
            DisposeResources();
        }
        
    }

    public static void Pause() => _waveOut?.Pause();
    
    public static void Resume() => _waveOut?.Play();

    public static void Stop()
    {
        _waveOut?.Stop();
        DisposeResources();
    }
    
    public static void SetVolume(float volume)
        => _waveOut?.Volume = Math.Clamp(volume, 0.0f, 1.0f);
    
    private static void WaveOutEvent_PlaybackStopped(object? sender, StoppedEventArgs e)
        => DisposeResources();

    private static void DisposeResources()
    {
        _audioFileReader?.Dispose();
        _audioFileReader = null;
        _waveOut?.Dispose();
        _waveOut = null;
    }
}