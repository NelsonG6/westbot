using Discord.Commands;
using Westbot.Preconditions;
using System.Threading.Tasks;
using Discord;
using System.Linq;
using System.Collections.Generic;

namespace Westbot
{
    public class Round
    {
        //This is which order the round is in.
        private int Round_number;

        //This is the name for the round, such as "winners finals"
        private string Round_name;

        //Determines if this round is in winners bracket or not
        private bool Winners;

        //This is a list of all the games in this round. May or may not be both winners and losers bracket games.
        private List<Game> Games_in_round;

        public Round(string name_set, bool winners_set)
        {
            Round_number = 1;
            Round_name = name_set;
            Winners = winners_set;
        }

        public void Increase_round_count()
        {
            ++Round_number;
        }
    }

    //This class is for allowing each player to store a record of the games they've played, with an added variable for 
    //ordering the games.
    public class GameRecord
    {
        //Allows for an index when the GameRecords are inserted into a list
        private int Order;
        private Game Game_played;
    }

    public class Participant
    {
        private string Name;
        private int ID;

        //The players game history so far; will also include the current game, prior to it being finished.
        private List<GameRecord> Game_History;

        private int Wins;
        private int Losses;
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

    public class Game
    {
        //The players in this game
        private Participant Player_one;
        private Participant Player_two;

        //This games number within the round it is in; for example, if there are 8 games in round 1, the games would be numbered 1-8.
        private int GameNumberWithinRound;

        //This game's number in terms of order games should be played, to give maximum time between games for players
        private int WholeTournamentGameNumber;

        //The game's round number
        private int RoundNumber;

        //If this game is in winner's backet. False means losers bracket.
        private bool Winner;

        //The game the winner of this match advances to
        private Game Winner_Advance;

        //The game the loser of this match goes to
        private Game Loser_Advance;

        //If applicable, the games that the players in this bracket came from. 
        private Game Source_1;

        //Source 2 will be the game from winners bracket for losers bracket matches, if applicable
        private Game Source_2;

        public Game()
        {
            Player_one = new Participant();
            Player_two = new Participant();
            GameNumberWithinRound = 0;

        }
    }

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
