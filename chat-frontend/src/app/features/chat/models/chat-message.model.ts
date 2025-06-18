export interface ChatMessage{
    id: number;
    senderId: number;
    content: number;
    timeStamp: Date;
    isRead: boolean;
}