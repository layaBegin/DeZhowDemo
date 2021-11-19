using UnityEngine;
using System.Collections;
using System;

// 用于记录每回合的牌型
// 包括类型，头牌，尾牌，数量以及出牌人的ID
public class PlayCardData : IComparable
{

    private PlayCardType playCardType;
    public PlayCardType CardType
    {
        get; set;
    }

    private int length;
    public int Length
    {
        get; set;
    }

    private CardData beginCard;
    public CardData BeginCard
    {
        get; set;
    }

    private CardData endCard;
    public CardData EndCard
    {
        get; set;
    }

    private int id;         // 记录打出当前这幅牌型的玩家
    public int ID
    {
        get; set;
    }

    public PlayCardData()
    {

    }

    public PlayCardData(PlayCardType t, int l, CardData bc, CardData ec, int i)
    {
        playCardType = t;
        length = l;
        beginCard = bc;
        endCard = ec;
        id = i;
    }

    public int CompareTo(object obj)
    {
        PlayCardData other = obj as PlayCardData;

        //PlayCardData other = (PlayCardData)obj;

        //Debug.Log("我的id = " + this.ID + "他的id = " + other.ID);

        // 四人斗地主大小判断，优先级如下
        // 0. 在无牌或已经打出的牌时自己的（其他人都不要，自己打出的牌，回到自己），可以任意出牌。
        // 1. 比较牌型，只有相同牌型才能比较,如果先出的单张，也无法用4带1打下。
        //    具体是 1-1，2-2，5-5
        // 2. 单个或者双张，比较开头的卡大小
        //    4带1 > 3带2 > 顺子
        if (other != null)
        {
            // 开局情况或者比较这副牌是自己的（即轮了一圈）,或者对方为空时，这种情况应该没有。。。
            if (other.ID == -1 || this.ID == other.ID || other.CardType == PlayCardType.None)
            {
                //Debug.Log("他没有 + id = " + other.ID + "My id = " + this.ID);
                return 1;
            }

            // 相等类型
            if (this.CardType == other.CardType)
            {
                //Debug.Log("同类型，值比较");

                return this.BeginCard.CompareTo(other.BeginCard);
            }
            else
            {
                // 不同类型
                // 当为空是，任何类型都打不过
                if (this.CardType == PlayCardType.None)
                {
                    //Debug.Log("不同类型，我类型为空，");

                    return -1;
                }
                else if ((this.CardType != PlayCardType.Single && this.CardType != PlayCardType.Double)
                        && (other.CardType == PlayCardType.Single && other.CardType != PlayCardType.Double))
                {
                    //Debug.Log("类型大小比较");

                    return this.CardType < other.CardType ? -1 : 1;
                }
                else
                {
                    //Debug.Log("不明原因");

                    return -1;
                }
            }
        }

        string errorMessage = String.Format("Can't use Type({0}) compareTo Type({1})", this.GetType(), obj.GetType());
        throw new Exception(errorMessage);
    }
}
