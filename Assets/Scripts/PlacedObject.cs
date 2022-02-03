using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacedObject : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;
    protected RectTransform rectTransform;

    public Cell LastParent{ get; private set; }
    public PlacedTypeSO PlacedType{ get; private set; }

    bool isDrag = false;

    public static PlacedObject Create (PlacedTypeSO placedType, Cell parent)
    {
        RectTransform transform = Instantiate(placedType.Prefab, parent.transform);

        PlacedObject placedObject = transform.GetComponent<PlacedObject>();
        placedObject.PlacedType = placedType;

        
        placedObject.SetParent(parent);
        parent.SetPlacedObject(placedObject);

        return placedObject;
    }

    private void Awake()
    {
        canvas = GameManager.Instance.Canvas.GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    #region DragDrop
    public void OnBeginDrag(PointerEventData eventData)
    {
        LastParent = transform.parent.GetComponent<Cell>();

        rectTransform.position = eventData.pointerCurrentRaycast.worldPosition;
        
        transform.SetParent(GameManager.Instance.Canvas.DragPanel);
        canvasGroup.blocksRaycasts = false;

        isDrag = true;
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == GameManager.Instance.Canvas.DragPanel)
        {
            SetParent(LastParent);
        }
        canvasGroup.blocksRaycasts = true;
        Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDrag)
        {
            ClickOnObject();
        }

        isDrag = false;
    }
    #endregion

    public void SetParent(Cell cell)
    {
        transform.SetParent(cell.transform);
        LastParent = cell;
        rectTransform.anchoredPosition = Vector3.zero;
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public PlacedTypeSO MergeObject(PlacedObject placedObject)
    {
        return PlacedType[NumberOfNextMergeObject(placedObject)];
    }

    public virtual int NumberOfNextMergeObject(PlacedObject placedObject)
    {
        if (placedObject.PlacedType.Name == this.PlacedType.Name)
        {
            return 0;
        }

        return 99;
    }


    public virtual void ClickOnObject()
    {
    }

}
