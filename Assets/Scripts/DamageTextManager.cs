using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance { get; private set; }

    public GameObject damageTextPrefab;
    public Canvas canvas;

    public Color defaultStartColor = Color.red;
    public Color defaultEndColor = Color.clear;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDamageText(Vector3 position, int damageAmount, Color? startColor = null, Color? endColor = null)
    {
        if (damageAmount > 0)
        {
            Color effectiveStartColor = startColor ?? defaultStartColor;
            Color effectiveEndColor = endColor ?? defaultEndColor;

            GameObject damageTextObject = Instantiate(damageTextPrefab, canvas.transform);
            TextMeshProUGUI damageText = damageTextObject.GetComponent<TextMeshProUGUI>();
            damageText.text = damageAmount.ToString();
            damageText.color = effectiveStartColor;

            damageTextObject.transform.position = position;

            RectTransform rectTransform = damageTextObject.GetComponent<RectTransform>();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 100f, 1.5f).SetEase(Ease.OutQuad));
            sequence.Join(damageText.DOFade(0, 1.5f));
            sequence.Join(damageText.DOColor(effectiveEndColor, 1.5f));

            sequence.OnComplete(() => Destroy(damageTextObject));
        }
    }
}
