import { ChatFriend } from "../models/chat-friend.model";
import { ChatType } from "../models/chat-type.enum";

export interface NewChatDto{
    name: string;
    creatorId: number;
    chatType: ChatType;
    participants: ChatFriend[];
}