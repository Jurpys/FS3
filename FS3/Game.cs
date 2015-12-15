using System;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using FS3.Enum;

namespace FS3
{
    public static class Game
    {
        public static string Play(string gameId)
        {
            var message = GameHttpClient.Get(gameId);

            var decodedMessage = MessageDecoder.Execute(message);

            if (decodedMessage.Count >= 9)
            {
                return GameResult(WinnerDeterminator.FindWinner(decodedMessage));
            }

            Task<MarkType> winnerFinderTask = Task.Factory.StartNew(() => WinnerDeterminator.FindWinner(decodedMessage));

            var newMove = MoveFinder.GetRandomMove(MessageDecoder.DecodeToCoordinates(decodedMessage));

            winnerFinderTask.Wait();

            var winner = winnerFinderTask.Result;

            if (winner != MarkType.Nothing)
            {
                return GameResult(winner);
            }

            var newMessage = MessageEncoder.UpdateMessage(message, decodedMessage.Count.ToString(), newMove);

            Task postNewMessageTask = Task.Factory.StartNew(() => GameHttpClient.Post(gameId, newMessage));

            var newWinner = WinnerDeterminator.FindWinner(MessageDecoder.Execute(newMessage));

            if (newWinner != MarkType.Nothing)
            {
                return GameResult(newWinner);
            }

            postNewMessageTask.Wait();

            return Play(gameId);
        }

        private static string GameResult(MarkType mark)
        {
            switch (mark)
            {
                case MarkType.Nothing:
                    return "Draw";
                case MarkType.O:
                    return "You won";
                case MarkType.X:
                    return "You lost";
            }

            return "Something went wrong";
        } 
    }
}