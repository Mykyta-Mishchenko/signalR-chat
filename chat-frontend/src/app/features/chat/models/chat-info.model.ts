import { ChatParticipant } from "./chat-participant.model";
import { ChatType } from "./chat-type.enum";

export interface ChatInfo{
    id: number;
    name: string;
    chatType: ChatType;
    participants: ChatParticipant[];
    lastMessage: string;
    lastMessageTimestamp: Date;
    unreadMessagesCount: number;
}