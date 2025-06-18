import { Component, inject, OnInit, signal } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatParticipant } from '../models/chat-participant.model';
import { ChatInfoComponent } from "../chat-info/chat-info.component";
import { NewChatInfoComponent } from "../new-chat-info/new-chat-info.component";
import { AuthService } from '../../../core/services/auth/auth.service';

@Component({
  selector: 'app-chats-list',
  standalone: true,
  imports: [ ChatInfoComponent, NewChatInfoComponent],
  templateUrl: './chats-list.component.html',
  styleUrl: './chats-list.component.css'
})
export class ChatsListComponent implements OnInit{
  private authService = inject(AuthService);
  private chatService = inject(ChatService);

  chatList = this.chatService.chatList;
  foundedUsers = signal<ChatParticipant[]>([]);

  ngOnInit(): void {
    this.chatService.geCurrentUserChats().subscribe();
  }

  onFindUser(request: string) {
    if (request == "") {
      this.foundedUsers.set([]);
      return;
    }
    
    this.chatService.getUserByNameOrEmail(request).subscribe({
      next: (users) => {
        this.foundedUsers.set(users);
        this.foundedUsers.update(users => users.filter(u => u.id != this.authService.User()?.userId));
      }
    })
  }

  isChatFriend(userId: number): boolean {
    return this.chatList().some(c =>
      c.participants.some(p => p.id == userId) &&
      c.participants.length === 2 );
  }
}
