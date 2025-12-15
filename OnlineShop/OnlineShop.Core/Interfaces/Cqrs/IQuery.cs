using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Interfaces.Cqrs
{
    public interface IQuery<out TResult>
    {
    }
}
