using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonSimpleSceneSwitch : MonoBehaviour
{

    public Leap.Unity.Interaction.Anchor _anchor;
    public GameObject _background;
    public GameObject _text;

    public Material _defaultBackgroundMaterial;
    public Material _redBackgroundMaterial;
    public Material _blueBackgroundMaterial;
    public Material _magentaBackgroundMaterial;

    public Material _defaultSkyboxMaterial;
    public Material _RTTSkyboxMaterial;

    public UnityEngine.Video.VideoPlayer _player;
    public UnityEngine.Video.VideoClip _redVideoClip;
    public UnityEngine.Video.VideoClip _blueVideoClip;
    public UnityEngine.Video.VideoClip _magentaVideoClip;

    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }

    public void doLoadPanorama()
    {

        // clean from previous
        _player.targetTexture.DiscardContents();
        _player.targetTexture.Release();

        // check wich one is loaded
        foreach (var anchObj in _anchor.anchoredObjects)
        {
            switch (anchObj.gameObject.name)
            {
                case "Dynamic UI Object (Blue)":
                {
                    _background.GetComponent<MeshRenderer>().material = _blueBackgroundMaterial;
                    _text.GetComponent<TextMesh>().text = _blueVideoClip.name;
                    _player.clip = _blueVideoClip;
                    break;
                }
                case "Dynamic UI Object (Red)":
                {
                    _background.GetComponent<MeshRenderer>().material = _redBackgroundMaterial;
                    _text.GetComponent<TextMesh>().text = _redVideoClip.name;
                    _player.clip = _redVideoClip;
                    break;
                }
                case "Dynamic UI Object (Magenta)":
                {
                    _background.GetComponent<MeshRenderer>().material = _magentaBackgroundMaterial;
                    _text.GetComponent<TextMesh>().text = _magentaVideoClip.name;
                    _player.clip = _magentaVideoClip;
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        UnityEngine.RenderSettings.skybox = _RTTSkyboxMaterial;
        _player.Play();

    } /* doLoadPanorama() */

    public void doUnloadPanorama()
    {

        // reset settings

        UnityEngine.RenderSettings.skybox = _defaultSkyboxMaterial;

        _player.Stop();

        _background.GetComponent<MeshRenderer>().material = _defaultBackgroundMaterial;

        _text.GetComponent<TextMesh>().text = "";

    } /* doUnloadPanorama() */

}
