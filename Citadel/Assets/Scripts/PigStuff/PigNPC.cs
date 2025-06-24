using UnityEngine;
using TMPro; // Required for TextMeshPro

// This script manages the interaction with an NPC, displaying different messages
// based on whether the player has found all the missing pigs.
public class PigNPC : MonoBehaviour
{
    // Assign your TextMeshProUGUI component in the Unity Editor.
    // This is the UI element where the NPC's dialogue will be displayed.
    public Canvas targetCanvas;
    public TextMeshProUGUI npcDialogueText;


    private const string initialComplaintMessage = "Πρέπει να βρεις τα έξι γουρούνια μου πριν μου πάρουν το κεφάλι! Έχουν σκορπιστεί στο κάστρο. Για να τα φέρεις πίσω, αρκεί να τα ακουπήσεις και να πατήσεις Ζ.";

    private const string thankYouMessage = "Με έσωσες, φίλε! Πάρε 50 νομίσματα ως αμοιβή.";

    // Define the target number of pigs that need to be collected.
    // This should match the total number of pigs in your game.
    private const int TOTAL_PIGS_TO_COLLECT = 6;

    // A flag to ensure the "thank you" message is only set once.
    private bool pigsFound = false;

    // A flag to track if the player is currently within interaction range of the NPC.
    private bool isPlayerInRange = false;
    public bool gotPaid = false;
    // Start is called before the first frame update.
    void Start()
    {
        // Ensure the TextMeshProUGUI component is assigned.
        if (npcDialogueText == null)
        {
            Debug.LogError("NPC Dialogue Text (TextMeshProUGUI) is not assigned in the inspector!", this);
            return;
        }
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame.
    void Update()
    {
        // Check if the player is in range and presses the "Z" key.
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Z))
        {
            // Toggle the visibility of the dialogue box.
            // If it's active, deactivate it; if inactive, activate it.
            bool isActive = targetCanvas.gameObject.activeSelf;
            targetCanvas.gameObject.SetActive(!isActive);
            // If we just activated the dialogue box, update its content.
            if (!isActive) // If it was inactive and now active
            {
                CheckAndSetDialogueMessage();
            }
        }

        // If the dialogue box is currently active, ensure the message is up-to-date
        // in case pigs are collected while the dialogue is open.
        if (npcDialogueText.gameObject.activeSelf)
        {
            CheckAndSetDialogueMessage();
        }
    }

    // This method checks the current count of collected pigs and
    // sets the appropriate dialogue message.
    private void CheckAndSetDialogueMessage()
    {
        // Only update the message if pigs haven't been found yet,
        // or if it's the initial display.
        if (!pigsFound)
        {
            // Assuming PigCollectionManager.Instance.GetCollectedCount() exists
            // and correctly returns the number of collected pigs.
            // Make sure your PigCollectionManager is a Singleton as implied by 'Instance'.
            int collectedPigs = PigCollectionManager.Instance.GetCollectedCount();

            // If the collected pig count reaches the total required,
            // update the message and set the flag.
            if (collectedPigs >= TOTAL_PIGS_TO_COLLECT)
            {
                npcDialogueText.text = thankYouMessage;
                pigsFound = true; // Prevent further updates to this message
                Debug.Log("All pigs found! NPC message updated to 'thank you'.");
                if(!gotPaid)
                {
                   CurrencyManager.Instance.AddMoney(50);
                   gotPaid = true; 
                }
            }
            else if (collectedPigs > 0 && collectedPigs < TOTAL_PIGS_TO_COLLECT)
            {
                int missingPigs = TOTAL_PIGS_TO_COLLECT - collectedPigs;
                npcDialogueText.text = $"Ακόμα λείπουν {missingPigs} γουρούνια. Βιάσου!";
            }
            else
            {
                // If pigs are still missing, display the complaint message.
                npcDialogueText.text = initialComplaintMessage;
            }
        }
    }

    // Called when another collider enters this NPC's trigger collider.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider belongs to the player.
        // You might need to tag your player GameObject with "Player".
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Called when another collider exits this NPC's trigger collider.
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Hide the dialogue box when the player leaves range.
            targetCanvas.gameObject.SetActive(false);
        }
    }
}
