using Microsoft.AspNetCore.Mvc;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing;

namespace Videoteca.Controllers
{
    public class WebcamController : Controller
    {
        private bool HayDispositivos;
        private FilterInfoCollection MisDispositivos;
        private VideoCaptureDevice MiWebCam = null;

        //Ejecuta la acción de encender la cámara
        public ActionResult Cam()
        {
            CargaDispositivos();
            return View();
        }

        //Carga los dispositivos disponibles
        public void CargaDispositivos()
        {
            MisDispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (MisDispositivos.Count > 0)
            {
                HayDispositivos = true;
                for (int i = 0; i < MisDispositivos.Count; i++)
                    ViewBag.Dispositivo = MisDispositivos[i].Name.ToString();
            }
            else
            {
                HayDispositivos = false;
                ViewBag.Dispositivo = "No se encontraron dispositivos de video.";
            }
        }

        //Cierra la webcam deteniendo así la grabación
        public void CerrarWebCam()
        {
            if (MiWebCam != null && MiWebCam.IsRunning)
            {
                MiWebCam.SignalToStop();
                MiWebCam = null;
            }
        }

        //Inicia el momento de la grabación y redirige a la acción cam.
        [HttpPost]
        public ActionResult StartCapture()
        {
            CerrarWebCam();
            int i = 0; 
            string NombreVideo = MisDispositivos[i].MonikerString;
            MiWebCam = new VideoCaptureDevice(NombreVideo);
            MiWebCam.NewFrame += new NewFrameEventHandler(Capturando);
            MiWebCam.Start();
            return RedirectToAction("Cam");
        }

        //Se ejecuta para capturar los fotogramas de la cámara web usando el objeto bitmap
        //Bitmap se utiliza en métodos relacionados al procesamiento de imagenes, como es este el caso.
        public void Capturando(object sender, NewFrameEventArgs eventArgs)
        {
            // Lógica para procesar los fotogramas capturados
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            // Realiza la lógica necesaria con la imagen capturada
            // Puedes almacenarla en una variable, enviarla a la vista, etc.
        }

        //cierra la cámara web y liberar los recursos.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CerrarWebCam();
            }
            base.Dispose(disposing);
        }
    }
}
