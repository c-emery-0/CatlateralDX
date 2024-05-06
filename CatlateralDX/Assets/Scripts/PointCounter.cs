using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointCounter : MonoBehaviour
{
    public int points = 0;
    public int propscountInitial, propscountCurrent;

    [SerializeField] private GameObject floatingTextPrefab;

    void Start() {
        propscountInitial = GameObject.Find("Props").transform.childCount;
        propscountCurrent = propscountInitial;
    }

    public void UpdatePoints(int pts, Vector3 position)
    {
        if (pts == 10) propscountCurrent = propscountCurrent - 1;
        points += pts;

        if (floatingTextPrefab) {
            GameObject prefab = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMeshProUGUI>().text = "" + pts;
        }
        Debug.Log("Points "+points+" Num Objects Destoyred "+(propscountInitial-propscountCurrent));
    }

    public int GetNumObjDestroyed() {
        return propscountInitial - propscountCurrent;
    }
    public bool IsAllObjDestroyed() {
        return propscountCurrent == 0;
    }
}