import { TextSentiment } from "./text-sentiment.enum";

export interface ChatMessage{
    id: number;
    senderId: number;
    chatId: number;
    sentiment: TextSentiment;
    content: string;
    timeStamp: Date;
    isRead: boolean;
}