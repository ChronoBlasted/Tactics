using BaseTemplate.Behaviours;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] Canvas _mainCanvas;


    [Header("Black Shade Ref")]
    [SerializeField] Button _blackShadeButton;
    [SerializeField] Image _blackShadeImg;

    View _currentView;

    public Canvas MainCanvas { get => _mainCanvas; }


    Tweener _blackShadeTweener;

    public void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleStateChange;

        InitView();

        //ChangeView(_startView);
    }

    public void InitView()
    {
        HideBlackShade();
    }

    public void ChangeView(View newPanel)
    {
        if (newPanel == _currentView) return;

        if (_currentView != null)
        {
            CloseView(_currentView);
        }

        _currentView = newPanel;
        _currentView.gameObject.SetActive(true);

        _currentView.OpenView();

    }

    void CloseView(View newPanel)
    {
        newPanel.CloseView();
    }

    #region GameState

    void HandleStateChange(GAMESTATE newState)
    {
        switch (newState)
        {
            case GAMESTATE.START:
                HandleMenu();
                break;
            case GAMESTATE.GAME:
                HandleGame();
                break;
            case GAMESTATE.END:
                HandleEnd();
                break;
            default:
                break;
        }
    }

    void HandleMenu()
    {
        //ChangeView(_startView);
    }
    void HandleGame()
    {
        //ChangeView(_gameView);
    }
    void HandleEnd()
    {
        // ChangeView(_endView);
    }

    #endregion

    public void ShowBlackShade()
    {
        if (_blackShadeTweener.IsActive()) _blackShadeTweener.Kill();

        _blackShadeTweener = _blackShadeImg.DOFade(1, .5f);

        _blackShadeImg.raycastTarget = true;
    }

    public void HideBlackShade(bool _instant = true)
    {
        if (_blackShadeTweener.IsActive()) _blackShadeTweener.Kill();

        if (_instant) _blackShadeTweener = _blackShadeImg.DOFade(0f, 0);
        else _blackShadeTweener = _blackShadeImg.DOFade(0f, .5f);

        _blackShadeImg.raycastTarget = false;
    }
}
