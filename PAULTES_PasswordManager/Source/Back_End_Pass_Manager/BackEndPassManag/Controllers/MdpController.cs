using BackEndPassManag.Models;
using BackEndPassManag.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BackEndPassManag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MdpController : ControllerBase
    {
        private readonly string ConnectionString = @"";
        public MdpController()
        {
            Microsoft.Extensions.Configuration.IConfigurationRoot appSettingsJson = AppSettingsJson.GetAppSettings();
            string connectionString = appSettingsJson["DefaultConnection"];
            ConnectionString = connectionString;
        }


        [HttpGet("{User}", Name = "GetMdp")]
        public List<MdpModels> GetMdp(string User)
        {
            List<MdpModels> correspond = new List<MdpModels>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetMdp", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", User);

                    using (SqlDataReader DR = cmd.ExecuteReader())
                    {
                        while (DR.Read())
                        {
                            MdpModels corres = new MdpModels
                            {
                                Mdp = DR["mdp"].ToString(),
                                Id = DR.GetInt64(DR.GetOrdinal("Id")),
                                ReferenceSite = DR["ReferenceSite"].ToString()
                            };
                            correspond.Add(corres);
                        }
                    }
                }
                connection.Close();
            }
            return correspond;
        }

        // POST: api/Mdp
        [HttpPost]
        public void Post(MdpModels model)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("sp_UpdMdp", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Ref", model.ReferenceSite);
                    cmd.Parameters.AddWithValue("@mdp", model.Mdp);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // PUT: api/Mdp/5
        [HttpPut]
        public void Put(MdpModels model)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("sp_AddMdp", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@site", model.ReferenceSite);
                    cmd.Parameters.AddWithValue("@mdp", model.Mdp);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("sp_DelMdp", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
