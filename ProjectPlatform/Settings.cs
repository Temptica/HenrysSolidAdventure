using System.IO;
using System.Linq;
using HenrySolidAdventure.Controller;

namespace HenrySolidAdventure
{
    internal class Settings
    {
        private static Settings _uSettings;

        internal static Settings Instance => _uSettings ??= new Settings();

        private Settings()
        {
            Setting = new Setting();
        }

        public Setting Setting { get; }

        private string _path;
        public void Initialise(string path)
        {
            _path = path;
            if (!File.Exists(_path)) return;
            using StreamReader reader = new StreamReader(path);
            var data = reader.ReadToEnd();
            data.Split('\n')
                .ToList()
                .ForEach(d =>
                {
                    var setting = d.Split('=');
                    switch (setting[0])
                    {
                        case "Volume":
                            Setting.Volume = float.Parse(setting[1]);
                            break;
                        case "FullScreen":
                            Setting.FullScreen = bool.Parse(setting[1]);
                            break;
                        case "Muted":
                            Setting.Muted = bool.Parse(setting[1]);
                            break;
                    }
                });

            AudioController.Volume = Setting.Muted? 0f : Setting.Volume;
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
        internal float Volume{ get; set; }
        internal bool FullScreen { get; set; }
        public bool Muted { get; set; }

        public override string ToString()
        {
            return $"Volume={Volume}\nFullScreen={FullScreen}\nMuted={Muted}";
        }

    }
}
