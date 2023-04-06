using UnityEngine.Playables;
using UnityEngine;

public class IntoBossFight : MonoBehaviour
{
    public Material matBlur, matTeleport, matHurt , def;
    public PlayableDirector director;
    public Transform Player, Boss, PlayerPos, BossPos;
    public float duration = 3f;

    private AudioManager audioManager;
    private float blurAmount = 5, value = 1;
    private bool reduceBlur = false, disappear = false, appear = false;

    void Start()
    {
        matBlur.SetFloat("_Blur_Amount", 5);
        matTeleport.SetFloat("_Value", 1);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (reduceBlur)
        {
            blurAmount -= Time.deltaTime * (5 / duration);
            if (blurAmount <= 0)
            {
                blurAmount = 0;
                reduceBlur = false;
            }
            matBlur.SetFloat("_Blur_Amount", blurAmount);
        }
        if (disappear)
        {
            value -= Time.deltaTime;
            if (value <= 0)
            {
                value = 0;
                disappear = false;
            }
            matTeleport.SetFloat("_Value", value);
        }
        if (appear)
        {
            value += Time.deltaTime;
            if (value >= 1)
            {
                value = 1;
                appear = false;
            }
            matTeleport.SetFloat("_Value", value);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Player into Trigger
            audioManager.Stop();
            audioManager.Play("Boss");
            Player.GetComponent<Player>().StopMoving();
            Player.GetComponent<Animator>().SetBool("Falling", false);
            Player.GetComponent<Player>().enabled = false;
            director.Play();
        }
    }

    public void BeginReduceBlur()
    {
        reduceBlur = true;
    }

    public void Disappear()
    {
        Boss.GetComponent<SpriteRenderer>().material = matHurt;
        disappear = true;
    }

    public void Appear()
    {
        appear = true;
    }

    public void ChangePosition()
    {
        Boss.position = BossPos.position;
        Player.position = PlayerPos.position;
    }

    public void StartBattle()
    {
        Player.GetComponent<SpriteRenderer>().material = def;
        Player.GetComponent<Player>().enabled = true;
        Boss.GetComponent<Boss>().fight = true;
        Boss.GetComponent<Animator>().SetBool("Fight", true);
    }
}
