using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerDistract : MonoBehaviour
{
    public GameObject line;
    public GameObject leftDistraction;
    public GameObject centerDistraction;
    public GameObject rightDistraction;
    public Camera mainCamera;
    public float longLineLengthPercentage = 0.5f;
    public float shortLineLengthPercentage = 0.25f;
    public float lineWidth = 0.125f;
    public int distractionStart = 6;
    [SerializeField] public int currentLevelIndex = 0;

    [SerializeField] private int[] levelPattern = { 0, 1, 2, 3, 4, 5, 4, 3, 2, 1 };
    [SerializeField] public int currentPatternIndex = 0;

    private int leftCount = 0;
    private int centerCount = 0;
    private int rightCount = 0;

    public int currentPositionCategory = 0;
    void Start()
    {
        LoadLevel(levelPattern[currentPatternIndex]);
    }

    public void NextLevel() {
        currentPatternIndex = (currentPatternIndex + 1) % levelPattern.Length;
        currentLevelIndex++;
        LoadLevel(levelPattern[currentPatternIndex]);
        if (currentLevelIndex == distractionStart) {
            //distraction.SetActive(true);
        }
    }

    public void LoadLevel(int currentLevelIndex) {
        if (line) {  // Make sure you're manipulating the correct object

            currentPositionCategory = GetPositionCategory();
            Vector3 objectPos = line.transform.position;
            Vector3 objectScale = line.transform.localScale;

            // Get screen dimensions in world units
            float screenHeightInWorldUnits = mainCamera.orthographicSize * 2f;
            float screenWidthInWorldUnits = screenHeightInWorldUnits * mainCamera.aspect;

            // Calculate the lengths of the objects based on screen width
            float longObjectLength = longLineLengthPercentage * screenWidthInWorldUnits;
            float shortObjectLength = shortLineLengthPercentage * screenWidthInWorldUnits;

            // Decide if the object is short or long
            bool isShort = currentLevelIndex % 2 == 0;
            float objectLength = isShort ? shortObjectLength : longObjectLength;
            objectScale = isShort ? new Vector3(shortObjectLength, lineWidth, 0f) : new Vector3(longObjectLength, lineWidth, 0f);

            // Get the position category (left, center, right)
            int positionCategory = GetPositionCategory();

            // Calculate half of the object length and screen width
            float halfObjectLength = objectLength / 2;
            float halfScreenWidth = screenWidthInWorldUnits / 2;

            // Set the position based on the category (left, center, right)
            if (currentPositionCategory == 0) { // Centered
                if (centerDistraction) centerDistraction.SetActive(true);
                if (leftDistraction) leftDistraction.SetActive(false);
                if (rightDistraction) rightDistraction.SetActive(false);
                centerCount++;
            }
            else if (currentPositionCategory == 1) { // Left of center
                if (leftDistraction) leftDistraction.SetActive(true);
                if (centerDistraction) centerDistraction.SetActive(false);
                if (rightDistraction) rightDistraction.SetActive(false);
                leftCount++;
            }
            else if (currentPositionCategory == 2) { // Right of center
                if (rightDistraction) rightDistraction.SetActive(true);
                if (leftDistraction) leftDistraction.SetActive(false);
                if (centerDistraction) centerDistraction.SetActive(false);
                rightCount++;
            }

            float halfLineLength = objectLength /2f;
            objectPos.x = Random.Range(-halfScreenWidth + halfLineLength, halfScreenWidth - halfLineLength);
            line.transform.position = objectPos;
            line.transform.localScale = objectScale;
        }
    }

    private int GetPositionCategory()
    {
        if (leftCount < Mathf.Floor(levelPattern.Length / 3f))
        {
            return 1;
        }
        else if (centerCount < Mathf.Floor(levelPattern.Length / 3f))
        {
            return 0;
        }
        else if (rightCount < Mathf.Floor(levelPattern.Length / 3f))
        {
            return 2;
        }
        else
        {
            return Random.Range(0, 3);
        }
    }
}
