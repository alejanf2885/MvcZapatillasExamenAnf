using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using MvcZapatillasExamenAnf.Models;
using MvcZapatillasExamenAnf.Repositories;

namespace MvcZapatillasExamenAnf.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;
        private IConfiguration configuration;

        public ZapatillasController(RepositoryZapatillas repo, IConfiguration configuration)
        {
            this.repo = repo;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Zapatilla zapatilla, IFormFile imagen)
        {
            if (imagen != null && imagen.Length > 0)
            {
                string bucketName = this.configuration["AWS:BucketName"];
                string region = this.configuration["AWS:Region"];

                AmazonS3Client s3Client = new AmazonS3Client(
                    Amazon.RegionEndpoint.GetBySystemName(region)
                );
                TransferUtility transferUtility = new TransferUtility(s3Client);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);

                using (Stream stream = imagen.OpenReadStream())
                {
                    await transferUtility.UploadAsync(stream, bucketName, fileName);
                }

                zapatilla.Imagen = $"https://{bucketName}.s3.{region}.amazonaws.com/{fileName}";
            }

            await this.repo.InsertZapatillaAsync(zapatilla);
            return RedirectToAction("Index");
        }
    }
}
