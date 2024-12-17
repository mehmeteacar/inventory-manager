using UnityEngine;

public class HoverGlowOutline : MonoBehaviour
{
    public GameObject outlineObject; // Reference to the outline GameObject
    private bool isHoldingLMB = false; // Tracks if LMB is being held

    void Start()
    {
        // Ensure the outline is initially hidden
        if (outlineObject != null)
            outlineObject.SetActive(false);
    }

    void Update()
    {
        // Update the LMB holding state
        isHoldingLMB = Input.GetMouseButton(0);
    }

    void OnMouseEnter()
    {
        // Show outline
        if (outlineObject != null)
            outlineObject.SetActive(true);
    }

    void OnMouseExit()
    {
        // Hide outline
        if (outlineObject != null)
            outlineObject.SetActive(false);
    }
}