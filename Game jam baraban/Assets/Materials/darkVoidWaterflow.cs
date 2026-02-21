using UnityEngine;

public class darkVoidWaterflow : MonoBehaviour
{
    public float scrollSpeedX = 0.05f;
    public float scrollSpeedY = 0.05f;
    private Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
    }

    void Update() {
        // This offsets the texture over time to create movement
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;
        
        // "_BaseMap" and "_BumpMap" (Normal) are the standard URP property names
        rend.material.SetTextureOffset("_BaseMap", new Vector2(offsetX, offsetY));
        rend.material.SetTextureOffset("_BumpMap", new Vector2(offsetX, offsetY));
    }
}