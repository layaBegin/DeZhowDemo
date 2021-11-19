using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public delegate void VoidIntDelegate(int i);
public delegate void VoidArrayDelegate(CardData[] cardDatas, PlayCardData data);
public delegate void VoidOneListDelegate(List<RankInfo> rankList);

public class GameControl : MonoBehaviour
{
    #region 单例

    private static GameControl _instance;
    public static GameControl Instance
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

        typeChecker = new TypeChecker();
        rankList = new List<RankInfo>();
        exchangeCardList = new List<CardData>();
    }
    #endregion

    public event VoidIntDelegate currentPlayerChanged;
    public event VoidArrayDelegate playCardEvent;
    public event VoidOneListDelegate rankInfoEvent;

    public Text tempExchangeText;
    public Text rankText;                                   // 玩家排序信息
    public List<HandCardControl> handCardControls;          // 当前所有游戏玩家的手牌控制器,且设玩家 0 为单机玩家的控制器
    public TypeChecker typeChecker;                         // 牌型检测器
    public List<RankInfo> rankList;                         // 排名
    //public bool isDoubleLanlord;                            // 上局是否是 "独地"
    public List<CardData> exchangeCardList;                 // 交换牌列表
    private int currentPlayerIndex;                         // 记录开始的玩家的索引
    public int CurrentPlyerIndex
    {
        get
        {
            return currentPlayerIndex;
        }
        set
        {
            currentPlayerIndex = value;
            if (currentPlayerChanged != null)
            {
                currentPlayerChanged(currentPlayerIndex);
            }
        }
    }

    private PlayCardData currentPlayCardType;               // 当前出牌类型
    public PlayCardData CurrentPlayCardType
    {
        get
        {
            return currentPlayCardType;
        }
        //set
        //{
        //    currentPlayCardType = value;
        //}
    }

    private int finishPlayerNum;                            // 已经打完牌的玩家数目
    public int FinishPlayerNum
    {
        get
        {
            return finishPlayerNum;
        }
    }

    public void OnClickBeginBtn()
    {
        StopCoroutine("GameRun");
        Init();
        StartCoroutine("GameRun");
    }

    public Text tempPCDT;

    void Start()
    {
        //playCardEvent += OnTestEvent;
    }

    public void TotalNewGame()
    {
        rankList.Clear();
        StopCoroutine("GameRun");
        Init();
        StartCoroutine("GameRun");
    }

    IEnumerator GameRun()
    {
        //SetFakeRankInfo();

        //if (rankInfoEvent != null)
        //{
        //    rankInfoEvent(rankList);
        //}
        //// 换牌阶段
        //yield return ExchangeCard();

        // 防止死循环参数
        int temp = 0;
        // 出牌阶段
        while (IsFinishGame() == false && temp < 100)
        {
            Debug.Log("GameRuning");
            temp++;



            // 玩家回合
            if (currentPlayerIndex == 0)
            {
                yield return handCardControls[currentPlayerIndex].PlayerRun();
            }
            else
            {
                yield return handCardControls[currentPlayerIndex].BotRun();
            }

            CurrentPlyerIndex = (CurrentPlyerIndex + 1) % handCardControls.Count;
            while (handCardControls[CurrentPlyerIndex].isFinishAllCard == true)
            {
                if (handCardControls[currentPlayerIndex].isFinishAllCard && currentPlayCardType.ID == currentPlayerIndex)
                {
                    PlayCardData newPCD = new PlayCardData();
                    newPCD.ID = -1;
                    newPCD.CardType = PlayCardType.None;
                    currentPlayCardType = newPCD;
                }
                CurrentPlyerIndex = (CurrentPlyerIndex + 1) % handCardControls.Count;
            }
        }

        Debug.Log("游戏结束");

        //// 补充剩余排名信息，剩余牌少的在前
        //ReplenishRankList();

        //if (rankInfoEvent != null)
        //{
        //    rankInfoEvent(rankList);
        //}
    }

    public IEnumerator ExchangeCard()
    {
        // 安全参数
        int temp = 0;
        // 是否需要换牌
        bool needChange = false;
        List<HandCardControl> win = new List<HandCardControl>();
        List<HandCardControl> lose = new List<HandCardControl>();
        // 交换数目
        int exchangeNum;

        if (CalculateOutcome(rankList, ref win, ref lose, out exchangeNum))
        {
            Debug.Log("交换数目为" + exchangeNum + "  败者给牌，点击开始");
            Debug.Log("Lose num = " + lose.Count);
            yield return TempProcessControl();

            // 败者给牌
            foreach (HandCardControl c in lose)
            {
                exchangeCardList.AddRange(c.GiveCard(exchangeNum, MaxOrMin.Max));
            }

            // 败者给出牌后，升降序排序exchangeList，将大的给排名先的玩家。
            exchangeCardList.Sort();
            exchangeCardList.Reverse();

            TempDisplayExchange(exchangeCardList);

            Debug.Log("胜者收牌，点击开始");
            yield return TempProcessControl();

            // 胜者收牌
            foreach (HandCardControl c in win)
            {
                List<CardData> parts = exchangeCardList.GetRange(0, exchangeNum);
                foreach (CardData p in parts)
                {
                    c.AddCardData(p);
                }
                exchangeCardList.RemoveRange(0, exchangeNum);
            }

            TempDisplayExchange(exchangeCardList);
            Debug.Log("胜者还牌阶段，点击开始");
            yield return TempProcessControl();

            exchangeCardList.Clear();
            // 胜者给牌
            foreach (HandCardControl c in win)
            {
                // 当时玩家时
                if (c.id == 0)
                {
                    Debug.Log("玩家还牌");
                    yield return c.PlayerGiveCardRun(exchangeNum);
                }
                else
                {
                    exchangeCardList.AddRange(c.GiveCard(exchangeNum, MaxOrMin.Min));
                }
            }

            // 败者收牌
            // 胜者给出牌后，升降序排序exchangeList，将大的给排名先的玩家。
            exchangeCardList.Sort();
            exchangeCardList.Reverse();

            TempDisplayExchange(exchangeCardList);
            Debug.Log("败者收牌阶段");
            yield return TempProcessControl();

            foreach (HandCardControl c in lose)
            {
                List<CardData> parts = exchangeCardList.GetRange(0, exchangeNum);
                foreach (CardData p in parts)
                {
                    c.AddCardData(p);
                }
                exchangeCardList.RemoveRange(0, exchangeNum);
            }
        }
        else
        {
            Debug.Log("无需交换");
        }

        Debug.Log("交换结束");
        TempDisplayExchange(exchangeCardList);

        // 清空上局名次信息
        exchangeCardList.Clear();

        rankList.Clear();
        yield return TempProcessControl();
    }

    private bool CalculateOutcome(List<RankInfo> rankInfos, ref List<HandCardControl> win, ref List<HandCardControl> lose, out int num)
    {
        bool isSingleLanlord = false;
        int singleIndex = 0;
        num = 0;
        int lanLordNum = 0;

        if (rankList.Count != handCardControls.Count)
        {
            return false;
        }

        for (int i = 0; i < rankInfos.Count; i++)
        {
            if (rankInfos[i].isLandlord == true)
            {
                lanLordNum++;
                singleIndex = i;
            }
        }
        if (lanLordNum == 1)
        {
            isSingleLanlord = true;
        }

        // 非 "独地" 的情况下
        if (isSingleLanlord == false)
        {
            if (rankInfos[0].isLandlord == rankInfos[1].isLandlord)
            {
                win.Add(handCardControls[rankInfos[0].index]);
                win.Add(handCardControls[rankInfos[1].index]);
                lose.Add(handCardControls[rankInfos[2].index]);
                lose.Add(handCardControls[rankInfos[3].index]);
                num = 2;
                return true;
            }

            if (rankInfos[0].isLandlord == rankInfos[2].isLandlord)
            {
                win.Add(handCardControls[rankInfos[0].index]);
                win.Add(handCardControls[rankInfos[2].index]);
                lose.Add(handCardControls[rankInfos[1].index]);
                lose.Add(handCardControls[rankInfos[3].index]);
                num = 1;
                return true;
            }

            if (rankInfos[0].isLandlord == rankInfos[2].isLandlord)
            {
                return false;
            }

            return false;
        }
        else
        {
            switch (singleIndex)
            {
                case 0:
                    win.Add(handCardControls[rankInfos[singleIndex].index]);
                    lose.Add(handCardControls[rankInfos[1].index]);
                    lose.Add(handCardControls[rankInfos[2].index]);
                    lose.Add(handCardControls[rankInfos[3].index]);
                    num = 3;
                    return true;
                case 1:
                    lose.Add(handCardControls[rankInfos[0].index]);
                    win.Add(handCardControls[rankInfos[singleIndex].index]);
                    lose.Add(handCardControls[rankInfos[2].index]);
                    lose.Add(handCardControls[rankInfos[3].index]);
                    num = 2;
                    return true;
                case 2:
                    win.Add(handCardControls[rankInfos[0].index]);
                    win.Add(handCardControls[rankInfos[1].index]);
                    lose.Add(handCardControls[rankInfos[singleIndex].index]);
                    win.Add(handCardControls[rankInfos[3].index]);
                    num = 2;
                    return true;
                case 3:
                    win.Add(handCardControls[rankInfos[0].index]);
                    win.Add(handCardControls[rankInfos[1].index]);
                    win.Add(handCardControls[rankInfos[2].index]);
                    lose.Add(handCardControls[rankInfos[singleIndex].index]);
                    num = 3;
                    return true;
            }

            return false;
        }
    }

    // 补充剩余排名信息，剩余牌少的在前
    private void ReplenishRankList()
    {
        // 当阵容胜负确定
        // 计算剩余玩家的名次
        if (rankList.Count == 3)
        {
            rankList.Add(new RankInfo(currentPlayerIndex, handCardControls[currentPlayerIndex].isLandlord));
        }
        // 剩余一方在3，4名次
        else if (rankList.Count == 2)
        {
            int firstIndex = currentPlayerIndex;
            int secondeIndex = -1;
            for (int i = 1; i < handCardControls.Count; i++)
            {
                secondeIndex = (firstIndex + i) % handCardControls.Count;
                if (handCardControls[secondeIndex].isFinishAllCard == false)
                {
                    break;
                }
            }

            if (handCardControls[firstIndex].HandCardData.CardDatas.Count > handCardControls[secondeIndex].HandCardData.CardDatas.Count)
            {
                rankList.Add(new RankInfo(secondeIndex, handCardControls[secondeIndex].isLandlord));
                rankList.Add(new RankInfo(firstIndex, handCardControls[firstIndex].isLandlord));
            }
            else
            {
                rankList.Add(new RankInfo(firstIndex, handCardControls[firstIndex].isLandlord));
                rankList.Add(new RankInfo(secondeIndex, handCardControls[secondeIndex].isLandlord));
            }
        }
        // 独地情况，即一人拿了包括黑桃A以及黑桃3
        else if (rankList.Count == 1)
        {
            int[] tempAry = new int[3];
            int num = 0;
            for (int i = 0; i < handCardControls.Count; i++)
            {
                if (handCardControls[i].isFinishAllCard == false)
                {
                    tempAry[num] = i;
                    num++;
                }
            }

            // 冒泡，根据剩余牌数排序，少的在前
            for (int i = 0; i < tempAry.Length; i++)
            {
                for (int j = i; j < tempAry.Length - 1; j++)
                {
                    if (handCardControls[j].HandCardData.CardDatas.Count > handCardControls[j + 1].HandCardData.CardDatas.Count)
                    {
                        int tempMid = tempAry[j];
                        tempAry[j] = tempAry[j + 1];
                        tempAry[j + 1] = tempMid;
                    }
                }
            }

            foreach (int i in tempAry)
            {
                rankList.Add(new RankInfo(tempAry[i], handCardControls[tempAry[i]].isLandlord));
            }
        }
    }

    // 指定游戏游戏停止规则
    // 只剩下一方时，结束游戏。
    private bool IsFinishGame()
    {

        bool hasLandlord = false;
        bool hasFarmer = false;

        for (int i = 0; i < handCardControls.Count; i++)
        {
            if (handCardControls[i].isFinishAllCard == false)
            {
                if (handCardControls[i].isLandlord && hasLandlord == false)
                {
                    Debug.Log("存在地主");
                    hasLandlord = true;
                }
                else if (handCardControls[i].isLandlord == false && hasFarmer == false)
                {
                    Debug.Log("存在农民");
                    hasFarmer = true;
                }
            }
        }

        return !(hasFarmer && hasLandlord);
    }

    // 初始化
    public void Init()
    {
        foreach (HandCardControl item in handCardControls)
        {
            item.Init();
        }

        currentPlayerIndex = 0;
        currentPlayCardType = new PlayCardData();
        currentPlayCardType.CardType = PlayCardType.None;
        currentPlayCardType.ID = -1;

        finishPlayerNum = 0;

        CardCreator.Instance.Init();

        //开局发牌
        for (int i = 0; i < handCardControls.Count; i++)
        {
            handCardControls[i].id = i;
            for (int j = 0; j < GlobalSetting.Instance.cardLength; j++)
            {
                CardData newCardData = CardCreator.Instance.GetNewCardData();
                handCardControls[i].AddCardData(newCardData);

                //记录方块4的所有者，方块4先出牌
                if (newCardData.num == CardNum.Four && newCardData.typeColor == ColorKind.fangkuai)
                {
                    currentPlayerIndex = i;
                }

                if ((newCardData.typeColor == ColorKind.heitao) && (newCardData.num == CardNum.A || newCardData.num == CardNum.Three))
                {
                    handCardControls[i].isLandlord = true;
                }
            }

            //数据无需排序，视图才应该排序
            //handCardControls[i].SortCardData();
        }
    }

    public void SetCurrentPlayType(CardData[] cards, PlayCardData playCardData)
    {
        // 只有更新类型不为None才进行替换，但是仍然会用传入的playCardDate进行传播
        if (playCardData.CardType != PlayCardType.None && cards != null)
        {
            //Debug.Log("更换了");
            currentPlayCardType = playCardData;
        }

        // 触发游戏类改变事件，注意，cards为null时，代表 "过" 或 "要不起"。
        if (playCardEvent != null)
        {
            playCardEvent(cards, currentPlayCardType);
        }
    }

    // 登记排名信息
    public void LoginFinish(int index, bool isLandlord)
    {
        RankInfo info = new RankInfo(index, isLandlord);

        // 在不存在的情况下添加
        bool isExist = false;
        for (int i = 0; i < rankList.Count; i++)
        {
            if (index == rankList[i].index)
            {
                isExist = true;
                break;
            }
        }

        if (isExist == false)
        {
            rankList.Add(info);
        }
    }

    //public void OnTestEvent(CardData[] cardDatas, PlayCardData data)
    //{
    //    if (cardDatas != null && currentPlayCardType != null)
    //    {
    //        tempPCDT.text = "牌型：" + currentPlayCardType.CardType.ToString() + "\n";
    //        tempPCDT.text += "ID : " + currentPlayCardType.ID + "\n";
    //        if (currentPlayCardType.CardType != PlayCardType.None)
    //        {
    //            tempPCDT.text += "Start : " + currentPlayCardType.BeginCard.num.ToString() + "\n";
    //            tempPCDT.text += "End : " + currentPlayCardType.EndCard.num.ToString() + "\n";
    //        }
    //    }
    //}

    public void TempDisplayExchange(List<CardData> cards)
    {
        Debug.Log("交换手牌长度：" + cards.Count);
        if (tempExchangeText != null)
        {

            tempExchangeText.text = "";
            foreach (CardData c in cards)
            {
                tempExchangeText.text += c.typeColor + " " + c.num + "\n";
            }
        }
    }

    public IEnumerator TempProcessControl()
    {
        Debug.Log("Waiting MoveDown");
        bool canMoveDown = false;
        while (canMoveDown == false)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                canMoveDown = true;
            }
            yield return 0;
        }
        Debug.Log("MoveDown");
    }

    // 设置临时测试排名数据
    public void SetFakeRankInfo()
    {
        RankInfo fakeRankInfo_0 = new RankInfo(0, true);
        RankInfo fakeRankInfo_1 = new RankInfo(1, true);
        RankInfo fakeRankInfo_2 = new RankInfo(2, false);
        RankInfo fakeRankInfo_3 = new RankInfo(3, false);

        rankList.Clear();
        rankList.Add(fakeRankInfo_0);
        rankList.Add(fakeRankInfo_1);
        rankList.Add(fakeRankInfo_2);
        rankList.Add(fakeRankInfo_3);

        Debug.Log("Fake rankInfoList finish.");
    }
}

public class RankInfo
{
    public int index;
    public bool isLandlord;

    public RankInfo(int index, bool isLandlord)
    {
        this.index = index;
        this.isLandlord = isLandlord;
    }
}

