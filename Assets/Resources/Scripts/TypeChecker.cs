using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TypeChecker
{
    List<CardData> cards;
    //打出的牌数量的分类,从大到小
    public List<NumCountData> numCountDatas;
    //打出结果
    string result = "";

    //private PlayCardData type;
    private int tipsTime;   //已经提示的次数
    private List<NumCountData> singles;
    private List<NumCountData> doubles;
    private List<NumCountData> three;
    private List<NumCountData> four;

    private int lastTimeIndex;
    private int secondLastTimeIndex;

    public TypeChecker()
    {
        cards = new List<CardData>();
        numCountDatas = new List<NumCountData>();
        tipsTime = 0;
        lastTimeIndex = -1;
        secondLastTimeIndex = -1;

        singles = new List<NumCountData>();
        doubles = new List<NumCountData>();
        three = new List<NumCountData>();
        four = new List<NumCountData>();
    }

    //public void Sset
    public void SetCards(CardData[] cardDatas)
    {
        cards.Clear();
        numCountDatas.Clear();
        tipsTime = 0;
        lastTimeIndex = -1;
        secondLastTimeIndex = -1;

        foreach (CardData cardData in cardDatas)
        {
            cards.Add(cardData);
        }

        //统计数字出现次数
        for (int i = 0; i < cards.Count; i++)
        {
            bool isExist = false;
            for (int j = 0; j < numCountDatas.Count; j++)
            {
                if ((int)cards[i].num == numCountDatas[j].num)
                {
                    numCountDatas[j].count++;
                    numCountDatas[j].cardDatas.Add(cards[i]);
                    isExist = true;
                    break;
                }
            }

            if (isExist == false)
            {
                numCountDatas.Add(new NumCountData(cards[i], 1));
            }
        }

        numCountDatas.Sort();
    }

    public PlayCardData AnalyseCard(CardData[] cardDatas, int id)
    {
        SetCards(cardDatas);
        PlayCardData result = new PlayCardData();
        result.CardType = PlayCardType.None;
        result.ID = id;
        result.Length = 0;
        switch (cards.Count)
        {
            case 0:
                break;
            case 1:
                result.BeginCard = cardDatas[0];
                result.EndCard = cardDatas[0];
                result.Length = 1;
                result.CardType = PlayCardType.Single;
                break;
            case 2:
                if (cards[0].num == cards[1].num)
                {
                    result.BeginCard = cardDatas[0];
                    result.EndCard = cardDatas[1];
                    result.Length = 1;
                    result.CardType = PlayCardType.Double;
                }
                break;
            case 5:
                // 两种数字，考虑 三带二 或 四带一
                if (numCountDatas.Count == 2)
                {
                    if (numCountDatas[0].count == 3)
                    {
                        result.BeginCard = cardDatas[0];
                        result.EndCard = cardDatas[3];
                        result.Length = 1;
                        result.CardType = PlayCardType.ThreeWithDouble;
                        break;
                    }
                    if (numCountDatas[0].count == 4)
                    {
                        result.BeginCard = cardDatas[0];
                        result.EndCard = cardDatas[4];
                        result.Length = 1;
                        result.CardType = PlayCardType.FourWithOne;
                        break;
                    }
                }
                //判断顺子
                else if (numCountDatas.Count == 5)
                {
                    bool isOk = true;
                    for (int i = 1; i < 5; i++)
                    {
                        if (numCountDatas[i].num != i + numCountDatas[0].num)
                        {
                            isOk = false;
                            break;
                        }
                    }

                    if (isOk == true)
                    {
                        result.BeginCard = cardDatas[0];
                        result.EndCard = cardDatas[4];
                        result.Length = 5;
                        result.CardType = PlayCardType.Straight;
                        break;
                    }
                }
                break;
            default:
                break;
        }

        return result;
    }


    // t = MaxOrMin.max 选择最大的，MaxOrMin.min 选择最小的
    public List<CardData> Tips(PlayCardType type, int id, out PlayCardData playCardData, MaxOrMin t)
    {

        playCardData = new PlayCardData();
        playCardData.ID = id;

        cards.Sort();

        if (t == MaxOrMin.Max)
        {
            cards.Reverse();
        }

        // 筛选
        switch (type)
        {
            case PlayCardType.Single:
                playCardData.CardType = PlayCardType.Single;
                playCardData.Length = 1;

                for (int i = lastTimeIndex; i < cards.Count; i++)
                {
                    if (lastTimeIndex == -1)
                    {
                        List<CardData> result = new List<CardData>();
                        result.Add(cards[0]);
                        lastTimeIndex = 0;

                        playCardData.BeginCard = cards[0];
                        playCardData.EndCard = cards[0];

                        return result;
                    }
                    if (cards[i].num != cards[lastTimeIndex].num)
                    {
                        List<CardData> result = new List<CardData>();
                        result.Add(cards[i]);
                        lastTimeIndex = i;

                        playCardData.BeginCard = cards[i];
                        playCardData.EndCard = cards[i];
                        return result;
                    }
                    if (i + 1 >= cards.Count)
                    {
                        lastTimeIndex = -1;
                    }
                }
                break;
            case PlayCardType.Double:
                playCardData.CardType = PlayCardType.Double;
                playCardData.Length = 1;

                for (int i = lastTimeIndex; i < cards.Count - 1; i++)
                {
                    if (lastTimeIndex == -1)
                    {
                        if (cards[0].num == cards[1].num)
                        {
                            List<CardData> result = new List<CardData>();
                            result.Add(cards[0]);
                            result.Add(cards[1]);
                            lastTimeIndex = 0;

                            playCardData.BeginCard = cards[0];
                            playCardData.EndCard = cards[1];

                            return result;
                        }

                        lastTimeIndex = 0;
                        i = 0;
                    }

                    if (cards[i].num == cards[i + 1].num && cards[i].num != cards[lastTimeIndex].num)
                    {
                        List<CardData> result = new List<CardData>();
                        result.Add(cards[i]);
                        result.Add(cards[i + 1]);
                        lastTimeIndex = i;

                        playCardData.BeginCard = cards[i];
                        playCardData.EndCard = cards[i + 1];

                        return result;
                    }

                    if (i + 1 >= cards.Count - 1)
                    {
                        lastTimeIndex = -1;
                    }
                }
                break;
            case PlayCardType.ThreeWithDouble:
                playCardData.CardType = PlayCardType.ThreeWithDouble;
                playCardData.Length = 1;

                // 寻找三连
                for (int i = lastTimeIndex; i < cards.Count - 2; i++)
                {
                    // 能否凑成三连
                    bool canThree = false;
                    List<CardData> result = new List<CardData>();
                    // 处理-1的情况
                    if (lastTimeIndex == -1)
                    {
                        if (cards[0].num == cards[1].num && cards[0].num == cards[2].num)
                        {
                            result.Add(cards[0]);
                            result.Add(cards[1]);
                            result.Add(cards[2]);
                            lastTimeIndex = 0;
                            canThree = true;

                            playCardData.BeginCard = cards[0];
                        }
                        lastTimeIndex = 0;
                    }
                    else if (cards[i].num == cards[i + 1].num && cards[i].num == cards[i + 2].num && cards[i].num != cards[lastTimeIndex].num)
                    {
                        result.Add(cards[i]);
                        result.Add(cards[i + 1]);
                        result.Add(cards[i + 2]);
                        canThree = true;

                        playCardData.BeginCard = cards[i];
                    }


                    if (canThree)
                    {
                        // 寻找对子
                        for (int j = secondLastTimeIndex; j < cards.Count - 1; j++)
                        {
                            // 去除重复
                            if (secondLastTimeIndex == -1)
                            {
                                if (cards[0].num != cards[i].num && cards[0].num == cards[1].num)
                                {
                                    result.Add(cards[0]);
                                    result.Add(cards[1]);
                                    secondLastTimeIndex = 0;

                                    playCardData.EndCard = cards[0];
                                    return result;
                                }

                                secondLastTimeIndex = 0;
                                j = 0;
                            }

                            if (cards[j].num == cards[j + 1].num && cards[j].num != cards[i].num && cards[j].num != cards[secondLastTimeIndex].num)
                            {

                                result.Add(cards[j]);
                                result.Add(cards[j + 1]);
                                secondLastTimeIndex = j;

                                playCardData.BeginCard = cards[j];
                                return result;
                            }

                            if (j + 1 >= cards.Count - 1)
                            {
                                secondLastTimeIndex = -1;
                                lastTimeIndex += 1;
                                if (lastTimeIndex >= cards.Count - 2)
                                {
                                    lastTimeIndex = -1;
                                }
                            }
                        }
                    }
                }
                break;
            case PlayCardType.FourWithOne:
                playCardData.CardType = PlayCardType.FourWithOne;
                playCardData.Length = 1;

                // 寻找四连
                for (int i = lastTimeIndex; i < cards.Count - 3; i++)
                {
                    // 能否凑成四连
                    bool canFour = false;
                    List<CardData> result = new List<CardData>();
                    // 处理-1的情况
                    if (lastTimeIndex == -1)
                    {
                        if (cards[0].num == cards[1].num && cards[0].num == cards[2].num && cards[0] == cards[3])
                        {
                            result.Add(cards[0]);
                            result.Add(cards[1]);
                            result.Add(cards[2]);
                            result.Add(cards[3]);
                            lastTimeIndex = 0;
                            canFour = true;

                            playCardData.BeginCard = cards[0];
                        }
                        lastTimeIndex = 0;
                    }
                    else if (cards[i].num == cards[i + 1].num && cards[i].num == cards[i + 2].num && cards[i].num == cards[i + 3].num && cards[i].num != cards[lastTimeIndex].num)
                    {
                        result.Add(cards[i]);
                        result.Add(cards[i + 1]);
                        result.Add(cards[i + 2]);
                        result.Add(cards[i + 3]);
                        canFour = true;

                        playCardData.BeginCard = cards[i];
                    }

                    if (canFour)
                    {
                        // 寻找单
                        for (int j = secondLastTimeIndex; j < cards.Count; j++)
                        {
                            // 去除重复
                            if (secondLastTimeIndex == -1)
                            {
                                if (cards[0].num != cards[i].num)
                                {
                                    result.Add(cards[0]);
                                    secondLastTimeIndex = 0;

                                    playCardData.EndCard = cards[0];
                                    return result;
                                }

                                secondLastTimeIndex = 0;
                                j = 0;
                            }

                            if (cards[j].num != cards[i].num && cards[j].num != cards[secondLastTimeIndex].num)
                            {
                                result.Add(cards[j]);
                                secondLastTimeIndex = j;

                                playCardData.EndCard = cards[j];
                                return result;
                            }

                            if (j + 1 >= cards.Count)
                            {
                                secondLastTimeIndex = -1;
                                lastTimeIndex += 1;
                                if (lastTimeIndex >= cards.Count - 3)
                                {
                                    lastTimeIndex = -1;
                                }
                            }
                        }
                    }
                }
                break;
            case PlayCardType.Straight:
                playCardData.CardType = PlayCardType.Straight;
                playCardData.Length = 5;

                for (int i = lastTimeIndex; i < cards.Count - 4; i++)
                {
                    List<CardData> result = new List<CardData>();
                    CardNum frontNum;
                    if (lastTimeIndex == -1)
                    {
                        frontNum = cards[0].num;
                        result.Add(cards[0]);

                        playCardData.BeginCard = cards[0];

                        for (int j = 1; j < cards.Count - 4; j++)
                        {
                            if (cards[j].num == frontNum)
                            {
                                continue;
                            }
                            else if (frontNum + 1 == cards[j].num)
                            {
                                result.Add(cards[j]);
                                frontNum += 1;
                            }

                            if (result.Count == 5)
                            {
                                lastTimeIndex = 0;

                                playCardData.EndCard = cards[j];

                                return result;
                            }
                        }

                        lastTimeIndex = 0;
                        i = 0;
                    }

                    if (cards[i].num != cards[lastTimeIndex].num)
                    {
                        frontNum = cards[i].num;
                        result.Add(cards[i]);

                        playCardData.BeginCard = cards[i];

                        for (int j = i + 1; j < cards.Count - 3; j++)
                        {
                            if (frontNum + 1 == cards[j].num)
                            {
                                result.Add(cards[j]);
                                frontNum += 1;
                            }
                            else if (frontNum == cards[j].num)
                            {
                                continue;
                            }

                            if (result.Count == 5)
                            {
                                lastTimeIndex = i;

                                playCardData.EndCard = cards[j];

                                return result;
                            }
                        }
                    }

                    if (i + 1 >= cards.Count - 4 || cards[i].num >= CardNum.K)
                    {
                        lastTimeIndex = -1;
                    }
                }
                break;
        }

        playCardData.CardType = PlayCardType.None;
        playCardData.Length = 0;
        return null;
    }

    public void ResetTipsTime()
    {
        tipsTime = 0;
    }
}


public class NumCountData : IComparable
{
    public int num;
    public int count;
    public List<CardData> cardDatas;

    public NumCountData(CardData cardData, int count)
    {
        this.num = (int)cardData.num;
        this.count = count;
        cardDatas = new List<CardData>();
        cardDatas.Add(cardData);
    }

    //1.数量多排前
    //2.数字小排前
    public int CompareTo(object other)
    {
        if (other is NumCountData)
        {
            if (this.count > ((NumCountData)other).count)
            {
                return -1;
            }
            if (this.count == ((NumCountData)other).count && this.num < ((NumCountData)other).num)
            {
                return -1;
            }
        }

        return 1;
    }
}
