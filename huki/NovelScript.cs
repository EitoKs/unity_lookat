using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPをつかうとき必要

//ノベルテキストの表示・非表示を管理するスクリプト
//ノベルの文章を書くためのスクリプト
public class NovelScript : MonoBehaviour
{
    public static NovelScript Instance;     //多くのTimelineで使うためインスタンス化しておく

    [SerializeField]
	private GameObject MessagePanel;       //ノベルテキストを表示するUI
    [SerializeField]
    private TextMeshProUGUI NameText;      //名前を表示するTMP（話している人名を表示）

    //もう一つのメッセージパネルと名前表示欄（追加）
    [SerializeField]
    private GameObject MessagePanel2;
    [SerializeField]
    private TextMeshProUGUI NameText2;


    //private NovelUI novelui;               //NovelUIスクリプトを取得するため
    public NovelUI[] novelui;

    private bool[] now_novel = new bool[2];         //メッセージ画面を表示しているか判定

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //シーンをまたいでも破棄されない
        }
        else
        {
            Destroy(gameObject);            //ロード2回目以降は新しい方のオブジェクトを破棄
        }
    }

    void Start()
    {
        //novelui = this.GetComponent<NovelUI>();   //スクリプトを取得
        now_novel[0] = false;
        now_novel[1] = false;

        // string[] demo = {
        //     "これはデモです",
        //     "あああああああああああああああ",
        //     "いいいいいいいいいいいいいいい"
        // };

        // int[] ndemo = {1,1,1};
        // NovelUIOpen(demo, ndemo);
    }

    //ノベルテキストを表示するUIを開く関数
    //引数は表示したいテキストの内容を格納した配列
    public void NovelUIOpen(string[] noveltext, int[] novelother){
        if(now_novel[0] == false && novelother[0] == 0){
            MessagePanel.SetActive(true);           //メッセージ画面を表示
            now_novel[0] = true;                       //ノベルUIを表示している判定にする
            EventManager.Instance.pause();          //タイムラインを一時停止させる
            StartCoroutine(Cotest(noveltext, novelother));  //文章を表示させるコルーチンを実行
        }
        else if(now_novel[1] == false && (novelother[0] == 1 || novelother[0] == 2))
        {
            MessagePanel2.SetActive(true);           //メッセージ画面を表示
            now_novel[1] = true;                       //ノベルUIを表示している判定にする
            EventManager.Instance.pause();          //タイムラインを一時停止させる
            StartCoroutine(Cotest(noveltext, novelother));  //文章を表示させるコルーチンを実行
        }
    }

    //ノベルUIを消す関数
    public void NovelUIClose(){
        if(now_novel[0] == true){
            MessagePanel.SetActive(false);           //メッセージ画面を非表示
            Invoke("NovelClose", 1.0f);              //1秒後に動かせるようにする
        }
        else if(now_novel[1] == true)
        {
            MessagePanel2.SetActive(false);           //メッセージ画面を非表示
            Invoke("NovelClose", 1.0f);              //1秒後に動かせるようにする
        }
    }

    //ノベル画面を閉じるか決める
    public void NovelClose(){
        if(now_novel[0] == true){
            now_novel[0] = false;                  //ノベルUIを消している判定にする
        }
        else if(now_novel[1] == true)
        {
            now_novel[1] = false;                  //ノベルUIを消している判定にする
        }
        EventManager.Instance.restart();    //止めているタイムラインを再開させる
    }

    // クリック待ちのコルーチン
    IEnumerator Skip()
    {
        //順々に表示しているとき待機させておく（第1関門）
        if(now_novel[0])
        {
            while (novelui[0].text_playing) yield return null;
        }
        else if(now_novel[1])
        {
            while (novelui[1].text_playing) yield return null;
        }
        //クリックされたとき待機させておく
        while(true){
            //画面をタップすると読み進める
            if(Input.GetMouseButtonDown(0)){
                break;
            //押されない限り待機
            }else{
                yield return null;
            }
        }
    }
    
     // 文章を表示させるコルーチン
    IEnumerator Cotest(string[] Novel_Text, int[] Novel_other)
    {
        //入れた文章数まで繰り返し処理
        for(int i=0; i < Novel_Text.Length; i++){
            switch(Novel_other[i]){
                case 0:     //主人公
                    NameText.text = "アン";
                    break;
                case 1:     //敵の親玉
                    NameText2.text = "？？？";
                    break;
                case 2:     //敵の親玉
                    NameText2.text = "蚊王";
                    break;
                default :
                    break;
            }
            //文章を表示させる
            if(now_novel[0]) novelui[0].DrawText(Novel_Text[i]);
            else if(now_novel[1]) novelui[1].DrawText(Novel_Text[i]);
            yield return StartCoroutine("Skip");
        }
        NovelUIClose();         //すべて表示しきったら画面を閉じる
    }
}
