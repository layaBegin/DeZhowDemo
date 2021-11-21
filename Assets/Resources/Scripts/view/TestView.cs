using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestView : MonoBehaviour {

    public CardItem[] cardItems1;
    public CardItem[] cardItems2;
    public CardItem[] cardItemsCommon;

    public Sprite[] nums;
    public Sprite[] colors;

    public Button btnStart;
    public Button btnAgain;

    public Text text1Type;
    public Text text2Type;
    public Text textWhoWin;

    private Color colorRed = new Color(255,0,0);
    private Color colorBlack = new Color(0,0,0);

    void Awake()
    {
        this._setCardActice(false);
    }
    // Use this for initialization
    void Start() {
       
        Debug.Log("==比较牌型：");

        this.btnAgain.onClick.AddListener(onBtnAgain);
    }


   

    void onBtnAgain()
    {
        Debug.Log("===进入Btn回调");
        this._setCardActice(false);
        Invoke("DelayFunc", 1);
    }

    void _setCardActice(bool active)
    {
        this.cardItems1[0].transform.parent.gameObject.SetActive(active);
        this.cardItems2[0].transform.parent.gameObject.SetActive(active);
        this.cardItemsCommon[0].transform.parent.gameObject.SetActive(active);
    }

    void DelayFunc()
    {
        int[] cardList = DZGameLogic.Instance.getRandCardList();

        int[] cardDataCommon = new int[] { cardList[0], cardList[1], cardList[2], cardList[3], cardList[4] };
        int[] cardData1 = new int[] { cardList[5], cardList[6] };
        int[] cardData2 = new int[] { cardList[7], cardList[8] };

        updateCardItem(cardItems1[0], cardData1[0]);
        //updateCardItem(cardItems1[1], cardData1[1]);
        //updateCardItem(cardItems2[0], cardData2[0]);
        //updateCardItem(cardItems2[1], cardData2[1]);
        //for (int i = 0;i < cardItemsCommon.Length;i++)
        //{
        //    updateCardItem(cardItemsCommon[i],cardDataCommon[i]);
        //}

        this._setCardActice(true);

        //牌型，谁赢了 
        List<int> fiveCardData1= DZGameLogic.Instance.fiveFromSeven(new List<int>(cardData1), new List<int>(cardDataCommon));
        List<int> fiveCardData2 = DZGameLogic.Instance.fiveFromSeven(new List<int>(cardData2), new List<int>(cardDataCommon));

        CardType cardType1 = DZGameLogic.Instance.getCardType(fiveCardData1);
        CardType cardType2 = DZGameLogic.Instance.getCardType(fiveCardData2);
        text1Type.text = cardType1.ToString();
        text2Type.text = cardType2.ToString();
        int winNum = DZGameLogic.Instance.compareCard(fiveCardData1, fiveCardData2);

        if (winNum == 2)
        {
            textWhoWin.text = "PLAYER 1 WIN";
        }else if (winNum == 1)
        {
            textWhoWin.text = "PLAYER 2 WIN";

        }else
        {
            textWhoWin.text = "Draw";
        }
    }

    void updateCardItem(CardItem cardItem,int data)
    {
        cardItem.CardNum = data;
        Debug.Log("===nums[data % 16 - 1]:"+ nums[data % 16 - 1]);
        Sprite numSprire = nums[data % 16 - 1];
        int colorNum = DZGameLogic.Instance.getCardColor(data) / 16;
        Sprite colorSprire = colors[colorNum];
        Color color = (colorNum % 2 == 0 ? colorBlack : colorRed);
        cardItem.UpdateItem(numSprire, colorSprire, color);
    }
    

}
