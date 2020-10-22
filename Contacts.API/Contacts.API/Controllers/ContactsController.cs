using Contacts_.API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Contacts_.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        readonly Repository<UserContact> _repository;
        public ContactsController(Repository<UserContact> repository)
        {
            _repository = repository;
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> Get(string userid)
        {
            var contacts = await _repository.GetItemsAsync(userid);
            return Ok(contacts);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserContact userContact)
        {
            await _repository.CreateItemAsync(userContact, "userid");
            return Ok();
        }


    }
}
