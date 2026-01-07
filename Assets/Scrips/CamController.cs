using UnityEngine;

public class CamController : MonoBehaviour
{
    public BallController ball;
    private float offset;

    private void Start()
    {
        offset= transform.position.y-ball.transform.position.y;
    }

    private void Update()
    {
        Vector3 actualPos = transform.position;
        actualPos.y = ball.transform.position.y+offset;
        transform.position = actualPos;
    }


}

