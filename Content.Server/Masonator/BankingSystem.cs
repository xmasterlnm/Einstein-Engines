using Content.Server.Administration.Commands;
using Content.Server.Station.Systems;
using Microsoft.Extensions.Configuration;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Timing;

using Content.Shared.Masonator;
using Content.Server.Cargo.Components;
using Microsoft.CodeAnalysis.CSharp;

namespace Content.Server.Masonator.Economy;

public sealed partial class BankingSystem : EntitySystem
{
    [Dependency] private readonly StationSystem _stationSystem = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BankingConsoleComponent, BankAccountMessage>(OnReceiveBankAccount);
    }
    public void UpdateComputerBui(EntityUid consoleUid, EntityUid? station) //updates BUI of Banking Computers
    {

        if (station == null ||
                !TryComp<StationBankingDatabaseComponent>(station, out var orderDatabase)) return;

        if (_uiSystem.TryGetUi(consoleUid, MasonomicsUiKey.BankingComputerBoundUserInterface, out var bui))
        {
            _uiSystem.SetUiState(bui, new BankingComputerBoundUserInterfaceState(
                MetaData(station.Value).EntityName,
                orderDatabase.BankAccounts
            ));
        }
    }
    public void OnReceiveBankAccount(EntityUid uid, BankingConsoleComponent component, BankAccountMessage acct)
    {
        var station = _stationSystem.GetOwningStation(uid);
        if (station == null ||
                !TryComp<StationBankingDatabaseComponent>(station, out var orderDatabase)) return;

        foreach (BankAccount account in orderDatabase.BankAccounts)
        {

        }

        orderDatabase.BankAccounts.Add(acct.account);
        UpdateComputerBui(uid, station);
    }
}

public sealed partial class TellerMachineSystem : EntitySystem
{
    TellerMachineSystem()
    {
    }
}
// Datatypes stored serverside to facilitate the system


