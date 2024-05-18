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
        SubscribeLocalEvent<BankingConsoleComponent, BankStringMessage>(BankingConsoleCommand);
    }
    public void UpdateComputerBui(EntityUid consoleUid, EntityUid? station) //updates BUI of Banking Computers
    {

        if (station == null ||
                !TryComp<StationBankingDatabaseComponent>(station, out var database)) return;

        if (_uiSystem.TryGetUi(consoleUid, MasonomicsUiKey.BankingComputerBoundUserInterface, out var bui))
        {
            _uiSystem.SetUiState(bui, new BankingComputerBoundUserInterfaceState(
                MetaData(station.Value).EntityName,
                database.BankAccounts
            ));
        }
    }
    public void OnReceiveBankAccount(EntityUid uid, BankingConsoleComponent component, BankAccountMessage acct)
    {
        var station = _stationSystem.GetOwningStation(uid);
        if (station == null ||
                !TryComp<StationBankingDatabaseComponent>(station, out var database)) return;

        foreach (BankAccount account in database.BankAccounts)
        {

        }

        database.BankAccounts.Add(acct.account);
        UpdateComputerBui(uid, station);
    }
    public void BankingConsoleCommand(EntityUid uid, BankingConsoleComponent component, BankStringMessage msg)
    {
        var station = _stationSystem.GetOwningStation(uid);
        if (station==null || !TryComp<StationBankingDatabaseComponent>(station, out var database)) return;
        string str = msg.str;
        //Command Parsing
        if (!str.Contains(":")) return;
        string command = str.Substring(0, str.IndexOf(":"));
        string data = str.Substring(str.IndexOf(":") + 1);
        List<string> parameters = new();
        while (data.Contains(","))
        {
            parameters.Add(data.Substring(0, data.IndexOf(",")));
            data.Remove(0, data.IndexOf(",") + 1);
        }
        parameters.Add(data);

        switch (command)
        {
            case "delete":
                foreach (BankAccount acc in database.BankAccounts)
                {
                    if (acc.name == data) database.BankAccounts.Remove(acc);
                }
                break;
            default:
                return;
        }
    }
}

public sealed partial class TellerMachineSystem : EntitySystem
{
    TellerMachineSystem()
    {
    }
}
// Datatypes stored serverside to facilitate the system


