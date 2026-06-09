using kuRomek.SimpleVG;
using DG.Tweening;
using UnityEngine;

public class Tower : GridObject, IInteractable
{
    private Tween _shooting;
    private readonly Transform _gunTip;

    public Tower(View view, Model model, GridSystem gridSystem, TriggerDetector
        triggerDetector, Transform gunTip)
        : base(view, model, gridSystem)
    {
        TriggerDetector = triggerDetector;
        _gunTip = gunTip;
        triggerDetector.SetTriggerRadius(Model.Params.Radius);
        triggerDetector.Toggle(Model.IsDraft == false);

        Model.ToggledDrafting += (isDraft) => triggerDetector.Toggle(isDraft == false);
        triggerDetector.EnteredTrigger += (this as IInteractable).OnTriggerEnter;
        triggerDetector.ExitedTrigger += (this as IInteractable).OnTriggerExited;
    }

    public new TowerModel Model => base.Model as TowerModel;
    public new TowerView View => base.View as TowerView;

    public TriggerDetector TriggerDetector { get; }

    void IInteractable.OnTriggerEnter(IDamageable damageable, IFactionRelated faction)
    {
        if (damageable is UnitModel && faction.Faction != Model.Faction)
        {
            AddTarget(damageable);
            damageable.Died += RemoveTarget;
        }
    }

    void IInteractable.OnTriggerExited(IDamageable damageable, IFactionRelated faction)
    {
        if (damageable is UnitModel && faction.Faction != Model.Faction)
            RemoveTarget(damageable);
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (Model.CurrentAttackTarget != null && Model.CurrentAttackTarget is Model model)
            View.LookAt(model.Transform.position);
        else
            View.LookForward();
    }

    private void AddTarget(IDamageable damageable)
    {
        Model.EnqueueTarget(damageable);

        if (Model.AttackTargets.Count == 1)
            StartShooting();
    }

    private void RemoveTarget(IDamageable damageable)
    {
        Model.DequeueTarget(damageable);
        damageable.Died -= RemoveTarget;

        if (Model.AttackTargets.Count == 0)
            StopShooting();
    }

    private void StartShooting()
    {
        StopShooting();

        Shoot();
        _shooting = DOVirtual.DelayedCall(1f / Model.Params.AttackRate, Shoot).SetLoops(-1, LoopType.Restart);
    }

    private void Shoot()
    {
        if (Model.CurrentAttackTarget is Model model)
        {
            Pools.Bullets.Get().Launch(
                _gunTip.position,
                new Vector3(model.Transform.position.x, _gunTip.position.y, model.Transform.position.z),
                Model.Params.Damage,
                1 - Model.Faction);
        }
    }

    private void StopShooting()
    {
        _shooting.Kill();
    }
}