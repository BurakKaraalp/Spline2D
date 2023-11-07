using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class CursorController : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    private void Update()
    {
        //Faremizin ekrandaki ve dünyadaki pozisyonunu alıyoruz
        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //nativespline oluşturuyoruz
        using (var spline = new NativeSpline(_spline.Spline, _spline.transform.localToWorldMatrix))
        {
            //SplineUtility sınıfı içerisindeki GetNearestPoint fonkisyonu ile faremizin dünya pozisyonunu referans alarak spline üzerindeki en yakın noktayı buluyoruz.
            var distance = SplineUtility.GetNearestPoint(
                spline,
                worldPosition,
                out var nearest,
                out var t
             );
            //en yakın noktanın ekrandaki pozisyonu ile faremizin ekrandaki pozisyonunun farkını alıp uzaklığı elde ediyoruz.
            float mouseNearPointDistance = Vector3.Distance(Camera.main.WorldToScreenPoint(nearest), screenPosition);

            /* Eğer Uzaklık 25'den büyükse ve sol kliğe basılı tutuluyorsa faremizi en yakın noktaya snapliyoruz. Kullandığım yöntemde sürekli olarak en yakın nokta hesaplandığı
             * için faremizi kaydırdıkça cursor spline üzerinde hareket edecektir*/

            if (mouseNearPointDistance <= 25 && Mouse.current.leftButton.isPressed)
            {
                Vector3 cursorPos = new Vector2(Camera.main.WorldToScreenPoint(nearest).x, Camera.main.WorldToScreenPoint(nearest).y);
                Mouse.current.WarpCursorPosition(cursorPos);
            }
        }
    }
}
