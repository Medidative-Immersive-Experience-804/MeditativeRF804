using UnityEngine;

public class VR_SandInteraction : MonoBehaviour
{
    public RenderTexture heightMap;
    public Shader drawShader;
    private Material drawMaterial;
    public float brushSize = 0.01f; // Size of the brush
    public float brushStrength = 0.01f; // Strength of the brush
    public LayerMask sandLayer; // Layer mask for the sand

    private void Start()
    {
        drawMaterial = new Material(drawShader);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the collision object has the right tag or layer
        if (collision.gameObject.tag == "Interactive" || collision.gameObject.layer == LayerMask.NameToLayer("InteractiveObjects"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Ray ray = new Ray(contact.point - contact.normal * 0.1f, contact.normal);
                if (Physics.Raycast(ray, out RaycastHit hit, 0.2f, sandLayer))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Sand"))
                    {
                        DrawOnSand(hit.textureCoord);
                    }
                }
            }
        }
    }

    void DrawOnSand(Vector2 textureCoord)
    {
        RenderTexture.active = heightMap;
        drawMaterial.SetVector("_Coordinate", new Vector4(textureCoord.x, textureCoord.y, 0, 0));
        drawMaterial.SetFloat("_Strength", brushStrength);

        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.TexCoord2(0, 0);
        GL.Vertex3(textureCoord.x - brushSize, textureCoord.y - brushSize, 0);
        GL.TexCoord2(1, 0);
        GL.Vertex3(textureCoord.x + brushSize, textureCoord.y - brushSize, 0);
        GL.TexCoord2(1, 1);
        GL.Vertex3(textureCoord.x + brushSize, textureCoord.y + brushSize, 0);
        GL.TexCoord2(0, 1);
        GL.Vertex3(textureCoord.x - brushSize, textureCoord.y + brushSize, 0);
        GL.End();
        GL.PopMatrix();

        RenderTexture.active = null;
    }

    Vector2 WorldToTextureCoordinates(Vector3 worldPosition)
    {
        // Implement conversion logic here based on your specific setup.
        // This usually involves transforming the world position to local coordinates relative to the mesh and then mapping these to UVs.
        return new Vector2(); // Placeholder
    }
}
