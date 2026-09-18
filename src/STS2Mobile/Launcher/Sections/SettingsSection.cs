using System;
using Godot;
using STS2Mobile.Launcher.Components;

namespace STS2Mobile.Launcher.Sections;

// Settings page: Local Backup and Auto Sync preferences plus the Steam
// beta-branch selector (public vs public-beta). Shown once the launcher
// reaches a launch-ready state; hidden otherwise.
public class SettingsSection : VBoxContainer
{
    public event Action<bool> LocalBackupToggled;
    public event Action<bool> CloudSyncToggled;
    public event Action<bool> BetaBranchToggled;
    public event Action BackPressed;

    private readonly StyledButton _localBackupToggle;
    private readonly StyledButton _cloudSyncToggle;
    private readonly StyledButton _betaBranchToggle;
    private readonly StyleBoxFlat _offStyle;
    private readonly StyleBoxFlat _onStyle;

    public SettingsSection(float scale)
    {
        var title = new StyledLabel("Settings", scale, fontSize: 16);
        AddChild(title);
        AddChild(new HSeparator());

        var r = (int)(4 * scale);
        var bw = System.Math.Max(1, (int)(2 * scale));
        _offStyle = StyledButton.MakeOutline(new Color(0.7f, 0.25f, 0.25f), r, bw);
        _onStyle = StyledButton.MakeOutline(new Color(0.25f, 0.65f, 0.3f), r, bw);

        _localBackupToggle = MakeToggle("Local Backup", scale);
        _localBackupToggle.Toggled += pressed =>
            OnToggle(_localBackupToggle, "Local Backup", pressed, LocalBackupToggled);
        AddChild(_localBackupToggle);

        _cloudSyncToggle = MakeToggle("Auto Sync", scale);
        _cloudSyncToggle.Toggled += pressed =>
            OnToggle(_cloudSyncToggle, "Auto Sync", pressed, CloudSyncToggled);
        AddChild(_cloudSyncToggle);

        _betaBranchToggle = MakeToggle("Beta Branch", scale);
        _betaBranchToggle.Toggled += pressed =>
            OnToggle(_betaBranchToggle, "Beta Branch", pressed, BetaBranchToggled);
        AddChild(_betaBranchToggle);

        var backButton = new StyledButton("BACK", scale, fontSize: 16, height: 48);
        backButton.Pressed += () => BackPressed?.Invoke();
        AddChild(backButton);
    }

    private static StyledButton MakeToggle(string label, float scale)
    {
        var button = new StyledButton($"{label}: OFF", scale, fontSize: 14, height: 44);
        button.ToggleMode = true;
        return button;
    }

    private void OnToggle(
        StyledButton button,
        string label,
        bool pressed,
        Action<bool> handler
    )
    {
        button.Text = pressed ? $"{label}: ON" : $"{label}: OFF";
        ApplyToggleStyle(button, pressed);
        handler?.Invoke(pressed);
    }

    public void SetLocalBackupChecked(bool value) =>
        SetChecked(_localBackupToggle, "Local Backup", value);

    public void SetCloudSyncChecked(bool value) =>
        SetChecked(_cloudSyncToggle, "Auto Sync", value);

    public void SetBetaBranchChecked(bool value) =>
        SetChecked(_betaBranchToggle, "Beta Branch", value);

    private void SetChecked(StyledButton button, string label, bool value)
    {
        button.SetPressedNoSignal(value);
        button.Text = value ? $"{label}: ON" : $"{label}: OFF";
        ApplyToggleStyle(button, value);
    }

    private void ApplyToggleStyle(Button button, bool on)
    {
        var style = on ? _onStyle : _offStyle;
        button.AddThemeStyleboxOverride("normal", style);
        button.AddThemeStyleboxOverride("hover", style);
        button.AddThemeStyleboxOverride("pressed", style);
        button.AddThemeStyleboxOverride("disabled", style);
    }
}
