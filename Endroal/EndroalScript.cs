using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPをつかうとき必要
//エンドロールを操作するスクリプト
public class EndroalScript : MonoBehaviour
{
    //エンドロールを表示するパネル
    [SerializeField]
    private GameObject EndroalPanel;
    //エンドロールで表示するテキスト２つ
    [SerializeField]
    private GameObject EndroalTextUpper;
    [SerializeField]
    private GameObject EndroalTextBottom;
    //TMP
    private TextMeshProUGUI UpperText;
    private TextMeshProUGUI BottomText;
    //アニメーター
    private Animator UpperAnimator;
    private Animator BottomAnimator;

    //それぞれのテキストに表示される文章
    [Multiline]
    public string[] endroaltextupper;
    [Multiline]
    public string[] endroaltextbottom;
    //カウント用変数
    private int[] count = new int[2];

    [SerializeField]
    private GameObject LastText;    //最後に表示されるテキスト
    private Animator LastTextAni;

    void Start()
    {
        EndroalPanel.SetActive(false);

        UpperText = EndroalTextUpper.GetComponent<TextMeshProUGUI>();
        BottomText = EndroalTextBottom.GetComponent<TextMeshProUGUI>();

        UpperAnimator = EndroalTextUpper.GetComponent<Animator>();
        BottomAnimator = EndroalTextBottom.GetComponent<Animator>();

        LastTextAni = LastText.GetComponent<Animator>();

        count[0] = 0;
        count[1] = 0;
    }

    public void EndroalStart()
    {
        Debug.Log("エンドロールスタート");
        EndroalPanel.SetActive(true);      //エンドロールを表示するパネルを表示
        EventManager.Instance.PlayEvent(15);     //Timelineを実行
    }
    //エンドロールのテキストを表示させる関数
    //上のテキスト表示用関数
    public void UpperTextShow()
    {
        if(count[0] >= endroaltextupper.Length)
        {
            count[0] = 0;
            return;
        }
        else
        {
            UpperText.text = endroaltextupper[count[0]];    //(カウント)番目のテキストをセット
            count[0]++;     //カウント
        }
        UpperAnimator.SetTrigger("Fadein");
    }
    //下のテキスト表示用関数
    public void BottomTextShow()
    {
        if(count[1] >= endroaltextbottom.Length)
        {
            count[1] = 0;
            return;
        }
        else
        {
            BottomText.text = endroaltextbottom[count[1]];    //(カウント)番目のテキストをセット
            count[1]++;     //カウント
        }
        BottomAnimator.SetTrigger("Fadein");
    }
    //エンドロールが終わった時の処理
    public void Endroalend()
    {
        EndroalPanel.SetActive(false);      //エンドロールを表示するパネルを表示
    }

    public void LastTextShow()
    {
        LastTextAni.SetBool("LastText", true);
    }

    public void LastTextErase()
    {
        LastTextAni.SetBool("LastText", false);
    }
}
