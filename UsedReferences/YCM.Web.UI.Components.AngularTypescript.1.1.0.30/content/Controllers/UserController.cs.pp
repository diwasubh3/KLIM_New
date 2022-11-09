using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using $rootnamespace$.Models;

namespace $rootnamespace$.Controllers
{
    [NoCache]
    public class UserController : ApiController
    {
        private List<UserModel> usersModels; 

        public UserController()
        {
            usersModels = new List<UserModel>() { new UserModel() { FirstName = "Santosh", LastName = "Singh" }, new UserModel() { FirstName = "Adam", LastName = "Gentry" } };
        }

        // GET: api/User
        public IEnumerable<UserModel> Get()
        {
            return usersModels;
        }

        // GET: api/User/5
        public UserModel Get(int id)
        {
            return usersModels.First(u=>u.Id==id);
        }

        // POST: api/User
        public bool Post([FromBody]UserModel value)
        {
            try
            {
                usersModels.Add(value);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // PUT: api/User/5
        public bool Put(int id, [FromBody]UserModel value)
        {
            try
            {
                usersModels.First(u => u.Id == value.Id).FirstName = value.FirstName;
                usersModels.First(u => u.Id == value.Id).LastName = value.LastName;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
            usersModels.Remove(usersModels.Find(u => u.Id == id));
        }
    }
}