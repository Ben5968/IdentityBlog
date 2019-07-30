using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blog.ViewModels
{
    public class Users_in_Role_ViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        //public IEnumerable<SelectListItem> UserRoles { get; set; }
        public SelectList UserRoles { get; set; }
        public int SelectedRoleId { get; set; }
    }
}
