using System;
namespace PetPaws.Models.DTOs
{
    public record struct GetAnimalDto (
        int AnimalId,
        string Name,
        string Type,
        int Age ,
        string ExtraExplanations ,
        string imagePath,
        string userName,
        string userPhone,
        List<CreateVaccineDto> animalVaccines 
    );
    
}

