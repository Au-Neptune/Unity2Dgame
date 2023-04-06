using UnityEngine;

public class Rotate : MonoBehaviour
{
    float degree;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            degree = transform.eulerAngles.z + 90f;
        }

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, degree), 30 * Time.deltaTime);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, degree), Time.deltaTime * 30);
    }
}
