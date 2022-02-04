using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToDestroy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    Transform destroyedObject;
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerData.Instance.ChangeScore(10);
        Destroy(destroyedObject.gameObject);
    }
}
