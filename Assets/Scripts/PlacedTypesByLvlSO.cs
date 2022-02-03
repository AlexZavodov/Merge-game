using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlacedTypesByLvl")]
public class PlacedTypesByLvlSO : ScriptableObject
{
    [SerializeField]
    private int level = 0;

    [SerializeField]
    private List<PlacedTypeSO> placedTypes;

    public int Level
    {
        get
        {
            return level;
        }
    }

    public List<PlacedTypeSO> PlacedTypes
    {
        get
        {
            return placedTypes;
        }
    }
}
