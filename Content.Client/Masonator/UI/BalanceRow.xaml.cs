using Content.Shared.Masonator;
using Robust.Client.UserInterface.Controls;

public sealed partial class BalanceRow : PanelContainer
{
    public AccountBalance? bal;
    public BalanceRow()
    {
        RobustXamlLoader.Load(this);
    }
}
