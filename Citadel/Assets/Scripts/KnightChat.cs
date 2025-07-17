using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator[] animatorsToControl; // Drag your two animated objects here
    
    [Header("UI Settings")]
    public Canvas targetCanvas; // The canvas to show/hide
    
    [Header("Interaction Settings")]
    public KeyCode interactionKey = KeyCode.Z;
    public float interactionRange = 3f; // How close player needs to be
    
    private Transform player;
    private bool isPlayerInRange = false;
    private bool isCanvasActive = false;
    private bool[] originalAnimatorStates; // Store original enabled states
    private bool isWaitingForAnimationEnd = false;
    
    void Start()
    {
        // Find the player - adjust the tag as needed
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Store original animator states
        originalAnimatorStates = new bool[animatorsToControl.Length];
        for (int i = 0; i < animatorsToControl.Length; i++)
        {
            originalAnimatorStates[i] = animatorsToControl[i].enabled;
        }
        
        // Make sure canvas starts hidden
        if (targetCanvas != null)
            targetCanvas.gameObject.SetActive(false);
    }
    
    void Update()
    {
        CheckPlayerDistance();
        HandleInput();
        CheckAnimationCompletion();
    }
    
    void CheckPlayerDistance()
    {
        if (player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        bool wasInRange = isPlayerInRange;
        isPlayerInRange = distance <= interactionRange;
        
        // If player moved out of range and canvas is active, hide it
        if (wasInRange && !isPlayerInRange && isCanvasActive)
        {
            HideCanvasAndResumeAnimations();
        }
    }
    
    void HandleInput()
    {
        if (!isPlayerInRange || isWaitingForAnimationEnd) return;
        
        if (Input.GetKeyDown(interactionKey))
        {
            if (isCanvasActive)
            {
                HideCanvasAndResumeAnimations();
            }
            else
            {
                StartAnimationPause();
            }
        }
    }
    
    void StartAnimationPause()
    {
        isWaitingForAnimationEnd = true;
        // Let current animations finish naturally
    }
    
    void CheckAnimationCompletion()
    {
        if (!isWaitingForAnimationEnd) return;
        
        bool allAnimationsCompleted = true;
        
        foreach (Animator animator in animatorsToControl)
        {
            if (animator != null)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                // Check if animation is still playing (normalized time < 1.0 means not finished)
                if (stateInfo.normalizedTime < 1.0f)
                {
                    allAnimationsCompleted = false;
                    break;
                }
            }
        }
        
        if (allAnimationsCompleted)
        {
            ShowCanvasAndPauseAnimations();
            isWaitingForAnimationEnd = false;
        }
    }
    
    void ShowCanvasAndPauseAnimations()
    {
        // Show canvas
        if (targetCanvas != null)
            targetCanvas.gameObject.SetActive(true);
        
        // Pause all animations and set to first frame
        foreach (Animator animator in animatorsToControl)
        {
            if (animator != null)
            {
                animator.speed = 0f; // Pause animation
                animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0f); // Go to first frame
            }
        }
        
        isCanvasActive = true;
    }
    
    void HideCanvasAndResumeAnimations()
    {
        // Hide canvas
        if (targetCanvas != null)
            targetCanvas.gameObject.SetActive(false);
        
        // Resume all animations
        foreach (Animator animator in animatorsToControl)
        {
            if (animator != null)
            {
                animator.speed = 1f; // Resume normal speed
            }
        }
        
        isCanvasActive = false;
        isWaitingForAnimationEnd = false; // Reset waiting state
    }
    
    // Optional: Draw the interaction range in the scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}