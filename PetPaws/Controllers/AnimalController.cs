using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPaws.Models;
using PetPaws.Models.DTOs;
using PetPaws.Repositories;


namespace PetPaws.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AnimalController : ControllerBase
    {

        private readonly ApiDbContext _dbRepository;

        public AnimalController(ApiDbContext dbRepository)
        {
            _dbRepository = dbRepository;
        }

        // GET: api/values
        [HttpGet]
        public async Task<ActionResult<List<Animal>>> GetAnimals()
        {
            return Ok(await _dbRepository.Animals.ToListAsync());
        }

        [HttpGet("filter/{type}")]
        public async Task<ActionResult<List<Animal>>> GetAnimalsbyidFilter([FromRoute(Name = "type")] string type)
        {
            return Ok(await _dbRepository.Animals.Where(a => a.Type == type).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public ActionResult<GetAnimalDto> GetAnimalbyId([FromRoute(Name = "id")] int id)
        {

            var animal = _dbRepository.Animals.Include(a => a.Vaccines)
    .FirstOrDefault(a => a.AnimalId == id);
            if (animal is null)
            {
                return BadRequest("animal is not found");
            }

            var vaccines = new List<CreateVaccineDto>();
            
            animal.Vaccines.ForEach(v =>
            {
                var vaccineToDto = new CreateVaccineDto();
                vaccineToDto.Name = v.Name;
                vaccines.Add(vaccineToDto);
            });

            var animalDto = new GetAnimalDto
            {
                AnimalId = animal.AnimalId,
                Name = animal.Name,
                Type = animal.Type,
                Age = animal.Age,
                imagePath = animal.imagePath,
                ExtraExplanations = animal.ExtraExplanations,
                userName = animal.UserName,
                userPhone = animal.UserPhone,
                animalVaccines = vaccines
            };

            return animalDto;
        }

        [HttpGet("saved/{email}")]
        public ActionResult<List<Animal>> GetSavedAnimals([FromRoute(Name = "email")] string email)
        {
            var user = _dbRepository.Users.Include(a => a.Animals).ThenInclude(a => a.Vaccines).FirstOrDefault(u => u.Email == email);
            var vaccines = new List<CreateVaccineDto>();
            var getAnimalDtos = new List<GetAnimalDto>();
            if (user is null)
            {
                return BadRequest("animal is not found");
            }
            user.Animals.ForEach(a =>
            {

                a.Vaccines.ForEach(v =>
                {
                    CreateVaccineDto vaccineToDto = new CreateVaccineDto();
                    vaccineToDto.Name = v.Name;
                    vaccines.Add(vaccineToDto);
                });
                var animalDto = new GetAnimalDto
                {
                    AnimalId = a.AnimalId,
                    Name = a.Name,
                    Type = a.Type,
                    Age = a.Age,
                    imagePath = a.imagePath,
                    ExtraExplanations = a.ExtraExplanations,
                    userName = a.UserName,
                    userPhone = a.UserPhone,
                    animalVaccines = vaccines
                };
                getAnimalDtos.Add(animalDto);


            });

            return Ok(getAnimalDtos);

        }
        
        [HttpGet("myanimal/{email}")]
        public async Task<ActionResult<Animal>> GetMyAnimalsAsync([FromRoute(Name = "email")] string email)
        {
            List<Animal> animalList = await _dbRepository.Animals.Where(u => u.UserName == email).ToListAsync();

            if (animalList is null)
            {
                return BadRequest("animal is not found");
            }

            return Ok(animalList);

        }


        // POST api/values
        [HttpPost]
        public async Task<ActionResult<List<Animal>>> PostAnimal(CreateAnimalDto request)
        {
            var newAnimal = new Animal
            {
                Name = request.Name,
                Type = request.Type,
                Age = request.Age,
                ExtraExplanations = request.ExtraExplanations,
                imagePath = request.imagePath,
                UserName = request.userName,
                UserPhone = request.userPhone
            };

            //var user = _dbRepository.Users.FirstOrDefault(u => u.Email == request.myUser.Email);
            //if (user is null)
            //{
            //    return BadRequest("User is not found");
            //};
            //newAnimal.UserId = user.UserId;

            request.animalVaccines.ForEach(v =>
            {

                var vaccine = _dbRepository.Vaccines.FirstOrDefault(a => a.Name == v.Name);
                if (vaccine is not null)
                {
                    newAnimal.Vaccines.Add(vaccine);
                }
            });

            _dbRepository.Animals.Add(newAnimal);
            await _dbRepository.SaveChangesAsync();
            return Ok(await _dbRepository.Animals.Include(c => c.Vaccines).ToListAsync());
        }




        [HttpPut("{id:int}")]
        public async Task<ActionResult<List<Animal>>> PutAnimal([FromRoute(Name = "id")] int id, GetAnimalDto animalDto)
        {
            if (id != animalDto.AnimalId)
            {
                return BadRequest("böyle bir hayvan yok");
            }

            var animal = _dbRepository.Animals.Include(a => a.Vaccines).FirstOrDefault(an => an.AnimalId == id);
            animal.Age = animalDto.Age;
            animal.ExtraExplanations = animalDto.ExtraExplanations;
            animal.imagePath = animalDto.imagePath;
            animal.Name = animalDto.Name;
            animalDto.animalVaccines.ForEach(v =>
            {

                var vaccine = _dbRepository.Vaccines.FirstOrDefault(a => a.Name == v.Name);
                bool flag = animal.Vaccines.Contains(vaccine);
                if (vaccine is not null)
                {
                    if (!flag)
                        animal.Vaccines.Add(vaccine);
                }
            });
            
            await _dbRepository.SaveChangesAsync();
            return Ok(await _dbRepository.Animals.Include(c => c.Vaccines).ToListAsync());

        }

        // DELETE api/values/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnimal([FromRoute(Name = "id")] int id)
        {
            var animal = await _dbRepository.Animals.FindAsync(id);
            if (animal == null)
                return NotFound();

            _dbRepository.Animals.Remove(animal);
            await _dbRepository.SaveChangesAsync();
            return Ok();
        }
        
    }
}


