namespace Football.Controllers
{
    using Football.Infrastructure.Data;
    using Football.Infrastructure.Data.Models;
    using Football.Models.Stadiums;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    public class StadiumsController : Controller
    {
        private readonly FootballDbContext data;

        private readonly IWebHostEnvironment webHostEnvironment;

        public StadiumsController(
            FootballDbContext _data,
            IWebHostEnvironment _webHostEnvironment)
        {
            this.data = _data;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All([FromQuery] AllStadiumQueryModel query)
        {
            var stadiumQuery = this.data.Stadiums.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.City))
            {
                stadiumQuery = stadiumQuery.Where(p => p.Name == query.City);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                stadiumQuery = stadiumQuery.Where(s => s.Name.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            stadiumQuery = query.Sorting switch
            {
                StadiumSorting.Name => stadiumQuery.OrderBy(s => s.Name),
                StadiumSorting or _ => stadiumQuery.OrderByDescending(s => s.Id)
            };

            var totalStadiims = stadiumQuery.Count();

            var stadiums = stadiumQuery
                .Skip((query.CurrentPage - 1) * AllStadiumQueryModel.StadiumPerPage)
                .Take(AllStadiumQueryModel.StadiumPerPage)
                .Select(s => new StadiumListingViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Capacity = s.Capacity,
                    Description = s.Description,
                    Image = s.Image,
                })
                .ToList();

            var stadiumCity = this.data
                .Cities
                .Select(s => s.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            query.TotalStadiums = totalStadiims;
            query.Cities = stadiumCity;
            query.Stadiums = stadiums;

            return View(query);
        }

        public IActionResult Add() => View(new AddStadiumFormModel
        {
            Cities = this.GetStadiumCities()
        });

        [HttpPost]
        public IActionResult Add(AddStadiumFormModel stadium)
        {
            if (!this.data.Cities.Any(c => c.Id == stadium.CityId))
            {
                this.ModelState.AddModelError(nameof(stadium.CityId), "City does not exist");
            }

            if (ModelState.IsValid)
            {
                stadium.Cities = this.GetStadiumCities();

                return View(stadium);
            }

            string stringFileName = UploadFile(stadium);

            var stadiumData = new Stadium
            {
                Name = stadium.Name,
                Address = stadium.Address,
                Capacity = stadium.Capacity,
                Description = stadium.Description,
                Image = stringFileName,
                Id = stadium.CityId
               
            };

            this.data.Stadiums.Add(stadiumData);

            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
            
        }

        private string UploadFile(AddStadiumFormModel model)
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

        private IEnumerable<StadiumCityViewModel> GetStadiumCities()
       => this.data
            .Cities
            .OrderBy(c => c.Name)
            .Select(c => new StadiumCityViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToList();
    }
}