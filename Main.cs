using System;
using UnityEngine;
using UnityModManagerNet;

namespace LowFPS
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static bool IsEnabled = false;
        public static Settings Settings;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnLateUpdate = OnLateUpdate;
            modEntry.OnSaveGUI = OnSaveGUI;
            Logger.Log("Loading Settings...");
            Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            Logger.Log("Load Completed!");
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "얼불춤 창을 선택하지 않을 시 FPS" : "FPS When You Don't Focus ADOFAI Window");
            string value = GUILayout.TextField(Settings.unfocusedFPS == -1 ? "" : Settings.unfocusedFPS.ToString());
            if (value.IsNullOrEmpty())
                Settings.unfocusedFPS = -1;
            else
                try
                {
                    Settings.unfocusedFPS = int.Parse(value);
                }
                catch (Exception) {
                }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void OnLateUpdate(UnityModManager.ModEntry modEntry, float f)
        {
            int frame = Application.isFocused ? PlayerPrefsJson.Select(SaveFileType.General).GetInt("targetFramerate") : Settings.unfocusedFPS == -1 ? 0 : Settings.unfocusedFPS;
            if (Application.targetFrameRate != frame)
                Application.targetFrameRate = frame;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Logger.Log("Saving Settings...");
            Settings.Save(modEntry);
            Logger.Log("Save Completed!");
        }
    }
}