using lab3_5.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_5.Server.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DeliverycourierserviceContext _context;

        public PersonRepository(DeliverycourierserviceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonDTO>> GetAllPeopleAsync()
        {
            return await _context.People
                .Select(person => new PersonDTO
                {
                    PersonId = person.PersonId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    PhoneNumber = person.PhoneNumber,
                    Client = person.Client,
                    Courier = person.Courier
                })
                .ToListAsync();
        }

        public async Task<PersonDTO?> GetPersonByIdAsync(int id)
        {
            var person = await _context.People
                .Include(p => p.Client)
                .Include(p => p.Courier)
                .FirstOrDefaultAsync(p => p.PersonId == id);

            if (person == null)
            {
                return null;
            }

            return new PersonDTO
            {
                PersonId = person.PersonId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNumber = person.PhoneNumber
            
            };
        }

        public async Task<PersonDTO> CreatePersonAsync(PersonDTO personDTO)
        {
            var person = new Person
            {
                FirstName = personDTO.FirstName,
                LastName = personDTO.LastName,
                PhoneNumber = personDTO.PhoneNumber,
                Client = personDTO.Client,
                Courier = personDTO.Courier
            };

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            personDTO.PersonId = person.PersonId;
            return personDTO;
        }

        public async Task<bool> UpdatePersonAsync(int id, PersonDTO personDTO)
        {
            if (id != personDTO.PersonId)
                return false;

            var person = await _context.People.FindAsync(id);
            if (person == null)
                return false;

            person.FirstName = personDTO.FirstName;
            person.LastName = personDTO.LastName;
            person.PhoneNumber = personDTO.PhoneNumber;

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
                return false;

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(p => p.PersonId == id);
        }
    }
}