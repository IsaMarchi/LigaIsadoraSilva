using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using System.Collections.Generic;

namespace LigaIsadoraSilva.Services
{
    public class MatchService: IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<List<FootballMatch>> GenerateMatchesAsync(List<FootballTeam> teams)
        {
            var matches = new List<FootballMatch>();
            int numberOfTeams = teams.Count;

            if (numberOfTeams % 2 != 0)
            {
                throw new ArgumentException("O número de times deve ser par.");
            }

            int totalJournadas = numberOfTeams - 1;

            for (int jornada = 0; jornada < totalJournadas; jornada++)
            {
                for (int i = 0; i < numberOfTeams / 2; i++)
                {
                    var homeTeam = teams[i];
                    var visitTeam = teams[numberOfTeams - 1 - i];

                    var match = new FootballMatch
                    {
                        HomeTeamId = homeTeam.Id,
                        VisitTeamId = visitTeam.Id,
                        StartDate = DateTime.Now.AddDays((jornada + 1) * 7) // Exemplo: cada jornada uma semana depois da anterior
                    };

                    matches.Add(match);
                }

                RotateTeams(teams);
            }

            return await Task.FromResult(matches);
        }

        private void RotateTeams(List<FootballTeam> teams)
        {
            var firstTeam = teams[0];
            teams.RemoveAt(0);
            teams.Add(firstTeam);
        }
    }
}
