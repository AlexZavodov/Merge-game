using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    //��������� ���� �������� � �� ��������� ��������� ������ ������ �������
    [SerializeField]
    private Vector2Int position;

    [SerializeField]
    private PlacedObject placedObject;

    public RectTransform rectTransform { get; private set; }

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

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    //������� �������, ��������� ����������� � �����
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

    //���������� ������ � ��� ������, empty ���� �� ����� ������ ���� ������
    public void MoveHereObject(PlacedObject moveObject, bool empty = true)
    {
        SetPlacedObject(moveObject);

        if (empty)
        {
            placedObject.LastParent.placedObject = null;
            rectTransform.SetAsLastSibling();
        }

        placedObject.SetParent(this);
    }

    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
    }
}
