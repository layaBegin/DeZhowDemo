using UnityEngine;
using System.Collections;
using System;

public delegate void voidBoolDelate(CardData data, bool changeTo);

// 出牌类型
public enum PlayCardType
{
    None,               // 无
    Single,             // 单张
    Double,             // 一对
    Three,              // 三张
    Four,               // 四张 / 炸弹
    ThreeWithOne,       // 三带一
    FourWithOne,        // 四带一
    Straight,           // 顺子
    ThreeWithDouble,    // 三带一对
    FourWithTwo        // 四带二
}

// 花色类型
public enum ColorKind
{
    fangkuai,
    meihua,
    hongtao,
    heitao
}

// 四人斗地主中，3最大
public enum CardNum
{
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    J,
    Q,
    K,
    A,
    Two,
    Three
}

public class CardData : IComparable
{
    // isSelected状态更改事件
    public event voidBoolDelate SelectedChanged;

    private bool isSelected; //是否别选择
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            bool isNew = false;
            if (isSelected != value)
            {
                isNew = true;
            }

            isSelected = value;
            if (SelectedChanged != null && isNew == true)
            {
                SelectedChanged(this, isSelected);
            }
        }
    }

    public CardNum num;
    public ColorKind typeColor;

    #region 构造
    public CardData(CardNum num, ColorKind typeColor)
    {
        this.num = num;
        this.typeColor = typeColor;
        IsSelected = false;
    }

    public CardData(CardData data)
    {
        this.num = data.num;
        this.typeColor = data.typeColor;
        IsSelected = false;
    }
    #endregion

    public int CompareTo(object obj)
    {
        //Debug.Log("compare");
        CardData othercard = obj as CardData;
        if (othercard != null)
        {
            int numresult = num.CompareTo(othercard.num);
            if (numresult == 0)
            {
                return typeColor.CompareTo(othercard.typeColor);
            }
            return numresult;
        }

        throw new Exception("Try to compare diffent object");
    }
}
