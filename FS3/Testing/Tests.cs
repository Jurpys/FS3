using System.Collections.Generic;
using FsCheck;
using FS3.Types;
using Xunit;

namespace FS3.Testing
{
    public class Tests
    {
        [Fact]
        public void MoveProperty()
        {
            var property = Prop.ForAll(MovesArbitrary.Move(),
                move =>
                {
                    var message =
                        "d1:0d1:v1:x1:xi2e1:yi1ee1:1d1:v1:o1:xi1e1:yi2ee1:2d1:v1:x1:xi0e1:yi0ee1:3d1:v1:o1:xi0e1:yi2ee1:4d1:v1:x1:xi2e1:yi0ee1:5d1:v1:o1:xi2e1:yi2eee";
                    var dictionaryFromMsg = MessageDecoder.Execute(message);

                    var newMessage = MessageEncoder.UpdateMessage(message, dictionaryFromMsg.Count.ToString(), move);
                    var dictionaryFromNewMessage = MessageDecoder.Execute(newMessage);

                    dictionaryFromMsg.Add(dictionaryFromMsg.Count.ToString(), new Dictionary<string, string>()
                    {
                        {"v", "o"},
                        {"x", move.Item1.ToString()},
                        {"y", move.Item2.ToString()}
                    });

                    Assert.Equal(dictionaryFromMsg, dictionaryFromNewMessage);
                });

            property.QuickCheckThrowOnFailure();
        }
    }
}