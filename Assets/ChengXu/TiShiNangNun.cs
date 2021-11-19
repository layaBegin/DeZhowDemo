using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TiShiNangNun : MonoBehaviour {
    public HandCardControl handCardControl;
    private Button TiShiButton;
	// Use this for initialization
	void Start () {
        TiShiButton = GetComponent<Button>();
        handCardControl = GetComponent<HandCardControl>();
        TiShiButton.onClick.AddListener(TiShiShiJiang);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void TiShiShiJiang()
    {
        handCardControl.Tips();

    }


}
