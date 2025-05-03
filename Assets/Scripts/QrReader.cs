using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

public class QrReader : MonoBehaviour
{
    [SerializeField] private WebCam webcam;
    [SerializeField] private TMP_Text output;

    private IBarcodeReader reader;
    private Texture2D cameraTexture;

    int width, height;
    const float scanInterval = 1f;
    float lastScanTime = 0;


    IEnumerator Start()
    {

        yield return new WaitUntil(() => webcam.IsInitiailized);

        reader = new BarcodeReader
        {
            AutoRotate = true,
            TryInverted = true,
            Options = new DecodingOptions
            {
                TryHarder = true
            }
        };

        var webcamTexture = webcam.GetTexture();
        width = webcamTexture.width;
        height = webcamTexture.height;
        cameraTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

    }

    public void Read()
    {
        var webcamTexture = webcam.GetTexture();
        if (webcamTexture == null || cameraTexture == null)
            return;


        cameraTexture.SetPixels32(webcamTexture.GetPixels32());
        cameraTexture.Apply();
        var result = reader.Decode(
                    cameraTexture.GetPixels32(),
                    width,
                    height
                );
        if (result != null)
        {
            output.text = result.Text;
            Debug.Log(result.Text);
        }
        else
            output.text = string.Empty;
    }

    private void Update()
    {
        if (Time.time - lastScanTime >= scanInterval)
        {

            lastScanTime = Time.time;

            Read();
        }
    }
}
