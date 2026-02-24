using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressImage;
    public Image backgroundProgressImage;

    public void SetProgress(float proportion)
    {
        proportion=Mathf.Clamp(proportion, 0.0f, 1.0f);
        progressImage.fillAmount = proportion;
    }

    public void SetOpacity(float opacity)
    {
        Color currentProgressColor = progressImage.color;
        currentProgressColor.a = opacity;
        progressImage.color = currentProgressColor;

        Color currentBackgroundProgressColor = backgroundProgressImage.color;
        currentBackgroundProgressColor.a = opacity;
        backgroundProgressImage.color = currentBackgroundProgressColor;
    }
}
