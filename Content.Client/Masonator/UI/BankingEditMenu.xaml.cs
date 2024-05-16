using System.Linq;
using Content.Client.Cargo.UI;
using Content.Client.Computer;
using Content.Client.UserInterface.Controls;
using Content.Shared.Masonator;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;
using SixLabors.ImageSharp.Memory;
using static Robust.Client.UserInterface.Controls.BaseButton;

namespace Content.Client.Masonator;
public sealed partial class BankingEditMenu : FancyWindow
{
    //Row Events
    public event Action<ButtonEventArgs>? RatifyBalanceRowEvent;
    public event Action<ButtonEventArgs>? ModifyBalanceRowEvent;
    public event Action<ButtonEventArgs>? RemoveBalanceRowEvent;
    //Important
    public event Action<ButtonEventArgs>? EditMenuRatifyEvent;
    public BankAccount? inspectedAccountCopy;
    public BankingEditMenu()
    {
        RobustXamlLoader.Load(this);
        RatifyAccount.OnPressed += (args) => { EditMenuRatifyEvent.Invoke(args); };
    }
    public void UpdateMenu(BankAccount inAccount)
    {
        if (inAccount is null || inAccount.balances is null)
        {
            RowHolder.RemoveAllChildren(); return;
        }
        Password = inAccount.password;
        PopulateBalances(inAccount.balances);
    }
    public void Clear()
    {
        RowHolder.RemoveAllChildren();
    }
    public void PopulateBalances(List<AccountBalance> balances)
    {
        RowHolder.RemoveAllChildren();
        foreach (var balance in balances)
        {
            var row = new BalanceRow()
            {
                bal = balance,
                CurrencyName = {Text = $"{balance.currencyName}:"},
                Quantity = {Text=$"{balance.quantity}"}
                //TODO: Icon
            };
            row.Ratify.OnPressed += (args) => { RatifyBalanceRowEvent?.Invoke(args); };
            row.Modify.OnPressed += (args) => { ModifyBalanceRowEvent?.Invoke(args); };
            row.Remove.OnPressed += (args) => { RemoveBalanceRowEvent?.Invoke(args); };
            RowHolder.AddChild(row);
        }
    }
    public BankAccount BuildAccount()
    {
        BankAccount returnAcct = new
            (Inspected.Text,
            Password.Text,
            new List<AccountBalance>());
        foreach (Button child in RowHolder.Children)
        {
            returnAcct.balances += new AccountBalance
            {
                currencyName = CurrencyName.Text,
                quantity = Quantity.Text
            };
        }
        return returnAcct;
    }
}

public sealed partial class BalanceRow : PanelContainer
{
    public AccountBalance? bal;
    public BalanceRow()
    {
        RobustXamlLoader.Load(this);
    }
}
