namespace Westbot
{
    /// <summary>
    /// The enum used to specify permission levels. A lower
    /// number means less permissions than a higher number.
    /// </summary>
    /// These values are utilized in "MinPermissionsAttribute.cs"

    public enum AccessLevel
    {
        Blocked,
        User,
        ServerMod,
        ServerAdmin,
        Nelson,
        ServerOwner,        
        BotOwner
    }
}
