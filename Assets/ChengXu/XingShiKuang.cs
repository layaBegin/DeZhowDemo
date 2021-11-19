using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XingShiKuang : MonoBehaviour
{
    private Image image;
    public Sprite ShuengZi;
    public bool a;
    private bool b;
    public Image faji;
    public Image HuongJiang;
  
    float c = 0;
    float d = 2f;
    int e = 0;
    void Start()
    {

        image = transform.FindChild("XingShiKuang").GetComponent<Image>();
        GameControl.Instance.playCardEvent += XingShiKuangShiJiang;

    }

    // Update is called once per frame
    void Update()
    {

       
        
    }
    public void XingShiKuangShiJiang(CardData[] cards, PlayCardData playCardData)
    {
        if (playCardData.CardType == PlayCardType.ThreeWithDouble)
        {


            faji.gameObject.SetActive(true);
            Invoke("FaiJingShiJian",4);

        }
        else if (playCardData.CardType == PlayCardType.FourWithOne)
        {
            HuongJiang.gameObject.SetActive(true);
            Invoke("HuongJiangShiJiang",4);

        }
        else if (playCardData.CardType == PlayCardType.Straight)
        {
            image.gameObject.SetActive(true);

            image.sprite = ShuengZi;
            Invoke("ShuanZiShiJiang",4);
        }
       
        }
   public void  FaiJingShiJian()
    {
        //yield return new WaitForSeconds(5); 
        faji.gameObject.SetActive(false);
       
    }
    public void ShuanZiShiJiang()
    {
        image.gameObject.SetActive(false);
       
    }
    public void HuongJiangShiJiang()
    {
        HuongJiang.gameObject.SetActive(false);
        
    }
   
}
