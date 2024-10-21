
using Microsoft.AspNetCore.Mvc;

[ApiController]
// [Route("api/[controller]")] //recuerda que la ruta es api/Person porque la ruta toma el nombre de la clase y le quita el sufijo Controller
[Route("api/klk")] // Cambiando la ruta de la API por un nombre personalizado
public class PersonController : ControllerBase {
    
    [HttpGet]
    public ActionResult<Person> GetPerson() {
        var person = new Person {
            Id = 1,
            Name = "John Doe",
            Age = 30
        };
        return Ok(person);
    }

    [HttpPost]
    public ActionResult<Person> PostPerson([FromBody] Person person) {
        if (person == null) {
            return BadRequest("Person is null.");
        }
        // Here you would typically add the person to a database
        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        // return Ok();
    }
}

public class Person {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}