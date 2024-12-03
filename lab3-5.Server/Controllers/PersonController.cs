using lab3_5.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonRepository _personRepository;
    private readonly PhantomTransactionConflictSimulator _simulator;

    public PersonController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
        _simulator = new PhantomTransactionConflictSimulator();
    }

    // GET: api/Person
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPeople()
    {
        await _simulator.SimulatePhantomAnomaly();
        var peopleDTO = await _personRepository.GetAllPeopleAsync();
        return Ok(peopleDTO);
    }

    // GET: api/Person/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDTO>> GetPerson(int id)
    {
        var personDTO = await _personRepository.GetPersonByIdAsync(id);
        if (personDTO == null)
        {
            return NotFound();
        }
        return Ok(personDTO);
    }

    // POST: api/Person
    [HttpPost]
    public async Task<ActionResult<PersonDTO>> PostPerson(PersonDTO personDTO)
    {
        await _personRepository.CreatePersonAsync(personDTO);
        return CreatedAtAction(nameof(GetPerson), new { id = personDTO.PersonId }, personDTO);
    }

    // PUT: api/Person/5
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonDTO>> PutPerson(int id, PersonDTO personDTO)
    {
        personDTO.PersonId = id;
        var updated = await _personRepository.UpdatePersonAsync(id, personDTO);
        if (!updated) return BadRequest();
        return Ok(personDTO);
    }

    // DELETE: api/Person/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var deleted = await _personRepository.DeletePersonAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}