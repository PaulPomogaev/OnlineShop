using Microsoft.AspNetCore.Identity;
using OnlineShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class Role : IdentityRole<int>, IBaseId
    {

    }
}
