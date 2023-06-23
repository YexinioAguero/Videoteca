using Videoteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Videoteca.Data;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Videoteca.Migrations;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Security.Claims;

namespace Videoteca.Controllers
{
    public class UsersController : Controller
    {
        private VideotecaContext db = new VideotecaContext();
        // GET: PersonController
        public ActionResult Index()
        {
            var personList = new List<AspNetUser>();
            using (var dbContext = new VideotecaContext())
            {
                personList = dbContext.AspNetUsers.ToList();
            }
            ViewBag.persona = personList.FirstOrDefault();
            return View(personList);
        }


        // Get: EditRole/Edit/5

        public ActionResult EditRole(string id)
        {
            var person = new AspNetUserRoles();
            var RoleUser = db.AspNetUserRoles.ToList();
            foreach (var role in RoleUser)
            {
                if (role.UserId == id)
                {
                    person = role; break;

                }
            }

            return View(person);
        }



        // POST: EditRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(string id, AspNetUserRoles person1, string RoleId)
        {

            var person = new AspNetUserRoles();
            var RoleUser = db.AspNetUserRoles.ToList();
            foreach (var role in RoleUser)
            {
                if (role.UserId == id)
                {
                    person = role; break;

                }
            }
            db.AspNetUserRoles.Remove(person);
            db.SaveChanges();

            person.RoleId = RoleId;
            person.UserId = id;
            db.AspNetUserRoles.Add(person);
            db.SaveChanges();

            return RedirectToAction("Index");


        }




        // GET: PersonController/Details/5
        public ActionResult Details(string id)
        {
            var person = db.AspNetUsers.Find(id);
            db.AspNetUsers.Where(c => c.Id == id);

            return View(person);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




        // GET: PersonController/Edit/5
        public ActionResult Edit(string id)
        {
            var person = db.AspNetUsers.Find(id);

            return View(person);
        }



        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, AspNetUser person, IFormFile photo)
        {
            try
            {//lee los datos de una foto, los convierte en un arreglo de bytes, los guarda en bd en una tabla y actualiza un campo "ProfilePicture" en AspNetUsers con el ID de la imagen guardada.
                db.Update(person);
                if (photo != null && photo.Length > 0)
                {
                    // Leer los datos de la foto y convertirlos a un arreglo de bytes
                    using (var memoryStream = new MemoryStream())
                    {
                        photo.CopyTo(memoryStream);
                        byte[] photoData = memoryStream.ToArray();

                        // Guardar la foto en la tabla "Image"
                        var image = new profilePicture
                        {
                            image = photoData
                        };

                        db.profilePictures.Add(image);
                        db.SaveChanges();

                        // Actualizar el campo "ProfilePicture" en la tabla "AspNetUsers" con la ruta de la foto guardada en la tabla "Image"
                        person.ProfilePicture = image.id.ToString();
                    }
                }


                ViewBag.Message = "Se realizó de manera correcta";
                db.SaveChanges();
                ViewBag.Message = new MessagePack { Text = "Se realizo de manera correcta", Tipo = Tipo.message.success.ToString() };
                return RedirectToAction("Edit");

            }
            catch
            {
                ViewBag.Message = new MessagePack { Text = "No se realizo de manera correcta", Tipo = Tipo.message.danger.ToString() };

                return View();
            }


        }
 
        //Este metodo recibe el id de para identificar la imagen,busca en la bd la imagen y la une con este id.
        //image se convierte en la representación binaria de la imagen, al devolverlo con File() se verás estos datos como correspondientes a una imagen y se mostrará como tal. 
        public ActionResult GetProfilePicture(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var image = db.profilePictures.Find(int.Parse(id));
            if (image == null)
            {
                return NotFound();
            }

           return File(image.image, "image/jpeg"); // Devuelve la imagen como un archivo de tipo "image/jpeg"
        }


        //Método para generar el PDF con los usuarios de la aplicación.

        public IActionResult DownloaderPDF()
        {
            var personList = new List<AspNetUser>();

            using (var dbContext = new VideotecaContext())
            {
                personList = dbContext.AspNetUsers.ToList();
            }

            var documentpdf = Document.Create(Container =>
            {
                Container.Page(Page =>
                {
                    Page.Content().Background("#F5F5DC").Padding(20).Column(col =>
                    {
                        Page.Header().Row(row =>
                        {

                            row.RelativeItem().Background("#F5F5DC").AlignCenter().PaddingTop(10).Text("Reporte de Usuarios VIDEOTECA ACY").FontFamily("Courier New").FontColor("#050505").FontSize(16);

                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Height(30).Background("#257272").AlignCenter().Text("Nombre")
                                    .FontColor("#fff").FontSize(11);
                                header.Cell().Height(30).Background("#257272").AlignCenter().Text("UserName")
                                    .FontColor("#fff").FontSize(11);
                                header.Cell().Height(30).Background("#257272").AlignCenter().Text("Correo")
                                    .FontColor("#fff").FontSize(11);
                            });

                            foreach (var person in personList)
                            {
                                table.Cell().Background(Colors.LightBlue.Lighten3).Border(0.5f).BorderColor(Colors.Black).AlignCenter().Padding(2).Text(person.Name)
                                    .FontColor("#000").FontSize(11);
                                table.Cell().Background(Colors.LightGreen.Lighten1).Border(0.5f).BorderColor(Colors.Black).AlignCenter().Padding(2).Text(person.UserName)
                                    .FontColor("#000").FontSize(11);
                                table.Cell().Background(Colors.LightBlue.Lighten3).Border(0.5f).BorderColor(Colors.Black).AlignCenter().Padding(2).Text(person.Email)
                                    .FontColor("#000").FontSize(11);
                            }
                        });
                    });
                });
            }).GeneratePdf();

            var stream = new MemoryStream(documentpdf);
            return File(stream, "application/pdf", "Person_List.pdf");
        }
    }
}

//< div class= "row" >
//    < div class= "col-md-4" >
//        < form asp - action = "Edit" >
//            < div asp - validation - summary = "ModelOnly" class= "text-danger" ></ div >
//            < input type = "hidden" asp -for= "Id" />
//            < div class= "form-group" >
//                < label asp -for= "Name" class= "control-label" ></ label >
//                < input asp -for= "Name" class= "form-control" />
//                < span asp - validation -for= "Name" class= "text-danger" ></ span >
//            </ div >

//            < div class= "form-group" >
//                < label asp -for= "UserName" class= "control-label" ></ label >
//                < input asp -for= "UserName" class= "form-control" />
//                < span asp - validation -for= "UserName" class= "text-danger" ></ span >
//            </ div >

//            < div class= "form-group" >
//                < label asp -for= "Email" class= "control-label" ></ label >
//                < input asp -for= "Email" class= "form-control" />
//                < span asp - validation -for= "Email" class= "text-danger" ></ span >
//            </ div >

//            < div class= "form-group" >
//                < input type = "submit" value = "Save" class= "btn btn-primary" />
//            </ div >
//        </ form >
//    </ div >
//</ div >