using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DZGameLogic : MonoBehaviour
{
    private static DZGameLogic instance;
    public static DZGameLogic Instance
    {
        get {
            if (instance == null)
            {
                instance = new DZGameLogic();
            }
            return instance;
        }
    }

    public  int[] CARD_DATA_ARRAY = new int[]{
       0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D,    //方块 A - K
       0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D,    //梅花 A - K
       0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D,    //红桃 A - K
       0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D     //黑桃 A - K
    };

    //数值掩码
    public byte MASK_COLOR = 0xF0;                               // 花色掩码
    public byte MASK_VALUE = 0x0F;                              //数值掩码


    //获取数值
    public int getCardValue(int cardData)
    {
        return cardData & MASK_VALUE;
    }
    //获取花色
    public int getCardColor(int cardData)
    {
        return cardData & MASK_COLOR;
    }


    //逻辑数值
    public int getCardLogicValue(int cardData)
    {
        //扑克属性
        int cardValue = getCardValue(cardData);
        //转换数值 A = 14;
        return (cardValue == 1) ? (cardValue + 13) : cardValue;
    }

    //public void test1()
    //{
    //    int[] b = (int[])CARD_DATA_ARRAY.Clone();
    //    b[0] = 5;
    //    Debug.Log("CARD_DATA_ARRAY[0]:" + CARD_DATA_ARRAY[0]);
    //    Debug.Log("b[0]:" + b[0]);
    //}

    // 洗牌
    public int[] getRandCardList()
    {
        int[] cardDataArr = (int[])CARD_DATA_ARRAY.Clone();


        int maxCount = cardDataArr.Length;
        
        
        int changeNum = 300;// 洗牌次数
        while (changeNum > 0)
        {
            changeNum--;
            int randomIndex_1 = UnityEngine.Random.Range(0, maxCount);
            int randomIndex_2 = UnityEngine.Random.Range(0, maxCount);
            if (randomIndex_1 == randomIndex_2) continue;
            int temp = cardDataArr[randomIndex_1];
            cardDataArr[randomIndex_1] = cardDataArr[randomIndex_2];
            cardDataArr[randomIndex_2] = temp;
        }
        return cardDataArr;
    }
}

