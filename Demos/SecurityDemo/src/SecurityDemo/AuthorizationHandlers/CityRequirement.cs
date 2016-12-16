using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityDemo.AuthorizationHandlers
{
    public class CityRequirement : IAuthorizationRequirement
    {
        public CityRequirement(string city)
        {
            City = city;
        }
        public string City { get; set; }
    }

}
