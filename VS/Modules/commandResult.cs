using Discord.Commands;

namespace Westbot
{
    //Note: When a reason is given, thsi converts the result to an error
    public class WestbotCommandResult : RuntimeResult
    {
        readonly bool DontReact;

        public WestbotCommandResult(CommandError? error, string reason, bool react_state) : base(error, reason)
        {
            DontReact = react_state;
        }

        public WestbotCommandResult(CommandError? error, string reason) : base(error, reason) {

        }

        public bool Get_reaction()
        {
            return DontReact;
        }

        public static WestbotCommandResult AcceptReact(string reason = null, bool react_state = true) =>
            new WestbotCommandResult(null, reason, react_state);

        public static WestbotCommandResult AcceptNoReaction(string reason = null, bool react_state = false) =>
            new WestbotCommandResult(null, reason, react_state);

        public static WestbotCommandResult ErrorReact(string reason, bool react_state = true) =>
            new WestbotCommandResult(CommandError.Unsuccessful, reason, react_state);

        public static WestbotCommandResult ErrorNoReact(string reason, bool react_state = false) =>
            new WestbotCommandResult(CommandError.Unsuccessful, reason, react_state);
    }
}
