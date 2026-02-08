using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    public Text loadingStatusText;
    public GameObject  GameUI;
    IEnumerator Start()
    {
        GameUI.SetActive(false);
        loadingStatusText.text = "正在读取存档...";
        yield return new WaitForSeconds(1.0f);
        //模拟读取的时间
        CurrencyManager.Instance.LoadGame();
        //这一句看起来很别扭
        //实际上可以看成
        //CurrencyManager myManager = new CurrencyManager();
        //myManager.LoadGame();
        //但在实际运用过程中，很多脚本是挂载在物体上的，不能随便new一个脚本，必须用场景里已经存在的那个
        //单例模式（Instance）就是省去了要用GameObject.Find去找那个对象的麻烦
        loadingStatusText.text = "加载完成！";
        yield return new WaitForSeconds(0.7f);
        //加载成功后的信息反馈
        loadingStatusText.gameObject.SetActive(false);
        //关闭加载提示
        GameUI.SetActive(true);
    }
}
