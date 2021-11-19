using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GuoChangJing : MonoBehaviour {
    public GameObject ChangJing1;
    public GameObject BaiJingTu1;
    public GameObject ChangJing2;
    public GameObject BaiJingTu2;
    public Button QiDongAnNu;
    public Button DuoDingZhuAnNu;
    // Use this for initialization
    void Start () {
        DuoDingZhuAnNu.onClick.AddListener(DuoDingZhuAnNuShiJing);
         QiDongAnNu.onClick.AddListener(QiDongAnNuShiJiang);
	}
	
	// Update is called once per frame
	void Update () {
      
          
         
	}
    public void DuoDingZhuAnNuShiJing()
    {
        QiDongAnNu.interactable = true;

    }
    public void QiDongAnNuShiJiang()
    {
        ChangJing1.transform.gameObject.SetActive(false);
        BaiJingTu1.transform.gameObject.SetActive(false);
        ChangJing2.transform.gameObject.SetActive(true);
        BaiJingTu2.transform.gameObject.SetActive(true);

    }
}
