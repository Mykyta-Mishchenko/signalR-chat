export interface ChatParticipant{
    id: number;
    name: string;
    email: string;
    isOnline: boolean;
    lastSeenAt: Date;
}