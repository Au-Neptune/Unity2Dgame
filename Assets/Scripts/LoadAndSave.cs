using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;


public class LoadAndSave : MonoBehaviour
{
    public GameObject player, boss, loadingProgress; 
    public GameObject[] enemies; //不包含BOSS
    public GameUI UI;
    public GameObject cams1, cams2;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("loadingNo:" + GameDatabase.loadingNo);

        if (GameDatabase.loadingNo != -1)
        {
            //需要載入紀錄
            //讀取manifest 知道有哪些records
            string FileToRead = Application.dataPath + "/Records/manifest.txt";
            string[] lines = File.ReadAllLines(FileToRead);

      
            //最後三筆是最新的檔案
            string targetFileName = lines[lines.Length - 1 - GameDatabase.loadingNo];
            Debug.Log(targetFileName);

            Load(targetFileName);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //P為存檔功能
        {
            if (boss.GetComponent<Boss>().fight == true)
            {
                loadingProgress.GetComponent<Text>().text = "Boss Fight Cant Save!";
                loadingProgress.SetActive(true);
                Invoke("hideLoadingProgress", 3.0f);
            }
            else
            {
                Invoke("hideLoadingProgress", 3.0f);
                doSave();            
            }
        }
    }

    void hideLoadingProgress()
    {
        loadingProgress.SetActive(false);
    }

    void doSave()
    {
        //格式:
        string characterInfo = player.transform.position.x + "|" + player.transform.position.y + "|" + player.transform.position.z + "|" + GameDatabase.PlayerHP.ToString() + "|" + GameDatabase.BowActive;
        string bossInfo = boss.transform.position.x + "|"+ boss.transform.position.y + "|"+ boss.transform.position.z + "|" + GameDatabase.BossHP.ToString() + "|" + (boss.GetComponent<Boss>().fight == true ?"True":"False");
        string enemiesInfo = "";
        foreach (GameObject obj in enemies)
        {
            if(obj == null)
            {
                enemiesInfo += "null" + "\r\n";
                continue;
            }
            enemiesInfo += obj.transform.position.x + "|" + obj.transform.position.y + "|" + obj.transform.position.z + "|" + obj.GetComponent<Enemy>().health + "\r\n";
        }
        string allDataString = characterInfo + "\r\n" + bossInfo + "\r\n" + enemiesInfo;
        Save(allDataString);
        loadingProgress.SetActive(true);
    }

    public static String GetTimestamp(DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }

    //寫檔
    public void Save(string data)
    {
        String timeStamp = GetTimestamp(DateTime.Now);
        //檔名為save.text
        FileStream fs = new FileStream(Application.dataPath + "/Records/save" + timeStamp + ".txt", FileMode.Create);
        //儲存時時二進位制,所以這裡需要把我們的字串轉成二進位制
        byte[] bytes = new UTF8Encoding().GetBytes(data);
        fs.Write(bytes, 0, bytes.Length);
        //每次讀取檔案後都要記得關閉檔案
        fs.Close();

        //增加檔名到manifest
        File.AppendAllText(Application.dataPath + "/Records/manifest.txt", "save" + timeStamp + Environment.NewLine);

        Debug.Log("Save success");
    }

    //讀取
    public void Load(string filename)
    {
        string FileToRead = Application.dataPath + "/Records/" + filename + ".txt";
        string[] lines = File.ReadAllLines(FileToRead);
        string[] temp;
        int enemiesIndex = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            temp = lines[i].Split('|');
            //第一行是玩家資訊 第二行是boss資訊 第三~六是骷髏資訊
            if (i == 0) //玩家資訊
            {
                player.transform.position = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
                player.GetComponent<Player>().health = int.Parse(temp[3]);
                GameDatabase.PlayerHP = int.Parse(temp[3]);

                if (temp[4] == "True")
                {
                    UI.UpdateBowState(true);
                    GameDatabase.BowActive = true;
                    player.GetComponent<Player>().canUseBow = true;
                }
                

            }else if(i == 1)
            {
                boss.transform.position = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
                boss.GetComponent<Boss>().health = int.Parse(temp[3]);
                GameDatabase.BossHP = int.Parse(temp[3]);

                if (temp[4] == "True")
                {
                    boss.GetComponent<Boss>().fight = true;
                    boss.GetComponent<Animator>().SetBool("Fight", true);
                    cams1.SetActive(false);
                    cams2.SetActive(true);
                  
                }
            }
            else
            {
                if (temp[0] == "null")
                {
                    Destroy(enemies[enemiesIndex]);
                }
                else
                {
                    enemies[enemiesIndex].transform.position = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
                    enemies[enemiesIndex].GetComponent<Enemy>().health = int.Parse(temp[3]);
                }
                ++enemiesIndex;
            }
            

            Debug.Log(lines[i]);
        }
    }
}
