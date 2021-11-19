using UnityEngine;
using System.Collections;

// 全局游戏设置参数存放类。
public class GlobalSetting : MonoBehaviour {

    #region 单例
    private static GlobalSetting _instance;
    public static GlobalSetting Instance
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

    public Vector3 cardSelectMoveDir;       // 卡片被选中时偏移量
    public float cardInterval;              // 卡片间隔
    public int cardLength;                  // 每个人手牌数量

    public float botThinkTime;              // 电脑回合时间

    public float smallCardInterval;         // 小牌的间隔

    public float cardDiangNao;             //电脑牌的间距
}
