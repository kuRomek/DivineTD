using UnityEngine;

public class TowerView : GridObjectView
{
    [SerializeField] private Transform _towerHead;

    public void LookAt(Vector3 target)
    {
        target = new(target.x, _towerHead.transform.position.y, target.z);
        Quaternion goalRotation = Quaternion.LookRotation(target - _towerHead.transform.position, Vector3.up);
        _towerHead.rotation = Quaternion.Lerp(_towerHead.rotation, goalRotation, 0.25f);
    }

    public void LookForward()
    {
        LookAt(Vector3.forward + transform.position);
    }
}
