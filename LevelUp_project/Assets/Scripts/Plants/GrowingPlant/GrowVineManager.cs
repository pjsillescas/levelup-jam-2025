using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowVineManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of all the meshes we want to affect")]
    private List<MeshRenderer> growVineMeshes;


    [SerializeField]
    private float timeToGrow = 5;
    [SerializeField]
    private float refreshRate = 0.05f;
    [SerializeField]
    [Range(0f, 1f)]
    private float minGrow = 0.2f;
    [SerializeField]
    [Range(0f, 1f)]
    private float maxGrow = 0.99f;

    List<Material> _growVineMaterials = new List<Material>();
    private bool fullyGrown;



    private void Start()
    {
        for (int i = 0; i < growVineMeshes.Count; i++)
        {
            for (int j = 0; j < growVineMeshes[i].materials.Length; j++)
            {
                if (growVineMeshes[i].materials[j].HasProperty("Grow_"))
                {
                    growVineMeshes[i].materials[j].SetFloat("Grow_", minGrow);
                    _growVineMaterials.Add(growVineMeshes[i].materials[j]);
                }
            }
        }  

        foreach(Material mat in _growVineMaterials)
        {
            mat.SetFloat("Grow_", 0f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleVines();
            gameObject.GetComponent<BoxCollider>().enabled = false;

        }
    }

    /*
     Grows and ungrows the vine with the shader.
     */
    public void ToggleVines()
    {
        for (int i = 0; i < _growVineMaterials.Count; i++)
        {
            StartCoroutine(GrowVines(_growVineMaterials[i]));
        }
    }


    IEnumerator GrowVines(Material mat)
    {
        float growValue = mat.GetFloat("Grow_");

        if (!fullyGrown)
        {
            while (growValue < maxGrow)
            {
                growValue += 1 / (timeToGrow / refreshRate);
                mat.SetFloat("Grow_", growValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            while (growValue > maxGrow)
            {
                growValue -= 1 / (timeToGrow / refreshRate);
                mat.SetFloat("Grow_", growValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }
        if (growValue <= maxGrow)
            fullyGrown = true;
        else
            fullyGrown = false;
    }
}