using UnityEngine;
using System.Collections;

public class CardViewCreator : MonoBehaviour
{

    #region 单例
    private static CardViewCreator _instance;
    public static CardViewCreator Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            return null;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    #endregion

    public GameObject cardPrefab;
    private string numSpriteDir = "Images/Num";
    private string typeSpriteDir = "Images/TypeColor";
    private Sprite[] numSprites;
    private Sprite[] typeSprites;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // 临时资源加载器，根据数字选择将相应的素材生成卡片。
    public GameObject CreateCard(CardData cardData)
    {
        if (cardPrefab != null)
        {
            GameObject newCard = Instantiate(cardPrefab);
            newCard.GetComponent<CardView>().SetData(cardData);
            return newCard;
        }

        throw new System.Exception("CardPrefab is null, can't create card game object");
    }

    public Sprite[] CardSpriteStore(CardData data)
    {
        if (data != null)
        {
            Sprite[] dataSprites = new Sprite[2];
            switch (data.num)
            {
                case CardNum.Four:
                case CardNum.Five:
                case CardNum.Six:
                case CardNum.Seven:
                case CardNum.Eight:
                case CardNum.Nine:
                case CardNum.Ten:
                case CardNum.J:
                case CardNum.Q:
                case CardNum.K:
                    dataSprites[0] = numSprites[(int)data.num + 3];
                    break;
                case CardNum.A:
                case CardNum.Two:
                case CardNum.Three:
                    dataSprites[0] = numSprites[(int)data.num - 10];
                    break;
            }

            dataSprites[1] = typeSprites[(int)data.typeColor];
            return dataSprites;
        }

        Debug.LogError("CardSpriteStore can't load null dataSprite");
        return null;
    }

    // 初始化，资源加载
    public void Init()
    {
        numSprites = Resources.LoadAll<Sprite>(numSpriteDir);
        typeSprites = Resources.LoadAll<Sprite>(typeSpriteDir);
        Debug.Log(numSpriteDir.Length);
        Debug.Log(typeSprites.Length);
        if (numSprites == null || typeSprites == null)
            Debug.LogError("Can't find card resources");
    }
}
