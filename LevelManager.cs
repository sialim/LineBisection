using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
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

            float screenHeightInWorldUnits = mainCamera.orthographicSize * 2f;
            float screenWidthInWorldUnits = screenHeightInWorldUnits * mainCamera.aspect;

            float longObjectLength = longLineLengthPercentage * screenWidthInWorldUnits;
            float shortObjectLength = shortLineLengthPercentage * screenWidthInWorldUnits;

            bool isShort = currentLevelIndex % 2 == 0;
            float objectLength = isShort ? shortObjectLength : longObjectLength;
            objectScale = isShort ? new Vector3(shortObjectLength, lineWidth, 0f) : new Vector3(longObjectLength, lineWidth, 0f);

            int positionCategory = GetPositionCategory();

            float halfObjectLength = objectLength / 2;
            float halfScreenWidth = screenWidthInWorldUnits / 2;

            // Set the position based on the category (left, center, right)
            if (positionCategory == 0) { // Centered
                objectPos.x = 0f;
                centerCount++;
            }
            else if (positionCategory == 1) { // Left of center
                objectPos.x = Random.Range(-halfScreenWidth + halfObjectLength, 0f - halfObjectLength);
                leftCount++;
            }
            else if (positionCategory == 2) { // Right of center
                objectPos.x = Random.Range(halfObjectLength, halfScreenWidth - halfObjectLength);
                rightCount++;
            }

            // Apply the calculated position and scale to the object
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
