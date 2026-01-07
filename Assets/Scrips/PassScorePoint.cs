using UnityEngine;

public class PassScorePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.singlenton.AddScore(1);

        FindFirstObjectByType<BallController>().perfectPass++;
    }

}
