using System.Collections.Generic;
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
        private Slider slider;
        private StateButton stateButton;
        private StateButton muteButton;
        private Button _goBackButton;
        Screen _screen;
        bool _loaded;
        public IGameScreen LastScreen { get; }

        public SettingsScreen(IGameScreen lastScreen, Screen screen, SpriteFont font, ContentManager content)//add content 
        {
            LastScreen = lastScreen;
            var settingText = "Settings";
            var volumeText = "Volume";
            var muteText = "Mute";
            var fullscreenText = "Fullscreen";
            var halfWidth = screen.Width / 2f;
            var length = font.MeasureString(settingText).Length();
            var textPos = new Vector2(halfWidth - length / 2, screen.Height / 10f);
            _texts = new List<Text>
            {
                new(textPos, settingText, Color.White, 1f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+165f), volumeText, Color.White, 0.2f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+250f), muteText, Color.White, 0.2f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+300f), fullscreenText, Color.White, 0.2f, 0f, font)
            };
            slider = new Slider(new Vector2(textPos.X +150f, textPos.Y + 150f), 0, 1, AudioController.Volume);
            muteButton = new StateButton(new Vector2(textPos.X + 150f, textPos.Y + 250f), false);
            stateButton = new StateButton(new Vector2(textPos.X +150f, textPos.Y + 300f), false);
            _goBackButton = new Button("Go back", content.Load<Texture2D>("Buttons/EmptyButton"), new Vector2(textPos.X, textPos.Y + 350f), "Back");
            _screen = screen;
            _loaded = true;
            _clickables = new List<Clickable>
            {
                slider, stateButton, _goBackButton, muteButton
            };
            muteButton.IsClicked = Settings.Instance.Setting.Muted;
        }
        public void Update(GameTime gameTime)
        {
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
            slider.Update(gameTime, _screen);
            if (muteButton.IsClicked)
            {
                AudioController.Volume = 0f;
                Settings.Instance.Setting.Muted = true;
                slider.Value = 0f;
            }
            else
            {
                if (slider.Value == 0)
                {
                    AudioController.Volume = slider.Value = Settings.Instance.Setting.Volume;
                }
                else AudioController.Volume = Settings.Instance.Setting.Volume = slider.Value;
            }
            
            stateButton.Update(_screen);
            muteButton.Update(_screen);
            
            
            Game1.SetFullScreen(Settings.Instance.Setting.FullScreen = stateButton.IsClicked);
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            BackGround.Instance.Draw(sprites);
            _texts.ForEach(t => t.Draw(spriteBatch));
            slider.Draw(sprites);
            stateButton.Draw(sprites);
            muteButton.Draw(sprites);
            _goBackButton.Draw(sprites, spriteBatch);
            
        }
    }
}
