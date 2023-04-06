using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Slider PlayerHP, BossHP;
    public Image Bow;
    public Text HP;
    public GameObject joystick, RE;
    private AudioManager audioManager;

    void Start()
    {
        if (GameDatabase.operationMode == 2) joystick.SetActive(true);
        else joystick.SetActive(false);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.Stop();
        audioManager.Play("Castle");
        BossHP.value = GameDatabase.BossHP;
        PlayerHP.value = GameDatabase.PlayerHP;
        HP.text = PlayerHP + "/200";
    }

    void Update()
    {

    }

    public void UpdatePlayerHP(int health)
    {
        PlayerHP.value = health;
        GameDatabase.PlayerHP = health;
        HP.text = health.ToString() + "/200";
    }

    public void UpdateBossHP(int health)
    {
        BossHP.gameObject.SetActive(true);
        GameDatabase.BossHP = health;
        BossHP.value = health;
    }

    public void UpdateBowState(bool bow)
    {
        GameDatabase.BowActive = bow;
        Bow.enabled = bow;
    }

    public void ShowReturnButton()
    {   
        RE.SetActive(true);
    }
}
