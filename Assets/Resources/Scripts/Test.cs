using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	public Button singleBtn;
	public Button doubleBtn;
	public Button threeWithDoubleBtn;
	public Button fourWithOneBtn;
	public Button straightBtn;

	public HandCardControl tempControl;
	// Use this for initialization
	void Start ()
	{
		singleBtn.onClick.AddListener(SingleBtn);
		doubleBtn.onClick.AddListener(DoubleBtn);
		threeWithDoubleBtn.onClick.AddListener(ThreeWithDoubleBtn);
		fourWithOneBtn.onClick.AddListener(FourWithOneBtn);
		straightBtn.onClick.AddListener(StraightBtn);
	}

	public void SingleBtn()
	{
		tempControl.SetType(PlayCardType.Single) ;
	}

	public void DoubleBtn()
	{
		tempControl.SetType(PlayCardType.Double);
	}
	
	public void ThreeWithDoubleBtn()
	{
		tempControl.SetType(PlayCardType.ThreeWithDouble);
	}
	
	public void FourWithOneBtn()
	{
		tempControl.SetType(PlayCardType.FourWithOne);
	}
	
	public void StraightBtn()
	{
		tempControl.SetType(PlayCardType.Straight);
	}
}
