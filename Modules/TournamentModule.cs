using Discord.Commands;
using Westbot.Preconditions;
using System.Threading.Tasks;
using Discord;
using System.Linq;
using System.Collections.Generic;

namespace Westbot
{
    //The round object for the tournament. Stores a group of games that would be represented as being along a single column,
    //and within a single side of the bracket, either winners or losers.
    public class Round
    {
        //This is which order the round is in. This is only relative to the winner or losers bracket.
        private int Round_number;

        //This is the name for the round, such as "winners finals"
        private string Round_name;

        //Determines if this round is in winners bracket or not
        private bool WinnersOrLosersBracket;

        //This is a list of all the games in this round. Should only be games within the same bracket level, either all winners or losers.
        private List<Game> Games_in_round;

        //Round objects are always created prior to games
        public Round(string name_set, bool winners_set)
        {
            Round_number = 0;
            Round_name = name_set;
            WinnersOrLosersBracket = winners_set;
        }

        public void Increase_round_count()
        {
            ++Round_number;
        }
    }

    //This class is for allowing each player to store a record of the games they've played, with an added variable for 
    //ordering the games.
    //Players will store a list of GameRecords, one for each completed game.
    public class GameRecord
    {
        //Allows for an index when the GameRecords are inserted into a list
        private int Order;
        private Game Game_played;
    }

    //Participants are the players in the tournament.
    public class Participant
    {

        private string Name;

        //ID is generated in the order the player is added to the bracket,
        //But this has no bearing on any functionality.
        private int ID;

        //The players game history so far; will also include the current game, prior to it being finished.
        private List<GameRecord> Game_History;

        private int Wins;
        private int Losses;

        //The seed value for the player.
        //This is a reflection of the player's skill.
        //This is used to rank each player, and seeds are given in ascending order, starting with 1.
        //The list of players in the tournament is ordered by seed.
        private int Seed;

        public Participant()
        {
            Name = "None";
            ID = 0;
            Game_History = new List<GameRecord>();
            Wins = 0;
            Losses = 0;
            Seed = 0;
        }
    }

    //This is a game within a round.
    public class Game
    {
        //The players in this game
        private Participant Player_one;
        private Participant Player_two;

        //This games number within the round it is in; for example, if there are 8 games in round 1, the games would be numbered 1-8.
        private int GameNumberWithinRound;

        //This game's number in terms of order games should be played, to give maximum time between games for players
        private int WholeTournamentGameNumber;

        //The round this game belongs to
        private Round ContainingRound;

        //The game the winner of this match advances to
        private Game Winner_Advance;

        //The game the loser of this match goes to
        private Game Loser_Advance;

        //If applicable, the games that the players in this bracket came from. 
        private Game Source_1;

        //Source 2 will be the game from winners bracket for losers bracket matches, if applicable
        private Game Source_2;

        //Games will be created within rounds.
        //Each round will provide itself to the game in the constructor, so the game can point back to it.
        public Game(Round containing_round)
        {
            Player_one = new Participant();
            Player_two = new Participant();
            GameNumberWithinRound = 0;
            ContainingRound = containing_round;
        }
    }

    //The object that stores the entire tournament.
    public class Tournament
    {
        //List of players
        private List<Participant> Player_list;

        //A list of rounds that just contain winners bracket games
        private List<Round> Winners_Rounds;

        //A list of rounds that just contain losers bracket games
        private List<Round> Losers_Rounds;      

        public Tournament()
        {
            Player_list = new List<Participant>();
            Winners_Rounds = new List<Round>();
            Losers_Rounds = new List<Round>();
        }

        public void Increase_winners_round_count()
        {
            foreach(Round temp_round in Winners_Rounds)
            {
                temp_round.Increase_round_count();
            }

        }
    }

    [Name("tournament commands")]
    public class TournamentModule : ModuleBase<SocketCommandContext>
    {
        private readonly Tournament _tournament;
        public TournamentModule(Tournament tournament)
        {
            _tournament = tournament;
        }

        [Command("check1")]
        [Remarks("testing")]
        [MinPermissions(AccessLevel.User)]
        public async Task Tourn()
        {
            await ReplyAsync("Test");
        }

        [Command("check2")]
        [Remarks("check")]
        [MinPermissions(AccessLevel.User)]
        public async Task Tourncheck()
        {
            await ReplyAsync("test");
        }

        [Command("checkin"), Alias("c", "check in", "check-in")]
        [Remarks("Check yourself in")]
        [MinPermissions(AccessLevel.User)]
        public async Task Checkin()
        {
            var user = Context.User;
            var checkedIn = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Checked In");

            if (checkedIn == null)
            {
                await ReplyAsync("Checking in is not enabled at the moment.");
            }
            else
            {
                await (user as IGuildUser).AddRoleAsync(checkedIn);
                await ReplyAsync($"{user.Mention} is checked in.");
            }
        }

        [Command("open check ins"), Alias("o")]
        [Remarks("Open check ins")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task OpenCheckIn()
        {
            var checkedIn = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Checked In");
            if (checkedIn != null)
            {
                await ReplyAsync("Check ins are already open.");
            }

            else
            {
                await Context.Guild.CreateRoleAsync("Checked In", null, null, true);
                await ReplyAsync("Check ins are now open.");
            }
        }

        [Command("tournament start"), Alias("s")]
        [Remarks("Start the tournament")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Start()
        {
            //Define variables for roles
            var registered = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Registered");
            var checkedIn = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Checked In");
            var inTournament = Context.Guild.Roles.FirstOrDefault(x => x.Name == "In Tournament");

            //Get a list of users in the CURRENT channel (command must be run from tournament_chat)
            var users = (await Context.Channel.GetUsersAsync().FlattenAsync());

            //Add in tournament to each user with "checked in", and remove the registered role
            foreach (IUser user in users)
            {
                var roleList = (user as IGuildUser).RoleIds;
                foreach(var u in roleList)
                {
                    if(u == checkedIn.Id)
                        await (user as IGuildUser).AddRoleAsync(inTournament);

                    await (user as IGuildUser).RemoveRoleAsync(registered);
                }
            }

            //Delete checked in role; it will be created when check ins open for the next tournament.
            await checkedIn.DeleteAsync();

            await ReplyAsync("Tournament started.");
        }

        [Command("tournament end"), Alias("e")]
        [Remarks("Deletes and recreates roles")]
        [MinPermissions(AccessLevel.User)]
        public async Task RemoveTournamentRoles()
        {
            //Define variables for each role
            var inTournament = Context.Guild.Roles.FirstOrDefault(x => x.Name == "In Tournament");
            var registered = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Registered");
            var users = (await Context.Channel.GetUsersAsync().FlattenAsync());

            //Remove the In Tournament from all roles in the current channel (must run this command from the tournament_chat)
            foreach (IUser user in users)
            {
                await (user as IGuildUser).RemoveRoleAsync(inTournament);
            }

            await ReplyAsync("Tournament has ended.");
        }
    }
    
}
