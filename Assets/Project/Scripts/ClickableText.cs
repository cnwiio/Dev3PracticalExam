using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro; // �Ӥѭ: ��ͧ�������������ҹ TextMeshPro ��

// ����¹��÷Ѵ public class ... ����� IScrollHandler ��ͷ����شẺ���
public class ClickableText : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{
    [Tooltip("�����������ͤ�ԡ���� (�������)")]
    public UnityEvent onLeftClick;

    [Tooltip("�����������ͤ�ԡ��� (Ŵ���)")]
    public UnityEvent onRightClick;

    [Tooltip("����������������͹�١���駢��")]
    public UnityEvent onScrollUp;

    [Tooltip("����������������͹�١����ŧ")]
    public UnityEvent onScrollDown;

    // ���������Ѻ�纤��������鹢ͧ Text
    private TextMeshProUGUI TMPtext;
    private Vector3 intialScale;
    private Color intialColor;

    #region Hover Animation Settings
    [Header("LeanTween Settings")]
    [SerializeField] float scale = 1.1f; // �й�����Ѻ�� 1.1 ���� 1.2 �����������˭��Թ仨��鹢ͺ��Ѻ
    [SerializeField] float duration = 0.15f; // Ŵ����ŧ�Դ˹��������������ٵͺʹͧ�Ǣ��
    [SerializeField] LeanTweenType easeType = LeanTweenType.easeOutBack; // �� easeOutBack �з�����ѹ�駴�맹Դ� ��Ѻ
    #endregion

    private void Awake()
    {
        // �֧����๹�� TextMeshProUGUI �������
        TMPtext = GetComponent<TextMeshProUGUI>();

        // �ѹ�֡��Ң�Ҵ������������ ��������͹����
        if (TMPtext != null)
        {
            intialScale = TMPtext.transform.localScale;
            intialColor = TMPtext.color;
        }
    }

    // �ѧ��ѹ���зӧҹ�͹��ԡ�����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            onLeftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            onRightClick.Invoke();
        SoundManager.Instance.PlaySFX("Click");
    }



    // �ѧ��ѹ���зӧҹ����� "�������仪��"
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpScale();
        ChangeColor();
        SoundManager.Instance.PlaySFX("Hover");
    }

    // �ѧ��ѹ���зӧҹ����� "��������͡"
    public void OnPointerExit(PointerEventData eventData)
    {
        DownScale();
        ResetColor();
    }

    public void OnScroll(PointerEventData eventData)
    {
        // scrollDelta.y ���繺ǡ���������͹��� �����ź���������͹ŧ
        if (eventData.scrollDelta.y > 0)
        {
            onScrollUp.Invoke();
            SoundManager.Instance.PlaySFX("Click"); // ������§��ԡ�͹����͹�١����
        }
        else if (eventData.scrollDelta.y < 0)
        {
            onScrollDown.Invoke();
            SoundManager.Instance.PlaySFX("Click"); // ������§��ԡ�͹����͹�١����
        }
    }

    #region Hover Animation Methods (�ҡ�鴢ͧ�س)
    private void UpScale()
    {
        // �� Vector3 ���������·��᡹ X ��� Y
        LeanTween.scale(TMPtext.gameObject, new Vector3(scale, scale, 1f), duration).setEase(easeType);
    }

    private void DownScale()
    {
        LeanTween.cancel(TMPtext.gameObject); // ��ش͹����ѹ��ҡ�͹
        TMPtext.transform.localScale = intialScale;
    }
    #endregion

    #region Color Change Methods (�ҡ�鴢ͧ�س)
    private void ChangeColor()
    {
        TMPtext.color = Color.yellow;
    }

    private void ResetColor()
    {
        TMPtext.color = intialColor;
    }
    #endregion

    private void OnDisable()
    {
        DownScale();
        ResetColor();
    }
}