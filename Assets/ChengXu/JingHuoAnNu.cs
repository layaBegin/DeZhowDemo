using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class JingHuoAnNu : MonoBehaviour {
    private Button Btn_Pass;
    private Button Btn_Tip;
    private Button Btn_Play;
    private Button Btn_Recover;
    private Image image;

    // Use this for initialization
    void Start () {
        Btn_Pass = transform.FindChild("Btn_Pass").GetComponent<Button>();
        Btn_Tip = transform.FindChild("Btn_Tip").GetComponent<Button>();
        Btn_Play = transform.FindChild("Btn_Play").GetComponent<Button>();
        Btn_Recover = transform.FindChild("Btn_Recover").GetComponent<Button>();
        image = transform.FindChild("NaoZhong").GetComponent<Image>();
        GameControl.Instance .currentPlayerChanged += AnNuShiJiang;


    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AnNuShiJiang(int currentPlayerIndex)
    {
        if (currentPlayerIndex ==0)
        {
            Btn_Pass.gameObject.SetActive(true);
            Btn_Play.gameObject.SetActive(true);
            Btn_Recover.gameObject.SetActive(true);
            Btn_Tip.gameObject.SetActive(true);
            image.gameObject.SetActive(true);
        }
        else
        {
            Btn_Pass.gameObject.SetActive(false);
            Btn_Play.gameObject.SetActive(false);
            Btn_Recover.gameObject.SetActive(false);
            Btn_Tip.gameObject.SetActive(false);
            image.gameObject.SetActive(false);
        }
       // Debug.LogFormat("a:" + currentPlayerIndex);
    }


}
