using UnityEngine;
using System.Collections;

public class TestView : MonoBehaviour {

    
	// Use this for initialization
	void Start () {

        int a11 = DZGameLogic.Instance.CARD_DATA_ARRAY[11];
        int a30 = DZGameLogic.Instance.CARD_DATA_ARRAY[30];
        Debug.Log("===a11:" + a11);
        Debug.Log("===a30:" + a30);
        Debug.Log("===a30:" + DZGameLogic.Instance.getCardValue(a30));
        Debug.Log("===a30 color:" + DZGameLogic.Instance.getCardColor(a30));

        int a35 = DZGameLogic.Instance.CARD_DATA_ARRAY[35];
        Debug.Log("===a35:" + DZGameLogic.Instance.getCardValue(a35));
        Debug.Log("===a35 color:" + DZGameLogic.Instance.getCardColor(a35));
        int a45 = DZGameLogic.Instance.CARD_DATA_ARRAY[45];
        Debug.Log("===a45:" + DZGameLogic.Instance.getCardValue(a45));
        Debug.Log("===a45 color:" + DZGameLogic.Instance.getCardColor(a45));

        DZGameLogic.Instance.test1();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
