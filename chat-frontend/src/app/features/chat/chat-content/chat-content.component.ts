import { Component, inject } from '@angular/core';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-chat-content',
  standalone: true,
  imports: [],
  templateUrl: './chat-content.component.html',
  styleUrl: './chat-content.component.css'
})
export class ChatContentComponent {
  private chatService = inject(ChatService);

  chat = this.chatService.selectedChat;
  messages = this.chatService.chatMessages;

  onSendMessage(message: string) {
    
  }
}
