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

        public ActionResult Index()
        {
            CargaDispositivos();
            return View();
        }

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

        public void CerrarWebCam()
        {
            if (MiWebCam != null && MiWebCam.IsRunning)
            {
                MiWebCam.SignalToStop();
                MiWebCam = null;
            }
        }

        [HttpPost]
        public ActionResult StartCapture()
        {
            CerrarWebCam();
            int i = 0; // Obtén el índice seleccionado según tu lógica
            string NombreVideo = MisDispositivos[i].MonikerString;
            MiWebCam = new VideoCaptureDevice(NombreVideo);
            MiWebCam.NewFrame += new NewFrameEventHandler(Capturando);
            MiWebCam.Start();
            return RedirectToAction("Cam");
        }

        public void Capturando(object sender, NewFrameEventArgs eventArgs)
        {
            // Lógica para procesar los fotogramas capturados
            // Puedes almacenarlos, mostrarlos en la vista, etc.
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            // Realiza la lógica necesaria con la imagen capturada
            // Puedes almacenarla en una variable, enviarla a la vista, etc.
        }

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
