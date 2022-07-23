using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles page/panel transitions(fade-in/fade-out)
/// </summary>
class PageSwitchAnimation : MonoBehaviour
{
    CanvasGroup currentPageCanvasGroup;

    RectTransform currentPage;

    int currentPageNumber;

    [SerializeField]
    List<RectTransform> pages;

    [Space(5), SerializeField]
    float fadeDuration;

    private void Awake()
    {
        // Excluding the first page, fade-out/disable other pages.
        for (int i = 1; i < pages.Count; i++)
        {
            var canvasGroup = pages[i].GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;

            canvasGroup.blocksRaycasts = false;
        }

        // Current page set to the first page-item on the [pages] list.
        currentPage = pages[0];

        currentPageNumber = 0;
    }

    /// <summary>
    /// Each page has its unique number assigned to the inspector.
    /// This is assigned to buttons(their callbacks) that'd fade in/out a new page when clicked.
    /// </summary>
    /// <param name="pageNumber">Current page unique number.</param>
    public void Page(int pageNumber)
    {
        SwitchPage(pageNumber);
    }

    /// <summary>
    /// Sets a page active/inactive based on its number.
    /// </summary>
    /// <param name="pageNumber">The current page's number to enable/disable</param>
    void SwitchPage(int pageNumber)
    {
        if (currentPageNumber == pageNumber)
            return;

        // Sets the previous page inactive
        currentPageCanvasGroup = currentPage.GetComponent<CanvasGroup>();

        currentPageCanvasGroup.blocksRaycasts = false;

        currentPageCanvasGroup.DOFade(0, fadeDuration).OnComplete
        (
            delegate
            {
                // Sets a new page active
                currentPage = pages[pageNumber];

                currentPageCanvasGroup = currentPage.GetComponent<CanvasGroup>();

                currentPageCanvasGroup.DOFade(1, fadeDuration);

                currentPageCanvasGroup.blocksRaycasts = true;

                currentPageNumber = pageNumber;
            }
        );
    }
}
