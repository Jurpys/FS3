using System;
using System.Collections.Generic;
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
                return GameResult(Maybe<Dictionary<string, Dictionary<string, string>>>.Bind(decodedMessage).FMap(WinnerDeterminator.FindWinner));
            }

            Task<MarkType> winnerFinderTask = Task.Factory.StartNew(() => WinnerDeterminator.FindWinner(decodedMessage));

            var newMove = MoveFinder.GetRandomMove(MessageDecoder.DecodeToCoordinates(decodedMessage));

            winnerFinderTask.Wait();

            var winner = Maybe<MarkType>.Bind(winnerFinderTask.Result);

            if (!winner.ToString().Equals("Just Nothing"))
            {
                return GameResult(winner);
            }

            var newMessage = MessageEncoder.UpdateMessage(message, decodedMessage.Count.ToString(), newMove);

            Task postNewMessageTask = Task.Factory.StartNew(() => GameHttpClient.Post(gameId, newMessage));

            var newWinner =
                Maybe<string>.Bind(newMessage).FMap(MessageDecoder.Execute).FMap(WinnerDeterminator.FindWinner);

            if (!newWinner.ToString().Equals("Just Nothing"))
            {
                return GameResult(newWinner);
            }

            postNewMessageTask.Wait();

            return Play(gameId);
        }

        private static string GameResult(Maybe<MarkType> mark)
        {
            switch (mark.ToString())
            {
                case "Just Nothing":
                    return "Draw";
                case "Just O":
                    return "You won";
                case "Just X":
                    return "You lost";
            }

            return "Something went wrong";
        } 
    }
}