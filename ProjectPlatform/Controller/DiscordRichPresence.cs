using System;
using DiscordRPC;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.MapFolder;

namespace HenrySolidAdventure.Controller
{
    internal class DiscordRichPresence
    {

        //signleton

        private static DiscordRichPresence _instance;
        public static DiscordRichPresence Instance => _instance ??= new DiscordRichPresence();
        
        public DiscordRpcClient Client;
        public DiscordRichPresence()
        {
            Client = new DiscordRpcClient("1046877999989002300");
            Client.Initialize();
            Client.SetPresence(new RichPresence
            {
                Details = "In Menu",
                State = "Testing his sword",
                Assets = new Assets
                {
                    LargeImageKey = "slime",
                    LargeImageText = "A slime has 6HP and does 3 damage. It can only detect you within a few meters"
                },
                Timestamps = new Timestamps
                {
                    Start = DateTime.UtcNow
                },
                Buttons = new[]
                {
                    new Button
                    {
                        Label = "Github repo",
                        Url = "https://github.com/Temptica/Otterly_Adventure/releases"
                    }
                }
            });
        }

        private GameState _lastGameState = GameState.Menu;

        public void UpdateState(GameState state)
        {
            if (state != _lastGameState)
            {
                Client.UpdateState(state.ToString());
                switch (state)
                {
                    case GameState.GameOver:
                    case GameState.Win:
                    case GameState.Menu:
                        Client.SetPresence(new RichPresence
                        {
                            State = "In Menu",
                            Details = "Drinking a energy potion",
                            Assets = new Assets
                            {
                                LargeImageKey = "bat",
                                LargeImageText = "A bat has 3 HP, does 4 damage and tracks you within a 50 meters range",
                                SmallImageKey = "",
                                SmallImageText = ""
                            },
                            Timestamps = new Timestamps
                            {
                                Start = DateTime.UtcNow
                            },
                            Buttons = new[]
                            {
                                new Button
                                {
                                    Label = "Github repo",
                                    Url = "https://github.com/Temptica/Otterly_Adventure/releases"
                                }
                            }
                        });
                        break;
                    case GameState.Settings:
                    case GameState.Paused:
                        Client.SetPresence(new RichPresence
                        {
                            State = "Paused",
                            Details = "Taking a break",
                            Assets = new Assets
                            {
                                LargeImageKey = "skeleton",
                                LargeImageText = "A skeleton has 14 HP, does 5 damage and detects you whenever you enter his platform",
                                SmallImageKey = "",
                                SmallImageText = ""
                            },
                            Timestamps = new Timestamps
                            {
                                Start = DateTime.UtcNow
                            },
                            Buttons = new[]
                            {
                                new Button
                                {
                                    Label = "Github repo",
                                    Url = "https://github.com/Temptica/Otterly_Adventure/releases"
                                }
                            }
                        });
                        break;
                    case GameState.Playing:
                        Client.SetPresence(new RichPresence
                        {
                            Details = "Playing",
                            State = "Level: " + MapLoader.MapId,
                            Assets = new Assets
                            {
                                LargeImageKey = "heroknight",
                                LargeImageText = "HP: " + Hero.Instance.Health + "/" + Hero.Instance.BaseHp,
                                SmallImageKey = "coin",
                                SmallImageText = "Coins: " + Hero.Instance.Coins
                            },
                            Timestamps = new Timestamps
                            {
                                Start = DateTime.UtcNow
                            },
                            Buttons = new[]
                            {
                                new Button
                                {
                                    Label = "Github repo",
                                    Url = "https://github.com/Temptica/Otterly_Adventure/releases"
                                }
                            }
                        });
                        break;
                }
            }
        }

        public void UpdateLevel()
        {
            Client.UpdateState("Level: " + MapLoader.MapId);//have to be like this, {} don't work here sadly
        }

        public void UpdateHealth()
        {
            Client.UpdateLargeAsset("heroknight", "HP: " + Hero.Instance.Health + "/" + Hero.Instance.BaseHp);
        }

        public void UpdateCoins()
        {
            Client.UpdateSmallAsset("coin", "Coins: " + Hero.Instance.Coins);
        }

    }
}
