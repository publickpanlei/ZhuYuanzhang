using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

//给空间添加监听事件要实现的一些接口
public class MyDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler,
    IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public RectTransform canvas;          //得到canvas的ugui坐标
    private ContentEdit contentEdit;
    private RectTransform imgRect;        //得到图片的ugui坐标
    Vector2 offset = new Vector3();    //用来得到鼠标和图片的差值
    private bool canDrag = true;

    // Use this for initialization
    void Start()
    {
        imgRect = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        contentEdit = GameObject.Find("PanelCellWorld").GetComponent<ContentEdit>();
    }

    //当鼠标按下时调用 接口对应  IPointerDownHandler
    public void OnPointerDown(PointerEventData eventData)
    {
        if(canDrag)
        {
            Vector2 mouseDown = eventData.position;    //记录鼠标按下时的屏幕坐标
            Vector2 mouseUguiPos = new Vector2();   //定义一个接收返回的ugui坐标
            //RectTransformUtility.ScreenPointToLocalPointInRectangle()：把屏幕坐标转化成ugui坐标
            //canvas：坐标要转换到哪一个物体上，这里img父类是Canvas，我们就用Canvas
            //eventData.enterEventCamera：这个事件是由哪个摄像机执行的
            //out mouseUguiPos：返回转换后的ugui坐标
            //isRect：方法返回一个bool值，判断鼠标按下的点是否在要转换的物体上
            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDown, eventData.enterEventCamera, out mouseUguiPos);

            mouseUguiPos = mouseUguiPos / contentEdit.GetScale();


            if (isRect)   //如果在
            {
                //计算图片中心和鼠标点的差值
                offset = imgRect.anchoredPosition - mouseUguiPos;
            }
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("左键按下");
            this.transform.GetComponent<CellScript>().findNephew();
        }
    }

    //当鼠标拖动时调用   对应接口 IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            Vector2 mouseDrag = eventData.position;   //当鼠标拖动时的屏幕坐标
            Vector2 uguiPos = new Vector2();   //用来接收转换后的拖动坐标

            //和上面类似
            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDrag, eventData.enterEventCamera, out uguiPos);

            uguiPos = uguiPos / contentEdit.GetScale();

            if (isRect)
            {
                //设置图片的ugui坐标与鼠标的ugui坐标保持不变
                //    imgRect.anchoredPosition = offset + uguiPos;
                //只能横移
                //    imgRect.anchoredPosition = new Vector2(offset.x + uguiPos.x, imgRect.anchoredPosition.y);
                //只能纵移
                imgRect.anchoredPosition = new Vector2(imgRect.anchoredPosition.x, offset.y + uguiPos.y);

                this.transform.GetComponent<CellScript>().setFatherLine();
            }
        }

    }

    //当鼠标抬起时调用  对应接口  IPointerUpHandler
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("右键点击");
            int thisType =this.GetComponent<CellScript>().cellType;
            contentEdit.RightButtonClick(this.GetComponent<CellScript>(), eventData.position, thisType); 
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
//             Debug.Log("左键点击");
             offset = Vector2.zero;
             this.GetComponent<CellScript>().SeeHeight();
        }

    }

    //当鼠标结束拖动时调用   对应接口  IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            offset = Vector2.zero;
        }        
    }

    //当鼠标进入图片时调用   对应接口   IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    //当鼠标退出图片时调用   对应接口   IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void SetCanDrag(bool b)
    {
        canDrag = b;
    }
}