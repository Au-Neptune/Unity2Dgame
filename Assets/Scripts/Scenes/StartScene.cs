using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Button startBtn, settingBtn, loadBtn, recordConfirmBtn;
    public GameObject setting, player, recordsList;
    public GameObject[] records;
    public Text[] recordsText;
    public Text recordConfirmBtnTxt;
    
    private bool noRecords = false;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        //讀取manifest 知道有哪些records
        string FileToRead = Application.dataPath + "/Records/manifest.txt";

        // 檢查目錄是否存在，如果不存在則建立目錄
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // 檢查檔案是否存在，如果不存在則建立檔案
        if (!File.Exists(FileToRead))
        {
            File.Create(FileToRead).Close();
        }
        
        string[] lines = File.ReadAllLines(FileToRead);

        foreach (string line in lines)
            Debug.Log(line);

        //最後三筆是最新的檔案
        int len = lines.Length;
        if (len == 0)
        {
            noRecords = true;
            recordConfirmBtnTxt.text = "沒有紀錄";
        }
        else
        {
            foreach (Text txtobj in recordsText)
            {
                if (len > 0)
                {
                    records[lines.Length - len].gameObject.SetActive(true);
                    txtobj.text = lines[len - 1];
                    --len;
                }
            }
        }

        startBtn.onClick.AddListener(delegate
        {
            startBtnClick();
        });
        settingBtn.onClick.AddListener(delegate
        {
            settingBtnClick();
        });
        loadBtn.onClick.AddListener(delegate
        {
            loadBtnClick();
        });
        recordConfirmBtn.onClick.AddListener(delegate
        {
            recordConfirmBtnClick();
        });
        if (GameDatabase.win) player.SetActive(true);
        audioManager.Stop();
        audioManager.Play("Menu");
    }

    void recordConfirmBtnClick()
    {
        if (noRecords)
        {
            recordsList.SetActive(!recordsList.activeSelf);
            return;
        }
        audioManager.Stop();
        //go loading scene
        SceneManager.LoadScene("Load");
    }

    void loadBtnClick()
    {
        //顯示records list
        recordsList.SetActive(!recordsList.activeSelf);
        GameDatabase.loadingNo = 0;
    }

    void startBtnClick()
    {
        audioManager.Stop();
        GameDatabase.loadingNo = -1;
        //go loading scene
        SceneManager.LoadScene("Load");
    }

    void settingBtnClick()
    {
        setting.SetActive(!setting.activeSelf);
    }

    public void ChangeWASDInput(bool state)
    {
        if (state) GameDatabase.operationMode = 0;
    }

    public void ChangeArrowInput(bool state)
    {
        if (state) GameDatabase.operationMode = 1;
    }

    public void ChangeJoyStick(bool state)
    {
        if (state) GameDatabase.operationMode = 2;
    }

    public void ChangeRecord1(bool state)
    {
        GameDatabase.loadingNo = 0;
        if (state) GameDatabase.loadingNo = 0;
    }
    public void ChangeRecord2(bool state)
    {
        GameDatabase.loadingNo = 1;
        if (state) GameDatabase.loadingNo = 1;
    }
    public void ChangeRecord3(bool state)
    {
        GameDatabase.loadingNo = 2;
        if (state) GameDatabase.loadingNo = 2;
    }
}
