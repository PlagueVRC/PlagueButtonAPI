using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlagueButtonAPI.External_Libraries
{
    public class ConfigLib<T> where T : class
    {
        public ConfigLib(string configPath)
        {
            ConfigPath = configPath;

            var PathToWatch = Path.GetDirectoryName(ConfigPath);

            if (PathToWatch != null)
            {
                var watcher = new FileSystemWatcher(PathToWatch, Path.GetFileName(ConfigPath))
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                watcher.Changed += UpdateConfig;
            }

            if (!File.Exists(ConfigPath))
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Activator.CreateInstance(typeof(T)), Newtonsoft.Json.Formatting.Indented));
            }

            InternalConfig = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));

            var timer = new System.Timers.Timer(1000);

            var ConfigCache = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(InternalConfig));

            timer.Elapsed += (sender, args) =>
            {
                if (!InternalConfig.DoesInstanceMatch(ConfigCache))
                {
                    ConfigCache = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(InternalConfig));

                    SaveConfig();

                    //MessageBox.Show("Saved!");
                }
            };

            timer.Enabled = true;
            timer.Start();
        }

        private string ConfigPath
        {
            get;
        }

        public T InternalConfig
        {
            get; private set;
        }

        public event Action OnConfigUpdated;

        private void UpdateConfig(object obj, FileSystemEventArgs args)
        {
            try
            {
                var UpdatedConfig = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));

                if (UpdatedConfig != null)
                {
                    foreach (var newProp in UpdatedConfig.GetType()?.GetProperties())
                    {
                        var OldProp = InternalConfig.GetType().GetProperty(newProp?.Name);

                        if (OldProp != null && newProp.GetValue(UpdatedConfig) != OldProp.GetValue(InternalConfig)) // Property Existed Before & Has Changed
                        {
                            InternalConfig = UpdatedConfig;

                            OnConfigUpdated?.Invoke();
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public void SaveConfig()
        {
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(InternalConfig, Newtonsoft.Json.Formatting.Indented));
        }
    }

    public static class ConfigExt
    {
        public static bool DoesInstanceMatch<T>(this T instance, T CompareTo) where T : class
        {
            foreach (var prop in instance.GetType().GetProperties())
            {
                if (JsonConvert.SerializeObject(prop.GetValue(instance)) != JsonConvert.SerializeObject(prop.GetValue(CompareTo)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
