using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestView : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        int count = 0;
        List<List<int>> list = DZGameLogic.Instance.getCombinationFlagArrs(7,5);
        for (int i = 0; i < list.Count;i++)
        {
            List<int> item = list[i];
            string s = "";
            for (int j = 0;j < item.Count;j++)
            {
                s += item[j];
            }
            Debug.Log("item:" + s);
            count++;

        }
        Debug.Log("count:"+count);

        //DZGameLogic.Instance.test1();
    }

    // Update is called once per frame
    void Update () {
	
	}


}
