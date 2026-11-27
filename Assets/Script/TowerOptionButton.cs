using UnityEngine;

public class TowerOptionButton : MonoBehaviour
{
    public int towerIndex;
    public TowerSpot parentSpot;
    public void SelectThisTower()
    {
        parentSpot.BuildTower(towerIndex);
    }
}
