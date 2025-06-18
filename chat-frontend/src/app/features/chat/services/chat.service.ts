import { inject, Injectable, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { ChatInfo } from '../models/chat-info.model';
import { environment } from '../../../../environments/environment';
import { Observable, tap, throwError } from 'rxjs';
import { ChatParticipant } from '../models/chat-participant.model';
import { ChatMessage } from '../models/chat-message.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private authService = inject(AuthService);
  private httpClient = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private currentUser = this.authService.User;

  private _chatList = signal<ChatInfo[]>([]);
  private _chatMessages = signal<ChatMessage[]>([]);
  private _selectedChat = signal<ChatInfo | null>(null);

  chatList = this._chatList.asReadonly();
  chatMessages = this._chatMessages.asReadonly();
  selectedChat = this._selectedChat.asReadonly();


  geCurrentUserChats() : Observable<ChatInfo[]> {
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

  createPersonalChat(userId: number): Observable<ChatInfo> {
    if (this.currentUser()) {
      return this.httpClient.post<ChatInfo>(`${this.apiUrl}/chats/new/personal`,
        { senderId: Number(this.currentUser()!.userId), recipientId: userId },
        { withCredentials: true })
        .pipe(tap(chat => {
          this._chatList.update(chats => [chat, ...chats]);
        }));
    }

    return throwError(() => new Error('User not authenticated'));
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
}
