using System.Collections;
using _Scripts.Sounds;
using _Scripts.UI;
using UnityEngine;

public class LoseWindow : MonoBehaviour, IFinishable
{
    [SerializeField] private UIMover[] _uiMovers;
    [SerializeField] private float _delayBetweenMove;
    [SerializeField] private GameObject[] _objectForDisable;

    private WaitForSeconds _delay;

    private void Start()
    {
        _delay = new WaitForSeconds(_delayBetweenMove);
    }

    public void StartActionOnWin()
    {
        
    }

    public void StartActionOnLose()
    {
        StartCoroutine(MoveUI());
    }

    private IEnumerator MoveUI()
    {
        for (int i = 0; i < _objectForDisable.Length; i++)
        {
            _objectForDisable[i].SetActive(false);
        }
        for (int i = 0; i < _uiMovers.Length; i++)
        {
            _uiMovers[i].Move();
            yield return _delay;
        }
    }
}
