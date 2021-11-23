using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardItem : MonoBehaviour {

    private Image imageNum;
    private Image imageColor;

    private Color colorRed = new Color(255, 0, 0);
    private Color colorBlack = new Color(0, 0, 0);
    private int cardNum;

    object[] spriteNums;
    object[] spriteColors;


    
    void Awake()
    {

        //spriteNums = Resources.LoadAll("Images/Num");
        imageNum = transform.Find("num").GetComponent<Image>();
        imageColor = transform.Find("color").GetComponent<Image>();

    }
    void Start () {
        

    }
	

    public int CardNum
    {
        get
        {
            return this.cardNum;
        }
        set
        {
            this.cardNum = value;
        }
    }



    public void UpdateItem(Sprite spriteNum, Sprite spriteColor,Color color)
    {
        //Sprite sp = Resources.Load<Sprite>("Images/" + "Num_11");

        
        //Sprite sp = (Sprite)spriteNums[1];
        //Sprite sp = (Sprite)spriteNums[1];
        
        imageNum.sprite = spriteNum;
        imageColor.sprite = spriteColor;
        imageNum.color = color;
        imageColor.color = color;
        this.transform.Find("back").gameObject.SetActive(false);
        
    }
}
