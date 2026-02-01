using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;//金币管理的实例
    [SerializeField] int gold = 100;//初始金币数
    [SerializeField] TMP_Text goldText;//UI的text显示变量
    public int goldMultiplier = 1;//默认的金币倍率是1
    public Slider powerUpSlider;//场景里的Slider
    private Coroutine powerUpRoutine;
    float timer = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    private void Start()
    {
        gold = PlayerPrefs.GetInt("SavedGold", 0);
        //如果有存档，就读取它；没有就默认为0
        StartCoroutine(AddGoldEverySecond());
        //游戏一开始，就启动“印钞机”
        
    }
    //这是一个协程函数
    //用于挂机存钱
    IEnumerator AddGoldEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            AddGold(10*goldMultiplier );//每秒增加10块钱
            Debug.Log("自动入账：10金币");
        }
    }
    void Update()
    {
        //timer += Time.deltaTime;
        //if (timer >= 1.0f)
        //{
        //    timer = 0;
        //    gold += 1;
        //}
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (PlayerPrefs.HasKey("SavedGold") )
            {
                PlayerPrefs.DeleteAll();
                gold = 0;
                UpdateUI();
                //删除后最好手动刷新UI，或者把内存里的变量清零
                Debug.Log("已经清除金币的存档");
            }
        }
        //按下D，删除金币挂机的存档
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (powerUpRoutine != null)
            {
                StopCoroutine(powerUpRoutine);
            }
            powerUpRoutine = StartCoroutine(PowerUpRoutine());
        }
        //按下B，进入双倍收益模式，持续5秒
    }
    IEnumerator PowerUpRoutine()
    {
        Debug.Log("进入双倍收益模式！");
        goldMultiplier = 2;
        powerUpSlider.gameObject.SetActive(true);//显示进度条
        float duration = 5f;
        float remainingTime = duration;
        while (remainingTime > 0)
        {
            powerUpSlider.value = remainingTime / duration;
            remainingTime-=Time.deltaTime;
            yield return null;
            //暂停一帧，不是暂停秒
        }
        powerUpSlider.value = 0;
        powerUpSlider.gameObject.SetActive(false);
        //隐藏进度条
        goldMultiplier = 1;
        Debug.Log("双倍收益模式结束！");
        powerUpRoutine = null;
    }
    void UpdateUI()
    {
      
        if (goldText == null)
        {
            return;
        }
        if (gold >= 1000000)
        {
            goldText.text = "Gold:" + (gold / 1000000f).ToString("F1") + "M";

        }
        else if (gold >= 1000)
        {
            goldText.text = "Gold:" + (gold / 1000f).ToString("F1") + "k";
        }
        else
        {
            goldText.text = "Gold:" + gold.ToString();
        }
        
    }
    //要做一个金币余额的显示功能
    //当金币大于一百万的时候，直接略写成xxM
    //当金币大于一千的时候，略写成xxk
    //ToString("F1")表示保留1位小数

    public void AddGold(int amount)
    {
        gold = Mathf.Clamp(gold + amount, 0, 999999999);

        PlayerPrefs.SetInt("SavedGold", gold);
        //把新的金币数存起来，钥匙名叫“SavedGold”
        //这是一个存档功能
    }
    //加钱的方法，防止金币值过大或者过小溢出
    //clamp——夹子
    //Mathf.Clamp()——把一个数值限制在制定的范围
    //gold+amount——我想要改变的值
    //0——这是最小值
    //999999999——这是最大值

    public bool TrySpendGold(int price)
    {
        if (gold >= price)
        {
            gold -= price;
            return true;
        }
        return false;
    }
    //检查并扣钱的方法
}
