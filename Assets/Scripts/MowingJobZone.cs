using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class MowingJobZone : MonoBehaviour
{
    // This class represents a mowing job zone in the game. It can be used to define areas where mowing jobs can be performed.

    [Header("UI EEMENTS")]
    public GameObject jobUI; // Reference to the UI element for the job
    public TextMeshProUGUI residentNameText; // Text element to display the name of the resident
    public TextMeshProUGUI jobStatusText; // Text element to display the status of the job
    public TextMeshProUGUI jobPriceText; // Text element to display the price of the job
    public TextMeshProUGUI progressText; // Text element to display the progress of the job
    public TextMeshProUGUI startPromptText; // Text element to prompt the player to start the job

    [Header("Job Settings")]
    public string residentName = "John Doe"; // Name of the resident for the job
    public float jobPrice = 50f; // Price for the mowing job

    private List<MowableGrass> grassBlades = new List<MowableGrass>(); // List to hold the grass blades in the job zone
    private int totalGrass = 0; // Total number of grass blades in the job zone
    private int grassCut = 0; // Number of grass blades cut in the job zone

    private bool playerInside = false; // Flag to check if the player is inside the job zone
    private bool jobStarted = false; // Flag to check if the job has started
    private bool jobCompleted = false; // Flag to check if the job is completed
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (jobUI != null)
        {
            jobUI.SetActive(false); // Hide the job UI at the start
        }
    }

    public void RegisterGrass(MowableGrass blade)
    {
        if (!grassBlades.Contains(blade))
        {
            grassBlades.Add(blade); // Add the grass blade to the list if not already present
            totalGrass++; // Increment the total grass count
        }
    }

    public void NotifyGrassCut(MowableGrass blade)
    {
        grassCut++;
        UpdateProgress();

        if (grassCut >= totalGrass)
        {
            CompleteJob(); // Complete the job if all grass has been cut
        }
    }

    void UpdateProgress()
    {
        float percent = (float)grassCut / totalGrass; // Calculate the percentage of grass cut
        int percentRounded = Mathf.RoundToInt(percent * 100); // Round the percentage to the nearest integer

        if (progressText != null)
        {
            progressText.text = percentRounded + "%"; // Update the progress text
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInside && !jobStarted && !jobCompleted && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartJob(); // Start the job when the player presses the 'E' key
        }
    }

    private void StartJob()
    {
        jobStarted = true; // Set job started flag
        startPromptText.gameObject.SetActive(false); // Hide the start prompt text 
        jobStatusText.text = "Lawn Mowing: In Progress"; // Update job status text
       
    }

    private void CompleteJob()
    {
        jobCompleted = true; // Set job completed flag
        jobStatusText.text = "Lawn Mowing: Completed"; // Update job status text
        jobPriceText.text = $"Paid: ${jobPrice:0}"; // Update job price text
        startPromptText.gameObject.SetActive(false); // Hide the start prompt text

        PlayerMoney playerMoney = FindAnyObjectByType<PlayerMoney>(); // Find the PlayerMoney component in the scene
        if (playerMoney != null)
        {
            playerMoney.AddMoney(jobPrice); // Add the job price to the player's money
        }

        Invoke(nameof(BeginAllGrassRegrowth), 6f); // Start regrowth after a delay
    }

    private void CheckGrassRegrown()
    {
        foreach (var  blade in grassBlades) 
        {
            if (blade.isCut || blade.IsStillGrowing()){
                return; // If any grass blade is still cut or growing, do not reset the job
            }
            
        }

        CancelInvoke(nameof(CheckGrassRegrown)); // Stop checking for regrowth
        ResetJob(); // Reset the job
        Debug.Log("All grass has regrown, job reset."); // Log the reset action
    }

    public void ResetJob()
    {
        grassCut = 0;
        jobCompleted = false;
        jobStarted = false;

        jobStatusText.text = "Lawn Mowing: Needs Mowed"; // Reset job status text  
        jobPriceText.text = $"Price: ${jobPrice:F0}"; // Reset job price text
        progressText.text = "0%"; // Reset progress text    

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            jobUI.SetActive(true);

            residentNameText.text = $"{residentName} Residence";

            if (jobCompleted)
            {
                jobStatusText.text = "Job Complete!";
                jobPriceText.text = $"Paid: ${jobPrice:F0}";
                startPromptText.gameObject.SetActive(false);
            }
            else if (jobStarted)
            {
                jobStatusText.text = "In Progress";
                startPromptText.gameObject.SetActive(false);
            }
            else
            {
                jobStatusText.text = "Lawn Mowing: Needs Mowed";
                jobPriceText.text = $"Price: ${jobPrice:F0}";
                startPromptText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (!jobStarted || jobCompleted)
            {
                jobUI.SetActive(false);
                startPromptText.gameObject.SetActive(false);
            }
        }
    }

    void BeginAllGrassRegrowth()
    {
        Debug.Log("All grass starting to regrow..."); // Log the start of regrowth

        foreach (var blade in grassBlades)
        {
            blade.BeginRegrowthFromZone(); // Start regrowth for each grass blade in the zone
        }

        InvokeRepeating(nameof(CheckGrassRegrown), 5f, 5f); // Start checking for regrowth every second 
    }
}



