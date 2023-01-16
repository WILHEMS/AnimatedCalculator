using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{
    public VerticalLayoutGroup buttonGroup;
    public HorizontalLayoutGroup bottomRow;
    public RectTransform canvasRect;

    public Text label;

    public Text digitLabel;
    public Text operatorLabel;
    bool errorDisplayed;
    bool displayValid;
    bool specialAction;
    double currentVal;
    double storedVal;
    double result;
    char storedOperator;

    bool canvasChanged;

    public Manager calcManager
    {
        get{
            if (_calcManager==null)
                _calcManager = GetComponentInParent<Manager>();
            return _calcManager;
        }
    }
    static Manager _calcManager ;


    private void Awake() {
        
    }
    public void onTapped()
    {
        Debug.Log("Tapped: " + label.text);
        calcManager.buttonTapped(label.text[0]);
    }
    // Start is called before the first frame update
    void Start()
    {
        canvasChanged = true;
        buttonTapped('c');
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasChanged)
        {
            canvasChanged = false;
        }
        
    }
    private void OnRectTransformDimensionsChange()
    {
        canvasChanged = true;
    }
    void clearCalc()
    {
        digitLabel.text = "0";
        operatorLabel.text = "";
        specialAction = displayValid = errorDisplayed = false;
        currentVal = result = storedVal = 0;
        storedOperator = ' ';
    }
    void updateDigitLabel()
    {
        if (!errorDisplayed)
        {
            digitLabel.text = currentVal.ToString();
        }
        displayValid = false;
    }
    void calcResult(char activeOp)
    {
        switch (activeOp)
        {
            case '=':
                result = currentVal;
                break;
            case '+':
                result = storedVal + currentVal;
                break;
            case '-':
                result = storedVal -  currentVal;
                break;
            case 'x':
                result = storedVal * currentVal;
                break;
            case '÷':
                if (currentVal !=0)
                {
                    result = storedVal / currentVal;
                }
                else
                {
                    errorDisplayed = true;
                    digitLabel.text = "ERROR";
                }
                break;
            default:
                Debug.Log("unknown: " + activeOp);
                break;
        }
        currentVal = result;
        updateDigitLabel();
        //!Trying to remove the equal sign after successfull calculation
        // operatorLabel.text = ' '.ToString();
    }
    public void buttonTapped(char caption)
    {
        if (errorDisplayed)
            clearCalc();
        if ((caption>='0' && caption<='9') || caption == '.')
        {
            if (digitLabel.text.Length<15 || !displayValid)
            {
                if(!displayValid)
                    digitLabel.text = (caption == '.' ? "0" : "");
                else if (digitLabel.text == "0" && caption!='.')
                    digitLabel.text = "";
                digitLabel.text += caption;
                displayValid = true;
            }
        }
        else if (caption == 'c')
        {
            currentVal =-  double.Parse(digitLabel.text);
            updateDigitLabel();
            clearCalc();
            specialAction = true;
        }
        else if (caption == '%')
        {
            currentVal = double.Parse(digitLabel.text) / 100.0d;
            updateDigitLabel();
            specialAction = true;
        }
        else if (displayValid || storedOperator == '=' || specialAction)
        {
            currentVal = double.Parse(digitLabel.text);
            displayValid = false;
            if(storedOperator != ' ')
            {
                calcResult(storedOperator);
                storedOperator = ' ';
            }
            operatorLabel.text = caption.ToString();
            storedOperator = caption;
            storedVal = currentVal;
            updateDigitLabel();
            specialAction = false;
        }
    }
    
}
