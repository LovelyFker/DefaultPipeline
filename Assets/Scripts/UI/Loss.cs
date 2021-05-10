using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loss : MonoBehaviour
{
    private float durability;//耐久度
    public Text WearText;//耐久度文本
    public Text GradeText;//成绩文本
    public GameObject GameOver;//游戏结束界面
    public Slider WearStrip;//耐久度条
    public AudioClip Gas;//汽油
    public AudioClip Ser;//维修
    public AudioClip Over;//结束

    // Start is called before the first frame update
    void Start()
    {
        
        durability = 100;
        GameOver.SetActive(false);
        WearText.text = "耐久度：" + durability.ToString() + "%";
    }

    // Update is called once per frame
    void Update()
    {
        if(durability<=0||Score.oil_mass<=0)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            GradeText.text = "游戏结束，您的最终成绩为：" + Score.journey.ToString("f1") + "米";
            GameOver.SetActive(true);
            //Debug.Log("游戏结束");
        }
        WearText.text = "耐久度：" + durability.ToString() + "%";
        WearStrip.value = durability;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Loss")
        {
            if (durability > 0)
            {
                durability -= 5;
            }
            else
            {
                durability = 0;
            }
            //Debug.Log("durability" + durability);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gasoline")//汽油
        {
            this.GetComponent<AudioSource>().clip = Gas;
            this.GetComponent<AudioSource>().Play();
            if (Score.oil_mass+5> 100)
            {
                Score.oil_mass = 100;
            }
            else
            {
                Score.oil_mass += 5;
            }
            Destroy(other.gameObject);
        }
        if (other.tag == "service")//维修工具
        {
            this.GetComponent<AudioSource>().clip = Ser;
            this.GetComponent<AudioSource>().Play();
            if (durability+5 > 100)
            {
                durability = 100;
            }
            else
            {
                durability += 5;
            }
            Destroy(other.gameObject);
        }
    }
        public void Restart()//重新开始按钮
        {
        durability = 100;
        Score.oil_mass = 100;
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene("ccc", LoadSceneMode.Single);
        
        }
}
