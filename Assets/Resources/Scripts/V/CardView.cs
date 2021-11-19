using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CardView : MonoBehaviour, IComparable, IPointerClickHandler, IDragHandler,
    IEndDragHandler, IPointerUpHandler,IPointerEnterHandler ,IPointerExitHandler ,IPointerDownHandler,IBeginDragHandler   {

    public CardData data;                               //绑定数据，监听
    public HandCardControl handCardControl;
    //public CardViewControl cardViewControl;        //绑定相应的控制器
    
    public SpriteRenderer cardBGSpriteRenderer;
    public SpriteRenderer numSpriteRenderer;
    public SpriteRenderer typeSpriteRenderer;
    public int index;//牌的索引号
    public bool BiangSi;
    public HandCardView handcardview;
    public bool IsSelected
    {
        // Trigger
        get
        {
            return data.IsSelected;
        }
        set
        {
            data.IsSelected = value;
        }
    }
    void Awake()
    {
       // handcardview =transform .parent .GetComponent<HandCardView>();
    
    }
    void Start()
    {
         handcardview = transform.parent.GetComponent<HandCardView>();
        handcardview = transform.root.GetComponent<HandCardView>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("abc");
        IsSelected = !IsSelected;

    }

    public void MoveTo(Vector3 pos) {
        transform.localPosition = pos;
    }

    public void OnSelectChanged(CardData cardData, bool changeTo) {
        if (changeTo == true)
        {
            transform.localPosition += GlobalSetting.Instance.cardSelectMoveDir;
        }
        else
        {
            transform.localPosition -= GlobalSetting.Instance.cardSelectMoveDir;
        }
    }

    #region 设置卡片上的所有素材
    public void SetSprite(Sprite numSprite, Sprite typeSprite)
    {
        if (numSprite != null)
            numSpriteRenderer.sprite = numSprite;
        if (typeSprite != null)
            typeSpriteRenderer.sprite = typeSprite;
    }

    public void SetSprite(Sprite bgSprite, Sprite numSprite, Sprite typeSprite)
    {
        if (bgSprite != null)
            cardBGSpriteRenderer.sprite = bgSprite;

        SetSprite(numSprite, typeSprite);
    }
    #endregion

    // 每次设置数据时，替换为相应的数字、花色Sprite素材。
    public void SetData(CardData data)
    {
        // 先解除存在的监听
        if (this.data != null)
        {
            this.data.SelectedChanged -= OnSelectChanged;
        }
        else
        {
            this.data = data;
            // 添加对选择状态的监听
            data.SelectedChanged += OnSelectChanged;
        }

        // 寻找对应素材
        Sprite[] dataSprites = CardViewCreator.Instance.CardSpriteStore(this.data);
        if (dataSprites != null)
            SetSprite(dataSprites[0], dataSprites[1]);
    }

    public int CompareTo(object obj)
    {
        CardView other = obj as CardView;
        if (other != null)
        {
            return data.CompareTo(other.data);
        }

        throw new Exception("Can't Compare");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //handcardview.isDrag = true;
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (eventData.pointerCurrentRaycast.gameObject.tag == "Card")  
           
        {
            int tempIndex = eventData.pointerCurrentRaycast.gameObject.GetComponent<CardView>().index;
            Debug.Log("index:" + index);
            Debug.Log("tempIndex:" + tempIndex);

            handcardview.ChangeList(index, tempIndex);
          
            // DiangJing = false;
        }

       handcardview.EndSelect();
        handcardview.isDrag =false;
      
    } 
    
    public void OnPointerUp(PointerEventData eventData)
    {
        handcardview.isDrag = false;
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (handcardview.isDrag==true) {
            Debug.Log("SetSecondIndex:" + index);
            handcardview.SetSecondIndex(this.index-1);
       }


    }
    public void OnPointerExit(PointerEventData eventData)
    {
      
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        handcardview.firstIndex = this.index-1;
       // handcardview.isDrag = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        handcardview.isDrag = true;
    }
}
