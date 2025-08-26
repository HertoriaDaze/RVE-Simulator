using UnityEngine;
using TMPro;
public class BotInfor : MonoBehaviour
{
    [Header("Display Settings")]
    public int botNumber = 0;
    public Vector3 textOffset = new Vector3(0, 0.5f, 0);
    public float textScale = 0.1f;
    public string sortingLayerName = "Bot"; 
    public float fontSize;
    private TextMeshProUGUI numberDisplay;
    public int sortingOrder = 100;      
    private Canvas canvas;
    
    [Header("Performance")]
    //success coefficient
    public float successCoffA = 1f;
    public float successCoffB = 1f;
    public float successCoffC = 1f;
    public float InitialsuccessCoffA = 1f;
    public float InitialsuccessCoffB = 1f;
    public float InitialsuccessCoffC = 1f;
    public int finishedA = 0;
    public int finishedB = 0;
    public int finishedC = 0;
    public float revenue = 0;
    
    [Header("Calculation Settings")]
    public int Smax = 2;
    public int Smin = 1;
    public float LearningSpeed = 0.5f;
    public int nmax = 10;
    public int nmaxc = 5;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateNumberDisplay();
        UpdateNumberDisplay();
        
    }

    
    void CreateNumberDisplay()
    {
       
        GameObject canvasObj = new GameObject("BotCanvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = Vector3.zero;
        
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        
   
        canvas.sortingLayerName = sortingLayerName; 
        canvas.sortingOrder = sortingOrder;       
        
        canvas.transform.localScale = Vector3.one * 0.03f;

        
        GameObject textObj = new GameObject("BotNumber");
        textObj.transform.SetParent(canvas.transform);
        textObj.transform.localPosition = textOffset;
        
        numberDisplay = textObj.AddComponent<TextMeshProUGUI>();
        numberDisplay.fontSize = fontSize;
        numberDisplay.rectTransform.sizeDelta = new Vector2(1, 0.5f);
        numberDisplay.alignment = TextAlignmentOptions.Center;
    }

    public void SetBotNumber(int number)
    {
        botNumber = number;
        UpdateNumberDisplay();
    }

    void UpdateNumberDisplay()
    {
        if (numberDisplay != null)
        {
            numberDisplay.text = botNumber.ToString();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(finishedA == 0) 
            successCoffA = InitialsuccessCoffA;
        else if (finishedA <= nmax)
        {   
            float exponent = -LearningSpeed*(finishedA - nmaxc);
            successCoffA = Smin + (Smax - Smin) /(1+Mathf.Exp(exponent));
        }
        
        if(finishedB == 0) 
            successCoffB = InitialsuccessCoffB;
        else if (finishedB <= nmax)
        {   
            float exponent = -LearningSpeed*(finishedB - nmaxc);
            successCoffB = Smin + (Smax - Smin) /(1+Mathf.Exp(exponent));
        }
        
        if(finishedC == 0) 
            successCoffC = InitialsuccessCoffC;
        else if (finishedC <= nmax)
        {   
            float exponent = -LearningSpeed*(finishedC - nmaxc);
            successCoffC = Smin + (Smax - Smin) /(1+Mathf.Exp(exponent));
        }
            


    }
}

//test