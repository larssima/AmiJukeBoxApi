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

        [HttpGet]
        //[Route("aselections")]
        public ActionResult<string> ASelections()
        {
            var aList = new List<JukeBoxSelection>();
            aList = GetAllASongs();
            foreach (JukeBoxSelection row in aList)
            {
                if (row.JbNumberA.Length == 1)
                    row.JbNumberA = "0" + row.JbNumberA;
            }
            aList = aList.OrderBy(r => r.JbLetter).ThenBy(r => r.JbNumberA).ToList();
            var result = "";
            var url = "http://192.168.0.110:75/api/jukebox/play/";
            foreach (JukeBoxSelection row in aList)
            {
                var number = row.JbNumberA.Replace("0", "");
                result = result + row.JbLetter + row.JbNumberA + ": " + row.Artist1 + " - " + row.A1Song + "\n";
            }
            return result;
        }

        private List<JukeBoxSelection> GetAllASongs()
        {
            var sql = "";
            sql = "SELECT jbletter,jbnumbera,Artist1,A1Song FROM amijukebox.jbselection WHERE Archived=0";
            using (System.Data.IDbConnection db = new MySqlConnection("Server = 127.0.0.1; Uid = lasse; Pwd = zals69; Database = amijukebox;"))
            {
                db.Open();
                return db.Query<JukeBoxSelection>(sql).ToList<JukeBoxSelection>();
            }
        }

        public class JukeBoxSelection
        {
            public string JbLetter { get; set; }
            public string JbNumberA { get; set; }
            public string Artist1 { get; set; }
            public string A1Song { get; set; }
        }

        // GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

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
