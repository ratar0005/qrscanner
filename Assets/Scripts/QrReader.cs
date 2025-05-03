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
    bool isReady;


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

        isReady = true;
    }

    public void Read()
    {
        var webcamTexture = webcam.GetTexture();
        if (webcamTexture == null || cameraTexture == null)
            return;

        Debug.Log($"{webcamTexture.didUpdateThisFrame}");

        var pixels = webcamTexture.GetPixels32();
        cameraTexture.SetPixels32(pixels);
        cameraTexture.Apply();
        var result = reader.Decode(
                    pixels,
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
        if (!isReady)
            return;

        if (Time.time - lastScanTime >= scanInterval)
        {

            lastScanTime = Time.time;

            Read();
        }
    }
}
