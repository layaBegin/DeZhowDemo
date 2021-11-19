using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 功能按钮视图，包括开始，过，重选，出牌，以及提示，闹钟时间的控制
public class HandCardFuncView : MonoBehaviour {

	Button BeginBtn;		// 开始按钮
	Button passBtn;			// 过按钮
	Button recoverBtn;		// 重选按钮
	Button tipBtn;			// 提示按钮
	Button playBtn;			// 出牌按钮
    //Text roundText;         // 当前出牌玩家显示
    //Text playTypeText;      // 打出牌型的显示

	void Awake() {
		// 添加按钮引用
		BeginBtn = transform.Find("Buttons/Btn_Begin").GetComponent<Button>();
		passBtn = transform.Find("Buttons/Btn_Pass").GetComponent<Button>();
		recoverBtn = transform.Find("Buttons/Btn_Recover").GetComponent<Button>();
		tipBtn = transform.Find("Buttons/Btn_Tip").GetComponent<Button>();
		playBtn = transform.Find("Buttons/Btn_Play").GetComponent<Button>();
        //roundText = transform.Find("RoundInfo/Text").GetComponent<Text>();
        //playTypeText = transform.Find("RoundInfo/playTypeText").GetComponent<Text>();

        // 添加事件委托
        BeginBtn.onClick.AddListener(Begin);
        passBtn.onClick.AddListener(Pass);
        recoverBtn.onClick.AddListener(Recover);
        tipBtn.onClick.AddListener(Tip);
        playBtn.onClick.AddListener(Play);
    }

    void Start(){
        // 添加监听
        //GameControl.Instance.currentPlayerChanged += OnCurrentPlayerChange;
        //GameControl.Instance.playCardEvent += OnPlayInfoChagne;
    }

    public void Begin()
    {
        //Debug.Log("Begin");

        GameControl.Instance.OnClickBeginBtn();
    }

    public void Pass()
    {
        Debug.Log("Pass");
        GameControl.Instance.handCardControls[0].Pass();
    }

    public void Recover()
    {
        GameControl.Instance.handCardControls[0].RecoverAll();
    }

    public void Tip()
    {
        GameControl.Instance.handCardControls[0].Tips();
    }

    public void Play()
    {
        GameControl.Instance.handCardControls[0].PlayCardData();
    }

    //public void OnCurrentPlayerChange(int index)
    //{
    //    roundText.text = "当前玩家为：" + index;
    //}

    //public void OnPlayInfoChagne(CardData[] cardDatas, PlayCardData data)
    //{
    //    playTypeText.text = "出这幅牌的玩家是 : " + data.ID + "\n";
    //    playTypeText.text += "牌型是 ：" + data.CardType.ToString() + "\n";
    //    if(cardDatas != null)
    //    {
    //        foreach (CardData card in cardDatas)
    //        {
    //            playTypeText.text += card.num + "  ";
    //        }
    //    }
        
    //}
}
