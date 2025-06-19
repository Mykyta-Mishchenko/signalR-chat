import { Component, inject, signal } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatFriend } from '../models/chat-friend.model';
import { NewChatDto } from '../dto/new-chat.dto';
import { AuthService } from '../../../core/services/auth/auth.service';
import { ChatType } from '../models/chat-type.enum';

@Component({
  selector: 'app-group-create',
  standalone: true,
  imports: [],
  templateUrl: './group-create.component.html',
  styleUrl: './group-create.component.css'
})
export class GroupCreateComponent {
  private chatService = inject(ChatService);
  private authService = inject(AuthService);

  friends = this.chatService.friends;

  groupParticipants = signal<ChatFriend[]>(
    [{
      id: this.authService.User()!.userId,
      name: this.authService.User()!.userName
    }]);

  onAddFriend(selectedElement: HTMLSelectElement) {
    const userId = Number(selectedElement.value);
    const newParticipant = this.friends().find(f => f.id == Number(userId))
    if (newParticipant) {
      this.groupParticipants.update(participants => [...participants, newParticipant]);
    }

    selectedElement.value = "";
  }

  isAlreadyInGroup(userId: number): boolean {
    return this.groupParticipants().some(p => p.id == userId);
  }

  onDeleteFromGroup(userId: number) {
    this.groupParticipants.update(participants => participants.filter(p => p.id != userId));
  }

  onGroupCreate(groupName: string) {
    const newChat: NewChatDto = {
      name: groupName,
      creatorId: this.authService.User()!.userId,
      chatType: ChatType.Group,
      participants: this.groupParticipants()
    };

    this.chatService.createNewChat(newChat);

    this.groupParticipants.set([]);
  }

  isCreator(userId: number): boolean {
    return userId == this.authService.User()?.userId;
  }
}
