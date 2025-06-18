import { Component, inject, input } from '@angular/core';
import { ChatInfo } from '../models/chat-info.model';
import { NgIf } from '@angular/common';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-chat-info',
  standalone: true,
  imports: [NgIf],
  templateUrl: './chat-info.component.html',
  styleUrl: './chat-info.component.css'
})
export class ChatInfoComponent {
  private chatService = inject(ChatService);
  chat = input.required<ChatInfo>();

  onGoToChat() {
    this.chatService.selectChat(this.chat().id);
  }
}
