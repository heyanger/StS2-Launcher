using System;
using Godot;
using STS2Mobile.Launcher.Components;

namespace STS2Mobile.Launcher.Sections;

public class ActionSection : VBoxContainer
{
    public event Action LaunchPressed;
    public event Action RetryPressed;
    public event Action SettingsPressed;
    public event Action CloudPushPressed;
    public event Action CloudPullPressed;
    public event Action CheckForUpdatesPressed;

    private readonly Button _launchButton;
    private readonly Button _retryButton;
    private readonly Button _settingsButton;
    private readonly Button _pushButton;
    private readonly Button _pullButton;
    private readonly Button _updateButton;

    public ActionSection(float scale)
    {
        _retryButton = new StyledButton("RETRY", scale);
        _retryButton.Visible = false;
        _retryButton.Pressed += () => RetryPressed?.Invoke();
        AddChild(_retryButton);

        var pushPullRow = new HBoxContainer();
        pushPullRow.Visible = false;
        pushPullRow.AddThemeConstantOverride("separation", (int)(6 * scale));

        _pushButton = new StyledButton("Push to Cloud", scale, fontSize: 14, height: 44);
        _pushButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _pushButton.Pressed += () => CloudPushPressed?.Invoke();
        pushPullRow.AddChild(_pushButton);

        _pullButton = new StyledButton("Pull from Cloud", scale, fontSize: 14, height: 44);
        _pullButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _pullButton.Pressed += () => CloudPullPressed?.Invoke();
        pushPullRow.AddChild(_pullButton);

        AddChild(pushPullRow);

        _updateButton = new StyledButton("CHECK FOR UPDATES", scale, fontSize: 16, height: 48);
        _updateButton.Visible = false;
        _updateButton.Pressed += () => CheckForUpdatesPressed?.Invoke();
        AddChild(_updateButton);

        _settingsButton = new StyledButton("SETTINGS", scale, fontSize: 16, height: 48);
        _settingsButton.Visible = false;
        _settingsButton.Pressed += () => SettingsPressed?.Invoke();
        AddChild(_settingsButton);

        _launchButton = new StyledButton("LAUNCH", scale, fontSize: 16, height: 48);
        _launchButton.Visible = false;
        _launchButton.Pressed += () => LaunchPressed?.Invoke();
        AddChild(_launchButton);
    }

    private HBoxContainer PushPullRow => (HBoxContainer)_pushButton.GetParent();

    public void ShowLaunch(string text, bool showCloudSync, bool showUpdate)
    {
        _launchButton.Text = text;
        _launchButton.Visible = true;
        PushPullRow.Visible = showCloudSync;
        _updateButton.Visible = showUpdate;
        _updateButton.Disabled = false;
        _updateButton.Text = "CHECK FOR UPDATES";
        _settingsButton.Visible = true;
        _retryButton.Visible = false;
    }

    public void ShowRetry()
    {
        _retryButton.Visible = true;
        _launchButton.Visible = false;
        PushPullRow.Visible = false;
        _updateButton.Visible = false;
        _settingsButton.Visible = false;
    }

    public void HideAll()
    {
        _launchButton.Visible = false;
        _retryButton.Visible = false;
        _settingsButton.Visible = false;
        PushPullRow.Visible = false;
        _updateButton.Visible = false;
    }

    public void SetPushPullDisabled(bool disabled)
    {
        _pushButton.Disabled = disabled;
        _pullButton.Disabled = disabled;
    }

    public void SetUpdateButtonText(string text) => _updateButton.Text = text;

    public void SetUpdateButtonDisabled(bool disabled) => _updateButton.Disabled = disabled;
}
