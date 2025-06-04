using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickPotato : MonoBehaviour
{
    
    public static void Pick(GameObject hitObj, GameObject pickupHintUI,ref int pickupCount, GameObject mission)
    {
        pickupCount = pickupCount + 1;
        Debug.Log("Đã nhặt vật phẩm: " + hitObj.name);
        Destroy(hitObj);

        if (pickupHintUI != null)
            pickupHintUI.SetActive(false);
        TMP_Text missionText = mission.GetComponent<TMP_Text>();
        missionText.text = "Tìm khoai lang (" + pickupCount + "/5)" ;
    }
}
