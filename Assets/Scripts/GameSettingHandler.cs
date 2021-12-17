using UnityEngine;
using System.Collections.Generic;

public class GameSettingHandler : MonoBehaviour
{
    public struct Resolution
    {
        public Resolution(string name, Vector2Int size)
        {
            this.name = name;
            this.size = size;
        }
        public string name;
        public Vector2Int size;
    }

    static List<Resolution> resolutionSettings = new List<Resolution>();

    // static Dictionary<string, Vector2Int> resolutionSettings = new Dictionary<string, Vector2Int>();
    public static List<Resolution> ResolutionSettings
    {
        get
        {
            if (resolutionSettings.Count == 0)
            {
                resolutionSettings.Add(new Resolution("1920x1080", new Vector2Int(1920, 1080)));
                resolutionSettings.Add(new Resolution("1600x900", new Vector2Int(1600, 900)));
                resolutionSettings.Add(new Resolution("1280x720", new Vector2Int(1280, 720)));
                resolutionSettings.Add(new Resolution("960x540", new Vector2Int(960, 540)));
            }

            return resolutionSettings;
        }
    }
    public static bool isFullScreen;
    public static Vector2Int currentResolution;

    public static void SetResolution(int index, bool fullScreen)
    {
        Screen.SetResolution(resolutionSettings[index].size.x, resolutionSettings[index].size.y, fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);
        isFullScreen = fullScreen;
        currentResolution = resolutionSettings[index].size;

        PlayerPrefs.SetInt("Resolution", index);
        PlayerPrefs.SetInt("FullScreen", fullScreen ? 1 : 0);
    }

    public static void SetResolution(Vector2Int size, bool fullScreen)
    {
        Screen.SetResolution(size.x, size.y, fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);
        isFullScreen = fullScreen;
        currentResolution = size;

        PlayerPrefs.SetInt("Resolution", resolutionSettings.FindIndex((x) => x.size == currentResolution));
        PlayerPrefs.SetInt("FullScreen", fullScreen ? 1 : 0);
    }
}