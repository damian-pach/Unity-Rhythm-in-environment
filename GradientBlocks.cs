using UnityEngine;

public class GradientBlocks : MonoBehaviour
{
    public int horizontalBlocksAmount = 5;
    public int verticalBlocksAmount = 5;
    public float[,] perlinNoiseArray_0;
    public float[,] perlinNoiseArray_1;
    public float xOffset = 1.22f;
    public float yOffset = 1.55f;

    public float[,] tempPerlinNoise_0;
    public float[,] tempPerlinNoise_1;

    private float[,] noiseScale_0;
    private float[,] noiseScale_1;
    private float scaleValue = 5f;
    private float noiseSpan = 6f;
    private GameObject[,] blocks;
    public Material blockMaterial;

    private float[] blocksBuffer;

    public float colorScale_0 = 3f;
    public float colorScale_1 = 2f;

    public SampleRecalculation sampleRecalculation;
    public float scaleMultiplier = 5f;

    public int firstBuffer = 0;
    public int secondBuffer = 1;

    public float noiseScale;

    public int axis = 0;

    void Start()
    {
        perlinNoiseArray_0 = new float[horizontalBlocksAmount, verticalBlocksAmount];
        perlinNoiseArray_1 = new float[horizontalBlocksAmount, verticalBlocksAmount];
        tempPerlinNoise_0 = new float[horizontalBlocksAmount, verticalBlocksAmount];
        tempPerlinNoise_1 = new float[horizontalBlocksAmount, verticalBlocksAmount];
        noiseScale_0 = new float[horizontalBlocksAmount, verticalBlocksAmount];
        noiseScale_1 = new float[horizontalBlocksAmount, verticalBlocksAmount];

        blocks = new GameObject[horizontalBlocksAmount, verticalBlocksAmount];
        blocksBuffer = new float[8];

        //int shorter = horizontalBlocksAmount > verticalBlocksAmount ? horizontalBlocksAmount : verticalBlocksAmount;

        for (int i = 0; i < horizontalBlocksAmount; i++)
        {
            for (int j = 0; j < verticalBlocksAmount; j++)
            {
                perlinNoiseArray_0[i, j] = Mathf.PerlinNoise((float)i/ noiseScale, (float)j / noiseScale );
                perlinNoiseArray_1[i, j] = Mathf.PerlinNoise((float)i / noiseScale + xOffset, (float)j / noiseScale + yOffset);

            }
        }

#region "Instantiate blocks"

        for (int i = 0; i < horizontalBlocksAmount; i++)
        {
            for (int j = 0; j < verticalBlocksAmount; j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.SetParent(gameObject.transform);
                float tempNoiseScale = perlinNoiseArray_0[i, j] * scaleValue;
                //obj.transform.localScale = new Vector3(1, tempNoiseScale, 1);
                obj.transform.localScale = new Vector3(1, 1, 1);

                switch (axis)
                {
                    case 0:
                        obj.transform.localPosition = new Vector3((float)i * 1.5f, 1 + tempNoiseScale / 2, (float)j * 1.5f);
                        obj.GetComponent<MeshRenderer>().material = blockMaterial;
                        blocks[i, j] = obj;
                        tempPerlinNoise_0 = SetTempPerlinNoise(perlinNoiseArray_0);
                        tempPerlinNoise_1 = SetTempPerlinNoise(perlinNoiseArray_1);
                        break;

                    case 1:
                        obj.transform.localPosition = new Vector3((float)i * 1.5f, (float)j * 1.5f, 1 + tempNoiseScale / 2);
                        obj.GetComponent<MeshRenderer>().material = blockMaterial;
                        blocks[i, j] = obj;
                        tempPerlinNoise_0 = SetTempPerlinNoise(perlinNoiseArray_0);
                        tempPerlinNoise_1 = SetTempPerlinNoise(perlinNoiseArray_1);
                        break;

                    case 2:
                        obj.transform.localPosition = new Vector3(1 + tempNoiseScale / 2, (float)i * 1.5f, (float)j * 1.5f);
                        obj.GetComponent<MeshRenderer>().material = blockMaterial;
                        blocks[i, j] = obj;
                        tempPerlinNoise_0 = SetTempPerlinNoise(perlinNoiseArray_0);
                        tempPerlinNoise_1 = SetTempPerlinNoise(perlinNoiseArray_1);
                        break;
                }

            }
        }

 #endregion

    }

