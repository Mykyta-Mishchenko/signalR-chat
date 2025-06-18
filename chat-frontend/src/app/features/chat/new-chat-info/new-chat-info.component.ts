import { Component, inject, input } from '@angular/core';
import { ChatParticipant } from '../models/chat-participant.model';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-new-chat-info',
  standalone: true,
  imports: [],
  templateUrl: './new-chat-info.component.html',
  styleUrl: './new-chat-info.component.css'
})
export class NewChatInfoComponent {
  private chatService = inject(ChatService);
  user = input.required<ChatParticipant>();
  isChatFriend = input.required<boolean>();

  onCreateChat(userId: number) {
    this.chatService.createPersonalChat(userId).subscribe();
  }
}
