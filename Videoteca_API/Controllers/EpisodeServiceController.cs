using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Videoteca_API.Data;

namespace Videoteca_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeServiceController : Controller
    {
        private VideotecaContext db = new VideotecaContext();

    }
}
