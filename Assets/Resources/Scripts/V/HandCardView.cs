using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//手牌视图层
public class HandCardView : MonoBehaviour{

    HandCardControl handCardControl;          //手牌数据

    private List<CardView> cardViewDatas;    // 每张卡牌视图的数据
    private List<CardView> selectViewDatas;  // 被选中的视图的数据
    public Color color;
    //public Vector3 cardSelectMoveDir;	//卡片被选中时偏移量
    //public float cardInterval;			//卡片间隔
    int a = 0;//当计数器用的
    public int firstIndex;// 开始点下鼠标选择的卡片索引

    public int secondIndex;// 拖动经过的卡片索引
    public bool ShiFo;//当拖动结束一次的开关
    public bool isDrag;//改变牌颜色的开关
      
    void Awake()
    {
      
        cardViewDatas = new List<CardView>();
        selectViewDatas = new List<CardView>();

        handCardControl = transform.GetComponent<HandCardControl>();
        
    }
    
    void Start()
    {
     
    
      

        if (handCardControl != null)
        {
            handCardControl.HandCardData.InitEvent += Init;
            handCardControl.HandCardData.AddCardsEvent += OnHandCardAdd;
            handCardControl.HandCardData.RemoveCardsEvent += OnHandCardRemove;
        }
        else
        {
            Debug.LogError("Can't find handCardControl");
        }
    }

    public void OnHandCardAdd(HandCardData handCardData, List<CardData> addCardDatas)
    {

        Debug.Log("add Card");
        foreach(CardData c in addCardDatas)
        {
            GameObject go = CardViewCreator.Instance.CreateCard(c);
            go.transform.SetParent(this.transform);
            
            //go.GetComponent<CardView>().index =a;
            cardViewDatas.Add(go.GetComponent<CardView>());
        }
      
        Sort();
          // 更新所有手上卡牌的索引 , 设置他们的index
        for(int i =0; i < cardViewDatas.Count; i++)
        {
            cardViewDatas[i].index = i+1;
        }

       
    }

    public void OnHandCardRemove(HandCardData handCardData, List<CardData> removeCardDatas)
    {
        Debug.Log("in Remove");
        foreach(CardData c in removeCardDatas)
        {
            for(int i = 0; i < cardViewDatas.Count; i++)
            {
                if(cardViewDatas[i].data.num == c.num && cardViewDatas[i].data.typeColor == c.typeColor)
                {
                    GameObject go = cardViewDatas[i].gameObject;
                    cardViewDatas.RemoveAt(i);
                    Destroy(go);
                    break;
                }
            }
        }

        Sort();
        // 更新所有手上卡牌的索引 , 设置他们的index
        for (int i =0; i < cardViewDatas.Count; i++)
        {
            cardViewDatas[i].index = i+1;
        }

    }

    // 当单张增加删除时可以在原来的位置上调整
    // 但在多张同时减少时，应该同一重新计算位置--待完善
    // 移动卡片
    public void Sort()
    {
        cardViewDatas.Sort();
        cardViewDatas.Reverse();
        float interval = GlobalSetting.Instance.cardInterval;
        Vector3 beginPos = Vector3.left * interval * (float)(cardViewDatas.Count - 1) * 0.5f;
        Vector3 beginDeep = Vector3.forward * cardViewDatas.Count * 0.1f * 0.5f;

        for (int i = 0; i < cardViewDatas.Count; i++)
        {
            Vector3 newPos = beginPos + Vector3.right * i * interval;
            Vector3 newDeep = beginDeep + Vector3.back * i * 0.2f;
            cardViewDatas[i].MoveTo(newPos + newDeep);
            cardViewDatas[i].IsSelected = false;
        }
    }

    public void Init()
    {
        for(int i = cardViewDatas.Count - 1; i >= 0; i--)
        {
            Destroy(cardViewDatas[i].gameObject);
        }

        for(int i = selectViewDatas.Count - 1; i >= 0; i--)
        {
            Destroy(selectViewDatas[i].gameObject);
        }

        cardViewDatas.Clear();
        selectViewDatas.Clear();
    }
   // 拖动出牌方法
    public void ChangeList(int begin, int end)
    { 
        int b = 1;
        if (a<b)
        {
            ShiFo = true;
            a++;
        }
        else
        {
            ShiFo = false;
            a = 0;
        }
        Debug.Log("a:" + a);


        if (begin != end)
        {
            for (int i = begin-1; i < end; i++)
            {



                if (ShiFo)
                {
                    cardViewDatas[i].IsSelected = true;

                   cardViewDatas[i].GetComponent<SpriteRenderer>().color=color;
                  
                }
                else
                {
                    cardViewDatas[i].IsSelected = false;
                    cardViewDatas[i].GetComponent<SpriteRenderer>().color = Color.white;
                }


            }
            for (int j = begin-1; j >= end-1; j--)
            {


                if (ShiFo)
                {
                    cardViewDatas[j].IsSelected = true;
                   cardViewDatas[j].GetComponent<SpriteRenderer>().color =color;
                }
                else
                {
                    cardViewDatas[j].IsSelected = false;
                    cardViewDatas[j].GetComponent<SpriteRenderer>().color = Color .white;
                }

            }

        }
    }

    //改变牌颜色的方法
    public void SetSecondIndex(int index)
    {
        secondIndex = index;

        foreach (CardView  c in cardViewDatas)
        {
            c.GetComponent<SpriteRenderer>().color = Color.white;

        }

        //  确保firstIndex小于或等于SecondIndex
        if (secondIndex < firstIndex)
        {
            for (int i = secondIndex; i <= firstIndex; i++)
            {

                cardViewDatas[i].GetComponent<SpriteRenderer>().color = color;
            }
        }
        else
        {
            for (int i = firstIndex; i <= secondIndex; i++)
            {

                cardViewDatas[i].GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
    public void EndSelect()
    {
        foreach (CardView  c in cardViewDatas )
        {
            c.GetComponent<SpriteRenderer>().color = Color.white;
        }

        firstIndex = secondIndex = 0;
    }
}



