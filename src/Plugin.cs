using BepInEx;
using BepInEx.Unity.Mono;
using HarmonyLib;
using Game.Audio.Radio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace CS2RussianRadio
{
    [BepInPlugin(GUID, "CS2 Russian Radio", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "cs2.russian.radio";
        public const string VERSION = "1.0.0";

        internal static Plugin Instance;
        internal static readonly Dictionary<string, AudioClip> Clips = new Dictionary<string, AudioClip>(StringComparer.OrdinalIgnoreCase);
        internal static readonly HashSet<string> SeenClipNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        internal static string MusicDir;
        internal static string ClipLog;
        internal static string PluginDir;

        private void Awake()
        {
            Instance = this;
            PluginDir = Path.Combine(Paths.PluginPath, "CS2RussianRadio");
            MusicDir = Path.Combine(PluginDir, "music");
            ClipLog = Path.Combine(PluginDir, "clip_names.log");
            Logger.LogInfo($"[CS2RussianRadio] plugin dir: {PluginDir}");
            Logger.LogInfo($"[CS2RussianRadio] music dir: {MusicDir}");

            var harmony = Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, GUID);
            Logger.LogInfo($"[CS2RussianRadio] patched methods: {harmony.GetPatchedMethods().Count()}");

            StartCoroutine(LoadAllClips());
        }

        private IEnumerator LoadAllClips()
        {
            if (!Directory.Exists(MusicDir))
            {
                Logger.LogError($"[CS2RussianRadio] music folder not found: {MusicDir}");
                yield break;
            }

            var files = Directory.GetFiles(MusicDir, "*.ogg", SearchOption.AllDirectories);
            Logger.LogInfo($"[CS2RussianRadio] loading {files.Length} ogg files");
            int loaded = 0;
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var url = new Uri(file).AbsoluteUri;
                var req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS);
                yield return req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success)
                {
                    Logger.LogError($"[CS2RussianRadio] load failed {name}: {req.error}");
                    req.Dispose();
                    continue;
                }
                var clip = DownloadHandlerAudioClip.GetContent(req);
                req.Dispose();
                if (clip == null)
                {
                    Logger.LogError($"[CS2RussianRadio] null clip {name}");
                    continue;
                }
                clip.name = name;
                AddClip(name, clip);
                loaded++;
            }
            Logger.LogInfo($"[CS2RussianRadio] loaded {loaded} clips, keys {Clips.Count}");
        }

        internal static void AddClip(string name, AudioClip clip)
        {
            AddKey(name, clip);
            AddKey(Path.GetFileNameWithoutExtension(name), clip);
            AddKey(Normalize(name), clip);
        }

        private static void AddKey(string key, AudioClip clip)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            Clips[key] = clip;
        }

        internal static AudioClip GetClip(string gameClipName)
        {
            if (string.IsNullOrWhiteSpace(gameClipName)) return null;

            if (Clips.TryGetValue(gameClipName, out var clip)) return clip;
            var baseName = Path.GetFileNameWithoutExtension(gameClipName);
            if (Clips.TryGetValue(baseName, out clip)) return clip;
            var normalized = Normalize(gameClipName);
            if (Clips.TryGetValue(normalized, out clip)) return clip;
            return null;
        }

        internal static string Normalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            var chars = s.Where(char.IsLetterOrDigit).ToArray();
            return new string(chars).ToLowerInvariant();
        }

        internal static void LogClipName(string name)
        {
            try
            {
                if (SeenClipNames.Add(name))
                    File.AppendAllText(ClipLog, name + Environment.NewLine);
            }
            catch { }
        }

        internal static void LogReplacement(string original, string replacement)
        {
            Instance?.Logger.LogInfo($"[CS2RussianRadio] replaced '{original}' -> '{replacement}'");
        }
    }

    [HarmonyPatch(typeof(Radio.RadioPlayer), "Play")]
    internal static class RadioPlayerPlayPatch
    {
        private static bool Prefix(Radio.RadioPlayer __instance, AudioClip clip, int timeSamples = 0)
        {
            if (clip == null)
                return true;

            Plugin.LogClipName(clip.name);
            var replacement = Plugin.GetClip(clip.name);
            if (replacement == null)
                return true;

            var source = Traverse.Create(__instance).Field("m_AudioSource").GetValue<AudioSource>();
            if (source == null)
                return true;

            source.clip = replacement;
            source.timeSamples = 0;
            source.Play();

            var isCreated = Traverse.Create(__instance).Field("isCreated").GetValue<bool>();
            var elapsed = isCreated && source.clip != null
                ? source.timeSamples / (double)source.clip.frequency
                : 0.0;
            Traverse.Create(__instance).Field("m_Elapsed").SetValue(elapsed);

            var timer = Traverse.Create(__instance).Field("m_Timer").GetValue<Stopwatch>();
            timer?.Restart();

            Plugin.LogReplacement(clip.name, replacement.name);
            return false;
        }
    }
}
