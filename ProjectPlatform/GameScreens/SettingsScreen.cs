﻿using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HenrySolidAdventure.GameScreens
{
    internal class SettingsScreen: IGameScreen
    {
        List<Text> _texts;
        private List<Clickable> _clickables;
        private Slider _SongSlider;
        private StateButton _muteSongButton;
        private StateButton _FullScreenButton;
        private Slider _EffectSlider;
        private StateButton _muteEffectButton;
        private Button _goBackButton;
        Screen _screen;
        bool _loaded;
        float _timer;
        public IGameScreen LastScreen { get; }

        public SettingsScreen(IGameScreen lastScreen, Screen screen, SpriteFont font, ContentManager content)//add content 
        {
            LastScreen = lastScreen;
            const string settingText = "Settings";
            const string volumeText = "MusicVolume";
            const string muteText = "Mute Music";
            const string effectText = "Effect\nVolume";
            const string muteEffectText = "Mute Effect";
            const string fullscreenText = "Fullscreen";
            var halfWidth = screen.Width / 2f;
            var length = font.MeasureString(settingText).Length();
            var textPos = new Vector2(halfWidth - length / 2, screen.Height / 10f);
            _texts = new List<Text>
            {
                new(textPos, settingText, Color.White, 1f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+165f), volumeText, Color.White, 0.2f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+230f), muteText, Color.White, 0.2f, 0f, font),
                new(new Vector2(textPos.X*2, textPos.Y+230f), muteEffectText, Color.White, 0.2f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+285f), effectText, Color.White, 0.2f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+375), fullscreenText, Color.White, 0.2f, 0f, font)
            };
            _SongSlider = new Slider(new Vector2(textPos.X +150f, textPos.Y + 150f), 0, 1, AudioController.Volume);
            _muteSongButton = new StateButton(new Vector2(textPos.X + 150f, textPos.Y + 230f), false);
            _muteEffectButton = new StateButton(new Vector2(textPos.X*2 + 150f, textPos.Y + 230f), false);

            _EffectSlider = new Slider(new Vector2(textPos.X + 150f, textPos.Y + 285f), 0, 1, AudioController.EffectVolume);
            _FullScreenButton = new StateButton(new Vector2(textPos.X + 150f, textPos.Y + 375), false);

            _goBackButton = new Button("Go back", content.Load<Texture2D>("Buttons/EmptyButton"), new Vector2(textPos.X+40, textPos.Y + 410f), "Back");
            
            _screen = screen;
            _loaded = true;
            
            _clickables = new List<Clickable>
            {
                _SongSlider, _FullScreenButton, _goBackButton, _muteSongButton, _muteEffectButton, _EffectSlider
            };
            
            _muteSongButton.IsClicked = Settings.Instance.Setting.MusicMuted;
            _muteEffectButton.IsClicked = Settings.Instance.Setting.EffectMuted;
        }
        public void Update(GameTime gameTime)
        {
            if (_timer <= 1000f) _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_loaded && InputController.ExitInput)
            {
                return;
            }
            _loaded = false;
            var selected = ClickableChecker.CheckHits(_clickables, _screen);

            if (InputController.ExitInput || (selected is Button && MouseController.IsLeftClicked))
            {
                Settings.Instance.Save();
                Game1.SetState(GameState.Menu);
            }
            _SongSlider.Update(gameTime, _screen);
            if (_muteSongButton.IsClicked)
            {
                AudioController.Volume = 0f;
                Settings.Instance.Setting.MusicMuted = true;
                _SongSlider.Value = 0f;
            }
            else
            {
                Settings.Instance.Setting.MusicMuted = false;
                if (_SongSlider.Value == 0)
                {
                    AudioController.Volume = _SongSlider.Value = Settings.Instance.Setting.MusicVolume;
                }
                else AudioController.Volume = Settings.Instance.Setting.MusicVolume = _SongSlider.Value;
            }

            _EffectSlider.Update(gameTime, _screen);
            if (_muteEffectButton.IsClicked)
            {
                AudioController.EffectVolume = 0f;
                Settings.Instance.Setting.EffectMuted = true;
                _EffectSlider.Value = 0f;
            }
            else
            {
                Settings.Instance.Setting.EffectMuted = false;
                if (_EffectSlider.Value == 0)
                {
                    

                    AudioController.EffectVolume = _EffectSlider.Value = Settings.Instance.Setting.EffectVolume;
                }
                else
                {
                    //if the volume changed
                    if (_EffectSlider.Value != Settings.Instance.Setting.EffectVolume)
                    {
                        AudioController.EffectVolume = Settings.Instance.Setting.EffectVolume = _EffectSlider.Value;
                        if (_timer >= 1000)
                        {
                            AudioController.Instance.PlayEffect(SoundEffects.Hurt1);
                            _timer = 0;
                        }
                    }
                    
                }
            }

            _FullScreenButton.Update(_screen);
            _muteSongButton.Update(_screen);
            _muteEffectButton.Update(_screen);


            Game1.SetFullScreen(Settings.Instance.Setting.FullScreen = _FullScreenButton.IsClicked);
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            BackGround.Instance.Draw(sprites);
            _texts.ForEach(t => t.Draw(spriteBatch));
            _SongSlider.Draw(sprites);
            _FullScreenButton.Draw(sprites);
            _muteSongButton.Draw(sprites);
            _goBackButton.Draw(sprites, spriteBatch);
            _EffectSlider.Draw(sprites);
            _muteEffectButton.Draw(sprites);

        }
    }
}
