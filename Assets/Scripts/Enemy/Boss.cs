using System.Security.Cryptography;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject[] weapons;
    public LineRenderer lineLeft, lineRight;
    public Transform player;
    public Material hurt, disappear;
    public GameUI UI;
    public Win win;
    public bool fight = false;
    public int health = 400;

    private Transform leftHand, rightHand, bothHand;
    private AudioManager audioManager;
    private bool left, right, both, damage;
    private GameObject[] m_weapon = new GameObject[10];
    private float mainTimer = 0, HurtTimer, releaseTimer, weaponTimer;
    private int[] weaponTable = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    private int weaponIndex = 0;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        leftHand = transform.Find("LeftHandPosition");
        rightHand = transform.Find("RightHandPosition");
        bothHand = transform.Find("BothHand");
        lineLeft.enabled = false;
        lineRight.enabled = false;
        left = right = both = false;
    }

    void Update()
    {   
        if (health <= 0) return;
        mainTimer += Time.deltaTime;
        if (fight)  UI.UpdateBossHP(health);
        if (mainTimer >= HurtTimer) hurt.SetInt("_Hurt", 0);


        if (left)
        {
            // Left Laser
            lineLeft.enabled = true;
            leftHand.Find("Particle").gameObject.SetActive(true);
            leftHand.Find("Beam").gameObject.SetActive(true);
            leftHand.Rotate(new Vector3(0,0,-90) * Time.deltaTime);
            RaycastHit2D hit = Physics2D.Raycast(leftHand.position, leftHand.up, 80f, LayerMask.GetMask("Player"));
            if (hit && hit.collider.name == "Player")
            {
                player.GetComponent<Player>().GetHit(20);             
            }
        }
        else if (right)
        {
            // Right Laser
            lineRight.enabled = true;
            rightHand.transform.Find("Particle").gameObject.SetActive(true);
            rightHand.transform.Find("Beam").gameObject.SetActive(true);
            rightHand.Rotate(new Vector3(0,0,90) * Time.deltaTime);
            RaycastHit2D hit = Physics2D.Raycast(rightHand.position, rightHand.up, 80f, LayerMask.GetMask("Player"));
            if (hit && hit.collider.name == "Player")
            {
                player.GetComponent<Player>().GetHit(20);               
            }
        }
        else if (both)
        {
            // Rotate the Weapons
            if (mainTimer < weaponTimer)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (m_weapon[i] == null) continue;
                    
                    Vector2 a = m_weapon[i].transform.position - player.position;
                    float z = 180 - (Mathf.Rad2Deg * Mathf.Atan(a.x/a.y));
                    m_weapon[i].transform.eulerAngles = new Vector3(0, 0, z);
                }  
                releaseTimer = mainTimer;              
            }

            // Release
            if (mainTimer >= weaponTimer)
            {
                if (mainTimer < releaseTimer)
                    return;
                
                m_weapon[weaponTable[weaponIndex]].GetComponent<EnemyWeapon>().move = true;
                releaseTimer = mainTimer + 0.3f;

                if (weaponIndex == 9) both = false;
                weaponIndex++;
            }
        }
    }

    public void AttackLeftHand()
    {
        if (!fight) return;
        //CameraShake.Instance.Shake(2, 1.92f);
        left = true;
        audioManager.Play("laser");
    }

    public void AttackRightHand()
    {
        if (!fight) return;
        //CameraShake.Instance.Shake(2, 1.92f);
        right = true;
        audioManager.Play("laser");
    }

    public void AttackBothHand()
    {
        if (!fight) return;
        for (int i =- 5; i < 5; i++)
        {
            m_weapon[5 + i] = Instantiate(weapons[Random.Range(0, 15)], new Vector3(bothHand.position.x + i * 3, bothHand.position.y), Quaternion.identity) as GameObject;
            m_weapon[5 + i].AddComponent<EnemyWeapon>();
        }

        for (int i = 0; i < weaponTable.Length - 1; i++)
        {
            int j = Random.Range(i, weaponTable.Length);
            int temp = weaponTable[i];
            weaponTable[i] = weaponTable[j];
            weaponTable[j] = temp;
        }

        both = true;
        weaponIndex = 0;
        weaponTimer = mainTimer + 1.2f;
    }

    private void Release(int i)
    {
        m_weapon[weaponTable[i]].GetComponent<EnemyWeapon>().move = true;
    }

    public void AttackEnd()
    {
        if (!fight) return;
        audioManager.Stop("laser");
        leftHand.rotation = Quaternion.identity;
        rightHand.rotation = Quaternion.identity;
        leftHand.transform.Find("Particle").gameObject.SetActive(false);
        leftHand.transform.Find("Beam").gameObject.SetActive(false);
        rightHand.transform.Find("Particle").gameObject.SetActive(false);
        rightHand.transform.Find("Beam").gameObject.SetActive(false);
        lineLeft.enabled = false;
        lineRight.enabled = false;
        left = right = both = false;
    }

    public void GetHit(int damage)
    {
        if (!fight) return;
        health -= damage;
        UI.UpdateBossHP(health);

        if (health <= 0)
        {
            End();
        }
        else
        {
            hurt.SetInt("_Hurt", 1);
            HurtTimer = mainTimer + 0.6f;            
        }

    }

    public void End()
    {
        win.WinGame();
        fight = false;
        Destroy(GetComponent<GameObject>());
    }
}
