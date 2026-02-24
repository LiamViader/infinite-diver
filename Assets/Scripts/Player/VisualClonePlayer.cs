using UnityEngine;
using System.Collections;

public class VisualClonePlayer : MonoBehaviour
{
    public ProgressBar _parachuteProgressBar;
    private Animator _animator;

    public SpriteRenderer _spriteRenderer;
    private Coroutine _flashCoroutine;
    private Color _flashColor = Color.black;
    private Color _initialColor;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _initialColor = Color.white;
    }
    public void SetParachuteProgress(float proportion, bool isOpen)
    {
        float newOpacity = 0f;
        if (isOpen)
        {
            newOpacity = 1f;
        }
        else if (!isOpen)
        {
            newOpacity = 0.4f;
        }
        if (proportion >= 1) newOpacity = 0f;
        _parachuteProgressBar.SetProgress(proportion);
        _parachuteProgressBar.SetOpacity(newOpacity);
    }

    public void OpenParachute()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("OpenParachute");
        }
    }

    public void CloseParachute()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("CloseParachute");
        }
    }

    public void DamageFlash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }
        _spriteRenderer.color = _initialColor;
        StartCoroutine(Flash(1.5f));
    }

    private IEnumerator Flash(float lapseOfTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < lapseOfTime)
        {
            float t = Mathf.PingPong(Time.time * 4f, 1f);
            _spriteRenderer.color = Color.Lerp(_initialColor, _flashColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _spriteRenderer.color = _initialColor;
    }
}
