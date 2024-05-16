
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Masonator;

//COMPONENTS
[RegisterComponent, NetworkedComponent, Serializable, NetSerializable]
public sealed partial class BankingConsoleComponent : Component //Empty Component to mark console
{
}
//SUPPLEMENTARY DATA STRUCTURES
[Serializable, NetSerializable]
public sealed class BankAccount
{
    public EntityUid? owner; //UID of player owning account (todo: make more advanced)
    public string name = "";
    public string password;
    public List<AccountBalance>? balances;
    public BankAccount(string desc, string pass, List<AccountBalance> initBalances)
    {
        name = desc;
        password = pass;
        balances = initBalances;
    }
}
[Serializable, NetSerializable]
public sealed partial class BankAccountMessage : BoundUserInterfaceMessage
{
    public BankAccount account;
    public BankAccountMessage(BankAccount acct)
    {
        account=acct;
    }
}
public sealed class AccountBalance
{
    public string currencyName = "default";
    public int quantity = 0;
}
public sealed class CurrencyBase
{

}
//EVENTS


//KEYS
[Serializable, NetSerializable]
public enum MasonomicsUiKey : byte
{
    BankingComputerBoundUserInterface
}