    private float[,] SetTempPerlinNoise(float[,] perlinNoiseArray)
    {
        int horizontalBlocksAmount = perlinNoiseArray.GetLength(0);
        int verticalBlocksAmount = perlinNoiseArray.GetLength(1);
        float[,] _tempPerlinNoise = new float[horizontalBlocksAmount, verticalBlocksAmount];

        for (int i = 0; i < horizontalBlocksAmount; i++)
        {
            for (int j = 0; j < verticalBlocksAmount; j++)
            {

                _tempPerlinNoise[i, j] = Mathf.Min(0.25f, perlinNoiseArray[i, j]);
                _tempPerlinNoise[i, j] = 1f - _tempPerlinNoise[i, j];
                _tempPerlinNoise[i, j] = _tempPerlinNoise[i, j] - 0.75f;
                _tempPerlinNoise[i, j] = _tempPerlinNoise[i, j] * 2f;
            }
        }

        return _tempPerlinNoise;
    }


    void FixedUpdate()
    {
        if(!(sampleRecalculation._audioBandBuffer[firstBuffer] > 0))
        {
            sampleRecalculation._audioBandBuffer[firstBuffer] = 0f;
        }

        if (!(sampleRecalculation._audioBandBuffer[secondBuffer] > 0))
        {
            sampleRecalculation._audioBandBuffer[secondBuffer] = 0f;
        }

        for (int i = 0; i < horizontalBlocksAmount; i++)
        {
            for (int j = 0; j < verticalBlocksAmount; j++)
            {
                noiseScale_0[i, j] = tempPerlinNoise_0[i, j] * sampleRecalculation._audioBandBuffer[firstBuffer] * scaleMultiplier;
                noiseScale_1[i, j] = tempPerlinNoise_1[i, j] * sampleRecalculation._audioBandBuffer[secondBuffer] * scaleMultiplier;

#region "Switch case axes"

                switch (axis)
                {
                    case 0:
                        if (noiseScale_0[i, j] > 0)
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, noiseScale_0[i, j] / 2 + 1 + this.transform.position.y, blocks[i, j].transform.position.z);
                            blocks[i, j].transform.localScale = new Vector3(1f, 1f + noiseScale_0[i, j], 1f);
                            blocks[i, j].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                            blocks[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red * noiseScale_0[i, j] * colorScale_0);
                        }

                        else if (noiseScale_1[i, j] > 0 && !(noiseScale_0[i, j] > 0))
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, noiseScale_1[i, j] / 2 + 1 + this.transform.position.y, blocks[i, j].transform.position.z);
                            blocks[i, j].transform.localScale = new Vector3(1f, 1f + noiseScale_1[i, j], 1f);
                            blocks[i, j].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                            blocks[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.cyan * noiseScale_1[i, j] * colorScale_1);
                        }

                        else
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, transform.position.y + 1f, blocks[i, j].transform.position.z);
                        }
                        break;

                    case 1:
                        if (noiseScale_0[i, j] > 0)
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, blocks[i, j].transform.position.y, noiseScale_0[i, j] / 2 + 1 + this.transform.position.z);
                            blocks[i, j].transform.localScale = new Vector3(1f, 1f, 1f + noiseScale_0[i, j]);
                            blocks[i, j].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                            blocks[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red * noiseScale_0[i, j] * colorScale_0);
                        }

                        else if (noiseScale_1[i, j] > 0 && !(noiseScale_0[i, j] > 0))
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, blocks[i, j].transform.position.y, noiseScale_1[i, j] / 2 + 1 + this.transform.position.z);
                            blocks[i, j].transform.localScale = new Vector3(1f, 1f, 1f + noiseScale_1[i, j]);
                            blocks[i, j].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                            blocks[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.cyan * noiseScale_1[i, j] * colorScale_1);
                        }

                        else
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, blocks[i, j].transform.position.y, transform.position.z + 1f);
                        }
                        break;

                    case 2:
                        if (noiseScale_0[i, j] > 0)
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, noiseScale_0[i, j] / 2 + 1 + this.transform.position.y, blocks[i, j].transform.position.z);
                            blocks[i, j].transform.localScale = new Vector3(1f, 1f + noiseScale_0[i, j], 1f);
                            blocks[i, j].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                            blocks[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red * noiseScale_0[i, j] * colorScale_0);
                        }

                        else if (noiseScale_1[i, j] > 0 && !(noiseScale_0[i, j] > 0))
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, noiseScale_1[i, j] / 2 + 1 + this.transform.position.y, blocks[i, j].transform.position.z);
                            blocks[i, j].transform.localScale = new Vector3(1f, 1f + noiseScale_1[i, j], 1f);
                            blocks[i, j].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                            blocks[i, j].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.cyan * noiseScale_1[i, j] * colorScale_1);
                        }

                        else
                        {
                            blocks[i, j].transform.position = new Vector3(blocks[i, j].transform.position.x, transform.position.y + 1f, blocks[i, j].transform.position.z);
                        }
                        break;
                }
#endregion  

            }
        }
    }
}
