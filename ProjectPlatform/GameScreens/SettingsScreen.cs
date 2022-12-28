using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Controller;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.GameScreens
{
    internal class SettingsScreen: IGameScreen
    {
        List<Text> _texts;
        private Slider slider;
        private StateButton stateButton;
        Screen _screen;
        bool _loaded;
        public IGameScreen LastScreen { get; }

        public SettingsScreen(IGameScreen lastScreen, Screen screen, SpriteFont font)//add content 
        {
            LastScreen = lastScreen;
            var settingText = "Settings";
            var volumeText = "Volume";
            var fullscreenText = "Fullscreen";
            var halfWidth = screen.Width / 2f;
            var length = font.MeasureString(settingText).Length();
            var textPos = new Vector2(halfWidth - length / 2, screen.Height / 10f);
            _texts = new List<Text>
            {
                new(textPos, settingText, Color.White, 1f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+165f), volumeText, Color.White, 0.2f, 0f, font),
                new(new Vector2(textPos.X, textPos.Y+250f), fullscreenText, Color.White, 0.2f, 0f, font)
            };
            slider = new Slider(new Vector2(textPos.X +150f, textPos.Y + 150f), 0, 1, AudioController.Volume);
            stateButton = new StateButton(new Vector2(textPos.X +150f, textPos.Y + 250f), false);
            _screen = screen;
            _loaded = true;
        }
        public void Update(GameTime gameTime)
        {
            if (_loaded && InputController.ExitInput)
            {
                return;
            }
            _loaded = false;
            if (InputController.ExitInput)
            {
                Settings.Instance.Save();
                Game1.SetState(GameState.Menu);
            }
            slider.Update(gameTime, _screen);
            AudioController.Volume = Settings.Instance.Setting.Volume = slider.Value;
            stateButton.Update(gameTime, _screen);
            Game1.SetFullScreen(Settings.Instance.Setting.FullScreen = stateButton.IsClicked);
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            BackGround.Instance.Draw(sprites);
            _texts.ForEach(t => t.Draw(spriteBatch));
            slider.Draw(sprites);
            stateButton.Draw(sprites);
        }
    }
}
