using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

// 选择大的，还是小的
public enum MaxOrMin
{
    Max,
    Min
}

//手牌控制类
public class HandCardControl : MonoBehaviour
{

    public int id;                          // 本控制器的id
    public bool isLandlord;                  // 是否是地主 
    private HandCardData handCardData;      // 用于绑定手牌数据
    public Text tempText;
    private TypeChecker typeChecker;        // 类型检查器
    private int exchangeCardNum;            // 交换数目

    private bool isFinishThisRound;         // 本回合是否完成
    public bool isFinishAllCard
    {
        get
        {
            return handCardData.CardDatas.Count > 0 ? false : true;
        }
    }

    public HandCardData HandCardData
    {
        get
        {
            return handCardData;
        }
    }

    public PlayCardType tempType;
    public Text tempTypeText;

    void Awake()
    {
        Init();
        //Debug.Log("handcard awake");
    }

    void Update()
    {
        if (tempTypeText != null)
        {
            tempTypeText.text = tempType.ToString();
        }
    }

    public IEnumerator BotRun()
    {
        //Debug.Log("Start Bot round");
        typeChecker.SetCards(handCardData.CardDatas.ToArray());
        for (float i = 0; i < GlobalSetting.Instance.botThinkTime; i += Time.deltaTime)
        {
            yield return 0;
        }

        BotAI();
        if (isFinishAllCard)
        {
            GameControl.Instance.LoginFinish(id, isLandlord);
        }
        //Debug.Log("Finish Bot round");
    }

    // 拿出自己最大的N张牌，黑桃3、黑桃A、方块4除外
    public List<CardData> GiveCard(int num, MaxOrMin t)
    {
        typeChecker.SetCards(handCardData.CardDatas.ToArray());
        // 安全参数
        int temp = 0;
        List<CardData> giveCard = new List<CardData>();
        PlayCardData newPCD = new PlayCardData();
        while (giveCard.Count < num && temp < 10)
        {
            temp++;

            List<CardData> result = typeChecker.Tips(PlayCardType.Single, id, out newPCD, t);
            if (result != null)
            {
                if (isGiveLegal(result[0]))
                {
                    giveCard.Add(result[0]);
                }
            }
        }

        // 移除给出的牌
        handCardData.RemoveCard(giveCard);
        return giveCard;
    }

    // 检测牌是否符合标准(即不能时黑桃3、黑桃A、方块4),是的话返回false
    private bool isGiveLegal(CardData cardData)
    {
        if ((cardData.num == CardNum.Three && cardData.typeColor == ColorKind.heitao)
            || (cardData.num == CardNum.A && cardData.typeColor == ColorKind.heitao)
            || (cardData.num == CardNum.Four && cardData.typeColor == ColorKind.fangkuai))
        {
            return false;
        }

        return true;
    }

    // 电脑玩家的出牌AI
    public void BotAI()
    {
        //Debug.Log("BotAI");
        PlayCardData tempPCD = GameControl.Instance.CurrentPlayCardType;
        PlayCardData newPCD;
        if (tempPCD.CardType == PlayCardType.None || tempPCD.ID == this.id || tempPCD.ID == -1)
        {
            // 出牌权在自己时，出单牌，并且从小到大出。
            List<CardData> temp = typeChecker.Tips(PlayCardType.Single, id, out newPCD, MaxOrMin.Min);
            if (temp != null)
            {
                CardData[] cards = temp.ToArray();
                if (newPCD.CardType != PlayCardType.None)
                {
                    handCardData.RemoveCard(cards);
                    GameControl.Instance.SetCurrentPlayType(cards, newPCD);
                }
            }
            else
            {
                GameControl.Instance.SetCurrentPlayType(null, newPCD);
                //Debug.Log("出完了所有牌，不应该再向我要牌");
                return;
            }
        }
        else
        {
            // 安全参数
            int tempCount = 0;
            while (tempCount < 20)
            {
                tempCount++;
                List<CardData> temp = typeChecker.Tips(GameControl.Instance.CurrentPlayCardType.CardType, id, out newPCD, MaxOrMin.Min);
                if (temp != null)
                {
                    CardData[] cards = temp.ToArray();
                    if (newPCD.CardType != PlayCardType.None)
                    {
                        // 能要的起
                        if (newPCD.CompareTo(GameControl.Instance.CurrentPlayCardType) == 1)
                        {
                            // 出牌
                            handCardData.RemoveCard(cards);
                            GameControl.Instance.SetCurrentPlayType(cards, newPCD);
                            return;
                        }
                        continue;
                    }
                }
                else
                {
                    // 要不起
                    GameControl.Instance.SetCurrentPlayType(null, newPCD);
                    //Debug.Log("要不起");
                    return;
                }
            }
        }
    }

