using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Properties")]
    public string itemName;
    public int identifier;
    public float weight;
    public string type;
    
    public Sprite icon;
    public GameObject itemObject;
    public Vector3 originalPosition;
}
