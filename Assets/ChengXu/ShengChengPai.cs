using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ShengChengPai : MonoBehaviour
{
    private RectTransform WeiZhi;
    public GameObject image;
    public RectTransform b;
    HandCardControl handCardControl;
    public Text text;
    public GameObject g;
    List<GameObject> game = new List<GameObject>();

    private int currentCardCount;//当前牌的数量
    // Use this for initialization
    void Start()
    {
        WeiZhi = GetComponent<RectTransform>();
        handCardControl = GetComponent<HandCardControl>();
        handCardControl.HandCardData.AddCardsEvent += AddOnPlayCard;
        handCardControl.HandCardData.RemoveCardsEvent += RemoveOnPlayCard;
    }


    void Update()
    {


    }
    public void AddOnPlayCard(HandCardData handCardData, List<CardData> changeCards)
    {

        // int num = handCardData.CardDatas.Count;
        //for (int i =0; i < num; i++)
        //{
        g = (GameObject)GameObject.Instantiate(image, b);//生成牌
       // 牌的位置
        g.transform.localPosition = Vector2.zero + Vector2.right * handCardData.CardDatas.Count * GlobalSetting.Instance.cardDiangNao;
        game.Add(g);//加入列表
        //}
        //Debug.Log("num:" + num);
        text.text = handCardData.CardDatas.Count.ToString();//显示牌的数量

        Debug.Log("game:" + game.Count.ToString());
        currentCardCount = handCardData.CardDatas.Count;//当前牌的数量

    }
    public void RemoveOnPlayCard(HandCardData handCardData, List<CardData> changeCards)
    {
        //    int num = handCardData.CardDatas.Count;
        //    for (int i = 1; i < num; i++)
        //    {
        //        g = (GameObject)GameObject.Instantiate(image, b);

        //        //  GameObject g = Instantiate(image, b.position, transform.rotation) as GameObject;
        //        // g.transform.SetParent(b);
        //        g.transform.localPosition = Vector2.zero + Vector2.right * i * GlobalSetting.Instance.cardDiangNao;
        //        game.Remove(g);
        //    }
        //    Debug.Log("num:" + num);
        //    text.text = num.ToString();
        int num = handCardData.CardDatas.Count;//现在牌数量
        int a = currentCardCount - num;//跟上次牌数量的差值


        for (int i = 0; i < a; i++)//遍历数值
        {
         
            Destroy(game[handCardData.CardDatas.Count + i]);//删除牌
        }
       
        text.text = handCardData.CardDatas.Count.ToString();//当前牌的数量

        currentCardCount = handCardData.CardDatas.Count;//重置当前牌的数量
    }
}

