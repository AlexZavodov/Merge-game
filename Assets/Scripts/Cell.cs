using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    //поскольку поле статично и не изменяемо заполняем данные клеток вручную
    [SerializeField]
    private Vector2Int position;

    [SerializeField]
    private PlacedObject placedObject;

    public PlacedObject PlacedObject
    {
        get
        {
            return placedObject;
        }
    }

    public Vector2Int Position
    {
        get
        {
            return position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        PlacedObject eventObject = eventData.pointerDrag.GetComponent<PlacedObject>();
        if (eventObject != null && eventObject!=placedObject)
        {
            if (placedObject == null)
            {
                MoveHereObject(eventObject);
            }else
            {
                PlacedTypeSO typeNewObject = placedObject.MergeObject(eventObject);
                if (typeNewObject != null)
                {
                    placedObject.DestroySelf();
                    eventObject.DestroySelf();

                    PlacedObject newObject = PlacedObject.Create(typeNewObject, this);
                }else
                {
                    Cell lastParent = eventObject.LastParent;
                    eventObject.LastParent.placedObject = null;

                    GameManager.Instance.RandomMoveObject(placedObject, position);

                    MoveHereObject(eventObject, false);
                }    
                
            }
        }
    }

    public void MoveHereObject(PlacedObject moveObject, bool empty = true)
    {
        placedObject = moveObject;
        if (empty) placedObject.LastParent.placedObject = null;

        placedObject.SetParent(this);
    }

    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
    }
}
