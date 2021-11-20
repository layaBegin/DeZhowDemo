using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class TestModel  {

    static string[] m_Data = { "A", "B", "C", "D", "E" };
    static void Main(string[] args)
    {
        Dictionary<string, int> dic = new Dictionary<string, int>();
        for (int i = 0; i < m_Data.Length; i++)
        {
            Console.WriteLine(m_Data[i]);//如果不需要打印单元素的组合，将此句注释掉
            dic.Add(m_Data[i], i);
        }
        GetString(dic);
        Console.ReadLine();
    }
    static void GetString(Dictionary<string, int> dd)
    {
        Dictionary<string, int> dic = new Dictionary<string, int>();
        foreach (KeyValuePair<string, int> kv in dd)
        {
            for (int i = kv.Value + 1; i < m_Data.Length; i++)
            {
                //Console.WriteLine(kv.Key + " " + kv.Value + " " + i);
                Console.WriteLine(kv.Key + m_Data[i]);
                dic.Add(kv.Key + m_Data[i], i);
            }
        }
        //Console.WriteLine(dic.Count);
        if (dic.Count > 0) GetString(dic);
    }


}

