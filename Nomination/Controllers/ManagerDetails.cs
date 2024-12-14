using Microsoft.AspNetCore.Mvc;
using Nomination.Models;
using Snowflake.Data.Client;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nomination.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerDetails : ControllerBase
    {
        private readonly IDbConnection _dbConnection;

        public ManagerDetails(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _dbConnection.Open();
        }
        // GET: api/<ManagerDetails>
        [HttpGet]
        public IActionResult Get()
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            DataTable dataTable = new DataTable();
            try
            {

                var cmd = _dbConnection.CreateCommand();
                cmd.CommandText = "select * from nominationmanager limit 100;";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    result.Add(row);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return Ok(result);
        }

        // GET api/<ManagerDetails>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManagerDetails>
        [HttpPost]
        public IActionResult Post(NominationManager nominationManager)
        {
            using (IDbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO NominationManager (MANAGERNAME, PERIOD, LOCATION, LEADERSHIPMEMBER,ID) VALUES (?, ?, ?, ?, ?)";
                AddParameters(command, nominationManager);

                command.ExecuteNonQuery();
            }
            return Ok();
        }

        // PUT api/<ManagerDetails>/5
        [HttpPut("{id}")]
        public IActionResult Put(NominationManager nominationManager)
        {
            using (IDbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandText = @"UPDATE NominationManager SET MANAGERNAME = ?, PERIOD = ?, LOCATION = ?, LEADERSHIPMEMBER = ? WHERE ID = ? ";
                AddParameters(command, nominationManager, true);

                command.ExecuteNonQuery();
            }
            return Ok();
        }

        // DELETE api/<ManagerDetails>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (IDbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandText = "DELETE FROM NominationManager WHERE Id = ?";
                var parameter = new SnowflakeDbParameter
                {
                    ParameterName = "1",
                    Value = id,
                    DbType = DbType.Int32
                };
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
            }
            return Ok();
        }

        private void AddParameters(IDbCommand command, NominationManager nominationManager, bool isUpdate = false)
        {
            var p2 = command.CreateParameter();
            p2.ParameterName = "1";
            p2.Value = nominationManager.ManagerName;
            p2.DbType = DbType.String;
            command.Parameters.Add(p2);

            var p3 = command.CreateParameter();
            p3.ParameterName = "2";
            p3.Value = nominationManager.Period;
            p3.DbType = DbType.String;
            command.Parameters.Add(p3);

            var p4 = command.CreateParameter();
            p4.ParameterName = "3";
            p4.Value = nominationManager.Location;
            p4.DbType = DbType.String;
            command.Parameters.Add(p4);

            var p5 = command.CreateParameter();
            p5.ParameterName = "4";
            p5.Value = nominationManager.LeadershipMember;
            p5.DbType = DbType.String;
            command.Parameters.Add(p5);

            var p1 = command.CreateParameter();
            p1.ParameterName = "5";
            p1.Value = nominationManager.Id;
            p1.DbType = DbType.Int32;
            command.Parameters.Add(p1);
        }
    }
}
