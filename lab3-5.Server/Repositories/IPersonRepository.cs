using lab3_5.Server.Models;

namespace lab3_5.Server.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<PersonDTO>> GetAllPeopleAsync();
        Task<PersonDTO?> GetPersonByIdAsync(int id);
        Task<PersonDTO> CreatePersonAsync(PersonDTO person);
        Task<bool> UpdatePersonAsync(int id, PersonDTO updatedPerson);
        Task<bool> DeletePersonAsync(int id);
    }
}