using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void VoidListDelegate(HandCardData handCardData, List<CardData> changeCards);
public delegate void VoidNoneDelegate();

public class HandCardData
{
    //single?

    public event VoidListDelegate AddCardsEvent;
    public event VoidListDelegate RemoveCardsEvent;
    public event VoidNoneDelegate InitEvent;

    //手牌数据
    private List<CardData> cardDatas;
    public List<CardData> CardDatas
    {
        get
        {
            if (cardDatas != null)
                return cardDatas;

            return null;
        }
    }

    //手牌中已选的数据
    private List<CardData> selectedCards;
    public List<CardData> SelectedCards
    {
        get
        {
            if (selectedCards != null)
                return selectedCards;

            return null;
        }
    }

    #region 构造函数
    public HandCardData()
    {
        cardDatas = new List<CardData>();
        selectedCards = new List<CardData>();
    }

    public HandCardData(float cardInterval, float selectDir)
    {
        cardDatas = new List<CardData>();
        selectedCards = new List<CardData>();
    }

    public HandCardData(List<CardData> temp, List<CardData> temp2)
    {
        cardDatas = temp;
        selectedCards = temp2;
    }
    #endregion


    // 初始化，清空手牌列表数据
    public void Init()
    {
        cardDatas.Clear();
        SelectedCards.Clear();

        if (InitEvent != null)
        {
            InitEvent();
        }
    }

    // 增加
    public void AddCard(CardData card)
    {
        cardDatas.Add(card);
        card.SelectedChanged += OnCardDataSelectChanged;

        // 触发事件
        List<CardData> addList = new List<CardData>();
        addList.Add(card);
        if (AddCardsEvent != null)
        {
            AddCardsEvent(this, addList);
        }
        else
        {
            Debug.Log("No evet");
        }
    }

    // 批量增加
    public void AddCard(List<CardData> cards)
    {
        List<CardData> addList = new List<CardData>();
        foreach (CardData c in cards)
        {
            cardDatas.Add(c);
            c.SelectedChanged += OnCardDataSelectChanged;

            addList.Add(c);
        }

        if (AddCardsEvent != null)
        {
            AddCardsEvent(this, addList);
        }
    }

    // 删除
    public void RemoveCard(CardData card)
    {
        selectedCards.Remove(card);
        cardDatas.Remove(card);
        card.SelectedChanged -= OnCardDataSelectChanged;

        // 触发事件
        List<CardData> removeList = new List<CardData>();
        removeList.Add(card);

        if (RemoveCardsEvent != null)
        {
            RemoveCardsEvent(this, removeList);
        }
        Debug.Log("remove in aldkfa");
    }

    // 批量删除
    public void RemoveCard(List<CardData> cards)
    {
        List<CardData> removeList = new List<CardData>();

        foreach (CardData c in cards)
        {
            selectedCards.Remove(c);
            cardDatas.Remove(c);
            c.SelectedChanged -= OnCardDataSelectChanged;

            removeList.Add(c);
        }


        if (RemoveCardsEvent != null)
        {
            RemoveCardsEvent(this, removeList);
        }
    }

    // 批量删除
    public void RemoveCard(CardData[] cards)
    {
        List<CardData> removeList = new List<CardData>();

        foreach (CardData c in cards)
        {
            selectedCards.Remove(c);
            cardDatas.Remove(c);
            c.SelectedChanged -= OnCardDataSelectChanged;

            removeList.Add(c);
        }

        if (RemoveCardsEvent != null)
        {
            RemoveCardsEvent(this, removeList);
        }
    }

    // public void RemoveSelectCard(CardData cardData)
    // {
    //     selectedCards.Remove(cardData);
    // }

    // public void RemoveAllSelectCard()
    // {
    //     selectedCards.Clear();
    // }

    public void SortCard()
    {
        cardDatas.Sort();
    }

    // 监听所有单个牌的选择变换情况
    public void OnCardDataSelectChanged(CardData cardData, bool changeTo)
    {
        if (changeTo == true)
        {
            selectedCards.Add(cardData);
        }
        else
        {
            selectedCards.Remove(cardData);
        }

        //Debug.Log("select Change");
    }
}
