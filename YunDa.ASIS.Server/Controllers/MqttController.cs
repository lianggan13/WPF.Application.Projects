using MQTTnet.Server;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YunDa.ASIS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MqttController : ControllerBase
    {
        private readonly MqttServer? mqttServer;

        public MqttController()
        {
            mqttServer = ServiceLocator.GetService<MqttServer>();
        }

        // GET: api/<MqttController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MqttController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MqttController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MqttController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MqttController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
