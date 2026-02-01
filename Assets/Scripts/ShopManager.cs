using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Test1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int[] itemPrices;//存放商品价格的数组
    [SerializeField] string[] itemNames;//存放商品名称的数组
    [SerializeField] GameObject[] itemPrefabs;//存放商品的预制体的数组
    [SerializeField] Transform spawnPoint;//物体出现的位置
    public List<GameObject> spawnedItems = new List<GameObject>();//专门装物体的列表，初始是空的
    // Update is called once per frame
    void Update()
    {
        string inputKey = "";
        if (Input.GetKeyDown(KeyCode.Alpha1)) inputKey = "1";
        if (Input.GetKeyDown(KeyCode.Alpha2)) inputKey = "2";
        if (Input.GetKeyDown(KeyCode.Alpha3)) inputKey = "3";
        if (Input.GetKeyDown(KeyCode.R)) inputKey = "Reset";
        switch (inputKey)
        {
            case "1":
                BuyItemByIndex(0);
                break;
            case "2":
                BuyItemByIndex(1);
                break;
            case "3":
                BuyItemByIndex(2);
                break;
            case "Reset":
                CurrencyManager.Instance.AddGold(200);
                break;
        }
        //按下对应的数字键，购买相应的商品
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearAllItems();
        }
        //按下空格，一键清空列表里所有东西
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach(GameObject item in spawnedItems)
            {
                if (item != null)
                {
                    item.GetComponent<Renderer>().material.color = Color.blue;
                    //物体颜色变为红色
                    item.transform.localScale *= 1.5f;
                    //物体变大一些weishe
                }
            }
            
        }
        //当按下C键之后，口袋里所有商品变大变红
    }
    void BuyItem(string nameItem,int priceItem)
    {
        if (CurrencyManager .Instance.TrySpendGold (priceItem))
        {
            Debug.Log("购买成功！,价格是："+priceItem+"商品名称是："+nameItem);
        }
        else
        {
            Debug.Log(nameItem+"钱不够，快去打怪赚钱！");
        }
    }
    public void BuyItemByIndex(int index)
    {
        if (index >= 0 && index < itemPrices.Length)
        {
            string name = itemNames[index];
            int price=itemPrices[index];

            if(CurrencyManager.Instance.TrySpendGold(price))
            {
                Debug.Log("从货架购买了：" + name);
                GameObject newItem=Instantiate(itemPrefabs[index], spawnPoint.position, Quaternion.identity);
                //生成物体
                Rigidbody rb = newItem.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    Vector3 forceDirection = new Vector3(Random.Range(-0.2f, 0.2f), 1f, Random.Range(-0.2f, 0.2f));
                    rb.AddForce(forceDirection * 8f,ForceMode.Impulse);
                }
                //获取刚体组件并且推它一把
                spawnedItems.Add(newItem);
                //把新买的商品加入口袋，即列表
                if (spawnedItems.Count > 5)
                {
                    GameObject oldestItem = spawnedItems[0];
                    //获取列表里最老的一个，第一个
                    Destroy(oldestItem);
                    //销毁这个物体
                    spawnedItems.RemoveAt(0);
                    //销毁列表的位置
                    Debug.Log("已经删除最老的商品");
                }
                //限制口袋的数量，上限为5个
                //多的商品加进来，会删除最老的商品
                Destroy(newItem, 3f);
                //销毁生成的物体
                
            }
            else
            {
                Debug.Log(name + "钱不够");
            }
        }
    }
    //按给的索引购买商店的物品
    //按下对应的按键会购买对应的商品
    //商品会生成并且“喷”出来
    void ClearAllItems()
    {
        //用foreach循环，对列表里每一个物体都执行操作
        foreach(GameObject item in spawnedItems)
        {
            Destroy(item);
        }
        //清空完列表里的物体，也要 清空列表
        spawnedItems.Clear();
        Debug.Log("货架已经清空！");
    }
    //清空列表这个口袋
}
