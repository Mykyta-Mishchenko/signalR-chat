import { Component, inject, signal } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatFriend } from '../models/chat-friend.model';

@Component({
  selector: 'app-group-create',
  standalone: true,
  imports: [],
  templateUrl: './group-create.component.html',
  styleUrl: './group-create.component.css'
})
export class GroupCreateComponent {
  private chatService = inject(ChatService);

  friends = this.chatService.friends();

  addedFriends = signal<ChatFriend[]>([]);
}
