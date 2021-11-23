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

    public Transform pos1;
    public Transform pos2;
    public Transform posCommon;

    private Color colorRed = new Color(255,0,0);
    private Color colorBlack = new Color(0,0,0);


    int[] cardData1;
    int[] cardData2;
    int[] cardDataCommon;

    private List<string> cardTypeText = new List<string>(){"None",
        "High Card","One Pair","Two Pairs",
        "Three of a Kind","Straight","Full House",
        "Flush","Four of a Kind","Straight Flush",
        "Royal Flush"};

    void Awake()
    {
        //this._setCardActice(false);
    }
    // Use this for initialization
    void Start() {
       
        this.btnAgain.onClick.AddListener(onBtnAgain);
    }

    private string getTypeText(int num){
        string str = "";
        switch(num){
            case 1:
                str = "High Card";
                break;
            case 2:
                str = "One Pair";
                break;
            case 3:
                str = "High Card";
                break;
            case 4:
                str = "High Card";
                break;
            case 5:
                str = "High Card";
                break;
            case 6:
                str = "High Card";
                break;
            case 7:
                str = "High Card";
                break;
            case 8:
                str = "High Card";
                break;
            case 9:
                str = "High Card";
                break;
            case 10:
                str = "High Card";
                break;
            default:
                break;
        }
        return str;

    }
   

    void onBtnAgain()
    {
        Debug.Log("===进入Btn回调");
        destroyCards();

        //this._setCardActice(true);
        DelayFunc();
        //Invoke("DelayFunc", 1);
        //StartCoroutine(ShowA());
    }


    void destroyCards()
    {
        for (int i= 0;i < pos1.childCount;i++)
        {
            GameObject chid = pos1.GetChild(i).gameObject;
            Destroy(chid);
        }
        for (int i = 0; i < pos2.childCount; i++)
        {
            
            Destroy(pos2.GetChild(i).gameObject);
        }
        for (int i = 0; i < posCommon.childCount; i++)
        {
           
            Destroy(posCommon.GetChild(i).gameObject);
        }

    }

    private IEnumerator ShowA()
    {
        yield return null;
        yield return new WaitForSeconds(0.5f);
        showCards();
    }


    void _setCardActice(bool active)
    {
        cardItems1[0].transform.gameObject.SetActive(active);
        cardItems1[1].transform.gameObject.SetActive(active);
        cardItems2[0].transform.gameObject.SetActive(active);
        cardItems2[1].transform.gameObject.SetActive(active);
        cardItems1[0].transform.gameObject.SetActive(active);
        for (int i = 0; i < cardItemsCommon.Length; i++)
        {
            cardItemsCommon[i].transform.gameObject.SetActive(active);
        }
    }



    

    void DelayFunc()
    {
        //this._setCardActice(true);
        int[] cardList = DZGameLogic.Instance.getRandCardList();

        cardDataCommon = new int[] { cardList[0], cardList[1], cardList[2], cardList[3], cardList[4] };
        cardData1 = new int[] { cardList[5], cardList[6] };
        cardData2 = new int[] { cardList[7], cardList[8] };

        Debug.Log("===cardData1[0]:" + cardData1[0]);
        Debug.Log("===cardData1[1]:" + cardData1[1]);
        Debug.Log("===cardData2[0]:" + cardData2[0]);
        Debug.Log("===cardData2[1]:" + cardData2[1]);
       

        //生成牌
        instantiateCards();
        StartCoroutine(ShowA());

        
    }

    void instantiateCards()
    {
        GameObject cardObj = Resources.Load<GameObject>("prefabs/cardItem");
        for (int i = 0; i < 9;i++)
        {
            GameObject temp = Instantiate(cardObj);
            if (i < 2)
            {
                temp.transform.SetParent(pos1);
                temp.transform.GetComponent<CardItem>().CardNum = cardData1[i];
            }else if (i < 4)
            {
                temp.transform.SetParent(pos2);
                temp.transform.GetComponent<CardItem>().CardNum = cardData2[i-2];

            }
            else
            {
                temp.transform.SetParent(posCommon);
                temp.transform.GetComponent<CardItem>().CardNum = cardDataCommon[i - 4];
            }
            _resetTransform(temp);
            

        }
    }

    void showCards()
    {
        for (int i = 0; i < pos1.childCount; i++)
        {
            GameObject chid = pos1.GetChild(i).gameObject;
            CardItem cardItem = chid.transform.GetComponent<CardItem>();
            updateCardItem(cardItem);
        }
        for (int i = 0; i < pos2.childCount; i++)
        {
            GameObject chid = pos2.GetChild(i).gameObject;
            CardItem cardItem = chid.transform.GetComponent<CardItem>();
            updateCardItem(cardItem);
        }
        for (int i = 0; i < posCommon.childCount; i++)
        {
            GameObject chid = posCommon.GetChild(i).gameObject;
            CardItem cardItem = chid.transform.GetComponent<CardItem>();
            updateCardItem(cardItem);
        }


        //牌型，谁赢了 
        List<int> fiveCardData1 = DZGameLogic.Instance.fiveFromSeven(new List<int>(cardData1), new List<int>(cardDataCommon));
        List<int> fiveCardData2 = DZGameLogic.Instance.fiveFromSeven(new List<int>(cardData2), new List<int>(cardDataCommon));

        CardType cardType1 = DZGameLogic.Instance.getCardType(fiveCardData1);
        CardType cardType2 = DZGameLogic.Instance.getCardType(fiveCardData2);
        text1Type.text = cardTypeText[(int)cardType1];
        text2Type.text = cardTypeText[(int)cardType2];
        int winNum = DZGameLogic.Instance.compareCard(fiveCardData1, fiveCardData2);

        if (winNum == 2)
        {
            textWhoWin.text = "PLAYER 1 WIN";
        }
        else if (winNum == 1)
        {
            textWhoWin.text = "PLAYER 2 WIN";

        }
        else
        {
            textWhoWin.text = "Draw";
        }
    }

    void _resetTransform(GameObject obj)
    {
        obj.transform.localPosition = new Vector3(0,0,0); 
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = new Vector3(1, 1, 1);
    }

    void updateCardItem(CardItem cardItem)
    {
        int data = cardItem.CardNum;
        Sprite numSprire = nums[data % 16 - 1];
        int colorNum = DZGameLogic.Instance.getCardColor(data) / 16;
        Debug.Log("==colorNum:" + colorNum);
        Sprite colorSprire = colors[colorNum];
        Color color = (colorNum % 2 == 0 ? colorRed : colorBlack);
        cardItem.UpdateItem(numSprire, colorSprire, color);
    }


}
