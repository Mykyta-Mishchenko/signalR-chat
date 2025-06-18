import { ChatParticipant } from "./chat-participant.model";

export interface ChatInfo{
    id: number;
    name: string;
    participants: ChatParticipant[];
    lastMessage: string;
    lastMessageTimestamp: Date;
    unreadMessagesCount: number;
}