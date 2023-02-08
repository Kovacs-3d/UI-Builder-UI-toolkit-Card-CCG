using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

public class CardUiController : MonoBehaviour
{
    private UIDocument _doc;
    private VisualTreeAsset tempAsset;
    private VisualElement cardRoot;
    private List<VisualElement> cardList = new List<VisualElement>();

    private VisualElement enemyRoot;
    private List<VisualElement> enemyList = new List<VisualElement>();

    [SerializeField]
    private int cardNumber = 3;
    [SerializeField]
    private int enemyNumber = 1;

    [Button("Draw cards")]
    private void TestDrawCards() {
        DrawCards();
    }

    [Button("Draw Enemy")]
    private void TestDrawEnemy()
    {
        DrawEnemy();
    }

    private void Start()
    {
        _doc = this.GetComponent<UIDocument>();
        tempAsset = _doc.visualTreeAsset;
        cardRoot = _doc.rootVisualElement[0];
        cardRoot.Q<VisualElement>("CardContainer").RemoveFromHierarchy();

        enemyRoot = _doc.rootVisualElement[1];
        enemyRoot.Q<VisualElement>("Enemy").RemoveFromHierarchy();
    }

    private void DrawCards()
    {
        //Remove All Card Ui Element
        foreach (VisualElement oldCards in cardList)
            oldCards.RemoveFromHierarchy();

        cardList.Clear();

        //Create Cads Ui
        for (int i = 0; i < cardNumber; i++)
        {
            VisualElement CardElement = tempAsset.CloneTree().Q<VisualElement>("CardContainer");
            CardElement.AddToClassList("Hide");
            CardElement.AddToClassList("CardStartAnimation");
            CardElement.Q<Label>("CardInfo").text = "Card" + i;
            cardRoot.Add(CardElement);
            cardList.Add(CardElement);
        }

        //Start Card Animation
        for (int i = 0; i < cardList.Count; i++)
        {
            StartCoroutine(ElementAnimation(cardList[i], i + 1, "Hide,CardStartAnimation"));
        }

        //Add Click event
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].RegisterCallback<PointerDownEvent, int>(OnClick, i);
        }
    }

    private void OnClick(PointerDownEvent evt, int index)
    {
        //Ramove all cards click event
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].UnregisterCallback<PointerDownEvent, int>(OnClick);
        }

        //Start all card down animation
        for (int i = cardList.Count - 1; i >= 0; i--)
        {
            StartCoroutine(ElementAnimation(cardList[i], i + 1, "Hide,CardStartAnimation"));
        }

        Debug.Log($"Click {index}");
    }

    //Animation Tweet
    public IEnumerator ElementAnimation(VisualElement element, int waitsecond, string animationList)
    {
            string[] instructionsList = animationList.Split(",");
            yield return new WaitForSeconds(waitsecond * 0.25f); //Offset Animation start
            foreach (string instruction in instructionsList)
            {
                element.ToggleInClassList(instruction);
            }
    }

    void DrawEnemy()
    {
        //Remove All Enemy Ui Element
        foreach (VisualElement oldEnemy in enemyList)
            oldEnemy.RemoveFromHierarchy();

        enemyList.Clear();

        //Create Enemys Ui
        for (int i = 0; i < enemyNumber; i++)
        {
            VisualElement EnemyElement = tempAsset.CloneTree().Q<VisualElement>("Enemy");
            EnemyElement.AddToClassList("Hide");
            EnemyElement.AddToClassList("EnemyStartAnimation");
            
            enemyRoot.Add(EnemyElement);
            enemyList.Add(EnemyElement);
            enemyList[i].RegisterCallback<PointerDownEvent, int>(SelectEnemy, i);

            //Automatic first enemy selected
            if (i == 0)
                enemyList[i].Q<VisualElement>("EnemySelect").RemoveFromClassList("Hide");
            else
            {
                enemyList[i].Q<VisualElement>("EnemySelect").AddToClassList("Hide");
                enemyList[i].Q<VisualElement>("EnemySelect").AddToClassList("ScaleAnimation");
            }
        }

        //Enemys start animation 
        for (int i = 0; i < enemyList.Count; i++)
        {
            StartCoroutine(ElementAnimation(enemyList[i], i + 1, "Hide,EnemyStartAnimation"));
        }
    }

    private void SelectEnemy(PointerDownEvent evt, int index)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if(index == i) { 
                enemyList[index].Q<VisualElement>("EnemySelect").RemoveFromClassList("Hide");
                enemyList[index].Q<VisualElement>("EnemySelect").RemoveFromClassList("ScaleAnimation");
            }
            else
            {
                enemyList[i].Q<VisualElement>("EnemySelect").AddToClassList("Hide");
                enemyList[i].Q<VisualElement>("EnemySelect").AddToClassList("ScaleAnimation");
            }
        }
        
        Debug.Log($"enemy {index}");
    }
}
