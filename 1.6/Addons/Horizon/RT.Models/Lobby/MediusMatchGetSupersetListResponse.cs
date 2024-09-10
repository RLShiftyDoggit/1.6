using MultiServer.Addons.Horizon.RT.Common;
using MultiServer.Addons.Horizon.LIBRARY.Common.Stream;

namespace MultiServer.Addons.Horizon.RT.Models
{
    /// <summary>
    /// Response returning a list of Match Supersets from the database <br></br>
    /// StatusCode: MediusNoResult, MediusSuccess
    /// </summary>
    [MediusMessage(NetMessageClass.MessageClassLobbyExt, MediusLobbyExtMessageIds.MatchGetSupersetListResponse)]
    public class MediusMatchGetSupersetListResponse : BaseLobbyExtMessage, IMediusRequest
    {

        public override byte PacketType => (byte)MediusLobbyExtMessageIds.MatchGetSupersetListResponse;

        /// <summary>
        /// Message ID
        /// </summary>
        public MessageId MessageID { get; set; }
        /// <summary>
        /// Response code to fetching superset list
        /// </summary>
        public MediusCallbackStatus StatusCode;
        /// <summary>
        /// End of List flag
        /// </summary>
        public bool EndOfList;
        /// <summary>
        /// Superset ID
        /// </summary>
        public long SupersetID;
        /// <summary>
        /// Superset Name
        /// </summary>
        public string SupersetName;
        /// <summary>
        /// Superset Description
        /// </summary>
        public string SupersetDescription;
        /// <summary>
        /// Superset ExtraInfo
        /// </summary>
        public string ExtraInfo;

        public override void Deserialize(MessageReader reader)
        {
            // 
            base.Deserialize(reader);

            //
            MessageID = reader.Read<MessageId>();

            //
            StatusCode = reader.Read<MediusCallbackStatus>();
            EndOfList = reader.Read<bool>();

            //
            SupersetID = reader.ReadInt32();
            SupersetName = reader.ReadString(Constants.SUPERSETNAME_MAXLEN);
            SupersetDescription = reader.ReadString(Constants.SUPERSETDESCRIPTION_MAXLEN);
            ExtraInfo = reader.ReadString(Constants.SUPERSETEXTRAINFO_MAXLEN);
        }

        public override void Serialize(MessageWriter writer)
        {
            // 
            base.Serialize(writer);

            //
            writer.Write(MessageID ?? MessageId.Empty);

            // 
            writer.Write(StatusCode);
            writer.Write(EndOfList);

            //
            writer.Write(SupersetID);
            writer.Write(SupersetName, Constants.SUPERSETNAME_MAXLEN);
            writer.Write(SupersetDescription, Constants.SUPERSETDESCRIPTION_MAXLEN);
            writer.Write(ExtraInfo, Constants.SUPERSETEXTRAINFO_MAXLEN);
        }

        public override string ToString()
        {
            return base.ToString() + " " +
                $"MessageID: {MessageID} " +
                $"StatusCode: {StatusCode} " +
                $"SupersetID: {SupersetID} " +
                $"SupersetName: {SupersetName} " +
                $"SupersetDescription: {SupersetDescription} " +
                $"SupersetExtraInfo: {ExtraInfo} " +
                $"EndOfList: {EndOfList}";
        }
    }
}
