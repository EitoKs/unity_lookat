using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//音関係をまとめたスクリプト
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;    //どこでも音を鳴らせるようにインスタンス化しておく

    [Header("音源")]
    [SerializeField]
    private AudioClip[] BGMSounds;
    [SerializeField]
    private AudioClip[] SESounds;

    [Header("AudioSource")]
    [SerializeField]
    private AudioSource BGMSource;
    [SerializeField]
    private AudioSource SESource;

    [Header("音量調整スライダーUI")]
    [SerializeField]
    private Slider BGMSlider;
    [SerializeField]
    private Slider SESlider;

    [Header("ミュートUI")]
    [SerializeField]
    private GameObject BGMButton;
    [SerializeField]
    private GameObject SEButton;

    private bool isFadeOut;
    private double FadeDeltaTime;
    public double FadeInSecond;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //シーンをまたいでも破棄されない
        }
        else
        {
            Destroy(gameObject);            //ロード2回目以降は新しい方のオブジェクトを破棄(シングルトン)
        }
    }

    void Start()
    {
        isFadeOut = false;
        FadeDeltaTime = 0;
    }

    void Update()
    {
        if(isFadeOut)
        {
            FadeDeltaTime += Time.deltaTime;
            if(FadeDeltaTime >= FadeInSecond)
            {
                FadeDeltaTime = FadeInSecond;
                isFadeOut = false;

                BGMSource.Stop();
                BGMSource.volume = 1;
                return;
            }
            BGMSource.volume = (float)(1.0 - FadeDeltaTime / FadeInSecond);
        }
    }

    //BGMを鳴らす関数
    public void PlayBGM(int num)
    {
        AudioClip clip = BGMSounds[num];    //登録した音を見つける
        BGMSource.clip = clip;      //引数の音源をセットする
        if(clip == null) return;    //音源が無ければ処理をしない

        BGMSource.Play();           //再生する
    }

    //SEを鳴らす関数
    public void PlaySE(int num)
    {
        AudioClip clip = SESounds[num]; //登録した音を見つける
        if(clip == null) return;        //音源が無ければ処理をしない

        SESource.PlayOneShot(clip);     //一度だけ鳴らす
    }

    //BGMのON/OFFを切り替える関数
    public void ToggleBGM()
    {
        BGMSource.mute = !BGMSource.mute;
        BGMButton.GetComponent<MuteButton>().MuteButtonChange(BGMSource.mute);
    }

    //SEのON/OFFを切り替える関数
    public void ToggleSE()
    {
        SESource.mute = !SESource.mute;
        SEButton.GetComponent<MuteButton>().MuteButtonChange(SESource.mute);
    }

    //BGMのボリュームをスライダーによって調整する関数
    public void BGMVolume()
    {
        BGMSource.volume = BGMSlider.value;
    }

    //SEのボリュームをスライダーによって調整する関数
    public void SEVolume()
    {
        SESource.volume = SESlider.value;
    }

    public void FadeOutBGM()
    {
        FadeDeltaTime = 0f;
        isFadeOut = true;
    }
}
