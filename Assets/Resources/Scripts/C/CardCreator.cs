using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


public class CardCreator : MonoBehaviour
{

    #region 单例
    private static CardCreator _instance;
    public static CardCreator Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            return null;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    #endregion

    private List<CardData> cardDatas;

    public void Start()
    {
        // Load resouces
        Init();
    }

    // 初始化
    public void Init()
    {
        if (cardDatas == null)
            cardDatas = new List<CardData>();
        else
            cardDatas.Clear();

        for (int i = 0; i < 13; i++)
            for (int j = 0; j < 4; j++)
                cardDatas.Add(new CardData((CardNum)i, (ColorKind)j));

        // 洗牌
        // 洗牌次数
        int changeNum = 100;
        while (changeNum > 0)
        {
            changeNum--;
            int randomIndex_1 = UnityEngine.Random.Range(0, cardDatas.Count);
            int randomIndex_2 = UnityEngine.Random.Range(0, cardDatas.Count);

            CardData temp = cardDatas[randomIndex_1];
            cardDatas[randomIndex_1] = cardDatas[randomIndex_2];
            cardDatas[randomIndex_2] = temp;
        }
    }

    // 获得一张牌
    public CardData GetNewCardData()
    {
        if (cardDatas.Count > 0)
        {
            CardData newCardData = cardDatas[0];
            cardDatas.RemoveAt(0);
            return newCardData;
        }

        return null;
    }

    // 批量获得
    public CardData[] GetNewCardData(int num)
    {
        if (num <= cardDatas.Count)
        {
            CardData[] newCardDatas = new CardData[num];
            for (int i = 0; i < num; i++)
            {
                newCardDatas[i] = GetNewCardData();
            }

            return newCardDatas;
        }

        return null;
    }
}
