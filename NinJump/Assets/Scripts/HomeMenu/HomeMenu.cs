using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HomeMenu : MonoBehaviour
{
    [SerializeField] private GameObject par1;
    [SerializeField] private GameObject par2;
    [SerializeField] private Image imageTransition;

    private bool inStartScreen = true;
    private float transitionSpeed = 1000;
    private bool inTransition = false;
    private bool transitionBack = false;
    private bool activePar2 = false;
    
    private void Start()
    {
        par1.SetActive(true);
        par2.SetActive(false);
    }
    
    private void Update()
    {
        if(inStartScreen)
        {
            if (Input.touches.Length > 0)
            {
                inStartScreen = false;
                par1.SetActive(false);
                inTransition = true;
            }
        }

        if (activePar2)
        {
            par2.SetActive(true);
        }

        if (inTransition)
        {
            if (imageTransition.rectTransform.rect.width >= Screen.width - 1000)
            {
                transitionBack = true;
            }
            
            if(!transitionBack)
            {
                imageTransition.rectTransform.sizeDelta = new Vector2(
                    imageTransition.rectTransform.rect.width + transitionSpeed * Time.deltaTime,
                    imageTransition.rectTransform.rect.width + transitionSpeed * Time.deltaTime);
            }
            else
            {
                imageTransition.rectTransform.sizeDelta = new Vector2(
                    imageTransition.rectTransform.rect.width - transitionSpeed * Time.deltaTime,
                    imageTransition.rectTransform.rect.width - transitionSpeed * Time.deltaTime);
            }

            if (imageTransition.rectTransform.rect.width <= 0)
            {
                inTransition = false;
                activePar2 = true;
            }
        }
    }

    public void ButtonLevelPressed(Button button)
    {
        SceneManager.LoadScene(button.name);
    }
}
