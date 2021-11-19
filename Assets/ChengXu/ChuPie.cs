using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
public class ChuPie : MonoBehaviour,IPointerDownHandler ,IPointerUpHandler ,
    IEndDragHandler, IDragHandler,IPointerEnterHandler,IPointerExitHandler  {
    private Transform rt;
    public  bool Shif =true;
    public int index;
   // public  Image image;

    //出牌
    // Use this for initialization
    void Start () {
        rt = GetComponent<Transform>();
       
	}

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag == "card"&&
            eventData.pointerCurrentRaycast.gameObject.GetComponent<ChuPie>().index!=index)
        {
            int tempIndex=eventData.pointerCurrentRaycast.gameObject.GetComponent<ChuPie>().index;
            Debug.Log("index:" + index);
            Debug.Log("tempIndex:" + tempIndex);
           
                 HandCard.Instance.ChangeList(index, tempIndex);       
        }
        else
        {
            Debug.Log("no");
        }

        HandCard.Instance.isDrag = false;
       // HandCard.Instance.EndSelect();
    }
    public void Change()
    {
        if (Shif)
        {
            rt.transform .position = new Vector2(0, 10);  
            Shif = false;
        }
        else
        {  
          rt.transform .position  = new Vector2(0, 0);
          
            Shif = true;
        }
       
    }
   
    public void OnDrag(PointerEventData eventData)
    {
        HandCard.Instance.isDrag = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandCard.Instance.firstIndex = this.index - 1;
        Debug.Log("PointerDown, index = " + (index - 1));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag == "card" && 
            eventData.pointerCurrentRaycast.gameObject.GetComponent<ChuPie>().index == index)
        {
            Change();
         
        }
       
          
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {

       // if(HandCard.Instance.isDrag == true)
           // HandCard.Instance.SetSecondIndex(this.index - 1);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
    
    //public void SetImage(bool b)
    //{
      // image.gameObject.SetActive(b);
    //}
}
