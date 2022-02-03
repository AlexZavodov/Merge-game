using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/PlacedType")]
public class PlacedTypeSO : ScriptableObject
{
    [SerializeField]
    private string _name = "%NaN%";

    [SerializeField]
    private RectTransform prefab;

    [SerializeField]
    private List<PlacedTypeSO> nextMergeObjects;

    public PlacedTypeSO this[int index]
    {
        get
        {
            if (nextMergeObjects.Count > index)
                return nextMergeObjects[index];
            return null;
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
    }
    public RectTransform Prefab
    {
        get
        {
            return prefab;
        }
    }
}
