using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : PlacedObject
{

    public override void ClickOnObject()
    {
        PlayerData.Instance.ChangeScore(10);
        DestroySelf();
    }
}
