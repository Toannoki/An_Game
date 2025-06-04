using System.Collections.Generic;
using UnityEngine;
using TMPro; // Sử dụng TMP thay vì Text

public class PickupItem : MonoBehaviour
{
    public float pickupRange = 3f;         // Khoảng cách tối đa để nhặt
    public LayerMask itemLayer;            // Layer chứa item
    public GameObject pickupHintUI;        // UI hiển thị gợi ý (phải có TMP_Text bên trong)
    int pickupCount = 0;
    public GameObject mission;
    private UnityEngine.Camera cam;

    Dictionary<string, string> hintTexts;
    public Movement movement;

    void Start()
    {
        cam = UnityEngine.Camera.main;

        if (pickupHintUI != null)
            pickupHintUI.SetActive(false);

        // Load nội dung từ file hint.txt trong Resources
        hintTexts = LoadHintTexts();
    }

    void Update()
    {
        if (pickupCount >= 5)
        {
            movement.ResumeDialogue();
        }

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, itemLayer))
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.CompareTag("Item"))
            {
                ShowHint("item");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickPotato.Pick(hitObj, pickupHintUI, ref pickupCount, mission);
                }
            }
            else if (hitObj.CompareTag("cho"))
            {
                ShowHint("cho");
            }
            else
            {
                HideHint();
            }
        }
        else
        {
            HideHint();
        }
    }

    Dictionary<string, string> LoadHintTexts()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        TextAsset hintFile = Resources.Load<TextAsset>("interact"); // Tải file Assets/Resources/hint.txt
        if (hintFile == null)
        {
            Debug.LogWarning("Không tìm thấy file hint.txt trong thư mục Resources.");
            return dict;
        }

        string[] lines = hintFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            if (lines[i].StartsWith("@"))
            {
                string key = lines[i].Substring(1).Trim().ToLower(); // "item" hoặc "cho"
                string value = lines[i + 1].Trim();
                dict[key] = value;
            }
        }

        return dict;
    }

    void ShowHint(string key)
    {
        if (pickupHintUI != null && hintTexts.ContainsKey(key))
        {
            TMP_Text hintText = pickupHintUI.GetComponentInChildren<TMP_Text>();
            if (hintText != null)
                hintText.text = hintTexts[key];

            pickupHintUI.SetActive(true);
        }
    }

    void HideHint()
    {
        if (pickupHintUI != null)
            pickupHintUI.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (UnityEngine.Camera.main != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.forward * pickupRange);
        }
    }
}
