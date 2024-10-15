using LigaIsadoraSilva.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LigaIsadoraSilva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase // Altere para ControllerBase para APIs
    {
        private readonly DataContext _context;

        public ApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("standings")]
        public async Task<ActionResult<IEnumerable<TeamStandingDto>>> GetStandings()
        {
            // Obtém todas as partidas finalizadas
            var matches = await _context.Games
                .Where(m => m.IsFinalized)
                .Include(m => m.HomeTeam)
                .Include(m => m.VisitTeam)
                .ToListAsync();

            // Dicionário para armazenar os pontos dos times
            var teamPoints = new Dictionary<string, int>();

            // Calcula os pontos para cada time
            foreach (var match in matches)
            {
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

            // Converte o dicionário para uma lista ordenada
            var standingsList = teamPoints.Select(tp => new TeamStandingDto
            {
                TeamName = tp.Key,
                Points = tp.Value
            })
            .OrderByDescending(s => s.Points)
            .ToList();

            return Ok(standingsList);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<MatchStatsDto>> GetMatchStats()
        {
            var matches = await _context.Games
                .Where(m => m.IsFinalized)
                .ToListAsync();

            var totalGames = matches.Count();
            var totalGoals = matches.Sum(m => (m.HomeGoals ?? 0) + (m.VisitGoals ?? 0));
            var totalYellowCards = matches.Sum(m => m.YellowCards ?? 0);
            var totalRedCards = matches.Sum(m => m.RedCards ?? 0);
            var homeWins = matches.Count(m => m.HomeGoals > m.VisitGoals);

            var stats = new MatchStatsDto
            {
                TotalGames = totalGames,
                TotalGoals = totalGoals,
                TotalYellowCards = totalYellowCards,
                TotalRedCards = totalRedCards,
                HomeWins = homeWins
            };

            return Ok(stats);
        }
    }

    // DTOs para o retorno da API
    public class TeamStandingDto
    {
        public string TeamName { get; set; }
        public int Points { get; set; }
    }

    public class MatchStatsDto
    {
        public int TotalGames { get; set; }
        public int TotalGoals { get; set; }
        public int TotalYellowCards { get; set; }
        public int TotalRedCards { get; set; }
        public int HomeWins { get; set; }
    }
}
