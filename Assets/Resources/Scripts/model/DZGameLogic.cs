using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum CardType
{
    None = 0, //High Card
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
    private void sortCardList(List<int> cardDataArr)
    {
        cardDataArr.Sort((a, b) =>
        {
            int aLogicValue = this.getCardLogicValue(a);
            int bLogicValue = this.getCardLogicValue(b);
            if (aLogicValue != bLogicValue) return bLogicValue - aLogicValue;
            else
            {
                return b - a;
            }
        });
        
    }


    //7选5排列组合 m中选 出n个
    /**
    * 获得从m中取n的所有组合
     */
    private List<List<int>> getCombinationFlagArrs(int m, int n)
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


    //分析扑克，前提是A 必须以14   传入
    private Dictionary<string,object> analyseCardData(List<int> cardDataArr)
    {
        //先倒序排列
        sortCardList(cardDataArr);
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
        for (int i = 0; i< cardDataArr.Count; i++){
            //变量定义
            int sameCount = 1;
            int[] sameCardData = new int[] { cardDataArr[i], 0, 0, 0 };
            int logicValue = this.getCardLogicValue(cardDataArr[i]);
            //获取同牌
            for (int j = i+1; j<cardDataArr.Count; j++){
                //逻辑对比
                if (this.getCardLogicValue(cardDataArr[j])!= logicValue) continue;
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
    public CardType getCardType(List<int> cardDataArr)
    {
        //先倒序排列
        sortCardList(cardDataArr);
        bool isSameColor = true;
        bool isLineCard = true;
        int firstColor = this.getCardColor(cardDataArr[0]);
        int firstValue = this.getCardLogicValue(cardDataArr[0]);

        //牌形分析
        for (int i = 1; i < cardDataArr.Count; i++)
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
            for (int i = 1; i < cardDataArr.Count; i++)
            {
                int logicValue = this.getCardLogicValue(cardDataArr[i]);
                if ((firstValue != (logicValue + i + 8))) break;
                if (i == cardDataArr.Count - 1) isLineCard = true;
            }
        }

        //皇家同花顺
        if (isSameColor && isLineCard && (this.getCardLogicValue(cardDataArr[1]) == 13))
            return CardType.KING_TONG_HUA_SHUN;
        //顺子类型
        if (!isSameColor && isLineCard) return CardType.SHUN_ZI;
        //同花类型
        if (isSameColor && !isLineCard) return CardType.TONG_HUA;
        //同花顺类型
        if (isSameColor && isLineCard) return CardType.TONG_HUA_SHUN;
        //扑克分析
        Dictionary<string,object> analyseResult = this.analyseCardData(cardDataArr);

        //类型判断
        if ((int)analyseResult["fourCount"] == 1) return CardType.TIE_ZHI;
        if ((int)analyseResult["doubleCount"] == 2) return CardType.TWO_DOUBLE;
        if (((int)analyseResult["doubleCount"] == 1) && ((int)analyseResult["threeCount"] == 1)) return CardType.HU_LU;
        if (((int)analyseResult["threeCount"] == 1) && ((int)analyseResult["doubleCount"] == 0)) return CardType.THREE;
        if (((int)analyseResult["doubleCount"] == 1) && ((int)analyseResult["singleCount"] == 3)) return CardType.ONE_DOUBLE;
        return CardType.SINGLE;
    }


    /*对比牌型
     * 返回值 2 ，玩家1获胜；1，玩家2获胜；0，平局
     */
    public int compareCard(List<int> firstDataArr,List<int> nextDataArr)
    {
        if (firstDataArr.Count != nextDataArr.Count)
        {
            Debug.Log("compareCard err: card count err");
            return 0;
        }
        sortCardList(firstDataArr);
        sortCardList(nextDataArr);
        //获取类型
        CardType nextType = this.getCardType(nextDataArr);
        CardType firstType = this.getCardType(firstDataArr);
        //类型判断
        //大
        Debug.Log("firstType:" + firstType);
        Debug.Log("nextType:" + nextType);
        if (firstType > nextType) return 2;
        //小
        if (firstType < nextType) return 1;
        //简单类型
        switch (firstType)
        {
            case CardType.SINGLE:
                {
                    for (int i = 0; i < firstDataArr.Count; i++)
                    {
                        int nextValue = this.getCardLogicValue(nextDataArr[i]);
                        int firstValue = this.getCardLogicValue(firstDataArr[i]);
                        // 大
                        if (firstValue > nextValue) return 2;
                        // 小
                        else if (firstValue < nextValue) return 1;
                        // 平
                        else if (i == firstDataArr.Count - 1) return 0;
                    }
                    break;
                }
            case CardType.ONE_DOUBLE:
            case CardType.TWO_DOUBLE:
            case CardType.THREE:
            case CardType.TIE_ZHI:
            case CardType.HU_LU:
                {
                    //分析扑克
                    Dictionary<string,object> analyseResultNext = this.analyseCardData(nextDataArr);
                    Dictionary<string, object> analyseResultFirst = this.analyseCardData(firstDataArr);

                    int nextsingleCount = (int)analyseResultNext["singleCount"];
                    int nextdoubleCount = (int)analyseResultNext["doubleCount"];
                    int nextthreeCount = (int)analyseResultNext["threeCount"];
                    int nextfourCount = (int)analyseResultNext["fourCount"];
                    List<int> nextsingleLogicValue = (List<int>)analyseResultNext["singleLogicValue"];
                    List<int> nextdoubleLogicValue = (List<int>)analyseResultNext["doubleLogicValue"];
                    List<int> nextthreeLogicValue = (List<int>)analyseResultNext["threeLogicValue"];
                    List<int> nextfourLogicValue = (List<int>)analyseResultNext["fourLogicValue"];

                    int firstsingleCount = (int)analyseResultFirst["singleCount"];
                    int firstdoubleCount = (int)analyseResultFirst["doubleCount"];
                    int firstthreeCount = (int)analyseResultFirst["threeCount"];
                    int firstfourCount = (int)analyseResultFirst["fourCount"];
                    List<int> firstsingleLogicValue = (List<int>)analyseResultFirst["singleLogicValue"];
                    List<int> firstdoubleLogicValue = (List<int>)analyseResultFirst["doubleLogicValue"];
                    List<int> firstthreeLogicValue = (List<int>)analyseResultFirst["threeLogicValue"];
                    List<int> firstfourLogicValue = (List<int>)analyseResultFirst["fourLogicValue"];
                    //四条数值
                    if ((int)analyseResultFirst["fourCount"] > 0)
                    {
                        int nextValue = nextfourLogicValue[0];
                        int firstValue = firstfourLogicValue[0];
                        //比较四条
                        if (firstValue != nextValue) return (firstValue > nextValue) ? 2 : 1;
                        //比较单牌
                        firstValue = firstsingleLogicValue[0];
                        nextValue = nextsingleLogicValue[0];
                        if (firstValue != nextValue) return (firstValue > nextValue) ? 2 : 1;
                        else return 0;
                    }
                    //三条数值
                    if ((int)analyseResultFirst["threeCount"] > 0)
                    {
                        int nextValue = nextthreeLogicValue[0] ;
                        int firstValue = firstthreeLogicValue[0];
                        //比较三条
                        if (firstValue != nextValue) return (firstValue > nextValue) ? 2 : 1;
                        //葫芦牌型
                        if (CardType.HU_LU == firstType)
                        {
                            //比较对牌
                            firstValue = firstdoubleLogicValue[0];
                            nextValue = nextdoubleLogicValue[0] ;
                            if (firstValue != nextValue) return (firstValue > nextValue) ? 2 : 1;
                            else return 0;
                        }
                        else
                        {
                            //散牌数值
                            for (int i = 0; i < firstsingleLogicValue.Count; i++)
                            {
                                int nextV = nextsingleLogicValue[i];
                                int firstV = firstsingleLogicValue[i];
                                //大
                                if (firstV > nextV) return 2;
                                //小
                                else if (firstValue < nextValue) return 1;
                                //等
                                else if (i == (firstsingleCount - 1)) return 0;
                            }
                        }
                    }

                    //对子数值
                    for (int i = 0; i < firstdoubleCount; i++)
                    {
                        int nextValue = nextdoubleLogicValue[i];
                        int firstValue = firstdoubleLogicValue[i];
                        //大
                        if (firstValue > nextValue) return 2;
                        //小
                        else if (firstValue < nextValue) return 1;
                    }

                    //比较单牌
                    {
                        //散牌数值
                        for (int i = 0; i < firstsingleCount; i++)
                        {
                            int nextValue = nextsingleLogicValue[i];
                            int firstValue = firstsingleLogicValue[i];
                            // 大
                            if (firstValue > nextValue) return 2;
                            // 小
                            else if (firstValue < nextValue) return 1;
                            // 等
                            else if (i == firstsingleCount - 1) return 0;
                        }
                    }
                    break;
                }
            case CardType.SHUN_ZI:
            case CardType.TONG_HUA_SHUN:
                {
                    // 数值判断
                    int nextValue = this.getCardLogicValue(nextDataArr[0]);
                    int firstValue = this.getCardLogicValue(firstDataArr[0]);

                    bool isFirstmin = (firstValue == (this.getCardLogicValue(firstDataArr[1]) + 9));
                    bool isNextmin = (nextValue == (this.getCardLogicValue(nextDataArr[1]) + 9));

                    //大小顺子
                    if (isFirstmin && !isNextmin) return 1;
                    //大小顺子
                    else if (!isFirstmin && isNextmin) return 2;
                    //等同顺子
                    else
                    {
                        //平
                        if (firstValue == nextValue) return 0;
                        return (firstValue > nextValue) ? 2 : 1;
                    }
                    break;
                }
            case CardType.TONG_HUA:
                {
                    //散牌数值
                    for (int i = 0; i < firstDataArr.Count; i++)
                    {
                        int nextValue = this.getCardLogicValue(nextDataArr[i]);
                        int firstValue = this.getCardLogicValue(firstDataArr[i]);
                        // 大
                        if (firstValue > nextValue) return 2;
                        // 小
                        else if (firstValue < nextValue) return 1;
                        // 平
                        else if (i == firstDataArr.Count - 1) return 0;
                    }
                    break;
                }
        }
        return 0;
    }

    //最大牌型
    public List<int> fiveFromSeven(List<int> handCardDataArr, List<int> centerCardDataArr)
    {
        List<int> tempCardDataArr = handCardDataArr.Concat(centerCardDataArr).ToList();
        if (tempCardDataArr.Count < 5)
        {
            return null;
        }
        //排列扑克
        this.sortCardList(tempCardDataArr);

        // 获取组合index
        List<List<int>> combinationFlagArrs = this.getCombinationFlagArrs(tempCardDataArr.Count, 5);
        // 检索最大组合
        CardType maxCardType = CardType.None;
        List<int> maxCardDataArr = new List<int>();
        for (int i = 0; i < combinationFlagArrs.Count; ++i)
        {
            List<int> arr = combinationFlagArrs[i];
            List<int> cardData = new List<int>();
            for (int j = 0; j < arr.Count; ++j)
            {
                if (arr[j] <= 0) continue;
                cardData.Add(tempCardDataArr[j]);
            }
            CardType cardT = this.getCardType(cardData);
            if (maxCardType < cardT)
            {
                maxCardType = cardT;
                maxCardDataArr = cardData;
            }
            else if (maxCardType == cardT)
            {
                if (this.compareCard(cardData, maxCardDataArr) == 2)
                {
                    maxCardType = cardT;
                    maxCardDataArr = cardData;
                }
            }
        }
        return maxCardDataArr;
    }
}

