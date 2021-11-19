using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// 电脑玩家手牌显示类
public class BotView : MonoBehaviour {

    private Vector2 sortDir;
    HandCardControl handCardControl;   

    Text numText;                     //显示剩余牌数
    List<RectTransform> cardBackRts;  //卡背集合
    List<RectTransform> playerCards;  //出的牌

    void Awake()
    {
        //numText = transform.FindChild("Text").GetComponent<Text>();
        handCardControl = GetComponent<HandCardControl>();
        if (handCardControl == null)
        {
            Debug.LogError("Can't find handCardControl");
        }

        
    }

    //public void Start()
    //{
    //    handCardControl.HandCardData.AddCardsEvent += OnCardNumChanged;
    //    handCardControl.HandCardData.RemoveCardsEvent += OnCardNumChanged;
    //}

    //// 监听数据变化
    //public void OnCardNumChanged(HandCardData handCardData, List<CardData> cardDatas)
    //{
    //    numText.text = handCardData.CardDatas.Count.ToString();
    //}
}
