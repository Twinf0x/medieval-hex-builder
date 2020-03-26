using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiPagePanel : MonoBehaviour
{
    public List<GameObject> pages;
    public Button firstPageButton;
    public Button previousPageButton;
    public Button nextPageButton;
    public Button lastPageButton;
    public TextMeshProUGUI pageCounter;

    private int currentPageIndex;
    private GameObject currentPage;

    private void Awake()
    {
        firstPageButton.onClick.AddListener(() => GoToPage(0));
        previousPageButton.onClick.AddListener(() => PreviousPage());
        nextPageButton.onClick.AddListener(() => NextPage());
        lastPageButton.onClick.AddListener(() => GoToPage(pages.Count - 1));

        GoToPage(0);
    }

    private void GoToPage(int pageIndex)
    {
        if(pageIndex >= pages.Count || pageIndex < 0)
        {
            return;
        }

        currentPage?.SetActive(false);
        currentPage = pages.ElementAt(pageIndex);
        currentPage?.SetActive(true);

        currentPageIndex = pageIndex;
        UpdatePageCounter();
    }

    private void NextPage()
    {
        GoToPage(currentPageIndex + 1);
    }

    private void PreviousPage()
    {
        GoToPage(currentPageIndex - 1);
    }

    private void UpdatePageCounter()
    {
        pageCounter.text = $"{(currentPageIndex + 1).ToString()}/{pages.Count.ToString()}";
    }
}
