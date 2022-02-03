using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestObject : PlacedObject
{
    PlacedTypesByLvlSO Lvl1;
    private void Start()
    {
        Lvl1 = Resources.Load<PlacedTypesByLvlSO>("Lvl1");
    }
    public override void ClickOnObject()
    {
        int random = Random.Range(0, Lvl1.PlacedTypes.Count);
        GameManager.Instance.CreateObject(Lvl1.PlacedTypes[random], LastParent.Position);
    }
}
