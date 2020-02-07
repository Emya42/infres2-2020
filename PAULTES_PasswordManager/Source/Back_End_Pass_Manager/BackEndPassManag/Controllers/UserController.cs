using BackEndPassManag.Models;
using BackEndPassManag.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BackEndPassManag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string CleChiffreDechiffre = "lacledecryptageestassezsimpleokk";
        private static string LeChallengeUser = "";
        private readonly string ConnectionString = @"";
        public UserController()
        {
            Microsoft.Extensions.Configuration.IConfigurationRoot appSettingsJson = AppSettingsJson.GetAppSettings();
            string connectionString = appSettingsJson["DefaultConnection"];
            ConnectionString = connectionString;
        }

        // GET: api/User/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id,User}", Name = "Get")]
        public ActionResult<BaseMdps> Get(int id, string User)
        {
            bool Resultat = false;
            BaseMdps utilisateur = new BaseMdps();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetUser", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", User);

                    using (SqlDataReader DR = cmd.ExecuteReader())
                    {
                        while (DR.Read())
                        {
                            utilisateur.Salt = (byte[])DR["Salt"];
                            utilisateur.Id = DR.GetInt64(DR.GetOrdinal("Id"));
                            Resultat = true;
                        }
                    }
                }
                connection.Close();
            }
            if (Resultat)
            {
                Crypto MonCrypt = new Crypto();
                utilisateur.ChallengeUser = MonCrypt.Encrypt(MonCrypt.RandomString(32, false), MonCrypt.RandomString(32, false));
                LeChallengeUser = utilisateur.ChallengeUser;
                return Ok(utilisateur);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/User
        [HttpPost]
        public void Post(BaseMdps model)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                connection.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CreateUser", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", model.User);
                    cmd.Parameters.AddWithValue("@email", model.Email);
                    cmd.Parameters.AddWithValue("@HA", model.HA);
                    cmd.Parameters.AddWithValue("@Salt", model.Salt);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // PUT: api/User/5
        [HttpPut]
        public ActionResult Put(BaseMdps model)
        {
            BaseMdps UserMdp = new BaseMdps();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetUser", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", model.User);

                    using (SqlDataReader DR = cmd.ExecuteReader())
                    {
                        while (DR.Read())
                        {
                            UserMdp.HA = DR["HA"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            Crypto MonCrypt = new Crypto();
            string variableChallenge = LeChallengeUser + UserMdp.HA;
            string ChallengeUser = MonCrypt.Encrypt(variableChallenge, CleChiffreDechiffre);
            if (ChallengeUser == model.ChallengeUser)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
