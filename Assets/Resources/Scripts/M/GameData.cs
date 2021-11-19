using UnityEngine;
using System.Collections;


public class GameData
{
    private GameData _instance;
    public GameData GetInstance
    {
        get
        {
            if (_instance == null)
                _instance = new GameData();
            return _instance;
        }
    }
    private GameData() { }
    public int timePreRound = 30;//每回合的时间
    public int index = 0;//当前出牌的玩家；

    //public CardOutKind cardstyle;
    public HandCardData handcards = new HandCardData();
    private CardData[] cards = new CardData[52];
    //public CardData CreatCard()
    //{
    //    for (int i = 0; i < cards.Length; i++)//首先为cards数组里面的元素CardBase开辟空间
    //    {
    //        cards[i] = new CardData();
    //    }
    //    int k = 0;
    //    for (int i = 4; i < 17; i++)//牌从3开始，15结束
    //    {
    //        for (int j = 0; j < 4; j++)
    //        {
    //            cards[k].num = (CardNum)i;
    //            cards[k].typeColor = (ColorKind)j;
    //            k++;
    //        }
    //    }
    //    return cards[cards.Length-1];
    //}
}
