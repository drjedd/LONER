using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

//inherit more interfaces for Drag'n'Drop 
public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    public Item item;
    public int amount;

    private ToolTipBehaviour toolTip;
    private InventoryManager inventory;
    //Messy IMO, needed for Drag'n'Drop logic
    public int slotID;

    private Vector2 offset;

    void Start ()
    {
        inventory = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        toolTip = inventory.GetComponent<ToolTipBehaviour>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
		if (eventData.button == PointerEventData.InputButton.Left && MainCameraBehaviour.aimingMode == false)
		{
			if (item != null)
			{
				//move out of slot for logic purposes and to address z-layer issues
				this.transform.SetParent(this.transform.parent.parent);

				//offset when mouse is not grabbing the object at its centre
				offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
				this.transform.position = eventData.position - offset;

				GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
	}

    public void OnDrag(PointerEventData eventData)
    {
		if (eventData.button == PointerEventData.InputButton.Left && MainCameraBehaviour.aimingMode == false)
		{
			if (item != null)
			{
				this.transform.position = eventData.position - offset;
			}
		}
	}

    public void OnEndDrag(PointerEventData eventData)
    {
		if (eventData.button == PointerEventData.InputButton.Left && MainCameraBehaviour.aimingMode == false)
		{
			//in case of unsuccessful drop
			this.transform.SetParent(inventory.slots[slotID].transform);
			this.transform.position = inventory.slots[slotID].transform.position;

			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.Deactivate();
    }
}
