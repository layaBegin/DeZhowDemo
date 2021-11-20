using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum cardType
{
    SINGLE = 1, //High Card
    ONE_DOUBLE = 2, //One Pair
    TWO_DOUBLE = 3, //Two Pairs
    THREE = 4, //Three of a Kind
    SHUN_ZI = 5, //Straight
    HU_LU = 6, //Full House
    TONG_HUA = 7, //Flush
    TIE_ZHI = 8, //Four of a Kind
    TONG_HUA_SHUN = 9, //Straight Flush
    KING_TONG_HUA_SHUN = 10 //Royal Flush
};

public class DZGameLogic
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
    //获取花色 得到的数值为 16 的整数倍，如view层要 使用 需 除以 16
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

    public void test1()
    {
        List<int> arr = sortCardList(new int[] { 8, 3, 5 });
       
        foreach (var item in arr)
        {
            Debug.Log("item:"+item);
        }
        Debug.Log("List arr:" + arr.ToArray().ToString());
    }

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


    //倒序排序扑克
    public List<int> sortCardList(int[] cardDataArr)
    {
        List<int> arr = cardDataArr.ToList();
        arr.Sort((a, b) =>
        {
            return b - a;
        });
        return arr;
        
    }


    //7选5排列组合 m中选 出n个
    /**
    * 获得从m中取n的所有组合
     */
    public List<List<int>> getCombinationFlagArrs(int m, int n)
    {
        if (n < 1 || m < n || m == n)
        {
            return null;
        }

        List<List<int>> resultArrs = new List<List<int>>();
        List<int> flagArr = new List<int>();
        bool isEnd = false;
        int i, j, leftCnt;

        for (i = 0; i < m; i++)
        {
            int num = i < n ? 1 : 0;
            flagArr.Add(num);
        }
        resultArrs.Add(new List<int>(flagArr));
        while (!isEnd)
        {
            leftCnt = 0;
            for (i = 0; i < m - 1; i++)
            {
                if (flagArr[i] == 1 && flagArr[i + 1] == 0)
                {
                    for (j = 0; j < i; j++)
                    {
                        flagArr[j] = j < leftCnt ? 1 : 0;
                    }
                    flagArr[i] = 0;
                    flagArr[i + 1] = 1;
                    List<int> aTmp = new List<int>(flagArr);
                    resultArrs.Add(aTmp);
                    List<int> rangeTmp = aTmp.GetRange(m-n,n);
                    string s = "";
                    for (int k = 0;k < rangeTmp.Count;k++)
                    {
                        s += rangeTmp[k];
                    }
                    Debug.Log("rangeTmp.ToString():" + s);
                    int index = s.IndexOf("0");
                    if (index == -1)
                    {
                        isEnd = true;
                    }
                    break;
                }

                if(flagArr[i] == 1) leftCnt++;
            }
        }
        return resultArrs;
    }


    //分析扑克
    public Dictionary<string,object> analyseCardData(int[] cardDataArr)
    {
        object a = new object();
        Dictionary<string, object> result = new Dictionary<string, object>();
       
        int singleCount = 0;
        int doubleCount = 0;
        int threeCount = 0;
        int fourCount = 0;

        List<int> singleLogicValue = new List<int>();
        List<int> doubleLogicValue = new List<int>();
        List<int> threeLogicValue = new List<int>();
        List<int> fourLogicValue = new List<int>();
       
        //扑克分析
        for (int i = 0; i< cardDataArr.Length; i++){
            //变量定义
            int sameCount = 1;
            int[] sameCardData = new int[] { cardDataArr[i], 0, 0, 0 };
            int logicValue = this.getCardLogicValue(cardDataArr[i]);
            //获取同牌
            for (int j = i+1; j<cardDataArr.Length; j++){
                //逻辑对比
                if (this.getCardLogicValue(cardDataArr[j])!= logicValue) break;
                //设置扑克
                sameCardData[sameCount++]=cardDataArr[j];
            }
            //保存结果
            switch (sameCount) {
                case 1:{
                    singleCount++;
                    singleLogicValue.Add(this.getCardLogicValue(sameCardData[0]));
                    break;
                }
                case 2: {
                    doubleCount++;
                    doubleLogicValue.Add(this.getCardLogicValue(sameCardData[0]));
                    break;
                }
                case 3: {
                    threeCount++;
                    threeLogicValue.Add(this.getCardLogicValue(sameCardData[0]));
                    break;
                }
                case 4: {
                    fourCount++;
                    fourLogicValue.Add(this.getCardLogicValue(sameCardData[0]));
                    break;
                }
            }
            //设置递增
            i+=(sameCount-1);
        }
        result.Add("singleCount", singleCount);
        result.Add("singleLogicValue", singleLogicValue);
        result.Add("doubleCount", doubleCount);
        result.Add("doubleLogicValue", doubleLogicValue);
        result.Add("threeCount", threeCount);
        result.Add("threeLogicValue", threeLogicValue);
        result.Add("fourCount", fourCount);
        result.Add("fourLogicValue", fourLogicValue);
        return result;
    }


    //获取牌型
    public cardType getCardType(int[] cardDataArr)
    {
        bool isSameColor = true;
        bool isLineCard = true;
        int firstColor = this.getCardColor(cardDataArr[0]);
        int firstValue = this.getCardLogicValue(cardDataArr[0]);

        //牌形分析
        for (int i = 1; i < cardDataArr.Length; i++)
        {
            //数据分析
            if (isSameColor && (this.getCardColor(cardDataArr[i]) != firstColor)) isSameColor = false;
            if (isLineCard && (firstValue != (this.getCardLogicValue(cardDataArr[i]) + i))) isLineCard = false;
            //结束判断
            if (!isSameColor && !isLineCard) break;
        }

        //最小同花顺 [14,5,4,3,2] =12345 
        if (!isLineCard && (firstValue == 14))
        {
            for (int i = 1; i < cardDataArr.Length; i++)
            {
                int logicValue = this.getCardLogicValue(cardDataArr[i]);
                if ((firstValue != (logicValue + i + 8))) break;
                if (i == cardDataArr.Length - 1) isLineCard = true;
            }
        }

        //皇家同花顺
        if (isSameColor && isLineCard && (this.getCardLogicValue(cardDataArr[1]) === 13)) return gameProto.cardType.KING_TONG_HUA_SHUN;
        //顺子类型
        if (!isSameColor && isLineCard) return cardType.SHUN_ZI;
        //同花类型
        if (isSameColor && !isLineCard) return cardType.TONG_HUA;
        //同花顺类型
        if (isSameColor && isLineCard) return cardType.TONG_HUA_SHUN;
        //扑克分析
        Dictionary<string,object> analyseResult = this.analyseCardData(cardDataArr);

        //类型判断
        if ((int)analyseResult["fourCount"] == 1) return cardType.TIE_ZHI;
        if ((int)analyseResult["doubleCount"] == 2) return cardType.TWO_DOUBLE;
        if (((int)analyseResult["doubleCount"] == 1) && ((int)analyseResult["threeCount"] == 1)) return cardType.HU_LU;
        if (((int)analyseResult["threeCount"] == 1) && ((int)analyseResult["doubleCount"] == 0)) return cardType.THREE;
        if (((int)analyseResult["doubleCount"] == 1) && ((int)analyseResult["singleCount"] == 3)) return cardType.ONE_DOUBLE;
        return cardType.SINGLE;
    }

}

