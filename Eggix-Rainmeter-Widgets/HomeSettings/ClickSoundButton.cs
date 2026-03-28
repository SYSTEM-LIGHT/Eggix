using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace HomeSettings;

internal class ClickSoundButton : Button
{
    public ClickSoundButton()
    {
        FlatStyle = FlatStyle.Flat;
        
        BackColor = AppStatus.IsDarkModeAndWin11
            ? SystemColors.Control
            : Color.FromArgb(242, 234, 96);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        MP3Player.Play(Path.Combine(AppStatus.AppPath, @"Resources\click_sound.mp3"));
    }
}