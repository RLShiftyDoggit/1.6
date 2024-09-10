using MultiServer.Addons.Horizon.RT.Common;
using MultiServer.Addons.Horizon.LIBRARY.Common.Stream;

namespace MultiServer.Addons.Horizon.RT.Models
{
    [MediusMessage(NetMessageClass.MessageClassLobbyExt, MediusLobbyExtMessageIds.BinaryFwdMessage1)]
    public class MediusBinaryFwdMessage1 : BaseLobbyExtMessage
    {

        public override byte PacketType => (byte)MediusLobbyExtMessageIds.BinaryFwdMessage1;

        public MessageId MessageID { get; set; }

        public int OriginatorAccountID;
        public MediusBinaryMessageType MessageType;
        public byte[] Message = new byte[Constants.BINARYMESSAGE_MAXLEN];

        public override void Deserialize(MessageReader reader)
        {
            // 
            base.Deserialize(reader);

            //
            MessageID = reader.Read<MessageId>();

            //
            //reader.ReadBytes(3);
            OriginatorAccountID = reader.ReadInt32();
            MessageType = reader.Read<MediusBinaryMessageType>();
            Message = reader.ReadBytes(Constants.BINARYMESSAGE_MAXLEN);
        }

        public override void Serialize(MessageWriter writer)
        {
            // 
            base.Serialize(writer);

            //
            writer.Write(MessageID ?? MessageId.Empty);

            // 
            //writer.Write(new byte[3]);
            writer.Write(OriginatorAccountID);
            writer.Write(MessageType);
            writer.Write(Message, Constants.BINARYMESSAGE_MAXLEN);
        }


        public override string ToString()
        {
            return base.ToString() + " " +
                $"MessageID:{MessageID} " +
                $"OriginatorAccountID:{OriginatorAccountID} " +
                $"MessageType:{MessageType} " +
                $"Message:{BitConverter.ToString(Message)}";
        }
    }
}
