using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Models;
using Microsoft.AspNetCore.Authorization;

namespace LigaIsadoraSilva.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;

        public DashboardController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentDate = DateTime.Now;

            // Obtenha as partidas que podem ser fechadas, incluindo os times associados
            var matchesToClose = await _context.Games // Certifique-se de usar a entidade correta
                .Include(m => m.HomeTeam) // Inclui o time da casa
                .Include(m => m.VisitTeam) // Inclui o time visitante
                .Where(m => m.StartDate < currentDate && !m.IsFinalized) // Certifique-se de ter a lógica para verificar se a partida está fechada
                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                MatchesToClose = matchesToClose
            };

            return View(viewModel);
        }

    }
}
