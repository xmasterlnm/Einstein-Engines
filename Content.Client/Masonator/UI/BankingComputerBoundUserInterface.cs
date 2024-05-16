using System.ComponentModel;
using Content.Client.Computer;
using Content.Shared.IdentityManagement;
using Content.Shared.Masonator;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using SixLabors.ImageSharp.Memory;
using static Robust.Client.UserInterface.Controls.BaseButton;

namespace Content.Client.Masonator;

public sealed partial class BankingComputerBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private BankingComputerWindow? _menu;
    private BankingEditMenu? _editMenu;
    private BankingComputerBoundUserInterfaceState? lastState;
    public BankingComputerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }
    protected override void Open()
    {
        base.Open();

        var spriteSystem = EntMan.System<SpriteSystem>();
        var dependencies = IoCManager.Instance!;
        _menu = new BankingComputerWindow(Owner, EntMan, dependencies.Resolve<IPrototypeManager>(), spriteSystem);
        var localPlayer = dependencies.Resolve<IPlayerManager>().LocalEntity;
        var description = new FormattedMessage();

        string orderRequester;

        _editMenu = new BankingEditMenu();

        if (EntMan.TryGetComponent<MetaDataComponent>(localPlayer, out var metadata))
            orderRequester = Identity.Name(localPlayer.Value,EntMan);
        else
            orderRequester = string.Empty;


        _menu.OnClose += Close;     //equivalent to base.Close for now unless overriden later. This makes the closing of the window _main to close the entire interface.
        //_menu.base.OnClose is invoked when clicking x to close the window, so adding base.close to this accomplishes the task outline above.

        _menu.OnInspectAccount += InspectAccount;

        _editMenu.RatifyBalanceRowEvent += (args) =>
        { SendMessage(new BankAccountMessage(_editMenu.BuildAccount())); };

        _menu.OpenCentered();
    }
    private void InspectAccount(ButtonEventArgs args)
    {
        if (args.Button.Parent?.Parent is not AccountRow row || _editMenu is null) return;
        _editMenu.UpdateMenu(row.acct);
        _editMenu.OpenCentered();
    }
    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not BankingComputerBoundUserInterfaceState cState) return;
        if (_menu is null || cState.BankAccounts is null) return;
        // Making sure things are as they should be - UpdateState must take BoundUserInterfaceState but for the compiler we must make sure that it is impossible for such a state to be other than the derived type we desire to use.
        lastState = cState;

        switch (_menu.inspecting)
        {
            default:
                _menu.PopulateAccounts(cState.BankAccounts);
                break;
        }
    }
}
