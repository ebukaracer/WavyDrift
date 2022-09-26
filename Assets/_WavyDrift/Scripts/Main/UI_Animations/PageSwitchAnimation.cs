using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles page/panel transitions(fade-in/fade-out)
/// </summary>
internal class PageSwitchAnimation : MonoBehaviour
{
    private CanvasGroup _currentPageCanvasGroup;

    private RectTransform _currentPage;

    private int _currentPageNumber;

    [SerializeField] private List<RectTransform> pages;

    [Space(5), SerializeField] private float fadeDuration;

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
        _currentPage = pages[0];

        _currentPageNumber = 0;
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
    private void SwitchPage(int pageNumber)
    {
        if (_currentPageNumber == pageNumber)
            return;

        // Sets the previous page inactive
        _currentPageCanvasGroup = _currentPage.GetComponent<CanvasGroup>();

        _currentPageCanvasGroup.blocksRaycasts = false;

        _currentPageCanvasGroup.DOFade(0, fadeDuration).OnComplete
        (
            delegate
            {
                // Sets a new page active
                _currentPage = pages[pageNumber];

                _currentPageCanvasGroup = _currentPage.GetComponent<CanvasGroup>();

                _currentPageCanvasGroup.DOFade(1, fadeDuration);

                _currentPageCanvasGroup.blocksRaycasts = true;

                _currentPageNumber = pageNumber;
            }
        );
    }
}
