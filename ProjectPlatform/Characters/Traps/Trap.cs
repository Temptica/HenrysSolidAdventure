namespace HenrySolidAdventure.Characters.Traps
{
    public enum TrapTier{One,Two,Three}
    internal abstract class Trap:Enemy
    {
        public TrapTier Tier { get; set; }
        

    }
}
