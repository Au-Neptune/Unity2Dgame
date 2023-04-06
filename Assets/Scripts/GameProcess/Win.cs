using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Win : MonoBehaviour
{
    public Material white, teleport;
    public Image winCG;
    public Transform player, boss;
    public PlayableDirector director;

    private AudioManager audioManager;
    private bool turnWhite = false, outWhite = false, disappear = false;
    private float value;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (turnWhite)
        {
            value += Time.deltaTime * (2.5f / 5f);
            if (value >= 1.5)
            {
                value = 1.5f;
                turnWhite = false;
            }
            white.SetFloat("_Value", value);
        }
        if (outWhite)
        {
            value -= Time.deltaTime * (2.5f / 5f);
            if (value <= -1f)
            {
                value = -1f;
                outWhite = false;
            }
            white.SetFloat("_Value", value);
        }
        if (disappear)
        {
            value -= Time.deltaTime;
            if (value <= 0)
            {
                value = 0;
                disappear = false;
            }
            teleport.SetFloat("_Value", value);
        }
    }


    public void WinGame()
    {
        audioManager.Stop();
        GameDatabase.win = true;
        audioManager.Play("Ending");
        player.GetComponent<Player>().StopMoving();
        player.GetComponent<Player>().enabled = false;
        director.Play();
    }

    public void IntoWhite()
    {
        value = -1;
        turnWhite = true;
    }

    public void OutWhite()
    {
        winCG.enabled = true;
        value = 1.5f;
        outWhite = true;
    }

    public void Gone()
    {
        boss.GetComponent<SpriteRenderer>().material = teleport;
        value = 1;
        disappear = true;
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
