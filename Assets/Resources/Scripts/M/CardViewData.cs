using UnityEngine;
using System.Collections;
using System;

public class CardViewData : IComparable
{
    public CardData carddata;
    public bool isSelect = false;
    Vector3 selectMoveDir = new Vector3();
    public int CompareTo(object obj)
    {
        carddata.CompareTo(obj);
        return 0;
    }
}
