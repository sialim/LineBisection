using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BisectionController : MonoBehaviour
{
    [SerializeField]
    public Camera mainCamera;
    public GameObject targetObject;
    public LevelManager levelManager;
    public LevelManagerDistract levelManagerDistract;
    
    public SceneManager sceneManager;

    public int levelCount = 9;

    // Mouse
    private bool isDragging = false;
    private Vector3 worldPosition;
    private Vector3 mousePosition;

    // Target
    private float targetAbsoluteBisection;
    private float targetOffset;
    private float neglectValue = 0.0f;

    float screenHeightInWorldUnits;
    float screenWidthInWorldUnits;

    void Start() {
        screenHeightInWorldUnits = mainCamera.orthographicSize * 2f;
        screenWidthInWorldUnits = screenHeightInWorldUnits * mainCamera.aspect;
        targetAbsoluteBisection = targetObject.transform.localScale.x;
        targetOffset = targetObject.transform.position.x;
    }

    void Update()
    {
        if (isDragging) {
        
            mousePosition = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            transform.position = new Vector3(worldPosition.x, transform.position.y, transform.position.z);
        }
        //neglectValue = transform.position.x - targetObject.transform.position.x;
        neglectValue = ((transform.position.x - targetObject.transform.position.x) / screenWidthInWorldUnits) * 100f;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }
    void OnMouseUp()
    {
        isDragging = false;
    }

    public void NextLevel() {
        string positionCategory = "Unknown";
        if (levelManager) {
            if (levelManager.currentPositionCategory == 0) {
                positionCategory = "Center";
            } else if (levelManager.currentPositionCategory == 1) {
                positionCategory = "Left";
            } else if (levelManager.currentPositionCategory == 2) {
                positionCategory = "Right";
            }
            float lineLength = targetObject.transform.localScale.x;
            float lineLengthPercentage = (lineLength / screenWidthInWorldUnits) * 100f;
            float lineOffsetFromLeft = (targetObject.transform.position.x - (lineLength / 2)) + (screenWidthInWorldUnits / 2);
            float lineOffsetPercentage = (lineOffsetFromLeft / screenWidthInWorldUnits) * 100f;
            Debug.Log("Turn: " + levelManager.currentLevelIndex +
            "\nNeglect Value: " + neglectValue +
            "%\nLine Length: " + lineLengthPercentage +
            "%\nLine Offset from left: " + lineOffsetPercentage +
            "%\nOrientation: " + positionCategory);
            if (levelManager.currentLevelIndex >= levelCount) {
                sceneManager.LoadSceneByIndex(0);   
                levelManager.currentPatternIndex = 0;
                return;
            }
            levelManager.NextLevel();
        } else if (levelManagerDistract) {
            if (levelManagerDistract.currentPositionCategory == 0) {
                positionCategory = "Center";
            } else if (levelManagerDistract.currentPositionCategory == 1) {
                positionCategory = "Left";
            } else if (levelManagerDistract.currentPositionCategory == 2) {
                positionCategory = "Right";
            }
            float lineLength = targetObject.transform.localScale.x;
            float lineLengthPercentage = (lineLength / screenWidthInWorldUnits) * 100f;
            float lineOffsetFromLeft = (targetObject.transform.position.x - (lineLength / 2)) + (screenWidthInWorldUnits / 2);
            float lineOffsetPercentage = (lineOffsetFromLeft / screenWidthInWorldUnits) * 100f;
            string output = "Turn: " + levelManagerDistract.currentLevelIndex +
            "\nNeglect Value: " + neglectValue +
            "%\nLine Length: " + lineLengthPercentage +
            "%\nLine Offset from left: " + lineOffsetPercentage +
            "%\nOrientation: " + positionCategory;

            string filePath = "data_output.txt";

            WriteToFile(output);

            if (levelManagerDistract.currentLevelIndex >= levelCount) {
                sceneManager.LoadSceneByIndex(0);
                levelManagerDistract.currentPatternIndex = 0;
                return;
            }
            levelManagerDistract.NextLevel();
        }
    }

    void WriteToFile(string content) {
        // Create the folder and file in the game's directory
        string folderPath = Path.Combine(Application.dataPath, "Output"); 
        string filePath = Path.Combine(folderPath, "data_output.txt");

        // Ensure the folder exists
        if (!Directory.Exists(folderPath)) {
        Directory.CreateDirectory(folderPath);
        }

        // Write the content to the file
        using (StreamWriter writer = new StreamWriter(filePath, true)) {
        writer.WriteLine(content);
        }
    }
}
