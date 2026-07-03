public class RecruitingSystem
{
    private readonly UnitFactory _unitFactory;
    private readonly EconomySystem _economySystem;
    private readonly GridSystem _gridSystem;

    public RecruitingSystem(UnitFactory unitFactory, EconomySystem economySystem, GridSystem gridSystem)
    {
        _unitFactory = unitFactory;
        _economySystem = economySystem;
        _gridSystem = gridSystem;
    }

    public bool TryRecruit(Faction faction, UnitType unitType)
    {
        int cost = Configs.Units.GetCost(faction, unitType);

        if (_economySystem.Funds[faction].Amount.Value < cost)
            return false;

        _economySystem.ChangeFundsAmount(faction, -cost);
        _economySystem.ChangeIncomeAmount(faction, Configs.Units.GetIncomeAdding(faction, unitType));

        UnitModel unit = _unitFactory.CreateUnit(faction, unitType);

        Faction enemyFaction = 1 - unit.Faction;
        unit.Transform.position = _gridSystem.GetWorldPosition(enemyFaction, _gridSystem.Map.SpawnPoints[enemyFaction].Item1);
        unit.Go();

        return true;
    }
}
