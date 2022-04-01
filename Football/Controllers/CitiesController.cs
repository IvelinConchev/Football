namespace Football.Controllers
{
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using Football.Models.Cities;
    using Microsoft.AspNetCore.Mvc;

    public class CitiesController : Controller
    {
        private readonly FootballDbContext data;

        private readonly IWebHostEnvironment webHostEnvironment;

        public CitiesController(
            FootballDbContext _data,
            IWebHostEnvironment _webHostEnvironment)
        {
            this.data = _data;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All([FromQuery] AllCityQueryModel query)
        {
            var cityQuery = this.data.Cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Team))
            {
                cityQuery = cityQuery.Where(c => c.Name == query.Team);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                cityQuery = cityQuery.Where(p => (p.Name).ToLower().Contains(query.SearchTerm.ToLower()));
            }

            cityQuery = query.Sorting switch
            {
                CitySorting.Name => cityQuery.OrderBy(p => p.Name),
                CitySorting.PostCode => cityQuery.OrderBy(p => p.Name),
            };

            var totalCities = cityQuery.Count();

            var cities = cityQuery
                .Skip((query.CurrentPage - 1) * AllCityQueryModel.CityPerPage)
                .Take(AllCityQueryModel.CityPerPage)
                .Select(c => new CityListingViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.Image,
                    PostCode = c.PostCode,
                })
                .ToList();

            var cityTeams = this.data
                .Cities
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            query.TotalCities = totalCities;
            query.Teams = cityTeams;
            query.Cities = cities;

            return View(query);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add() => View(new AddCityFormModel
        {
            Teams = this.GetCityTeams()
        });

        [HttpPost]
        public IActionResult Add(AddCityFormModel city)
        {
            if (!ModelState.IsValid)
            {
                city.Teams = this.GetCityTeams();

                return View(city);
            }

            string stringFileName = UploadFile(city);

            var cityData = new City
            {
                Name = city.Name,
                PostCode = city.PostCode,
                Desctription = city.Description,
                Image = stringFileName
            };

            this.data.Add(cityData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private string UploadFile(AddCityFormModel model)
        {
            string fileName = null;
            if (model.Image != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "-" + model.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        private IEnumerable<CityTeamViewModel> GetCityTeams()
       => this.data
            .Teams
            .OrderBy(p => p.Name)
            .Select(p => new CityTeamViewModel
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToList();
    }
}

