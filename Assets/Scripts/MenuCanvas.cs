using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private void OnEnable()
    {
        BindSettingPanel();
    }

    void BindSettingPanel()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var r in GameSettingHandler.ResolutionSettings)
            options.Add(r.name);

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("Resolution", 0));
        fullscreenToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("FullScreen", 1) == 1);

        GameSettingHandler.SetResolution(PlayerPrefs.GetInt("Resolution", 0), PlayerPrefs.GetInt("FullScreen", 1) == 1);

        resolutionDropdown.onValueChanged.AddListener((i) =>
        {
            GameSettingHandler.SetResolution(i, GameSettingHandler.isFullScreen);
        });

        fullscreenToggle.onValueChanged.AddListener((b) =>
        {
            GameSettingHandler.SetResolution(GameSettingHandler.currentResolution, b);
        });
    }
}