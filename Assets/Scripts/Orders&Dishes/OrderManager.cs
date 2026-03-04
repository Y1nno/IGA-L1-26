using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    private static OrderManager _instance;
    public static OrderManager Instance => _instance;
    public GameObject orderPrefab;
    public GameObject orderPanel;
    public GameObject orderContainer;

    public float orderApprovalReward = 5.0f;
    public float orderPenalty = 0.05f;
    public PlatingStation platingStation;

    public List<GameObject> orders = new List<GameObject>();
    public List<CustomerSO> customers = new List<CustomerSO>();
    private List<CustomerSO> customersAlreadyOrdered = new List<CustomerSO>();
    public List<IngredientDef> possibleOrders = new List<IngredientDef>();
    public int pendingOrders = 10;
    public float orderSpawnMin = 10.0f;
    public float orderSpawnMax = 20.0f;
    public float timeSinceLastOrder = 0.0f;
    private float timeUntilNextOrder = 0.0f;
    [Header("Chef Prompts")]
    public List<ChefPrompts> RandomChefPrompts;
    public ChefPrompts servedWrongPrompt;
    public GameObject chefPromptPanel;
    public float promptSpawnMin = 10.0f;
    public float promptSpawnMax = 20.0f;
    public float timeSinceLastPrompt = 0.0f;
    private float timeUntilNextPrompt = 0.0f;
    private List<ChefPrompts> usedChefPrompts = new List<ChefPrompts>();

    public IngredientDef BLT;
    public ChefPrompts explanationBLT;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            enabled = false;
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Start()
    {
        timeUntilNextOrder = Random.Range(orderSpawnMin, orderSpawnMax);
        timeUntilNextPrompt = Random.Range(promptSpawnMin, promptSpawnMax);
    }

    public void Update()
    {

        timeSinceLastOrder += Time.deltaTime;
        timeSinceLastPrompt += Time.deltaTime;

        if (pendingOrders > 0 && timeSinceLastOrder >= timeUntilNextOrder)
        {
            //Debug.Log("Creating new order...");
            CreateOrder();
            pendingOrders--;
            timeSinceLastOrder = 0.0f;
            timeUntilNextOrder = Random.Range(orderSpawnMin, orderSpawnMax);
        }

        if (timeSinceLastPrompt >= timeUntilNextPrompt)
        {
            AddPrompt();
            timeSinceLastPrompt = 0.0f;
            timeUntilNextPrompt = Random.Range(promptSpawnMin, promptSpawnMax);
        }
    }

    public void CreateOrder()
    {
        if (customers.Count == 0)
        {
            ResetCustomers();
        }
        GameObject newOrder = Instantiate(orderPrefab, orderPanel.transform);
        newOrder.GetComponent<Ticket>().SetOrder(GenerateRandomOrder());
        newOrder.GetComponent<Ticket>().SetCustomer(GenerateRandomCustomer());
        orders.Add(newOrder);
    }

    public void AddPrompt()
    {
        if (RandomChefPrompts.Count == 0)
        {
            return;
        }
        ChefPrompts prompt = RandomChefPrompts[Random.Range(0, RandomChefPrompts.Count)];
        AddPrompt(prompt);
    }

    public void AddPrompt(ChefPrompts prompt)
    {
        RandomChefPrompts.Remove(prompt);
        usedChefPrompts.Add(prompt);
        GameObject newPrompt = Instantiate(chefPromptPanel, orderPanel.transform);
        newPrompt.GetComponent<ChefPrompt>().SetPrompt(prompt);
    }

    public IngredientDef GenerateRandomOrder()
    {
        IngredientDef result = possibleOrders[Random.Range(0, possibleOrders.Count)];
        return result;
    }

    public CustomerSO GenerateRandomCustomer()
    {
        CustomerSO result = customers[Random.Range(0, customers.Count)];
        customers.Remove(result);
        customersAlreadyOrdered.Add(result);
        return result;
    }

    public void ResetCustomers()
    {
        customers.AddRange(customersAlreadyOrdered);
        customersAlreadyOrdered.Clear();
    }

    public void ResolveOrderAtIndex(int index)
    {
        if (index >= 0 && index < orders.Count)
        {
            GameObject orderToResolve = orders[index];
            ResolveOrder(orderToResolve);
        }
    }

    public void ResolveOrder(GameObject order)
    {
        //Debug.Log("Resolving order...");
        //Debug.Log("Boolean check 1: " + (order != null));
        //Debug.Log("Boolean check 2: " + orders.Contains(order));
        if (order != null && orders.Contains(order))
        {
            //Debug.Log("Removing order...");
            RemoveOrder(order);
            //Debug.Log("Grabbing ingredients...");
            List<Ingredient> platedIngredients = platingStation.GetIngredients();
            //Debug.Log("Clearing plating station...");
            platingStation.ClearIngredients();
            //Debug.Log("Evaluating order...");
            EvaluateOrder(order, platedIngredients);
        }
        if (pendingOrders <= 0 && orders.Count == 0)
        {
            OnFinish();
        }
    }

    public void ResolvePrompt(GameObject prompt)
    {
        Destroy(prompt);
    }

    public void EvaluateOrder(GameObject order, List<Ingredient> platedIngredients)
    {
        if (GetComponent<TutorialSignaler>())
        {
            GetComponent<TutorialSignaler>().Signal(LevelSignal.ResolvedFirstOrder);
        }
        if (platedIngredients.Count == 1 && platedIngredients[0].definition == order.GetComponent<Ticket>().order)
        {
            PatienceManager.Instance.IncreasePatience(orderApprovalReward);
        }
        else
        {
            AffectionManager.Instance.DecreaseAffection(orderPenalty);
            AddPrompt(servedWrongPrompt);
        }
    }

    public void RemoveOrder(GameObject order)
    {
        if (orders.Contains(order))
        {
            orders.Remove(order);
            Destroy(order);
        }
    }

    public GameObject GetOrder(int index)
    {
        if (index >= 0 && index < orders.Count)
        {
            return orders[index];
        }
        return null;
    }

    public void RemoveFirstOrder()
    {
        if (orders.Count > 0)
        {
            GameObject firstOrder = orders[0];
            orders.RemoveAt(0);
            Destroy(firstOrder);
        }
    }

    public void StartTakingOrders()
    {
        if (orderContainer.activeSelf == false)
        {
            orderContainer.SetActive(true);
        }
        PatienceManager.Instance.ResetPatience();
        PatienceManager.Instance.StartDecay();
        PatienceManager.Instance.patienceBar.gameObject.SetActive(true);
    }

    public void OnFinish()
    {
        switch(GlobalGameManager.Instance.currentLevelName)
        {
            case "Tutorial Level":
                DialogueManager.Instance.StartDialogue(DialogueInstances.TutorialFinish);
                break;
            case "Level 1":
                DialogueManager.Instance.StartDialogue(DialogueInstances.Level1Finish);
                break;
            // Add more cases as needed for other levels
            default:
                // Handle default case
                break;
        }
    }
}