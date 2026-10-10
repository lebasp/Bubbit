using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialView : MonoBehaviour 
{
    [SerializeField]
    GameObject m_tutorialGroup;

    [SerializeField]
    GameObject m_nextButton;

    [SerializeField]
    GameObject m_prevButton;

    [SerializeField]
    ColorBlock m_activeStepIndicatorColor;

    [SerializeField]
    ColorBlock m_stepIndicatorColor;

    [SerializeField]
    Button[] m_stepIndicator;

    [SerializeField]
    ITutorialStep[] m_steps;

    [SerializeField]
    Text m_tutorialText;

    [SerializeField]
    Text m_tipText;

    int currentStep = -1;

    public void ShowTutorial()
    {        

        m_tutorialGroup.SetActive(true);
        EnableStepIndex(0);
    }

    public void Hidetutorial()
    {
        m_tutorialGroup.SetActive(false);
    }

    public void NextStep()
    {
        if (currentStep < m_steps.Length -1)
        {
            EnableStepIndex(currentStep + 1);
        }
            
    }

    public void PrevStep()
    {
        if (currentStep > 0)
        {
            EnableStepIndex(currentStep - 1);            
        }
    }

    void DisableStepIndex(int index)
    {
        m_steps[index].Disable();
        m_stepIndicator[index].colors = m_stepIndicatorColor;
    }

    public void EnableStepIndex(int index)
    {
        if (currentStep >= 0)
            DisableStepIndex(currentStep);
        
        currentStep = index;

        m_steps[index].Enable();
        m_stepIndicator[index].colors = m_activeStepIndicatorColor;
        m_tutorialText.text = m_steps[index].Text;
        m_tipText.text = m_steps[index].Tip;


        if (currentStep == m_steps.Length - 1)
            m_nextButton.SetActive(false);
        else
            m_nextButton.SetActive(true);

        if (currentStep == 0)
            m_prevButton.SetActive(false);
        else
            m_prevButton.SetActive(true);
        
    }
}
