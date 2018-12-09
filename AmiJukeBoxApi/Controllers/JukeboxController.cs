using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AmiJukeBoxApi.MqttFolder;
using Dapper;
using MySql.Data.MySqlClient;

namespace AmiJukeBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JukeboxController : ControllerBase
    {
        private readonly Mqtt _mqtt = new Mqtt();

        [HttpGet]
        [Route("play/{jbsong}")]
        public ActionResult<string> PlaySong(string jbsong)
        {
            if (jbsong.Length > 3) return jbsong+" not valid selection";
            var aletter = jbsong.Substring(0, 1);
            var anumber = jbsong.Substring(1, jbsong.Length - 1);
            _mqtt.PlaySelectionOnJukebox(aletter.ToUpper(),anumber);
            var songName = GetArtistSongName(aletter.ToUpper(),anumber);
            return songName;
        }

        private string GetArtistSongName(string letter,string number)
        {
            var nbr = 0;
            var sql = "";
            if (!int.TryParse(number, out nbr)) return "";
            if (nbr % 2 == 0)
            {
                sql = string.Format("SELECT CONCAT(Artist1,' - ',B1Song) FROM amijukebox.jbselection WHERE jbletter='{0}' AND jbnumberb='{1}' AND Archived=0", letter, nbr);
            }
            else
            {
                sql = string.Format("SELECT CONCAT(Artist1,' - ',A1Song) FROM amijukebox.jbselection WHERE jbletter='{0}' AND jbnumbera='{1}' AND Archived=0", letter, nbr);
            }


            using (System.Data.IDbConnection db = new MySqlConnection("Server = 127.0.0.1; Uid = lasse; Pwd = zals69; Database = amijukebox;"))

            {
                db.Open();
                return db.Query<string>(sql).ToList()[0].ToString();
            }
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
