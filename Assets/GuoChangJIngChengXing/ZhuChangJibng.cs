using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ZhuChangJibng : MonoBehaviour {
    public GameObject ChangJIng2;
    public GameObject BeiJingTu2;
    public GameObject ChangJIng3;
    public GameObject BeiJingTu3;
    public GameObject BeiJingTu3_1;
    public Button GuoChanAnNu;
    // Use this for initialization
    void Start () {
        GuoChanAnNu.onClick.AddListener(GuoChanAnNuShuJiang);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void GuoChanAnNuShuJiang()
    {
        ChangJIng2.gameObject.SetActive(false);
        BeiJingTu2.gameObject.SetActive(false);
        ChangJIng3.gameObject.SetActive(true);
        BeiJingTu3.gameObject.SetActive(true);
        BeiJingTu3_1.gameObject.SetActive(true);
    }
}
