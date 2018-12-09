using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AmiJukeBoxApi.MqttFolder;

namespace AmiJukeBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JukeboxController : ControllerBase
    {
        private readonly Mqtt _mqtt = new Mqtt();

        [HttpGet]
        [Route("play/{jbsong}")]
        public ActionResult<bool> PlaySong(string jbsong)
        {
            if (jbsong.Length > 3) return false;
            var aletter = jbsong.Substring(0, 1);
            var anumber = jbsong.Substring(1, jbsong.Length - 1);
            _mqtt.PlaySelectionOnJukebox(aletter.ToUpper(),anumber);
            return true;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
