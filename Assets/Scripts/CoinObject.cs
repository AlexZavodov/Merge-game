using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : PlacedObject
{

    public override void ClickOnObject()
    {//вместо объекта ClickToDestroy можно раскоментировать эти строки
        //PlayerData.Instance.ChangeScore(10);
        //DestroySelf();
    }
}
