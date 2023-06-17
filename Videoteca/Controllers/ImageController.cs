//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.Differencing;
//using Microsoft.Data.SqlClient;
//using Videoteca.Models.DTO;

//namespace Videoteca.Controllers
//{
//    public class ImageController : Controller
//    {
//        public IActionResult Index()
//        {
//            [HttpPost]
//            public async Task<IActionResult> UploadProfileImage(IFormFile file)
//            {
//                // Obtén el usuario actual
//                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

//                if (user != null)
//                {
//                    // Verifica si se seleccionó un archivo
//                    if (file != null && file.Length > 0)
//                    {
//                        // Convierte el archivo en una cadena base64
//                        string profilePictureBase64;
//                        using (var memoryStream = new MemoryStream())
//                        {
//                            await file.CopyToAsync(memoryStream);
//                            byte[] fileBytes = memoryStream.ToArray();
//                            profilePictureBase64 = Convert.ToBase64String(fileBytes);
//                        }

//                        // Actualiza la imagen de perfil en el modelo del usuario
//                        var userBD = db.tb_USER.FromSqlRaw(@"exec getUserByName @username", new SqlParameter("@username", user.UserName)).AsEnumerable().FirstOrDefault();
//                        userBD.IMG = profilePictureBase64;
//                        user.ProfilePicture = profilePictureBase64;

//                        // Guarda los cambios en la base de datos
//                        var result = await _userManager.UpdateAsync(user);
//                        db.tb_USER.Update(userBD);
//                        db.SaveChanges();
//                        if (!result.Succeeded)
//                        {
//                            foreach (var error in result.Errors)
//                            {
//                                ModelState.AddModelError(string.Empty, error.Description);
//                            }
//                            return View("ChangeProfilePicture");
//                        }
//                    }
//                }
//                return RedirectToAction(nameof(edit));
//            }

//            public async Task<IActionResult> ChangeProfilePicture()
//            {
//                // Obtener el usuario actualmente autenticado
//                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

//                if (user != null)
//                {
//                    string profilePictureBase64 = user.ProfilePicture;
//                    // Verificar si la imagen de perfil existe
//                    if (!string.IsNullOrEmpty(user.ProfilePicture))
//                    {
//                        // Convertir la cadena base64 en una URL válida
//                        string imageFormat = "image/png"; // Cambia esto según el formato de imagen que estés utilizando
//                        string profilePictureUrl = $"data:{imageFormat};base64,{profilePictureBase64}";
//                        // Pasar la URL de la imagen de perfil a la vista
//                        ViewBag.ProfilePicture = profilePictureUrl;
//                    }
//                    else
//                    {
//                        ViewBag.ProfilePicture = "https://www.kindpng.com/picc/m/24-248253_user-profile-default-image-png-clipart-png-download.png";
//                    }
//                }
//                return View();
//            }
//        }
//    }
//}
