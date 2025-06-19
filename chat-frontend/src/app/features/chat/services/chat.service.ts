import { computed, inject, Injectable, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { ChatInfo } from '../models/chat-info.model';
import { environment } from '../../../../environments/environment';
import { Observable, tap, throwError } from 'rxjs';
import { ChatParticipant } from '../models/chat-participant.model';
import { ChatMessage } from '../models/chat-message.model';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ChatType } from '../models/chat-type.enum';
import { SendMessageDto } from '../dto/send-message.dto';
import { ChatFriend } from '../models/chat-friend.model';
import { NewChatDto } from '../dto/new-chat.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private authService = inject(AuthService);
  private httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private currentUser = this.authService.User;
  private hubConnection: HubConnection | null = null;
  private chatHubUrl = environment.chatHubUrl;

  private _chatList = signal<ChatInfo[]>([]);
  private _chatMessages = signal<ChatMessage[]>([]);
  private _selectedChat = signal<ChatInfo | null>(null);

  chatList = this._chatList.asReadonly();
  chatMessages = this._chatMessages.asReadonly();
  selectedChat = this._selectedChat.asReadonly();
  friends = computed(() =>
    this._chatList()
      .filter(chat => chat.chatType === ChatType.Personal)
      .flatMap(chat =>
        chat.participants
          .filter(p => p.id !== this.currentUser()?.userId)
          .map(p => ({ id: p.id, name: p.name }))
      )
  );

  constructor() {
    this.createConnection();
    this.startConnection().then(() => {
      this.registerEventHandlers();
    });
  }

  private createConnection(): void {
    console.log(this.authService.ACCESS_TOKEN())
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.chatHubUrl}?access_token=${this.authService.ACCESS_TOKEN()}`)
      .withAutomaticReconnect()
      .build();
  }

  private async startConnection(): Promise<void> {
    if (this.hubConnection) {
      try {
        await this.hubConnection.start();
      } catch (error) {
        console.error("SignalR connection error:" + error);
        setTimeout(() => this.startConnection(), 2000);
      }
    }
  }

  getCurrentUserChats() : Observable<ChatInfo[]> {
    if (this.currentUser()) {
      return this.httpClient
        .get<ChatInfo[]>(`${this.apiUrl}/chats/user/${this.currentUser()!.userId}`, { withCredentials: true })
        .pipe(tap(chats => {
          this._chatList.set(chats);
        }));
    }

    return throwError(() => new Error('User not authenticated'));
  }

  getUserByNameOrEmail(request: string) : Observable<ChatParticipant[]> {
    return this.httpClient.get<ChatParticipant[]>(`${this.apiUrl}/chats/find/user/${request}`, { withCredentials: true });
  }

  selectChat(chatId: number) {
    this.getChatInfo(chatId).subscribe();
    this.getChatMessages(chatId).subscribe();
  }

  getChatMessages(chatId: number): Observable<ChatMessage[]> {
    return this.httpClient.get<ChatMessage[]>(`${this.apiUrl}/chats/messages/chat/${chatId}`, { withCredentials: true })
      .pipe(tap(messages => {
        this._chatMessages.set(messages);
      }));
  }

  getChatInfo(chatId: number): Observable<ChatInfo> {
    return this.httpClient.get<ChatInfo>(`${this.apiUrl}/chats/info/chat/${chatId}`, { withCredentials: true })
      .pipe(tap(chat => {
        this._selectedChat.set(chat);
      }));
  }

  private registerEventHandlers(): void {
    if (this.hubConnection) {

      this.hubConnection.on('UserBecameOnline', (userId: number) => {
        this._chatList.update(chats => chats.map(chat => this.setUserStatus(chat, userId, true)));
      });

      this.hubConnection.on('UserBecameOffline', (userId: number) => {
        this._chatList.update(chats => chats.map(chat => this.setUserStatus(chat, userId, false)));
      });

      this.hubConnection.on('GetChatMessage', (message: ChatMessage) => {
        console.log(message);
        console.log(this._selectedChat()?.id)
        console.log(message.chatId);
        if (message.chatId == this._selectedChat()?.id) {
          this._chatMessages.update(messages => [...messages, message]);
        }
        else {
          this.addNewMessageToChatList(message);
        }
      })

      this.hubConnection.on('MessageRead', (message: ChatMessage) => {
        this._chatMessages.update(messages => messages
          .map(m => m.id == message.id ? message : m));
        
        this._chatList.update(chats =>
          chats.map(chat => chat.id == message.chatId &&
            chat.unreadMessagesCount > 0 ?
            { ...chat, unreadMessagesCount: --chat.unreadMessagesCount } : chat));
      })

      this.hubConnection.on('NewChatCreated', (chat: ChatInfo) => {
        this._chatList.update(chats => [chat, ...chats]);
      })
    }
  }

  private async invokeHubMethod(methodName: string, ...args: any[]): Promise<any> {
    if (!this.isConnected()) {
      console.warn(`Cannot invoke ${methodName}: Hub not connected`);
      return;
    }

    try {
      return await this.hubConnection!.invoke(methodName, ...args);
    } catch (error) {
      console.error(`Hub method ${methodName} failed:`, error);
    }
  }

  sendChatMessage(message: SendMessageDto) {
    this.invokeHubMethod("SendMessageToChat", message);
  }

  setChatMessageAsRead(messageId: number) {
    this.invokeHubMethod("ReadChatMessage", messageId);
  }

  createNewChat(chat: NewChatDto) {
    this.invokeHubMethod("CreateNewChat", chat);
  }

  private isConnected(): boolean {
    return this.hubConnection?.state === 'Connected';
  }

  private setUserStatus(chat: ChatInfo, userId: number, status: boolean): ChatInfo {
    if (chat.chatType == ChatType.Personal) {
      return {
        ...chat,
        participants: chat.participants.map(p => p.id == userId ? { ...p, isOnline: status } : p)
      }
    }
    return chat;
  }

  private addNewMessageToChatList(message: ChatMessage) {
    const chat = this._chatList().find(c => c.id == message.chatId);
    if (chat) {
      chat.lastMessage = message.content;
      chat.lastMessageTimestamp = message.timeStamp;
      chat.unreadMessagesCount = ++chat.unreadMessagesCount;

      this._chatList.update(chats => [chat, ...chats.filter(c => c.id != message.chatId)]);
    }
  }

}
