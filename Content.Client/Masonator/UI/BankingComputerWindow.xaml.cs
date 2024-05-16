using System.Linq;
using Content.Client.Cargo.UI;
using Content.Client.Computer;
using Content.Client.UserInterface.Controls;
using Content.Shared.Masonator;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;
using static Robust.Client.UserInterface.Controls.BaseButton;


namespace Content.Client.Masonator;

public sealed partial class BankingComputerWindow : FancyWindow
{
    private EntityUid _owner;

    private IEntityManager _entityManager;
    private IPrototypeManager _protoManager;
    private SpriteSystem _spriteSystem;
    public event Action<ButtonEventArgs>? OnInspectAccount;
    public string inspecting {get; private set;} = "accounts";

    public BankingComputerWindow(EntityUid owner, IEntityManager entMan, IPrototypeManager protoManager, SpriteSystem spriteSystem)
    {
        this.inspecting = "kek";
        RobustXamlLoader.Load(this);
        _owner = owner;
        _entityManager = entMan;
        _protoManager = protoManager;
        _spriteSystem = spriteSystem;

        Title = "SPESSBANK";
    }

    public void PopulateAccounts(List<BankAccount> accounts)
    {
        AccountsBox.RemoveAllChildren();
        var product = _protoManager.Index<EntityPrototype>("SpaceCash10");
        foreach (var account in accounts)
        {
            string str = "";
            foreach (var balance in account.balances)
            {
                str += $"{balance.currencyName}: {balance.quantity}\n";
            }
            var row = new AccountRow(account)
            {
                acct = account,
                AccountSummary =
                {
                    Text = str
                }
            };
            row.Inspect.OnPressed += (args) => { OnInspectAccount?.Invoke(args);};
            AccountsBox.AddChild(row);
        }
    }
}
public sealed partial class AccountRow : PanelContainer
{
    public BankAccount acct;
    public AccountRow(BankAccount account)
    {
        RobustXamlLoader.Load(this);
        acct = account;
    }
}
