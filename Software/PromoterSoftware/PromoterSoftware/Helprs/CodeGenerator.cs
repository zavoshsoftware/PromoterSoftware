using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Helpers
{
    public class CodeGenerator
    {
        private DatabaseContext db = new DatabaseContext();
      
        public int ReturnUserCode()
        {
            User user = db.Users.Where(c => c.IsDeleted == false).OrderByDescending(current => current.Code).FirstOrDefault();

            if (user != null)
            {
                return Convert.ToInt32(user.Code) + 1;
            }
            return 100;
        }


    }
}