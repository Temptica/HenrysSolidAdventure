﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace OtterlyAdventure.Controller
{
    internal class AudioController
    {
        //implement singleton

        private static AudioController _uAudio;

        internal static AudioController Instance => _uAudio ??= new AudioController();
        private Dictionary<string, Song> _songs;
        private Dictionary<string, SoundEffect> _soundEffects;
        public static float Volume { get { return MediaPlayer.Volume; } set { MediaPlayer.Volume = value; } }

        public static void Initialise(ContentManager content)
        {
            Instance._songs = new Dictionary<string, Song>
            {
                {"GameOver", content.Load<Song>(@"Audio\Music\HANGOVER") },
                {"MainMenu", content.Load<Song>(@"Audio\Music\The_Adventure")},
                {"GamePlay", content.Load<Song>(@"Audio\Music\The_Aquaticans")}
            };
            MediaPlayer.IsRepeating = true;
            

        }

        public void PlayEffect(string key)
        {
            _soundEffects.GetValueOrDefault(key)?.Play();
        }

        public void PlaySong(string key)
        {
            MediaPlayer.Play(_songs.GetValueOrDefault(key));
        }

    }
}
