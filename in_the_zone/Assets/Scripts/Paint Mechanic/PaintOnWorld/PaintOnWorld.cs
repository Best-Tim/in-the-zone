using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintOnWorld : MonoBehaviour
{
    [Header("Brush Settings")]
    public Color color;
    [SerializeField] GameObject brush;
    public float BrushSize = 0.002f;

    [Header("ParticleController")]
    [SerializeField] ParticlesController particlesController;
    //Paint Variables Needed
    public Themes theme;

    PointEventLibrary pel;

    private MiniObjectivesController moc;

    void Start()
    {
        pel = FindObjectOfType<PointEventLibrary>();
        //change brush color at runtime, cart color should be taken from the enums of themes
        brush.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
        moc = GetComponentInParent<MiniObjectivesController>();
    }
    private void Update()
    {
        if (GetComponentInParent<PaintbarController>().isPainting)
        {
            SpawnPaint();
            particlesController.IsOn = true;
            
        }
        if (!GetComponentInParent<PaintbarController>().isPainting)
        {
            particlesController.IsOn = false;
        } 
    }
    private void SetPaintTheme(GameObject go)
    {
        PaintedBy paint = go.GetComponent<PaintedBy>();
        paint.theme = theme;
        paint.playerName = GetComponentInParent<KartController>().nickname;
    }
    //this will need to be called from the controllers script
    public void SpawnPaint()
    {
        //Debug.Log("Inside Spawn Piant");
        
        Vector3 pos = this.transform.position;
        RaycastHit hit;


        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.layer == 9)
            {

                var go = Instantiate(brush, hit.point + new Vector3(0f,0.05f,0), Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), hit.transform);
                //multiplies the brush scale by the size of it's parent. EG: brush looks too small, parent size might be (1,1,1) thus increase size.
                //If brush looks too big, parent size might be (10,10,10) and then brush size should be decreased.
                go.transform.localScale = Vector3.one * BrushSize;
                SetPaintTheme(go); 
                pel.PublicEventAddPointsPaint(this.transform.parent.parent.name);
            }
            else if (hit.transform.gameObject.layer == 28 && hit.transform.gameObject.GetComponent<PaintedBy>().playerName !=
                     GetComponentInParent<KartController>().nickname)
            {
               
                var go = Instantiate(brush, hit.point + new Vector3(0f,0.05f,0), Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)), hit.transform);
                Destroy(hit.transform.gameObject);
                go.transform.localScale = Vector3.one * BrushSize;
                pel.RemovePaintPoints(hit.transform.gameObject.GetComponent<PaintedBy>().playerName);
                SetPaintTheme(go);   
                pel.PublicEventAddPointsPaint(this.transform.parent.parent.name);
                moc.AddPointQ3();
                moc.onTextChange.Invoke();
            }
        }
    }
}
