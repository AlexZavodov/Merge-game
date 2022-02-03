using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField]
    public int Integer;
}
