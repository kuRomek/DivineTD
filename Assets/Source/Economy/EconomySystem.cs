using System.Collections.Generic;
using UnityEngine;

public class EconomySystem : IUpdatable
{
    private readonly LevelsSystem _levelsSystem;

    private readonly Dictionary<Faction, (RV<int> Income, RV<int> Amount)> _funds = new()
    {
        { Faction.Heaven, (new(0), new(0)) },
        { Faction.Hell, (new(0), new(0)) }
    };

    private readonly Dictionary<Faction, (float CD, RV<float> RemainingTime)> _cooldowns = new()
    {
        { Faction.Heaven, (float.PositiveInfinity, new(float.PositiveInfinity)) },
        { Faction.Hell, (float.PositiveInfinity, new(float.PositiveInfinity)) }
    };

    public EconomySystem(LevelsSystem levelsSystem)
    {
        _levelsSystem = levelsSystem;

        _levelsSystem.LevelStarted += OnLevelStarted;
    }

    public IReadOnlyDictionary<Faction, (RV<int> Income, RV<int> Amount)> Funds => _funds;

    public void Update(float deltaTime)
    {
        foreach (var faction in _cooldowns.Keys)
        {
            var (CD, RemainingTime) = _cooldowns[faction];
            RemainingTime.Value = Mathf.Max(0f, RemainingTime.Value - deltaTime);

            if (RemainingTime.Value == 0f)
            {
                GetIncome(faction);
                RemainingTime.Value = CD;
            }
        }
    }

    public void GetIncome(Faction faction)
    {
        _funds[faction].Amount.Value += _funds[faction].Income.Value;
    }

    private void OnLevelStarted()
    {
        var (IncomeHeaven, AmountHeaven) = _funds[Faction.Heaven];
        var dataHeaven = Configs.Levels.GetFunds(GameState.CurrentPlayerFaction, Faction.Heaven, GameState.CurrentLevel);

        IncomeHeaven.Value = dataHeaven.Income;
        AmountHeaven.Value = dataHeaven.Amount;

        var (IncomeHell, AmountHell) = _funds[Faction.Hell];
        var dataHell = Configs.Levels.GetFunds(GameState.CurrentPlayerFaction, Faction.Hell, GameState.CurrentLevel);

        IncomeHell.Value = dataHell.Income;
        AmountHell.Value = dataHell.Amount;

        var cdHeaven = _cooldowns[Faction.Heaven];

        cdHeaven.CD = Configs.Economy.StartingCooldowns[Faction.Heaven];
        cdHeaven.RemainingTime.Value = cdHeaven.CD;
        _cooldowns[Faction.Heaven] = cdHeaven;

        var cdHell = _cooldowns[Faction.Hell];

        cdHell.CD = Configs.Economy.StartingCooldowns[Faction.Hell];
        cdHell.RemainingTime.Value = cdHell.CD;
        _cooldowns[Faction.Hell] = cdHell;
    }
}
