using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    private UIDocument document;
    private Label titleLabel;
    private bool isAnimating = false;
    
    // Animation parameters
    private const float SCALE_MIN = 0.98f;
    private const float SCALE_MAX = 1.02f;
    private const float ROTATION_MIN = -1f;
    private const float ROTATION_MAX = 1f;
    private const float ANIMATION_SPEED = 1f;

    void Start()
    {
        document = GetComponent<UIDocument>();
        var root = document.rootVisualElement;
        
        // Setup button clicks
        root.Q<Button>("ai-battle-button").clicked += () => SceneManager.LoadScene("screen_heroselect");
        root.Q<Button>("online-battle-button").clicked += () => StartOnlineBattle();

        // Get title reference
        titleLabel = root.Q<Label>(null, "title");
        
        // Start continuous animation
        StartCoroutine(nameof(AnimateTitleContinuously));
    }

    private IEnumerator AnimateTitleContinuously()
    {
        float time = 0;
        isAnimating = true;

        while (isAnimating)
        {
            // Calculate animation values using a sine wave
            float progress = (Mathf.Sin(time * ANIMATION_SPEED) + 1) * 0.5f; // Convert -1,1 to 0,1
            
            // Interpolate scale and rotation
            float scale = Mathf.Lerp(SCALE_MIN, SCALE_MAX, progress);
            float rotation = Mathf.Lerp(ROTATION_MIN, ROTATION_MAX, progress);
            
            // Apply transformations if titleLabel is not null
            if (titleLabel != null)
            {
                titleLabel.style.scale = new StyleScale(new Scale(new Vector3(scale, scale, 1)));
                titleLabel.style.rotate = new StyleRotate(new Rotate(rotation));
                
                // Add some vertical movement
                float yOffset = Mathf.Sin(time * ANIMATION_SPEED * 1.5f) * 5f;
                titleLabel.style.translate = new StyleTranslate(new Translate(0, yOffset, 0));
            }
            
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDisable()
    {
        isAnimating = false;
    }

    private void StartOnlineBattle()
    {
        // TODO: Implement online battle logic
        Debug.Log("Online Battle not implemented yet");
    }
}