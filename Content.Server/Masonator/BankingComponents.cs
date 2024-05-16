using Content.Server.Administration.Commands;
using Content.Server.Station.Systems;
using Microsoft.Extensions.Configuration;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Timing;

using Content.Shared.Masonator;

namespace Content.Server.Masonator.Economy;

[RegisterComponent]
public sealed partial class StationBankingDatabaseComponent : Component
{
    public List<string> currencyNames = new();
    public List<BankAccount> BankAccounts = new();
}
