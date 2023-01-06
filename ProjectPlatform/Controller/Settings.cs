using System.IO;
using System.Linq;

namespace HenrySolidAdventure.Controller
{
    internal class Settings
    {
        private static Settings _uSettings;

        internal static Settings Instance => _uSettings ??= new Settings();

        private Settings()
        {
            Setting = new Setting();
        }

        public Setting Setting { get; private set; }

        private string _path;
        public void Initialise(string path)
        {
            _path = path;
            if (!File.Exists(_path))
            {
                Setting = new Setting
                {
                    MusicVolume = 1,
                    MusicMuted = false,
                    EffectVolume = 1,
                    EffectMuted = false,
                    FullScreen = false
                };
                return;
            }
            using StreamReader reader = new StreamReader(path);
            var data = reader.ReadToEnd();
            data.Split('\n')
                .ToList()
                .ForEach(d =>
                {
                    var setting = d.Split('=');
                    switch (setting[0])
                    {
                        case "MusicVolume":
                            Setting.MusicVolume = float.Parse(setting[1]);
                            break;
                        case "FullScreen":
                            Setting.FullScreen = bool.Parse(setting[1]);
                            break;
                        case "MusicMuted":
                            Setting.MusicMuted = bool.Parse(setting[1]);
                            break;
                        case "EffectMuted":
                            Setting.EffectMuted = bool.Parse(setting[1]);
                            break;
                        case "EffectVolume":
                            Setting.EffectVolume = float.Parse(setting[1]);
                            break;
                    }
                });

            AudioController.Volume = Setting.MusicMuted ? 0f : Setting.MusicVolume;
            AudioController.EffectVolume = Setting.EffectMuted ? 0f : Setting.EffectVolume;
            Game1.SetFullScreen(Setting.FullScreen);

        }

        public void Save()
        {
            //write to file
            using StreamWriter writer = new StreamWriter(_path);
            writer.Write(Setting);
        }
    }

    internal class Setting
    {
        internal float MusicVolume { get; set; }
        public bool MusicMuted { get; set; }
        public float EffectVolume { get; set; }
        public bool EffectMuted { get; set; }
        internal bool FullScreen { get; set; }
        
        public override string ToString()
        {
            return $"MusicVolume={MusicVolume}\nFullScreen={FullScreen}\nMusicMuted={MusicMuted}\nEffectMuted={EffectMuted}\nEffectVolume={EffectVolume}";
        }

    }
}
