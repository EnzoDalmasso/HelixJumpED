using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;//New Input System (Mouse, Touchscreen, etc.)
using Random = UnityEngine.Random;

public class HelixController : MonoBehaviour
{
    // Velocidad de rotación del helix (ajustable desde el Inspector)
    [SerializeField] private float rotationSpeed = 0.15f;

    // Guarda la posición X del frame anterior (mouse o dedo)
    private float lastX;

    // Indica si el usuario está arrastrando actualmente
    private bool isDragging;


    private Vector3 startRotation;

    public Transform topTransform;
    public Transform goalTransform;

    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();

    public float helixDistance;

    private List<GameObject> spawnedLevels = new List<GameObject>();


    private void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
        
    }
    private void Start()
    {
        LoadStage(0);
    }

    void Update()
    {
        if (Pointer.current == null)
            return;

        // PRESIONA (mouse o dedo)
        if (Pointer.current.press.wasPressedThisFrame)
        {
            lastX = Pointer.current.position.ReadValue().x;
            isDragging = true;
        }

        // ARRASTRA
        if (Pointer.current.press.isPressed && isDragging)
        {
            float currentX = Pointer.current.position.ReadValue().x;
            float deltaX = lastX - currentX;

            lastX = currentX;

            transform.Rotate(Vector3.up * deltaX * rotationSpeed);
        }

        // SUELTA
        if (Pointer.current.press.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

   

    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];
        if (stage == null)
        {
            Debug.Log("No stages");
            return;
        }

        Camera.main.backgroundColor = allStages[stageNumber].stageBackGroundColor;
        FindFirstObjectByType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;

        transform.localEulerAngles = startRotation;

        foreach (GameObject go in spawnedLevels)
        {
            Destroy(go);
        }

        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;

            GameObject level = Instantiate(helixLevelPrefab, transform);

            level.transform.localPosition = new Vector3(0, spawnPosY, 0);

            spawnedLevels.Add(level);

            int partsToDisable = 12 - stage.levels[i].partCount;

            List<GameObject> disabledParts = new List<GameObject>();

            while (disabledParts.Count < partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }

            List<GameObject> leftParts = new List<GameObject>();

            foreach (Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;

                if (t.gameObject.activeInHierarchy)
                {
                    leftParts.Add(t.gameObject);
                }

            }
        
            List<GameObject> deathParts = new List<GameObject>();
            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];

                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            }


        }
    }

}
