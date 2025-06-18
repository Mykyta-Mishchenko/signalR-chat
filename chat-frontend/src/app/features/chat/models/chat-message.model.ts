export interface ChatMessage{
    id: number;
    senderId: number;
    chatId: number;
    content: string;
    timeStamp: Date;
    isRead: boolean;
}