    public IEnumerator PlayerGiveCardRun(int num)
    {

        exchangeCardNum = num;
        isFinishThisRound = false;
        while (isFinishThisRound == false)
        {
            Debug.Log("等待玩家给牌");
            yield return 0;
        }

        Debug.Log("玩家给完牌了");
    }

    // 玩家给牌
    public void PlayerGiveCard()
    {
        Debug.Log("Give Card");
        if (handCardData.SelectedCards.Count == exchangeCardNum)
        {
            bool isAllOk = true;
            foreach (CardData c in handCardData.SelectedCards)
            {
                if (isGiveLegal(c) == false)
                {
                    isAllOk = false;
                    break;
                }
            }

            if (isAllOk == true)
            {
                List<CardData> giveCard = new List<CardData>();
                giveCard.AddRange(handCardData.SelectedCards);

                GameControl.Instance.exchangeCardList.AddRange(giveCard);
                this.handCardData.RemoveCard(giveCard.ToArray());
                isFinishThisRound = true;
            }
            else
            {
                Debug.Log("不能包含地主牌！");
            }
        }
        else
        {
            Debug.Log("牌数不符合够 " + exchangeCardNum + "张");
        }
    }

    public IEnumerator PlayerRun()
    {
        //Debug.Log("In player round");
        typeChecker.SetCards(handCardData.CardDatas.ToArray());
        isFinishThisRound = false;
        while (isFinishThisRound == false)
        {
            //Debug.Log("wating input");
            yield return 0;
        }

        if (isFinishAllCard)
        {
            GameControl.Instance.LoginFinish(id, isLandlord);
        }
        //Debug.Log("Finish player round ");
    }

    public void AddCardData(CardData cardData)
    {
        handCardData.AddCard(cardData);
        //Debug.Log("add card in control"); 
    }

    // 不要
    public void Pass()
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        PlayCardData newPCD = new PlayCardData();
        newPCD.ID = id;
        newPCD.CardType = PlayCardType.None;
        GameControl.Instance.SetCurrentPlayType(null, newPCD);
        isFinishThisRound = true;
    }

    public void SetType(PlayCardType type)
    {
        tempType = type;
        typeChecker.ResetTipsTime();
    }

    public void Tips()
    {
        // 先恢复全部已选卡牌
        RecoverAll();
        PlayCardData tempPCD;

        List<CardData> result = typeChecker.Tips(tempType, id, out tempPCD, MaxOrMin.Min);
        // 让推荐的卡牌全部抬起
        if (result != null)
        {
            foreach (CardData c in result)
            {
                c.IsSelected = true;
            }
        }
        else
        {
            // 要不起
            GameControl.Instance.SetCurrentPlayType(null, tempPCD);
            isFinishThisRound = true;
        }
    }

    // 玩家打出已选的牌
    public void PlayCardData()
    {
        if (handCardData.SelectedCards.Count > 0)
        {
            PlayCardData newPCD = GameControl.Instance.typeChecker.AnalyseCard(handCardData.SelectedCards.ToArray(), id);
            // 不为空，且相对较大。
            if (newPCD.CardType != PlayCardType.None && newPCD.CompareTo(GameControl.Instance.CurrentPlayCardType) == 1)
            {
                Debug.Log("MyID :" + id + "beforeID :" + GameControl.Instance.CurrentPlayCardType.ID);
                GameControl.Instance.SetCurrentPlayType(handCardData.SelectedCards.ToArray(), newPCD);
                handCardData.RemoveCard(handCardData.SelectedCards.ToArray());
                isFinishThisRound = true;
            }
            else
            {
                Debug.Log("出牌不合法，或不够大");
            }
        }
    }

    // 初始化
    public void Init()
    {
        typeChecker = new TypeChecker();
        isLandlord = false;
        id = -1;
        if (handCardData == null)
        {
            handCardData = new HandCardData();
        }
        isFinishThisRound = false;
        handCardData.Init();
    }

    // 重选，将已经选择的卡牌放回原位
    public void RecoverAll()
    {
        // 为了安全，设置最大尝试删除数为20
        int temp = 0;
        while (handCardData.SelectedCards.Count > 0 && temp < 20)
        {
            Debug.Log("Recover All");
            handCardData.SelectedCards[0].IsSelected = false;
            temp++;
        }
    }

    public void SortCardData()
    {
        handCardData.SortCard();
    }
}

//abstract class P
//{
//    public abstract void Func(); 
//    public virtual void Func1()
//    {

//    }
//}

//class C : P
//{
//    public override void Func()
//    {
//        throw new NotImplementedException();
//    }

//    public void Fun3()
//    {

//    }
//}

//class CC: C
//{
//    public virtual void Func()
//    {
//        Debug.Log("abc");
//    }
//}


