using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI bestScoreText;

    public Slider slider;

    public TextMeshProUGUI actualLevel;
    public TextMeshProUGUI nextlLevel;

    public Transform topTransform;
    public Transform goalTransform;

    public Transform ball;

    // Update is called once per frame
    void Update()
    {
        currentScoreText.text= "Score: "+ GameManager.singlenton.currentScore;

        bestScoreText.text = "Best: " + GameManager.singlenton.bestScore;

        ChangeSliderLevelProgress();

    }


    public void ChangeSliderLevelProgress()
    {
        actualLevel.text = "" + (GameManager.singlenton.currentLevel+1);
        nextlLevel.text = "" + (GameManager.singlenton.currentLevel +2);

        float totalDistance = (topTransform.position.y - goalTransform.position.y);

        float distanceLeft = totalDistance-(ball.position.y-goalTransform.position.y);

        float value = (distanceLeft / totalDistance);

        slider.value = Mathf.Lerp(slider.value, value, 5);

    }
}
