using CartMicroservice.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartMicroservice.Controllers
{
    public class CartsController: ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        

    }
}
