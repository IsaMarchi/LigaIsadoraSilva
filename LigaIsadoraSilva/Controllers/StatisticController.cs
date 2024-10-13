using LigaIsadoraSilva.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LigaIsadoraSilva.Controllers
{
    public class StatisticController : Controller
    {
        private readonly DataContext _context;

        public StatisticController(DataContext context)
        {
            _context = context;
        }

        public IActionResult TeamStandings()
        {
            // Obtém todas as partidas
            var matches = _context.Games
                                  .Include(m => m.HomeTeam)
                                  .Include(m => m.VisitTeam)
                                  .ToList();

            // Dicionário para armazenar os pontos dos times
            var teamPoints = new Dictionary<string, int>();

            // Calcula os pontos para cada time
            foreach (var match in matches)
            {
                // Inicializa os times no dicionário, se ainda não estiverem
                if (!teamPoints.ContainsKey(match.HomeTeam.Name))
                    teamPoints[match.HomeTeam.Name] = 0;

                if (!teamPoints.ContainsKey(match.VisitTeam.Name))
                    teamPoints[match.VisitTeam.Name] = 0;

                // Adiciona pontos baseados no resultado da partida
                if (match.HomeGoals > match.VisitGoals) // Vitória do time da casa
                {
                    teamPoints[match.HomeTeam.Name] += 3;
                }
                else if (match.HomeGoals < match.VisitGoals) // Vitória do time visitante
                {
                    teamPoints[match.VisitTeam.Name] += 3;
                }
                else // Empate
                {
                    teamPoints[match.HomeTeam.Name] += 1;
                    teamPoints[match.VisitTeam.Name] += 1;
                }
            }

            // Ordena por pontuação decrescente
            var sortedTeamPoints = teamPoints.OrderByDescending(tp => tp.Value).ToList();

            return View(sortedTeamPoints);
        }

        public IActionResult Index()
        {
            var matches = _context.Games.ToList();

            var totalHomeGoals = matches.Sum(m => m.HomeGoals);
            var totalVisitGoals = matches.Sum(m => m.VisitGoals);
            var totalGames = matches.Count();
            var homeWins = matches.Count(m => m.HomeGoals > m.VisitGoals);
            var visitWins = matches.Count(m => m.VisitGoals > m.HomeGoals);

            var stats = new
            {
                totalHomeGoals,
                totalVisitGoals,
                totalGames,
                homeWins,
                visitWins
            };

            return View(stats);
        }
    }
}
