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

    public static PlacedObject Create (PlacedTypeSO placedType, Cell parent, Transform creator = null)
    {
        RectTransform transform;
        if (creator == null)
            transform = Instantiate(placedType.Prefab, parent.transform);
        else
            transform = Instantiate(placedType.Prefab, creator.position, Quaternion.identity, parent.transform);

        PlacedObject placedObject = transform.GetComponent<PlacedObject>();
        placedObject.PlacedType = placedType;
        placedObject.SetParent(parent);

        parent.SetPlacedObject(placedObject);
        parent.rectTransform.SetAsLastSibling();

        return placedObject;
    }

    private void Awake()
    {
        canvas = GameManager.Instance.Canvas.GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    #region DragDrop
    //классический DragDrop
    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.pointerCurrentRaycast.worldPosition; // перемещение к положению мыши

        LastParent.rectTransform.SetAsLastSibling(); //фикс отображения

        canvasGroup.blocksRaycasts = false;

        isDrag = true;
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        //Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetParent(LastParent);
        canvasGroup.blocksRaycasts = true;
        //Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
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
        //rectTransform.anchoredPosition = Vector2.zero; //закоментите Update и раскоментите строку для отключения анимации перемещения
       
    }

    //движение к точке
    private void Update()
    {
        if (Vector2.Distance(Vector2.zero, rectTransform.anchoredPosition) > .5f && !isDrag)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, Vector2.zero, Time.deltaTime * 5);
        }else
        if (Vector2.Distance(Vector2.zero, rectTransform.anchoredPosition) < .5f && !isDrag && rectTransform.anchoredPosition != Vector2.zero)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    
    public PlacedTypeSO MergeObject(PlacedObject placedObject)
    {
        return PlacedType[NumberOfNextMergeObject(placedObject)];
    }

    //переопределять если в наследниках больше одного возможного merge объекта
    public virtual int NumberOfNextMergeObject(PlacedObject placedObject)
    {
        if (placedObject.PlacedType.Name == this.PlacedType.Name)
        {
            return 0;
        }

        return 99;
    }

    //переопределять для клика по объекту
    public virtual void ClickOnObject()
    {
    }

}
