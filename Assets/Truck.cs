using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour
{
    public int numHousePassed = 0;
    private GameFloor _gameFloor;
    [SerializeField] private float moveSpeedSlowRate = 0.5f;
    public bool slowed = false;

    [SerializeField] private Image exclaimationMark;
    private bool pendingDecision = false;
    private BinItemsAllocator currentBin;

    private RoadGenerator _roadGenerator;
    
    void Start()
    {
        _gameFloor = FindObjectOfType<GameFloor>();
        _roadGenerator = FindObjectOfType<RoadGenerator>();
    }

    void Update()
    {
        // Near bin
        if (!slowed || !pendingDecision) return;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            CollectItems();
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            RejectItems();
        }
    }

    void CollectItems()
    {
        if (currentBin == null)
        {
            Debug.LogError("Can't locate BinAllocator!");
            return;
        }

        currentBin.CollectBin();
        LeaveHouse();
    }

    void RejectItems()
    {
        if (currentBin == null)
        {
            Debug.LogError("Can't locate BinAllocator!");
            return;
        }

        foreach (var collectedItem in currentBin.binItems)
        {
            if (collectedItem.Equals("A") || collectedItem.Equals("B") || collectedItem.Equals("C"))
            {
                // missed out. shame
            }
            else
            {
                // avoided. well done
            }
        }

        LeaveHouse();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "House")
        {
            Debug.Log("House entered: " + numHousePassed);
            _gameFloor.moveSpeed *= moveSpeedSlowRate;
            currentBin = col.GetComponentInChildren<BinItemsAllocator>();
            slowed = true;
            exclaimationMark.enabled = true;
            pendingDecision = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (pendingDecision)
        {
            // player hasn't made a choice. apply penalty
            GameManager.Instance.SubtractScore(20);
        }
        if (!slowed) return;
        
        // Undo slow as truck leaves house
        if (col.gameObject.tag == "House")
        {
            LeaveHouse();
        }

    }

    // return to normal speed, update variables
    void LeaveHouse()
    {
        
        _gameFloor.moveSpeed /= moveSpeedSlowRate;
        currentBin = null;
        
        int remainingHouseCount = _roadGenerator.housesQueue.Count - 1;
        Debug.Log("House passed: " + ++numHousePassed + ". Remaining: " + remainingHouseCount);

        if (numHousePassed == 30)
        {
            Debug.Log("Last house passed. Game over.");
            GameManager.Instance.GameOver();
        }
        else
        {
            _roadGenerator.housesQueue.Dequeue();
        }
        slowed = false;
        exclaimationMark.enabled = false;
        pendingDecision = false;
    }
}
