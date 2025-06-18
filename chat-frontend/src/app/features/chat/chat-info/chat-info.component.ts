import { Component, inject, input } from '@angular/core';
import { ChatInfo } from '../models/chat-info.model';
import { NgIf } from '@angular/common';
import { ChatService } from '../services/chat.service';
import { ChatType } from '../models/chat-type.enum';
import { AuthService } from '../../../core/services/auth/auth.service';

@Component({
  selector: 'app-chat-info',
  standalone: true,
  imports: [NgIf],
  templateUrl: './chat-info.component.html',
  styleUrl: './chat-info.component.css'
})
export class ChatInfoComponent {
  private authService = inject(AuthService);
  private chatService = inject(ChatService);
  private userId = this.authService.User()!.userId;
  chat = input.required<ChatInfo>();

  onGoToChat() {
    this.chatService.selectChat(this.chat().id);
  }

  get isOnline(): boolean{
    return this.chat().chatType == ChatType.Personal &&
      this.chat().participants.find(p => p.id != this.userId && p.isOnline) != undefined;
  }
}
