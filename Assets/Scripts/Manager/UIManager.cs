using BaseTemplate.Behaviours;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] Canvas _mainCanvas;

    [SerializeField] MenuView _menuView;
    [SerializeField] GameView _gameView;
    [SerializeField] EndView _endView;

    [SerializeField] SacrificePopup _sacrificePopup;

    [SerializeField] FloatingText _floatingText;
    [SerializeField] Transform _floatingTextSpawnTransform;

    [SerializeField] Image _blackShade;

    View _currentView;
    Tweener _blackShadeTweener;

    public Canvas MainCanvas { get => _mainCanvas; }
    public SacrificePopup SacrificePopup { get => _sacrificePopup; }

    public void Init()
    {
        GameManager.Instance.OnGameStateChanged += HandleStateChange;

        InitView();
        InitPopup();

        ChangeView(_gameView);
    }

    public void InitView()
    {
        _menuView.Init();
        _gameView.Init();
        _endView.Init();
    }
    public void InitPopup()
    {
        _sacrificePopup.Init();
    }

    public void AddPopup(Popup popupToAdd)
    {
        popupToAdd.OpenPopup();
    }

    public void ClosePopup(Popup popupToAdd)
    {
        popupToAdd.ClosePopup();
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
            case GAMESTATE.MENU:
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
        ChangeView(_menuView);
    }
    void HandleGame()
    {
        ChangeView(_gameView);
    }
    void HandleEnd()
    {
        ChangeView(_endView);
    }

    #endregion

    public void ShowBlackShade()
    {
        if (_blackShadeTweener.IsActive()) _blackShadeTweener.Kill();

        _blackShadeTweener = _blackShade.DOFade(.8f, .1f);

        _blackShade.raycastTarget = true;
    }

    public void HideBlackShade(bool _instant = true)
    {
        if (_blackShadeTweener.IsActive()) _blackShadeTweener.Kill();

        if (_instant) _blackShadeTweener = _blackShade.DOFade(0f, 0);
        else _blackShadeTweener = _blackShade.DOFade(0f, .1f);

        _blackShade.raycastTarget = false;
    }

    public void DoFloatingText(string text, Color color)
    {
        FloatingText textGO = Instantiate(_floatingText, _floatingTextSpawnTransform);

        textGO.Init(text, color);
    }
}
