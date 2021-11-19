using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class HuaiDang : MonoBehaviour, IDropHandler, IPointerDownHandler 
{
    public RectTransform viewportRt;
    private Vector2 rectSize;
    public RectTransform rt;
    private bool canMove = true;
    public float speed=0.1f;
    public Vector2 desPos;
  

    public Button leftButton;
    public Button rightButton;
   
    
    void Start()
    {
        rt = GetComponent<RectTransform>();
        desPos = rt.anchoredPosition;
        rightButton.onClick.AddListener(rightButtonShiJiang);
        leftButton.onClick.AddListener(leftButtonShiJiang);
    }
    void Update()
    {



        if (Vector2.Distance(rt.anchoredPosition, desPos) > 0.1f && canMove == true)
        {
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, desPos, speed);
        }
        
    }
    



    public void OnDrop(PointerEventData eventData)
    {

       


        if (rt.anchoredPosition.x >= 500)
        {
            SetDesPos(new Vector2(900, 6));
        }
        else if (rt.anchoredPosition.x <= -600)
        {
            SetDesPos(new Vector2(-935, 6));
        }

        else if (rt.anchoredPosition.x <= 0)
        {
            SetDesPos(new Vector2(-325, 6));
        }

        else if (rt.anchoredPosition.x <= 650)
        {
            SetDesPos(new Vector2(270, 6));
        }

    //}
        canMove = true;
    }

    public void SetDesPos(Vector2 desPos)
    {
        this.desPos = desPos;
        //canMove = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
     
        canMove = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
      
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

   
    public void rightButtonShiJiang()
    {
        
        if (rt.anchoredPosition.x <= -900 && rt.anchoredPosition.x >= -940 )
        {
            SetDesPos(new Vector2(-325, 6));
          
           
        }

        else if (rt.anchoredPosition.x >= -340 && rt.anchoredPosition.x <= -300)
        {
            SetDesPos(new Vector2(270, 6));
            
           
        }

        else if (rt.anchoredPosition.x >= 250 && rt.anchoredPosition.x <= 290)
        {
            SetDesPos(new Vector2(900, 6));
            
            
        }


    }

    public void leftButtonShiJiang()
    {
        // b = false;
        if (rt.anchoredPosition.x >= 880 && rt.anchoredPosition.x <= 940) 
        {
            SetDesPos(new Vector2(270, 6));
           
         
        }

        else if (rt.anchoredPosition.x >= -340 && rt.anchoredPosition.x <= -300 )
        {
            SetDesPos(new Vector2(-919, 6));
          
            
        }

        else if (rt.anchoredPosition.x >=250 && rt.anchoredPosition.x <= 290 )
        {
            SetDesPos(new Vector2(-325, 6));
            
           
        }


    }


  


}











