using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;


public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float impulseForce = 3f;
    private bool ignoreNextcollision;

    private Vector3 startPosition;

    public int perfectPass;
    private float SuperSpeed = 10;
    private bool isSuperSpeedActive;
    private int perfectPassCount = 3;

    public GameObject splash;

    public AudioSource collisionAudio;

    private void Start()
    {
        startPosition = transform.position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        collisionAudio.Play();
        addSplash(collision);

        if (ignoreNextcollision)
        {
            return;
        }

        if (isSuperSpeedActive && !collision.transform.GetComponent<GoalController>())
        {
            Destroy(collision.transform.parent.gameObject, 0.2f);
        }
        else
        {

            DeathPart deathPart = collision.gameObject.GetComponent<DeathPart>();
            if (deathPart)
            {
                GameManager.singlenton.RestartLevel();

            }
        }

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextcollision = true;

        Invoke("AllownextCollision", 0.2f);

        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {

        if (perfectPass >= perfectPassCount && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * SuperSpeed, ForceMode.Impulse);
        }
    }


    private void AllownextCollision()
    {
        ignoreNextcollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPosition;
    }

    public void addSplash(Collision collision)
    {
        GameObject newSplash;

        newSplash = Instantiate(splash);

        newSplash.transform.SetParent(collision.transform);

        newSplash.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.11f, this.transform.position.z);

        Destroy(newSplash,3);
    }

}
