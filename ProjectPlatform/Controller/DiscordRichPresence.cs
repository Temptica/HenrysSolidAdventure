using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Mapfolder;

namespace HenrySolidAdventure.Controller
{
    internal class DiscordRichPresence
    {

        //signleton

        private static DiscordRichPresence _instance;
        public static DiscordRichPresence Instance => _instance ??= new DiscordRichPresence();
        
        public DiscordRpcClient client;
        public DiscordRichPresence()
        {
            client = new DiscordRpcClient("1046877999989002300");
            client.Initialize();
            client.SetPresence(new RichPresence
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

        private GameState lastGameState = GameState.Menu;

        public void UpdateState(GameState state)
        {
            if (state != lastGameState)
            {
                client.UpdateState(state.ToString());
                switch (state)
                {
                    case GameState.GameOver:
                    case GameState.Win:
                    case GameState.Menu:
                        client.SetPresence(new RichPresence
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
                        client.SetPresence(new RichPresence
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
                        client.SetPresence(new RichPresence
                        {
                            Details = "Playing",
                            State = "Level: " + MapLoader.MapID,
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
            client.UpdateState("Level: " + MapLoader.MapID);//have to be like this, {} don't work here sadly
        }

        public void UpdateHealth()
        {
            client.UpdateLargeAsset("heroknight", "HP: " + Hero.Instance.Health + "/" + Hero.Instance.BaseHp);
        }

        public void UpdateCoins()
        {
            client.UpdateSmallAsset("coin", "Coins: " + Hero.Instance.Coins);
        }

    }
}
