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
                                  .Where(m => m.IsFinalized)
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
                    teamPoints[match.HomeTeam.Name] += 3; // 3 pontos para o time da casa
                }
                else if (match.HomeGoals < match.VisitGoals) // Vitória do time visitante
                {
                    teamPoints[match.VisitTeam.Name] += 3; // 3 pontos para o time visitante
                }
                else // Empate
                {
                    teamPoints[match.HomeTeam.Name] += 1; // 1 ponto para o time da casa
                    teamPoints[match.VisitTeam.Name] += 1; // 1 ponto para o time visitante
                }
            }

            // Ordena por pontuação decrescente
            var sortedTeamPoints = teamPoints.OrderByDescending(tp => tp.Value).ToList();

            return View(sortedTeamPoints);
        }

        public IActionResult Index()
        {
            var matches = _context.Games
                .Where(m => m.IsFinalized) // Somente jogos finalizados
                .ToList();

            var totalGames = matches.Count(); // Total de jogos realizados
            var totalGoals = matches.Sum(m => (m.HomeGoals ?? 0) + (m.VisitGoals ?? 0)); // Total de gols marcados
            var totalYellowCards = matches.Sum(m => m.YellowCards ?? 0); // Total de cartões amarelos
            var totalRedCards = matches.Sum(m => m.RedCards ?? 0); // Total de cartões vermelhos
            var homeWins = matches.Count(m => m.HomeGoals > m.VisitGoals); // Total de vitórias em casa

            var stats = new
            {
                TotalGames = totalGames,
                TotalGoals = totalGoals,
                TotalYellowCards = totalYellowCards,
                TotalRedCards = totalRedCards,
                HomeWins = homeWins
            };

            return View(stats);
        }
    }
}

