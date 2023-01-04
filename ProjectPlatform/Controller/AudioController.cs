using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace HenrySolidAdventure.Controller
{
    public enum Songs{GameOver,MainMenu, GamePlay, }
    public enum SoundEffects{Coin, Die, HeavySwing, Hurt1, Hurt2,Hurt3,ShieldBlock,SlimeAttack,SlimeHit,Sword}
    internal class AudioController
    {
        //implement singleton

        private static AudioController _uAudio;

        internal static AudioController Instance => _uAudio ??= new AudioController();
        private Dictionary<Songs, Song> _songs;
        private Dictionary<SoundEffects, SoundEffect> _soundEffects;
        public static float Volume { get => MediaPlayer.Volume;
            set => MediaPlayer.Volume = value;
        }
        
        public void Initialise(ContentManager content)
        {
            _songs = new Dictionary<Songs, Song>
            {
                {Songs.GameOver, content.Load<Song>(@"Audio\Music\HANGOVER") },
                {Songs.MainMenu, content.Load<Song>(@"Audio\Music\The_Adventure")},
                {Songs.GamePlay, content.Load<Song>(@"Audio\Music\The_Aquaticans")}
            };
            _soundEffects = new Dictionary<SoundEffects, SoundEffect>()
            {
                { SoundEffects.Coin, content.Load<SoundEffect>(@"Audio\Effects\Coin") },
                { SoundEffects.Die, content.Load<SoundEffect>(@"Audio\Effects\Die") },
                { SoundEffects.HeavySwing, content.Load<SoundEffect>(@"Audio\Effects\HeavySwing") },
                { SoundEffects.Hurt1, content.Load<SoundEffect>(@"Audio\Effects\Hurt1") },
                { SoundEffects.Hurt2, content.Load<SoundEffect>(@"Audio\Effects\Hurt2") },
                { SoundEffects.Hurt3, content.Load<SoundEffect>(@"Audio\Effects\Hurt3") },
                { SoundEffects.ShieldBlock, content.Load<SoundEffect>(@"Audio\Effects\ShieldBlock") },
                { SoundEffects.SlimeAttack, content.Load<SoundEffect>(@"Audio\Effects\SlimeAttack") },
                { SoundEffects.SlimeHit, content.Load<SoundEffect>(@"Audio\Effects\SlimeHit") },
                { SoundEffects.Sword, content.Load<SoundEffect>(@"Audio\Effects\Sword") }
            };
            MediaPlayer.IsRepeating = true;
            
        }

        public void PlayEffect(SoundEffects key)
        {
           var effect = _soundEffects.GetValueOrDefault(key)?.CreateInstance();
           if (effect is not null)
           {
               effect.Play();
               effect.Volume = Volume;
           }
           

        }

        public void PlaySong(Songs key)
        {
            MediaPlayer.Play(_songs.GetValueOrDefault(key));
        }

    }
}
