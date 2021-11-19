using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class HandCard : MonoBehaviour {
    public bool b = true;
    public bool ste =true;
   
    // 开始点下鼠标选择的卡片索引
    public int firstIndex;
    // 拖动经过的卡片索引
    public int secondIndex;

    public bool isDrag = false;
    public List<ChuPie> cards = new List<ChuPie>();

    private static HandCard _instance;
    public static HandCard Instance
    {
        get {
            if (_instance != null)
                return _instance;
            else return null;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
   


    public void ChangeList(int begin, int end)
    {
      
      
        if(begin != end)
        {
            for(int i = begin-1; i < end; i++)
            {
                
                Debug.Log("3:" + cards.Count);
                cards[i].Change();
               // cards[i].Toun ();
            }
            for (int j = begin-1; j >=end-1; j--)
            {

                cards[j].Change();
                //cards[j].Toun();
            }
         
        }
        else
        {
            Debug.Log("4:" + cards.Count);
            cards[begin - 1].Change();
           // cards[begin -1].Toun();

        } 
            
    }

    //public void SetSecondIndex(int index)
    //{
    //    secondIndex = index;

    //    foreach (ChuPie c in cards)
    //    {
    //        c.SetImage(false);
    //    }

    //  //  确保firstIndex小于或等于SecondIndex
    //    if (secondIndex < firstIndex)
    //    {
    //        for (int i = secondIndex; i <= firstIndex; i++)
    //        {
    //            cards[i].SetImage(true);
    //        }
    //    }
    //    else
    //    {
    //        for (int i = firstIndex; i <= secondIndex; i++)
    //        {
    //            cards[i].SetImage(true);
    //        }
    //    }
    //}
    //public void EndSelect()
    //{
    //    foreach (ChuPie c in cards)
    //    {
    //        c.SetImage(false);
    //    }

    //    firstIndex = secondIndex = 0;
    //}
}
