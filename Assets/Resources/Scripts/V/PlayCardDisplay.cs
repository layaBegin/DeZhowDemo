using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayCardDisplay : MonoBehaviour {

    public Transform[] displays;
    public GameObject smallCardPrefab;  //小牌预制体
    GameObject newCard;
    public Image image;
  

    // Use this for initialization
    void Start () {
        GameControl gameControl = GameControl.Instance;
        gameControl.playCardEvent += OnPlayCardChanged;
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
   

    // 每当玩家或电脑出牌事件触发，调用
    public void OnPlayCardChanged(CardData[] cards, PlayCardData playCardData)
    {
        // 。。。
        if(cards == null)
        {
          
           Debug.Log("有人打不出牌");
            image.gameObject.SetActive(true);

        }
        else
        {
            image.gameObject.SetActive(false);

            int count = cards.Length;
            // 如果存在子物体，先删除
            if (displays[playCardData.ID].childCount > 0)
            {
                for(int i = displays[playCardData.ID].childCount -1; i >=0; i--)
                {
                    Destroy(displays[playCardData.ID].GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < count; i++)
            {
                newCard = (GameObject)GameObject.Instantiate(smallCardPrefab, displays[playCardData.ID]);
                newCard.transform.localPosition = Vector3.zero + Vector3.left * i * GlobalSetting.Instance.smallCardInterval;
                
                newCard.GetComponent<CardView>().SetData(cards[i]);
              
            }
        }
        Debug.Log("出牌咯~");
    }
}
