using Robust.Shared.Serialization;

namespace Content.Shared.Masonator;
[Serializable, NetSerializable]
public sealed partial class BankingComputerBoundUserInterfaceState : BoundUserInterfaceState
{
    public string stationName = "N/A";
    public List<BankAccount>? BankAccounts { get; set;}
    public BankingComputerBoundUserInterfaceState(string str, List<BankAccount> accts)
    {
        stationName = str;
        BankAccounts = accts;
    }
}
