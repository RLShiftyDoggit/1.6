using MultiServer.Addons.Horizon.RT.Common;
using MultiServer.Addons.Horizon.LIBRARY.Common.Stream;

namespace MultiServer.Addons.Horizon.RT.Models
{
    [MediusMessage(NetMessageClass.MessageClassLobby, MediusLobbyMessageIds.SessionEnd)]
    public class MediusSessionEndRequest : BaseLobbyMessage, IMediusRequest
    {
        public override byte PacketType => (byte)MediusLobbyMessageIds.SessionEnd;

        public MessageId MessageID { get; set; }

        public string SessionKey; // SESSIONKEY_MAXLEN

        public override void Deserialize(MessageReader reader)
        {
            // 
            base.Deserialize(reader);

            //
            MessageID = reader.Read<MessageId>();

            // 
            SessionKey = reader.ReadString(Constants.SESSIONKEY_MAXLEN);
        }

        public override void Serialize(MessageWriter writer)
        {
            // 
            base.Serialize(writer);

            //
            writer.Write(MessageID ?? MessageId.Empty);

            // 
            writer.Write(SessionKey, Constants.SESSIONKEY_MAXLEN);
        }


        public override string ToString()
        {
            return base.ToString() + " " +
                $"MessageID: {MessageID} " +
                $"SessionKey: {SessionKey}";
        }
    }
}
